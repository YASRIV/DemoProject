using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WasherApi.Process;
using WasherApi.Repository;
using WasherApi.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace UnitTests

{
    [TestClass]
    public class WasherTests
    {

        private WashersDbContext _context;
        private WasherProcess _process;
        private WasherRepository _repository;
        private WasherController _controller;


        [TestInitialize]
        public void setup()
        {
            //Set - in Memory Use in Test 
            var option = new DbContextOptionsBuilder<WashersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new WashersDbContext(option);
            _repository = new WasherRepository(_context);
            _process = new WasherProcess(_repository);
            _controller = new WasherController(_process);

            //_context.Washers.Add(new Washer { WasherId = 1, WasherName = "Arjun Kumar", Email = "arjun.kumar@example.com", PhoneNo = "9876543210", IsActive = true });
            //_context.SaveChanges();
        }

        Washer washer = new Washer
        { WasherName = "Ravi Sharma", Email = "ravi.sharma@example.com", PhoneNo = "8765432109", IsActive = true };


        [TestMethod]
        public async Task AddWasher_ShouldReturnOk_WhenWasherDataIsAdded()
        {
            //Arrange: Put Above Demo data for testing 

            //Act: Call AddWasher() of Washer Conrtroller
            var result = _controller.AddWasher(washer);

            //Assert: Check the response of the above method

            var OkResult = result.Result as OkObjectResult;

            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);

            //Assert: CHeck the above resposnse in DataBase
            var storedData = await _context.Washers.FirstOrDefaultAsync(c => c.Email == washer.Email);
            Assert.AreEqual("Ravi Sharma", storedData.WasherName);
        }

        [TestMethod]
        public async Task GetWashers_ReturnsOk_WhenWashersExist()
        {
            // Arrange
            _context.Washers.Add(new Washer { WasherId = 1, WasherName = "John", Email = "john@example.com", PhoneNo = "1234567890", IsActive = true });
            _context.SaveChanges();

            // Act
            var result = await _controller.getwashers();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task GetWashers_ReturnsBadRequest_WhenNoWashersExist()
        {
            _context.Washers.RemoveRange();
            // Act
            var result = await _controller.getwashers();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetWasherById_ReturnsOk_WhenWasherExists()
        {
            // Arrange
            _context.Washers.Add(new Washer { WasherId = 2, WasherName = "Mike", Email = "mike@example.com", PhoneNo = "9876543210", IsActive = true });
            _context.SaveChanges();

            // Act
            var result = await _controller.getwasherbyid(2);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetWasherById_ReturnsNotFound_WhenWasherDoesNotExist()
        {
            // Act
            var result = await _controller.getwasherbyid(99);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task AddWasher_ReturnsBadRequest_WhenDuplicateWasherIsAdded()
        {
            // Arrange
            var washer = new Washer { WasherId = 4, WasherName = "Tom", Email = "tom@example.com", PhoneNo = "5566778899", IsActive = false };
            _context.Washers.Add(washer);
            _context.SaveChanges();

            // Act
            var result = await _controller.AddWasher(washer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteWasher_ReturnsOk_WhenWasherIsDeleted()
        {
            // Arrange
            _context.Washers.Add(new Washer { WasherId = 7, WasherName = "Liam", Email = "liam@example.com", PhoneNo = "8899001122", IsActive = true });
            _context.SaveChanges();

            // Act
            var result = await _controller.Deletewasherbyid(7);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteWasher_ReturnsNotFound_WhenWasherDoesNotExist()
        {
            // Act
            var result = await _controller.Deletewasherbyid(10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task UpdateWasher_Returns_OkResult()
        {
            //Arrange
            _context.Washers.Add(new Washer { WasherId = 7, WasherName = "Liam", Email = "liam@example.com", PhoneNo = "8899001122", IsActive = true });
            _context.SaveChanges();

            //Act
            //test data
            Washer obj = new Washer();
            obj.WasherId = 7;
            obj.WasherName = "Yash";
            obj.Email = "liam@example.com";
            obj.PhoneNo = "8899001122";
            obj.IsActive = true;
            var result = await _controller.UpdateWasher(7, obj);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var res = await _context.Washers.FirstOrDefaultAsync(c => c.WasherId == obj.WasherId);
            Assert.AreEqual("Yash", res.WasherName);



        }

        [TestMethod]
        public async Task UpdateWasher_Returns_BadRequest()
        {
            //Arrange
            Washer obj = new Washer();
            obj.WasherId = 99;
            obj.WasherName = "Yash";
            obj.Email = "liam@example.com";
            obj.PhoneNo = "8899001122";
            obj.IsActive = true;


            //Act
            var result = await _controller.UpdateWasher (99,obj);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }

}