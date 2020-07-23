namespace Jewellery.Jewellery.Dto
{
    public class DueDto
    {
        public System.Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Dues { get; set; }
    }
}
