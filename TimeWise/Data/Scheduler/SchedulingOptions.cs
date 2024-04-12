namespace TimeWise.Data.Scheduler;
public class SchedulingOptions{
    public bool ExpenseMinimizing { get; set; } = true;
    public bool StrainMinimizing { get; set; } = true;
    public bool EnsureQualifiedPersonnelConstraint { get; set; } = true;
    public bool NoDoubleShiftConstraint { get; set; } = true;
    public bool CapMaximumWorkingHoursConstraint { get; set; } = true;

    public SchedulingOptions() {}
    public SchedulingOptions(bool StrainMinimizing,
                             bool EnsureQualifiedPersonnelConstraint,
                             bool NoDoubleShiftConstraint,
                             bool CapMaximumWorkingHoursConstraint) 
    {
        this.StrainMinimizing = StrainMinimizing;
        this.EnsureQualifiedPersonnelConstraint = EnsureQualifiedPersonnelConstraint;
        this.NoDoubleShiftConstraint = NoDoubleShiftConstraint;
        this.CapMaximumWorkingHoursConstraint = CapMaximumWorkingHoursConstraint;
    }
}
