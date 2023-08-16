namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
public class CompasionAlgorithm
{
    private class EmployeeWrapper
    {
        public Employee Employee { get; set; } = default!;
        public double SortingValue { get; set; }
    }
    public static Solution ProcedualScheduling(Problem problem)
    {
        var schedule = problem.Schedule;
        var employees = problem.Workers;
        var solution = new Solution();

        foreach(var week in schedule.Weeks)
        {
            foreach(var day in week.Days)
            {
                foreach(var slot in day.TimeSlots)
                {
                    foreach(var shift in slot.Shifts)
                    {
                        
                    }
                }
            }
        }
    }
}



