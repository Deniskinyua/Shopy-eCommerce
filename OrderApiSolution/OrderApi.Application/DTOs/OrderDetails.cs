using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record OrderDetails(
    [Required] int OrderId,
    [Required] int OrderQuantity,
    [Required] int ProductId,
    [Required] string ProductName,
    [Required] int ClientId,
    [Required] string ClientEmail,
    [Required] string ClientPhone,
    [Required, DataType(DataType.Currency)] decimal UnitProductPrice,
    [Required, DataType(DataType.Currency)] decimal TotalOrderPrice,
    [Required] DateTime OrderDate
    );