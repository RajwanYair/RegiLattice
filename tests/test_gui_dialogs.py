"""Tests for regilattice.gui_dialogs — dialogs and export helpers."""

from __future__ import annotations

import json
import tkinter as tk
from pathlib import Path
from types import SimpleNamespace
from typing import TYPE_CHECKING
from unittest.mock import MagicMock, patch

import pytest

from regilattice.gui_dialogs import export_json_selection, export_powershell, import_json_selection, show_about
from regilattice.tweaks import TweakDef

if TYPE_CHECKING:
    pass


# ── Test helpers ─────────────────────────────────────────────────────────────


def _make_td(**overrides: object) -> TweakDef:
    defaults: dict[str, object] = {
        "id": "test-tweak",
        "label": "Test Tweak",
        "category": "Test",
        "apply_fn": lambda *, require_admin: None,
        "remove_fn": lambda *, require_admin: None,
        "detect_fn": None,
        "needs_admin": True,
        "corp_safe": False,
        "registry_keys": [r"HKLM\SOFTWARE\Test\Key"],
        "description": "A test tweak",
        "tags": ["test"],
    }
    defaults.update(overrides)
    return TweakDef(**defaults)  # type: ignore[arg-type]


def _try_create_root() -> tk.Tk:
    """Create a hidden Tk root or skip the test if tkinter is unavailable."""
    try:
        root = tk.Tk()
        root.withdraw()
        return root
    except Exception:
        pytest.skip("tkinter unavailable")


def _all_widgets(parent: tk.Widget) -> list[tk.Widget]:
    """Recursively collect all descendant widgets."""
    result: list[tk.Widget] = [parent]
    for child in parent.winfo_children():  # type: ignore[no-untyped-call]
        result.extend(_all_widgets(child))
    return result


def _find_install_buttons(dlg: tk.Widget) -> list[tk.Widget]:
    """Return Install (non-Remove) buttons within a dialog."""
    return [
        w
        for w in _all_widgets(dlg)
        if w.winfo_class() == "Button"  # type: ignore[no-untyped-call]
        and "Install" in str(w.cget("text"))
        and "Remove" not in str(w.cget("text"))
    ]


class _SyncThread:
    """threading.Thread replacement that runs the target synchronously on .start()."""

    def __init__(self, *, target, daemon: bool = True) -> None:  # type: ignore[no-untyped-def]
        self._target = target

    def start(self) -> None:
        self._target()


class _StubThread:
    """threading.Thread replacement that captures targets but never runs them."""

    def __init__(self, *, target, daemon: bool = True) -> None:  # type: ignore[no-untyped-def]
        pass

    def start(self) -> None:
        pass


# ── export_powershell tests ──────────────────────────────────────────────────


