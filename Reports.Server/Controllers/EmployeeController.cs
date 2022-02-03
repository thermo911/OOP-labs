using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reports.Services.Services;
using Reports.Shrd.Dto;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        // GET
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeService.FindByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/chied_id/{id}")]
        public async Task<IActionResult> GetByChiefIdAsync(int id)
        {
            var result = await _employeeService.GetByChiefIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(EmployeeDto employee)
        {
            await _employeeService.AddAsync(employee);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(EmployeeDto employee)
        {
            await _employeeService.UpdateAsync(employee);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _employeeService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}