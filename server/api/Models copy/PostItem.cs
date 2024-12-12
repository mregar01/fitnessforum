namespace fitnessapi.Models
{
	public class PostItem
	{
		public int Id { get; set; }
        public int? Score { get; set; }
        public int? Votes { get; set; }
        public int? AnswerCount { get; set; }
        public int? ViewCount { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? ParentTitle { get; set; }
        public string? ParentTags { get; set; }
        public string? Body { get; set; }
        public PostType PostTypeId { get; set; }
        public int? OwnerUserId { get; set; }
        public string? OwnerDisplayName { get; set; }
        public int? OwnerRep { get; set; }
        public int? OwnerGoldBadges { get; set; }
        public int? OwnerSilverBadges { get; set; }
        public int? OwnerBronzeBadges { get; set; }
        public int? ParentId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public List<Comment>? Comments { get; set; }
        public int? AcceptedAnswerId { get; set; }
    }
}