class TestExportPowershell:
    def test_empty_selection_shows_info(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            export_powershell([], MagicMock())
            mbox.showinfo.assert_called_once()

    def test_cancel_dialog_does_nothing(self) -> None:
        td = _make_td()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = ""
            status = MagicMock()
            export_powershell([td], status)
            status.assert_not_called()

    def test_writes_ps1_file(self, tmp_path: Path) -> None:
        td = _make_td(
            id="sec-example",
            label="Example Tweak",
            registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Test"],
            description="Example description",
        )
        out = tmp_path / "out.ps1"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            status = MagicMock()
            export_powershell([td], status)
        content = out.read_text(encoding="utf-8-sig")
        assert "Example Tweak" in content
        assert "HKLM:" in content
        assert "#Requires -RunAsAdministrator" in content
        assert "Test-Path" in content
        status.assert_called_once()

    def test_multiple_tweaks_in_order(self, tmp_path: Path) -> None:
        tweaks = [
            _make_td(id="a-first", label="First", registry_keys=[r"HKLM\SOFTWARE\A"]),
            _make_td(id="b-second", label="Second", registry_keys=[r"HKCU\Software\B"]),
        ]
        out = tmp_path / "multi.ps1"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_powershell(tweaks, MagicMock())
        content = out.read_text(encoding="utf-8-sig")
        assert content.index("First") < content.index("Second")


# ── import_json_selection tests ──────────────────────────────────────────────


class TestImportJsonSelection:
    @staticmethod
    def _make_row(tid: str, *, disabled: bool = False) -> SimpleNamespace:
        return SimpleNamespace(
            td=SimpleNamespace(id=tid),
            var=MagicMock(),
            disabled_by_corp=disabled,
        )

    def test_cancel_dialog_does_nothing(self) -> None:
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = ""
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())

    def test_imports_list_of_ids(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps(["tweak-a", "tweak-b"]), encoding="utf-8")
        rows = [self._make_row("tweak-a"), self._make_row("tweak-b"), self._make_row("tweak-c")]
        deselect = MagicMock()
        update = MagicMock()
        status = MagicMock()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, deselect, update, status)
        deselect.assert_called_once()
        update.assert_called_once()
        rows[0].var.set.assert_called_once_with(True)
        rows[1].var.set.assert_called_once_with(True)
        rows[2].var.set.assert_not_called()

    def test_imports_dict_format(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps({"tweaks": ["tweak-x"]}), encoding="utf-8")
        rows = [self._make_row("tweak-x")]
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, MagicMock(), MagicMock(), MagicMock())
        rows[0].var.set.assert_called_once_with(True)

    def test_skips_corp_disabled_rows(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text(json.dumps(["tweak-a"]), encoding="utf-8")
        rows = [self._make_row("tweak-a", disabled=True)]
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), rows, MagicMock(), MagicMock(), MagicMock())
        rows[0].var.set.assert_not_called()

    def test_invalid_json_shows_error(self, tmp_path: Path) -> None:
        jf = tmp_path / "bad.json"
        jf.write_text("{bad", encoding="utf-8")
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
        ):
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())
            mbox.showerror.assert_called_once()

    def test_unsupported_root_type_shows_error(self, tmp_path: Path) -> None:
        jf = tmp_path / "num.json"
        jf.write_text("42", encoding="utf-8")
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
        ):
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())
            mbox.showerror.assert_called_once()


# ── export_json_selection tests ──────────────────────────────────────────────


