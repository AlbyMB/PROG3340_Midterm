using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class DetailsModel : PageModel
	{
		private readonly ApiClient _api;
		public CustomerDto? Item { get; set; }
		public List<RentalDto> Rentals { get; set; } = new();
		public string? Error { get; set; }
		public DetailsModel(ApiClient api) { _api = api; }
		public async Task<IActionResult> OnGetAsync(int id)
		{
			var resp = await _api.GetAsync($"Customer/{id}");
			if (!resp.IsSuccessStatusCode)
			{
				Error = $"Failed to load: {(int)resp.StatusCode} {resp.ReasonPhrase}";
				return Page();
			}
			Item = await resp.Content.ReadFromJsonAsync<CustomerDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			var rResp = await _api.GetAsync($"Customer/{id}/rentals");
			if (rResp.IsSuccessStatusCode)
				Rentals = await rResp.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new();
			return Page();
		}
	}
}
