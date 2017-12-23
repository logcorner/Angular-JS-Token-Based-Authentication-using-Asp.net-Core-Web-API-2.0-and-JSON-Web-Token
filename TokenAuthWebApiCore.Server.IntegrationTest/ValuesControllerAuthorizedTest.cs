using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TokenAuthWebApiCore.Server.IntegrationTest.Setup;
using TokenAuthWebApiCore.Server.Models;
using Xunit;

namespace TokenAuthWebApiCore.Server.IntegrationTest
{
    [TestCaseOrderer("TokenAuthWebApiCore.Server.IntegrationTest.Setup.PriorityOrderer",
        "TokenAuthWebApiCore.Server.IntegrationTest")]
    public class ValuesControllerAuthorizedTest : IClassFixture<TestFixture<TestStartupLocalDb>>
    {
        public ValuesControllerAuthorizedTest(TestFixture<TestStartupLocalDb> fixture)
        {
            Client = fixture.HttpClient;
        }

        private HttpClient Client { get; }

        [Fact, TestPriority(1)]
        public async Task WhenNoRegisteredUser_SignUpForToken_WithValidModelState_Return_OK()
        {
            // Arrange
            var obj = new RegisterViewModel
            {
                Email = "simpleuser@yopmail.com",
                Password = "WebApiCore1#",
                ConfirmPassword = "WebApiCore1#"
            };
            string stringData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync($"/api/auth/register", contentData);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory, TestPriority(2)]
        [InlineData("POST", "myRequestBody")]
        [InlineData("GET", "myRequestBody")]
        [InlineData(new object[] { "PUT", "myRequestBody", 1 })]
        [InlineData(new object[] { "DELETE", "myRequestBody", 1 })]
        public async Task WhenAuthenticatedUser_MakeRequestRequest_Return_Ok(string method, string obj = null,
            int? id = null)
        {
            // Arrange
            var jwToken = await GetJwToken();
            string token = $"bearer {jwToken.Token}";

            string stringData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/values/{id}");
            request.Content = contentData;
            request.Headers.Add("Authorization", token);
            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<JwToken> GetJwToken()
        {
            var loginViewModel = new LoginViewModel
            {
                Email = "simpleuser@yopmail.com",
                Password = "WebApiCore1#"
            };
            string loginViewModelData = JsonConvert.SerializeObject(loginViewModel);
            var contentData = new StringContent(loginViewModelData, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync($"/api/auth/token", contentData);
            response.EnsureSuccessStatusCode();
            var jwToken = JsonConvert.DeserializeObject<JwToken>(
                await response.Content.ReadAsStringAsync());
            return jwToken;
        }
    }
}