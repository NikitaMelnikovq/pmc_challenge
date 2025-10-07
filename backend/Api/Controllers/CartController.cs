using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelShop.Api.Dtos;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;
using SteelShop.Infrastructure.Services;

namespace SteelShop.Api.Controllers;

[ApiController]
[Route("api/cart")]
public sealed class CartController : ControllerBase
{
    private readonly AppDbContext _app;
    private readonly CatalogDbContext _cat;
    private readonly ICartService _cart;

    public CartController(AppDbContext app, CatalogDbContext cat, ICartService cart)
    {
        _app = app; _cat = cat; _cart = cart;
    }

    [HttpPost("{cartId:guid}/items")]
    public async Task<ActionResult<CartDto>> AddItem(Guid cartId, [FromBody] AddToCartDto dto, CancellationToken ct)
    {
        // Добавляем с учётом единицы (метры/тонны) — сервис сам получит ступень и зафиксирует UnitPricePerMeter
        await _cart.AddItemAsync(cartId, dto.ProductId, dto.StockId, dto.Quantity, dto.Unit, ct);
        return await Get(cartId, ct);
    }

    [HttpGet("{cartId:guid}")]
    public async Task<ActionResult<CartDto>> Get(Guid cartId, CancellationToken ct)
    {
        var cart = await _app.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId, ct)
            ?? await _cart.GetOrCreateAsync(cartId, ct);

        if (!cart.Items.Any())
            return Ok(new CartDto(cart.Id, Enumerable.Empty<CartItemDto>(), 0m));

        var ids = cart.Items.Select(i => i.ProductId).Distinct().ToArray();
        var noms = await _cat.Nomenclature.Where(n => ids.Contains(n.ID)).ToDictionaryAsync(n => n.ID, ct);

        var items = cart.Items.Select(i =>
        {
            var nom = noms[i.ProductId];
            var meters = i.Unit == QuantityUnit.Meter
                ? i.Quantity
                : i.Quantity / (nom.Koef ?? throw new InvalidOperationException("Koef required"));

            var line = decimal.Round((decimal)meters * i.UnitPricePerMeter, 2);
            return new CartItemDto(i.Id, i.ProductId, i.StockId, i.Quantity, i.Unit, i.UnitPricePerMeter, line);
        }).ToList();

        var total = items.Sum(x => x.LineTotal);
        return Ok(new CartDto(cart.Id, items, total));
    }
}