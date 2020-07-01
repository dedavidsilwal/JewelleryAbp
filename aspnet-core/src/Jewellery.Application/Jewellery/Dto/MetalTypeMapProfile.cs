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
            CreateMap<CreateEditMetalTypeDto, MetalType>().ReverseMap();

            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CreateEditProductDto, Product>().ReverseMap();

            CreateMap<CustomerDto, Customer>().ReverseMap();
                      
            CreateMap<CreateOrderDto, Order>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();

            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            CreateMap<CreateEditOrderDetailDto, OrderDetail>().ReverseMap();

            CreateMap<EditOrderDto, Order>().ReverseMap();
            CreateMap<EditOrderDto, OrderDto>().ReverseMap();

            CreateMap<CreateEditOrderDetailDto, OrderDetailDto>().ReverseMap();

            CreateMap<Invoice, InvoiceDto>().ReverseMap();
        }
    }
}
