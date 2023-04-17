using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<RandomService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddScoped<IPayment, Boleto>();
builder.Services.AddScoped<IPayment, CreditCard>();

builder.Services.AddDbContext<TestDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));
var app = builder.Build();

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
