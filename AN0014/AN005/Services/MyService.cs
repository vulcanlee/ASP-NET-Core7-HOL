namespace AN005.Services
{
    public class MyService : IMyService
    {
        public string Hi(string name)
        {
            return $"Hi {name}";
        }
    }
}
