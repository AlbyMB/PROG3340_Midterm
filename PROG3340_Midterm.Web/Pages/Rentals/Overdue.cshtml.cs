using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Rentals
{
	public class OverdueModel : PageModel
	{
		private readonly ApiClient _api;
		public List<RentalDto> Items { get; set; } = new();
		public OverdueModel(ApiClient api) { _api = api; }
		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Rental/overdue");
			if (resp.IsSuccessStatusCode)
			{
				Items = await resp.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new();
			}
			else
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to load overdue: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
		}
	}
}
