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
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{
    public class InvoiceAppService : ApplicationService
    {
        private readonly IRepository<Invoice, Guid> _repository;

        public InvoiceAppService(
            IRepository<Invoice, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<decimal> TodayInvoicedAmount() =>
                    await _repository.GetAll()
                    .Where(s => s.InvoiceDate.Date == DateTime.Today.Date)
                    .SumAsync(s => s.PaidAmount);



        public async Task<PagedResultDto<InvoiceDto>> GetAllAsync(PagedUserResultRequestDto input)
        {

            var invoice = await _repository.GetAll().Include(s => s.Sale).ThenInclude(c => c.Customer).Include(o => o.Order).ThenInclude(c => c.Customer)
                               .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Sale.Customer.DisplayName.Contains(input.Keyword) || x.Order.Customer.DisplayName.Contains(input.Keyword))
                               .Skip(input.SkipCount)
                               .Take(input.MaxResultCount)
                               .Select(x => new InvoiceDto
                               {
                                   Id = x.Id,
                                   OrderId = x.OrderId,
                                   SaleId = x.SaleId,
                                   InvoiceNumber = x.InvoiceNumber,
                                   PaidAmount = x.PaidAmount,
                                   InvoiceDate = x.InvoiceDate,
                                   CustomerName = x.OrderId.HasValue ? x.Order.Customer.DisplayName : x.Sale.Customer.DisplayName
                               })
                               .ToListAsync();


            var result = new PagedResultDto<InvoiceDto>
            {
                Items = invoice,
                TotalCount = await _repository.CountAsync()
            };

            return result;
        }
    }


}
