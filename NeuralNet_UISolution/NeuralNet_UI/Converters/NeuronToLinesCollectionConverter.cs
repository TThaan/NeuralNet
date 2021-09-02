using NeuralNet_UI.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class NeuronToLinesCollectionConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // throw new ArgumentException($"{value.GetType().Name}\n");
            // throw new ArgumentException($"value = null: {value == null}\n");
            var neuron = value as NeuronPlusWeightsVM;
            if (value == null) return null;

            var result = Enumerable.Range(0, neuron.PrecedingLayersLength)
                .Select((x, i) => new Tuple<NeuronPlusWeightsVM, int>(item1: neuron, item2: i));
            return result.ToList();//neuron.IncomingWeights;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
