namespace FlowCiao.Persistence.Providers.Cache
{
    public class FlowCiaoCacheRepository
    {
        protected readonly FlowHub FlowHub;

        public FlowCiaoCacheRepository(FlowHub flowHub)
        {
            FlowHub = flowHub;
        }

        protected FlowHub GetDbConnection()
        {
            return FlowHub;
        }
    }
}
