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

public enum SupportedFileTypes
{
    JSON, CSV_H, CSV_P, XLSX
}

public class ScheduleExporter
{
    public const string FileName = "Schedule";
    public MemoryStream GetStreamByFileExt(Solution Solution, SupportedFileTypes FileExt, Problem problem)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case SupportedFileTypes.JSON: return GetJSONStream(Solution);
            case SupportedFileTypes.CSV_H: return GetCSVStream(Solution, problem);
            case SupportedFileTypes.CSV_P: return GetCSVStreamProgramReadable(Solution, problem);
            case SupportedFileTypes.XLSX: return GetXLSXStream(Solution, problem);
        }
    }

    public string FileTypeToExtStr(SupportedFileTypes fileExt)
    {
        return fileExt switch
        {
            SupportedFileTypes.JSON => "JSON",
            SupportedFileTypes.CSV_H => "CSV",
            SupportedFileTypes.CSV_P => "CSV",
            SupportedFileTypes.XLSX => "XLSX",
            _ => throw new NotImplementedException("No such filetype supported")
        };
    }

    public MemoryStream[] GetMultipleStreamsByFileExt(Solution Solution, SupportedFileTypes FileExt, Problem problem)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            //case SupportedFileTypes.JSON: return GetJSONStreamsByEmployee(Solution);
            case SupportedFileTypes.CSV_H: return GetCSVStreamsByEmployee(Solution, problem, false);
            case SupportedFileTypes.CSV_P: return GetCSVStreamsByEmployee(Solution, problem, true);
            //case SupportedFileTypes.XLSX: return new MemoryStream[] { GetXLSXStreamsByEmployee(Solution, problem) };
            case SupportedFileTypes.XLSX: return GetXLSXStreamsByEmployee(Solution, problem);

        }
    }

    public MemoryStream GetJSONStream(Solution Solution)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Solution)));
    }

    public MemoryStream[] GetCSVStreamsByEmployee(Solution solution, Problem problem, bool programReadable = false)
    {
        MemoryStream[] memStreams = new MemoryStream[problem.Workers.Count];
        foreach (int i in Enumerable.Range(0, problem.Workers.Count))
        {
            if (!programReadable)
            {
                memStreams[i] = GetCSVStream(solution, problem, problem.Workers[i].Name);
            } else
            {
                memStreams[i] = GetCSVStreamProgramReadable(solution, problem, problem.Workers[i].Name);
            }
        }
        return memStreams;
    }
    public MemoryStream GetCSVStream(Solution solution, Problem problem, string? employeeNameFilter = null)
    {
        // this condition should not exist, but the program-readable method is the one that doesnt lose
        // information when mutliple workers exist
        return ListToCSVStream(GetOutputAs2dList(solution, problem, employeeNameFilter));
    }
    
    public MemoryStream[] GetXLSXStreamsByEmployee(Solution solution, Problem problem)
    {
        MemoryStream[] memStreams = new MemoryStream[problem.Workers.Count];
        foreach (int i in Enumerable.Range(0, problem.Workers.Count))
        {
            memStreams[i] = GetXLSXStream(solution, problem, problem.Workers[i].Name);
        }
        return memStreams;
    }

    public MemoryStream GetXLSXStream(Solution solution, Problem problem, string? employeeNameFilter = null)
    {
        return ListToXLSXStream(GetOutputAs2dList(solution, problem, employeeNameFilter));
    }


    private static List<List<string>> GetOutputAs2dList(Solution solution, Problem problem, string? employeeNameFilter = null) 
    {
        List<List<string>> list = new();
        SortedSet<Tuple<string, string>> groupedShifts = GetGroupedShifts(problem);

        list = WriteEmptyLines(list);
        (list, Dictionary<Tuple<int, int>, int> indexOfDay) = WriteColumnHeader(list, solution, problem);
        list = WriteRowHeader(list, groupedShifts);
        list = WriteWorkerContent(list, problem, solution, groupedShifts, indexOfDay, employeeNameFilter);
        

        return list;
    }

    private static SortedSet<Tuple<string, string>> GetGroupedShifts(Problem problem)
    {
        SortedSet<Tuple<string, string>> groupedShifts = new();

        for (int weekI = 0; weekI < problem.Schedule.Weeks.Count; weekI++)
        {
        for (int dayI = 0; dayI < problem.Schedule.Weeks[weekI].Days.Count; dayI++)
        {
        for (int timeSlotI = 0; timeSlotI < problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots.Count; timeSlotI++)
        {
        for (int shiftI = 0; shiftI < problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts.Count; shiftI++)
        {
            string timeSlot = $"Timeslot {timeSlotI+1}";
            string shift = problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name;
            groupedShifts.Add(Tuple.Create(timeSlot, shift));
        }
        }
        }
        }
        return groupedShifts;
    }

    private static List<List<string>> WriteEmptyLines(List<List<string>> list)
    {
        list.Add(new List<string> {"", ""});
        list.Add(new List<string> {"", ""});

        return list;
    }

    private static Tuple<List<List<string>>, Dictionary<Tuple<int, int>, int>> WriteColumnHeader(List<List<string>> list, Solution solution, Problem problem)
    {

        var result = solution.Result;
        Dictionary<Tuple<int, int>, int> indexOfDay = new();
        int index = 0;

        for (int weekI = 0; weekI < result.Count; weekI++)
        {
            for (int dayI = 0; dayI < result[weekI].Count; dayI++)
            {
                list[0].Add($"Week {weekI + 1}");
                list[1].Add($"Day {dayI + 1}");

                indexOfDay.Add(Tuple.Create(weekI, dayI), index);
                index++;
            }
        }

        return Tuple.Create(list, indexOfDay);
    }

    private static List<List<string>> WriteRowHeader(List<List<string>> list, SortedSet<Tuple<string, string>> groupedShifts)
    {
        foreach ((string timeSlot, string shift) in groupedShifts)
        {
            list.Add(new List<string> { timeSlot, shift });
        }

        return list;
    }

    private static List<List<string>> WriteWorkerContent(List<List<string>> list, Problem problem, Solution solution, SortedSet<Tuple<string, string>> groupedShifts, Dictionary<Tuple<int, int>, int> indexOfDay, string? employeeNameFilter = null)
    {
        var result = solution.Result;
        List<Tuple<string, string>> groupedShiftsList = groupedShifts.ToList();

        for (int weekI = 0; weekI < problem.Schedule.Weeks.Count; weekI++)
        {
            for (int dayI = 0; dayI < problem.Schedule.Weeks[weekI].Days.Count; dayI++)
            {
                for (int timeSlotI = 0; timeSlotI < problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots.Count; timeSlotI++)
                {
                    for (int shiftI = 0; shiftI < problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts.Count; shiftI++)
                    {
                        string timeSlot = $"Timeslot {timeSlotI+1}";
                        string shift = problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name;

                        int iX = indexOfDay[Tuple.Create(weekI, dayI)] + 2;
                        int iY = groupedShiftsList.IndexOf(Tuple.Create(timeSlot, shift)) + 2;

                        if (employeeNameFilter == null)
                        {
                            list[iY].Add(string.Join(" & ", result[weekI][dayI][timeSlotI][shiftI]));
                        } else
                        {
                            if (result[weekI][dayI][timeSlotI][shiftI].Contains(employeeNameFilter)) {
                                list[iY].Add(employeeNameFilter);
                            } else
                            {
                                list[iY].Add("");
                            }
                        }
                    }
                }
            }
        }
        return list;
    }

    private static MemoryStream ListToXLSXStream(List<List<string>> list)
    {
        using (var notebook = new XLWorkbook())
        {
            notebook.Author = "SchedulerApp";
            var ws = notebook.Worksheets.Add("Schedule");
            
            foreach (int i1 in Enumerable.Range(0, list.Count))
            {
                foreach (int i2 in Enumerable.Range(0, list[i1].Count))
                {
                    ws.Cell(IndicesToCellId(i1, i2)).Value = list[i1][i2].Replace(" & ", ", ");

                }
            }
            using (var memStream = new MemoryStream())
            {
                notebook.SaveAs(memStream);
                return new MemoryStream(memStream.ToArray());
            }
        }
    }
    private static MemoryStream ListToCSVStream(List<List<string>> list)
    {
        StringBuilder csvString = new StringBuilder();

        foreach (var item in list)
        {
            csvString.AppendLine(string.Join(",", item));
        }
        return new MemoryStream(Encoding.UTF8.GetBytes(csvString.ToString()));
    }

    private static string IndicesToCellId(int row, int column)
    {
        // 1-based indexing
        column++;
        row++;

        string columnName = "";
        while (column > 0)
        {
            int modulo = (column - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            column = (column - modulo) / 26;
        }
        return columnName + row.ToString();
    }

    public MemoryStream GetCSVStreamProgramReadable(Solution solution, Problem problem, string? employeeNameFilter = null, bool differentWorkersInSameRow = false)
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
                            if (!differentWorkersInSameRow)
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
                                sb.Append($"Week {weekI + 1},");
                                sb.Append($"Day {dayI + 1},");
                                sb.Append($"Time slot {timeSlotI + 1},");
                                sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");

                                sb.AppendLine(string.Join(" & ", result[weekI][dayI][timeSlotI][shiftI]));
                            }


                        }
                        else
                        {
                            if (result[weekI][dayI][timeSlotI][shiftI].Contains(employeeNameFilter))
                            {
                                sb.Append($"Week {weekI + 1},");
                                sb.Append($"Day {dayI + 1},");
                                sb.Append($"Time slot {timeSlotI + 1},");
                                sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");
                                
                                sb.AppendLine(employeeNameFilter);
                            } else
                            {
                                sb.Append($"Week {weekI + 1},");
                                sb.Append($"Day {dayI + 1},");
                                sb.Append($"Time slot {timeSlotI + 1},");
                                sb.Append($"{problem.Schedule.Weeks[weekI].Days[dayI].TimeSlots[timeSlotI].Shifts[shiftI].Name},");

                                sb.AppendLine();
                            }
                        }
                    }
                }
            }
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }










    // DEPRECATED

    public MemoryStream GetXLSXStream_Depr(Solution Solution, Problem problem, string? employeeNameFilter = null)
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
                        if (dayI == 0)
                        {
                            ws.Column(2).LastCellUsed().CellBelow().SetValue(timeSlotI + 1);
                        }
                        for (int shiftI = 0; shiftI < result[weekI][dayI][timeSlotI].Count; shiftI++)
                        {
                            if (dayI == 0)
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
            using (var memStream = new MemoryStream())
            {
                notebook.SaveAs(memStream);
                return new MemoryStream(memStream.ToArray());
            }
        }
    }

    public MemoryStream GetXLSXStreamsByEmployee_Depr(Solution solution, Problem problem)
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
