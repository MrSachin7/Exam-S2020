using System.Text;
using System.Text.Json;
using AuthorAPI.Models;
using AuthorBlazor.Services;
using Microsoft.Extensions.Primitives;

namespace AuthorBlazor.HttpClients; 

public class AuthorHttpClient :IAuthorService {
    public async Task AddAuthorAsync(Author newAuthorItem) {
        using HttpClient client = new HttpClient();
        string authorAsJson = JsonSerializer.Serialize(newAuthorItem);
        StringContent content = new StringContent(authorAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await client.PostAsync("http://localhost:5154/Author", content);
        await GetResponseContentFromResponseMessageAndThrowAppropriateException(responseMessage);

    }

    public async Task<List<Author>> GetAllAuthors() {
        using HttpClient client = new HttpClient();
        HttpResponseMessage responseMessage = await client.GetAsync("http://localhost:5154/Author");
        String responseContent = await GetResponseContentFromResponseMessageAndThrowAppropriateException(responseMessage);
        return GetDeserialized<List<Author>>(responseContent);
    }


    private T GetDeserialized<T>(string jsonFormat) {
        T obj = JsonSerializer.Deserialize<T>(jsonFormat, new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true
        }) !;
        return obj;
    }

    private async Task<string> GetResponseContentFromResponseMessageAndThrowAppropriateException(
        HttpResponseMessage responseMessage) {
        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Error :{responseMessage.StatusCode}, {responseContent}");
        }

        return responseContent;
    }

}