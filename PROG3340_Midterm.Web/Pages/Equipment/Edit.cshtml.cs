using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Equipment
{
	public class EditModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public EquipmentDto Item { get; set; } = new();

		public EditModel(ApiClient api) { _api = api; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			var resp = await _api.GetAsync($"Equipment/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			var details = await resp.Content.ReadFromJsonAsync<EquipmentDetailsDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			Item = details?.Equipment ?? new();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var resp = await _api.PutAsync($"Equipment/{Item.Id}", Item);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Failed to update: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}");
				return Page();
			}
			TempData["Success"] = "Updated";
			return RedirectToPage("Index");
		}
	}
}
