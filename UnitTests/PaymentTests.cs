using Car_Wash_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Controllers;
using PaymentsApi.PaymentProcess;
using PaymentsApi.PaymentRepository;


namespace UnitTests;

[TestClass]
public class PaymentTests
{
    private PaymentDbContext _context;
    private PayProcess _process;
    private PaymentRepository _repository;
    private PaymentController _controller;


    [TestInitialize]
    public void setup()
    {
        //Set - in Memory Use in Test 
        var option = new DbContextOptionsBuilder<PaymentDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new PaymentDbContext(option);
        _repository = new PaymentRepository(_context);
        _process = new PayProcess(_repository);
        _controller = new PaymentController(_process);

        //_context.Washers.Add(new Washer { WasherId = 1, WasherName = "Arjun Kumar", Email = "arjun.kumar@example.com", PhoneNo = "9876543210", IsActive = true });
        //_context.SaveChanges();
    }

    Payment service = new Payment
    { PaymentId = 1, TransactionId = 12345, TransactionDate = new DateTime(2025, 3, 1, 14, 30, 0), Amount = 500, PaymentMethod="card",PaymentStatus="success", IsActive = true };



    [TestMethod]
    public async Task AddPayment_ShouldReturnOk_WhenPaymentIsAdded()
    {
        //Arrange: Put Above Demo data for testing 

        //Act: Call AddWasher() of Washer Conrtroller
        var result = _controller.AddPayment(service);

        //Assert: Check the response of the above method

        var OkResult = result.Result as OkObjectResult;

        Assert.IsNotNull(OkResult);
        Assert.AreEqual(200, OkResult.StatusCode);

        //Assert: CHeck the above resposnse in DataBase
        var storedData = await _context.Payments.FirstOrDefaultAsync(c => c.PaymentId == service.PaymentId);
        Assert.AreEqual(500, storedData.Amount);
    }

    [TestMethod]
    public async Task GetPayments_ReturnsOk_WhenPaymentExist()
    {
        // Arrange
        _context.Payments.Add(service);
        _context.SaveChanges();

        // Act
        var result = await _controller.GetPayments();

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }
    [TestMethod]
    public async Task GetPayment_ReturnsBadRequest_WhenNoServiceExist()
    {
        _context.Payments.RemoveRange();
        // Act
        var result = await _controller.GetPayments();

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GetPaymentById_ReturnsOk_WhenPaymentExists()
    {
        // Arrange
        _context.Payments.Add(service);
        _context.SaveChanges();

        // Act
        var result = await _controller.GetPaymentById(1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetPaymentById_ReturnsNotFound_WhenPaymentDoesNotExist()
    {
        // Act
        var result = await _controller.GetPaymentById(99);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task AddPayment_ReturnsBadRequest_WhenDuplicatePaymentIsAdded()
    {
        // Arrange
        // var duplicateservice = new ServicePlan { ServiceId = 1, WasherName = "Tom", Email = "tom@example.com", PhoneNo = "5566778899", IsActive = false };
        _context.Payments.Add(service);
        _context.SaveChanges();

        // Act
        var result = await _controller.AddPayment(service);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task DeletePayment_ReturnsOk_WhenPaymentIsDeleted()
    {
        // Arrange
        _context.Payments.Add(service);
        _context.SaveChanges();

        // Act
        var result = await _controller.DeletePayment(1);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeletePayment_ReturnsNotFound_WhenPaymentDoesNotExist()
    {
        // Act
        var result = await _controller.DeletePayment(10);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task UpdatePayment_Returns_OkResult()
    {
        //Arrange
        _context.Payments.Add(service);
        _context.SaveChanges();

        //Act
        //test data
        Payment obj = new Payment();
        obj.PaymentId = 1;
        obj.TransactionId = 1335;
        obj.TransactionDate = new DateTime(2025, 3, 1, 14, 30, 0);
        obj.Amount = 500;
        obj.PaymentMethod = "card";
        obj.PaymentStatus = "success";
        obj.IsActive = true;
        var result = await _controller.UpdatePayment(1, obj);

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var res = await _context.Payments.FirstOrDefaultAsync(c => c.PaymentId == obj.PaymentId);
        Assert.AreEqual(1335, res.TransactionId);



    }

    [TestMethod]
    public async Task UpdatePayment_Returns_BadRequest()
    {
        //Arrange
        Payment obj = new Payment();
        obj.PaymentId = 99;
        obj.TransactionId = 1335;
        obj.TransactionDate = new DateTime(2025, 3, 1, 14, 30, 0);
        obj.Amount = 500;
        obj.PaymentMethod = "card";
        obj.PaymentStatus = "success";
        obj.IsActive = true;


        //Act
        var result = await _controller.UpdatePayment(99, obj);

        //Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
  
}
