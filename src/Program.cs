using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Extensions;
using src.Services;

var allowSpecificOrigins = "_specificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins, policy => policy.WithOrigins("http://localhost:5173").WithOrigins("https://cam.sirat.me").AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Migrate db
// await using var scope = app.Services.CreateAsyncScope();
// await using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
// await db.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseCors(allowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

