using System;
using Car_Wash_Library.CustomException;
using Car_Wash_Library.Models;
using CustomersAPI.Controllers;
using CustomersAPI.Process;
using CustomersAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarWashManagementTest
{
    [TestClass]
    public sealed class CustomerAndCarTests
    {
        private CustomerDbContext _context;
        private CustomerRepository _repository;
        private CarRepository _repo;
        private CustomerProcess _process;
        private CustomersController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up In-Memory Database
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new CustomerDbContext(options);
            _repository = new CustomerRepository(_context);
            _repo = new CarRepository(_context);
            _process = new CustomerProcess(_repo, _repository);
            _controller = new CustomersController(_process);

            _context.Cars.Add(new Car { CarId = 1, CustomerId = 100, CarModel = "Tesla", CarMake = "Model S", CarYear = 2022, IsActive = true });
            _context.Cars.Add(new Car { CarId = 2, CustomerId = 101, CarModel = "BMW", CarMake = "X5", CarYear = 2021, IsActive = false });
            _context.SaveChanges();
        }

        Customer customer = new Customer
        {

            Name = "Sourav Kourav",
            Email = "sourav.demo@gmail.com",
            PhoneNumber = "9858543210",
            Address = "xyz town ",
            City = "Bhopal",
            ZipCode = 466653,
            IsActive = true
        };

        [TestMethod]
        public async Task AddCustomer_ShouldReturnOkAndStoreInDatabase()
        {
            // Arrange: Create a new customer object
            var newCustomer = new Customer
            {
                Name = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Address = "123 Street",
                City = "New York",
                ZipCode = 10001,
                IsActive = true
            };

            // Act: Call the controller method
            var result = await _controller.PostCustomer(newCustomer);

            // Assert: Verify response status is 200 OK
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Customer Add Successfully", okResult.Value);

            // Assert: Check if the customer was actually added to the database
            var storedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == newCustomer.Email);
            Assert.IsNotNull(storedCustomer);
            Assert.AreEqual("John Doe", storedCustomer.Name);
            Assert.AreEqual("New York", storedCustomer.City);

            //
            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();

        }
        //For Unsuccessfull data
        [TestMethod]
        public async Task AddCustomer_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange: Create a customer with missing required fields
            var invalidCustomer = new Customer
            {
                CustomerId = 1,
                Name = "John Doe",
                Email = "", // Invalid empty email
                PhoneNumber = "1234567890",
                Address = "123 Street",
                City = "New York",
                ZipCode = 10001,
                IsActive = true
            };
            var Result = await _controller.PostCustomer(invalidCustomer);

            // Act: Call the controller method
            var result = await _controller.PostCustomer(invalidCustomer);

            // Assert: Verify response status is 400 BadRequest
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid Customer Data", badRequestResult.Value);

            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();
        }

        // TEst for InvalidDataInsertException 
        [TestMethod]
        [ExpectedException(typeof(InvalidDataInsertException))]
        public async Task AddCustomer_ShouldThrowInvalidDataInsertException_WhenInvalidCustomerData()
        {
            // Arrange: Add a customer but simulate a failure in update
            var customer = new Customer { Name = "Jane Doe", IsActive = false };

            // Act: Check for Exception
            var result = await _controller.PostCustomer(customer); // Non-existing ID

        }

        [TestMethod]
        public async Task GetCustomers_ShouldReturnOk_WhenCustomersDataExist()
        {
            // Arrange: Add test data to in-memory database
            var customers = new List<Customer>
    {
        new Customer { Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890", Address = "123 Street", City = "NY", ZipCode = 10001, IsActive = true },
        new Customer { Name = "Jane Doe", Email = "jane@example.com", PhoneNumber = "0987654321", Address = "456 Street", City = "LA", ZipCode = 90001, IsActive = true }
    };
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync(); // Ensure data is saved in database

            // Act: Call the controller method to get result
            var result = await _controller.GetCustomers();

            // Assert: Check response type of result/output
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            // Assert: Check returned data Value
            var returnedCustomers = okResult.Value as IEnumerable<Customer>;
            Assert.IsNotNull(returnedCustomers);
            Assert.AreEqual(2, returnedCustomers.Count()); // Expecting 2 customers

            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();
        }

        //TestFor_DataNotFound
        [TestMethod]
        public async Task GetCustomers_ShouldReturnNotFound_WhenNoCustomersDataExist()
        {
            // Arrange: Ensure database is empty
            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();

            // Act: Call the controller method
            var result = await _controller.GetCustomers();

            // Assert: Check response type
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        //Test for update of data successful.
        [TestMethod]
        public async Task UpdateCustomer_ShoudReturnOk_WhenCustomersDataUpdated()
        {
            //Arrange: add data to the database for update
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            customer.Email = "saket1232@gmail.com";

            //Act: Call Controller Update Method to get result
            var result = _controller.UpdateCustomer(customer.CustomerId, customer);

            //Assert: Check the Response of controller method
            var OkResult = result.Result as OkObjectResult;
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);

            //Assert: Data is updated in database or Not.
            var storedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            Assert.IsNotNull(storedCustomer);
            Assert.AreEqual("saket1232@gmail.com", storedCustomer.Email);

            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();
        }

        //Test for updateCustomer when update is fails.
        [TestMethod]
        public async Task UpdateCustomer_ShouldReturnBadRequest_WhenCustomersDataUpdateFails()
        {
            //Arrange: remove data from database so no data available for update.
            _context.Customers.RemoveRange(_context.Customers);
            await _context.SaveChangesAsync();

            // Act: Call the controller method to check result.
            var result = _controller.UpdateCustomer(customer.CustomerId, customer);

            // Assert: Check response type
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomerById_ShouldReturnOk_WhenCustomerFoundById()
        {
            //Arrange: Add customer object to the Database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            //Act: Call Controller Method GetCustomer(int id) 
            var result = _controller.GetCustomerById(customer.CustomerId);

            //Assert: Check the Response of above controller method
            var OkResult = result.Result as OkObjectResult;
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);


            //Assert: Data is updated in database or Not.
            var storedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            Assert.IsNotNull(storedCustomer);
            Assert.AreEqual("sourav.demo@gmail.com", storedCustomer.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(NoDataAvailableException))]
        public async Task GetCustomerById_ShouldReturnNotFound_WhenCustomerNotFoundById()
        {
            // Act
            var result = await _controller.ActivateCustomer(999); // Non-existing ID

        }

        [TestMethod]
        public async Task Delete_ShouldReturnOk_WhenCustomerDataRemove()
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            //Act : call delete method of controller
            var result = await _controller.DeleteCustomer(customer.CustomerId);

            //Assert : Check Response
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var OkResult = result as OkObjectResult;
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);

            //Assert: CHeck in DataBase 
            var storedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            Assert.IsNotNull(storedCustomer);
            Assert.IsTrue(storedCustomer.IsActive == false);
        }

        //For NotFound Status
        [TestMethod]
        public async Task Delete_ShouldReturnNotFound_WhenCustomerNotFound()
        {

            //Act : call delete method of controller
            var result = await _controller.DeleteCustomer(customer.CustomerId);

            //Assert : Check Response
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var OkResult = result as NotFoundResult;
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(404, OkResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCarsByCustomerId_ShouldReturnOk_WhenCarAvailableForCustomer()
        {
            // Arrange: Add test data to the in-memory database
            var customerId = 1;
            var cars = new List<Car>
    {
        new Car { CarId = 101, CustomerId = customerId, CarModel = "Toyota", CarMake = "Red",CarYear = 2020,IsActive=true },
        new Car { CarId = 102, CustomerId = customerId, CarModel = "Honda", CarMake = "Blue",CarYear=2020,IsActive=true }
    };

            _context.Cars.AddRange(cars);
            await _context.SaveChangesAsync();

            // Act: Call the controller method
            var result = await _controller.GetCarsByCustomerId(customerId);

            // Assert: Check if response is OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            // Assert: Verify the returned data
            var returnedCars = okResult.Value as IEnumerable<Car>;
            Assert.IsNotNull(returnedCars);
            Assert.AreEqual(2, returnedCars.Count()); // Expecting 2 cars
        }

        [TestMethod]
        public async Task GetCarsByCustomerId_ShouldReturnNotFound_WhenNoCarsExist()
        {
            // Arrange: Ensure no cars exist for a given customerId
            var customerId = 99; // Use an ID that is not in the database
            _context.Cars.RemoveRange(_context.Cars);
            await _context.SaveChangesAsync();

            // Act: Call the controller method
            var result = await _controller.GetCarsByCustomerId(customerId);

            // Assert: Check if response is NotFound
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("No cars found for this customer.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task ActivateCustomer_ShouldReturnOk_WhenActivationIsSuccessful()
        {
            //Arrange: Add Data to the DataBase
            customer.IsActive = false;
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            // Act
            var result = await _controller.ActivateCustomer(customer.CustomerId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Customer is active now...", okResult.Value);

            // Verify database update
            var cust = await _context.Customers.FindAsync(1);
            Assert.IsTrue(cust.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(NoDataAvailableException))]
        public async Task ActivateCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var result = await _controller.ActivateCustomer(999); // Non-existing ID

        }

        //Test for BadRequest
        [TestMethod]
        public async Task ActivateCustomer_ShouldReturnBadRequest_WhenActivationFails()
        {
            // Arrange: Add a customer but simulate a failure in update
            //var customerId = 2;
            //var customer = new Customer { CustomerId = customerId, Name = "Jane Doe", IsActive = false };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Simulating failure in UpdateCustomer by removing the customer before updating
            //_context.Customers.Remove(customer);
            //await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ActivateCustomer(customer.CustomerId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Customer is inactive.", badRequestResult.Value);
        }

        //========================TestForCar================================================
        [TestMethod]
        public async Task AddCar_ShouldReturnOk_WhenCarIsAdded()
        {
            // Arrange
            var newCar = new Car { CarId = 3, CustomerId = 102, CarModel = "Audi", CarMake = "A6", CarYear = 2020, IsActive = true };

            // Act
            var result = await _controller.PostCar(newCar);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Car Add Successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task AddCar_ShouldReturnBadRequest_WhenCarAlreadyExists()
        {
            // Arrange: Car with CarId 1 already exists in Setup()
            var existingCar = new Car { CarId = 1, CustomerId = 100, CarModel = "Tesla", CarMake = "Model S", CarYear = 2022, IsActive = true };

            // Act
            var result = await _controller.PostCar(existingCar);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid Customer Data", badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetCarById_ShouldReturnCar_WhenCarExists()
        {
            // Act
            var result = await _controller.GetCarById(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var car = okResult.Value as Car;
            Assert.IsNotNull(car);
            Assert.AreEqual(1, car.CarId);
        }

        [TestMethod]
        public async Task GetCarById_ShouldReturnBadRequest_WhenCarNotFound()
        {
            // Act
            var result = await _controller.GetCarById(99);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("No Car Found.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetCars_ShouldReturnOkWithList_WhenCarsExist()
        {
            // Act
            var result = await _controller.GetCars();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var cars = okResult.Value as List<Car>;
            Assert.IsNotNull(cars);
            Assert.IsTrue(cars.Count > 0);
        }

        [TestMethod]
        public async Task GetCars_ShouldReturnBadRequest_WhenNoCarsExist()
        {
            // Arrange: Remove all cars
            _context.Cars.RemoveRange(_context.Cars);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCars();

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("No Car Found.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task RemoveCar_ShouldReturnOk_WhenCarIsRemoved()
        {
            // Act
            var result = await _controller.RemoveCar(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Deletion successfull.", okResult.Value);
        }

        [TestMethod]
        public async Task RemoveCar_ShouldReturnBadRequest_WhenCarNotFound()
        {
            // Act
            var result = await _controller.RemoveCar(99);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Deletion unsuccessfull.", badRequestResult.Value);
        }

    }
}
