using NeuralNet_UI.FactoriesAndStewards;
using NeuralNetBuilder;
using System;
using System.Collections.Generic;

namespace NeuralNet_UI.ViewModels
{
    public interface INetVM
    {
        INet Net { get; set; }
        INetVM ThisNetVM { get; }
        List<IWeightPlusNeuron> AllWeightsOld { get; }
        List<IWeightPlusNeuron> AllWeights { get; }
        bool UpdateWholeNet { get; }
    }

    public class NetVM : BaseVM, INetVM
    {
        #region fields & ctor

        private readonly IAllWeightsFactory _allWeightsFactory;
        private List<IWeightPlusNeuron> allWeights;

        public NetVM(ISessionContext sessionContext, ISimpleMediator mediator, IAllWeightsFactory allWeightsFactory)
            : base(sessionContext, mediator)
        {
            _allWeightsFactory = allWeightsFactory;

            RegisterEvents();
            RegisterMediatorHandlers();
        }

        #region helpers

        private void RegisterEvents()
        {
            SessionContext.Initializer.PropertyChanged += Initializer_PropertyChanged;  // redundant?
            SessionContext.Trainer.PropertyChanged += Trainer_PropertyChanged;
        }
        private void RegisterMediatorHandlers()
        {
            _mediator.Register(MediatorToken.NetVM_OnNetInitialized.ToString(), OnNetInitialized);
            _mediator.Register(MediatorToken.NetVM_OnNetChanged.ToString(), OnNetChanged);
        }

        #endregion

        #endregion

        #region mediator handlers
        
        private void OnNetInitialized(object obj)
        {
            UpdateWholeNet = true;
            OnNetChanged(obj);
        }
        private void OnNetChanged(object obj)
        {
            CreateAllWeights(obj);
        }

        #endregion

        #region INet

        public INet Net
        {
            get { return SessionContext.Net; }
            set
            {
                SessionContext.Net = value;
                OnPropertyChanged();
            }
        }
        public INetVM ThisNetVM => this;
        public List<IWeightPlusNeuron> AllWeightsOld { get; private set; }
        public List<IWeightPlusNeuron> AllWeights 
        {
            get { return allWeights; }
            private set
            {
                if(allWeights != null)
                    AllWeightsOld = new List<IWeightPlusNeuron>(allWeights);

                allWeights = value;
            }
        }
        public bool UpdateWholeNet { get; private set; }

        #endregion

        #region helpers

        private void CreateAllWeights(object obj)
        {
            if (obj is INet)
                AllWeights = _allWeightsFactory.CreateAllWeights(obj as INet);
            else if (obj is ILearningNet)
                AllWeights = _allWeightsFactory.CreateAllWeights(obj as ILearningNet);
            else throw new ArgumentException();

            OnPropertyChanged(nameof(MainWindowVM.NetVM));
            OnAllPropertiesChanged();   // Only "ThisNetVM" needed?
        }

        #endregion
    }
}
