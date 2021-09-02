using NeuralNet_UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NeuralNet_UI.Converters
{
    public class DataToCanvasConverter : IValueConverter
    {
        #region static fields

        private static bool positionsAreSet;

        private static Canvas canvas
            = new Canvas { Width = 1200, Height = 450, Background = new SolidColorBrush(Colors.AliceBlue) };
        // Tuple1: LayerId, NeuronId, Tuple2: CanvaxX, CanvasTop
        private static Dictionary<(int, int), (double, double)> neuronPositions
            = new Dictionary<(int, int), (double, double)>();

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // No need to remove neurons, only (active) weights..
            // except after net parameters change and re initializing net..
            canvas.Children.Clear();    

            var netVM = value as INetVM;

            if (netVM == null)
            {
                canvas.Children.Add(new TextBlock { Text = $"{GetType().Name}: 'value as INetVM' is null." });
                return canvas;
            }
            else if (!netVM.Net.IsInitialized)
            {
                canvas.Children.Add(new TextBlock { Text = $"{GetType().Name}: Net is not initialized yet." });
                return canvas;
            }

            var allWeights = netVM.AllWeights;
            var allWeightsOld = netVM.AllWeightsOld;
            int allWeightsCount = allWeights.Count;
            int layersCount = netVM.Net.Layers.Count();
            int maxLayerLength = netVM.Net.Layers.Max(x => x.N);

            if (allWeights == null)
            {
                canvas.Children.Add(new TextBlock { Text = $"{GetType().Name}: netVM's WeightPlusReceivingNeuronVMs is null." });
                return canvas;
            }

            if (!positionsAreSet || netVM.UpdateWholeNet)
            {
                neuronPositions.Clear();

                for (int layerId = 0; layerId < layersCount; layerId++)
                {
                    for (int neuronId = 0; neuronId < netVM.Net.Layers[layerId].N; neuronId++)
                    {
                        neuronPositions[(layerId, neuronId)] = (
                            (canvas.Width / layersCount) * (layerId) + 100,
                            (canvas.Height / maxLayerLength) * neuronId + 25);
                    }
                }

                positionsAreSet = true;
            }

            // Draw a circle for each neuron on the canvas and input/output values.

            foreach (var neuronPosition in neuronPositions)
            {
                var circle = new Ellipse
                {
                    Width = 40,
                    Height = 40,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2, 
                    Fill = new SolidColorBrush(Colors.White)
                };
                Panel.SetZIndex(circle, 2);

                //neuronCircle.RenderTransform. = new Point()
                Canvas.SetTop(circle, neuronPosition.Value.Item2 - 20);
                Canvas.SetLeft(circle, neuronPosition.Value.Item1 - 20);
                canvas.Children.Add(circle);
            }

            // Draw a line for each weight on the canvas.

            for (int i = 0; i < allWeightsCount; i++)
            {
                var weight = allWeights.ElementAt(i);
                var oldWeight = allWeightsOld?.ElementAt(i);

                var senderPosition = neuronPositions[(weight.LayerId - 1, weight.SenderId)];
                var receiverPosition = neuronPositions[(weight.LayerId, weight.ReceiverId)];

                var weightLine = new Line
                {
                    X1 = senderPosition.Item1,
                    X2 = receiverPosition.Item1,
                    Y1 = senderPosition.Item2,
                    Y2 = receiverPosition.Item2,
                    Stroke = GetColor(weight, oldWeight),
                    StrokeThickness = Math.Abs((float)weight.WeightValue) * 5
                    //, 
                    //StrokeDashArray = GetDashing()
                };

                canvas.Children.Add(weightLine);
            }

            return canvas;

            // throw new ArgumentException($"{value.GetType().Name}\n");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region helpers

        private SolidColorBrush GetColor(IWeightPlusNeuron weight, IWeightPlusNeuron oldWeight)
        {
            //if(oldWeight == null || weight.WeightValue == oldWeight.WeightValue)
            //    return (float)weight.WeightValue < 0 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);

            // return new SolidColorBrush(Colors.Blue);

            return (float)weight.WeightValue < 0 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
        }
        //private DoubleCollection GetDashing(IWeightPlusNeuron weight, IWeightPlusNeuron oldWeight)
        //{
        //    if (oldWeight == null || weight.WeightValue == oldWeight.WeightValue)
        //        new DoubleCollection();


        //}

        #endregion
    }
}
