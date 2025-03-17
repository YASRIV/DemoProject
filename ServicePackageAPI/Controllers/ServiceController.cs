using Car_Wash_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicePackageAPI.Interface;
using ServicePackageAPI.ServiceProcess;

namespace ServicePackageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly Process _Process;
        public ServiceController(Process Process)
        {
            _Process = Process;
        }

        [HttpPost("AddService")]
        public async Task<ActionResult> AddService([FromBody] ServicePlan service)
        {
            var status = await _Process.AddService(service);
            if (status)
            {
                return Ok("Service Added Successfully.");
            }
            return BadRequest("Invalid Service Data");
        }

        [HttpPut("UpdateService/{id}")]
        public async Task<ActionResult> UpdateService(int id, ServicePlan service)
        {
            var item = await _Process.UpdateService(id,service);
            if (item)
            {
                return Ok("Update successfully");
            }
            return BadRequest("Invalid Service Detail");
        }

        [HttpGet("GetServiceById/{id}")]

        public async Task<ActionResult<ServicePlan>> getservicebyid(int id)
        {
            var result = await _Process.GetServiceById(id);
            if (result == null)
            {
                return NotFound("No Service Found");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteServiceById/{id}")]

        public async Task<ActionResult> DeleteServicebyid(int id)
        {
            var result = await _Process.DeleteService(id);
            if (!result)
            {
                return NotFound("No Service Found");
            }
            return Ok("Deleted Successfully");
        }


        [HttpGet("GetServices")]

        public async Task<ActionResult<IEnumerable<ServicePlan>>> getservice()
        {
            var result = await _Process.GetServices();
            if (result.Count() == 0)
            {
                return BadRequest("No Service Found");
            }
            return Ok(result);
        }
    }
}
