using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace UnitTest_Eshop_API
{
    public static class MockClientTest
    {
        public static HttpClient GetMockHttpClient(HttpResponseMessage httpResponseMessage){
            var httpMessageHandMock = new Mock<HttpMessageHandler>();

            httpMessageHandMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(),ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpMessageHandMock.Object){
                BaseAddress = new System.Uri("http://localhost")
            };

            return httpClient;
        }
    }
}