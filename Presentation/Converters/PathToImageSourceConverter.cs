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
        if (value is string path && !string.IsNullOrWhiteSpace(path) && File.Exists(path))
            return new BitmapImage(new Uri(path, UriKind.Absolute));
        // Retourne une image par défaut si aucun chemin
        return new BitmapImage(new Uri("pack://application:,,,/Presentation/Resources/Images/Uploaded/default-store.png")); // Mets ton image par défaut ici
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

}