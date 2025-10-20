using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class EditProfileModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public CustomerDto Item { get; set; } = new();

		public EditProfileModel(ApiClient api) { _api = api; }

		public async Task<IActionResult> OnGetAsync()
		{
			var id = HttpContext.Session.GetString("UserId");
			if (string.IsNullOrEmpty(id)) return RedirectToPage("/Account/Login");
			var resp = await _api.GetAsync($"Customer/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			var data = await resp.Content.ReadFromJsonAsync<CustomerDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			Item = data ?? new();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var id = HttpContext.Session.GetString("UserId");
			if (string.IsNullOrEmpty(id)) return RedirectToPage("/Account/Login");
			Item.Id = int.Parse(id);
			var resp = await _api.PutAsync($"Customer/{id}", Item);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to save: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Saved";
			}
			return RedirectToPage("/Dashboard/UserDashboard");
		}
	}
}
