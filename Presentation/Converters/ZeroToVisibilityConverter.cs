﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionCollectes.Presentation.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is int i && i == 0) ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
