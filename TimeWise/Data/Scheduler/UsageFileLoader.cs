using Newtonsoft.Json;
using TimeWise.Modules;

namespace TimeWise.Data.Scheduler;

public static class UsageFileLoader
{

    public static async Task<UserRatings> LoadFileAsync()
    {
        // TODO: Fix this
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string json = await File.ReadAllTextAsync(Constants.UsageFilePath);
        return JsonConvert.DeserializeObject<UserRatings>(json)!;
    }

    public static async Task SaveFileAsync(UserRatings ro)
    {
        string json = JsonConvert.SerializeObject(ro, Formatting.Indented);
        await File.WriteAllTextAsync(Constants.UsageFilePath, json);
    }
}

