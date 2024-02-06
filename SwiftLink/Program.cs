using Microsoft.EntityFrameworkCore;
using SwiftLink.Services;
using SwiftLink;
using SwiftLink.Models;
using SwiftLink.Entities;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Database")));
//builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<UrlShorteningService>();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    AppDbContext dbContext,
    HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified URL is invalid");
    }
    var code = await urlShorteningService.GenerateUniqueCode();
    var shortenedUrl = new ShortenedURL()
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedOnUtc = DateTime.Now
    };

    dbContext.ShortenedUrls.Add(shortenedUrl);
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});



app.MapGet("api/{code}", async (string code, AppDbContext dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

    if (shortenedUrl is null)
    {
        return Results.NotFound();
    }

    return Results.Redirect(shortenedUrl.LongUrl);
});

app.MapGet("api/getAll", async (AppDbContext dbContext) =>
{
    var urlsFromDb = await dbContext.ShortenedUrls.ToListAsync();
    var models = new List<UrlModel>();
    foreach (var url in urlsFromDb)
    {
        var model = new UrlModel
        {
            CreatedAt = url.CreatedOnUtc,
            ShortUrl = url.ShortUrl,
            LongUrl = url.LongUrl
        };
        models.Add(model);
    }
    return Results.Ok(models);
    
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwiftLinkApi v1");
});
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();




app.Run();

