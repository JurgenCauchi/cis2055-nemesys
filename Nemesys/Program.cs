using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nemesys.Data;
using Nemesys.Repositories.Interfaces;
using Nemesys.Repositories;
using Microsoft.AspNetCore.Identity;
using Nemesys.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // For IdentityDbContext
using IEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NemesysContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("NemesysContext") ?? throw new InvalidOperationException("Connection string 'NemesysContext' not found.")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<NemesysContext>().AddDefaultTokenProviders();

//builder.Services.AddIdentity<AppUser, IdentityRole>()
//   .AddEntityFrameworkStores<NemesysContext>()
//   .AddDefaultTokenProviders(); // Fixed missing semicolon  

// Add services to the container.  
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IInvestigationRepository, InvestigationRepository>();

// Bind configuration to AuthMessageSenderOptions
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("Email:Smtp"));

// Register the email sender
builder.Services.AddTransient<IEmailSender, EmailSender>();


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


app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
