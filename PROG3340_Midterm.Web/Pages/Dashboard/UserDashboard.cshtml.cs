using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROG3340_Midterm.Web.Pages.Dashboard
{
	public class UserDashboardModel : PageModel
	{
		public string Username { get; set; } = "";
		public void OnGet()
		{
			Username = HttpContext.Session.GetString("Username") ?? "User";
		}
	}
}
