using System.Net;
using System.Net.Http.Json;
using eshop_api.Entities;
using eshop_api.Models.DTO.Products;
using eshop_api.Service.Products;
using Eshop_API.Controllers.Address;
using eshop_pbl6.Services.Addresses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace UnitTest_Eshop_API
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Mock<IAddressService> addressService;
        public UnitTest1()
        {
            addressService = new Mock<IAddressService>();
        }
        [TestMethod]
        public async Task TestGetProduct()
        {
            // using var application =  new WebApplicationFactory<Program>();
            // var client = application.CreateClient();
            // var products = await client.GetFromJsonAsync<List<Product>>("api/Product/get-list-product");
            // Assert.IsNotNull(products);
            HttpResponseMessage httpResponseMessage = new(){
                StatusCode = HttpStatusCode.InternalServerError 
            };
            var mockClient = MockClientTest.GetMockHttpClient(httpResponseMessage);

            using var application = new WebApplicationFactory<ProductDto>()
                .WithWebHostBuilder(buidler => 
                {
                    buidler.ConfigureServices(services =>
                    {
                        ServiceDescriptor serviceDescriptor = new(typeof(IProductService),
                                typeof(ProductService), ServiceLifetime.Scoped);
                        services.Remove(serviceDescriptor);
                       // services.AddScoped<IProductService>(s => new ProductService());
                    });

                    
                });
                var client = application.CreateClient();
                var reponseMessage = await client.GetAsync("/api/Product/get-list-product");
                Assert.AreEqual(HttpStatusCode.OK, reponseMessage.StatusCode);

        }
        [TestMethod]
        public void GetProvinceList_GetProvince()
        {
            HttpResponseMessage httpResponseMessage = new()
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
            List<Province> provinces = GetListProvince();
            addressService.Setup(x => x.GetProvince()).Returns(provinces);
            var addressController = new AddressController(addressService.Object);

            var addressResult =  addressController.GetProvince();
            var responseMessage = addressResult.ToString();
            Assert.IsNotNull(addressResult);
            //Assert.AreEqual(GetListProvince().Count(), provinces.Count());
            //Assert.Equals(GetListProvince().ToString(), addressResult.ToString());
            //Assert.IsTrue(provinces.Equals(addressResult));
            //Assert.AreEqual(HttpStatusCode.OK, addressResult);

        }
        private List<Province> GetListProvince()
        {
            List<Province> provinces = new List<Province>
            {
                new Province
                {
                    Id = 1,
                    Name = "Hà Nội"
                },
                new Province
                {
                    Id = 2,
                    Name = "Đà Nẵng"
                },
                new Province
                {
                    Id = 3,
                    Name = "Hồ Chí Minh"
                },
            };
            return provinces;
        }
    }
}
