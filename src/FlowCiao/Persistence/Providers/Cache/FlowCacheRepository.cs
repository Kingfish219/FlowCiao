namespace FlowCiao.Persistence.Providers.Cache
{
    public class FlowCacheRepository
    {
        protected readonly FlowHub FlowHub;

        public FlowCacheRepository(FlowHub flowHub)
        {
            FlowHub = flowHub;
        }

        protected FlowHub GetDbConnection()
        {
            return FlowHub;
        }
    }
}
