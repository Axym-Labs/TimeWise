namespace TimeWise.Modules;
public class Constants
{

    public const string contentDir = "static/content";
    public const string projectDir = "TimeWise";
    public int currentYear = DateTime.Now.Year;

    public const string apiEndpoint = "http://localhost:8080/api";
    public const string apiTestEndpoint = "http://localhost:8080/api";

    public const string DemoFilePath = "static/content/inputsDemo.json";

    public const string ScheduleBaseUrl = "/scheduler";

#if DEBUG
    public const string ResultsFilePath = "static/content/scheduleResultsDevelopment.json";
#else
   public const string ResultsFilePath = "static/content/scheduleResults.json";
#endif

#if DEBUG
    public const string UsageFilePath = "static/content/usageDevelopment.json";
#else
    public const string UsageFilePath = "static/content/usage.json";
#endif
}
