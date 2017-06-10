using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PowerManager.GUI
{
    /// <summary>
    /// Boolean To String Converter
    /// </summary>
    public class BoolToStringConverter : BooleanToValueConverter<String> { }

    /// <summary>
    /// Boolean to Color Converter
    /// </summary>
    public class BoolToBrushConverter : BooleanToValueConverter<Brush> { }

    /// <summary>
    /// Boolean To String Value Converter
    /// </summary>
    public class BooleanToValueConverter<T> : IValueConverter
    { 
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public BooleanToValueConverter()
        {

        }

        public BooleanToValueConverter(T falseVal, T trueVal)
        {
            FalseValue = falseVal;
            TrueValue = trueVal;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }
}
