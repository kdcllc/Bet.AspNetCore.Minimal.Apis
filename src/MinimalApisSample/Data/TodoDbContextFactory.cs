using Microsoft.EntityFrameworkCore.Design;

public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args.
    /// dotnet ef database update -- --environment Production.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public TodoDbContext CreateDbContext(string[] args)
    {
        throw new NotImplementedException();
    }
}
