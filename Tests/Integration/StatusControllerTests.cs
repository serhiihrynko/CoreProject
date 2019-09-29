using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Integration
{
    public class StatusControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetStatusReturnsUptime()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync("/");
            string message = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(message));
        }
    }
}
