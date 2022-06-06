using AuthorAPI.DbAccess;
using AuthorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AuthorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase {
    private readonly DataBaseContext _context;

    public AuthorController(DataBaseContext context) {
        _context = context;
    }


    [HttpPost]
    public async Task<ActionResult<Author>> AddAuthor([FromBody] Author author) {
        try {
          
                                              /////// Regarding the model validation ////

           //Web API controllers don't have to check ModelState.IsValid if they have the [ApiController] attribute.
           //In that case, an automatic HTTP 400 response containing error details is returned when model state is invalid.
           //For more information, see Automatic HTTP 400 responses.
           // src : https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0

            EntityEntry<Author> entry = await _context.AddAsync(author);
            Author addedAuthor = entry.Entity;
            await _context.SaveChangesAsync();
            return Created($"/{addedAuthor.Id}", addedAuthor);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors() {
        try {
            IQueryable<Author> authors = _context.Authors.Include(author => author.Books);
            List<Author> authorsToReturn = await authors.ToListAsync();
            return Ok(authorsToReturn);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
}