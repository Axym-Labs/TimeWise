using Newtonsoft.Json;
using SchedulerApp.Data.Scheduler;

namespace SchedulerApp.Modules;

public class InputsImporter
{

    public Problem? JsonToProblem(string input)
    {
        try { return JsonConvert.DeserializeObject<Problem>(input); }
        catch { return null; }
    }
}