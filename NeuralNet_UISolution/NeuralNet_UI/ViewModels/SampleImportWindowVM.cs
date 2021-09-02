using NeuralNet_UI.Commands.Async;
using NeuralNet_UI.Views;
using DeepLearningDataProvider.SampleSetHelpers;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static NeuralNet_UI.Helpers;

namespace NeuralNet_UI.ViewModels
{
    public interface ISampleImportWindowVM : IBaseVM
    {
        int LabelColumn { get; set; }
        int TestSplitInPercent { get; set; }
        int SamplesCount { get; }
        /// <summary>
        /// Full name of the sample set file.
        /// Bad naming though.. 
        /// </summary>
        string SampleSet { get; }
        bool IsBusy { get; set; }   // redundant?
        IAsyncCommand InitializeCommand { get; }
        IAsyncCommand UnloadSamplesCommand { get; }
        IAsyncCommand OkCommand { get; }
        IAsyncCommand LoadSamplesCommand { get; }
    }

    public class SampleImportWindowVM : BaseVM, ISampleImportWindowVM
    {
        #region fields & ctor
                
        bool isBusy;

        public SampleImportWindowVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            RegisterEvents();
            DefineCommands();
        }

        #region helpers

        private void RegisterEvents()
        {
            SessionContext.Initializer.PropertyChanged += Initializer_PropertyChanged;
        }
        private void DefineCommands()
        {
            LoadSamplesCommand = new SimpleAsyncRelayCommand(LoadSamplesAsync);
            InitializeCommand = new SimpleAsyncRelayCommand(InitializeSampleSetAsync);
            UnloadSamplesCommand = new SimpleAsyncRelayCommand(UnloadSamplesAsync);
            OkCommand = new SimpleAsyncRelayCommand(OkAsync);
        }

        #endregion

        #endregion

        #region properties (No Commands)

        public string IgnoredColumnsAsString { get; set; }
        public int ColumnsCount { get; set; }
        public int LabelColumn { get; set; }
        public int TestSplitInPercent { get; set; } = 10;
        public int PreviewOfTargetsLimit { get; set; } = 10;
        public string SampleSet => SessionContext.Initializer.PathBuilder.SampleSet;
        public int SamplesCount => SessionContext.SampleSet.Samples.Length;
        public string SamplesPreview => SessionContext.SampleSet.GetPreviewOfSamples();
        public string LabelsPreview => SessionContext.SampleSet.GetPreviewOfTargets(PreviewOfTargetsLimit);

        // redundant?
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public IAsyncCommand LoadSamplesCommand { get; private set; }
        public IAsyncCommand UnloadSamplesCommand { get; private set; }
        public IAsyncCommand InitializeCommand { get; private set; }
        public IAsyncCommand OkCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task LoadSamplesAsync(object parameter)
        {
            int[] ignoredCols = IgnoredColumnsAsString?.FromStringToCollection<int>(new char[] { ',', ';', ' ' }).ToArray();    // from params

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            openFileDialog.Title = "Load Net Parameters";
            openFileDialog.Filter = "Parameters|*.csv; *.tsv; *.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    SessionContext.Initializer.PathBuilder.SetSampleSetPath(openFileDialog.FileName);
                    await SessionContext.SampleSet.LoadSamplesAsync(SampleSet, LabelColumn, ignoredCols);
                }
                catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
                finally
                {
                    OnPropertyChanged(nameof(SamplesPreview));
                    OnPropertyChanged(nameof(LabelsPreview));
                    _mediator.NotifyColleagues(MediatorToken.StartStopVM_OnSampleSetInitChange.ToString(), null);
                }
            }
        }
        private async Task InitializeSampleSetAsync(object parameter)
        {
            try
            {
                await Task.Run(() =>
                {
                    SessionContext.SampleSet.Initialize(TestSplitInPercent / 100m);
                });
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            finally
            {
                OnPropertyChanged(nameof(SamplesPreview));
                OnPropertyChanged(nameof(LabelsPreview));
                _mediator.NotifyColleagues(MediatorToken.StartStopVM_OnSampleSetInitChange.ToString(), null); 
            }

        }
        private async Task UnloadSamplesAsync(object parameter)
        {
            try
            {
                await Task.Run(() =>
                {
                    SessionContext.SampleSet.Reset();
                });
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            finally
            {
                OnPropertyChanged(nameof(SamplesPreview));
                OnPropertyChanged(nameof(LabelsPreview));
                _mediator.NotifyColleagues(MediatorToken.StartStopVM_OnSampleSetInitChange.ToString(), null); 
            }
        }
        private async Task OkAsync(object parameter)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    (parameter as SampleImportWindow)?.Hide();
                });
            });
        }

        #endregion

        #endregion
    }
}
