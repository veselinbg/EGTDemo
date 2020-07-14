using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;

namespace EGT.ClientApp.Pages
{
    public class IndexModel : PageModel
    {
        IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void OnGet()
        {
            ViewData["MessageHubUrl"] = _configuration["MessageHubUrl"];
        }
    }
}
