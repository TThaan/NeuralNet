using NeuralNet_UI.ViewModels;
using MachineLearningDataProvider.SampleSetHelpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace NeuralNet_UI
{
    public static class DefaultValues
    {
        #region properties

        public static string BaseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        public static string ExamplesDirectory = BaseDirectory + @"\Examples";
        public static string FileName_NetParameters { get; } = ExamplesDirectory + @"\FourPixCam\NetParameters.txt";
        public static string FileName_TrainerParameters { get; } = ExamplesDirectory + @"\FourPixCam\TrainerParameters.txt";
        public static string FileName_MockSampleSet { get; } = ExamplesDirectory + @"\FourPixCam\Samples.csv";

        #endregion

        #region

        public static async Task<IMainWindowVM> SetDefaultValues(this IMainWindowVM mainWindowVM, ISessionContext sessionContext)
        {
            try
            {
                await sessionContext.SetDefaultValuesAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Default Parameters could not be loaded.\n({e.Message})");
            }

            return mainWindowVM;
        }
        public static async Task<ISessionContext> SetDefaultValuesAsync(this ISessionContext sessionContext, bool allInitialized = false, bool inclSamples = false)
        {
            if (allInitialized && !inclSamples)
                throw new ArgumentException("Trainer can only be initialized with loaded and initialized sample set.");

            if (inclSamples)
                await sessionContext.SampleSet.LoadSamplesAsync(FileName_MockSampleSet, 0, null);

            await sessionContext.Initializer.ParameterBuilder.LoadNetParametersAsync(FileName_NetParameters);
            await sessionContext.Initializer.ParameterBuilder.LoadTrainerParametersAsync(FileName_TrainerParameters);

            if(allInitialized)
            {
                sessionContext.SampleSet.Initialize(20 / 100m);
                sessionContext.Net.Initialize(sessionContext.NetParameters);
                sessionContext.Trainer.Initialize(sessionContext.TrainerParameters, sessionContext.Net, sessionContext.SampleSet);
            }

            return sessionContext;
        }

        #endregion
    }
}
