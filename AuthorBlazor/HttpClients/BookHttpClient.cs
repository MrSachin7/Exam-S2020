using System.Text;
using System.Text.Json;
using AuthorAPI.Models;
using AuthorBlazor.Services;
using Microsoft.Extensions.Primitives;

namespace AuthorBlazor.HttpClients;

public class BookHttpClient : IBookService {
    public async Task AddBook(Book newBookItem, int? authorId) {
        using HttpClient client = new HttpClient();
        string bookAsJson = JsonSerializer.Serialize(newBookItem);
        StringContent content = new StringContent(bookAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await client.PostAsync($"http://localhost:5154/Books/{authorId}", content);
        await GetResponseContentFromResponseMessageAndThrowAppropriateException(responseMessage);


    }

    public async Task<Book> RemoveBook(int ISBN) {
        using HttpClient client = new HttpClient();
        HttpResponseMessage responseMessage = await client.DeleteAsync($"http://localhost:5154/Books/{ISBN}");
        string responseContent =
            await GetResponseContentFromResponseMessageAndThrowAppropriateException(responseMessage);
        return GetDeserialized<Book>(responseContent);


    }

    private async Task<string> GetResponseContentFromResponseMessageAndThrowAppropriateException(
        HttpResponseMessage responseMessage) {
        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Error :{responseMessage.StatusCode}, {responseContent}");
        }

        return responseContent;
    }

    private T GetDeserialized<T>(string jsonFormat) {
        T obj = JsonSerializer.Deserialize<T>(jsonFormat, new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true
        }) !;
        return obj;
    }
}