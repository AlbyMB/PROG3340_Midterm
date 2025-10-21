using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class EditProfileModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public string Name { get; set; } = string.Empty;
		[BindProperty]
		public string? Email { get; set; }
		[BindProperty]
		public string? Password { get; set; }
		public string? Error { get; set; }

		public EditProfileModel(ApiClient api) { _api = api; }

		public async Task<IActionResult> OnGetAsync()
		{
			var id = HttpContext.Session.GetString("UserId");
			if (string.IsNullOrEmpty(id)) return RedirectToPage("/Account/Login");
			var resp = await _api.GetAsync($"Customer/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			var dto = await resp.Content.ReadFromJsonAsync<dynamic>();
			Name = dto?.GetProperty("name").GetString() ?? string.Empty;
			Email = dto?.GetProperty("email").GetString();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var id = HttpContext.Session.GetString("UserId");
			if (string.IsNullOrEmpty(id)) return RedirectToPage("/Account/Login");
			var payload = new { Name, Email, Password };
			var resp = await _api.PatchAsync($"Customer/{id}/profile", payload);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				Error = $"Failed to save: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
				return Page();
			}
			TempData["Success"] = "Profile saved";
			return RedirectToPage("/Dashboard/UserDashboard");
		}
	}
}
