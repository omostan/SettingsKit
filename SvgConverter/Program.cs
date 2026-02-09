using Svg;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string svgPath = args.Length > 0 ? args[0] : "icon.svg";
        string outputPath = args.Length > 1 ? args[1] : "icon.png";
        int width = args.Length > 2 ? int.Parse(args[2]) : 256;
        int height = args.Length > 3 ? int.Parse(args[3]) : 256;

        try
        {
            if (!File.Exists(svgPath))
            {
                Console.Error.WriteLine($"Error: SVG file not found: {svgPath}");
                Environment.Exit(1);
            }

            // Load SVG
            SvgDocument svgDocument = SvgDocument.Open(svgPath);
            svgDocument.Width = new SvgUnit(SvgUnitType.Pixel, width);
            svgDocument.Height = new SvgUnit(SvgUnitType.Pixel, height);

            // Convert to bitmap
            using (var bitmap = svgDocument.Draw())
            {
                bitmap.Save(outputPath);
                Console.WriteLine($"Successfully converted {svgPath} to {outputPath}");
                Console.WriteLine($"Output size: {width}x{height}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error converting SVG: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}

