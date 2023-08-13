namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using Newtonsoft.Json;
using Csv;
using System;


public class Exporter
{
    public string ProblemToJson(Problem problem)
    {
        return JsonConvert.SerializeObject(problem);
    }

    public string GetCSVString() {
        throw new NotImplementedException("Implement CSV exporter");
    }
    public string GetXLSXString() {
        throw new NotImplementedException("Implement XLSX exporter");
    }
    public string GetGDOCString() {
        throw new NotImplementedException("Implement GDOC exporter");
    }
}
