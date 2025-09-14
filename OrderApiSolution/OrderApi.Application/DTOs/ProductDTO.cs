using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record ProductDTO(
    int ProductId, 
    [Required] string ProductName,
    [Required, Range(1, int.MaxValue)] int ProductQuantity,
    [Required, DataType(DataType.Currency)] decimal Price
    );