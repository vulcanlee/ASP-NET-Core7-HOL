namespace AN006.Services
{
    public class MyService
    {
        private readonly ILogger<MyService> logger;
        private readonly YourService yourService;

        public MyService(ILogger<MyService> logger,
            YourService yourService)
        {
            this.logger = logger;
            this.yourService = yourService;
        }
        public void DoSomething()
        {
            logger.LogInformation("Execute DoSomething of MyService");
            yourService.DoSomething();
        }
    }
}
