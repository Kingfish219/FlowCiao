namespace SmartFlow.Persistence.Providers.Cache
{
    public class SmartFlowCacheRepository
    {
        private readonly SmartFlowHub _smartFlowHub;

        public SmartFlowCacheRepository(SmartFlowHub smartFlowHub)
        {
            _smartFlowHub = smartFlowHub;
        }

        protected SmartFlowHub GetDbConnection()
        {
            return _smartFlowHub;
        }
    }
}
