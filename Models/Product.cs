using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}