using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITCC.Logging.Core;
using ITCC.Logging.Core.Loggers;

namespace GraphLayoutSample.Engine.Tests
{
    [TestClass]
    public class Initializer
    {
        [AssemblyInitialize]
        public static void MyTestInitialize(TestContext testContext)
        {
            Logger.Level = LogLevel.Trace;
            Logger.RegisterReceiver(new DebugLogger());
        }
    }
}
