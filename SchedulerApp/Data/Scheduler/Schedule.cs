namespace SchedulingApp.Data.Scheduler;


public class Schedule{

    public List<Week> Weeks { get; set; } = new List<Week>();

    public Schedule(){ }
    public Schedule(List<Week> weeks) { this.Weeks = weeks; }
}