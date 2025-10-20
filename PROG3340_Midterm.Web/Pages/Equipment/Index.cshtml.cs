using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Equipment
{
	public class IndexModel : PageModel
	{
		private readonly ApiClient _api;
		public List<EquipmentDto> Items { get; set; } = new();
		public bool IsAdmin => (HttpContext.Session.GetString("Role") ?? "") == "Admin";
		public string? Error { get; set; }
		public string? Success { get; set; }

		public IndexModel(ApiClient api)
		{
			_api = api;
		}

		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Equipment");
			if (!resp.IsSuccessStatusCode)
			{
				Error = "Failed to load equipment.";
				return;
			}
			var data = await resp.Content.ReadFromJsonAsync<List<EquipmentDto>>();
			Items = data ?? new();
		}
	}
}
