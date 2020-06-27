using System.Threading.Tasks;
using Jewellery.Models.TokenAuth;
using Jewellery.Web.Controllers;
using Shouldly;
using Xunit;

namespace Jewellery.Web.Tests.Controllers
{
    public class HomeController_Tests: JewelleryWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}