using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Invoice, Guid> _invoiceRepository;

        public SaleAppService(
            IRepository<Sale, Guid> saleRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Invoice, Guid> invoiceRepository) : base(saleRepository)
        {
            _saleRepository = saleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _invoiceRepository = invoiceRepository;
        }


        protected override IQueryable<Sale> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = Repository.GetAllIncluding(x => x.Customer)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Customer.DisplayName.Contains(input.Keyword));
            return query;
        }

        public override async Task<SaleDto> CreateAsync(CreateEditSaleDto input)
        {

            using var uow = _unitOfWorkManager.Begin();

            var sale = ObjectMapper.Map<Sale>(input);

            if (sale.PaidAmount.HasValue && input.TotalAmount > input.PaidAmount.Value)
            {
                sale.PaymentStatus = PaymentStatus.PartialPaid;
            }
            else
            {
                sale.PaymentStatus = PaymentStatus.Paid;
            }

            sale.SaleStatus = SaleStatus.Sold;
            var saleResult = await _saleRepository.InsertAsync(sale);

            var invoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                SaleId = saleResult.Id,
                Sale = saleResult,
                PaidAmount = sale.PaidAmount.Value
            };

            await _invoiceRepository.InsertAsync(invoice);

            await uow.CompleteAsync();

            return ObjectMapper.Map<SaleDto>(saleResult);

        }

        public async Task<SalesReportDashboard[]> RecentSale()
        {
            return await _saleRepository
                .GetAll()
                .Include(s => s.Customer)
                .Where(s => s.SaleStatus == SaleStatus.Sold)
                .Select(s => new SalesReportDashboard
                {
                    Id = s.Id,
                    saleNumber = s.SaleNumber,
                    CustomerName = s.Customer.DisplayName,
                    SaleDate = s.SalesDate,
                    PaidAmount = s.PaidAmount,
                    TotalAmount = s.TotalAmount

                })
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .ToArrayAsync();
        }

        public async Task<int> TodaySaleCount() =>
            await _saleRepository
            .GetAll()
            .Where(s => s.SaleStatus == SaleStatus.Sold && s.SalesDate.Date == DateTime.Today.Date)
            .CountAsync();

        public async Task<decimal?> TodaySaleAmount() =>
                                  await _saleRepository
                                  .GetAll()
                                  .Where(s => s.SaleStatus == SaleStatus.Sold && s.SalesDate.Date == DateTime.Today.Date)
                                  .SumAsync(s => s.PaidAmount);


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
                    Customer = x.Customer.DisplayName,
                    SaleNumber = x.SaleNumber,
                    SalesDate = x.SalesDate,
                    PaymentStatus = x.PaymentStatus.ToString(),
                    SaleStatus = x.SaleStatus.ToString(),
                    PaidAmouunt = x.PaidAmount
                })
                .OrderByDescending(s => s.SalesDate)
                .ToListAsync();

            return new PagedResultDto<SaleDto>() { Items = query, TotalCount = await _saleRepository.CountAsync() };

        }

        public async Task<EditSaleDto> FetchSaleWithDetails(Guid saleId) =>
           await _saleRepository.GetAll()
            .Include(p => p.SaleDetails)
            .Include(c => c.Customer)

             .Where(x => x.Id == saleId)
             .Select(x => new EditSaleDto
             {
                 CustomerName = x.Customer.DisplayName,
                 PaidAmount = x.PaidAmount,
                 Id = x.Id,
                 SaleNumber = x.SaleNumber,
                 SaleDetails = x.SaleDetails.Select(y => new CreateEditSaleDetailDto
                 {
                     MakingCharge = y.MakingCharge,
                     MetalType = y.MetalType,
                     SaleId = y.SaleId,
                     ProductId = y.ProductId,
                     Quantity = y.Quantity,
                     TodayMetalCost = y.TodayMetalCost,
                     Wastage = y.Wastage,
                     Weight = y.Weight
                 }).ToList()

             })
            .FirstOrDefaultAsync();


        public async Task CancelAsync(Guid id)
        {
            using var uow = _unitOfWorkManager.Begin();

            var sale = _saleRepository.Get(id);
            sale.SaleStatus = SaleStatus.Canceled;
            sale.PaymentStatus = PaymentStatus.None;

            //if invoiced  delete

            var invoice = await _invoiceRepository.GetAll().Where(s => s.OrderId == id).FirstOrDefaultAsync();
            await _invoiceRepository.DeleteAsync(invoice);
            await _saleRepository.UpdateAsync(sale);

            await uow.CompleteAsync();
        }


        public async Task<CustomerSaleDisplayDto> FetchSaleReport(Guid saleId)
        {
            var result = await _saleRepository.GetAll()
                .Where(s => s.Id == saleId).
                 Include(s => s.SaleDetails).ThenInclude(s => s.Product)
                .Include(c => c.Customer)
                .Select(query => new CustomerSaleDisplayDto
                {

                    CustomerName = query.Customer.DisplayName,
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
