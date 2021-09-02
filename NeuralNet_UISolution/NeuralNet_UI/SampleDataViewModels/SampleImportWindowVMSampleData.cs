using NeuralNet_UI.ViewModels;
using System;
using static NeuralNet_UI.SampleData.MockData;

namespace NeuralNet_UI.SampleDataViewModels
{
    public class SampleImportWindowVMSampleData : SampleImportWindowVM
    {
        #region ctor
                
        public SampleImportWindowVMSampleData()
            : base(MockSessionContext, MockMediator)
        {
            //throw new ArgumentException($"SampleSet: {SampleSet}");
            
        }

        #endregion
    }
}
