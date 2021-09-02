using NeuralNet_Core;
using System.Windows;

namespace NeuralNet_UI.ViewModels
{
    public interface IStatusVM : IBaseVM
    {
        int CurrentEpoch { get; }
        int CurrentSample { get; }
        float CurrentTotalCost { get; }
        int SamplesTotal { get; }
        int Epochs { get; }
        float LastEpochsAccuracy { get; }
        string Notification { get; }
        Visibility DetailsVisibility { get; }
    }

    public class StatusVM : BaseVM, IStatusVM
    {
        #region fields & ctor

        public StatusVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            RegisterEvents();
        }

        #region helpers

        private void RegisterEvents()
        {
            SessionContext.Initializer.PropertyChanged += Initializer_PropertyChanged;
        }

        #endregion

        #endregion

        #region properties

        public string Notification
        {
            get
            {
                OnPropertyChanged(nameof(DetailsVisibility));
                if (SessionContext.Trainer?.Status == TrainerStatus.Running ||
                    SessionContext.Trainer?.Status == TrainerStatus.Paused ||
                    SessionContext.Trainer?.Status == TrainerStatus.Finished)
                    return $"{SessionContext.Trainer.Notification}";
                else
                    return $"{SessionContext.Initializer.Notification}";
            }
        }
        // Use only value of Status instead of those props?
        public int SamplesTotal => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.SamplesTotal;
        public int Epochs => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.Epochs;
        public int CurrentEpoch => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.CurrentEpoch + 1;
        public int CurrentSample => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.CurrentSample + 1;
        public float CurrentTotalCost => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.CurrentTotalCost;
        public float LastEpochsAccuracy => SessionContext.Trainer == null ? 0 : SessionContext.Trainer.LastEpochsAccuracy;
        public Visibility DetailsVisibility => GetDetailsVisibility();

        #region helpers

        protected virtual Visibility GetDetailsVisibility()
        {
            if (SessionContext.SampleSet == null ||
                SessionContext.Trainer == null || 
                SessionContext.Trainer.Status == TrainerStatus.Undefined ||
                SessionContext.Trainer.Status == TrainerStatus.Initialized)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        #endregion

        #endregion
    }
}