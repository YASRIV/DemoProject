using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentsApi.PaymentInterface;
using Car_Wash_Library.Models;
using PaymentsApi.PaymentProcess;
using Car_Wash_Library.DTOClass;

namespace PaymentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PayProcess _process;
    

    public PaymentController(PayProcess process)
        {
            _process = process;
        }

        [HttpGet("GetPayments")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var result = await _process.GetPayments();
            if (result.Count() == 0)
            {
                return BadRequest("No Payments Found");
            }
            return Ok(result);
        }

        [HttpGet("(GetPaymentById/{id})")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {

            var result = await _process.GetPaymentById(id);
            if (result == null)
            {
                return NotFound("No Payment Found");
            }
            return Ok(result);
        }

        [HttpPost("AddPayment")]
        public async Task<ActionResult> AddPayment(Payment payment)
        {
            var status = await _process.AddPayment(payment);
            if (status)
            {
                return Ok("Service Added Successfully.");
            }
            return BadRequest("Invalid Service Data");
        }

        [HttpPut("(UpdatePayment/{id})")]
        public async Task<IActionResult> UpdatePayment(int id, Payment payment)
        {
            var item = await _process.UpdatePayment(id, payment);
            if (item)
            {
                return Ok("Update successfully");
            }
            return BadRequest("Invalid payment Detail");
        }

        [HttpDelete("(DeletePayment/{id})")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var result = await _process.DeletePayment(id);
            if (!result)
            {
                return NotFound("No Service Found");
            }
            return Ok("Deleted Successfully");
        }
        [HttpPut(template: "ProcessPayment")]
        public async Task<IActionResult> ProcessPayment(PaymentRequest request)
        {
            try
            {
                var isActivate = await _process.ProcessPayment(request);
                if (isActivate.IsSuccess) return Ok(isActivate);
                return BadRequest(isActivate.Message);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


    }

}
