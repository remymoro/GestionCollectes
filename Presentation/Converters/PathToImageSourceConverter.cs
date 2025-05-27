using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace GestionCollectes.Presentation.Converters


{

    public class PathToImageSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                // S'il n'est pas absolu, on le base sur l'emplacement de l'exe
                string finalPath = Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                if (File.Exists(finalPath))
                    return new BitmapImage(new Uri(finalPath, UriKind.Absolute));
            }
            // Image par défaut
            return new BitmapImage(new Uri("pack://application:,,,/Presentation/Resources/Images/Uploaded/default-store.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }


}