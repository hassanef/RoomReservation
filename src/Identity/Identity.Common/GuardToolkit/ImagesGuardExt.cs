using Microsoft.AspNetCore.Http;
using SkiaSharp;
using System.IO;

namespace Identity.Common.GuardToolkit
{
    public static class ImagesGuardExt
    {
        public static bool IsImageFile(this byte[] photoFile)
        {
            if (photoFile == null || photoFile.Length == 0)
                return false;

            using var memoryStream = new MemoryStream(photoFile);
            return TryGetImageInfo(memoryStream, out _);
        }

        public static bool IsValidImageFile(this IFormFile photoFile, int maxWidth = 150, int maxHeight = 150)
        {
            if (photoFile == null || photoFile.Length == 0)
                return false;

            using var stream = photoFile.OpenReadStream();
            return TryGetImageInfo(stream, out var info)
                && info.Width <= maxWidth
                && info.Height <= maxHeight;
        }

        public static bool IsImageFile(this IFormFile photoFile)
        {
            if (photoFile == null || photoFile.Length == 0)
                return false;

            using var stream = photoFile.OpenReadStream();
            return TryGetImageInfo(stream, out _);
        }

        private static bool TryGetImageInfo(Stream stream, out SKImageInfo info)
        {
            using var managedStream = new SKManagedStream(stream, false);
            using var codec = SKCodec.Create(managedStream);
            if (codec == null)
            {
                info = default;
                return false;
            }

            info = codec.Info;
            return info.Width > 0 && info.Height > 0;
        }
    }
}
