//using AIDemoUI.ViewModels;
//using System;
//using System.Globalization;
//using System.Windows.Controls;
//using System.Windows.Data;

//namespace AIDemoUI.Converters
//{
//    public class NeuronToX1Converter : IMultiValueConverter
//    {
//        #region IValueConverter

//        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//        {
//            double result = 0;
//            var weight = values[0] as WeightPlusNeuron;
//            // var neuron = weight.Item1;
//            // var index = weight.Item2; // Index of neuron in preceding layer.
//            var canvas = values[1] as Canvas;

//            var point = parameter as string;

//            switch (point)
//            {
//                case "X1":
//                    if (weight.LayerId > 0)
//                        result = (canvas.Width / weight.LayersCount) * (weight.LayerId - 1) + 25;
//                    else result = 0;
//                    break;
//                case "X2":
//                    if (weight.LayerId > 0)
//                        result = (canvas.Width / weight.LayersCount) * weight.LayerId + 25;
//                    else result = 0;
//                    break;
//                case "Y1":
//                    result = (canvas.Height / weight.MaxLayerLength) * weight.SenderId + 25;
//                    break;
//                case "Y2":
//                    result = (canvas.Height / weight.MaxLayerLength) * weight.ReceiverId + 25;
//                    break;
//                default:
//                    break;
//            }

//            //throw new ArgumentException($"{leftOfPrecedingLayersNeurons}");

//            return result;
//        }
//        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion
//    }
//}
