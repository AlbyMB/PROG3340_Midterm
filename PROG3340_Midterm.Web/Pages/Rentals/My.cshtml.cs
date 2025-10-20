using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Rentals
{
	public class MyModel : PageModel
	{
		private readonly ApiClient _api;
		public List<RentalDto> Items { get; set; } = new();
		public string? Error { get; set; }

		public MyModel(ApiClient api)
		{
			_api = api;
		}

		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Rental");
			if (!resp.IsSuccessStatusCode)
			{
				Error = "Failed to load rentals.";
				return;
			}
			var data = await resp.Content.ReadFromJsonAsync<List<RentalDto>>();
			Items = data ?? new();
		}

		public async Task<IActionResult> OnPostReturnAsync(int id)
		{
			var rental = new RentalDto { Id = id, CustomerId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0") };
			var resp = await _api.PostAsync("Rental/return", rental);
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to return: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Returned";
			}
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostExtendAsync(int id)
		{
			var rental = Items.FirstOrDefault(r => r.Id == id) ?? new RentalDto { Id = id };
			rental.DueDate = DateTime.Now.AddDays(7);
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
	}
}
