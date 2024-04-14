using System.Collections.Generic;

namespace TimeWise.Data.Scheduler;

public class Solution
{
    public bool Status { get; set; } = false;
    public List<List<List<List<List<string>>>>> Result { get; set; } = new List<List<List<List<List<string>>>>>();
    public double ObjectiveCost { get; set; }
    public List<Tuple<string,double>> ObjectiveStrain { get; set; }


    public Solution() { }
    public Solution(bool status, List<List<List<List<List<string>>>>> result, double objectiveCost, List<Tuple<string, double>> objectiveStrain)
    {
        Status = status;
        Result = result;
        ObjectiveCost = objectiveCost;
        ObjectiveStrain = objectiveStrain;
    }
}