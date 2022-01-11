public sealed class TodoApi
{
    /// <summary>
    /// utilizes di injection.
    /// </summary>
    /// <param name="routes"></param>
    public static void MapRoutes(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/todos", async ([FromServices] TodoDbContext db) =>
        {
            return await db.TodoItems.ToListAsync();
        });

        routes.MapGet("/todos/{id}", async ([FromServices] TodoDbContext db, int id) =>
        {
            return await db.TodoItems.FindAsync(id) is TodoItem todo ? Ok(todo) : NotFound();
        })
        .WithMetadata(new EndpointNameMetadata("todos"));

        routes.MapPost("/todos", async ([FromServices] TodoDbContext db, TodoItem todo) =>
        {
            await db.TodoItems.AddAsync(todo);
            await db.SaveChangesAsync();

            return CreatedAt(todo, "todos", new { todo.Id });
        });

        routes.MapPut("/todos", async ([FromServices] TodoDbContext db, TodoItem todo) =>
        {
            var found = await db.TodoItems.FindAsync(todo.Id);
            if (found is null)
            {
                return NotFound();
            }

            found.IsCompleted = todo.IsCompleted;
            await db.SaveChangesAsync();

            return Status(204);
        });

        routes.MapDelete("/todos/{id}", async ([FromServices] TodoDbContext db, int id) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            if (todo is null)
            {
                return NotFound();
            }

            db.TodoItems.Remove(todo);
            await db.SaveChangesAsync();

            return Ok();
        });
    }
}
