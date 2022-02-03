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
    public class CommentController : Controller
    {
        private ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _commentService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _commentService.FindByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/author_id/{id}")]
        public async Task<IActionResult> GetByAuthorIdAsync(int id)
        {
            var result = await _commentService.GetByAuthorIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get/author_period/{id}")]
        public async Task<IActionResult> GetByAuthorAndDateAsync(
            int id, 
            [FromQuery] DateTime from, 
            [FromQuery] DateTime to)
        {
            var result = await _commentService.GetByAuthorAndDateAsync(id, from, to);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(CommentDto comment)
        {
            await _commentService.AddAsync(comment);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(CommentDto comment)
        {
            await _commentService.UpdateAsync(comment);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _commentService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}