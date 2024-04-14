namespace TimeWise.Modules;
using TimeWise.Data.Scheduler;
using RestSharp;
using TimeWise.Modules;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.FSharp.Collections;
using SchedulingLib;
using System.Collections.Immutable;
using System.Linq;

public class SchedulingAPIService
{

    public async Task<TimeWise.Data.Scheduler.Solution?> GetSolution(TimeWise.Data.Scheduler.Problem problem)
    {
        try
        {
            
            

            var scheduleList = ListModule.OfSeq(problem.Schedule.Weeks.Select(weeks => ListModule.OfSeq(weeks.Days.Select(day => ListModule.OfSeq(day.TimeSlots.Select(slot => ListModule.OfSeq(slot.Shifts.Select(shift => new SchedulingLib.ShiftInfo(shift.Name, shift.Length, ListModule.OfSeq(shift.RequiredPersonnel.Select(req => new SchedulingLib.RequiredPersonnel(req.Count, ListModule.OfSeq(req.RequiredQualifications)))), shift.Strain)))))))));
            var newSchedule = new SchedulingLib.Schedule(ListModule.OfSeq(scheduleList.Select(week => new SchedulingLib.Week(ListModule.OfSeq(week.Select(day => new SchedulingLib.Day(ListModule.OfSeq(day.Select(slot => new SchedulingLib.TimeSlot(slot))))))))));
            var newOptions = new SchedulingLib.Options(problem.Options.StrainMinimizing, problem.Options.EnsureQualifiedPersonnelConstraint, problem.Options.NoDoubleShiftConstraint, problem.Options.CapMaximumWorkingHoursConstraint, problem.Options.MinimumWorkingHoursConstraint);
            var newEmployees = ListModule.OfSeq(problem.Workers.Select(w => new SchedulingLib.Employee(w.Name, ListModule.OfSeq(w.Occupations), w.Wage)));
            var newProblem = new SchedulingLib.Problem(newEmployees, newSchedule, problem.MaxHoursPerWeek, problem.MinHoursPerWeek, newOptions);


            var solution = Scheduler.solve(newProblem);
            
            var result = solution.Result.Select(week => week.Select(day => day.Select(slot => slot.Select(shift => shift.ToList()).ToList()).ToList()).ToList()).ToList();
            return new Data.Scheduler.Solution(true, result, solution.ObjectiveCost, solution.ObjectiveStrain.ToList());
        }
        catch (ModelError)
        {
            return null;
        }
    }
}
