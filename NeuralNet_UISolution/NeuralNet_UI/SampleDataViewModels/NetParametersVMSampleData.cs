using NeuralNet_UI.ViewModels;
using System;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(MockSessionContext, MockMediator, null, null)
        {
            // throw new ArgumentException($"Count: {SessionContext.Initializer.ParameterBuilder.NetParameters.LayerParametersCollection.Count}");
        }

        #endregion
    }
}
