namespace TimeWise.Data.Scheduler;
public class SchedulingOptions{
    public bool ExpenseMinimizing { get; set; } = true;
    public bool StrainMinimizing { get; set; } = true;
    public bool EnsureQualifiedPersonnelConstraint { get; set; } = true;
    public bool NoDoubleShiftConstraint { get; set; } = true;
    public bool MaximumWorkingHoursConstraint { get; set; } = true;
    public bool MinimumWorkingHoursConstraint { get; set; } = false;

    public SchedulingOptions() {}
    public SchedulingOptions(bool ExpenseMinimizing,
                             bool StrainMinimizing,
                             bool EnsureQualifiedPersonnelConstraint,
                             bool NoDoubleShiftConstraint,
                             bool MaximumWorkingHoursConstraint,
                             bool MinimumWorkingHoursConstraint)
    {
        this.ExpenseMinimizing = ExpenseMinimizing;
        this.StrainMinimizing = StrainMinimizing;
        this.EnsureQualifiedPersonnelConstraint = EnsureQualifiedPersonnelConstraint;
        this.NoDoubleShiftConstraint = NoDoubleShiftConstraint;
        this.MaximumWorkingHoursConstraint = MaximumWorkingHoursConstraint;
        this.MinimumWorkingHoursConstraint = MinimumWorkingHoursConstraint;
    }
}
