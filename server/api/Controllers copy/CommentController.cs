using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace fitnessapi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly FitnessContext _context;
        private readonly ILogger<CommentController> _logger;

        public CommentController(FitnessContext context, ILogger<CommentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
        {
            var post = await _context.Posts.FindAsync(commentDto.PostId);

            var ID = 28201;
            var userName = "MaxN'Motion";
            if (Request != null)
            {
                string? idstring = Request?.Cookies["Id"];
                if (idstring != null)
                {
                    ID = int.Parse(idstring);
                }

                userName = Request?.Cookies["Name"];
            }


            if (post == null)
            {
                return NotFound();
            }

            var newComment = new Comment
            {
                PostId = commentDto.PostId,
                Score = 0,
                Text = commentDto.CommentBody,
                UserId = ID,
                UserDisplayName = userName,
                CreationDate = DateTimeOffset.Now,
                ContentLicense = "Null"
            };

            _context.Comments.Add(newComment);

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to add the comment. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to add the comment");
            }
        }
    }
}

