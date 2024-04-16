namespace TimeWise.Data.Scheduler;
public class SchedulingOptions{
    public bool StrainMinimizing { get; set; } = true;
    public bool EnsureQualifiedPersonnelConstraint { get; set; } = true;
    public bool NoDoubleShiftConstraint { get; set; } = true;
    public bool MaximumWorkingHoursConstraint { get; set; } = true;
    public bool MinimumWorkingHoursConstraint { get; set; } = false;

    public SchedulingOptions() {}
    public SchedulingOptions(bool StrainMinimizing,
                             bool EnsureQualifiedPersonnelConstraint,
                             bool NoDoubleShiftConstraint,
                             bool MaximumWorkingHoursConstraint,
                             bool MinimumWorkingHoursConstraint) 
    {
        this.StrainMinimizing = StrainMinimizing;
        this.EnsureQualifiedPersonnelConstraint = EnsureQualifiedPersonnelConstraint;
        this.NoDoubleShiftConstraint = NoDoubleShiftConstraint;
        this.MaximumWorkingHoursConstraint = MaximumWorkingHoursConstraint;
        this.MinimumWorkingHoursConstraint = MinimumWorkingHoursConstraint;
    }
}
