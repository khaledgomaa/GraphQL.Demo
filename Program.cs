using GraphQL.Demo;
using GraphQL.Demo.AuthorizationHandlers;
using GraphQL.Demo.DataLoaders;
using GraphQL.Demo.Schema.Mutations;
using GraphQL.Demo.Schema.Queries;
using GraphQL.Demo.Schema.Subscriptions;
using GraphQL.Demo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContextPool<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("appDbCon"))
        .LogTo(Console.WriteLine));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGraphQLServer()
    //.AddAuthorizationCore()
    .RegisterDbContext<AppDbContext>() // Enabling injecting the AppDbContext without the need of using [Service] attributes in method
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorizationHandler<DefaultAuthorizationHandler>()
    .AddInMemorySubscriptions();

builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenSettings = builder.Configuration
                .GetSection("JwtSettings").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenSettings.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = tokenSettings.Audience,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    //ClockSkew = TimeSpan.Zero // enable this line to validate the expiration time below 5mins
                };
            });

//builder.Services.AddAuthorization();

builder.Services
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services
    .AddScoped<InstructorDataLoader>();

builder.Services
    .AddAuthorization();

var app = builder.Build();

//using var dbContext = app.Services.GetRequiredService<AppDbContext>();
//dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.MapGraphQL();
//.RequireAuthorization();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}