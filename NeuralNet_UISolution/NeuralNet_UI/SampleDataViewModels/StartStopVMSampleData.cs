using NeuralNet_UI.FactoriesAndStewards;
using NeuralNet_UI.ViewModels;
using Autofac;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class StartStopVMSampleData : StartStopVM
    {
        public StartStopVMSampleData()
            : base(MockSessionContext, MockMediator, MockContainer.Resolve<IDelegateFactory>())
        {
        }
    }
}
