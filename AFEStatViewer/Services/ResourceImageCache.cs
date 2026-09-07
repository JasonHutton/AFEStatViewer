using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AFEStatViewer.Services
{
    internal static class ResourceImageCache
    {
        private static readonly ConcurrentDictionary<string, ImageSource?> Cache = new();

        public static ImageSource? LoadClassIcon(string? filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return null;
            }

            return Cache.GetOrAdd(filename, LoadClassIconInternal);
        }

        private static ImageSource? LoadClassIconInternal(string filename)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/Resources/ClassIcons/{filename}", UriKind.Absolute);
                var resource = Application.GetResourceStream(uri);

                if (resource == null)
                {
                    return null;
                }

                using (resource.Stream)
                {
                    var image = new BitmapImage();

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = resource.Stream;
                    image.EndInit();
                    image.Freeze();

                    return image;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}