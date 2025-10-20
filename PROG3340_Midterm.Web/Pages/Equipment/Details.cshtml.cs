using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Equipment
{
	public class DetailsModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public RentalDto Issue { get; set; } = new();
		public EquipmentDto? Item { get; set; }
		public List<RentalDto> RentalHistory { get; set; } = new();
		public bool CanIssue => (HttpContext.Session.GetString("Role") ?? "") == "User" && (Item?.IsAvailable ?? false);
		public string? Error { get; set; }

		public DetailsModel(ApiClient api)
		{
			_api = api;
		}

		public async Task<IActionResult> OnGet(int id)
		{
			var resp = await _api.GetAsync($"Equipment/{id}");
			if (!resp.IsSuccessStatusCode)
			{
				Error = "Not found";
				return Page();
			}

			var details = await resp.Content.ReadFromJsonAsync<EquipmentDetailsDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			Item = details?.Equipment;
			RentalHistory = details?.RentalHistory ?? new();

			Issue.EquipmentId = id;
			var userIdStr = HttpContext.Session.GetString("UserId");
			if (int.TryParse(userIdStr, out var uid)) Issue.CustomerId = uid;
			Issue.IssuedAt = DateTime.Now;
			Issue.DueDate = DateTime.Now.AddDays(7);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				Error = "Invalid request";
				return Page();
			}

			var resp = await _api.PostAsync("Rental/issue", Issue);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				Error = $"Failed to issue equipment: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
				return await OnGet(id);
			}
			TempData["Success"] = "Issued successfully";
			return RedirectToPage("/Rentals/My");
		}
	}
}
