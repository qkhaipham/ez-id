namespace MinimalApi.Models;

public record Order(OrderId OrderId, string CustomerName, decimal TotalPrice);
