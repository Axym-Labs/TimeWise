namespace TimeWise.Data.Scheduler;

public class Problem {

    public List<Employee> Workers { get; set; } = new List<Employee>();
    public Schedule Schedule { get; set; } = new Schedule();
    public double MaxHoursPerWeek { get; set; } = 40.0;
    public double MinHoursPerWeek { get; set; } = 0;
    public SchedulingOptions Options { get; set; } = new SchedulingOptions();
    
    public Problem() {}

    public Problem(List<Employee> employees, Schedule schedule, double maxHoursPerWeek, double minHoursPerWeek, SchedulingOptions schedulingOptions){
        Workers = employees;
        Schedule = schedule;
        MaxHoursPerWeek = maxHoursPerWeek;
        MinHoursPerWeek = minHoursPerWeek;
        Options = schedulingOptions;
    }
}