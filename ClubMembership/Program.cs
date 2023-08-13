using Application.Services;
using CloudinaryAdapter;
using CloudinaryDotNet;
using ClubMembership.Middlewares;
using Domain.Interfaces.Adapters;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Repositories.Repository;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var cloudinaryUri = builder.Configuration.GetSection("Cloudinary:CLOUDINARY_URL") ?? throw new ArgumentException("not find CloudinaryURL schema");
Cloudinary cloudinary = new Cloudinary(cloudinaryUri.Value);
cloudinary.Api.Secure = true;
builder.Services.AddScoped(opts => cloudinary);

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddSession(opts => opts.IdleTimeout = TimeSpan.FromMinutes(30));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<AppExceptionMiddleware>();

app.MapRazorPages();

app.Run();
