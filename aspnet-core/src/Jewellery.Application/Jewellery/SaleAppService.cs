using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Jewellery.Authorization;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{
    [AbpAuthorize(PermissionNames.Pages_Orders)]

    public class SaleAppService : AsyncCrudAppService<Sale, SaleDto, Guid, PagedUserResultRequestDto, CreateEditSaleDto, CreateEditSaleDto>
    {
        private readonly IRepository<Sale, Guid> _saleRepository;

        public SaleAppService(IRepository<Sale, Guid> saleRepository) : base(saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override Task<SaleDto> CreateAsync(CreateEditSaleDto input)
        {
            return base.CreateAsync(input);
        }

        public async Task<SalesReportDashboard[]> RecentSale()
        {
            return await _saleRepository
                .GetAll()
                .Include(s => s.Customer)
                .Select(s => new SalesReportDashboard
                {
                    Id = s.Id,
                    saleNumber = s.SaleNumber,
                    CustomerName = s.Customer.CustomerName,
                    SaleDate = s.SalesDate
                })
                .Take(5)
                .ToArrayAsync();
        }

        public async Task<int> TotalSaleCount() =>
            await _saleRepository
            .GetAll()
            .Where(s => s.SaleStatus == SaleStatus.Sold)
            .CountAsync();


        public override async Task<PagedResultDto<SaleDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var query = await _saleRepository
                .GetAll()
                .Include(c => c.Customer)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(x => new SaleDto
                {
                    Id = x.Id,
                    Customer = x.Customer.CustomerName,
                    SaleNumber = x.SaleNumber,
                    SalesDate = x.SalesDate,
                    PaymentStatus = x.PaymentStatus.ToString(),
                    SaleStatus = x.SaleStatus.ToString(),
                    PaidAmouunt = x.PaidAmount
                }).ToListAsync();

            return new PagedResultDto<SaleDto>() { Items = query, TotalCount = query.Count };

        }

        public async Task<CreateEditSaleDetailDto> FetchOrderWithDetails(Guid saleId) =>
           await _saleRepository.GetAll()
             .Include(p => p.SaleStatus)
             .Where(x => x.Id == saleId)
             .Select(x => ObjectMapper.Map<CreateEditSaleDetailDto>(x)).FirstOrDefaultAsync();


        public async Task CancelAsync(Guid id)
        {
            var sale = _saleRepository.Get(id);
            sale.SaleStatus = SaleStatus.Canceled;
            sale.PaymentStatus = PaymentStatus.None;

            await _saleRepository.UpdateAsync(sale);
        }


        public async Task<CustomerSaleDisplayDto> FetchSaleReport(Guid saleId)
        {
            var result = await _saleRepository.GetAll()
                .Where(s => s.Id == saleId).
                 Include(s => s.SaleDetails).ThenInclude(s => s.Product)
                .Include(c => c.Customer)
                .Select(query => new CustomerSaleDisplayDto
                {

                    CustomerName = query.Customer.CustomerName,
                    CustomerAddress = query.Customer.Address,
                    PhoneNumber = query.Customer.PhoneNumber,
                    SaleDate = query.SalesDate,
                    SaleNumber = query.SaleNumber,
                    PaidAmount = query.PaidAmount,
                    SaleStatus = query.SaleStatus.ToString(),
                    PaymentStatus = query.PaymentStatus.ToString(),
                    SaleDetails = query.SaleDetails.Select(o => new CustomerSaleDetailDisplayDto
                    {
                        Quantity = o.Quantity,
                        MakingCharge = o.MakingCharge,
                        Weight = o.Weight,
                        Wastage = o.Wastage,
                        MetalType = o.MetalType,
                        TodayMetalCost = o.TodayMetalCost,
                        SubTotal = o.SubTotal,
                        TotalWeight = o.TotalWeight,
                        ProductName = o.Product.ProductName
                    }).ToList()

                })
                .FirstOrDefaultAsync();

            return result;

        }




    }
}
