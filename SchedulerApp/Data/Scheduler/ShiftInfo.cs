namespace SchedulingApp.Data.Scheduler;


public class ShiftInfo {
    public ShiftInfo() {}

    public ShiftInfo(string Name,double Length, List<RequiredPersonnel> RequiredPersonnel,double Strain){
        this.Name = Name;
        this.Length = Length;
        this.RequiredPersonnel = RequiredPersonnel;
        this.Strain = Strain;
    }
    public string? Name { get; set; }
    public double Length { get; set; }
    public List<RequiredPersonnel> RequiredPersonnel { get; set;} = new List<RequiredPersonnel>();
    public double Strain { get; set; }
}

public class RequiredPersonnel
{
    public int Count { get; set;}
    public List<string> RequiredQualifications { get; set; } = new List<string>();

    public RequiredPersonnel(int count, List<string> requiredQualifications)
    {
        Count = count;
        RequiredQualifications = requiredQualifications;
    }
}
