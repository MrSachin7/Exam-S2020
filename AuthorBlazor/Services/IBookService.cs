using AuthorAPI.Models;

namespace AuthorBlazor.Services; 

public interface IBookService {
      Task AddBook(Book newBookItem, int? authorId);
      Task<Book> RemoveBook(int ISBN);
}