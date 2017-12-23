using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TokenAuthWebApiCore.Server.IntegrationTest.Setup;
using Xunit;

namespace TokenAuthWebApiCore.Server.IntegrationTest
{
    public class ValuesControllerUnauthorizedTest : IClassFixture<TestFixture<TestStartupLocalDb>>
    {
        public ValuesControllerUnauthorizedTest(TestFixture<TestStartupLocalDb> fixture)
        {
            Client = fixture.HttpClient;
        }

        private HttpClient Client { get; }

        [Theory]
        [InlineData("POST")]
        [InlineData("GET")]
        [InlineData(new object[] { "PUT", 1 })]
        [InlineData(new object[] { "DELETE", 1 })]
        public async Task WhenNoAuthenticatedUser_MakeRequestRequest_Return_UnAuthorized(string method,
            int? id = null)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/values/{id}");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}