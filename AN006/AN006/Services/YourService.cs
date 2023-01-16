namespace AN006.Services
{
    public class YourService
    {
        private readonly ILogger<YourService> logger;

        public YourService(ILogger<YourService> logger)
        {
            this.logger = logger;
        }
        public void DoSomething()
        {
            logger.LogInformation("Execute DoSomething of YourService");
        }
    }
}
