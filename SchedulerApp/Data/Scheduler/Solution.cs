using System.Collections.Generic;

namespace SchedulerApp.Data.Scheduler;

public class Solution
{
    public bool Status { get; set; } = false;
    public List<List<List<List<List<string>>>>> Result { get; set; } = new List<List<List<List<List<string>>>>>();
    public double ObjectiveCost { get; set; }
    public double ObjectiveStrain { get; set; }
}