using NeuralNet_Core;
using NeuralNet_Core.FactoriesAndParameters;
using System;
using System.Collections.Generic;

namespace NeuralNet_UI.ViewModels
{
    public interface ILayerParametersVM : IBaseVM
    {
        ILayerParameters LayerParameters { get; set; }
        int Id { get; }
        int NeuronsPerLayer { get; set; }
        float BiasMin { get; set; }
        float BiasMax { get; set; }
        float WeightMin { get; set; }
        float WeightMax { get; set; }
        ActivationType ActivationType { get; set; }
        IEnumerable<ActivationType> ActivationTypes { get; }
    }

    public class LayerParametersVM : BaseVM, ILayerParametersVM
    {
        #region fields & ctor

        private IEnumerable<ActivationType> activationTypes;

        public LayerParametersVM(ISessionContext sessionContext, ISimpleMediator mediator)//
            : base(sessionContext, mediator) { }

        #endregion

        #region properties

        public ILayerParameters LayerParameters { get; set; }
        public int Id => LayerParameters.Id;
        public int NeuronsPerLayer
        {
            get { return LayerParameters.NeuronsPerLayer; }
            set
            {
                if (LayerParameters.NeuronsPerLayer != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetNeuronsAtLayer(Id, value);
                    // OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return LayerParameters.WeightMin; }
            set
            {
                if (LayerParameters.WeightMin != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetWeightMinAtLayer(Id, value);
                    //OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return LayerParameters.WeightMax; }
            set
            {
                if (LayerParameters.WeightMax != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetWeightMaxAtLayer(Id, value);
                    // OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return LayerParameters.BiasMin; }
            set
            {
                if (LayerParameters.BiasMin != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetBiasMinAtLayer(Id, value);
                    // OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return LayerParameters.BiasMax; }
            set
            {
                if (LayerParameters.BiasMax != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetBiasMaxAtLayer(Id, value);
                    // OnPropertyChanged();
                }
            }
        }
        public ActivationType ActivationType
        {
            get { return LayerParameters.ActivationType; }
            set
            {
                if (LayerParameters.ActivationType != value)
                {
                    SessionContext.Initializer.ParameterBuilder.SetActivationTypeAtLayer(Id, (int)value);
                    // OnPropertyChanged();
                }
            }
        }
        public IEnumerable<ActivationType> ActivationTypes => activationTypes ??
            (activationTypes = Enum.GetValues(typeof(ActivationType)).ToList<ActivationType>());

        #endregion
    }
}