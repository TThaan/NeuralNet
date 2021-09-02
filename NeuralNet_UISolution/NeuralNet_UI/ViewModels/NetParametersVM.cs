using NeuralNet_UI.Commands;
using NeuralNet_UI.Commands.Async;
using NeuralNet_UI.FactoriesAndStewards;
using NeuralNet_Core;
using NeuralNet_Core.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NeuralNet_UI.ViewModels
{
    public interface INetParametersVM : IBaseVM
    {
        ObservableCollection<ILayerParameters> LayerParametersCollection { get; }
        ILayerParametersVMFactory LayerParametersVMFactory { get; }
        IEnumerable<CostType> CostTypes { get; }
        IEnumerable<WeightInitType> WeightInitTypes { get; }
        CostType CostType { get; set; }//Selected?
        WeightInitType WeightInitType { get; set; }//Selected?
        //bool AreParametersGlobal { get; set; }
        //float BiasMin_Global { get; set; }
        //float BiasMax_Global { get; set; }
        //float WeightMin_Global { get; set; }
        //float WeightMax_Global { get; set; }
        int EpochCount { get; set; }
        float LearningRate { get; set; }
        float LearningRateChange { get; set; }
        // string FileName { get; set; }

        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand MoveLeftCommand { get; }
        ICommand MoveRightCommand { get; }
        IAsyncCommand UseGlobalParametersCommand { get; }
    }

    public class NetParametersVM : BaseVM, INetParametersVM
    {
        #region fields & ctor

        private IEnumerable<CostType> costTypes;
        private IEnumerable<WeightInitType> weightInitTypes;

        public NetParametersVM(ISessionContext sessionContext, ISimpleMediator mediator, 
            ILayerParametersVMFactory layerParametersVMFactory, ILayerParametersFactory layerParametersFactory)
            : base(sessionContext, mediator)
        {
            LayerParametersVMFactory = layerParametersVMFactory;
            
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
            AddCommand = new SimpleRelayCommand(Add);
            DeleteCommand = new SimpleRelayCommand(Delete);
            MoveLeftCommand = new SimpleRelayCommand(MoveLeft);
            MoveRightCommand = new SimpleRelayCommand(MoveRight);
            // UseGlobalParametersCommand = new SimpleAsyncRelayCommand(UseGlobalParametersAsync);
        }

        #endregion

        #endregion

        #region public

        public ObservableCollection<ILayerParameters> LayerParametersCollection => SessionContext.LayerParametersCollection;
        public ILayerParametersVMFactory LayerParametersVMFactory { get; }
        public IEnumerable<CostType> CostTypes => costTypes ??
            (costTypes = Enum.GetValues(typeof(CostType)).ToList<CostType>());
        public IEnumerable<WeightInitType> WeightInitTypes => weightInitTypes ??
            (weightInitTypes = Enum.GetValues(typeof(WeightInitType)).ToList<WeightInitType>());

        //public string FileName
        //{
        //    get { return _sessionContext.Initializer.Paths.NetParameters; }
        //    set
        //    {
        //        if (_sessionContext.Initializer.Paths.NetParameters != value)
        //        {
        //            _sessionContext.Initializer.Paths.SetNetParametersPath(value);
        //            // OnPropertyChanged();
        //        }
        //    }
        //}
        //public bool AreParametersGlobal
        //{
        //    get { return areParametersGlobal; }
        //    set
        //    {
        //        if (areParametersGlobal != value)
        //        {
        //            areParametersGlobal = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public bool AreParametersGlobal_IsCheckboxDisabled
        //{
        //    get { return areParametersGlobal_IsCheckboxDisabled; }
        //    set
        //    {
        //        if (areParametersGlobal_IsCheckboxDisabled != value)
        //        {
        //            areParametersGlobal_IsCheckboxDisabled = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public float WeightMin_Global
        //{
        //    get { return weightMin_Global; }
        //    set
        //    {
        //        if (weightMin_Global != value)
        //        {
        //            weightMin_Global = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public float WeightMax_Global
        //{
        //    get { return weightMax_Global; }
        //    set
        //    {
        //        if (weightMax_Global != value)
        //        {
        //            weightMax_Global = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public float BiasMin_Global
        //{
        //    get { return biasMin_Global; }
        //    set
        //    {
        //        if (biasMin_Global != value)
        //        {
        //            biasMin_Global = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public float BiasMax_Global
        //{
        //    get { return biasMax_Global; }
        //    set
        //    {
        //        if (biasMax_Global != value)
        //        {
        //            biasMax_Global = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public CostType CostType
        {
            get { return SessionContext.TrainerParameters.CostType; }
            set
            {
                if (SessionContext.TrainerParameters.CostType != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetCostType((int)value);
                    // OnPropertyChanged();
                }
            }
        }
        public WeightInitType WeightInitType
        {
            get { return SessionContext.NetParameters.WeightInitType; }
            set
            {
                if (SessionContext.NetParameters.WeightInitType != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetWeightInitType((int)value);
                    // OnPropertyChanged();
                }
            }
        }

        public float LearningRate
        {
            get { return SessionContext.TrainerParameters.LearningRate; }
            set
            {
                if (SessionContext.TrainerParameters.LearningRate != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetLearningRate(value);
                    // OnPropertyChanged();
                }
            }
        }
        public float LearningRateChange
        {
            get { return SessionContext.TrainerParameters.LearningRateChange; }
            set
            {
                if (SessionContext.TrainerParameters.LearningRateChange != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetLearningRateChange(value);
                    // OnPropertyChanged();
                }
            }
        }
        public int EpochCount
        {
            get { return SessionContext.TrainerParameters.Epochs; }
            set
            {
                if (SessionContext.TrainerParameters.Epochs != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetEpochs(value);
                    // OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand MoveLeftCommand { get; private set; }
        public ICommand MoveRightCommand { get; private set; }
        public IAsyncCommand UseGlobalParametersCommand { get; private set; }   // redundant


        #region Executes and CanExecutes

        private void Add(object parameter)
        {
            int layerId = (parameter as ILayerParametersVM).LayerParameters.Id;
            SessionContext.Initializer.ParameterBuilder.AddLayerAfter(layerId);
        }
        private void Delete(object parameter)
        {
            int layerId = (parameter as ILayerParametersVM).LayerParameters.Id;
            SessionContext.Initializer.ParameterBuilder.DeleteLayer(layerId);
        }
        private void MoveLeft(object parameter)
        {
            int layerId = (parameter as ILayerParametersVM).LayerParameters.Id;
            SessionContext.Initializer.ParameterBuilder.MoveLayerLeft(layerId);
        }
        private void MoveRight(object parameter)
        {
            int layerId = (parameter as ILayerParametersVM).LayerParameters.Id;
            SessionContext.Initializer.ParameterBuilder.MoveLayerRight(layerId);
        }
        //private async Task UseGlobalParametersAsync(object parameter)
        //{
        //    await Task.Run(() =>
        //    {
        //        AreParametersGlobal_IsCheckboxDisabled = true;

        //        if (AreParametersGlobal)
        //        {
        //            AreParametersGlobal = false;
        //            OverrideLocalParameters();
        //        }
        //        else
        //        {
        //            // 'Invoke' dispatcher to block this thread until the message box is closed.
        //            // (BeginInvoke would only block the ui thread (this thread continued execution).)
        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                if (MessageBox.Show(Application.Current.MainWindow, 
        //                    "Switching to global parameters will override the current (maybe individual) local values.\n" +
        //                    "Do you want to proceed?", "....", MessageBoxButton.YesNo)
        //                == MessageBoxResult.Yes)
        //                {
        //                    AreParametersGlobal = true;
        //                    OverrideLocalParameters();
        //                }
        //            });
        //        }

        //        AreParametersGlobal_IsCheckboxDisabled = false;

        //        void OverrideLocalParameters()
        //        {
        //            foreach (var layerParameters in LayerParametersCollection)
        //            {
        //                layerParameters.BiasMin = biasMin_Global;
        //                layerParameters.BiasMax = biasMax_Global;
        //                layerParameters.WeightMin = weightMin_Global;
        //                layerParameters.WeightMax = weightMax_Global;
        //            }
        //            OnAllPropertiesChanged();
        //        }
        //    });
        //}

        #endregion

        #endregion
    }
}
