using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PROG3340_Midterm.Data;
using PROG3340_Midterm.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PROG3340_Midterm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;

		public AuthController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("login")]
		public ActionResult Login([FromBody] LoginRequest request)
		{
			var user = _context.Customers
				.FirstOrDefault(u => u.UserName == request.Username && u.Password == request.Password);
			if (user == null)
			{
				return Unauthorized("Invalid username or password.");
			}
			var token = GenerateToken(user);
			return Ok(new { Token = token, Role = user.Role });
		}

		private string GenerateToken(Customer customer)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, customer.UserName),
				new Claim(ClaimTypes.Role, customer.Role),
				new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()) // User ID claim for CustomerService
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("secret_key682348762431615432012848rhf8ebf438fr"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				//issuer: "yourdomain.com",
				//audience: "yourdomain.com",
				claims: claims,
				expires: DateTime.Now.AddMinutes(300), // I added time for testing purposes
				signingCredentials: creds);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
