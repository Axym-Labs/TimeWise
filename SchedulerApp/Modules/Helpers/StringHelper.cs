namespace SchedulerApp.Modules.Helpers;
public static class StringHelper
{
    public static string JoinList(IEnumerable<string> list)
    {
        return string.Join(", ", list);
    }
    public static List<string> FormatSplitString(string str)
    {
        return str.Split(',').Select(s => s.TrimStart()).ToList();
    }
}



