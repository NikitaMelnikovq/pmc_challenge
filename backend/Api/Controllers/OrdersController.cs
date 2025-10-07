using Microsoft.AspNetCore.Mvc;
using SteelShop.Api.Dtos;
using SteelShop.Infrastructure.Services;

namespace SteelShop.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;
    public OrdersController(IOrderService orders) => _orders = orders;

    [HttpPost("{cartId:guid}")]
    public async Task<ActionResult<OrderResponse>> Create(Guid cartId, [FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        var order = await _orders.CreateAsync(cartId, dto.FirstName, dto.LastName, dto.INN, dto.Phone, dto.Email, ct);
        return Ok(new OrderResponse(order.Id, order.Total));
    }
}