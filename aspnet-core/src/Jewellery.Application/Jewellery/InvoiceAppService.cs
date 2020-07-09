using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Jewellery.Jewellery
{
    public class InvoiceAppService : ApplicationService
    {
        private readonly IRepository<Invoice, Guid> _repository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IObjectMapper _objectMapper;

        public InvoiceAppService(
            IRepository<Invoice, Guid> repository,
            IRepository<Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository,
            IObjectMapper objectMapper)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _objectMapper = objectMapper;
        }


        public PagedResultDto<InvoiceDto> GetAll(PagedUserResultRequestDto input)
        {
            var query = (from inv in _repository.GetAll()
                         join o in _orderRepository.GetAll() on inv.OrderId equals o.Id
                         join c in _customerRepository.GetAll() on o.CustomerId equals c.Id
                         select new InvoiceDto
                         {
                             Id = inv.Id,
                             OrderId = o.Id,
                             InvoiceNumber = inv.InvoiceNumber,
                             PaidAmount = inv.PaidAmount,
                             TotalPaymentAmount = inv.TotalPaymentAmount,
                             PaymentStatus = inv.PaymentStatus.ToString(),
                             InvoiceDate = inv.InvoiceDate,
                             CustomerName = c.CustomerName,
                         })
                               .Skip(input.SkipCount)
                               .Take(input.MaxResultCount)
                               .ToList();

            var result = new PagedResultDto<InvoiceDto>
            {
                Items = query,
                TotalCount = query.Count
            };

            return result;
        }
    }


}
