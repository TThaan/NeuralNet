using NeuralNet_UI.ViewModels;
using NeuralNetBuilder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NeuralNet_UI.Converters
{
    public class LayerOutputToItemsCollectionConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new ArgumentException($"{value.GetType().Name}");
            //ILayer layer = value as ILayer;
            //if (value as INeuronPlusWeightsVM[] == null)
            //    throw new ArgumentException("Hm..");
            INeuronPlusWeightsVM[] neuronPlusWeightsVMs = value as INeuronPlusWeightsVM[];
            return neuronPlusWeightsVMs;
            // var neurons = layer.Output.Select((x, i) => i).ToObservableCollection();
            // return neurons;

            //return new ObservableCollection<int> { 1, 2, 3 };
            //throw new ArgumentException($"{value.GetType().Name}");
            //try
            //{
            //    return new List<int> { 1, 2, 3 };
            //}
            //catch (Exception)
            //{
            //    return null;
            //}
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
