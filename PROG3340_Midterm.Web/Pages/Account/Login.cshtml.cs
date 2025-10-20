using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace PROG3340_Midterm.Web.Pages.Account
{
	public class LoginModel : PageModel
	{
		private readonly IHttpClientFactory _factory;
		public string? ErrorMessage { get; set; }

		[BindProperty] public string Username { get; set; } = "";
		[BindProperty] public string Password { get; set; } = "";

		public LoginModel(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var client = _factory.CreateClient("API");
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				ErrorMessage = "Username and password are required.";
				return Page();
			}

			var response = await client.PostAsJsonAsync("Auth/login", new { Username, Password });

			if (!response.IsSuccessStatusCode)
			{
				ErrorMessage = "Invalid credentials";
				return Page();
			}

			var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
			HttpContext.Session.SetString("JWT", tokenResponse!.Token);
			HttpContext.Session.SetString("Role", tokenResponse.Role);
			HttpContext.Session.SetString("Username", Username);
			// Extract user id from JWT (NameIdentifier)
			try
			{
				var parts = tokenResponse.Token.Split('.');
				if (parts.Length == 3)
				{
					var payloadJson = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.Nodes.JsonObject>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(parts[1]))));
					var nameId = payloadJson?["nameid"]?.ToString();
					if (!string.IsNullOrWhiteSpace(nameId))
					{
						HttpContext.Session.SetString("UserId", nameId);
					}
				}
			}
			catch { }
			Console.WriteLine($"Role returned: {tokenResponse.Role}");


			return RedirectToPage($"/Dashboard/{tokenResponse.Role}Dashboard");
		}

		public class TokenResponse
		{
			public string Token { get; set; } = "";
			public string Role { get; set; } = "";
		}

	private static string PadBase64(string base64)
	{
		// JWT base64url to base64
		base64 = base64.Replace('-', '+').Replace('_', '/');
		switch (base64.Length % 4)
		{
			case 2: base64 += "=="; break;
			case 3: base64 += "="; break;
		}
		return base64;
	}
	}
}
