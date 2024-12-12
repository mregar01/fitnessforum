using System;
namespace fitnessapi.Models
{
	public class PostItemWithResponses
	{
        public PostItem? PostItem { get; set; }
        public List<PostItem>? Responses { get; set; }
        //public List<Comment>? Comments { get; set; }
    }
}

