namespace SchedulerApp.Data.Scheduler;
public class Day
{
    public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    public Day(){}
    public Day(List<TimeSlot> timeslots){ this.TimeSlots = timeslots; }
}