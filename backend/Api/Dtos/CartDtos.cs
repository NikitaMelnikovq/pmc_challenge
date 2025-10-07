using SteelShop.Core.Entities;

namespace SteelShop.Api.Dtos;

public sealed record AddToCartDto(int ProductId, string StockId, double Quantity, QuantityUnit Unit);

public sealed record CartItemDto(long Id, int ProductId, string StockId, double Quantity,
                                 QuantityUnit Unit, decimal UnitPricePerMeter, decimal LineTotal);


public sealed record CartDto(
    Guid Id,
    IEnumerable<CartItemDto> Items,
    decimal Total
);
