using SteelShop.Core.Entities;

namespace SteelShop.Api.Dtos;

public sealed record ProductFilterDto(
    string? StockId,              // ← string
    string? StockCity,
    string? IDType,               // ← string
    double? Diameter,
    double? PipeWallThickness,
    string? Gost,
    string? SteelGrade,
    QuantityUnit? Unit = null,
    double? Quantity = null,
    int Page = 1,
    int PageSize = 50
);

public sealed record ProductListItemDto(
    int Id, string Name, string? Gost, string? SteelGrade,
    double? Diameter, double? PipeWallThickness, string? IDType,  // ← string?
    string StockId,                                               // ← string
    decimal? BasePricePerMeter,
    decimal? EffectivePricePerMeter
);

public sealed record ProductQuoteResponse(
    int ProductId,
    string StockId,                 // ← string
    QuantityUnit Unit,
    double Quantity,
    decimal EffectivePricePerMeter
);