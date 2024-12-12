using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fitnessapi.Models
{
	public class Comment
	{
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        public int? Score { get; set; }
        public string? Text { get; set; }
        public int? UserId { get; set; }
        public string? UserDisplayName { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? ContentLicense { get; set; }
    }
}