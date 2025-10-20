using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Models;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Customers
{
	public class ManageModel : PageModel
	{
		private readonly ApiClient _api;
		public List<CustomerDto> Items { get; set; } = new();
		public ManageModel(ApiClient api) { _api = api; }
		public async Task OnGet()
		{
			var resp = await _api.GetAsync("Customer");
			if (resp.IsSuccessStatusCode)
				Items = await resp.Content.ReadFromJsonAsync<List<CustomerDto>>() ?? new();
		}
	}
}
