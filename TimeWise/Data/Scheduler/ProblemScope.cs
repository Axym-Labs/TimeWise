namespace TimeWise.Data.Scheduler;
using SchedulingLib;
public class ProblemScope
{
    public required Problem Problem { get; set; }
    public int SelectedWeek { get; set; }
    public int SelectedDay { get; set; }
    public int SelectedTimeSlot { get; set; }
    public int SelectedShift { get; set; }
}
