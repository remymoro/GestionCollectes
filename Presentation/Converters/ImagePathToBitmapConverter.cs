using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

public class ImagePathToBitmapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var path = value as string;
        if (string.IsNullOrWhiteSpace(path))
            path = "pack://application:,,,/Presentation/Resources/Images/Uploaded/default-store.png";

        if (!path.StartsWith("pack://") && !File.Exists(path))
        {
            // Essayez de le retrouver à partir du dossier de l'appli
            var absolute = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            if (File.Exists(absolute))
                path = absolute;
        }

        try { return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)); }
        catch { return null; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
