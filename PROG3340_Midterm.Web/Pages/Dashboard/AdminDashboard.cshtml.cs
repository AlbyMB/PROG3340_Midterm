using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROG3340_Midterm.Web.Pages.Dashboard
{
    public class AdminDashboardModel : PageModel
    {
		public string Username { get; set; } = "";

		public void OnGet()
		{
			Username = HttpContext.Session.GetString("Username") ?? "Admin";
		}
	}
}
