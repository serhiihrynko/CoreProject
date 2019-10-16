using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Integration
{
    public class StatusControllerTests : IntegrationTest
    {
        public StatusControllerTests(WebApplicationFactory<API.Startup> factory) 
            : base(factory) 
        { 
        }


        [Fact]
        public async Task GetStatusReturnsUptime()
        {
            // Arrange

            // Act
            var response = await _httpclient.GetAsync("/status");
            string message = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(message));
        }
    }
}
