using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitnessapi.Models
{
	public class UserItem
	{
        public int Id { get; set; }
        public int? Reputation { get; set; }
        public int? Answers { get; set; }
        public int? Questions { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string? DisplayName { get; set; }
        public DateTimeOffset? LastAccessDate { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }
        public string? AboutMe { get; set; }
        public int? Views { get; set; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }
        public List<BadgeItem>? GoldBadges { get; set; }
        public List<BadgeItem>? SilverBadges { get; set; }
        public List<BadgeItem>? BronzeBadges { get; set; }
        public List<PostItem>? Posts { get; set; }
    }
}

