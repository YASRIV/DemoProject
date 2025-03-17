using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WasherApi.Process;
using Car_Wash_Library.Models;

namespace WasherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[CustomerAuthentication(Roles = "Admin,Washer")]
    public class WasherController : ControllerBase
    {
        private readonly WasherProcess _Process;
        public WasherController(WasherProcess Process)
        {
            _Process = Process;
        }

        [HttpPost("AddWasher")]
        public async Task<ActionResult> AddWasher([FromBody] Washer washer)
        {
            var status = await _Process.AddWasher(washer);
            if (status)
            {
                return Ok("Washer Added Successfully.");
            }
            return BadRequest("Invalid Washer Data");
        }

        [HttpPut("UpdateWasher/{id}")]
        public  async Task<ActionResult> UpdateWasher(int id,Washer washer)
        {
            var item = await _Process.UpdateWasher(id,washer);
            if(item)
            {
                return Ok("Update successfully");
            }
            return BadRequest("Invalid Washer Detail");
        }

        [HttpGet("GetWasherById/{id}")]

        public async Task<ActionResult<Washer>> getwasherbyid(int id)
        {
            var result = await _Process.GetWasherById(id);
            if (result == null)
            {
                return NotFound("No Washer Found");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteWasherById/{id}")]

        public async Task<ActionResult> Deletewasherbyid(int id)
        {
            var result = await _Process.DeleteWasher(id);
            if (!result)
            {
                return NotFound("No Washer Found");
            }
            return Ok("Deleted Successfully");
        }


        [HttpGet("GetWashers")]

        public async Task<ActionResult<IEnumerable<Washer>>> getwashers()
        {
            var result = await _Process.GetWashers();
            if (result.Count()==0)
            {
                return BadRequest("No Washer Found");
            }
            return Ok(result);
        }


        [HttpPost("AcceptWashRequest/{requestId}")]
        public async Task<IActionResult> AcceptWashRequest(int requestId, int washerId)
        {
            var result = await _Process.AcceptWashRequest(requestId, washerId);
            return result ? Ok("Wash request accepted.") : BadRequest("Failed to accept wash request.");
        }

        //For Manage WashRequest 
        [HttpPost("HandleWashRequest/{requestId}")]
        public async Task<IActionResult> HandleWashRequest(int requestId)
        {
            var result = await _Process.HandleWashRequest(requestId);
            return result ? Ok("Wash request is now in progress.") : BadRequest("Failed to handle wash request.");
        }

        [HttpPost("UpdateWashRequest/{requestId}")]
        public async Task<IActionResult> UpdateWashRequest(int requestId)
        {
            var result = await _Process.UpdateWashRequest(requestId);
            return result ? Ok("Wash request updated to completed.") : BadRequest("Failed to update wash request.");
        }
    }
}
