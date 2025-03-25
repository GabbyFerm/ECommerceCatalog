namespace ECommerceCatalog.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }  // Or categoryId if using categoryId
    }
}
