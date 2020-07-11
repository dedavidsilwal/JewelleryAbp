using AutoMapper;
using Jewellery.Jewellery.Dto;

namespace Jewellery.Jewellery.Mappings
{
    public class MetalTypeMapProfile : Profile
    {
        public MetalTypeMapProfile()
        {
            CreateMap<MetalTypeDto, MetalType>().ReverseMap();
            CreateMap<CreateEditMetalTypeDto, MetalType>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(p => p.MetalType, p => p.MapFrom(s => s.MetalType.Name))
                .ReverseMap();
            //   .ForMember(p => p.MetalType, p => p.MapFrom(s => s.MetalType.Name));    

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

            CreateMap<Sale, SaleDto>().ReverseMap();
            CreateMap<Sale, CreateEditSaleDto>().ReverseMap();
            CreateMap<SaleDetail, SaleDetailDto>().ReverseMap();
            CreateMap<SaleDetail, CreateEditSaleDetailDto>().ReverseMap();
        }
    }
}
