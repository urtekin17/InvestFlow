using InvestFlow.API.ExceptionHandlers;
using InvestFlow.Application.Customers.CreateCustomer;
using InvestFlow.Application.Customers.GetCustomerById;
using InvestFlow.Application.Handlers;
using InvestFlow.Application.Interfaces;
using InvestFlow.Infrastructure;
using InvestFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPortfolioPositionRepository, PortfolioPositionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
builder.Services.AddScoped<CreateCustomerHandler>();
builder.Services.AddScoped<GetCustomerByIdHandler>();
builder.Services.AddScoped<PlaceBuyOrderHandler>();
builder.Services.AddScoped<PlaceSellOrderHandler>();

var connectionString =
    builder.Configuration.GetConnectionString("InvestFlowDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContextFactory<AppDbContext>(
    options => options.UseNpgsql(connectionString),
    ServiceLifetime.Scoped);

var app = builder.Build();

app.UseExceptionHandler(_ => { });
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();