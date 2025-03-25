using AutoMapper;
using ECommerceCatalog.DTOs;
using ECommerceCatalog.Models;

namespace ECommerceCatalog.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping for Product to ProductDTO
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));  // Mapping for category name

            // Mapping for CreateProductDTO to Product (no category handling here)
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // Ignore Category since it's handled separately in the controller
        }
    }
}
