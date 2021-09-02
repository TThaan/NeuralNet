using Autofac;
using NeuralNet_Core;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNet_UI.FactoriesAndStewards
{
    public interface IAllWeightsFactory
    {
        List<IWeightPlusNeuron> CreateAllWeights(INet net);
        List<IWeightPlusNeuron> CreateAllWeights(ILearningNet net);
    }
    
    public class AllWeightsFactory : IAllWeightsFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public AllWeightsFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region ILayerParametersVMCollectionFactory

        // Task: Refactor!
        public List<IWeightPlusNeuron> CreateAllWeights(ILearningNet net)
        {
            int layersCount = net.Layers.Length;
            int maxLayerLangth = net.Layers.Max(x => x.N);
            int neuronsCount = net.Layers.Sum(x => x.N);

            var result = new List<IWeightPlusNeuron>(neuronsCount);

            // for each layer except the input layer

            for (int layerId = 1; layerId < layersCount; layerId++)
            {
                var layer = net.Layers[layerId];
                int precedingLayersLength = layerId == 0 ? 0 : net.Layers[layerId - 1].N;

                // for each neuron in this layer (receiving a weight)

                for (int i = 0; i < layer.N; i++)
                {
                    // for each neuron in the preceding layer ("sending" a weight)

                    for (int k = 0; k < precedingLayersLength; k++)
                    {
                        // Create a class that holds the data of the sender neuron, the receiver neuron and the weight value

                        var neuronPlusWeight = _context.Resolve<IWeightPlusNeuron>();

                        // neuronPlusWeight.LayersCount = layersCount;
                        // neuronPlusWeight.MaxLayerLength = maxLayerLangth;
                        neuronPlusWeight.LayerId = layerId;
                        neuronPlusWeight.ReceiverId = i;
                        neuronPlusWeight.SenderId = k;
                        neuronPlusWeight.WeightValue = layerId == 0 ? null : (float?)layer.Weights[i, k];

                        result.Add(neuronPlusWeight);
                    }
                }
            }

            return result;
        }
        // Task: Refactor!
        public List<IWeightPlusNeuron> CreateAllWeights(INet net)
        {
            int layersCount = net.Layers.Length;
            int maxLayerLangth = net.Layers.Max(x => x.N);
            int neuronsCount = net.Layers.Sum(x => x.N);

            var result = new List<IWeightPlusNeuron>(neuronsCount);

            // for each layer except the input layer

            for (int layerId = 1; layerId < layersCount; layerId++)
            {
                var layer = net.Layers[layerId];
                int precedingLayersLength = layerId == 0 ? 0 : net.Layers[layerId - 1].N;

                // for each neuron in this layer (receiving a weight)

                for (int i = 0; i < layer.N; i++)
                {
                    // for each neuron in the preceding layer ("sending" a weight)

                    for (int k = 0; k < precedingLayersLength; k++)
                    {
                        // Create a class that holds the data of the sender neuron, the receiver neuron and the weight value
                        
                        var neuronPlusWeight = _context.Resolve<IWeightPlusNeuron>();

                        // neuronPlusWeight.LayersCount = layersCount;
                        // neuronPlusWeight.MaxLayerLength = maxLayerLangth;
                        neuronPlusWeight.LayerId = layerId;
                        neuronPlusWeight.ReceiverId = i;
                        neuronPlusWeight.SenderId = k;
                        neuronPlusWeight.WeightValue = layerId == 0 ? null : (float?)layer.Weights[i, k];                        

                        result.Add(neuronPlusWeight);
                    }
                }
            }

            return result;
        }

        #region helpers



        #endregion

        #endregion
    }
}
