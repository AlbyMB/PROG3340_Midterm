using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class CreateModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public CustomerDto Item { get; set; } = new();
		public CreateModel(ApiClient api) { _api = api; }
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid) return Page();
			var resp = await _api.PostAsync("Customer", Item);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Failed to create: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}");
				return Page();
			}
			TempData["Success"] = "Created";
			return RedirectToPage("Manage");
		}
	}
}
