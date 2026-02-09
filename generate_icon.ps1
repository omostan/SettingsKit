Add-Type -AssemblyName System.Drawing

$size = 256
$outputPath = "icon.png"

# Create bitmap
$bmp = [System.Drawing.Bitmap]::new($size, $size)

# Create graphics
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::Transparent)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias

# Background gradient
$gradBrush = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
    [System.Drawing.Point]::new(0, 0),
    [System.Drawing.Point]::new($size, $size),
    [System.Drawing.Color]::FromArgb(37, 99, 235),
    [System.Drawing.Color]::FromArgb(30, 64, 175)
)
$g.FillRectangle($gradBrush, 0, 0, $size, $size)

# Decorative circles
$decor1 = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(40, 255, 255, 255))
$g.FillEllipse($decor1, 24, 24, 80, 80)

$decor2 = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(20, 255, 255, 255))
$g.FillEllipse($decor2, 142, 142, 100, 100)

# Center point
$centerX = $size / 2
$centerY = $size / 2

# Function to draw gear
function DrawGear {
    param($graphics, $x, $y, $radius, $color, $toothCount)
    
    # Outer circle
    $brush = [System.Drawing.SolidBrush]::new($color)
    $graphics.FillEllipse($brush, $x - $radius, $y - $radius, $radius * 2, $radius * 2)
    
    # Teeth
    $toothLength = $radius / 2
    for ($i = 0; $i -lt $toothCount; $i++) {
        $angle = ($i * 360.0 / $toothCount) * [math]::PI / 180.0
        $x1 = $x + [int]($radius * [math]::Cos($angle))
        $y1 = $y + [int]($radius * [math]::Sin($angle))
        $x2 = $x + [int](($radius + $toothLength) * [math]::Cos($angle))
        $y2 = $y + [int](($radius + $toothLength) * [math]::Sin($angle))
        
        $pen = [System.Drawing.Pen]::new($color, 3)
        $graphics.DrawLine($pen, $x1, $y1, $x2, $y2)
        $pen.Dispose()
    }
    
    # Inner white circle
    $innerRadius = $radius / 2
    $whiteBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::White)
    $graphics.FillEllipse($whiteBrush, $x - $innerRadius, $y - $innerRadius, $innerRadius * 2, $innerRadius * 2)
    
    # Inner gradient
    $innerGrad = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
        [System.Drawing.Point]::new($x - $innerRadius, $y - $innerRadius),
        [System.Drawing.Point]::new($x + $innerRadius, $y + $innerRadius),
        [System.Drawing.Color]::FromArgb(37, 99, 235),
        [System.Drawing.Color]::FromArgb(30, 64, 175)
    )
    $graphics.FillEllipse($innerGrad, $x - ($innerRadius / 2), $y - ($innerRadius / 2), $innerRadius, $innerRadius)
    
    $brush.Dispose()
    $whiteBrush.Dispose()
    $innerGrad.Dispose()
}

# Draw gears
DrawGear $g $centerX $centerY 48 ([System.Drawing.Color]::FromArgb(96, 165, 250)) 8
DrawGear $g ($centerX + 52) ($centerY - 52) 28 ([System.Drawing.Color]::FromArgb(150, 255, 255, 255)) 5
DrawGear $g ($centerX - 52) ($centerY + 52) 24 ([System.Drawing.Color]::FromArgb(150, 255, 255, 255)) 4

# Save image
$bmp.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)

# Cleanup
$gradBrush.Dispose()
$decor1.Dispose()
$decor2.Dispose()
$g.Dispose()
$bmp.Dispose()

Write-Host "Icon created successfully at: $outputPath"

