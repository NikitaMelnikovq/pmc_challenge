using Microsoft.EntityFrameworkCore;
using SteelShop.Infrastructure.Data;
using SteelShop.Api.Config;
using SteelShop.Core.Services;
using SteelShop.Infrastructure.Services;
using SteelShop.Infrastructure.Background;
using SteelShop.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

// Db
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS
builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection("Cors"));
builder.Services.AddCors(o =>
{
    var cors = builder.Configuration.GetSection("Cors").Get<CorsOptions>();
    o.AddPolicy("default", p =>
        p.WithOrigins(cors?.AllowedOrigins ?? Array.Empty<string>())
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

// Options
builder.Services.Configure<DiscountOptions>(builder.Configuration.GetSection("Discounts"));
builder.Services.Configure<PriceRefreshOptions>(builder.Configuration.GetSection("PriceRefresh"));

// Services (DI)
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Background price refresh
var priceRefresh = builder.Configuration.GetSection("PriceRefresh").Get<PriceRefreshOptions>();
if (priceRefresh?.Enabled == true)
    builder.Services.AddHostedService<PriceRefreshWorker>();

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ApiExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("default");
app.MapControllers();

app.Run();