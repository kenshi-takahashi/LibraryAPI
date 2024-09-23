using Hangfire;
using LibraryAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomLogging(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddMapper();
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomServices(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryAPIDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithDarkTheme();
}

app.UseHangfireDashboard();
app.UseHangfireServer();

RecurringJob.AddOrUpdate<IBookService>(
    "check-return-dates",
    service => service.NotifyUsersAboutReturnDatesAsync(),
    Cron.Daily);

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
