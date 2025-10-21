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
		public List<RentalListItem> Items { get; set; } = new();
		public string? Error { get; set; }

		public MyModel(ApiClient api)
		{
			_api = api;
		}

		public async Task OnGet()
		{
			var rentalsResp = await _api.GetAsync("Rental");
			if (!rentalsResp.IsSuccessStatusCode)
			{
				Error = "Failed to load rentals.";
				return;
			}
			var rentals = await rentalsResp.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new();

			// Fetch customers and equipment for name lookup (admin will see all)
			var customersTask = _api.GetAsync("Customer");
			var equipmentTask = _api.GetAsync("Equipment");
			await Task.WhenAll(customersTask, equipmentTask);
			var customers = new List<CustomerDto>();
			var equipment = new List<EquipmentDto>();
			if (customersTask.Result.IsSuccessStatusCode)
				customers = await customersTask.Result.Content.ReadFromJsonAsync<List<CustomerDto>>() ?? new();
			if (equipmentTask.Result.IsSuccessStatusCode)
				equipment = await equipmentTask.Result.Content.ReadFromJsonAsync<List<EquipmentDto>>() ?? new();

			var custLookup = customers.ToDictionary(c => c.Id, c => c.Name);
			var equipLookup = equipment.ToDictionary(e => e.Id, e => e.Name);

			Items = rentals.Select(r => new RentalListItem
			{
				Id = r.Id,
				EquipmentId = r.EquipmentId,
				EquipmentName = equipLookup.TryGetValue(r.EquipmentId, out var en) ? en : $"#{r.EquipmentId}",
				CustomerId = r.CustomerId,
				CustomerName = custLookup.TryGetValue(r.CustomerId, out var cn) ? cn : $"#{r.CustomerId}",
				IssuedAt = r.IssuedAt,
				ReturnedAt = r.ReturnedAt,
				DueDate = r.DueDate,
				Status = r.Status
			}).ToList();
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
			var rental = Items.FirstOrDefault(r => r.Id == id) ?? new RentalListItem { Id = id };
			var dto = new RentalDto { Id = rental.Id, DueDate = DateTime.Now.AddDays(7) };
			var resp = await _api.PutAsync($"Rental/{id}", dto);
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
