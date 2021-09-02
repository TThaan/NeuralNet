using NeuralNet_UI.ViewModels;
using System.Windows;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(MockSessionContext, MockMediator)
        {
        }

        protected override Visibility GetDetailsVisibility() => Visibility.Visible;
    }
}
