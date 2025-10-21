using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG3340_Midterm.Web.Services;
using System.Net.Http.Json;
using PROG3340_Midterm.Web.Models;

namespace PROG3340_Midterm.Web.Pages.Dashboard
{
    public class AdminDashboardModel : PageModel
    {
        private readonly ApiClient _api;
        public string Username { get; set; } = "";
        public int RentedCount { get; set; }
        public int OverdueCount { get; set; }

        public AdminDashboardModel(ApiClient api)
        {
            _api = api;
        }

        public async Task OnGet()
        {
            Username = HttpContext.Session.GetString("Username") ?? "Admin";

            try
            {
                var rentedResp = await _api.GetAsync("Equipment/rented");
                if (rentedResp.IsSuccessStatusCode)
                {
                    var rented = await rentedResp.Content.ReadFromJsonAsync<RentedEquipmentResponse>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    RentedCount = rented?.Count ?? 0;
                }

                var overdueResp = await _api.GetAsync("Rental/overdue");
                if (overdueResp.IsSuccessStatusCode)
                {
                    var overdue = await overdueResp.Content.ReadFromJsonAsync<List<RentalDto>>();
                    OverdueCount = overdue?.Count ?? 0;
                }
            }
            catch { }
        }
    }
}
