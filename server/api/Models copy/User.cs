using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fitnessapi.Models
{
	public class User
	{
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public int? Reputation { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public string? DisplayName { get; set; }
        public DateTimeOffset? LastAccessDate { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }
        public string? AboutMe { get; set; }
        public int? Views { get; set; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }
    }
}

