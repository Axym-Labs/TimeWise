namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using Newtonsoft.Json;
using System;
using System.Text;
using System.IO;
using ClosedXML.Excel;
using SchedulerApp.Modules.Helpers;
using DocumentFormat.OpenXml.Bibliography;
using SchedulerApp.Shared.Sections;
using System.Linq;

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
            case SupportedFileTypes.XLSX: return GetXLSXStream(Solution, problem);
        }
    }

    public MemoryStream[] GetMultipleStreamsByFileExt(Solution Solution, SupportedFileTypes FileExt, Problem problem)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            //case SupportedFileTypes.JSON: return GetJSONStreamsByEmployee(Solution);
            case SupportedFileTypes.CSV: return GetCSVStreamsByEmployee(Solution, problem);
            //case SupportedFileTypes.XLSX: return new MemoryStream[] { GetXLSXByEmployee(Solution, problem) };
            case SupportedFileTypes.XLSX: return GetXLSXByEmployee(Solution, problem);

        }
    }

    public MemoryStream GetJSONStream(Solution Solution)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Solution)));
    }

    public MemoryStream GetCSVStream(Solution solution, Problem problem, string? employeeNameFilter = null)
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
                        if (employeeNameFilter == null)
                        {
                            for (int workerI = 0; workerI < result[weekI][dayI][timeSlotI][shiftI].Count; workerI++)
                            {
                                sb.Append($"Week {weekI + 1},");
                                sb.Append($"Day {dayI + 1},");
                                sb.Append($"Time slot {timeSlotI + 1},");
                                sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");
                            
                                sb.AppendLine($"{result[weekI][dayI][timeSlotI][shiftI][workerI]}");
                            }
                        } else
                        {
                            if (result[weekI][dayI][timeSlotI][shiftI].Contains(employeeNameFilter)) {
                                sb.Append($"Week {weekI + 1},");
                                sb.Append($"Day {dayI + 1},");
                                sb.Append($"Time slot {timeSlotI + 1},");
                                sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");

                                sb.AppendLine(employeeNameFilter);
                            }
                        }
                    }
                }
            }
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    public MemoryStream[] GetCSVStreamsByEmployee(Solution solution, Problem problem)
    {
        MemoryStream[] memStreams = new MemoryStream[problem.Workers.Count];
        foreach (int i in Enumerable.Range(0,problem.Workers.Count))
        {
            memStreams[i] = GetCSVStream(solution, problem, problem.Workers[i].Name);
        }
        return memStreams;
    }


    public MemoryStream GetXLSXStream(Solution Solution, Problem problem, string? employeeNameFilter = null)
    {
        var result = Solution.Result;
        using (var notebook = new XLWorkbook())
        {
            notebook.Author = "SchedulerApp";
            var ws = notebook.Worksheets.Add("Schedule");
            ws.Cell("A1").Value = "Schedule";
            ws.Cell("A2").Value = "Weeks";
            ws.Cell("B2").Value = "Time Slots";
            ws.Cell("C2").Value = "Shifts";
            ws.Cell("D2").Value = "Day 1";
            ws.Cell("E2").Value = "Day 2";
            ws.Cell("F2").Value = "Day 3";
            ws.Cell("G2").Value = "Day 4";
            ws.Cell("H2").Value = "Day 5";
            ws.Cell("I2").Value = "Day 6";
            ws.Cell("J2").Value = "Day 7";

            for (int weekI = 0; weekI < result.Count; weekI++)
            {
                ws.LastRowUsed(XLCellsUsedOptions.AllContents).FirstCell().CellBelow().Value = $"Week {weekI + 1}";

                for (int dayI = 0; dayI < result[weekI].Count; dayI++)
                {
                    for (int timeSlotI = 0; timeSlotI < result[weekI][dayI].Count; timeSlotI++)
                    {
                        if(dayI == 0)
                        {
                            ws.Column(2).LastCellUsed().CellBelow().SetValue(timeSlotI + 1);
                        }
                        for (int shiftI = 0; shiftI < result[weekI][dayI][timeSlotI].Count; shiftI++)
                        {
                            if(dayI == 0)
                                ws.Column(3).LastCellUsed().CellBelow().SetValue(problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name);
                            if (employeeNameFilter == null)
                            {
                                ws.Column(4 + dayI).LastCellUsed().CellBelow().SetValue($"{string.Join(", ", result[weekI][dayI][timeSlotI][shiftI])}");
                            }
                            else
                            {
                                if (result[weekI][dayI][timeSlotI][shiftI].Contains(employeeNameFilter))
                                {
                                    ws.Column(4 + dayI).LastCellUsed().CellBelow().SetValue(employeeNameFilter);
                                }
                            }
                        }
                    }
                }
            }
            using (var memStream  = new MemoryStream())
            {
                notebook.SaveAs(memStream);
                return new MemoryStream(memStream.ToArray());
            }
        }
    }

    public MemoryStream[] GetXLSXByEmployee(Solution solution, Problem problem)
    {
        MemoryStream[] memStreams = new MemoryStream[problem.Workers.Count];
        Console.WriteLine(problem.Workers.Count);
        foreach (int i in Enumerable.Range(0, problem.Workers.Count))
        {
            Console.WriteLine(i);
            memStreams[i] = GetXLSXStream(solution, problem, problem.Workers[i].Name);
            Console.WriteLine(memStreams[i]);
        }
        return memStreams;
    }

        public MemoryStream GetXLSXByEmployee2(Solution solution, Problem problem)
    {
        var result = solution.Result;
        using var nb = new XLWorkbook();
        nb.Author = "SchedulerApp";
        var ws = nb.Worksheets.Add("Schedule by Employee");
        ws.FirstCell().SetValue("Employees")
            .CellRight().SetValue("Weeks")
            .CellRight().SetValue("Days")
            .CellRight().SetValue("Time Slots")
            .CellRight().SetValue("Shifts");

        foreach(var employee in problem.Workers)
        {
            ws.LastRowUsed().FirstCell().CellBelow().SetValue(employee.Name);
            var indicesCollection = StringHelper.FindStringIndices(solution, employee.Name, false);

            foreach(var entry in indicesCollection)
            {
                ws.LastRowUsed()
                    .RowBelow().FirstCell().CellRight()
                    .SetValue($"Week {entry.Item1 + 1}")
                    .CellRight().SetValue($"Day {entry.Item2 + 1}")
                    .CellRight().SetValue($"Time Slot {entry.Item3 + 1}")
                    .CellRight().SetValue(problem.Schedule.Weeks[entry.Item1].Days[entry.Item2].TimeSlots[entry.Item3].Shifts[entry.Item4].Name);
            }
        }

        using (var memStream = new MemoryStream())
        {
            nb.SaveAs(memStream);
            //File.WriteAllBytes("C:\\Users\\jona4\\Desktop\\test.xlsx", memStream.ToArray());
            return new MemoryStream(memStream.ToArray());
        }
    }

    public MemoryStream GetGSHEETStream(Solution Solution)
    {
        throw new NotImplementedException("Implement GSHEET exporter");
    }
}
