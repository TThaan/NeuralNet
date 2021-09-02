using NeuralNet_UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class NeuronPlusWeightsToCanvasLeftConverter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // throw new ArgumentException($"{values[1].GetType().Name}");
            var contentPresenter = values[0] as ContentPresenter;
            var canvas = values[1] as Canvas;
            var neuron = contentPresenter.DataContext as INeuronPlusWeightsVM;
            
            // left = canvas width / layersCount * layerId + offset
            var left = (canvas.Width / neuron.LayersCount) * neuron.LayerId + 25;

            //throw new ArgumentException($"{left}");

            return left;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
