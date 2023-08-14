namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using Newtonsoft.Json;
using Csv;
using System;
using System.Text;
using System.IO;


public enum SupportedFileTypes
{
    JSON, CSV, XLSX, GSHEET
}

public class Exporter
{
    public MemoryStream GetStreamByFileExt(ProblemScope ProblemScope, string FileExt)
    {
        switch (FileExt.ToLower())
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case "json": return GetJSONStream(ProblemScope);
            case "csv": return GetCSVStream(ProblemScope);
            case "xslx": return GetXLSXStream(ProblemScope);
            case "gsheet": return GetGSHEETStream(ProblemScope);
        }
    }
    public MemoryStream GetJSONStream(ProblemScope ProblemScope)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ProblemScope)));
    }

    public MemoryStream GetCSVStream(ProblemScope ProblemScope) {
        throw new NotImplementedException("Implement CSV exporter");
    }
    public MemoryStream GetXLSXStream(ProblemScope ProblemScope) {
        throw new NotImplementedException("Implement XLSX exporter");
    }
    public MemoryStream GetGSHEETStream(ProblemScope ProblemScope) {
        throw new NotImplementedException("Implement GSHEET exporter");
    }
}
