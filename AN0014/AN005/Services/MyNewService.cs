namespace AN005.Services
{
    public class MyNewService : IMyService
    {
        public string Hi(string name)
        {
            return $"Hello {name}";
        }
    }
}
