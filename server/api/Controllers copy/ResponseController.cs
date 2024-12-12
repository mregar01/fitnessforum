using System;
using Azure.Core;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace fitnessapi.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class ResponseController : ControllerBase
	{
        private readonly FitnessContext _context;
        public readonly ILogger<ResponseController> _logger;

        public ResponseController(FitnessContext context, ILogger<ResponseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpPut("edit/{postId}")]
        public async Task<IActionResult> EditResponse(int postId, [FromBody] ResponseDto responseDto)
        {
            var response = await _context.Posts.FindAsync(postId);

            if (response == null)
            {
                return NotFound();
            }

            response.Body = responseDto.ResponseBody;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to edit the answer. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to edit the answer");
            }
        }
    }
}

