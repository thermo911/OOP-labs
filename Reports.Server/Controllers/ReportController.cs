using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reports.Services.Services;
using Reports.Shrd.Dto;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private IReportService _reportService;

        public ReportController(IReportService ReportService)
        {
            _reportService = ReportService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get/chief_id/{id}")]
        public async Task<IActionResult> GetByChiefIdInPeriodAsync(
            int chiefId, 
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService.GetByChiefIdAsync(chiefId, from, to);
            return Ok(result);
        }

        [HttpGet("get/new")]
        public IActionResult GetNew()
        {
            return Ok(new ReportDto());
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _reportService.FindByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(ReportDto report)
        {
            await _reportService.AddAsync(report);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(ReportDto report)
        {
            await _reportService.UpdateAsync(report);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await _reportService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}