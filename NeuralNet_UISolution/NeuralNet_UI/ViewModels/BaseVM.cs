using NeuralNet_UI.Commands;
using NeuralNet_Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace NeuralNet_UI.ViewModels
{
    public interface IBaseVM
    {
        ICommand UnfocusCommand { get; set; }
        void Unfocus(object parameter);

        event PropertyChangedEventHandler PropertyChanged;
        bool IsPropertyChangedNull();
        void Initializer_PropertyChanged(object sender, PropertyChangedEventArgs e);    // hm..
        void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e);        // hm..
        void OtherVM_PropertyChanged(object sender, PropertyChangedEventArgs e);        // hm..
        // void OnAllPropertiesChanged();
    }

    public class BaseVM : INotifyPropertyChanged, IBaseVM
    {
        #region fields & ctor

        private Type viewModelType;
        protected readonly ISimpleMediator _mediator;

        public BaseVM(ISessionContext sessionContext, ISimpleMediator mediator)
        {
            SessionContext = sessionContext;
            _mediator = mediator;
            viewModelType = GetType();

            DefineCommands();
        }

        #region helpers

        private void DefineCommands()
        {
            UnfocusCommand = new SimpleRelayCommand(Unfocus);
        }

        #endregion

        #endregion

        public ISessionContext SessionContext { get; }

        #region Commands

        public ICommand UnfocusCommand { get; set; }

        #region Executes and CanExecutes

        public void Unfocus(object parameter)
        {
            var element = parameter as UIElement;
            element.Focusable = true;
            element.Focus();
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        // Check repeated registrations?
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Initializer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionContext.NetParameters) ||
                e.PropertyName == nameof(SessionContext.TrainerParameters))
                OnAllPropertiesChanged();
            else if (e.PropertyName == nameof(SessionContext.SampleSet) && viewModelType == typeof(SampleImportWindowVM))
            {
                OnPropertyChanged(nameof(SampleImportWindowVM.SamplesPreview));
                OnPropertyChanged(nameof(SampleImportWindowVM.LabelsPreview));
            }

            OnPropertyChanged(e.PropertyName);
        }
        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Redundant..
            if (e.PropertyName == nameof(ITrainer.Status))
            {
                _mediator.NotifyColleagues(MediatorToken.StartStopVM_OnTrainerStatusChanged.ToString(), null);
                _mediator.NotifyColleagues(MediatorToken.PredictVM_CanExecutesChanged.ToString(), null);
            }
            // only optional:
            else if (e.PropertyName == nameof(ITrainer.CurrentEpoch))
                _mediator.NotifyColleagues(MediatorToken.NetVM_OnNetChanged.ToString(), SessionContext.Trainer.OriginalNet);

            //else if (e.PropertyName == nameof(INotificationChanged.Notification) && viewModelType == typeof(StatusVM))
            //{ var stopper = true; }
            OnPropertyChanged(e.PropertyName);
        }
        public void OtherVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        protected virtual void OnAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        // debugging
        public bool IsPropertyChangedNull()
        {
            return PropertyChanged == null;
        }

        #endregion
    }
}