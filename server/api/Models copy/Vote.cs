//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace fitnessapi.Models
//{
//	public class Vote
//	{
//        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//        [Required]
//        public int PostId { get; set; }
//        public VoteType VoteTypeID { get; set; }

//    }
//}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fitnessapi.Models
{
    public class Vote
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        public VoteType VoteTypeID { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
