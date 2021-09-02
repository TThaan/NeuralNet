namespace NeuralNet_UI
{
    public interface IWeightPlusNeuron
    {
        //int LayersCount { get; set; }
        //int MaxLayerLength { get; set; }
        // int PrecedingLayersLength { get; set; }
        int LayerId { get; set; }
        int ReceiverId { get; set; }
        int SenderId { get; set; }

        float? WeightValue { get; set; }
        //int? CurrentlyHandledWeight { get; set; }
    }

    public class WeightPlusNeuron : IWeightPlusNeuron
    {
        #region INeuronPlusWeightsVM

        //public int LayersCount { get; set; }
        //public int MaxLayerLength { get; set; }
        // public int PrecedingLayersLength { get; set; }
        public int LayerId { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }

        public float? WeightValue { get; set; }
        //public int? CurrentlyHandledWeight { get; set; }

        #endregion
    }
}
