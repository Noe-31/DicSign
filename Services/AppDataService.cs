using System.Security.Cryptography;
using System.Text.Json;
using DicSign.Models;
using Microsoft.Maui.Storage;

namespace DicSign.Services;

public sealed class AppDataService
{
    private const int HashIterations = 120_000;
    private const int HashLength = 32;
    private const string SessionKey = "dicsign_session_username";
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "dicsign_data.json");
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static AppDataService Instance { get; } = new();

    private AppDataService() { }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        username = username.Trim();
        if (username.Length < 3)
            throw new ArgumentException("El usuario debe tener al menos 3 caracteres.");
        if (password.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            if (users.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, HashIterations, HashAlgorithmName.SHA256, HashLength);
            users.Add(new UserRecord
            {
                Username = username,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hash)
            });
            await SaveUsersAsync(users);
            Preferences.Default.Set(SessionKey, username);
            return true;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            var user = users.FirstOrDefault(x => x.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));
            if (user is null)
                return false;

            byte[] salt = Convert.FromBase64String(user.PasswordSalt);
            byte[] savedHash = Convert.FromBase64String(user.PasswordHash);
            byte[] candidateHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, HashIterations, HashAlgorithmName.SHA256, HashLength);
            bool valid = CryptographicOperations.FixedTimeEquals(savedHash, candidateHash);
            if (valid)
                Preferences.Default.Set(SessionKey, user.Username);
            return valid;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<UserRecord?> GetCurrentUserAsync()
    {
        string? username = Preferences.Default.Get<string?>(SessionKey, null);
        if (string.IsNullOrWhiteSpace(username))
            return null;

        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            _gate.Release();
        }
    }

    public void Logout() => Preferences.Default.Remove(SessionKey);

    public async Task<bool> ToggleFavoriteAsync(string entryId)
    {
        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            var user = FindSessionUser(users);
            if (user is null)
                return false;

            bool isFavorite = user.FavoriteIds.Contains(entryId);
            if (isFavorite)
                user.FavoriteIds.Remove(entryId);
            else
                user.FavoriteIds.Add(entryId);

            await SaveUsersAsync(users);
            return !isFavorite;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<bool> IsFavoriteAsync(string entryId)
    {
        var user = await GetCurrentUserAsync();
        return user?.FavoriteIds.Contains(entryId) ?? false;
    }

    public async Task<List<DictionaryEntry>> GetFavoriteEntriesAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
            return new List<DictionaryEntry>();

        return DictionaryCatalog.Entries
            .Where(x => user.FavoriteIds.Contains(x.Id))
            .ToList();
    }

    public async Task RecordPracticeAsync(bool learned)
    {
        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            var user = FindSessionUser(users);
            if (user is null)
                return;

            user.StudiedCount++;
            if (learned)
                user.LearnedCount++;
            await SaveUsersAsync(users);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task ResetProgressAsync()
    {
        await _gate.WaitAsync();
        try
        {
            var users = await LoadUsersAsync();
            var user = FindSessionUser(users);
            if (user is null)
                return;

            user.StudiedCount = 0;
            user.LearnedCount = 0;
            await SaveUsersAsync(users);
        }
        finally
        {
            _gate.Release();
        }
    }

    private UserRecord? FindSessionUser(List<UserRecord> users)
    {
        string? username = Preferences.Default.Get<string?>(SessionKey, null);
        if (string.IsNullOrWhiteSpace(username))
            return null;
        return users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<List<UserRecord>> LoadUsersAsync()
    {
        if (!File.Exists(_filePath))
            return new List<UserRecord>();

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<UserRecord>>(stream, JsonOptions) ?? new List<UserRecord>();
    }

    private async Task SaveUsersAsync(List<UserRecord> users)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, users, JsonOptions);
    }
}
