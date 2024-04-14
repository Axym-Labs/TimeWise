namespace TimeWise.Modules;

using DocumentFormat.OpenXml.Spreadsheet;
using Irony;
using MathNet.Numerics.Statistics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.FSharp.Collections;
using SchedulingLib;
using System.Linq;

public class ComparisonAlgorithm
{
    private class EmployeeWrapper
    {
        public Employee Employee { get; set; }
        public double SortingValue { get; set; }
        public double HoursWorked { get; set; }
        public bool IsAvailable { get; set; } = true;

        public EmployeeWrapper(Employee employee)
        {
            Employee = employee;
        }
    }

    public static bool IsScheduleComplete(Solution solution, Problem problem)
    {
        var workSchedule = solution.Result;
        var schedule = problem.Schedule.Weeks.Select(week => week.Days.Select(day => day.TimeSlots.Select(slot => slot.Shifts.ToList()).ToList()).ToList()).ToList();

        var isValid = new List<bool>();


        foreach (var week in schedule)
        {
            foreach (var day in week)
            {
                foreach (var slot in day)
                {
                    foreach (var shift in slot)
                    {
                        foreach (var requirement in shift.RequiredPersonnel)
                        {
                            isValid.Add(requirement.Count == workSchedule.SelectMany(w => w.SelectMany(d => d.SelectMany(s => s))).Count());
                        }
                    }
                }
            }
        }

        return isValid.All(x => x);
    }

    public static Solution ProceduralScheduling(Problem problem)
    {
        var schedule = problem.Schedule.Weeks.Select(week => week.Days.Select(day => day.TimeSlots.Select(slot => slot.Shifts.ToList()).ToList()).ToList()).ToList();
        var workers = Shuffle(problem.Workers.Select(emp => new EmployeeWrapper(emp)).ToList());

        List<List<List<List<List<string>>>>> result = new List<List<List<List<List<string>>>>>();

        foreach (var weekI in Enumerable.Range(0,schedule.Count))
        {
            List<List<List<List<string>>>> weekResult = new List<List<List<List<string>>>>();

            foreach (var dayI in Enumerable.Range(0, schedule[weekI].Count))
            {
                List<List<List<string>>> dayResult = new List<List<List<string>>>();

                foreach (var slotI in Enumerable.Range(0, schedule[weekI][dayI].Count))
                {
                    List<List<string>> slotResult = new List<List<string>>();

                    foreach (var shiftI in Enumerable.Range(0, schedule[weekI][dayI][slotI].Count))
                    {
                        List<string> shiftResult = new List<string>();
                        foreach(var requirementI in Enumerable.Range(0,schedule[weekI][dayI][slotI][shiftI].RequiredPersonnel.Count()))
                        {
                            var reqPersonals = schedule[weekI][dayI][slotI][shiftI].RequiredPersonnel.ToList();
                            var reqPersonnel = reqPersonals[requirementI];
                            foreach (var count in Enumerable.Range(0, reqPersonnel.Count))
                            {
                                workers.OrderBy(emp => emp.SortingValue);
                                foreach (var employee in workers)
                                {
                                    var timeOfShift = problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Length;
                                    /*
                                    if (employee.HoursWorked + timeOfShift <= problem.MaxHoursPerWeek)
                                    { */
                                        if (employee.Employee.Occupations.SequenceEqual(problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].RequiredPersonnel[requirementI].RequiredQualifications))
                                        {
                                            if (employee.IsAvailable)
                                            {
                                                employee.HoursWorked += timeOfShift;
                                                employee.SortingValue += 1;
                                                employee.IsAvailable = false;
                                                shiftResult.Add(employee.Employee.Name);
                                                break;
                                            }
                                        }
                                    //}
                                }
                            }
                        }
                        slotResult.Add(shiftResult);
                    }
                    dayResult.Add(slotResult);
                }
                weekResult.Add(dayResult);
                workers.ForEach(emp => emp.IsAvailable = true);
            }
            workers.ForEach(emp => emp.HoursWorked = 0);
            result.Add(weekResult);
        }
        

        var (cost, strain) = CalculateObjectiveValues(result, problem);


        return new Solution(result, cost, strain);
    }

    public static (double cost,List<Tuple<string,double>> strain) CalculateObjectiveValues(List<List<List<List<List<string>>>>> solution, Problem problem)
    {
        var workSchedule = solution;
        var strainList = new List<Tuple<string,double>>();
        var costList = new List<double>();

        var workers = problem.Workers.OrderBy(x => x.Wage).ToList();

        foreach(var employee in workers)
        {
            foreach (var weekI in Enumerable.Range(0, workSchedule.Count()))
            {
                foreach (var dayI in Enumerable.Range(0, workSchedule.ElementAt(weekI).Count()))
                {
                    foreach (var slotI in Enumerable.Range(0, workSchedule.ElementAt(weekI).ElementAt(dayI).Count()))
                    {
                        foreach (var shiftI in Enumerable.Range(0, workSchedule.ElementAt(weekI).ElementAt(dayI).ElementAt(slotI).Count()))
                        {
                            foreach(var workerI in Enumerable.Range(0, workSchedule.ElementAt(weekI).ElementAt(dayI).ElementAt(slotI).ElementAt(shiftI).Count()))
                            {
                                if (workSchedule.ElementAt(weekI).ElementAt(dayI).ElementAt(slotI).ElementAt(shiftI).ElementAt(workerI) == employee.Name)
                                {
                                    costList.Add(employee.Wage * problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Length);
                                    strainList.Add(new Tuple<string,double>(employee.Name, problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[slotI].Shifts[shiftI].Strain));
                                }
                            }
                        }
                    }
                }
            }
        }

        return (costList.Sum(), strainList);
    }

    public static List<T> Shuffle<T>(List<T> list)
    {
        var random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}

