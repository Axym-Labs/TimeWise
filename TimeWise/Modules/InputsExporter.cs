namespace TimeWise.Modules;
using TimeWise.Data.Scheduler;
using Newtonsoft.Json;
using Csv;
using System;
using System.Text;
using System.IO;


public enum SupportedFileTypes2
{
    JSON
}

public class InputsExporter
{
    public const string FileName = "TimeWise_Inputs";

    public MemoryStream GetStreamByFileExt(ProblemScope ProblemScope, SupportedFileTypes2 FileExt)
    {
        switch (FileExt)
        {
            default: throw new NotImplementedException("No implemented collection handler");
            case SupportedFileTypes2.JSON: return GetJSONStream(ProblemScope);
            //case SupportedFileTypes2.CSV: return GetCSVStream(ProblemScope);
            //case SupportedFileTypes2.XLSX: return GetXLSXStream(ProblemScope);
            //case SupportedFileTypes2.GSHEET: return GetGSHEETStream(ProblemScope);
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
