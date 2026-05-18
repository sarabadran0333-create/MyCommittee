using Microsoft.EntityFrameworkCore;
using MyCommittee.Data;
using MyCommittee.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor(); //--------------------
builder.Services.AddSession();// -------------------------------------------------------------------------------------------------


// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ApplicationDbContext with SQL Server provider using DefaultConnection
builder.Services.AddDbContext<MyCommittee.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ICommitteeRepository, CommitteeRepository>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IDecisionRepository, DecisionRepository>();
builder.Services.AddScoped<IMinutesOfMeetingRepository, MinutesOfMeetingRepository>();



// 1.??? ?? ??????? ?? appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ????? ??? DbContext ??????? SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
