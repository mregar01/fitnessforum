using System;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessapi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FitnessContext _context;

        public UserController(FitnessContext context)
        {
            _context = context;
        }

        

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserItem>> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            var goldBadges = await _context.Badges
               .Where(badge => badge.UserId == user.Id && badge.Class == BadgeType.Gold)
               .Select(badge => new BadgeItem()
               {
                   Name = badge.Name,
                   Date = badge.Date,
                   Id = badge.Id
               })
               .ToListAsync();


            var silverBadges = await _context.Badges
                .Where(badge => badge.UserId == user.Id && badge.Class == BadgeType.Silver)
                .Select(badge => new BadgeItem()
                {
                    Name = badge.Name,
                    Date = badge.Date,
                    Id = badge.Id
                })
                .ToListAsync();

            var bronzeBadges = await _context.Badges
                .Where(badge => badge.UserId == user.Id && badge.Class == BadgeType.Bronze)
                .Select(badge => new BadgeItem()
                {
                    Name = badge.Name,
                    Date = badge.Date,
                    Id = badge.Id
                })
                .ToListAsync();

            var answers = _context.Posts
                .Where(post => post.PostTypeId == PostType.Answer)
                .Sum(post => (post.OwnerUserId == user.Id) ? 1 : 0);

            var questions = _context.Posts
                .Where(post => post.PostTypeId == PostType.Question)
                .Sum(post => (post.OwnerUserId == user.Id) ? 1 : 0);



            var posts = await _context.Posts
                .Where(post => post.OwnerUserId == user.Id)
                .Select(post => new PostItem()
                {
                    Id = post.Id,
                    Votes = _context.Votes
                        .Where(vote => vote.PostId == post.Id)
                        .Sum(vote =>
                            vote.VoteTypeID == VoteType.UpMod ? 1 :
                            vote.VoteTypeID == VoteType.DownMod ? -1 : 0),
                    Title = post.Title,
                    ParentTitle = _context.Posts
                        .Where(post1 => post1.PostTypeId == PostType.Question && post1.Id == post.ParentId)
                        .Select(post1 => post1.Title)
                        .FirstOrDefault(),
                    ParentId = post.ParentId,
                    PostTypeId = post.PostTypeId,
                    CreationDate = post.CreationDate
                })
                .ToListAsync();

            posts = posts.OrderByDescending(item => item.Votes).ToList();

            var finalUser = new UserItem()
            {
                Id = user.Id,
                Reputation = user.Reputation,
                Answers = answers,
                Questions = questions,
                CreationDate = user.CreationDate,
                DisplayName = user.DisplayName,
                LastAccessDate = user.LastAccessDate,
                WebsiteUrl = user.WebsiteUrl,
                Location = user.Location,
                AboutMe = user.AboutMe,
                Views = user.Views,
                UpVotes = user.UpVotes,
                DownVotes = user.DownVotes,
                GoldBadges = goldBadges,
                SilverBadges = silverBadges,
                BronzeBadges = bronzeBadges,
                Posts = posts
            };

            return finalUser;
        }
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetTopUsers()
        {
            var topUsers = await _context.Users
                .OrderByDescending(user => user.Reputation)
                .Take(10)
                .Select(user => new UserItem
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName
                })
                .ToListAsync();

            var maxnmotion = await _context.Users
                .Where(user => user.Id == 28201)
                .Select(user => new UserItem
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName
                })
                .FirstOrDefaultAsync();

            if (maxnmotion != null)
            {
                topUsers.Add(maxnmotion);
            }

            return topUsers;
        }
    }
}