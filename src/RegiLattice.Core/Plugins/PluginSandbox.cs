// RegiLattice.Core — Plugins/PluginSandbox.cs
// T7.4 — Plugin sandboxing: executes RegOp lists from third-party packs in an
// isolated child process via a named pipe, with a 30-second crash-containing timeout.

#nullable enable

using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Plugins;

// ── IPC protocol DTOs (sealed, internal to this assembly) ────────────────────

/// <summary>Serialised RegOp DTO used in the sandbox named-pipe protocol.</summary>
internal sealed class SandboxOpDto
{
    [JsonPropertyName("kind")]
    public string Kind { get; init; } = "";

    [JsonPropertyName("path")]
    public string Path { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("dwordValue")]
    public int? DwordValue { get; init; }

    [JsonPropertyName("stringValue")]
    public string? StringValue { get; init; }

    [JsonPropertyName("qwordValue")]
    public long? QwordValue { get; init; }

    /// <summary>Base-64 encoded bytes for Binary ops.</summary>
    [JsonPropertyName("binaryValue")]
    public string? BinaryValue { get; init; }

    [JsonPropertyName("multiSzValue")]
    public IReadOnlyList<string>? MultiSzValue { get; init; }
}

/// <summary>Request sent from parent → child process over the named pipe.</summary>
internal sealed class PluginSandboxRequest
{
    [JsonPropertyName("dryRun")]
    public bool DryRun { get; init; }

    [JsonPropertyName("ops")]
    public IReadOnlyList<SandboxOpDto> Ops { get; init; } = [];
}

/// <summary>Response returned child → parent over the named pipe.</summary>
internal sealed class PluginSandboxResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; } = "";
}

// ── Public result returned to callers ────────────────────────────────────────

/// <summary>Result of a sandboxed plugin execution.</summary>
public sealed class PluginSandboxResult
{
    public bool Success { get; init; }
    public string ErrorMessage { get; init; } = "";

    /// <summary>True when execution was aborted due to the sandbox timeout.</summary>
    public bool TimedOut { get; init; }
}

// ── Sandbox orchestrator ──────────────────────────────────────────────────────

/// <summary>
/// Executes a <see cref="RegOp"/> list from a third-party Tweak Pack in an isolated
/// child process (via a named pipe), enforcing a hard crash-containing timeout.
/// </summary>
/// <remarks>
/// The child process is expected to be <c>RegiLattice.CLI.exe --plugin-host &lt;pipeName&gt;</c>.
/// If the child crashes or does not respond within <see cref="DefaultTimeoutSeconds"/>,
/// the process is killed and a <see cref="PluginSandboxResult"/> with
/// <see cref="PluginSandboxResult.TimedOut"/> is returned.
/// </remarks>
public sealed class PluginSandbox
{
    /// <summary>Default timeout in seconds before the child process is killed.</summary>
    public const int DefaultTimeoutSeconds = 30;

