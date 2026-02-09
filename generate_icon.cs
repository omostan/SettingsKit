using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

class IconGenerator
{
    static void Main()
    {
        const int size = 256;
        const string outputPath = @"D:\Tutorials\SettingsKit\icon.png";

        // Create a new bitmap
        using (Bitmap bmp = new Bitmap(size, size))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Background gradient
                using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                    new Point(0, 0), new Point(size, size),
                    Color.FromArgb(37, 99, 235),    // #2563eb
                    Color.FromArgb(30, 64, 175)))   // #1e40af
                {
                    g.FillRectangle(bgBrush, 0, 0, size, size);
                }

                // Draw decorative circles
                using (Brush decor1 = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                {
                    g.FillEllipse(decor1, 24, 24, 80, 80);
                }

                using (Brush decor2 = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
                {
                    g.FillEllipse(decor2, 142, 142, 100, 100);
                }

                // Main gear center point
                int centerX = size / 2;
                int centerY = size / 2;

                // Draw main gear (central large gear)
                DrawGear(g, centerX, centerY, 48, Color.FromArgb(96, 165, 250), 8);

                // Draw secondary gear (top-right)
                DrawGear(g, centerX + 52, centerY - 52, 28, Color.FromArgb(150, 255, 255, 255), 5);

                // Draw secondary gear (bottom-left)
                DrawGear(g, centerX - 52, centerY + 52, 24, Color.FromArgb(150, 255, 255, 255), 4);

                // Save the image
                bmp.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine($"Icon created successfully at: {outputPath}");
            }
        }
    }

    static void DrawGear(Graphics g, int centerX, int centerY, int radius, Color color, int teethCount)
    {
        // Draw outer gear circle
        using (Brush brush = new SolidBrush(color))
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius, radius * 2, radius * 2);
        }

        // Draw teeth
        int toothLength = radius / 2;
        for (int i = 0; i < teethCount; i++)
        {
            double angle = (i * 360.0 / teethCount) * Math.PI / 180.0;
            int x1 = centerX + (int)(radius * Math.Cos(angle));
            int y1 = centerY + (int)(radius * Math.Sin(angle));
            int x2 = centerX + (int)((radius + toothLength) * Math.Cos(angle));
            int y2 = centerY + (int)((radius + toothLength) * Math.Sin(angle));

            using (Pen pen = new Pen(color, 3))
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        // Draw center white circle
        int innerRadius = radius / 2;
        using (Brush whiteBrush = new SolidBrush(Color.White))
        {
            g.FillEllipse(whiteBrush, centerX - innerRadius, centerY - innerRadius,
                innerRadius * 2, innerRadius * 2);
        }

        // Draw gradient fill inside
        using (LinearGradientBrush gradBrush = new LinearGradientBrush(
            new Point(centerX - innerRadius, centerY - innerRadius),
            new Point(centerX + innerRadius, centerY + innerRadius),
            Color.FromArgb(37, 99, 235), Color.FromArgb(30, 64, 175)))
        {
            g.FillEllipse(gradBrush, centerX - innerRadius / 2, centerY - innerRadius / 2,
                innerRadius, innerRadius);
        }
    }
}

