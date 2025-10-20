using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class EditModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public CustomerDto Item { get; set; } = new();
		public EditModel(ApiClient api) { _api = api; }
		public async Task<IActionResult> OnGetAsync(int id)
		{
			var resp = await _api.GetAsync($"Customer/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			Item = await resp.Content.ReadFromJsonAsync<CustomerDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var resp = await _api.PutAsync($"Customer/{Item.Id}", Item);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Failed to save: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}");
				return Page();
			}
			TempData["Success"] = "Saved";
			return RedirectToPage("Manage");
		}
	}
}
