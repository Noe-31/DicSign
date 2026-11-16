namespace DicSign.Models;

public sealed class UserRecord
{
    public string Username { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<string> FavoriteIds { get; set; } = new();
    public int StudiedCount { get; set; }
    public int LearnedCount { get; set; }
}
