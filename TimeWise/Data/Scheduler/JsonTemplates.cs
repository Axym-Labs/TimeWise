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
