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

public class InputsExporter
{
    public const string FileName = "Schedule";
    public MemoryStream GetStreamByFileExt(ProblemScope ProblemScope, SupportedFileTypes FileExt)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case SupportedFileTypes.JSON: return GetJSONStream(ProblemScope);
            case SupportedFileTypes.CSV: return GetCSVStream(ProblemScope);
            case SupportedFileTypes.XLSX: return GetXLSXStream(ProblemScope);
            case SupportedFileTypes.GSHEET: return GetGSHEETStream(ProblemScope);
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
