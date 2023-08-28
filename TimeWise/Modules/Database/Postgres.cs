namespace TimeWise.Modules.Database;

using Newtonsoft.Json;
using Npgsql;
using TimeWise.Data.Scheduler;

public static class Postgres
{
    public static string ConnectionString = string.Empty;

    public static async Task<bool> LogProblem(Problem problem)
    {
        try
        {
        using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand("INSERT INTO Problems (problemdata) VALUES (CAST(@problemdata AS JSON))", conn);
        cmd.Parameters.Add(new NpgsqlParameter("problemdata", JsonConvert.SerializeObject(problem)));
        var result = await cmd.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return result != -1;
        } catch
        {
            return false;
        }
    }

}