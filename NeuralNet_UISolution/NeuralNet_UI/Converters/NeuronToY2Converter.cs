using NeuralNet_UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class NeuronToY2Converter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // var contentPresenter = values[0] as ContentPresenter;
            var neuronPlusCurrentWeightIndex = values[0] as Tuple<NeuronPlusWeightsVM, int>;
            var neuron = neuronPlusCurrentWeightIndex.Item1;
            var index = neuronPlusCurrentWeightIndex.Item2;
            var canvas = values[1] as Canvas;

            // left = canvas width / layersCount * layerId + offset
            var topOfPrecedingLayersNeurons = (canvas.Height / neuron.MaxLayerLength) * index + 25;

            //throw new ArgumentException($"{left}");

            return topOfPrecedingLayersNeurons;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
