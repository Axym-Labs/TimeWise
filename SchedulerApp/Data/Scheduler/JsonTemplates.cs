using Newtonsoft.Json;

namespace TimeWise.Data.Scheduler;

public class RatingUser
{
    public int Rating { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
}

public class UserRatings
{
    public int TotalUsers { get; set; }
    public List<int> Ratings { get; set; } = new List<int>();
    public Dictionary<string, RatingUser> RatingUsers { get; set; } = new Dictionary<string, RatingUser>();
}


public static class usageFileLoader
{
    public static async Task<UserRatings> LoadFileAsync()
    {
        // TODO: Fix this
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(basePath, "/static/content/usage.json");
        string json = await File.ReadAllTextAsync("static/content/usage.json");
        return JsonConvert.DeserializeObject<UserRatings>(json)!;
    }

    public static async Task SaveFileAsync(UserRatings ro)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(basePath, "/static/content/usage.json");
        string json = JsonConvert.SerializeObject(ro, Formatting.Indented);
        await File.WriteAllTextAsync("static/content/usage.json", json);
    }
}