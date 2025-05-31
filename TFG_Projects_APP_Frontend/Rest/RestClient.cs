using System.Text.Json;

namespace TFG_Projects_APP_Frontend.Rest;

public class RestClient
{
    HttpClient _client = new HttpClient();
    private readonly string baseURL = Preferences.Get("APIurl", "http://localhost:8000");
    public JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

    public async Task<HttpResponseMessage> GetAllAsync(string query)
    {
        var response = await _client.GetAsync($"{baseURL}/{query}");
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> GetByIdAsync(string query, int id)
    {
        var response = await _client.GetAsync($"{baseURL}/{query}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> PostAsync(string query, object data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{baseURL}/{query}", content);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> PatchAsync(string query, int id, object data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync($"{baseURL}/{query}/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> DeleteAsync(string query, int id)
    {
        var response = await _client.DeleteAsync($"{baseURL}/{query}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> TestConnection(string address)
    {
        try
        {
            var response = await _client.GetAsync(address);
            return response;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> AuthenticateUser(string address, object data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{baseURL}/{address}", content);
        return response;
    }
}
