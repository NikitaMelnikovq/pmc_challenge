using SteelShop.Core.Entities;

namespace SteelShop.Api.Dtos;

public sealed record AddToCartDto(
    int ProductId,
    int StockId,
    double Quantity,
    QuantityUnit Unit // "Meter" | "Ton"
);

public sealed record CartItemDto(
    long Id,
    int ProductId,
    int StockId,
    double Quantity,
    QuantityUnit Unit,
    decimal UnitPricePerMeter, // зафиксировано при добавлении
    decimal LineTotal          // Quantity -> (м) * UnitPricePerMeter
);

public sealed record CartDto(
    Guid Id,
    IEnumerable<CartItemDto> Items,
    decimal Total
);
