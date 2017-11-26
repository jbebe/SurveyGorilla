using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SurveyGorilla;
using SurveyGorilla.Controllers;
using SurveyGorilla.Misc;
using SurveyGorilla.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestAdmin()
        {
            var admins = new List<AdminEntity>
            {
                new AdminEntity { Id = 1, Email = "1@1.com", Info = "{}", PasswordHash = "abcd" },
                new AdminEntity { Id = 2, Email = "2@2.com", Info = "{}", PasswordHash = "abcd" },
                new AdminEntity { Id = 3, Email = "3@3.com", Info = "{}", PasswordHash = "abcd" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AdminEntity>>();
            mockSet.As<IQueryable<AdminEntity>>().Setup(m => m.Provider).Returns(admins.Provider);
            mockSet.As<IQueryable<AdminEntity>>().Setup(m => m.Expression).Returns(admins.Expression);
            mockSet.As<IQueryable<AdminEntity>>().Setup(m => m.ElementType).Returns(admins.ElementType);
            mockSet.As<IQueryable<AdminEntity>>().Setup(m => m.GetEnumerator()).Returns(() => admins.GetEnumerator());

            var mockContext = new Mock<SurveyContext>();
            mockContext.Setup(c => c.Admins).Returns(mockSet.Object);

            var controller = new AdminController(mockContext.Object);

            var result = controller.GetAdmins() as List<AdminEntity>;
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void TestClient()
        {
            var clients = new List<ClientEntity>
            {
                new ClientEntity { Id = 1, Email = "1@1.com", Info = "{}", SurveyId = 1 },
                new ClientEntity { Id = 2, Email = "2@2.com", Info = "{}", SurveyId = 2 },
                new ClientEntity { Id = 3, Email = "3@3.com", Info = "{}", SurveyId = 3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ClientEntity>>();
            mockSet.As<IQueryable<ClientEntity>>().Setup(m => m.Provider).Returns(clients.Provider);
            mockSet.As<IQueryable<ClientEntity>>().Setup(m => m.Expression).Returns(clients.Expression);
            mockSet.As<IQueryable<ClientEntity>>().Setup(m => m.ElementType).Returns(clients.ElementType);
            mockSet.As<IQueryable<ClientEntity>>().Setup(m => m.GetEnumerator()).Returns(() => clients.GetEnumerator());

            var mockContext = new Mock<SurveyContext>();
            mockContext.Setup(c => c.Clients).Returns(mockSet.Object);

            var controller = new ClientController(mockContext.Object);

            var result = controller.GetClients(1) as List<AdminEntity>;
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestSurvey()
        {
            var survey = new List<SurveyEntity>
            {
                new SurveyEntity { Id = 1, Info = "{}"},
                new SurveyEntity { Id = 2, Info = "{}"},
                new SurveyEntity { Id = 3, Info = "{}"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<SurveyEntity>>();
            mockSet.As<IQueryable<SurveyEntity>>().Setup(m => m.Provider).Returns(survey.Provider);
            mockSet.As<IQueryable<SurveyEntity>>().Setup(m => m.Expression).Returns(survey.Expression);
            mockSet.As<IQueryable<SurveyEntity>>().Setup(m => m.ElementType).Returns(survey.ElementType);
            mockSet.As<IQueryable<SurveyEntity>>().Setup(m => m.GetEnumerator()).Returns(() => survey.GetEnumerator());

            var mockContext = new Mock<SurveyContext>();
            mockContext.Setup(c => c.Surveys).Returns(mockSet.Object);

            var controller = new SurveyController(mockContext.Object);

            var result = controller.GetSurveys() as List<SurveyEntity>;
            Assert.AreEqual(1, result.Count);
        }
    }
}
