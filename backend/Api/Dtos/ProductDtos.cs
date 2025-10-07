using SteelShop.Core.Entities;

namespace SteelShop.Api.Dtos;

public sealed record ProductFilterDto(
    int? StockId,
    string? StockCity,
    int? IDType,
    double? Diameter,
    double? PipeWallThickness,
    string? Gost,
    string? SteelGrade,
    // опционально: если фронту нужно сразу увидеть конечную цену за метр по количеству
    QuantityUnit? Unit = null,
    double? Quantity = null,
    int Page = 1,
    int PageSize = 50
);

public sealed record ProductListItemDto(
    int Id,
    string Name,
    string? Gost,
    string? SteelGrade,
    double? Diameter,
    double? PipeWallThickness,
    int? IDType,
    int StockId,
    decimal? BasePricePerMeter,         // PriceM из прайса
    decimal? EffectivePricePerMeter     // если переданы Unit+Quantity+StockId — цена за метр со скидкой/ступенью
);

public sealed record ProductQuoteResponse(
    int ProductId,
    int StockId,
    QuantityUnit Unit,
    double Quantity,
    decimal EffectivePricePerMeter
);