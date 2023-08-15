namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using Newtonsoft.Json;
using Csv;
using System;
using System.Text;
using System.IO;



public class Exporter
{
    public const string FileName = "Schedule";
    public MemoryStream GetStreamByFileExt(Solution Solution, SupportedFileTypes FileExt)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case SupportedFileTypes.JSON: return GetJSONStream(Solution);
            case SupportedFileTypes.CSV: return GetCSVStream(Solution);
            case SupportedFileTypes.XLSX: return GetXLSXStream(Solution);
            case SupportedFileTypes.GSHEET: return GetGSHEETStream(Solution);
        }
    }

    public MemoryStream GetJSONStream(Solution Solution)
    {
        throw new NotImplementedException("Implement JSON exporter");
        // return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Solution)));
    }

    public MemoryStream GetCSVStream(Solution Solution)
    {
        throw new NotImplementedException("Implement CSV exporter");
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
