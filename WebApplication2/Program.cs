using Microsoft.EntityFrameworkCore;
using ProxyKit;
using WebApplication2.DataBaseContext;
using WebApplication2.Interfaces;
using WebApplication2.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TestApiDB2>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TestDbString")), ServiceLifetime.Scoped);

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IReadersService, ReadersService>();
builder.Services.AddScoped<IRentalService, RentalService>();


builder.Services.AddProxy();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Books"),
applicationBuilder => applicationBuilder.RunProxy(context =>
context.ForwardTo("http://localhost:5141/").AddXForwardedHeaders().Send()));
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Readers"),
applicationBuilder => applicationBuilder.RunProxy(context =>
context.ForwardTo("https://localhost:5141/").AddXForwardedHeaders().Send()));
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Photo"),
applicationBuilder => applicationBuilder.RunProxy(context =>
context.ForwardTo("https://localhost:5141/").AddXForwardedHeaders().Send()));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


