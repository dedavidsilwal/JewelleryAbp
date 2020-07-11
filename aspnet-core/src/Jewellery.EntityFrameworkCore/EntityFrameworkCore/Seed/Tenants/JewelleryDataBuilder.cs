using Jewellery.Jewellery;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.EntityFrameworkCore.Seed.Tenants
{
    public class JewelleryDataBuilder
    {
        private readonly JewelleryDbContext _context;
        private readonly int _tenantId;

        public JewelleryDataBuilder(JewelleryDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }
        public void Create()
        {

            if (!_context.MetalTypes.Any())
            {

                var metalTypes = new List<MetalType>()
                {
                    new MetalType
                {
                    Name="Gold",
                    Price=87000,
                    WeightType = WeightType.Gram
                },
                      new MetalType
                {
                    Name="Silver",
                    Price=87000,
                    WeightType = WeightType.Gram
                }
                };

                _context.MetalTypes.AddRange(metalTypes);
                _context.SaveChanges();
            }

            if (!_context.Customers.Any())
            {
                _context.Customers.Add(new Customer
                {
                    CustomerName = "David Silwal",
                    PhoneNumber = "9849861177",
                    Address = "Hetauda"
                });
                _context.SaveChanges();
            }


        }


    }
}
