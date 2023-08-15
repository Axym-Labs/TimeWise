namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using Newtonsoft.Json;
using Csv;
using System;
using System.Text;
using System.IO;
using Microsoft.Extensions.Primitives;

public class ScheduleExporter
{
    public const string FileName = "Schedule";
    public MemoryStream GetStreamByFileExt(Solution Solution, SupportedFileTypes FileExt, Problem problem)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case SupportedFileTypes.JSON: return GetJSONStream(Solution);
            case SupportedFileTypes.CSV: return GetCSVStream(Solution, problem);
            case SupportedFileTypes.XLSX: return GetXLSXStream(Solution);
            case SupportedFileTypes.GSHEET: return GetGSHEETStream(Solution);
        }
    }

    public MemoryStream GetJSONStream(Solution Solution)
    {
        //throw new NotImplementedException("Implement JSON exporter");
        return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Solution)));
    }

    public MemoryStream GetCSVStream(Solution solution, Problem problem)
    {
        var result = solution.Result;
        var sb = new StringBuilder();
        sb.AppendLine("Weeks,Days,TimeSlots,Shifts,Workers");

        for (int weekI = 0; weekI < result.Count; weekI++)
        {
            for (int dayI = 0; dayI < result[weekI].Count; dayI++)
            {
                for (int timeSlotI = 0; timeSlotI < result[weekI][dayI].Count; timeSlotI++)
                {
                    for (int shiftI = 0; shiftI < result[weekI][dayI][timeSlotI].Count; shiftI++)
                    {
                        for (int workerI = 0; workerI < result[weekI][dayI][timeSlotI][shiftI].Count; workerI++)
                        {
                            sb.Append($"Week {weekI + 1},");
                            sb.Append($"Day {dayI + 1},");
                            sb.Append($"Time slot {timeSlotI + 1},");
                            sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");
                            sb.AppendLine($"{result[weekI][dayI][timeSlotI][shiftI][workerI]}");
                        }
                    }
                }
            }
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }
    public MemoryStream GetXLSXStream(Solution Solution)
    {
        throw new NotImplementedException("Implement XLSX exporter");
    }
    public MemoryStream GetGSHEETStream(Solution Solution)
    {
        throw new NotImplementedException("Implement GSHEET exporter");
    }
}
