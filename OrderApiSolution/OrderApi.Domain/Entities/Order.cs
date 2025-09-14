namespace OrderApi.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ClientId { get; set; }
    public int OrderQuantity { get; set; }
    public DateTime OrderData { get; set; } = DateTime.UtcNow;
}