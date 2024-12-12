namespace fitnessapi.Models
{
	public class SearchResult
	{
		public int Id { get; set; }
		public string? Body { get; set; }
		public string? Title { get; set; }
		public string? Tags { get; set; }
		public int? VoteCount { get; set; }
		public int? ViewCount { get; set; }
		public int QuestionId { get; set; }
        public int PostTypeId { get; set; }
		public int? AcceptedAnswerId { get; set; }
		public int? AnswerCount { get; set; }
		public int? IsAcceptedAnswer { get; set; }
		public int? OwnerUserId { get; set; }
    }
}

