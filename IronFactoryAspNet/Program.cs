using IronFactory;
using IronFactoryAspNet.Data;
using Microsoft.Data.SqlClient;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // UserRepository'i DI konteynerine ekleyin
        builder.Services.AddScoped<UserRepository>();

        // Veritabaný baðlantý dizesini yapýlandýrma dosyasýndan alalým
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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
                    pattern: "{controller=Account}/{action=Login}/{id?}"); // Giriþ sayfasýný varsayýlan olarak ayarladýk.


        app.Run();
        // Aþaðýdaki satýrý ekleyin
    }
}