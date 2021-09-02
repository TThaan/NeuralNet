using NeuralNet_UI.ViewModels;
using System.Linq;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        static int changingId = 0;

        public LayerParametersVMSampleData()
            : base(MockSessionContext, MockMediator)
        {
            // if(changingId >= SessionContext.Initializer.ParameterBuilder.NetParameters.LayerParametersCollection.Count)
            //     throw new ArgumentException($"Id: {changingId}, Count: {SessionContext.Initializer.ParameterBuilder.NetParameters.LayerParametersCollection.Count}");

            LayerParameters = MockSessionContext.NetParameters.LayerParametersCollection.ElementAt(changingId); // new MockLayerParameters(changingId);
            
            if (changingId < SessionContext.NetParameters.LayerParametersCollection.Count - 1)
                changingId++;
        }
    }
}
