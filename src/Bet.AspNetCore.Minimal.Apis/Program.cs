using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("TodosDb") ?? "Data Source=Todos.db";
// builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("Todos"));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
});

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
}

app.MapGet("/", () => "Todo Web Api");

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

TodoApi.MapRoutes(app);

await app.RunAsync();
