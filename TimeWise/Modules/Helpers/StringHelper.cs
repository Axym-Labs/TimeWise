namespace TimeWise.Modules.Helpers;
using Data.Scheduler;
using SchedulingLib;

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
    public static List<Tuple<int, int, int, int, int>> FindStringIndices(Solution solution, string targetString, bool allOnEmptyTarget = false)
	{
        List<Tuple<int, int, int, int, int>> indices = new();
        var result = solution.Result;
        for (int i = 0; i < result.Count(); i++)
        {
            for (int j = 0; j < result.ElementAt(i).Count(); j++)
            {
                for (int k = 0; k < result.ElementAt(i).ElementAt(j).Count(); k++)
                {
                    for (int l = 0; l < result.ElementAt(i).ElementAt(j).ElementAt(k).Count(); l++)
                    {
                        for (int m = 0; m < result.ElementAt(j).ElementAt(k).ElementAt(l).Count(); m++)
                        {
                            if (result.ElementAt(j).ElementAt(k).ElementAt(l).ElementAt(m) == targetString || (targetString == string.Empty && allOnEmptyTarget))
                            {
                                indices.Add(Tuple.Create(i, j, k, l, m));
                            }
                        }
                    }
                }
            }
        }

        return indices;
    }

}



