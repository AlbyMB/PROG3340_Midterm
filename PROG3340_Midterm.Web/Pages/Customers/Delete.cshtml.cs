using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class DeleteModel : PageModel
	{
		private readonly ApiClient _api;
		[BindProperty]
		public int Id { get; set; }
		public CustomerDto? Item { get; set; }
		public DeleteModel(ApiClient api) { _api = api; }
		public async Task<IActionResult> OnGetAsync(int id)
		{
			Id = id;
			var resp = await _api.GetAsync($"Customer/{id}");
			if (!resp.IsSuccessStatusCode) return NotFound();
			Item = await resp.Content.ReadFromJsonAsync<CustomerDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var resp = await _api.DeleteAsync($"Customer/{Id}");
			if (!resp.IsSuccessStatusCode)
			{
				var details = await resp.Content.ReadAsStringAsync();
				TempData["Success"] = $"Failed to delete: {(int)resp.StatusCode} {resp.ReasonPhrase} {(string.IsNullOrWhiteSpace(details) ? string.Empty : "- " + details)}";
			}
			else
			{
				TempData["Success"] = "Deleted";
			}
			return RedirectToPage("Manage");
		}
	}
}
