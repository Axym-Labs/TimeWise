namespace SchedulerApp.Data.Scheduler;
public class TimeSlot
{
    public List<ShiftInfo> Shifts{ get; set; } = new List<ShiftInfo>(); 

    public TimeSlot(){}
    public TimeSlot(List<ShiftInfo> shifts){ this.Shifts = shifts; }
}