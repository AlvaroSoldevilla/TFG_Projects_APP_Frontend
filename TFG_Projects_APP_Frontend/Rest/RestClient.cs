using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.Users;
using TFG_Projects_APP_Frontend.Services.UsersService;

namespace TFG_Projects_APP_Frontend.Rest;

/*The class responsible for connecting to the API*/
public class RestClient(UserSession userSession)
{
    /*HTTP Client for the API Calls*/
    HttpClient _client = new HttpClient();
    /*Base URL to construct the url depending on the method called. It is obtained from the preferences but is set to localhost by default*/
    private readonly string baseURL = Preferences.Get("APIurl", "http://localhost:8000");
    /*Serializing options to convert to and from snake_case*/
    public JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

    /*Call to get endpoints, uses baseUrl and adds query to the end. Returns the response if the status code was a success, otherwise returns null*/
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

    /*Call to get endpoints, uses baseUrl and adds query and id to the end. Returns the response if the status code was a success, otherwise returns null*/
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

    /*Call to get post, uses baseUrl and adds query to the end, it also passes the data of the object to be created. Returns the response if the status code was a success, otherwise returns null*/
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

    /*Call to get patch, uses baseUrl and adds query and id to the end, it also passes the new data of the object to be updated. Returns the response if the status code was a success, otherwise returns null*/
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

    /*Call to delete endpoints, uses baseUrl and adds query and id to the end. Returns the response if the status code was a success, otherwise returns null*/
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

    /*An API call to test the connection.*/
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

    /*Call to get Authenticate user. Returns the user and calls GetToken to update the token*/
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

    /*Call to get API Token. Updates the token in UserSession*/
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

    /*Call to get refresh the API Token. Updates the token in UserSession*/
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
