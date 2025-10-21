using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		public IEnumerable<SelectListItem> EquipmentOptions { get; set; } = Enumerable.Empty<SelectListItem>();
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
			Issue.Status = "Active";
			if (equipmentId.HasValue) Issue.EquipmentId = equipmentId.Value;
			BuildOptions();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			// Ensure critical fields are set from session regardless of form state
			var userIdStr = HttpContext.Session.GetString("UserId");
			if (int.TryParse(userIdStr, out var uid)) Issue.CustomerId = uid;
			Issue.IssuedAt = Issue.IssuedAt == default ? DateTime.Now : Issue.IssuedAt;
			Issue.Status = string.IsNullOrWhiteSpace(Issue.Status) ? "Active" : Issue.Status;

			await LoadAvailableAsync();
			BuildOptions();

			if (!ModelState.IsValid)
			{
				Error = "Invalid form";
				return Page();
			}
			var resp = await _api.PostAsync("Rental/issue", Issue);
			if (!resp.IsSuccessStatusCode)
			{
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

		private void BuildOptions()
		{
			EquipmentOptions = Available.Select(e => new SelectListItem
			{
				Value = e.Id.ToString(),
				Text = $"{e.Name} ({e.RentalPrice:C})",
				Selected = e.Id == Issue.EquipmentId
			});
		}
	}
}
