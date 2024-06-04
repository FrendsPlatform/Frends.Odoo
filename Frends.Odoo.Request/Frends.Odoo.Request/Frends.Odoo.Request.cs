namespace Frends.Odoo.Request;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading;
using Definitions;
using System.Linq;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Odoo
{
    /// <summary>
    /// Frends Task for making requests to Odoo API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Odoo.Request).
    /// </summary>
    /// <param name="input">Task input.</param>
    /// <param name="options">Task options.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { bool Success, string Error, dynamic Data }.</returns>
    public static async Task<Result> Request(
        [PropertyTab] Input input,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            var session = await AuthenticateAsync(httpClient, options, cancellationToken);
            var odooResult = await CallAsync(httpClient, session, input, options, cancellationToken);
            return new Result { Success = true, Error = null, Data = odooResult };
        }
        catch (Exception e)
        {
            if (options.ThrowExceptionOnErrorResponse)
            {
                throw;
            }

            return new Result { Success = false, Error = e.Message + Environment.NewLine + e.StackTrace, Data = null };
        }
    }

    private static async Task<string> AuthenticateAsync(
        HttpClient httpClient,
        Options options,
        CancellationToken cancellationToken)
    {
        var request = new
        {
            jsonrpc = "2.0",
            @params = new
            {
                db = options.Database,
                login = options.Username,
                password = options.Password,
            },
        };

        using var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(
            $"{options.OdooUrl}/web/session/authenticate", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        IEnumerable<string> cookies;
        try
        {
            cookies = response.Headers.GetValues("Set-Cookie");
        }
        catch (InvalidOperationException)
        {
            // If the response does not contain the Set-Cookie header, we assume that the authentication failed.
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
            throw new Exception("Authentication failed. Odoo's response: " + jsonResponse?.error?.data?.message);
        }

        var sessionId = cookies.Where(o => o.StartsWith("session_id"))
            .Select(o => o.Split(';')[0].Split('=')[1])
            .FirstOrDefault();

        if (sessionId == null)
        {
            throw new Exception("Authentication failed. Could not find session_id from the response.");
        }

        return sessionId;
    }

    private static async Task<dynamic> CallAsync(
        HttpClient httpClient, string session, Input input, Options options, CancellationToken cancellationToken)
    {
        var request = new
        {
            jsonrpc = "2.0",
            @params = new
            {
                model = input.Model,
                method = input.Method,
                args = JsonConvert.DeserializeObject(input.Args),
                kwargs = JsonConvert.DeserializeObject(input.Kwargs),
            },
        };

        var jsonRequest = JsonConvert.SerializeObject(request);
        Console.WriteLine(jsonRequest);
        using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Add("Cookie", $"session_id={session}");
        var response = await httpClient.PostAsync($"{options.OdooUrl}/web/dataset/call_kw", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

        if (jsonResponse.error != null)
        {
            throw new Exception(JsonConvert.SerializeObject(jsonResponse.error.data, Formatting.Indented));
        }

        return jsonResponse.result;
    }
}
