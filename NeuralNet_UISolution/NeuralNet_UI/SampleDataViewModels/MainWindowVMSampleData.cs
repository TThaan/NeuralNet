using NeuralNet_UI.ViewModels;
using System;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class MainWindowVMSampleData : MainWindowVM
    {
        #region ctor

        public MainWindowVMSampleData()
            : base(MockSessionContext, new NetParametersVMSampleData(), new NetVMSampleData(), new PredictVMSampleData(), new StartStopVMSampleData(), new StatusVMSampleData(), MockMediator)
        {
            // throw new ArgumentException($"Exception in: {GetType().Name}");
        }

        #endregion
    }
}
