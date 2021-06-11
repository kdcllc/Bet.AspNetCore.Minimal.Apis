using Microsoft.EntityFrameworkCore.Diagnostics;

#nullable disable
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected TodoDbContext()
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // disable null check for [required] properties
        optionsBuilder
            .LogTo(Console.WriteLine, new[] { InMemoryEventId.ChangesSaved })
            .UseInMemoryDatabase("UserContextWithNullCheckingDisabled", b => b.EnableNullabilityCheck(false));
    }
}
#nullable restore
