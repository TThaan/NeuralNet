using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Threading.Tasks;

namespace NeuralNet_UI
{

    public delegate Task OkBtnEventHandler(INetParameters netParameters, bool isTurnBased);
}