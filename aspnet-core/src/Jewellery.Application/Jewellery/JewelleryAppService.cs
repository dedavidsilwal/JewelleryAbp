using Abp.Application.Services;

namespace Jewellery.Jewellery
{
    public class JewelleryAppService : ApplicationService
    {
        public string[] GetAllWeighTypes() => new string[] { "gm", "tola", "kg" };
    }
}
