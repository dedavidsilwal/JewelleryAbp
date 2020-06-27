using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewellery.Jewellery.Dto
{
    public class MetalTypeMapProfile : Profile
    {
        public MetalTypeMapProfile()
        {
            CreateMap<MetalTypeDto, MetalType>().ReverseMap();

            CreateMap<ProductDto, Product>().ReverseMap();

            CreateMap<CustomerDto, Customer>().ReverseMap();
                      
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<CreateOrderDto, Order>().ReverseMap();
            CreateMap<EditOrderDto, Order>().ReverseMap();
        }
    }
}
