using DotNetEnv;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SteelShop.Api.Filters;
using SteelShop.Infrastructure.Data;
using SteelShop.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) .env и переменные окружения
// .env лежит в папке проекта API:
var envPath = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envPath)) Env.Load(envPath);

// чтобы ENV попадали в IConfiguration (для CORS и т.п.)
builder.Configuration.AddEnvironmentVariables();

// 2) строки подключения (env → appsettings fallback)
var catalogConn =
    Environment.GetEnvironmentVariable("CATALOG_DB") ??
    builder.Configuration.GetConnectionString("Catalog");

var appConn =
    Environment.GetEnvironmentVariable("APP_DB") ??
    builder.Configuration.GetConnectionString("App");

if (string.IsNullOrWhiteSpace(catalogConn) || string.IsNullOrWhiteSpace(appConn))
    throw new InvalidOperationException("DB connection strings are not set (CATALOG_DB / APP_DB or ConnectionStrings:Catalog/App).");

// 3) DB-контексты
builder.Services.AddDbContext<CatalogDbContext>(opt => opt.UseNpgsql(catalogConn));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(appConn));

// 4) DI
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// 5) MVC + Swagger
builder.Services.AddControllers(o => o.Filters.Add<ApiExceptionFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 6) CORS (список в .env: CORS_ALLOWEDORIGINS=...)
var allowedCsv = Environment.GetEnvironmentVariable("CORS_ALLOWEDORIGINS")
                ?? builder.Configuration["Cors:AllowedOrigins"]; // поддержим и appsettings
var allowedList = (allowedCsv ?? "http://localhost:5173,http://localhost:3000")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

builder.Services.AddCors(o =>
{
    o.AddPolicy("default", p =>
    {
        p.AllowAnyHeader().AllowAnyMethod().AllowCredentials();

        // Явные урлы без звёздочек
        var fixedOrigins = allowedList.Where(x => !x.Contains('*')).ToArray();
        if (fixedOrigins.Length > 0) p.WithOrigins(fixedOrigins);

        // Поддержка масок вида https://*.ngrok-free.app
        p.SetIsOriginAllowed(origin =>
        {
            if (fixedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase)) return true;
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var u)) return false;
            var host = u.Host.ToLowerInvariant();

            foreach (var pattern in allowedList.Where(x => x.Contains('*')))
            {
                // вытаскиваем суффикс после "*."
                var suffix = pattern.Split("*.").Last();
                if (host.EndsWith(suffix.Trim().TrimEnd('/'))) return true;
            }
            return false;
        });
    });
});

// 7) Прокси-заголовки (туннели) + HTTPS редирект
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("default");
app.MapControllers();

app.Run();

public partial class Program {}
