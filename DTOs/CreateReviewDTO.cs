using System.ComponentModel.DataAnnotations;

namespace ECommerceCatalog.DTOs
{
    public class CreateReviewDTO
    {
        public int ProductId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
