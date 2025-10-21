using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Rentals
{
	public class ExtendModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public RentalDto Item { get; set; } = new();
		public DateTime? OriginalDueDate { get; set; }
		public string? Error { get; set; }

		public ExtendModel(ApiClient api) { _api = api; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			var resp = await _api.GetAsync($"Rental/{id}");
			if (!resp.IsSuccessStatusCode)
			{
				Error = $"Failed to load rental: {(int)resp.StatusCode} {resp.ReasonPhrase}";
				return Page();
			}
			var r = await resp.Content.ReadFromJsonAsync<RentalDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			if (r is null)
				return NotFound();
			Item.Id = r.Id;
			Item.DueDate = r.DueDate;
			OriginalDueDate = r.DueDate;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (id != Item.Id)
			{
				ModelState.AddModelError(string.Empty, "ID mismatch");
				return Page();
			}
			if (!ModelState.IsValid)
				return Page();

			var resp = await _api.PutAsync($"Rental/{id}", Item);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Failed to extend: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}");
				return Page();
			}
			TempData["Success"] = "Rental extended";
			return RedirectToPage("/Rentals/Manage");
		}
	}
}
