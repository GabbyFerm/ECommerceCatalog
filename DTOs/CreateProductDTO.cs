namespace ECommerceCatalog.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }  // Instead of a Category object, you only use the category name
    }
}
