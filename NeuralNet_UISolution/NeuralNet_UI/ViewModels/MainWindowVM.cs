using NeuralNet_UI.Commands;
using NeuralNet_UI.Commands.Async;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static NeuralNet_UI.Helpers;

namespace NeuralNet_UI.ViewModels
{
    public interface IMainWindowVM : IBaseVM
    {
        INetParametersVM NetParametersVM { get; set; }
        IStartStopVM StartStopVM { get; set; }
        IStatusVM StatusVM { get; set; }

        IAsyncCommand EnterLogNameCommand { get; }
        IAsyncCommand LoadNetParametersCommand { get; }
        IAsyncCommand SaveNetParametersCommand { get; }
        IAsyncCommand LoadTrainerParametersCommand { get; }
        IAsyncCommand SaveTrainerParametersCommand { get; }
        IAsyncCommand LoadInitializedNetCommand { get; }
        IAsyncCommand SaveInitializedNetCommand { get; }
        ICommand ExitCommand { get; }
    }

    public class MainWindowVM : BaseVM, IMainWindowVM
    {
        #region fields & ctor

        private INetParametersVM netParametersVM;
        private INetVM netVM;
        private IPredictVM predictVM;
        private IStatusVM statusVM;
        private IStartStopVM startStopVM;

        public MainWindowVM(ISessionContext sessionContext, INetParametersVM netParametersVM, INetVM netVM,
            IPredictVM predictVM, IStartStopVM startStopVM, IStatusVM statusVM, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            NetParametersVM = netParametersVM;
            NetVM = netVM;
            PredictVM = predictVM;
            StartStopVM = startStopVM;
            startStopVM.PropertyChanged += OtherVM_PropertyChanged;
            StatusVM = statusVM;

            DefineCommands();
        }

        #region helpers

        private void DefineCommands()
        {
            ExitCommand = new SimpleRelayCommand(Exit);
            LoadNetParametersCommand = new SimpleAsyncRelayCommand(LoadNetParametersAsync);
            SaveNetParametersCommand = new SimpleAsyncRelayCommand(SaveNetParametersAsync);
            LoadTrainerParametersCommand = new SimpleAsyncRelayCommand(LoadTrainerParametersAsync);
            SaveTrainerParametersCommand = new SimpleAsyncRelayCommand(SaveTrainerParametersAsync);
            LoadInitializedNetCommand = new SimpleAsyncRelayCommand(LoadInitializedNetAsync);
            SaveInitializedNetCommand = new SimpleAsyncRelayCommand(SaveInitializedNetAsync);
            EnterLogNameCommand = new SimpleAsyncRelayCommand(EnterLogNameAsync);
        }

        #endregion

        #endregion

        #region properties

        public INetParametersVM NetParametersVM
        {
            get { return netParametersVM; }
            set
            {
                if (netParametersVM != value)
                {
                    netParametersVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public INetVM NetVM
        {
            get { return netVM; }
            set
            {
                if (netVM != value)
                {
                    netVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public IPredictVM PredictVM
        {
            get { return predictVM; }
            set
            {
                if (predictVM != value)
                {
                    predictVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public IStatusVM StatusVM
        {
            get { return statusVM; }
            set
            {
                if (statusVM != value)
                {
                    statusVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public IStartStopVM StartStopVM
        {
            get { return startStopVM; }
            set
            {
                if (startStopVM != value)
                {
                    startStopVM = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public IAsyncCommand EnterLogNameCommand { get; private set; }
        public IAsyncCommand LoadNetParametersCommand { get; private set; }
        public IAsyncCommand SaveNetParametersCommand { get; private set; }
        public IAsyncCommand LoadTrainerParametersCommand { get; private set; }
        public IAsyncCommand SaveTrainerParametersCommand { get; private set; }
        public IAsyncCommand LoadInitializedNetCommand { get; private set; }
        public IAsyncCommand SaveInitializedNetCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        #region Executes

        private async Task EnterLogNameAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            saveFileDialog.Title = "Enter Logfile Name";
            // saveFileDialog.Filter = "Text| *.txt";
            // saveFileDialog.DefaultExt = ".txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            // StartStopVM.LogName = saveFileDialog.FileName;
                            // _sessionContext.Initializer.LogName = saveFileDialog.FileName;  // Use back end method or bring back OnPropChgd!
                            SessionContext.Initializer.PathBuilder.SetLogPath(saveFileDialog.FileName);
                        }
                        catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
                    });
                }
            }
        }
        private async Task LoadNetParametersAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            openFileDialog.Title = "Load Net Parameters";
            //openFileDialog.Filter = "Parameters|*.par";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    await SessionContext.Initializer.ParameterBuilder.LoadNetParametersAsync(openFileDialog.FileName);
                }
                catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            }
        }
        private async Task SaveNetParametersAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            saveFileDialog.Title = "Save Net Parameters";
            // saveFileDialog.Filter = "Parameters| *.par";
            // saveFileDialog.DefaultExt = ".par";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    try
                    {
                        await SessionContext.Initializer.ParameterBuilder.SaveNetParametersAsync(saveFileDialog.FileName, Formatting.Indented);
                    }
                    catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
                }
            }
        }
        private async Task LoadTrainerParametersAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            openFileDialog.Title = "Load Trainer Parameters";
            //openFileDialog.Filter = "Parameters|*.par";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    await SessionContext.Initializer.ParameterBuilder.LoadTrainerParametersAsync(openFileDialog.FileName);
                }
                catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            }
        }
        private async Task SaveTrainerParametersAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            saveFileDialog.Title = "Save Trainer Parameters";
            // saveFileDialog.Filter = "Parameters| *.par";
            // saveFileDialog.DefaultExt = ".par";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    try
                    {
                        await SessionContext.Initializer.ParameterBuilder.SaveTrainerParametersAsync(saveFileDialog.FileName, Formatting.Indented);
                    }
                    catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
                }
            }
        }
        private async Task LoadInitializedNetAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
            openFileDialog.Title = "Load Initialized Net";
            // openFileDialog.Filter = "Initialized Net|*.net";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    await SessionContext.Initializer.LoadNetAsync(openFileDialog.FileName);
                    //NetParametersVM.FileName = openFileDialog.FileName;
                    //_sessionContext.Net = DeSerialize<INet>(openFileDialog.FileName);
                }
                catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            }
        }
        private async Task SaveInitializedNetAsync(object parameter)
        {
            if (SessionContext.Net == null)
            {
                MessageBox.Show("No net initialized yet!");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = DefaultValues.ExamplesDirectory;
                saveFileDialog.Title = "Save Initialized Net";
                // saveFileDialog.Filter = "Net| *.net";
                // saveFileDialog.DefaultExt = ".net";

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await SessionContext.Initializer.SaveInitializedNetAsync(saveFileDialog.FileName);
                    }
                    catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
                }
            }
        }
        private void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }

        #region helpers

        private void Serialize<T>(T target, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))//
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, target);
            }
        }
        private T DeSerialize<T>(string name)
        {
            using (Stream stream = File.Open(name, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
