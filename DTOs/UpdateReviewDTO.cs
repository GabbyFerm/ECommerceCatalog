﻿namespace ECommerceCatalog.DTOs
{
    public class UpdateReviewDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
