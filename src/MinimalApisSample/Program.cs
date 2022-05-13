using System.Security.Claims;

using Bet.AspNetCore.Minimal.Api.Security.JwtBearer;

using Microsoft.OpenApi.Models;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("TodosDb") ?? "Data Source=Todos.db";
// builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("Todos"));

// Configure JSON options.
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.IncludeFields = true;
});

builder.Services.AddControllers();

// add security
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
    c.AddJwtBearerAuthentication();
});

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseSecurity();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Todo Web Api");

TodoApi.MapRoutes(app);

app.MapPost("/api/login", (AppUser user, IJwtBearerGeneratorService jwtBearerGeneratorService) =>
{
    var claims = new List<Claim>
        {
            new (ClaimTypes.GivenName, user.UserName),
            new (ClaimTypes.Surname, user.UserName)
        };

    var token = jwtBearerGeneratorService.CreateToken(user.UserName, claims);
    return Ok(new { token });
})
.Accepts<AppUser>("application/json")
.Produces<string>(StatusCodes.Status200OK)
.WithName("login").WithTags("Auth");

app.MapGet("/api/userInfo", (HttpContext context) =>
{
    var userName = new { username = context.User.Identity!.Name };
    return Ok(userName);
})
.Produces(StatusCodes.Status200OK)
.WithName("userInfo").WithTags("Auth")
.RequireAuthorization();

await app.RunAsync();

public record AppUser(string UserName, string Password);
