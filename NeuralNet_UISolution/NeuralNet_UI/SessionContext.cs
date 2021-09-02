using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace NeuralNet_UI
{
    public interface ISessionContext
    {
        Initializer Initializer { get; }
        INet Net { get; set; }
        ITrainer Trainer { get; }
        ISampleSet SampleSet { get; }
        INetParameters NetParameters { get; }
        ITrainerParameters TrainerParameters { get; }
        ObservableCollection<ILayerParameters> LayerParametersCollection { get; }
        bool IsLogged { get; set; }
    }

    /// <summary>
    /// Contains (model) data shared among different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region fields & ctor

        private INet net;

        public SessionContext(Initializer initializer)
        {
            Initializer = initializer;
            Net = initializer.Net;
        }

        #endregion

        #region properties

        public Initializer Initializer { get; }
        public INet Net { get => net; set => net = value; }
        public ISampleSet SampleSet => Initializer.SampleSet;
        public INetParameters NetParameters => Initializer.ParameterBuilder.NetParameters;
        public ITrainerParameters TrainerParameters => Initializer.ParameterBuilder.TrainerParameters;
        public ITrainer Trainer => Initializer.Trainer;

        public ObservableCollection<ILayerParameters> LayerParametersCollection => Initializer.ParameterBuilder.NetParameters.LayerParametersCollection;

        public bool IsLogged { get; set; }

        #endregion
    }
}
