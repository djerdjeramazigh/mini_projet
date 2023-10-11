using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFGeneratorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public string[] Columns { get; set; }
        public string[][] Rows { get; set; }
        public void OnGet()
        {
            // Simulate SQL query result
            Columns = new[] { "ID", "Name", "Age" };
            Rows = new[]
            {
            new[] { "1", "John Doe", "25" },
            new[] { "2", "Jane Doe", "30" },
            new[] { "3", "Bob Smith", "22" }
        };
        }
    }
}