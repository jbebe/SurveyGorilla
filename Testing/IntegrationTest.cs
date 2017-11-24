using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyGorilla;
using System.Net.Http;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class IntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>().UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        [TestMethod]
        public async Task TestMethod1Async()
        {
            // Act
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            System.Console.WriteLine(responseString);
        }
    }
}
