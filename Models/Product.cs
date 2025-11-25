namespace ProductAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }  // ← Phải là decimal, không phải int
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}