﻿using Microsoft.AspNetCore.Authentication.Cookies;
using SV20T1020078.Web;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddMvcOptions(option =>
    {
        option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; // không sử dụng thông báo mặc định
    });
//builder.Services.AddControllersWithViews(); Sử dụng thông báo mặc định
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.Cookie.Name = "AuthenticationCookie";
                    option.LoginPath = "/Account/Login";
                    option.AccessDeniedPath = "/Account/AccessDenined";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                });


builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

ApplicationContext.Configure
(
    httpContextAccessor: app.Services.GetRequiredService<IHttpContextAccessor>(),
    hostEnvironment: app.Services.GetService<IWebHostEnvironment>()
);

string connectionString = @"server=DESKTOP-10KQE7D\THANHPHUC;user id= sa;password=123;database=LiteCommerceDB_Update2023;TrustServerCertificate=true";
SV20T1020078.BusinessLayers.Configuration.Initialize(connectionString); // TRƯỚC KHI CHẠY ỨNG DỤNG KHỞI TẠO THÔNG TIN 
app.Run();
