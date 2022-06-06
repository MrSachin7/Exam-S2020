using AuthorAPI.DbAccess;
using AuthorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Primitives;

namespace AuthorAPI.Controllers; 


[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase {

    private readonly DataBaseContext _context;

    public BooksController(DataBaseContext context) {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks() {
        try {
            IQueryable<Book> queryable = _context.Books;
            List<Book> books = await queryable.ToListAsync();
            return Ok(books);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    [Route("{authorId:int}")]
    public async Task<ActionResult<Book>> AddBook([FromBody] Book book, [FromRoute] int authorId) {

        try {
            Author? author = await _context.Authors.Include(author1 => author1.Books).FirstOrDefaultAsync(author1 => author1.Id == authorId);
            if (author==null) {
                return StatusCode(500, "Author doesnt exist");
            }
            author.Books.Add(book);                  // this adds book to appropraite author
            EntityEntry<Book> entry = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            Book bookToReturn = entry.Entity;
            return Created($"/{bookToReturn.ISBN}", bookToReturn);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("{ISBN:int}")]
    public async Task<ActionResult<Book>> DeleteBook([FromRoute] int ISBN) {
        try {
            Book? book = await _context.Books.FirstOrDefaultAsync(book1 => book1.ISBN == ISBN);
            if (book==null) {
                return StatusCode(500, "Book not found in database");
            }

            EntityEntry<Book> removedBook = _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(removedBook.Entity);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }

    }
}