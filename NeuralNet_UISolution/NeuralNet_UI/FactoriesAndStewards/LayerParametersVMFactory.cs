using NeuralNet_UI.ViewModels;
using Autofac;
using NeuralNetBuilder.FactoriesAndParameters;

namespace NeuralNet_UI.FactoriesAndStewards
{
    public interface ILayerParametersVMFactory
    {
        ILayerParametersVM CreateLayerParametersVM(ILayerParameters layerParameters);
    }

    public class LayerParametersVMFactory : ILayerParametersVMFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public LayerParametersVMFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region ILayerParametersVMFactory

        public ILayerParametersVM CreateLayerParametersVM(ILayerParameters layerParameters)
        {
            // Consider scope!
            var result = _context.Resolve<ILayerParametersVM>();
            result.LayerParameters = layerParameters;
            layerParameters.PropertyChanged += result.Initializer_PropertyChanged;

            return result;
        }

        #endregion
    }
}
