using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fitnessapi.Models
{
	public class Badge
	{
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset? Date { get; set; }
        public BadgeType Class { get; set; }
        public int? TagBased { get; set; }
    }
}

