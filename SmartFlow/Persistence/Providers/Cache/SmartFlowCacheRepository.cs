namespace SmartFlow.Persistence.Providers.Cache
{
    public class SmartFlowCacheRepository
    {
        protected readonly SmartFlowHub SmartFlowHub;

        public SmartFlowCacheRepository(SmartFlowHub smartFlowHub)
        {
            SmartFlowHub = smartFlowHub;
        }

        protected SmartFlowHub GetDbConnection()
        {
            return SmartFlowHub;
        }
    }
}
