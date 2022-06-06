using AuthorAPI.Models;

namespace AuthorBlazor.Services; 

public interface IAuthorService {
    Task AddAuthorAsync(Author newAuthorItem);
    Task<List<Author>> GetAllAuthors
        ();
}