using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Rentals
{
	public class RequestModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public RentalDto Issue { get; set; } = new();
		public List<EquipmentDto> Available { get; set; } = new();
		public string? Error { get; set; }
		public string? Success { get; set; }

		public RequestModel(ApiClient api) { _api = api; }

		public async Task OnGet(int? equipmentId)
		{
			await LoadAvailableAsync();
			var userIdStr = HttpContext.Session.GetString("UserId");
			if (int.TryParse(userIdStr, out var uid)) Issue.CustomerId = uid;
			Issue.IssuedAt = DateTime.Now;
			Issue.DueDate = DateTime.Now.AddDays(7);
			if (equipmentId.HasValue) Issue.EquipmentId = equipmentId.Value;
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				await LoadAvailableAsync();
				Error = "Invalid form";
				return Page();
			}
			var resp = await _api.PostAsync("Rental/issue", Issue);
			if (!resp.IsSuccessStatusCode)
			{
				await LoadAvailableAsync();
				var details = await resp.Content.ReadAsStringAsync();
				Error = $"Failed to request: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
				return Page();
			}
			TempData["Success"] = "Request submitted";
			return RedirectToPage("/Rentals/My");
		}

		private async Task LoadAvailableAsync()
		{
			var resp = await _api.GetAsync("Equipment/available");
			if (resp.IsSuccessStatusCode)
			{
				Available = await resp.Content.ReadFromJsonAsync<List<EquipmentDto>>() ?? new();
			}
			else
			{
				var details = await resp.Content.ReadAsStringAsync();
				Error = $"Failed to load available equipment: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
		}
	}
}
