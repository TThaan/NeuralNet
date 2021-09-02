using NeuralNet_UI.FactoriesAndStewards;
using Autofac;
using NeuralNetBuilder;
using System;
using System.Threading.Tasks;

namespace NeuralNet_UI.SampleData
{
    public class MockData
    {
        static MockData()
        {
            var container = new DIManager().Container;

            var mockSessionContext = Task.Run(async () =>
            {
                var mockInit = new Initializer
                {
                    Notification = "Mocked Initializer Notification"
                };

                var result = await new SessionContext(mockInit).SetDefaultValuesAsync(true, true);
                return result;
            })
                .Result;

            // throw new ArgumentException($"SampleSet: {MockSessionContext.Initializer.PathBuilder.SampleSet}");

            MockContainer = container;
            MockSessionContext = mockSessionContext;
            MockAllWeightsFactory = container.Resolve<IAllWeightsFactory>();
            MockMediator = container.Resolve<ISimpleMediator>();

            // throw new ArgumentException($"Exception in: {typeof(MockData).Name}");
        }

        public static IContainer MockContainer { get; set; }
        public static ISessionContext MockSessionContext { get; set; }
        public static IAllWeightsFactory MockAllWeightsFactory { get; set; }
        public static ISimpleMediator MockMediator { get; set; }
    }
}
