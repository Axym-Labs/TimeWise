namespace SchedulerApp.Data.Scheduler;

public class Problem {

    public List<Employee> Workers { get; set; } = new List<Employee>();
    public Schedule Schedule { get; set; } = new Schedule();
    public double MaxHoursPerWeek { get; set; } = 48.0;
    public SchedulingOptions Options { get; set; } = new SchedulingOptions();
    
    public Problem() {}

    public Problem(List<Employee> employees, Schedule schedule, double MaxHoursPerWeek, SchedulingOptions schedulingOptions){
        Workers = employees;
        Schedule = schedule;
        this.MaxHoursPerWeek = MaxHoursPerWeek;
        Options = schedulingOptions;
    }
}