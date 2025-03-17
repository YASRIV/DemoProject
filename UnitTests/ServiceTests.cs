using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Wash_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePackageAPI.Controllers;
using ServicePackageAPI.ServiceProcess;
using ServicePackageAPI.ServicePackageRepository;


namespace UnitTests
{
    [TestClass]
    public class ServiceTests
    {

        private ServicePackageDbContext _context;
        private Process _process;
        private ServicePackageRepository _repository;
        private ServiceController _controller;


        [TestInitialize]
        public void setup()
        {
            //Set - in Memory Use in Test 
            var option = new DbContextOptionsBuilder<ServicePackageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ServicePackageDbContext(option);
            _repository = new ServicePackageRepository(_context);
            _process = new Process(_repository);
            _controller = new ServiceController(_process);

            //_context.Washers.Add(new Washer { WasherId = 1, WasherName = "Arjun Kumar", Email = "arjun.kumar@example.com", PhoneNo = "9876543210", IsActive = true });
            //_context.SaveChanges();
        }

        ServicePlan service = new ServicePlan
        { ServiceId = 2, ServiceName = "Car Wash", ServiceDescription= "Exterior", Price = 500, IsActive = true };
       


        [TestMethod]
        public async Task AddService_ShouldReturnOk_WhenServiceIsAdded()
        {
            //Arrange: Put Above Demo data for testing 

            //Act: Call AddWasher() of Washer Conrtroller
            var result = _controller.AddService(service);

            //Assert: Check the response of the above method

            var OkResult = result.Result as OkObjectResult;

            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);

            //Assert: CHeck the above resposnse in DataBase
            var storedData = await _context.Services.FirstOrDefaultAsync(c => c.ServiceName == service.ServiceName);
            Assert.AreEqual("Car Wash", storedData.ServiceName);
        }

        [TestMethod]
        public async Task GetService_ReturnsOk_WhenServiceExist()
        {
            // Arrange
            _context.Services.Add(service);
            _context.SaveChanges();

            // Act
            var result = await _controller.getservice();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task GetService_ReturnsBadRequest_WhenNoServiceExist()
        {
            _context.Services.RemoveRange();
            // Act
            var result = await _controller.getservice();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetServiceById_ReturnsOk_WhenserviceExists()
        {
            // Arrange
            _context.Services.Add( service);
            _context.SaveChanges();

            // Act
            var result = await _controller.getservicebyid(2);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetServiceById_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            // Act
            var result = await _controller.getservicebyid(99);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task AddService_ReturnsBadRequest_WhenDuplicateServiceIsAdded()
        {
            // Arrange
           // var duplicateservice = new ServicePlan { ServiceId = 1, WasherName = "Tom", Email = "tom@example.com", PhoneNo = "5566778899", IsActive = false };
            _context.Services.Add(service);
            _context.SaveChanges();

            // Act
            var result = await _controller.AddService(service);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteService_ReturnsOk_WhenServiceIsDeleted()
        {
            // Arrange
            _context.Services.Add(service);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteServicebyid(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteWasher_ReturnsNotFound_WhenWasherDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteServicebyid(10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task UpdateService_Returns_OkResult()
        {
            //Arrange
            _context.Services.Add(service);
            _context.SaveChanges();

            //Act
            //test data
            ServicePlan obj = new ServicePlan();
            obj.ServiceId = 2;
            obj.ServiceName = "tirewash";
            obj.ServiceDescription = "fullroundwash";
            obj.Price = 500;
            obj.IsActive = true;
            var result = await _controller.UpdateService(2, obj);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var res = await _context.Services.FirstOrDefaultAsync(c => c.ServiceId == obj.ServiceId);
            Assert.AreEqual("tirewash", res.ServiceName);



        }

        [TestMethod]
        public async Task UpdateService_Returns_BadRequest()
        {
            //Arrange
            ServicePlan obj = new ServicePlan();
            obj.ServiceId = 99;
            obj.ServiceName = "tirewash";
            obj.ServiceDescription = "fullroundwash";
            obj.Price = 500;
            obj.IsActive = true;


            //Act
            var result = await _controller.UpdateService(99, obj);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
