using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("TodosDb") ?? "Data Source=Todos.db";
// builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("Todos"));

builder.Services.AddControllers();

builder.Services.AddEndpoints();

// https://github.com/dotnet/aspnetcore/commit/fd19f92df938c42525c9f3a2f3acd13d7e7d88f2#diff-44573e6ff8f15f9e7b43fffc01cf68a3fe9380f95ddc8de10dfe142587a8d7f7
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning();

builder.Services.AddSwaggerGen(c =>
{
    var appName = typeof(App).GetTypeInfo().Assembly.GetName().Name;

    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Todo web api implementation using Minimal Api in AspNetCore",
        Title = appName,
        Version = "v1",

        Contact = new OpenApiContact()
        {
            Name = "kdcllc",
            Url = new Uri("https://kingdavidconsulting.com"),
        }
    });

    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/923c7e6ae3b451aa22ed9a71a2a80764941dd846/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L35
    c.DocInclusionPredicate((s, description) =>
    {
        foreach (var metaData in description.ActionDescriptor.EndpointMetadata)
        {
            if (metaData is IIncludeOpenApi)
            {
                return true;
            }

            if (metaData is ApiVersionAttribute)
            {
                return true;
            }
        }

        return false;
    });
});

builder.Services.AddOptions<SwaggerGenOptions>()
       .Configure<IApiDescriptionGroupCollectionProvider>((options, provider) =>
       {
           var appliationName = typeof(App).GetTypeInfo().Assembly.GetName().Name;

           var applicableApiDescriptions = provider.ApiDescriptionGroups.Items
                                 .SelectMany(group => group.Items);

           foreach (var description in applicableApiDescriptions)
           {
               foreach (var metaData in description.ActionDescriptor.EndpointMetadata)
               {
                   if (metaData is IIncludeOpenApi)
                   {
                       var d = metaData as IIncludeOpenApi;
                   }

                   if (metaData is ApiVersionAttribute)
                   {
                       var attr = metaData as ApiVersionAttribute;

                       foreach (var ver in attr?.Versions)
                       {
                           var info = new OpenApiInfo
                           {
                               Title = $"{appliationName} API {ver}",
                               Version = ver.ToString()
                           };

                           //if (ver.)
                           //{
                           //    info.Description += " This API version has been deprecated.";
                           //}

                           options.SwaggerDoc(appliationName, info);
                       }
                   }
               }

               //options.SwaggerEndpoint(
               // $"/swagger/{description.EndpointMetadata}/swagger.json",
               // description.GroupName.ToUpperInvariant());
           }
       });


builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Api v1");
        // c.RoutePrefix = string.Empty;
    });
}

// app.MapFallback("/swagger");

app.MapGet("/", () => "Todo Web Api");

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapEndpoints();

await app.RunAsync();

public record App();
