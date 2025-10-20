using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Rentals
{
	public class ManageModel : PageModel
	{
		private readonly ApiClient _api;
		public List<RentalDto> Items { get; set; } = new();

		public ManageModel(ApiClient api) { _api = api; }

		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Rental");
			if (resp.IsSuccessStatusCode)
				Items = await resp.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new();
		}

		public async Task<IActionResult> OnPostExtendAsync(int id)
		{
			var rental = new RentalDto { Id = id, DueDate = DateTime.Now.AddDays(7) };
			var resp = await _api.PutAsync($"Rental/{id}", rental);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to extend: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Extended";
			}
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostCancelAsync(int id)
		{
			var resp = await _api.DeleteAsync($"Rental/{id}");
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to cancel: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Cancelled";
			}
			return RedirectToPage();
		}
	}
}
