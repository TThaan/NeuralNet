using NeuralNet_UI.ViewModels;
using NeuralNet_UI.Views;
using Autofac;
using System;
using System.ComponentModel;

namespace NeuralNet_UI.FactoriesAndStewards
{
    public interface IDelegateFactory
    {
        //Action HideSampleImportWindow();
        Func<bool?> ShowSampleImportWindow();
        PropertyChangedEventHandler GetPropertyChangedHandler_StatusVM();
    }

    public class DelegateFactory : IDelegateFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public DelegateFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region IDelegateFactory

        public Func<bool?> ShowSampleImportWindow()
        {
            return () => _context.Resolve<SampleImportWindow>().ShowDialog();
        }
        public PropertyChangedEventHandler GetPropertyChangedHandler_StatusVM()
        {
            return _context.Resolve<IStatusVM>().Trainer_PropertyChanged;
        }
        //public Action HideSampleImportWindow()
        //{
        //    return () => _context.Resolve<SampleImportWindow>().Hide();
        //}

        #endregion
    }
}
