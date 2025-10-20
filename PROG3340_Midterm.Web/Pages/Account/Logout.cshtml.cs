using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROG3340_Midterm.Web.Pages.Account
{
	public class LogoutModel : PageModel
	{
		public IActionResult OnGet()
		{
			HttpContext.Session.Clear();
			return RedirectToPage("/Account/Login");
		}
	}
}
