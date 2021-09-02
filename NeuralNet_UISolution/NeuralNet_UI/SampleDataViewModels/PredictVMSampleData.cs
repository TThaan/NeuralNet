using NeuralNet_UI.ViewModels;
using System.Linq;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class PredictVMSampleData : PredictVM
    {
        public PredictVMSampleData()
            : base(MockSessionContext, MockMediator)
        {
            //throw new ArgumentException($"{SessionContext.Net.Layers.First().Input[0]}");
            SessionContext.Net.Layers.First().Input = new float[] { -.9566f, .9007f, .8040f, -.8989f };
            SessionContext.Net.Layers.Last().Output = new float[] { 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f };
            _mediator.NotifyColleagues(MediatorToken.PredictVM_OnInputOrOutputChanged.ToString(), SessionContext.Net);
        }
    }
}
