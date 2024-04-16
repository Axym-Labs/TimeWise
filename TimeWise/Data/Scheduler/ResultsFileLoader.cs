using Newtonsoft.Json;
using TimeWise.Modules;

namespace TimeWise.Data.Scheduler;


public static class ResultsFileLoader
{

    public static async Task SaveComparisonResults(List<Tuple<string, double>> Strain, double Cost)
    {
        ScheduleResults sr = await LoadFileAsync();

        double strain;
        if (Strain.Count > 0)
        {
            strain = Strain.Average(x => x.Item2);
            sr.ComparisonStrainAvg = sr.ComparisonStrainAvg + (strain - sr.ComparisonStrainAvg) / sr.ComparisonStrainNum;
            sr.ComparisonStrainNum += 1;
        }

        sr.ComparisonCostAvg = sr.ComparisonCostAvg + (Cost - sr.ComparisonCostAvg) / sr.ComparisonCostNum;
        sr.ComparisonCostNum += 1;

        await SaveFileAsync(sr);
    }

    public static async Task SaveIlpResults(List<Tuple<string, double>> Strain, double Cost)
    {
        ScheduleResults sr = await LoadFileAsync();

        double strain;
        if (Strain.Count > 0)
        {
            strain = Strain.Average(x => x.Item2);
            sr.IlpStrainAvg = sr.IlpStrainAvg + (strain - sr.IlpStrainAvg) / sr.IlpStrainNum;
            sr.IlpStrainNum += 1;
        }

        sr.IlpCostAvg = sr.IlpCostAvg + (Cost - sr.IlpCostAvg) / sr.IlpCostNum;
        sr.IlpCostNum += 1;

        await SaveFileAsync(sr);
    }

    public static async Task<ScheduleResults> LoadFileAsync()
    {
        // TODO: Fix this
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string json = await File.ReadAllTextAsync(Constants.ResultsFilePath);
        return JsonConvert.DeserializeObject<ScheduleResults>(json)!;
    }

    public static async Task SaveFileAsync(ScheduleResults sr)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string json = JsonConvert.SerializeObject(sr, Formatting.Indented);
        await File.WriteAllTextAsync(Constants.ResultsFilePath, json);
    }
}