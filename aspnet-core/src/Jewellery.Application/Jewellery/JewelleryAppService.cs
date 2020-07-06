using Abp.Application.Services;

namespace Jewellery.Jewellery
{
    public class JewelleryAppService : ApplicationService
    {
        //get weighttype enum 

        public async System.Threading.Tasks.Task<string[]> GetAllWeighTypes() => new string[] { "gm","tola","kg" };
    }
}
