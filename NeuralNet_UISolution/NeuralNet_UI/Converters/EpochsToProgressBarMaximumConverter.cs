using System;
using System.Globalization;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class EpochsToProgressBarMaximumConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int epochs = (int)value;
                return epochs == 0 ? 1 : epochs;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
