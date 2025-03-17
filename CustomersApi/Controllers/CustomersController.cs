using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Car_Wash_Library.Models;
using CustomersAPI.Interface;
using CustomersAPI.Process;
namespace CustomersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[CustomerAuthentication(Roles = "Admin,Customer")]
    public class CustomersController : ControllerBase
    {
       
        private readonly CustomerProcess _process;
       
        public CustomersController(CustomerProcess process)
        {
            _process = process;
        }

        #region CarOperation
        [HttpPost("addcar")]
        public async Task<ActionResult<Customer>> PostCar([FromBody] Car car)
        {
            var status = await _process.AddCar(car);
            if (status)
            {
                
                    return Ok("Car Add Successfully.");
            }

            return BadRequest("Invalid Customer Data");
        }

        [HttpPut("UpdateCar/{id}")]
        public async Task<ActionResult> UpdateCar(int id, Car car)
        {
            var item = await _process.UpdateCar(car);
            if (item)
            {
                return Ok("Update Successful.");
            }
            return BadRequest("Update Fails.");
        }

        [HttpDelete("Remove/{carId}")]
        public async Task<ActionResult> RemoveCar(int carId)
        {
            var item = await _process.RemoveCar(carId);
            if (item)
            {
                return Ok("Deletion successfull.");
            }
            return BadRequest("Deletion unsuccessfull.");
        }
        [HttpGet("CarById/{carId}")]
        public async Task<ActionResult<Car>> GetCarById(int carId)
        {
            var item = await _process.GetCarById(carId);
            if(item == null)
            {
                return BadRequest("No Car Found.");
            }
            return Ok(item);
        }

        [HttpGet("GetCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var item = await _process.GetCars();
            if(item.Count()==0)
            {
                return BadRequest("No Car Found.");
            }
            return Ok(item);
        }
        #endregion

        #region CustomerOperation
        // GET: api/Customers/GetCustomers
        [HttpGet("GetCustomers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var item =await _process.GetCustomers();
            if (item.Count() == 0)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // GET: api/Customers/GetCustomer/5
        [HttpGet("GetCustomer{id}")]
        public async Task<ActionResult> GetCustomerById(int id)
        {
            var customer = await _process.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound("");
            }

            return Ok(customer);
        }

        [HttpPut("ActivateCustomer/{id}")]
        public async Task<IActionResult> ActivateCustomer(int id)
        {
            var item = await _process.ActivateCustomer(id);
            if (item)
            {
                return Ok("Customer is active now...");
            }
            return BadRequest("Customer is inactive.");
        }

        // PUT: api/Customers/UpdateCustomer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCustomer{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            var item = await _process.UpdateCustomer(customer);
            if (item)
            {
                return Ok("Update Successful");
            }
            return BadRequest("Update Fails");
        }

            // POST: api/Customers/AddCustomer
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost("AddCustomer")]
        public async Task<ActionResult<Customer>> PostCustomer([FromBody]Customer customer)
        {
            var status = await _process.AddCustomer(customer);
            if (status)
            {
                return Ok("Customer Add Successfully");
            }

            return BadRequest("Invalid Customer Data");
        }
        //GET: api/Customers/CarByCustomerId/2
        [HttpGet("CarsBycustomerId/{id}")]
        public async Task<IActionResult> GetCarsByCustomerId(int id)
        {
            var item = await _process.GetCarsByCustomerId(id);
            if (item.Count() == 0)
            {
                return NotFound("No cars found for this customer.");
            }
            return Ok(item);
        }

        // DELETE: api/Customers/DeleteCustomer/5
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _process.RemoveCustomer(id);
            if (customer)
            {
                return Ok("CustomerRemove");
            }
            return NotFound();
        }
        #endregion

    }
}
