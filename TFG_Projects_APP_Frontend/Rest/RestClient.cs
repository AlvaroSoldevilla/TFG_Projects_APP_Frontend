using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.Rest;

public class RestClient(UserSession userSession)
{
    HttpClient _client = new HttpClient();
    private readonly string baseURL = Preferences.Get("APIurl", "http://localhost:8000");
    public JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

    public async Task<HttpResponseMessage> GetAllAsync(string query)
    {
        await RefreshToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
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
        await RefreshToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
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
        await RefreshToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
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
        await RefreshToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
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
        await RefreshToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
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
        if (response.IsSuccessStatusCode)
        {
            await GetToken(data);
        }
        return response;
    }

    public async Task GetToken(object data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{baseURL}/users/token", content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<TokenResponse>(_options);
            userSession.Token = token.access_token;
        }
    }

    public async Task RefreshToken()
    {
        if (userSession.User != null) {
            var json = JsonSerializer.Serialize(new RefreshToken{
                Email = userSession.User.Email,
                Token = userSession.Token
            }, _options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseURL}/users/refresh", content);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<TokenResponse>(_options);
                userSession.Token = token.access_token;
            }
        }
    }
}
