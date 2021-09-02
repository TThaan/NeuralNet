using NeuralNet_UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class NeuronToX2Converter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // var contentPresenter = values[0] as ContentPresenter;
            var neuronPlusCurrentWeightIndex = values[0] as Tuple<NeuronPlusWeightsVM, int>;
            var neuron = neuronPlusCurrentWeightIndex.Item1;
            var index = neuronPlusCurrentWeightIndex.Item2;
            var canvas = values[1] as Canvas;

            double leftOfPrecedingLayersNeurons;

            if (neuron.LayerId > 0)
                leftOfPrecedingLayersNeurons = (canvas.Width / neuron.LayersCount) * (neuron.LayerId - 1) + 25;
            else leftOfPrecedingLayersNeurons = 0;

            //throw new ArgumentException($"{leftOfPrecedingLayersNeurons}");

            return leftOfPrecedingLayersNeurons;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
