using System;
using System.Collections.Generic;

namespace fitnessapi.Models
{
	public class SearchItems
	{
		public int? UserId { get; set; }
		public int? MinScore { get; set; }
		public int? NumAnswers { get; set; }
        public IReadOnlyList<string>? SearchWords { get; set; }
        public List<string>? SearchPhrases { get; set; }
        public List<string>? Tags { get; set; }
		public bool? IsAccepted { get; set; }
	}
}