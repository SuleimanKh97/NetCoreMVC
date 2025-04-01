using TestMasterPeace.Models;

namespace TestMasterPeace.DTOs.ProductsDTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public long? CategoryId { get; set; }
}
