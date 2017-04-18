namespace TestProject
{
    class Driver
    {
        public static Bridge getBridge()
        {
            ProxyBridge bridge = new ProxyBridge();
            bridge.setRealBridge(null);
            return bridge;
        }
    }
}
