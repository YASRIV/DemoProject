using Car_Wash_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderDetailsApi.Interface;
using OrderDetailsApi.OrderProcess;

namespace OrderDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrdProcess _Process;

        public OrderController(OrdProcess Process)
        {
            _Process = Process;
        }

        //[HttpPost("AddOrder")]
        //public async Task<ActionResult> AddOrder([FromBody] Order order)
        //{
        //    var status = await _Process.AddOrder(order);
        //    if (status)
        //    {
        //        return Ok("Order Added Successfully.");
        //    }
        //    return BadRequest("Invalid Order Data");
        //}

        [HttpPost("BookOrder")]
        public async Task<IActionResult> BookOrder(int customerId, List<int> serviceIds)
        {
            try
            {
                var result = await _Process.BookOrder(customerId, serviceIds);
                if (result == null)
                    return BadRequest("Order booking failed.");

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"External Service Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }




        [HttpPost("GenerateWashRequest/{orderId}")]
        public async Task<IActionResult> GenerateWashRequest(int orderId)
        {
            try
            {
                var success = await _Process.GenerateWashRequest(orderId);
                if (!success)
                    return BadRequest("Failed to generate wash request.");

                return Ok("Wash request generated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost("MakePayment/{orderId}")]
        public async Task<IActionResult> MakePayment(int orderId)
        {
            try
            {
                var paymentSuccess = await _Process.MakePayment(orderId);
                if (!paymentSuccess)
                    return BadRequest("Payment failed.");

                return Ok("Payment successful.");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"External Payment Service Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("ViewCustomerOrders/{customerId}")]
        public async Task<IActionResult> ViewCustomerOrders(int customerId)
        {
            try
            {
                var orders = await _Process.ViewCustomerOrders(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("ViewWasherOrders/{washerId}")]
        public async Task<IActionResult> ViewWasherOrders(int washerId)
        {
            try
            {
                var orders = await _Process.ViewWasherOrders(washerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }












        [HttpGet("GetOrderById/{id}")]

        public async Task<ActionResult<Order>> getorderbyid(int id)
        {
            var result = await _Process.GetOrderById(id);
            if (result == null)
            {
                return NotFound("No Order Found");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteOrderById/{id}")]

        public async Task<ActionResult> DeleteOrderbyid(int id)
        {
            var result = await _Process.DeleteOrder(id);
            if (!result)
            {
                return NotFound("No Order Found");
            }
            return Ok("Deleted Successfully");
        }


        [HttpGet("GetOrders")]

        public async Task<ActionResult<IEnumerable<Order>>> getorders()
        {
            var result = await _Process.GetOrders();
            if (result.Count() == 0)
            {
                return BadRequest("No Order Found");
            }
            return Ok(result);
        }

        //ServiceOrder



        [HttpPost("AddServiceOrder")]
        public async Task<ActionResult> AddServiceOrder([FromBody] ServiceOrder serviceorder)
        {
            var status = await _Process.AddServiceOrder(serviceorder);
            if (status)
            {
                return Ok("ServiceOrder Added Successfully.");
            }
            return BadRequest("Invalid ServiceOrder Data");
        }

        [HttpPut("UpdateServiceOrder/{id}")]
        public async Task<ActionResult> UpdateServiceOrder(int id, ServiceOrder serviceorder)
        {
            var item = await _Process.UpdateServiceOrder(id, serviceorder);
            if (item)
            {
                return Ok("Update successfully");
            }
            return BadRequest("Invalid ServiceOrder Detail");
        }

        [HttpGet("GetServiceOrderById/{id}")]

        public async Task<ActionResult<ServiceOrder>> getserviceorderbyid(int id)
        {
            var result = await _Process.GetServiceOrderById(id);
            if (result == null)
            {
                return NotFound("No ServiceOrder Found");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteServiceOrderById/{id}")]

        public async Task<ActionResult> DeleteServiceOrderbyid(int id)
        {
            var result = await _Process.DeleteServiceOrder(id);
            if (!result)
            {
                return NotFound("No ServiceOrder Found");
            }
            return Ok("Deleted Successfully");
        }


        [HttpGet("GetServiceOrders")]

        public async Task<ActionResult<IEnumerable<ServiceOrder>>> getserviceorders()
        {
            var result = await _Process.GetServiceOrders();
            if (result.Count() == 0)
            {
                return BadRequest("No ServiceOrder Found");
            }
            return Ok(result);
        }



        //WashRequest


        [HttpPost("AddWashRequest")]
        public async Task<ActionResult> AddWashRequest([FromBody] WashRequest washRequest)
        {
            var status = await _Process.AddWashRequest(washRequest);
            if (status)
            {
                return Ok("WashRequest Added Successfully.");
            }
            return BadRequest("Invalid WasRequest Data");
        }

        //[HttpPut("UpdateWashRequest/{id}")]
        //public async Task<ActionResult> UpdateWashRequest(int id)
        //{
        //    var item = await _Process.UpdateWashRequest(id);
        //    if (item)
        //    {
        //        return Ok("Update successfully");
        //    }
        //    return BadRequest("Invalid WashRequest Detail");
        //}

        [HttpGet("GetWashRequestById/{id}")]

        public async Task<ActionResult<WashRequest>> getWashRequestbyid(int id)
        {
            var result = await _Process.GetWashRequestById(id);
            if (result == null)
            {
                return NotFound("No WashRequest Found");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteWashRequestById/{id}")]

        public async Task<ActionResult> DeleteWashRequestbyid(int id)
        {
            var result = await _Process.DeleteWashRequest(id);
            if (!result)
            {
                return NotFound("No WashRequest Found");
            }
            return Ok("Deleted Successfully");
        }


        [HttpGet("GetWashRequests")]

        public async Task<ActionResult<IEnumerable<WashRequest>>> getwashrequest()
        {
            var result = await _Process.GetWashRequests();
            if (result.Count() == 0)
            {
                return BadRequest("No WashRequest Found");
            }
            return Ok(result);
        }


        // Washrequest
        //[HttpGet("GetWashRequestsg")]
        //public async Task<IActionResult> GetAllWashRequests()
        //{
        //    var requests = await _Process.GetWashRequests();
        //    return Ok(requests);
        //}

        //[HttpGet("GetWashRequestById/{requestId}")]
        //public async Task<IActionResult> GetWashRequestById(int requestId)
        //{
        //    var request = await _Process.GetWashRequestById(requestId);
        //    return request != null ? Ok(request) : NotFound("Wash request not found.");
        //}

        [HttpPut("AcceptWashRequest/{requestId}")]
        public async Task<IActionResult> AcceptWashRequest(int requestId, int washerId)
        {
            var result = await _Process.AcceptWashRequest(requestId, washerId);
            return result ? Ok("Request accepted.") : BadRequest("Failed to accept request.");
        }

        [HttpPut("HandleWashRequest/{requestId}")]
        public async Task<IActionResult> HandleWashRequest(int requestId)
        {
            var result = await _Process.HandleWashRequest(requestId);
            return result ? Ok("Request is now in progress.") : BadRequest("Failed to handle request.");
        }

        [HttpPut("UpdateWashRequestStatus/{requestId}")]
        public async Task<IActionResult> UpdateWashRequest(int requestId)
        {
            var result = await _Process.UpdateWashRequest(requestId);
            return result ? Ok("Request updated to completed.") : BadRequest("Failed to update request.");
        }

        [HttpGet("Invoice/{requestId}")]
        public async Task<IActionResult> GetInvoice(int requestId)
        {
            var invoice = await _Process.GenerateInvoice(requestId);
            return invoice != null ? Ok(invoice) : NotFound("Invoice details not found.");
        }




    }
}
