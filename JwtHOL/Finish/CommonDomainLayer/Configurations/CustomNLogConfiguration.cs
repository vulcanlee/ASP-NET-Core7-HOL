namespace CommonDomainLayer.Configurations
{
    public class CustomNLogConfiguration
    {
        public string LogRootPath { get; set; } = string.Empty;
        public string AllLogMessagesFilename { get; set; } = string.Empty;
        public string AllWebDetailsLogMessagesFilename { get; set; } = string.Empty;
    }
}
