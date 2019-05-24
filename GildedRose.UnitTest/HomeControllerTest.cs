using System;
using Xunit;
using GildedRose.WebApi.Controllers;
using GildedRose.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

using System.Linq;
using System.Collections.Generic;

namespace GildedRose.UnitTest
{
    public class HomeControllerTest
    {
        private TrovTestDB TestDB { get; set; }
        public HomeControllerTest()
        {
            DbContextOptionsBuilder<TrovTestDB> optionBuilder = new DbContextOptionsBuilder<TrovTestDB>();
            optionBuilder.UseInMemoryDatabase("TestDBMock");
            this.TestDB = new TrovTestDB(optionBuilder.Options);
        }
        [Fact]
        public void GetItem_ReturnOK()
        {
            var itemMock = new Mock<DbSet<Item>>();
            itemMock.AddList(
                new Item { Id = 1, Name = "Banana", Description = "Test Banana" },
                new Item { Id = 1, Name = "Banana", Description = "Test Banana" }
            );

            this.TestDB.Items = itemMock.Object;
            HomeController homeCtrl = new HomeController(this.TestDB);
            IActionResult result = homeCtrl.GetItem();
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void GetItem_ReturnFail()
        {
            HomeController homeCtrl = new HomeController(this.TestDB);
            IActionResult result = homeCtrl.GetItem();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Theory]
        [ClassData(typeof(UserTestData))]
        public void Login_Test(string user, string password, bool ok)
        {
            var postUser = new Mock<PostUser>();
            postUser.Setup(x => x.UserName).Returns(user);
            postUser.Setup(x => x.Password).Returns(password);

            this.TestDB.Customers.Add(new Customer
            {
                FirstName = "Test1",
                LastName = "Last",
                UserName = "user1",
                Password = "123"
            });
            this.TestDB.Customers.Add(new Customer
            {
                FirstName = "Test2",
                LastName = "Last2",
                UserName = "user2",
                Password = "abc"
            });
            this.TestDB.SaveChanges();

            HomeController homeCtrl = new HomeController(this.TestDB);
            IActionResult result = homeCtrl.Login(postUser.Object);
            bool pass = result is OkObjectResult;
            Assert.Equal(pass, ok);
        }
        public class UserTestData:TheoryData<string, string, bool>
        {
            public UserTestData()
            {
                Add("user1", "123", true);
                Add("user2", "abc", true);
                Add("hacker", "xxx", false);
            }
        }



    }


    public static class MockExtensions
    {
        public static void AddList<T>(this Mock mock, params T[] entities)
        {
            var list = entities.ToList<T>().AsQueryable();
            mock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(list.Provider);
            mock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(list.Expression);
            mock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(list.ElementType);
            mock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(list.GetEnumerator());
        }
        public static void AsQuerable<T>(this Mock mock)
        {
            var list = new List<T>().AsQueryable();
            mock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(list.Provider);
            mock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(list.Expression);
            mock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(list.ElementType);
            mock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(list.GetEnumerator());
        }
    }
}
