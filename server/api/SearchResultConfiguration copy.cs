using System;
using Microsoft.EntityFrameworkCore;
using fitnessapi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fitnessapi
{
	public class SearchResultConfiguration: IEntityTypeConfiguration<SearchResult>
    {
        public void Configure(EntityTypeBuilder<SearchResult> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.QuestionId);
            builder.Property(c => c.Title).IsRequired(false);
            builder.Property(c => c.Tags).IsRequired(false);
            builder.Property(c => c.Body).IsRequired(false);
            builder.Property(c => c.ViewCount).IsRequired(false);
            builder.Property(c => c.VoteCount).IsRequired(false);
            builder.Property(c => c.PostTypeId);
            builder.Property(c => c.AcceptedAnswerId).IsRequired(false);
            builder.Property(c => c.AnswerCount).IsRequired(false);
        }
    }
}

