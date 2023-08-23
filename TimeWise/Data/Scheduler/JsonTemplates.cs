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

public class ScheduleResults
{
    public double IlpCostAvg { get; set; }

    public int IlpCostNum { get; set; }

    public double ComparisonCostAvg { get; set; }

    public int ComparisonCostNum { get; set; }

    public double IlpStrainAvg { get; set; }

    public int IlpStrainNum { get; set; }

    public double ComparisonStrainAvg { get; set; }

    public int ComparisonStrainNum { get; set; }
}

public static class ResultsFileLoader
{
    public static async Task SaveComparisonResults(double Strain, double Cost)
    {
        ScheduleResults sr = await LoadFileAsync();
        double Ds = (Strain - sr.ComparisonStrainAvg) / sr.ComparisonStrainNum;
        double Dc = (Cost - sr.ComparisonCostAvg) / sr.ComparisonCostNum;

        sr.ComparisonStrainAvg += Math.Round(Ds, 2, MidpointRounding.AwayFromZero);
        sr.ComparisonCostAvg += Math.Round(Dc, 2, MidpointRounding.AwayFromZero);
        await SaveFileAsync(sr);
    }

    public static async Task SaveIlpResults(double Strain, double Cost)
    {
        ScheduleResults sr = await LoadFileAsync();
        double Ds = (Strain - sr.IlpStrainAvg) / sr.IlpStrainNum;
        double Dc = (Cost - sr.IlpCostAvg) / sr.IlpCostNum;

        sr.IlpStrainAvg += Math.Round(Ds, 2, MidpointRounding.AwayFromZero);
        sr.IlpCostAvg += Math.Round(Dc, 2, MidpointRounding.AwayFromZero);
        await SaveFileAsync(sr);
    }

    public static async Task<ScheduleResults> LoadFileAsync()
    {
        // TODO: Fix this
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(basePath, "/static/content/scheduleResults.json");
        string json = await File.ReadAllTextAsync("static/content/scheduleResults.json");
        return JsonConvert.DeserializeObject<ScheduleResults>(json)!;
    }

    public static async Task SaveFileAsync(ScheduleResults sr)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(basePath, "/static/content/scheduleResults.json");
        string json = JsonConvert.SerializeObject(sr, Formatting.Indented);
        await File.WriteAllTextAsync("static/content/scheduleResults.json", json);
    }
}

public static class UsageFileLoader
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
