using StackExchange.Redis;
using WebSignTool;
using IRedis = WebSignTool.IRedis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(WebSignTool.LogFilter));
});

IConfigurationSection Options = builder.Configuration.GetSection("Options");
string dataBaseType = Options.GetValue<string>("DataBaseType");

//if (dataBaseType == "Redis")
//    builder.Services.AddStackExchangeRedisCache(options =>
//    {
//        options.Configuration = builder.Configuration.GetConnectionString("Redis");
//        options.InstanceName = "Redis";
//    });

Redis redis = new(builder.Configuration);
builder.Services.AddSingleton<IRedis>(redis);

Global.AddTelegram(builder, redis);

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "createcert",
    pattern: "{controller=CreatecertController}/{action=Createcert}/{id?}");

app.MapControllerRoute(
    name: "createguid",
    pattern: "{controller=CreateguidController}/{action=Createguid}/{id?}");

app.MapControllerRoute(
    name: "filehash",
    pattern: "{controller=FileHashController}/{action=FileHash}/{id?}");

app.MapControllerRoute(
    name: "signapp",
    pattern: "{controller=SignappController}/{action=Signapp}/{id?}");

app.MapControllerRoute(
    name: "convert",
    pattern: "{controller=ConvertController}/{action=Convert}/{id?}");

app.MapControllerRoute(
    name: "dh",
    pattern: "{controller=DHController}/{action=Dh}/{id?}");

app.MapControllerRoute(
    name: "rsaencrypt",
    pattern: "{controller=RSAEncryptController}/{action=RSAEncrypt}/{id?}");

app.MapControllerRoute(
    name: "rsadecrypt",
    pattern: "{controller=RSADecryptController}/{action=RSADecrypt}/{id?}");

app.MapControllerRoute(
    name: "aesencrypt",
    pattern: "{controller=AESEncryptController}/{action=AESEncrypt}/{id?}");

app.MapControllerRoute(
    name: "aesdecrypt",
    pattern: "{controller=AESDecryptController}/{action=AESDecrypt}/{id?}");

app.MapControllerRoute(
    name: "signfile",
    pattern: "{controller=SignfileController}/{action=Signfile}/{id?}");

app.MapControllerRoute(
    name: "verify",
    pattern: "{controller=VerifyController}/{action=Verify}/{id?}");

app.MapControllerRoute(
    name: "log",
    pattern: "{controller=LogController}/{action=Log}/{id?}");

app.Run(); 
