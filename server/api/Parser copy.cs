using System.Diagnostics;
using System.Text.RegularExpressions;

namespace fitnessapi.Models

{
	public class Parser
	{        
        public SearchItems Parse(string query)
        {
            SearchItems items = new();

            // Regular expression to extract phrases enclosed in double quotes
            var phraseRegex = new Regex("\"([^\"]*)\"");
            var phraseMatches = phraseRegex.Matches(query);
            items.SearchPhrases = phraseMatches.Select(match => match.Groups[1].Value).ToList();

            // Regular expression to extract phrases enclosed in square brackets
            var tagRegex = new Regex("\\[([^\\]]*)\\]");
            var tagMatches = tagRegex.Matches(query);
            items.Tags = tagMatches.Select(match => match.Groups[1].Value).ToList();


            // Regular expression to extract user ID in the format user:1234
            var userRegex = new Regex(@"user:(\d+)");
            var userMatch = userRegex.Match(query);
            //items.UserId = null;

            // Regular expression to extract score in the format score:1234
            var scoreRegex = new Regex(@"score:(\d+)");
            var scoreMatch = scoreRegex.Match(query);
            //items.MinScore = null;

            // Regular expression to extract answers in the format answers:1234
            var answersRegex = new Regex(@"answers:(\d+)");
            var answersMatch = answersRegex.Match(query);
            //items.NumAnswers = null;

            // Regular expression to extract answers in the format isaccepted:yes/no
            var acceptedRegex = new Regex(@"isaccepted:(yes|no)", RegexOptions.IgnoreCase);
            var acceptedMatch = acceptedRegex.Match(query);
            //items.IsAccepted = null;

            // If user ID is found in the query, extract and store it
            if (userMatch.Success)
            {
                if (int.TryParse(userMatch.Groups[1].Value, out int id))
                {
                    items.UserId = id;
                }

                // Remove the user ID part from the query
                query = userRegex.Replace(query, "").Trim();
            }

            // If score is found in the query, extract and store it
            if (scoreMatch.Success)
            {
                if (int.TryParse(scoreMatch.Groups[1].Value, out int value))
                {
                    items.MinScore = value;
                }

                // Remove the user ID part from the query
                query = scoreRegex.Replace(query, "").Trim();
            }

            // If answers is found in the query, extract and store it
            if (answersMatch.Success)
            {
                if (int.TryParse(answersMatch.Groups[1].Value, out int value))
                {
                    items.NumAnswers = value;
                }

                // Remove the user ID part from the query
                query = answersRegex.Replace(query, "").Trim();
            }

            // If isaccepted:yes is found in the query, set accpeted to true; otherwise, set it to false
            if (acceptedMatch.Success)
            {
                string value = acceptedMatch.Groups[1].Value.ToLower();
                items.IsAccepted = value == "yes";

                // Remove the isaccepted part from the query
                query = acceptedRegex.Replace(query, "").Trim();
            }


            // Remove phrases from the query to get individual words
            query = phraseRegex.Replace(query, "").Trim();
            query = tagRegex.Replace(query, "").Trim();

            // Split the remaining query into individual words
            items.SearchWords = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return items;

        }       
    }
}