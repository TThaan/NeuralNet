using NeuralNet_UI.Commands.Async;
using NeuralNet_UI.FactoriesAndStewards;
using NeuralNet_Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static NeuralNet_UI.Helpers;

namespace NeuralNet_UI.ViewModels
{
    public interface IStartStopVM : IBaseVM
    {
        string ImportSamplesButtonText { get; }
        string InitializeNetButtonText { get; }
        string StepButtonText { get; }
        string TrainButtonText { get; }

        IAsyncCommand ShowSampleImportWindowCommand { get; }
        IAsyncRaisableCommand InitializeNetCommand { get; }
        IAsyncRaisableCommand TrainCommand { get; }
    }

    public class StartStopVM : BaseVM, IStartStopVM
    {
        #region fields & ctor

        private readonly Func<bool?> _showSampleImportWindow;
        private readonly PropertyChangedEventHandler _propertyChangedHandler_StatusVM;


        public StartStopVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            IDelegateFactory delegateFactory)
            : base(sessionContext, mediator)
        {
            _showSampleImportWindow = delegateFactory.ShowSampleImportWindow();
            _propertyChangedHandler_StatusVM = delegateFactory.GetPropertyChangedHandler_StatusVM();

            RegisterMediatorHandlers();
            DefineCommands();
        }

        #region helpers

        private void RegisterMediatorHandlers()
        { 
            _mediator.Register(MediatorToken.StartStopVM_OnSampleSetInitChange.ToString(), OnSamplesLoadedOrUnloaded);
            _mediator.Register(MediatorToken.StartStopVM_OnTrainerStatusChanged.ToString(), OnTrainerStatusChanged);
        }
        private void DefineCommands()
        {
            ShowSampleImportWindowCommand = new SimpleAsyncRelayCommand(ShowSampleImportWindow);
            InitializeNetCommand = new AsyncRelayCommand_Raisable(InitializeNetAsync, InitializeNetAsync_CanExecute)
            { IsConcurrentExecutionAllowed = true };    // CanExecute is called inside Execute. Thus 'IsConcurrentExecutionAllowed = true'.
            TrainCommand = new AsyncRelayCommand_Raisable(TrainAsync, TrainAsync_CanExecute)
            { IsConcurrentExecutionAllowed = true };
        }

        #endregion

        #endregion

        #region mediator handlers

        private void OnSamplesLoadedOrUnloaded(object obj)
        {
            OnPropertyChanged(nameof(ImportSamplesButtonText));
            OnPropertyChanged(nameof(InitializeNetButtonText));
            OnPropertyChanged(nameof(TrainButtonText));
            OnPropertyChanged(nameof(StepButtonText));

            TrainCommand.RaiseCanExecuteChanged();
        }
        private void OnTrainerStatusChanged(object obj = null)
        {
            OnPropertyChanged(nameof(TrainButtonText));
            OnPropertyChanged(nameof(StepButtonText));
            
            TrainCommand.RaiseCanExecuteChanged();
            InitializeNetCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Button Texts

        public string ImportSamplesButtonText => GetImportSamplesButtonText();
        public string InitializeNetButtonText => GetInitializeNetButtonText();
        public string TrainButtonText => GetTrainButtonText();
        public string StepButtonText => GetStepButtonText();

        #region helpers

        private string GetImportSamplesButtonText()
        {
            if(SessionContext.SampleSet.IsInitialized)
                return "Re-Load Samples";
            else return "Load Samples"; 
        }
        private string GetInitializeNetButtonText()
        {
            if (SessionContext.Net.IsInitialized)
                return "Re-Initialize Net";
            else return "Initialize Net";
        }
        private string GetTrainButtonText()
        {
            if (SessionContext.Trainer.Status == TrainerStatus.Running)
                return "Pause";
            else if (SessionContext.Trainer.Status == TrainerStatus.Paused)
                return "Continue";
            else if (SessionContext.Trainer.Status == TrainerStatus.Finished)
                return "Reset Trainer";
            else return "Train";
        }
        private string GetStepButtonText()
        {
            return "Step";
        }

        #endregion

        #endregion

        #region Commands

        public IAsyncCommand ShowSampleImportWindowCommand { get; private set; }
        public IAsyncRaisableCommand InitializeNetCommand { get; private set; }
        public IAsyncRaisableCommand TrainCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task ShowSampleImportWindow(object parameter)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _showSampleImportWindow();
                });
            });
        }
        private async Task InitializeNetAsync(object parameter)
        {
            try
            {
                await Task.Run(() =>
                {
                    SessionContext.Net.Initialize(SessionContext.NetParameters);
                });
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            finally
            {
                _mediator.NotifyColleagues(MediatorToken.NetVM_OnNetInitialized.ToString(), SessionContext.Net);   // in finally?
                _mediator.NotifyColleagues(MediatorToken.PredictVM_CanExecutesChanged.ToString(), SessionContext.Net);
                _mediator.NotifyColleagues(MediatorToken.PredictVM_OnInputOrOutputChanged.ToString(), SessionContext.Net);
                OnPropertyChanged(nameof(InitializeNetButtonText));
                OnTrainerStatusChanged();
            }
        }
        private bool InitializeNetAsync_CanExecute(object parameter)
        {
            return SessionContext.Trainer.Status == TrainerStatus.Undefined ||
                SessionContext.Trainer.Status == TrainerStatus.Finished;
        }
        private async Task TrainAsync(object parameter)
        {
            try
            {
                EnsureInitializedTrainer();

                if (SessionContext.Trainer.Status == TrainerStatus.Finished)
                {
                    SessionContext.Trainer.Reset();
                    return;
                }

                // Prevent shuffling samples after a pause.
                bool shuffleSamplesBeforeTraining = SessionContext.Trainer.Status == TrainerStatus.Initialized
                    ? true
                    : false; // Get from Parameters! 

                if (parameter as string == "Step")
                {
                    SessionContext.Trainer.Status = TrainerStatus.Paused;
                }
                else if (SessionContext.Trainer.Status == TrainerStatus.Running)
                {
                    SessionContext.Trainer.Status = TrainerStatus.Paused;
                    return;
                }
                else SessionContext.Trainer.Status = TrainerStatus.Running;

                await SessionContext.Trainer.TrainAsync(shuffleSamplesBeforeTraining, SessionContext.IsLogged ? SessionContext.Initializer.PathBuilder.Log : string.Empty);
                SessionContext.Net = SessionContext.Trainer.OriginalNet.GetCopy();
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); return; }
            finally 
            {
                // Check: Called twice after trainings end.
                _mediator.NotifyColleagues(MediatorToken.NetVM_OnNetChanged.ToString(), SessionContext.Trainer.OriginalNet);
                _mediator.NotifyColleagues(MediatorToken.PredictVM_OnInputOrOutputChanged.ToString(), SessionContext.Trainer.LearningNet);
            }
        }
        private bool TrainAsync_CanExecute(object parameter)
        {
            return SessionContext.SampleSet.IsInitialized
                && SessionContext.Net.IsInitialized;
        }

        #region helpers

        private void EnsureInitializedTrainer()
        {
            if (!SessionContext.Trainer.IsInitialized)
            {
                SessionContext.Trainer.PropertyChanged += _propertyChangedHandler_StatusVM;
                SessionContext.Trainer.Initialize(SessionContext.TrainerParameters, SessionContext.Net, SessionContext.SampleSet);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}