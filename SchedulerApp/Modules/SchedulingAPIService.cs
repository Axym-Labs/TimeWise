namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using RestSharp;
using SchedulerApp.Modules;
using System.Net.Http;
using Newtonsoft.Json;

public class SchedulingAPIService
{
#if DEBUG
    private readonly RestClient _client = new(new RestClientOptions(Constants.apiTestEndpoint) { MaxTimeout = 20 * 1000 });
#else
    private readonly RestClient _client = new(new RestClientOptions(Constants.apiEndpoint) { MaxTimeout = 20 * 1000 });
#endif
    public async Task<string?> GetAPIVersion()
    {
        try{
            return (await _client.GetAsync(new RestRequest("version") { Timeout = 3000})).Content;
        }
        catch (Exception ex) {
            await Console.Out.WriteLineAsync(ex.Message);
            return null;
        }
    }

    public async Task<Solution?> GetSolution(Problem problem)
    {
        try
        {
            return await _client.PostAsync<Solution>(new RestRequest("solve").AddJsonBody(JsonConvert.SerializeObject(problem)));
        }
        catch (Exception)
        {
            return null;
        }
    }


    ~SchedulingAPIService(){
        _client.Dispose();
    }
}
