﻿using SV20T1020078.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(); //MVC

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(60);
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
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
ApplicationContext.Configure
    (
        httpContextAccessor : app.Services.GetRequiredService<IHttpContextAccessor>(),
        hostEnvironment: app.Services.GetService<IWebHostEnvironment>()
    );
string connectionString = @"server=DESKTOP-10KQE7D\THANHPHUC;user id= sa;password=123;database=LiteCommerceDB_Update2023;TrustServerCertificate=true";
SV20T1020078.BusinessLayers.Configuration.Initialize(connectionString); // TRƯỚC KHI CHẠY ỨNG DỤNG KHỞI TẠO THÔNG TIN 
app.Run();
