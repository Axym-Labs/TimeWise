﻿using Newtonsoft.Json;
using TimeWise.Data.Scheduler;
using SchedulingLib;
namespace TimeWise.Modules;

public class InputsImporter
{
    public Problem? JsonToProblem(string input)
    {
        try { return JsonConvert.DeserializeObject<Problem>(input); }
        catch { return null; }
    }
}