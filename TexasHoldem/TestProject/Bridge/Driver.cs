using TestProject.Bridge;

namespace TestProject
{
    class Driver
    {
        public static IBridge getBridge()
        {
            ProxyBridge bridge = new ProxyBridge();
            bridge.setRealBridge(new RealBridge());
            return bridge;
        }
    }
}
