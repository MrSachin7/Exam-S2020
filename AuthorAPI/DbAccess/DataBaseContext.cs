using AuthorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorAPI.DbAccess; 

public class DataBaseContext : DbContext {

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite(@"Data Source = D:\SEM 3\DNP1\Exam-S2020\AuthorAPI\database.db");
    }
}