using System;
using System.Globalization;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class TestConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(value==null)
            //    throw new ArgumentException($"value = null: {value == null}\n");
            //throw new ArgumentException($"{value.GetType().Name}\n");
            
            if (value == null) 
                return "value is null";
            else
                return "value type: " + value.GetType().Name;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
