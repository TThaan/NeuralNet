using NeuralNet_UI.ViewModels;
using System;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class NetVMSampleData : NetVM
    {
        #region ctor

        public NetVMSampleData()
            : base(MockSessionContext, MockMediator, MockAllWeightsFactory)
        {
            // throw new ArgumentException($"Exception in: {GetType().Name}");

            // Update UI at design time.
            _mediator.NotifyColleagues(MediatorToken.NetVM_OnNetInitialized.ToString(), SessionContext.Net);
        }

        #endregion
    }
}
