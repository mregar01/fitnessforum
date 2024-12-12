using System;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessapi.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly FitnessContext _context;
        private readonly ILogger<PostController> _logger;

        public PostController(FitnessContext context, ILogger<PostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddPost([FromBody] PostDto postDto)
        {
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

            var newPost = new Post
            {
                PostTypeId = PostType.Question,
                CreationDate = DateTimeOffset.Now,
                Score = 0,
                ViewCount = 0,
                Body = postDto.PostBody,
                OwnerUserId = ID,
                OwnerDisplayName = userName,
                Title = postDto.PostTitle,
                Tags = postDto.PostTags,
                AnswerCount = 0,
                CommentCount = 0,
                ContentLicense = "Null"
            };

            _context.Posts.Add(newPost);

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to add the post. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to add the post");
            }
        }
        [HttpPost("{id}/answers/add")]
        public async Task<IActionResult> AddResponse([FromBody] ResponseDto responseDto, int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }


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
            var newPost = new Post
            {
                PostTypeId = PostType.Answer,
                ParentId = id,
                CreationDate = DateTimeOffset.Now,
                Score = 0,
                ViewCount = 0,
                Body = responseDto.ResponseBody,
                OwnerUserId = ID,
                OwnerDisplayName = userName,
                AnswerCount = 0,
                CommentCount = 0,
                ContentLicense = "Null"
            };

            _context.Posts.Add(newPost);

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to add the answer. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to add the answer");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetPosts(int page = 1, int pageSize = 10)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            int skipCount = (page - 1) * pageSize;

            var posts = await _context.Posts
                .Where(p => p.PostTypeId == PostType.Question)
                .OrderByDescending(p => p.CreationDate)
                .Skip(skipCount)
                .Take(pageSize)
                .GroupJoin(
                    _context.Votes,
                    post => post.Id,
                    vote => vote.PostId,
                    (post, votes) => new { Post = post, Votes = votes }
                )
                .Select(x => new PostItem()
                {
                    Id = x.Post.Id,
                    Votes = x.Votes.Sum(vote =>
                        vote.VoteTypeID == VoteType.UpMod ? 1 :
                        vote.VoteTypeID == VoteType.DownMod ? -1 : 0),
                    AnswerCount = _context.Posts
                        .Count(p => p.PostTypeId == PostType.Answer && p.ParentId == x.Post.Id),
                    ViewCount = x.Post.ViewCount,
                    Tags = x.Post.Tags,
                    Title = x.Post.Title,
                    OwnerUserId = x.Post.OwnerUserId,
                    OwnerDisplayName = x.Post.OwnerDisplayName,
                    AcceptedAnswerId = x.Post.AcceptedAnswerId
                })
                .ToListAsync();

            return posts;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PostItemWithResponses>> GetPostWithResponses(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var postItem = new PostItem()
            {
                Id = post.Id,
                Votes = _context.Votes
                    .Where(vote => vote.PostId == id)
                    .Sum(vote =>
                        vote.VoteTypeID == VoteType.UpMod ? 1 :
                        vote.VoteTypeID == VoteType.DownMod ? -1 : 0),
                AnswerCount = _context.Posts
                    .Count(p => p.PostTypeId == PostType.Answer && p.ParentId == id),
                ViewCount = post.ViewCount,
                Tags = post.Tags,
                Title = post.Title,
                Body = post.Body,
                OwnerUserId = post.OwnerUserId,
                OwnerDisplayName = _context.Users
                    .Where(user => user.Id == post.OwnerUserId)
                    .Select(user => user.DisplayName)
                    .FirstOrDefault(),
                OwnerRep = _context.Users
                    .Where(user => user.Id == post.OwnerUserId)
                    .Select(user => user.Reputation)
                    .FirstOrDefault(),
                OwnerGoldBadges = _context.Badges
                    .Where(badge => badge.UserId == post.OwnerUserId)
                    .Sum(badge => badge.Class == BadgeType.Gold ? 1 : 0),
                OwnerSilverBadges = _context.Badges
                    .Where(badge => badge.UserId == post.OwnerUserId)
                    .Sum(badge => badge.Class == BadgeType.Silver ? 1 : 0),
                OwnerBronzeBadges = _context.Badges
                    .Where(badge => badge.UserId == post.OwnerUserId)
                    .Sum(badge => badge.Class == BadgeType.Bronze ? 1 : 0),
                CreationDate = post.CreationDate,
                Comments = await _context.Comments
                        .Where(c => c.PostId == post.Id)
                        .Select(c => new Comment()
                        {
                            Id = c.Id,
                            PostId = c.PostId,
                            Score = c.Score,
                            Text = c.Text,
                            CreationDate = c.CreationDate,
                            UserId = c.UserId,
                            UserDisplayName = _context.Users
                                .Where(user => user.Id == c.UserId)
                                .Select(user => user.DisplayName)
                                .FirstOrDefault()

                        })
                        .ToListAsync()
            };

            var postResponses = await _context.Posts
                .Where(p => p.PostTypeId == PostType.Answer && p.ParentId == id)
                .ToListAsync();

            var postResponseItems = new List<PostItem>();

            foreach (var response in postResponses)
            {
                var responseItem = new PostItem()
                {
                    Id = response.Id,
                    ParentId = post.Id,
                    Votes = _context.Votes
                        .Where(vote => vote.PostId == response.Id)
                        .Sum(vote =>
                            vote.VoteTypeID == VoteType.UpMod ? 1 :
                            vote.VoteTypeID == VoteType.DownMod ? -1 : 0),
                    AnswerCount = _context.Posts
                        .Count(answer => answer.PostTypeId == PostType.Answer && answer.ParentId == response.Id),
                    ViewCount = response.ViewCount,
                    Tags = response.Tags,
                    Title = response.Title,
                    Body = response.Body,
                    OwnerUserId = response.OwnerUserId,
                    CreationDate = response.CreationDate,
                    OwnerGoldBadges = _context.Badges
                        .Where(badge => badge.UserId == response.OwnerUserId)
                        .Sum(badge => badge.Class == BadgeType.Gold ? 1 : 0),
                    OwnerSilverBadges = _context.Badges
                        .Where(badge => badge.UserId == response.OwnerUserId)
                        .Sum(badge => badge.Class == BadgeType.Silver ? 1 : 0),
                    OwnerBronzeBadges = _context.Badges
                        .Where(badge => badge.UserId == response.OwnerUserId)
                        .Sum(badge => badge.Class == BadgeType.Bronze ? 1 : 0),
                    Comments = await _context.Comments
                        .Where(c => c.PostId == response.Id)
                        .Select(c => new Comment()
                        {
                            Id = c.Id,
                            PostId = c.PostId,
                            Score = c.Score,
                            Text = c.Text,
                            UserId = c.UserId,
                            CreationDate = c.CreationDate,
                            UserDisplayName = _context.Users
                                .Where(user => user.Id == c.UserId)
                                .Select(user => user.DisplayName)
                                .FirstOrDefault()
                        })
                        .ToListAsync()
                };

                var owner = _context.Users.FirstOrDefault(user => user.Id == response.OwnerUserId);
                responseItem.OwnerDisplayName = owner != null ? owner.DisplayName : string.Empty;

                var rep = _context.Users.FirstOrDefault(user => user.Id == response.OwnerUserId);
                responseItem.OwnerRep = rep != null ? rep.Reputation : (int?)null;


                postResponseItems.Add(responseItem);
            }

            postResponseItems = postResponseItems.OrderByDescending(item => item.Votes).ToList();

            var postItemWithResponses = new PostItemWithResponses()
            {
                PostItem = postItem,
                Responses = postResponseItems,
            };

            return postItemWithResponses;
        }

        [HttpGet("tag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetTags(string tagName, int page)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            int pageSize = 25;

            int skipCount = (page - 1) * pageSize;


            var posts = await _context.Posts
                .Where(p => p.Tags != null && p.Tags.Contains($"<{tagName}>"))
                .OrderByDescending(p => p.CreationDate)
                .Skip(skipCount)
                .Take(pageSize)
                .GroupJoin(
                    _context.Votes,
                    post => post.Id,
                    vote => vote.PostId,
                    (post, votes) => new { Post = post, Votes = votes }
                )
                .Select(x => new PostItem

                {
                    Id = x.Post.Id,
                    Votes = x.Votes.Sum(vote =>
                        vote.VoteTypeID == VoteType.UpMod ? 1 :
                        vote.VoteTypeID == VoteType.DownMod ? -1 : 0),
                    AnswerCount = _context.Posts
                        .Count(p => p.PostTypeId == PostType.Answer && p.ParentId == x.Post.Id),
                    ViewCount = x.Post.ViewCount,
                    Tags = x.Post.Tags,
                    Title = x.Post.Title,
                    OwnerUserId = x.Post.OwnerUserId,
                    OwnerDisplayName = x.Post.OwnerDisplayName
                })
                .ToListAsync();

            return posts;
        }
        [HttpPut("edit/{postId}")]
        public async Task<IActionResult> EditPost(int postId, [FromBody] PostDto postDto)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            post.Body = postDto.PostBody;
            post.Title = postDto.PostTitle;
            post.Tags = postDto.PostTags;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to edit the post. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to edit the post");
            }
        }
        [HttpPost("{postId}/increment")]
        public async Task<IActionResult> Upvote(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            var newVote = new Vote
            {
                PostId = postId,
                VoteTypeID = VoteType.UpMod,
                CreationDate = DateTimeOffset.Now
            };
            _context.Votes.Add(newVote);

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to upvote the post. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to upvote the post");
            }
        }


        [HttpPost("{postId}/decrement")]
        public async Task<IActionResult> DecrementVote(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            var newVote = new Vote
            {
                PostId = postId,
                VoteTypeID = VoteType.DownMod,
                CreationDate = DateTimeOffset.Now
            };

            _context.Votes.Add(newVote);

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var exceptionStackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                _logger.LogError(ex, "Failed to downvote the post. Error: {ExceptionMessage}\nStackTrace: {ExceptionStackTrace}",
                                    exceptionMessage, exceptionStackTrace);
                return BadRequest("Failed to downvote the post");
            }
        }
    }
}

