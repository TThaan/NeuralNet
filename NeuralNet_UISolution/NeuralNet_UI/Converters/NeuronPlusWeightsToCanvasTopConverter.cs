using NeuralNet_UI.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class NeuronPlusWeightsToCanvasTopConverter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contentPresenter = values[0] as ContentPresenter;
            var canvas = values[1] as Canvas;
            var neuron = contentPresenter.DataContext as INeuronPlusWeightsVM;

            // left = canvas width / layersCount * layerId + offset
            var top = (canvas.Height / neuron.MaxLayerLength) * neuron.NeuronId + 25;

            //throw new ArgumentException($"{top}");

            return top;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
