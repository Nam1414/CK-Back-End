using System.ComponentModel.DataAnnotations;// <--- Quan trọng: Dòng này để dùng [Required], [Range], etc.

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Range(0, double.MaxValue)]// Giá không âm
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]// Số lượng tồn kho không âm
    public int Stock { get; set; }
}