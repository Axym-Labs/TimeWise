namespace TimeWise.Data.Scheduler;
public class Week
{
    public List<Day> Days{ get; set; } = new List<Day>();

    public Week(){}
    public Week(List<Day> days){ this.Days = days; }
}
