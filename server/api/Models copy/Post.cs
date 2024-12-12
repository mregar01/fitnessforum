using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace fitnessapi.Models
{
	public class Post
	{
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public PostType PostTypeId { get; set; }
        public int? AcceptedAnswerId { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public DateTimeOffset CreationDate { get; set; }
        public int? Score { get; set; }
        public int? ViewCount { get; set; }
        public string? Body { get; set; }
        public int? OwnerUserId { get; set; }
        public string? OwnerDisplayName { get; set; }
        public int? LastEditorUserId { get; set; }
        public string? LastEditorDisplayName { get; set; }
        public DateTimeOffset? LastEditDate { get; set; }
        public DateTimeOffset? LastActivityDate { get; set; }
        public string? Title { get; set; }
        public string? Tags { get; set; }
        public int? AnswerCount { get; set; }
        public int? CommentCount { get; set; }
        public int? FavoriteCount { get; set; }
        public DateTimeOffset? ClosedDate { get; set; }
        public DateTimeOffset? CommunityOwnedDate { get; set; }
        [Required]
        public string ContentLicense { get; set; } = string.Empty;
    }
}

