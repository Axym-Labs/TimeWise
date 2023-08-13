namespace SchedulingApp.Data.Scheduler;

public class Employee {
    public string Name { get; set; } = "";
    public List<string> Occupations { get; set; } = new List<string>();
    public double Wage { get; set; }

    public Employee() {}

    public Employee(string name,List<string> occupations ,double wage) {
        Name = name;
        Occupations = occupations;
        Wage = wage;
    }

    override public string ToString() {
        var qualis = "";
        if (Occupations != null) {
            foreach (var qualification in this.Occupations) {
                qualis += qualification.ToString() + ",";
            }
        }
        return string.Format("Name: {0}, Wage: {1}, Qualifications: {2}", this.Name, this.Wage.ToString(), qualis);
    }
}
