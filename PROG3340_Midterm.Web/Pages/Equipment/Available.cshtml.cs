using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Equipment
{
	public class AvailableModel : PageModel
	{
		private readonly ApiClient _api;
		public List<EquipmentDto> Items { get; set; } = new();
		public string? Error { get; set; }
		public AvailableModel(ApiClient api) { _api = api; }
		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Equipment/available");
			if (!resp.IsSuccessStatusCode)
			{
				Error = $"Failed to load available: {(int)resp.StatusCode} {resp.ReasonPhrase}";
				return;
			}
			Items = await resp.Content.ReadFromJsonAsync<List<EquipmentDto>>() ?? new();
		}
	}
}
