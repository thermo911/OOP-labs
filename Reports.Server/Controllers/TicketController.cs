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
    public class TicketController : Controller
    {
        private ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _ticketService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _ticketService.FindByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/assignee_id/{id}")]
        public async Task<IActionResult> GetByAssigneeIdAsync(int id)
        {
            var result = await _ticketService.GetByAssigneeIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/author_id/{id}")]
        public async Task<IActionResult> GetByAuthorIdAsync(int id)
        {
            var result = await _ticketService.GetByAuthorIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/updated_by_id/{id}")]
        public async Task<IActionResult> GetUpdatedByEmployeeIdAsync(int id)
        {
            var result = await _ticketService.GetUpdatedByEmployeeIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/open_date")]
        public async Task<IActionResult> GetByOpenDateAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _ticketService.GetByOpenDateAsync(from, to);
            return Ok(result);
        }
        
        [HttpGet("get/update_date")]
        public async Task<IActionResult> GetByUpdateDateAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _ticketService.GetByUpdateDateAsync(from, to);
            return Ok(result);
        }

        [HttpGet("get/resolved_in_period")]
        public async Task<IActionResult> GetResolvedInPeriod(DateTime from, DateTime to)
        {
            var result = await _ticketService.GetResolvedInPeriod(from, to);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(TicketDto ticket)
        {
            await _ticketService.AddAsync(ticket);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(TicketDto ticket)
        {
            await _ticketService.UpdateAsync(ticket);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            await _ticketService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}