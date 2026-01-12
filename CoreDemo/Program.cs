using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>();//bu ikisini identity register iþleminde ekledim
builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
})//yukarýda identityden zorunlu olarak gelen kurallarý kendi isteðimize göre þekillendirebiliyoruz

    .AddEntityFrameworkStores<Context>();

builder.Services.AddMvc(config =>  //aþaðýdaki metod yerine bunu yazdýk o 5.0 versiyonunda geçerli ayný iþlem yine
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddMvc();
builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.LoginPath = "/Login/Index/"; //login indexe gidecek return url komutu
    }
    );
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
    options.AccessDeniedPath = new PathString("/Login/AccessDenied/");
    options.LoginPath = "/Login/Index/";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



/* public void ConfigureServices(IServiceCollection services) //proje seviyesinde authorize iþlemi yaptýk
{
    services.AddControllersWithViews();

    services.AddMvc(config =>
    {
        var policy = new AuthorizationPolicyBuilder()  //bir tane authorization policy builder isimli sýnýfý newleyip buna baðlý olarak otontike (sisteme login olmasý) iþlemini zorunlu kýlan metod  
                       .RequireAuthenticatedUser() //kullanýcýnýn sisteme login olmasý
                       .Build();//oluþtur
        config.Filters.Add(new AuthorizeFilter(policy));//policyden gelen deðeri filtrele
    });
}*/


//app.UseStatusCodePages();//olmayan sayfalarý gösteren sayfa 404 yanii ,Status Code: 404; Not Found  yazýyor sadece

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1", "?code={0}");//error sayfasýna yönlendirecek



app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
// ^
// |
app.MapControllerRoute( //uygulama sunucu üzerinde ayaða kaldýrýldýðý zaman ilk çalýþacak kýsým, biz view in browser dediðimizde hangi viewden çalýþtýrýrsak o sayfa ayaða kalkýyor, ama projeyði canlýya taþýrsak ya da f5 dersek bizi karþýlayan ilk sayfa buradaki sayfa olacak buna ek olarak areayý programa gösterebilmek için ScaffoldingReadMe içinde verilen kodu kullanmamýz gerekiyor 

    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
