#Requires -Version 5.1
<#
.SYNOPSIS
    Generates a multi-size, 32bpp RGBA ICO file for RegiLattice.

.DESCRIPTION
    Draws the app icon (gradient blue-to-cyan background with bold "R" and key notch)
    at 16, 32, 48, 64, 128, and 256 px, saves each frame as a PNG, then packs them
    into a valid ICO file using the modern PNG-in-ICO format (Vista+, 32bpp).

    The resulting ICO is placed at:
        src/RegiLattice.GUI/app.ico

.NOTES
    Run from the repository root:
        . .\scripts\Generate-AppIcon.ps1
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms

$repoRoot = Split-Path $PSScriptRoot -Parent
$outPath  = Join-Path $repoRoot 'src\RegiLattice.GUI\app.ico'

# ── Helper: draw a rounded rectangle ──────────────────────────────────────────
function Invoke-RoundedRectFill {
    param(
        [System.Drawing.Graphics]$g,
        [System.Drawing.Brush]$brush,
        [System.Drawing.Rectangle]$rect,
        [int]$radius
    )
    $path = [System.Drawing.Drawing2D.GraphicsPath]::new()
    $d = $radius * 2
    $path.AddArc($rect.X,              $rect.Y,               $d, $d, 180, 90)
    $path.AddArc($rect.Right  - $d,    $rect.Y,               $d, $d, 270, 90)
    $path.AddArc($rect.Right  - $d,    $rect.Bottom - $d,     $d, $d,   0, 90)
    $path.AddArc($rect.X,              $rect.Bottom - $d,     $d, $d,  90, 90)
    $path.CloseFigure()
    $g.FillPath($brush, $path)
    $path.Dispose()
}

# ── Draw the RegiLattice app icon at any size ─────────────────────────────────
function Invoke-AppIconDraw {
    param([System.Drawing.Graphics]$g, [int]$s)

    $g.SmoothingMode      = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.InterpolationMode  = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $g.TextRenderingHint  = [System.Drawing.Text.TextRenderingHint]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)

    # Gradient background: blue → cyan (ForwardDiagonal)
    $gradient = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
        [System.Drawing.Rectangle]::new(0, 0, $s, $s),
        [System.Drawing.Color]::FromArgb(30, 102, 245),
        [System.Drawing.Color]::FromArgb(0, 195, 255),
        [System.Drawing.Drawing2D.LinearGradientMode]::ForwardDiagonal
    )

    $radius = [Math]::Max(3, [int]($s * 0.15))
    $margin = 1
    Invoke-RoundedRectFill $g $gradient ([System.Drawing.Rectangle]::new($margin, $margin, $s - 2*$margin, $s - 2*$margin)) $radius
    $gradient.Dispose()

    # Bold "R" centred on the icon
    $fgBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::White)
    $fontSize = [float]([Math]::Max(8, $s * 0.50))
    $font = [System.Drawing.Font]::new('Segoe UI', $fontSize, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
    $sf = [System.Drawing.StringFormat]::new()
    $sf.Alignment     = [System.Drawing.StringAlignment]::Center
    $sf.LineAlignment = [System.Drawing.StringAlignment]::Center
    # Shift slightly up to leave room for the key notch
    $centerY = $s / 2.0 - ($s * 0.02)
    $g.DrawString('R', $font, $fgBrush, [float]($s / 2.0), [float]$centerY, $sf)
    $font.Dispose()
    $sf.Dispose()
    $fgBrush.Dispose()

    # Small key notch in the bottom-right quadrant (scaled with icon size)
    $penW  = [float]([Math]::Max(1.0, $s / 20.0))
    $pen   = [System.Drawing.Pen]::new([System.Drawing.Color]::White, $penW)
    $pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.EndCap   = [System.Drawing.Drawing2D.LineCap]::Round

    # Horizontal bar of the key
    $kx1 = $s - [int]($s * 0.30)
    $kx2 = $s - [int]($s * 0.08)
    $ky  = $s - [int]($s * 0.20)
    $g.DrawLine($pen, [float]$kx1, [float]$ky, [float]$kx2, [float]$ky)

    # Two small vertical teeth
    $tooth = [int]($s * 0.09)
    $tx1   = $s - [int]($s * 0.22)
    $tx2   = $s - [int]($s * 0.14)
    $g.DrawLine($pen, [float]$tx1, [float]$ky, [float]$tx1, [float]($ky - $tooth))
    $g.DrawLine($pen, [float]$tx2, [float]$ky, [float]$tx2, [float]($ky - $tooth * 0.6))
    $pen.Dispose()
}

# ── Sizes to include in the ICO ───────────────────────────────────────────────
$sizes = @(16, 32, 48, 64, 128, 256)

Write-Host "[Generate-AppIcon] Rendering $($sizes.Count) sizes: $($sizes -join ', ') px..."

# Build PNG byte arrays for each size
$pngChunks = foreach ($sz in $sizes) {
    $bmp = [System.Drawing.Bitmap]::new($sz, $sz, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    Invoke-AppIconDraw $g $sz
    $g.Dispose()

    $ms = [System.IO.MemoryStream]::new()
    $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    , $ms.ToArray()   # wrap in array to avoid unrolling in foreach
    $ms.Dispose()
}

# ── Pack into ICO format ───────────────────────────────────────────────────────
# Header: 6 bytes
# Directory: N × 16 bytes
# PNG data: variable
$headerSize    = 6
$dirEntrySize  = 16
$dataOffset    = $headerSize + $sizes.Count * $dirEntrySize

$ms   = [System.IO.MemoryStream]::new()
$bw   = [System.IO.BinaryWriter]::new($ms)

# ICO file header
$bw.Write([int16]0)                  # Reserved
$bw.Write([int16]1)                  # Type: 1 = ICO
$bw.Write([int16]$sizes.Count)       # Number of images

# Directory entries
$offset = $dataOffset
for ($i = 0; $i -lt $sizes.Count; $i++) {
    $sz    = $sizes[$i]
    $chunk = $pngChunks[$i]
    $w     = if ($sz -ge 256) { 0 } else { $sz }   # 0 encodes 256 in ICO format
    $h     = if ($sz -ge 256) { 0 } else { $sz }

    $bw.Write([byte]$w)               # Width
    $bw.Write([byte]$h)               # Height
    $bw.Write([byte]0)                # Color count (0 = TrueColor)
    $bw.Write([byte]0)                # Reserved
    $bw.Write([int16]1)               # Planes
    $bw.Write([int16]32)              # Bit depth
    $bw.Write([int32]$chunk.Length)   # Data size
    $bw.Write([int32]$offset)         # Offset from start of file
    $offset += $chunk.Length
}

# PNG data
foreach ($chunk in $pngChunks) {
    $bw.Write($chunk)
}

$bw.Flush()
$icoBytes = $ms.ToArray()
$bw.Dispose()
$ms.Dispose()

[System.IO.File]::WriteAllBytes($outPath, $icoBytes)
Write-Host "[Generate-AppIcon] Written: $outPath  ($($icoBytes.Length) bytes, $($sizes.Count) images)"

# Print a quick summary of what was embedded
Write-Host "[Generate-AppIcon] ICO directory:"
for ($i = 0; $i -lt $sizes.Count; $i++) {
    $sz  = $sizes[$i]
    $len = $pngChunks[$i].Length
    Write-Host "  ${sz}x${sz} PNG  $len bytes"
}
Write-Host "[Generate-AppIcon] Done."
