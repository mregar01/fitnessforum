using Microsoft.EntityFrameworkCore;
using fitnessapi.Models;
using fitnessapi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<FitnessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FitnessConnection")));

builder.Services.AddScoped<Parser>();
builder.Services.AddScoped<SqlServerQueryBuilder>();
builder.Services.AddScoped<OpenSearchQueryBuilder>();
builder.Services.AddScoped<SqlServerSearchProvider>();
builder.Services.AddScoped<SqlServerSearchProvider>();
builder.Services.AddScoped<OpenSearchSearchProvider>();
builder.Services.AddScoped<ISearchProviderFactory, SearchProviderFactory>();
builder.Services.AddScoped<Func<string, ISearchProvider>>(c => name =>
{
    if (name == "sql")
    {
        return c.GetRequiredService<SqlServerSearchProvider>();
    }
    if (name == "opensearch")
    {
        return c.GetRequiredService<OpenSearchSearchProvider>();
    }

    throw new NotImplementedException();

});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()          
            .SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();



app.Run();

