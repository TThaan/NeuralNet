using NeuralNet_UI.Commands.Async;
using NeuralNetBuilder;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static NeuralNet_UI.Helpers;

namespace NeuralNet_UI.ViewModels
{
    public interface IPredictVM
    {
        DataType[] Input { get; set; }
        DataType[] Output { get; }
        //string Target { get; }
        //string Prediction { get; }
    }   

    public class PredictVM : BaseVM, IPredictVM
    {
        #region fields & ctor

        private DataType[] input;
        private DataType[] output;

        public PredictVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            RegisterEvents();
            RegisterMediatorHandlers();
            DefineCommands();
        }

        #region helpers

        private void RegisterEvents()
        {
            
        }
        private void RegisterMediatorHandlers()
        {
            _mediator.Register(MediatorToken.PredictVM_OnInputOrOutputChanged.ToString(), PredictVM_OnInputOrOutputChanged);
            _mediator.Register(MediatorToken.PredictVM_CanExecutesChanged.ToString(), PredictVM_CanExecutesChanged);
        }
        private void DefineCommands()
        {
            PredictCommand = new AsyncRelayCommand_Raisable(PredictAsync, PredictAsync_CanExecute);
        }

        #endregion

        #endregion

        #region mediator handlers

        private void PredictVM_OnInputOrOutputChanged(object obj)
        {
            if (!(obj is INet) && !(obj is ILearningNet))
                throw new ArgumentException(
                        $"{GetType().Name}.{nameof(PredictVM_OnInputOrOutputChanged)}:\n" +
                        $"Parameter must be of type {nameof(INet)} or {nameof(ILearningNet)}");

            // Set current input value.

            dynamic dyn = obj;

            try
            {
                Input = (dyn.Layers as ILayer[]).First().Input
                    .Select(x => new DataType { Content = x })
                    .ToArray();
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); return; }

            // Set current output value.

            try
            {
                Output = (dyn.Layers as ILayer[]).Last().Output
                    .Select(x => new DataType { Content = x })
                    .ToArray();
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); return; }
        }

        private void PredictVM_CanExecutesChanged(object obj)
        {
            PredictCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region IPredict

        public DataType[] Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                OnPropertyChanged();
            }
        }
        public DataType[] Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
                OnPropertyChanged();
            }
        }


        //public string Target 
        //{
        //    get
        //    {
        //        if (SessionContext.Trainer.IsInitialized)
        //            return SessionContext.Trainer.SampleSet.ArrangedTrainSet[SessionContext.Trainer.CurrentSample].Label;

        //        return "None";
        //    }
        //}
        //public string Prediction
        //{
        //    get
        //    {
        //        if (SessionContext.Trainer.IsInitialized)
        //            return SessionContext.Trainer.SampleSet.ArrangedTrainSet[SessionContext.Trainer.CurrentSample].Label;

        //        return "None";
        //    }
        //}

        #endregion

        #region Commands

        public IAsyncRaisableCommand PredictCommand { get; private set; }

        #region Executes and CanExecutes

        private async Task PredictAsync(object parameter)
        {
            try
            {
                var lb = parameter as ListBox;
                int inputLength = SessionContext.Net.Layers.First().Input.Length;

                if (lb == null || lb.Items.Count != inputLength)
                    throw new ArgumentException($"An input of {inputLength} values is needed.");

                DataType[] inputValues = new DataType[inputLength];
                lb.Items.CopyTo(inputValues, 0);
                float[] input = inputValues.Select(x => x.Content).ToArray();

                await SessionContext.Net.FeedForwardAsync(input);
            }
            catch (Exception e) { MessageBox.Show(GetFormattedExceptionMessage(e)); }
            finally
            {
                PredictVM_OnInputOrOutputChanged(SessionContext.Net);
            }
        }
        private bool PredictAsync_CanExecute(object parameter)
        {
            return SessionContext.Net.IsInitialized &&
                SessionContext.Trainer.Status != TrainerStatus.Running && 
                SessionContext.Trainer.Status != TrainerStatus.Paused;
        }

        #region helpers



        #endregion

        #endregion

        #endregion
    }
}
