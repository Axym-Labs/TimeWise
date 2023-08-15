using Newtonsoft.Json;

namespace SchedulerApp.Data.Scheduler;

public class RatingUser
{
    public int Rating { get; set; }
    public string Role { get; set; }
}

public class RootObject
{
    public int TotalUsers { get; set; }
    public List<int> Ratings { get; set; }
    public Dictionary<string, RatingUser> RatingUsers { get; set; }
}


public static class usageFileLoader
{
    public static RootObject LoadFile()
    {
        // TODO: Fix this
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        Console.WriteLine(basePath);
        string fullPath = Path.Combine(basePath, "/static/content/usage.json");
        string json = File.ReadAllText("static/content/usage.json");
        return JsonConvert.DeserializeObject<RootObject>(json);
    }

    public static void SaveFile(RootObject ro)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(basePath, "/static/content/usage.json");
        string json = JsonConvert.SerializeObject(ro, Formatting.Indented);
        File.WriteAllText("static/content/usage.json", json);
    }
}