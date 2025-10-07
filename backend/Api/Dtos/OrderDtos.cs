namespace SteelShop.Api.Dtos;

public sealed record CreateOrderDto(
    string FirstName,
    string LastName,
    string INN,
    string Phone,
    string Email
);

public sealed record OrderResponse(
    long OrderId,
    decimal Total
);