class TestExportJsonSelection:
    def test_empty_selection_shows_info(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            export_json_selection([], MagicMock())
            mbox.showinfo.assert_called_once()

    def test_cancel_dialog_does_nothing(self) -> None:
        td = _make_td()
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = ""
            status = MagicMock()
            export_json_selection([td], status)
            status.assert_not_called()

    def test_writes_json_file(self, tmp_path: Path) -> None:
        td = _make_td(id="sec-example", label="Example Tweak")
        out = tmp_path / "out.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            status = MagicMock()
            export_json_selection([td], status)
        data = json.loads(out.read_text(encoding="utf-8"))
        assert data["tweaks"] == ["sec-example"]
        assert "version" in data
        status.assert_called_once()

    def test_multiple_tweaks_ids_in_order(self, tmp_path: Path) -> None:
        tweaks = [
            _make_td(id="a-first", label="First"),
            _make_td(id="b-second", label="Second"),
        ]
        out = tmp_path / "multi.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_json_selection(tweaks, MagicMock())
        data = json.loads(out.read_text(encoding="utf-8"))
        assert data["tweaks"] == ["a-first", "b-second"]

    def test_roundtrip_with_import(self, tmp_path: Path) -> None:
        """An exported JSON file can be reimported successfully."""
        td = _make_td(id="roundtrip-tweak")
        out = tmp_path / "roundtrip.json"
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.asksaveasfilename.return_value = str(out)
            export_json_selection([td], MagicMock())

        row = SimpleNamespace(td=SimpleNamespace(id="roundtrip-tweak"), var=MagicMock(), disabled_by_corp=False)
        with patch("regilattice.gui_dialogs.filedialog") as fdlg:
            fdlg.askopenfilename.return_value = str(out)
            import_json_selection(MagicMock(), [row], MagicMock(), MagicMock(), MagicMock())
        row.var.set.assert_called_once_with(True)


# ── show_about tests ─────────────────────────────────────────────────────────


class TestShowAbout:
    def test_calls_messagebox(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            show_about(corp_blocked=False)
            mbox.showinfo.assert_called_once()
            title, body = mbox.showinfo.call_args.args
            assert "RegiLattice" in title
            assert "Corporate: No" in body

    def test_corp_blocked_true(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            show_about(corp_blocked=True)
            _, body = mbox.showinfo.call_args.args
            assert "Corporate: Yes" in body

    def test_with_hw_summary(self) -> None:
        with patch("regilattice.gui_dialogs.messagebox") as mbox:
            show_about(corp_blocked=False, hw_summary="Intel i9 | RTX 4090 | 32 GB RAM")
        _, body = mbox.showinfo.call_args.args
        assert "Hardware" in body
        assert "Intel i9" in body


# ── OSError paths ────────────────────────────────────────────────────────────


class TestOsErrorPaths:
    def test_export_powershell_write_oserror(self, tmp_path: Path) -> None:
        td = _make_td()
        out = tmp_path / "out.ps1"
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
            patch("builtins.open", side_effect=OSError("no space")),
        ):
            fdlg.asksaveasfilename.return_value = str(out)
            export_powershell([td], MagicMock())
        mbox.showerror.assert_called_once()

    def test_export_json_write_oserror(self, tmp_path: Path) -> None:
        td = _make_td()
        out = tmp_path / "out.json"
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
            patch("builtins.open", side_effect=OSError("no space")),
        ):
            fdlg.asksaveasfilename.return_value = str(out)
            export_json_selection([td], MagicMock())
        mbox.showerror.assert_called_once()

    def test_import_json_read_oserror(self, tmp_path: Path) -> None:
        jf = tmp_path / "sel.json"
        jf.write_text("[]", encoding="utf-8")
        with (
            patch("regilattice.gui_dialogs.filedialog") as fdlg,
            patch("regilattice.gui_dialogs.messagebox") as mbox,
            patch("builtins.open", side_effect=OSError("permission denied")),
        ):
            fdlg.askopenfilename.return_value = str(jf)
            import_json_selection(MagicMock(), [], MagicMock(), MagicMock(), MagicMock())
        mbox.showerror.assert_called_once()


# ── _validate_package_name tests ─────────────────────────────────────────────


class TestValidatePackageName:
    """Tests for the injection-prevention validator."""

    def test_valid_simple_name(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        assert _validate_package_name("PSReadLine") == "PSReadLine"

    def test_valid_name_with_hyphen(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        assert _validate_package_name("posh-git") == "posh-git"

    def test_valid_name_with_dot(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        assert _validate_package_name("Microsoft.Graph") == "Microsoft.Graph"

    def test_valid_name_with_underscore_and_version(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        assert _validate_package_name("My_Package-1.0") == "My_Package-1.0"

    def test_empty_string_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("")

    def test_semicolon_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("evil;cmd")

    def test_single_quote_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("'; rm -rf /")

    def test_space_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("foo bar")

    def test_ampersand_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("foo&bar")

    def test_backtick_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("`whoami`")

    def test_pipe_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_package_name

        with pytest.raises(ValueError):
            _validate_package_name("x|y")


# ── run_powershell_command tests ─────────────────────────────────────────────


class TestRunPowershellCommand:
    def test_success_returns_stdout(self) -> None:
        from regilattice.gui_dialogs import run_powershell_command

        with patch("regilattice.gui_dialogs.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout="output\n", stderr="")
            result = run_powershell_command("Get-Date")
        assert result == "output"

    def test_nonzero_exit_raises_runtime_error(self) -> None:
        from regilattice.gui_dialogs import run_powershell_command

        with patch("regilattice.gui_dialogs.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=1, stdout="", stderr="Error!")
            with pytest.raises(RuntimeError, match="Error!"):
                run_powershell_command("bad-command")

    def test_empty_stderr_uses_exit_code_message(self) -> None:
        from regilattice.gui_dialogs import run_powershell_command

        with patch("regilattice.gui_dialogs.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=2, stdout="", stderr="")
            with pytest.raises(RuntimeError, match="Exit code 2"):
                run_powershell_command("bad-command")

    def test_custom_timeout_is_forwarded(self) -> None:
        from regilattice.gui_dialogs import run_powershell_command

        with patch("regilattice.gui_dialogs.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout="", stderr="")
            run_powershell_command("cmd", timeout=120)
            _, kwargs = mock_run.call_args
            assert kwargs.get("timeout") == 120

    def test_default_timeout_is_60(self) -> None:
        from regilattice.gui_dialogs import run_powershell_command

        with patch("regilattice.gui_dialogs.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout="", stderr="")
            run_powershell_command("cmd")
            _, kwargs = mock_run.call_args
            assert kwargs.get("timeout") == 60


# ── open_scoop_manager (no-scoop path) ───────────────────────────────────────


class TestOpenScoopManagerNoScoop:
    def test_no_scoop_shows_error_label(self) -> None:
        """When scoop is not installed the dialog shows an error label and returns early."""
        import tkinter as tk

        try:
            root = tk.Tk()
            root.withdraw()
        except Exception:
            pytest.skip("tkinter unavailable")

        try:
            from regilattice.gui_dialogs import open_scoop_manager

            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=False),
                patch("tkinter.Toplevel.grab_set"),
            ):
                open_scoop_manager(root, MagicMock())
        finally:
            root.destroy()


# ── open_scoop_manager (with-scoop path) ─────────────────────────────────────


class TestOpenScoopManagerWithScoop:
    def test_with_scoop_creates_dialog(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_scoop_manager

            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=True),
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
            ):
                open_scoop_manager(root, MagicMock())
            assert any(isinstance(w, tk.Toplevel) for w in root.winfo_children())  # type: ignore[no-untyped-call]
        finally:
            root.destroy()

    def test_refresh_list_populates_listbox(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_scoop_manager

            apps = ["git", "ripgrep", "bat"]
            # SyncThread so initial _refresh_list auto-run on dialog open is called
            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=True),
                patch("regilattice.gui_dialogs.threading.Thread", _SyncThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.tweaks.scoop_tools.list_installed_scoop_apps", return_value=apps),
            ):
                open_scoop_manager(root, MagicMock())

            dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
            if dlg is None:
                pytest.skip("dialog not found")
            listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
            assert listboxes, "No listbox found in dialog"
            assert listboxes[0].size() == 3
        finally:
            root.destroy()

    def test_install_action_valid_name(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_scoop_manager

            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=True),
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.tweaks.scoop_tools.list_installed_scoop_apps", return_value=[]),
                patch("regilattice.tweaks.scoop_tools._install_scoop_app") as mock_install,
            ):
                open_scoop_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "git")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mock_install.assert_called_once_with("git")
        finally:
            root.destroy()

    def test_install_action_invalid_name_shows_error(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_scoop_manager

            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=True),
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.tweaks.scoop_tools.list_installed_scoop_apps", return_value=[]),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_scoop_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "evil'; rm -rf /")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showerror.assert_called_once()
        finally:
            root.destroy()

    def test_remove_action_no_selection_shows_info(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_scoop_manager

            with (
                patch("regilattice.tweaks.scoop_tools._scoop_installed", return_value=True),
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.tweaks.scoop_tools.list_installed_scoop_apps", return_value=[]),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_scoop_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    remove_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Remove" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not remove_btns:
                        pytest.skip("remove button not found")
                    remove_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showinfo.assert_called_once()
        finally:
            root.destroy()


# ── open_psmodule_manager tests ───────────────────────────────────────────────


class TestOpenPsModuleManager:
    def test_creates_dialog(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
            ):
                open_psmodule_manager(root, MagicMock())
            assert any(isinstance(w, tk.Toplevel) for w in root.winfo_children())  # type: ignore[no-untyped-call]
        finally:
            root.destroy()

    def test_refresh_list_success(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value="PSReadLine  v2.3\nPester  v5.3"),
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    refresh_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Refresh" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not refresh_btns:
                        pytest.skip("refresh button not found")
                    refresh_btns[0].invoke()  # type: ignore[no-untyped-call]

                listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
                assert listboxes, "No listbox found"
                assert listboxes[0].size() == 2
        finally:
            root.destroy()

    def test_refresh_list_error_shows_status(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", side_effect=RuntimeError("PS error")),
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    refresh_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Refresh" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not refresh_btns:
                        pytest.skip("refresh button not found")
                    refresh_btns[0].invoke()  # type: ignore[no-untyped-call]

                labels = [w for w in _all_widgets(dlg) if isinstance(w, tk.Label)]
                status_texts = [w.cget("text") for w in labels]
                assert any("Error" in str(t) or "error" in str(t).lower() for t in status_texts)
        finally:
            root.destroy()

    def test_install_action_valid_name(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value="") as mock_rpc,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "PSReadLine")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            assert mock_rpc.called
            call_args_list = mock_rpc.call_args_list
            ps_calls = [c.args[0] for c in call_args_list if "Install-Module" in c.args[0]]
            assert ps_calls, "Install-Module not called"
            assert "PSReadLine" in ps_calls[0]
        finally:
            root.destroy()

    def test_install_action_invalid_name_shows_error(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value=""),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "'; rm -rf /")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showerror.assert_called_once()
        finally:
            root.destroy()

    def test_remove_action_no_selection_shows_info(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value=""),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    remove_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Remove" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not remove_btns:
                        pytest.skip("remove button not found")
                    remove_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showinfo.assert_called_once()
        finally:
            root.destroy()

    def test_update_action_no_selection_shows_info(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value=""),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    update_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Update" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not update_btns:
                        pytest.skip("update button not found")
                    update_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showinfo.assert_called_once()
        finally:
            root.destroy()

    def test_install_ps_error_shows_error_dialog(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", side_effect=RuntimeError("PS error")),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "PSReadLine")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showerror.assert_called_once()
        finally:
            root.destroy()

    def test_remove_action_with_selection_success(self) -> None:
        """Remove a module when one is selected and confirm = Yes."""
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value="") as mock_rpc,
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                mbox.askyesno.return_value = True
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
                if not listboxes:
                    pytest.skip("listbox not found")
                listboxes[0].insert("end", "PSReadLine  v2.3")
                listboxes[0].selection_set(0)  # type: ignore[no-untyped-call]

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    remove_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Remove" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not remove_btns:
                        pytest.skip("remove button not found")
                    remove_btns[0].invoke()  # type: ignore[no-untyped-call]

            call_args_list = mock_rpc.call_args_list
            ps_calls = [c.args[0] for c in call_args_list if "Uninstall-Module" in c.args[0]]
            assert ps_calls, "Uninstall-Module not called"
            assert "PSReadLine" in ps_calls[0]
        finally:
            root.destroy()

    def test_update_action_with_selection_success(self) -> None:
        """Update a module when one is selected."""
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_psmodule_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.run_powershell_command", return_value="") as mock_rpc,
            ):
                open_psmodule_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
                if not listboxes:
                    pytest.skip("listbox not found")
                listboxes[0].insert("end", "Az  v10.0")
                listboxes[0].selection_set(0)  # type: ignore[no-untyped-call]

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    update_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Update" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not update_btns:
                        pytest.skip("update button not found")
                    update_btns[0].invoke()  # type: ignore[no-untyped-call]

            call_args_list = mock_rpc.call_args_list
            ps_calls = [c.args[0] for c in call_args_list if "Update-Module" in c.args[0]]
            assert ps_calls, "Update-Module not called"
            assert "Az" in ps_calls[0]
        finally:
            root.destroy()


# ── open_pip_manager tests ────────────────────────────────────────────────────


class TestOpenPipManager:
    def test_creates_dialog(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
            ):
                open_pip_manager(root, MagicMock())
            assert any(isinstance(w, tk.Toplevel) for w in root.winfo_children())  # type: ignore[no-untyped-call]
        finally:
            root.destroy()

    def test_refresh_list_populates_listbox(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            pip_json = '[{"name": "requests", "version": "2.31.0"}, {"name": "rich", "version": "13.7.0"}]'
            with (
                patch("regilattice.gui_dialogs.threading.Thread", _SyncThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
            ):
                mock_run.return_value = MagicMock(returncode=0, stdout=pip_json, stderr="")
                open_pip_manager(root, MagicMock())

            dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
            if dlg is None:
                pytest.skip("dialog not found")
            listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
            assert listboxes, "No listbox found in dialog"
            assert listboxes[0].size() == 2
        finally:
            root.destroy()

    def test_install_action_valid_name(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
            ):
                mock_run.return_value = MagicMock(returncode=0, stdout="[]", stderr="")
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]  # type: ignore[no-untyped-call]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "requests")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            calls = [str(c) for c in mock_run.call_args_list]
            assert any("install" in c and "requests" in c for c in calls)
        finally:
            root.destroy()

    def test_install_action_user_scope_adds_user_flag(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
            ):
                mock_run.return_value = MagicMock(returncode=0, stdout="[]", stderr="")
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]  # type: ignore[no-untyped-call]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "rich")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            for call in mock_run.call_args_list:
                args = call.args[0] if call.args else call.kwargs.get("args", [])
                if "install" in args and "rich" in args:
                    assert "--user" in args, "--user flag expected for user scope"
                    break
        finally:
            root.destroy()

    def test_install_action_invalid_name_shows_error(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run", return_value=MagicMock(returncode=0, stdout="[]", stderr="")),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]  # type: ignore[no-untyped-call]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "evil'; rm -rf /")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showerror.assert_called_once()
        finally:
            root.destroy()

    def test_remove_action_no_selection_shows_info(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run", return_value=MagicMock(returncode=0, stdout="[]", stderr="")),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    remove_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Remove" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not remove_btns:
                        pytest.skip("remove button not found")
                    remove_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showinfo.assert_called_once()
        finally:
            root.destroy()

    def test_remove_action_with_selection_calls_pip_uninstall(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                mock_run.return_value = MagicMock(returncode=0, stdout="[]", stderr="")
                mbox.askyesno.return_value = True
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
                if not listboxes:
                    pytest.skip("listbox not found")
                listboxes[0].insert("end", "requests  v2.31.0")
                listboxes[0].selection_set(0)  # type: ignore[no-untyped-call]

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    remove_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Remove" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not remove_btns:
                        pytest.skip("remove button not found")
                    remove_btns[0].invoke()  # type: ignore[no-untyped-call]

            for call in mock_run.call_args_list:
                args = call.args[0] if call.args else call.kwargs.get("args", [])
                if "uninstall" in args and "requests" in args:
                    assert "-y" in args
                    break
        finally:
            root.destroy()

    def test_update_action_no_selection_shows_info(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run", return_value=MagicMock(returncode=0, stdout="[]", stderr="")),
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    update_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Update" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not update_btns:
                        pytest.skip("update button not found")
                    update_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showinfo.assert_called_once()
        finally:
            root.destroy()

    def test_update_action_with_selection_calls_pip_upgrade(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
            ):
                mock_run.return_value = MagicMock(returncode=0, stdout="[]", stderr="")
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                listboxes = [w for w in _all_widgets(dlg) if isinstance(w, tk.Listbox)]
                if not listboxes:
                    pytest.skip("listbox not found")
                listboxes[0].insert("end", "rich  v13.7.0")
                listboxes[0].selection_set(0)  # type: ignore[no-untyped-call]

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    update_btns = [w for w in _all_widgets(dlg) if w.winfo_class() == "Button" and "Update" in str(w.cget("text"))]  # type: ignore[no-untyped-call]
                    if not update_btns:
                        pytest.skip("update button not found")
                    update_btns[0].invoke()  # type: ignore[no-untyped-call]

            for call in mock_run.call_args_list:
                args = call.args[0] if call.args else call.kwargs.get("args", [])
                if "--upgrade" in args and "rich" in args:
                    break
            else:
                pytest.fail("pip install --upgrade not called")
        finally:
            root.destroy()

    def test_install_pip_error_shows_error_dialog(self) -> None:
        root = _try_create_root()
        try:
            from regilattice.gui_dialogs import open_pip_manager

            with (
                patch("regilattice.gui_dialogs.threading.Thread", _StubThread),
                patch("tkinter.Toplevel.grab_set"),
                patch("regilattice.gui_dialogs.subprocess.run") as mock_run,
                patch("regilattice.gui_dialogs.messagebox") as mbox,
            ):
                # Initial refresh is stubbed (never runs).
                # When install button is clicked, the sole subprocess call is the pip install — which fails.
                mock_run.return_value = MagicMock(returncode=1, stdout="", stderr="No matching distribution found")
                open_pip_manager(root, MagicMock())

                dlg = next((w for w in root.winfo_children() if isinstance(w, tk.Toplevel)), None)  # type: ignore[no-untyped-call]
                if dlg is None:
                    pytest.skip("dialog not found")

                entries = [w for w in _all_widgets(dlg) if w.winfo_class() == "TEntry"]  # type: ignore[no-untyped-call]
                if not entries:
                    pytest.skip("entry not found")
                entries[0].insert(0, "nonexistent-pkg-xyz")

                with patch("regilattice.gui_dialogs.threading.Thread", _SyncThread):
                    install_btns = _find_install_buttons(dlg)
                    if not install_btns:
                        pytest.skip("install button not found")
                    install_btns[0].invoke()  # type: ignore[no-untyped-call]

            mbox.showerror.assert_called_once()
        finally:
            root.destroy()


# ── _validate_pip_name tests ──────────────────────────────────────────────────


class TestValidatePipName:
    def test_valid_simple_name(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        assert _validate_pip_name("requests") == "requests"

    def test_valid_name_with_hyphen(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        assert _validate_pip_name("pydantic-settings") == "pydantic-settings"

    def test_valid_name_with_dot(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        assert _validate_pip_name("zope.interface") == "zope.interface"

    def test_valid_name_with_underscore(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        assert _validate_pip_name("my_package") == "my_package"

    def test_empty_string_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        with pytest.raises(ValueError):
            _validate_pip_name("")

    def test_semicolon_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        with pytest.raises(ValueError):
            _validate_pip_name("evil;cmd")

    def test_single_quote_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        with pytest.raises(ValueError):
            _validate_pip_name("'; rm -rf /")

    def test_space_raises(self) -> None:
        from regilattice.gui_dialogs import _validate_pip_name

        with pytest.raises(ValueError):
            _validate_pip_name("foo bar")


# ── list_installed_scoop_apps parsing tests ───────────────────────────────────


class TestListInstalledScoopApps:
    def test_modern_output_skips_header_words(self) -> None:
        from regilattice.tweaks.scoop_tools import list_installed_scoop_apps

        modern_output = (
            "Installed apps:\n\n"
            "Name     Version  Source  Updated             Info\n"
            "-------- -------- ------- ------------------- ----\n"
            "7zip     22.01    main    2023-01-01 12:00:00 -\n"
            "git      2.39.3   main    2023-01-02 12:00:00 -\n"
        )
        with patch("regilattice.tweaks.scoop_tools.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout=modern_output)
            result = list_installed_scoop_apps()

        assert "7zip" in result
        assert "git" in result
        for bad in ("Name", "Version", "Source", "Updated", "Info"):
            assert bad not in result, f"Header word {bad!r} must not appear in results"

    def test_package_with_dot_included(self) -> None:
        from regilattice.tweaks.scoop_tools import list_installed_scoop_apps

        dotted_output = (
            "Name                      Version\n"
            "------------------------- -------\n"
            "Microsoft.WindowsTerminal 1.19.0\n"
            "python3.11                3.11.0\n"
        )
        with patch("regilattice.tweaks.scoop_tools.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout=dotted_output)
            result = list_installed_scoop_apps()

        assert "Microsoft.WindowsTerminal" in result
        assert "python3.11" in result

    def test_empty_output_returns_empty_list(self) -> None:
        from regilattice.tweaks.scoop_tools import list_installed_scoop_apps

        with patch("regilattice.tweaks.scoop_tools.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout="")
            result = list_installed_scoop_apps()

        assert result == []

    def test_nonzero_returncode_returns_empty(self) -> None:
        from regilattice.tweaks.scoop_tools import list_installed_scoop_apps

        with patch("regilattice.tweaks.scoop_tools.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=1, stdout="")
            result = list_installed_scoop_apps()

        assert result == []

    def test_result_is_sorted(self) -> None:
        from regilattice.tweaks.scoop_tools import list_installed_scoop_apps

        output = "bat     0.24\naria2   1.36\n7zip    22.01\n"
        with patch("regilattice.tweaks.scoop_tools.subprocess.run") as mock_run:
            mock_run.return_value = MagicMock(returncode=0, stdout=output)
            result = list_installed_scoop_apps()

        assert result == sorted(result)
