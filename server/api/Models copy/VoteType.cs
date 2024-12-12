#pragma warning disable CA1008
public enum VoteType
#pragma warning restore CA1008
{
    AcceptedByOriginator = 1,
    UpMod = 2,
    DownMod = 3,
    Offensive = 4,
    Bookmark = 5,
    Close = 6,
    Reopen = 7,
    BountyStart = 8,
    BountyClose = 9,
    Deletion = 10,
    Undeletion = 11,
    Spam = 12,
    NominateModerator = 14,
    ModeratorReview = 15,
    ApproveEditSuggestion = 16
}