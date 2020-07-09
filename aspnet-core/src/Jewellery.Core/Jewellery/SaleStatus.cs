using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery
{
    public enum SaleStatus
    {
        [Display(Name = "Sold")]


        Sold,
        [Display(Name = "Canceled")]

        Canceled,
        [Display(Name = "Returned")]

        Returned
    }
}
