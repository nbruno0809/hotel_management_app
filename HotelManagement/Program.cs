using FluentValidation;
using HotelManagement;
using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using HotelManagement.DAL.UserManagement;
using HotelManagement.DTO;
using HotelManagement.Email;
using HotelManagement.Hosting;
using HotelManagement.Services;
using HotelManagement.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using HotelContext = HotelManagement.DAL.HotelContext;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddIdentity<HotelUser,IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HotelContext>().AddDefaultTokenProviders();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];

} ).AddFacebook(facebookOptions =>
{
	facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
	facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});

//Policies
builder.Services.AddAuthorization(optinos =>
{
    optinos.AddPolicy("Admin",p => p.RequireRole(Roles.Administrator));
});

builder.Services.AddRazorPages( options =>
{
    options.Conventions.AuthorizeFolder("/Admin","Admin");
});


//More DI
builder.Services.AddScoped<IAdminCreator,AdminCreator>();
builder.Services.AddTransient<IEmailSender,EmailSender>();
builder.Services.AddScoped<ReservationManagerService,ReservationManagerService>();
builder.Services.AddScoped<CurrencyConverterService,CurrencyConverterService>();
builder.Services.AddScoped<IValidator<ReservationPlan>,ReservationPlanValidator>();

//Email config
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

await app.MigrateDatabase<HotelContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
