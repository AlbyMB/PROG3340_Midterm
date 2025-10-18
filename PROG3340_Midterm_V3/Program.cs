using PROG3340_MidtermProject_V3.Models.Mapping;
using AutoMapper;
using PROG3340_MidtermProject_V3.Data;
using Microsoft.EntityFrameworkCore;
using PROG3340_MidtermProject_V3.Data.UnitOfWork;
using PROG3340_MidtermProject_V3.Services.Interfaces;
using PROG3340_MidtermProject_V3.Services.Implementations;
using PROG3340_MidtermProject_V3.Repositories.Interfaces;
using PROG3340_MidtermProject_V3.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddJsonOptions(x =>
	{
		x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
		x.JsonSerializerOptions.WriteIndented = true;
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options=>options.UseInMemoryDatabase("MidtermDb"));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.TokenValidationParameters = new()
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
				System.Text.Encoding.UTF8.GetBytes("secret_key682348762431615432012848rhf8ebf438fr")),
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
