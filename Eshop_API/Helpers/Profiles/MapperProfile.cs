using AutoMapper;
using eshop_api.Entities;
using eshop_api.Models.DTO.Order;
using eshop_api.Models.DTO.Products;

namespace Eshop_API.Helpers.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<Order, OrderView>();
        }
    }
}
