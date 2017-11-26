using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyGorilla;
using SurveyGorilla.Misc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class IntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        public IntegrationTest()
        {
            // Arrange
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(Path.Combine("."), startupAssembly);
            _server = new TestServer(
                new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Testing")
                .UseContentRoot(contentRoot)
            );
            _client = _server.CreateClient();
        }

        [TestMethod]
        public void TestRegisterLoginCreateSurveyCreateClient()
        {
            {
                var body = "{ \"email\": \"asd@asd.hu\", \"password\": \"asdasd\", \"info\": \"Test Name\" }";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var result = _client.PostAsync("/Register", content).Result;
                Assert.AreEqual(200, result.StatusCode);
            }
            {
                var body = "{ \"email\": \"asd@asd.hu\", \"password\": \"asdasd\" }";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var result = _client.PostAsync("/Login", content).Result;
                Assert.AreEqual(200, result.StatusCode);
            }
            int surveyId;
            {
                var body = "{ \"name\": \"Test Survey\", \"info\": \"{}\" }";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var result = _client.PostAsync("/api/Survey", content).Result;
                Assert.AreEqual(200, result.StatusCode);
                surveyId = result.Content.ReadAsStringAsync().Result.ToObject().Value<int>("id");
            }
            {
                var body = "{ \"email\": \"client@client.hu\", \"info\": \"{}\" }";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var result = _client.PostAsync($"/api/Survey/{surveyId}/Client", content).Result;
                Assert.AreEqual(200, result.StatusCode);
            }
        }
    }
}
