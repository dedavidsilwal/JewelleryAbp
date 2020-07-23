using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{
    public class DuesAppService : ApplicationService
    {
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Sale, Guid> _saleRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public DuesAppService(
            IRepository<Invoice, Guid> invoiceRepository,
            IRepository<Order, Guid> orderRepository,
            IRepository<Sale, Guid> saleRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
        }

        public async Task<PagedResultDto<DueDto>> GetOrderDuesAmount(PagedUserResultRequestDto input)
        {

            //var query = _orderRepository.GetAll()
            //    .Include(s => s.Invoices)
            //    .Include(x => x.OrderDetails).Include(c => c.Customer)
            //                    .Where(

            //    x => !(x.OrderStatus == OrderStatus.Canceled) && x.OrderDetails.Sum(s => ((s.Weight + s.Wastage) * s.TodayMetalCost) + s.MakingCharge)
            //                                    != x.Invoices.Sum(s => s.PaidAmount))
            //                    .OrderByDescending(s => s.OrderNumber)
            //                    .Select(s => new DueDto
            //                    {
            //                        CustomerName = s.Customer.CustomerName,
            //                        Dues = s.Total - s.Invoices.Sum(s => s.PaidAmount)
            //                    });

            //var result = await query.Skip(input.SkipCount)
            //        .Take(input.MaxResultCount)
            //         .ToListAsync();

            var query = (await _orderRepository
                .GetAllIncluding(s => s.Invoices, s => s.OrderDetails, s => s.Customer)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Customer.DisplayName.Contains(input.Keyword))
                .Where(c => c.OrderStatus != OrderStatus.Canceled)
                .ToListAsync())
                .Where(c => c.Total != c.TotalPaidAmount)
                .ToList();

            var result =
                query.Skip(input.SkipCount)
                .Select(s =>
                new DueDto
                {
                    OrderId = s.Id,
                    CustomerName = s.Customer.DisplayName,
                    Dues = s.Total - s.TotalPaidAmount,
                    TotalAmount = s.Total,
                    TotalPaidAmount = s.TotalPaidAmount
                })
                .Take(input.MaxResultCount)
                .ToList();


            return new PagedResultDto<DueDto>
            {
                Items = result,
                TotalCount = query.Count
            };
        }

        public async Task<PagedResultDto<DueDto>> GetSaleDuesAmount(PagedUserResultRequestDto input)
        {

            var query = (await _saleRepository
               .GetAllIncluding(s => s.Invoices, s => s.SaleDetails, s => s.Customer)
               .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Customer.DisplayName.Contains(input.Keyword))
               .Where(c => c.SaleStatus == SaleStatus.Sold)
               .ToListAsync())
               .Where(c => c.TotalAmount != c.TotalPaidAmount)
               .ToList();

            var result =
                query.Skip(input.SkipCount)
                .Select(s => new DueDto
                {
                    OrderId = s.Id,
                    CustomerName = s.Customer.DisplayName,
                    Dues = s.TotalAmount - s.TotalPaidAmount,
                    TotalAmount = s.TotalAmount,
                    TotalPaidAmount = s.TotalPaidAmount
                })
                .Take(input.MaxResultCount)
                .ToList();


            return new PagedResultDto<DueDto>
            {
                Items = result,
                TotalCount = query.Count
            };
        }
    }
}
