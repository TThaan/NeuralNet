using NeuralNet_UI.FactoriesAndStewards;
using NeuralNet_UI.ViewModels;
using NeuralNet_UI.Views;
using Autofac;
using Autofac.Features.AttributeFilters;
using NeuralNet_Core;
using NeuralNet_Core.FactoriesAndParameters;
using IContainer = Autofac.IContainer;

namespace NeuralNet_UI
{
    public class DIManager
    {
        #region fields

        IContainer container;

        #endregion

        #region properties

        public IContainer Container => container ?? (container = GetContainer());

        #region Registrations

        private IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            #region Windows/Views

            builder.Register(x => new MainWindow()
            {
                DataContext = x.Resolve<IMainWindowVM>()
            })
                .SingleInstance();

            builder.Register(x => new SampleImportWindow()
            {
                DataContext = x.Resolve<ISampleImportWindowVM>()
            })
                .SingleInstance();

            #endregion

            #region View Models

            builder.RegisterType<MainWindowVM>()
                .SingleInstance()
                .As<IMainWindowVM>();

            builder.RegisterType<NetParametersVM>()
                .SingleInstance()
                .As<INetParametersVM>();

            builder.RegisterType<LayerParametersVM>()
                .As<ILayerParametersVM>();

            builder.RegisterType<NetVM>()
                .SingleInstance()
                .As<INetVM>();

            builder.RegisterType<PredictVM>()
                .SingleInstance()
                .As<IPredictVM>();

            builder.RegisterType<StartStopVM>()
                .SingleInstance()
                .WithAttributeFiltering()
                .As<IStartStopVM>();

            builder.RegisterType<SampleImportWindowVM>()
                .SingleInstance()
                .As<ISampleImportWindowVM>();

            builder.RegisterType<StatusVM>()
                .As<IStatusVM>()
                .SingleInstance();

            #endregion

            #region Data/Model classes

            builder.RegisterType<Initializer>()
                .SingleInstance();
            //builder.RegisterType<NetParameters>()
            //    .As<INetParameters>()
            //    .SingleInstance();
            builder.RegisterType<LayerParameters>()
                .As<ILayerParameters>();
            //builder.RegisterType<TrainerParameters>()
            //    .SingleInstance()
            //    .As<ITrainerParameters>();

            //builder.Register(x => Initializer.GetRawNet())
            //    .As<INet>()
            //    .SingleInstance();
            //builder.Register(x => Initializer.GetRawTrainer()).
            //    OnActivated(x =>
            //    {
            //        // Reconsider:
            //        x.Instance.PropertyChanged += x.Context.Resolve<ILayerParametersVM>().Any_PropertyChanged;
            //        x.Instance.PropertyChanged += x.Context.Resolve<IStartStopVM>().Any_PropertyChanged;
            //        x.Instance.PropertyChanged += x.Context.Resolve<IStatusVM>().Any_PropertyChanged;
            //        x.Instance.TrainerStatusChanged += x.Context.Resolve<IStartStopVM>().Trainer_TrainerStatusChanged;
            //    })
            //    .As<ITrainer>()
            //    .SingleInstance();

            #endregion

            #region Factories and Stewards

            builder.RegisterType<DelegateFactory>()
                .As<IDelegateFactory>();

            // Implement in DelegateFactory?:
            builder.RegisterType<LayerParametersVMFactory>()
                .As<ILayerParametersVMFactory>();
            builder.RegisterType<LayerParametersFactory>()
                .As<ILayerParametersFactory>();
            builder.RegisterType<AllWeightsFactory>()
                .As<IAllWeightsFactory>();

            #endregion

            #region Others

            builder.RegisterType<SessionContext>()
                .As<ISessionContext>()
                .SingleInstance();

            builder.RegisterType<SimpleMediator>()
                .As<ISimpleMediator>()
                .SingleInstance();

            builder.RegisterType<WeightPlusNeuron>()
                .As<IWeightPlusNeuron>();

            #endregion

            return builder.Build();
        }

        #endregion

        #endregion
    }
}
