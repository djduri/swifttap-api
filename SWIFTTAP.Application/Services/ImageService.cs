using SkiaSharp;
using Svg;
using SWIFTTAP.Application.Constants;
using SWIFTTAP.Application.Helpers;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.Application.Services;

internal sealed class ImageService : IImageService
{
    // Główna metoda, która sprawdza typ pliku i wywołuje odpowiednią metodę
    public FileInMemory FitImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight)
    {
        // Sprawdzanie typu pliku
        switch (imageFile.ContentType.ToLower())
        {
            case "image/png":
                return FitPngImageToSize(imageFile, maxWidth, maxHeight);
            case "image/jpeg":
                return FitJpgImageToSize(imageFile, maxWidth, maxHeight);
            case "image/bmp":
                return FitBmpImageToSize(imageFile, maxWidth, maxHeight);
            case "image/svg+xml":
                return FitSvgImageToSize(imageFile, maxWidth, maxHeight);
            default:
                throw new InvalidOperationException("Unsupported image type.");
        }
    }

    // Metoda do generowania pustego obrazu (tło)
    public FileInMemory BlankImage(int maxWidth, int maxHeight)
    {
        using var bitmap = new SKBitmap(maxWidth, maxHeight, false);

        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColor.Parse("#FFFFFF"));

        using var outputBitmapStream = new MemoryStream();

        bitmap.Encode(outputBitmapStream, SKEncodedImageFormat.Png, 100);

        outputBitmapStream.Seek(0, SeekOrigin.Begin);

        return FileInMemory.CreateFromStream(outputBitmapStream, "blank.png", MimeTypes.Image.Png);
    }

    // Prywatna metoda do obsługi PNG
    private FileInMemory FitPngImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight)
    {
        using var bitmap = SKBitmap.Decode(imageFile.Data);
        using var outputBitmapStream = new MemoryStream();

        if (bitmap.Width <= maxWidth && bitmap.Height <= maxHeight)
        {
            bitmap.Encode(outputBitmapStream, SKEncodedImageFormat.Png, 100);
        }
        else
        {
            using var imageScaled = new SKBitmap(maxWidth, maxHeight);
            bitmap.ScalePixels(imageScaled, SKFilterQuality.High);
            imageScaled.Encode(outputBitmapStream, SKEncodedImageFormat.Png, 100);
        }

        outputBitmapStream.Seek(0, SeekOrigin.Begin);
        return FileInMemory.CreateFromStream(outputBitmapStream, imageFile.Name, MimeTypes.Image.Png);
    }

    // Prywatna metoda do obsługi JPG
    private FileInMemory FitJpgImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight)
    {
        using var bitmap = SKBitmap.Decode(imageFile.Data);
        using var outputBitmapStream = new MemoryStream();

        if (bitmap.Width <= maxWidth && bitmap.Height <= maxHeight)
        {
            bitmap.Encode(outputBitmapStream, SKEncodedImageFormat.Jpeg, 100);
        }
        else
        {
            float aspectRatio = (float)bitmap.Width / bitmap.Height;
            int newWidth = maxWidth;
            int newHeight = (int)(maxWidth / aspectRatio);

            if (newHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)(maxHeight * aspectRatio);
            }

            using var imageScaled = new SKBitmap(newWidth, newHeight);
            bitmap.ScalePixels(imageScaled, SKFilterQuality.High);
            imageScaled.Encode(outputBitmapStream, SKEncodedImageFormat.Jpeg, 100);
        }

        outputBitmapStream.Seek(0, SeekOrigin.Begin);
        return FileInMemory.CreateFromStream(outputBitmapStream, imageFile.Name, MimeTypes.Image.Jpeg);
    }

    // Prywatna metoda do obsługi BMP
    private FileInMemory FitBmpImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight)
    {
        using var bitmap = SKBitmap.Decode(imageFile.Data);
        using var outputBitmapStream = new MemoryStream();

        if (bitmap.Width <= maxWidth && bitmap.Height <= maxHeight)
        {
            bitmap.Encode(outputBitmapStream, SKEncodedImageFormat.Bmp, 100);
        }
        else
        {
            float aspectRatio = (float)bitmap.Width / bitmap.Height;
            int newWidth = maxWidth;
            int newHeight = (int)(maxWidth / aspectRatio);

            if (newHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)(maxHeight * aspectRatio);
            }

            using var imageScaled = new SKBitmap(newWidth, newHeight);
            bitmap.ScalePixels(imageScaled, SKFilterQuality.High);
            imageScaled.Encode(outputBitmapStream, SKEncodedImageFormat.Bmp, 100);
        }

        outputBitmapStream.Seek(0, SeekOrigin.Begin);
        return FileInMemory.CreateFromStream(outputBitmapStream, imageFile.Name, MimeTypes.Image.Bmp);
    }

    // Prywatna metoda do obsługi SVG
    private FileInMemory FitSvgImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight)
    {
        var svgDocument = SvgDocument.Open<SvgDocument>(new MemoryStream(imageFile.Data));

        var width = svgDocument.Width.Value;
        var height = svgDocument.Height.Value;

        float scaleX = (float)maxWidth / (float)width;
        float scaleY = (float)maxHeight / (float)height;
        float scale = Math.Min(scaleX, scaleY);

        svgDocument.Width = width * scale;
        svgDocument.Height = height * scale;

        using var outputSvgStream = new MemoryStream();
        svgDocument.Write(outputSvgStream);

        outputSvgStream.Seek(0, SeekOrigin.Begin);
        return FileInMemory.CreateFromStream(outputSvgStream, imageFile.Name, MimeTypes.Image.Svg);
    }


}
