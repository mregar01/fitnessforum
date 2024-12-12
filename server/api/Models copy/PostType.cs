namespace fitnessapi.Models;

#pragma warning disable CA1008
public enum PostType
#pragma warning restore CA1008
{
    Question = 1,
    Answer = 2,
    Wiki = 3,
    TagWikiExcerpt = 4,
    TagWiki = 5,
    ModeratorNomination = 6,
    WikiPlaceholder = 7,
    PrivilegeWiki = 8,
}