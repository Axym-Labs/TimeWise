namespace SchedulerApp.Modules;

using DocumentFormat.OpenXml.Spreadsheet;
using SchedulerApp.Data.Scheduler;
public class CompasionAlgorithm
{
    private class EmployeeWrapper
    {
        public Employee Employee { get; set; }
        public double SortingValue { get; set; }
        public double HoursWorked { get; set; }

        public EmployeeWrapper(Employee employee)
        {
            Employee = employee;
        }
    }
    public static Solution ProcedualScheduling(Problem problem)
    {
        var schedule = problem.Schedule;
        var workers = problem.Workers.Select(emp => new EmployeeWrapper(emp)).ToList();
        var solution = new Solution();

        List<List<List<List<List<string>>>>> result = new List<List<List<List<List<string>>>>>();

        foreach (var weekI in Enumerable.Range(0,schedule.Weeks.Count))
        {
            List<List<List<List<string>>>> weekResult = new List<List<List<List<string>>>>();

            foreach (var dayI in Enumerable.Range(0, problem.Schedule.Weeks[weekI].Days.Count))
            {
                List<List<List<string>>> dayResult = new List<List<List<string>>>();

                foreach (var slotI in Enumerable.Range(0, problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots.Count))
                {
                    List<List<string>> slotResult = new List<List<string>>();

                    foreach (var shiftI in Enumerable.Range(0, problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts.Count))
                    {
                        List<string> shiftResult = new List<string>();

                        foreach (var employee in workers)
                        {
                            if (employee.HoursWorked < problem.MaxHoursPerWeek)
                            {
                                var requiredPersonnel = problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].RequiredPersonnel;
                                if (employee.Employee.Occupations.All(emp => requiredPersonnel.Select(shift => shift.RequiredQualifications.Select(quali => quali.Contains(emp)))))
                                {
                                    employee.HoursWorked += problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Length;
                                    shiftResult.Add(employee.Employee.Name);
                                }
                            }
                        }
                        slotResult.Add(shiftResult);
                    }
                    dayResult.Add(slotResult);
                }
                weekResult.Add(dayResult);
            }
            result.Add(weekResult);
        }
        solution.Result = result;
        solution.Status = true;
        var (cost, strain) = CalculateObjectiveValues(solution, problem);
        solution.ObjectiveCost = cost;
        solution.ObjectiveStrain = strain;

        return solution;
    }

    private static (double cost,double strain) CalculateObjectiveValues(Solution solution, Problem problem)
    {
        var workSchedule = solution.Result;
        var strainList = new List<double>();
        var costList = new List<double>();

        foreach(var employee in problem.Workers)
        {
            foreach (var weekI in Enumerable.Range(0, workSchedule.Count))
            {
                foreach (var dayI in Enumerable.Range(0, workSchedule[weekI].Count))
                {
                    foreach (var slotI in Enumerable.Range(0, workSchedule[weekI][dayI].Count))
                    {
                        foreach (var shiftI in Enumerable.Range(0, workSchedule[weekI][dayI][slotI].Count))
                        {
                            foreach(var workerI in Enumerable.Range(0, workSchedule[weekI][dayI][slotI][shiftI].Count))
                            {
                                if (workSchedule[weekI][dayI][slotI][shiftI][workerI] == employee.Name)
                                {
                                    costList.Add(employee.Wage * problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Length);
                                    strainList.Add(problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Strain);
                                }
                            }
                        }
                    }
                }
            }
        }

        return (costList.Sum(), strainList.Sum());
    }
}



