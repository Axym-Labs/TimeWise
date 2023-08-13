namespace SchedulerApp.Modules;
using SchedulerApp.Data.Scheduler;
using RestSharp;
using SchedulerApp.Modules;
using System.Net.Http;
using Newtonsoft.Json;

public class SchedulingAPIService
{
    private readonly RestClient _client = new(new RestClientOptions(Constants.apiEndpoint));

    public async Task<string?> GetAPIVersion()
    {
        try{
            return (await _client.GetAsync(new RestRequest("version"))).Content;
        }
        catch (Exception) {
            return null;
        }
    }



    public async Task<Solution?> GetSolution(Problem problem)
    {
        try
        {
            // Post the JSON to the API and get the response
            return await _client.PostAsync<Solution>(new RestRequest("solve").AddJsonBody(JsonConvert.SerializeObject(problem)));
        }
        catch (Exception ex)
        {
            // Handle any exceptions here
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }


    ~SchedulingAPIService(){
        _client.Dispose();
    }
}
