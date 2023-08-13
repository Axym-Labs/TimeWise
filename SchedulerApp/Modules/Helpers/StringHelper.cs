namespace SchedulerApp.Modules.Helpers;
using Data.Scheduler;

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
    public static List<Tuple<int, int, int, int, int>> FindStringIndices(Solution solution, string targetString)
    {
        List<Tuple<int, int, int, int, int>> indices = new List<Tuple<int, int, int, int, int>>();

        for (int i = 0; i < solution.Result.Count; i++)
        {
            for (int j = 0; j < solution.Result[i].Count; j++)
            {
                for (int k = 0; k < solution.Result[i][j].Count; k++)
                {
                    for (int l = 0; l < solution.Result[i][j][k].Count; l++)
                    {
                        for (int m = 0; m < solution.Result[i][j][k][l].Count; m++)
                        {
                            if (solution.Result[i][j][k][l][m] == targetString)
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



