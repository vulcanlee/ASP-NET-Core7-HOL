using AN005.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AN005.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMyService myService;

        public string Message { get; set; } = string.Empty;

        public IndexModel(ILogger<IndexModel> logger,
            IMyService myService)
        {
            _logger = logger;
            this.myService = myService;
        }

        public void OnGet()
        {
            var hi = myService.Hi("Vulcan Lee");
            _logger.LogInformation($"In OnGet, Call Hi Method : {hi}");
            Message= hi;
        }
    }
}