using Microsoft.EntityFrameworkCore;

namespace PassIn.Infrastructure;
internal class PassInDbContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\db\\PassInDb.db");
    }
}
