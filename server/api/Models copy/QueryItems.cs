using System;
using Microsoft.Data.SqlClient;

namespace fitnessapi.Models
{
	public class QueryItems
	{
		public string? Query { get; set; }
		public List<SqlParameter>? Parameters { get; set; }
    }
}