    private static readonly JsonSerializerOptions s_json = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = false };

    // ── DTO conversions ───────────────────────────────────────────────────────

    /// <summary>Convert a list of <see cref="RegOp"/> instances to serialisable DTOs.</summary>
    internal static IReadOnlyList<SandboxOpDto> ToDto(IReadOnlyList<RegOp> ops)
    {
        var dtos = new List<SandboxOpDto>(ops.Count);
        foreach (var op in ops)
        {
            dtos.Add(
                op.Kind switch
                {
                    RegOpKind.SetValue => op.ValueKind switch
                    {
                        Microsoft.Win32.RegistryValueKind.DWord => new SandboxOpDto
                        {
                            Kind = "setdword",
                            Path = op.Path,
                            Name = op.Name,
                            DwordValue = (int?)op.Value,
                        },
                        Microsoft.Win32.RegistryValueKind.QWord => new SandboxOpDto
                        {
                            Kind = "setqword",
                            Path = op.Path,
                            Name = op.Name,
                            QwordValue = op.Value is long l ? l : (long?)Convert.ToInt64(op.Value),
                        },
                        Microsoft.Win32.RegistryValueKind.Binary => new SandboxOpDto
                        {
                            Kind = "setbinary",
                            Path = op.Path,
                            Name = op.Name,
                            BinaryValue = op.Value is byte[] b ? Convert.ToBase64String(b) : null,
                        },
                        Microsoft.Win32.RegistryValueKind.MultiString => new SandboxOpDto
                        {
                            Kind = "setmultisz",
                            Path = op.Path,
                            Name = op.Name,
                            MultiSzValue = op.Value is string[] sa ? sa : null,
                        },
                        Microsoft.Win32.RegistryValueKind.ExpandString => new SandboxOpDto
                        {
                            Kind = "setexpandstring",
                            Path = op.Path,
                            Name = op.Name,
                            StringValue = op.Value as string,
                        },
                        _ => new SandboxOpDto
                        {
                            Kind = "setstring",
                            Path = op.Path,
                            Name = op.Name,
                            StringValue = op.Value as string,
                        },
                    },
                    RegOpKind.DeleteValue => new SandboxOpDto
                    {
                        Kind = "deletevalue",
                        Path = op.Path,
                        Name = op.Name,
                    },
                    RegOpKind.DeleteTree => new SandboxOpDto { Kind = "deletetree", Path = op.Path },
                    RegOpKind.CheckValue => op.ValueKind switch
                    {
                        Microsoft.Win32.RegistryValueKind.DWord => new SandboxOpDto
                        {
                            Kind = "checkdword",
                            Path = op.Path,
                            Name = op.Name,
                            DwordValue = (int?)op.Value,
                        },
                        _ => new SandboxOpDto
                        {
                            Kind = "checkstring",
                            Path = op.Path,
                            Name = op.Name,
                            StringValue = op.Value as string,
                        },
                    },
                    RegOpKind.CheckMissing => new SandboxOpDto
                    {
                        Kind = "checkmissing",
                        Path = op.Path,
                        Name = op.Name,
                    },
                    _ => new SandboxOpDto { Kind = "checkkeymissing", Path = op.Path },
                }
            );
        }
        return dtos;
    }

    /// <summary>Reconstruct <see cref="RegOp"/> instances from serialised DTOs.</summary>
    internal static IReadOnlyList<RegOp> FromDto(IReadOnlyList<SandboxOpDto> dtos)
    {
        var ops = new List<RegOp>(dtos.Count);
        foreach (var dto in dtos)
        {
            var op = (dto.Kind?.ToLowerInvariant()) switch
            {
                "setdword" => RegOp.SetDword(dto.Path, dto.Name, dto.DwordValue ?? 0),
                "setstring" => RegOp.SetString(dto.Path, dto.Name, dto.StringValue ?? ""),
                "setexpandstring" => RegOp.SetExpandString(dto.Path, dto.Name, dto.StringValue ?? ""),
                "setqword" => RegOp.SetQword(dto.Path, dto.Name, dto.QwordValue ?? 0),
                "setbinary" => RegOp.SetBinary(dto.Path, dto.Name, dto.BinaryValue is not null ? Convert.FromBase64String(dto.BinaryValue) : []),
                "setmultisz" => RegOp.SetMultiSz(dto.Path, dto.Name, dto.MultiSzValue is not null ? [.. dto.MultiSzValue] : []),
                "deletevalue" => RegOp.DeleteValue(dto.Path, dto.Name),
                "deletetree" => RegOp.DeleteTree(dto.Path),
                "checkdword" => RegOp.CheckDword(dto.Path, dto.Name, dto.DwordValue ?? 0),
                "checkstring" => RegOp.CheckString(dto.Path, dto.Name, dto.StringValue ?? ""),
                "checkmissing" => RegOp.CheckMissing(dto.Path, dto.Name),
                _ => RegOp.CheckKeyMissing(dto.Path),
            };
            ops.Add(op);
        }
        return ops;
    }

    // ── Parent-side: spawn child and communicate ──────────────────────────────

    /// <summary>
    /// Execute <paramref name="ops"/> in a sandboxed child process.
    /// </summary>
    /// <param name="ops">Registry operations to run.</param>
    /// <param name="dryRun">When <c>true</c>, no registry writes occur.</param>
    /// <param name="executablePath">
    /// Full path to the CLI executable that supports <c>--plugin-host &lt;pipeName&gt;</c>.
    /// </param>
    /// <param name="timeoutSeconds">
    /// Seconds before the child process is killed. Default: <see cref="DefaultTimeoutSeconds"/>.
    /// </param>
    /// <param name="ct">Caller cancellation token (stacked with the timeout).</param>
    public static async Task<PluginSandboxResult> ExecuteAsync(
        IReadOnlyList<RegOp> ops,
        bool dryRun,
        string executablePath,
        int timeoutSeconds = DefaultTimeoutSeconds,
        CancellationToken ct = default
    )
    {
        var pipeName = $"rl-plugin-{Guid.NewGuid():N}";

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

        using var pipe = new NamedPipeServerStream(
            pipeName,
            PipeDirection.InOut,
            maxNumberOfServerInstances: 1,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous
        );

        using var proc = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo(executablePath, $"--plugin-host {pipeName}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true,
            },
        };

        try
        {
            proc.Start();

            await pipe.WaitForConnectionAsync(cts.Token);
            using var writer = new StreamWriter(pipe, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };
            using var reader = new StreamReader(pipe, Encoding.UTF8, leaveOpen: true);

            var request = new PluginSandboxRequest { DryRun = dryRun, Ops = ToDto(ops) };
            var requestJson = JsonSerializer.Serialize(request, s_json);
            await writer.WriteLineAsync(requestJson.AsMemory(), cts.Token);

            var responseJson = await reader.ReadLineAsync(cts.Token);
            if (responseJson is null)
            {
                return new PluginSandboxResult { Success = false, ErrorMessage = "Plugin host disconnected without sending a response." };
            }

            var response = JsonSerializer.Deserialize<PluginSandboxResponse>(responseJson, s_json);
            if (response is null)
            {
                return new PluginSandboxResult { Success = false, ErrorMessage = "Plugin host returned an invalid (null) response." };
            }

            return new PluginSandboxResult { Success = response.Success, ErrorMessage = response.ErrorMessage };
        }
        catch (OperationCanceledException)
        {
            return new PluginSandboxResult
            {
                Success = false,
                TimedOut = true,
                ErrorMessage = $"Plugin host did not respond within {timeoutSeconds} second(s) and was killed.",
            };
        }
        catch (Exception ex)
        {
            // Catch process-spawn failures (file not found, access denied, etc.)
            return new PluginSandboxResult { Success = false, ErrorMessage = $"Failed to start plugin host process: {ex.Message}" };
        }
        finally
        {
            // Always attempt to kill the child process — crash containment guarantee.
            try
            {
                if (!proc.HasExited)
                    proc.Kill(entireProcessTree: true);
            }
            catch (Exception)
            {
                // Best-effort kill; ignore if process never started or already exited.
            }
        }
    }

    // ── Child-side: plugin host entry point ───────────────────────────────────

    /// <summary>
    /// Run as the sandboxed plugin host child process.
    /// Connects to the named pipe <paramref name="pipeName"/>, reads a
    /// <see cref="PluginSandboxRequest"/>, executes the ops, and writes back a
    /// <see cref="PluginSandboxResponse"/>.
    /// </summary>
    /// <returns>Exit code: 0 = success, 1 = execution error.</returns>
    public static async Task<int> RunHostAsync(string pipeName)
    {
        try
        {
            using var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            // Allow up to 5 seconds to connect to the server pipe.
            await pipe.ConnectAsync(5_000);

            using var reader = new StreamReader(pipe, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(pipe, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

            var requestJson = await reader.ReadLineAsync();
            if (requestJson is null)
                return 1;

            PluginSandboxRequest? request;
            try
            {
                request = JsonSerializer.Deserialize<PluginSandboxRequest>(requestJson, s_json);
            }
            catch (JsonException ex)
            {
                var errResp = new PluginSandboxResponse { Success = false, ErrorMessage = $"Failed to parse sandbox request: {ex.Message}" };
                await writer.WriteLineAsync(JsonSerializer.Serialize(errResp, s_json));
                return 1;
            }

            if (request is null)
            {
                var errResp = new PluginSandboxResponse { Success = false, ErrorMessage = "Received null sandbox request." };
                await writer.WriteLineAsync(JsonSerializer.Serialize(errResp, s_json));
                return 1;
            }

            try
            {
                var ops = FromDto(request.Ops);
                var session = new RegistrySession(dryRun: request.DryRun);
                session.Execute(ops);

                var response = new PluginSandboxResponse { Success = true };
                await writer.WriteLineAsync(JsonSerializer.Serialize(response, s_json));
                return 0;
            }
            catch (Exception ex)
            {
                var response = new PluginSandboxResponse { Success = false, ErrorMessage = ex.Message };
                await writer.WriteLineAsync(JsonSerializer.Serialize(response, s_json));
                return 1;
            }
        }
        catch (Exception ex)
        {
            // Unhandled pipe / connection error — write to stderr as last resort.
            await Console.Error.WriteLineAsync($"[PluginSandbox host error] {ex.Message}");
            return 1;
        }
    }
}
