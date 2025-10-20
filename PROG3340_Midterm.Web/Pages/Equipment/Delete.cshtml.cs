using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Equipment
{
	public class DeleteModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public int Id { get; set; }
		public EquipmentDto? Item { get; set; }

		public DeleteModel(ApiClient api) { _api = api; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Id = id;
			var resp = await _api.GetAsync($"Equipment/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			var details = await resp.Content.ReadFromJsonAsync<EquipmentDetailsDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			Item = details?.Equipment;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var resp = await _api.DeleteAsync($"Equipment/{Id}");
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to delete: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Deleted";
			}
			return RedirectToPage("Index");
		}
	}
}
