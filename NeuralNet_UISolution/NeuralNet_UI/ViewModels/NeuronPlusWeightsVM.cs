using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNet_UI.ViewModels
{
    public interface INeuronPlusWeightsVM
    {
        int LayersCount { get; set; }
        int MaxLayerLength { get; set; }
        int PrecedingLayersLength { get; set; }
        int LayerId { get; set; }
        int NeuronId { get; set; }

        float[] IncomingWeights { get; set; }
        //int? CurrentlyHandledWeight { get; set; }
    }

    public class NeuronPlusWeightsVM : BaseVM, INeuronPlusWeightsVM
    {
        #region fields & ctor

        public NeuronPlusWeightsVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            // If Net is created only in the root (once) and initialized afterwards then inject it!

            //RegisterEvents();
            //RegisterMediatorHandlers();
            //DefineCommands();
        }

        #region helpers

        private void RegisterEvents()
        {
            // SessionContext.Initializer.PropertyChanged += Initializer_PropertyChanged;
            // SessionContext.Trainer.PropertyChanged += Trainer_PropertyChanged;
        }
        private void RegisterMediatorHandlers()
        {
            // _mediator.Register(MediatorToken.StartStopVM_OnNetInitializedOrChanged.ToString(), OnNetInitializedOrChanged);
        }
        private void DefineCommands()
        {

        }

        #endregion

        #endregion

        #region mediator handlers

        private void OnNetInitializedOrChanged(object obj)
        {

        }

        #endregion

        #region INeuronPlusWeightsVM

        public int LayersCount { get; set; }
        public int MaxLayerLength { get; set; }
        public int PrecedingLayersLength { get; set; }
        public int LayerId { get; set; }
        public int NeuronId { get; set; }

        public float[] IncomingWeights { get; set; }
        //public int? CurrentlyHandledWeight { get; set; }

        #endregion
    }
}
