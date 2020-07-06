using AutoMapper;

namespace Jewellery.Jewellery.Dto
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
        }
    }
}
