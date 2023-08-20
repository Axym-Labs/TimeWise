namespace TimeWise.Data.Scheduler;

public class ProblemScope
{
    public Problem Problem { get; set; } = new Problem();
    public int SelectedWeek { get; set; }
    public int SelectedDay { get; set; }
    public int SelectedTimeSlot { get; set; }
    public int SelectedShift { get; set; }
}
