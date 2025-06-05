namespace DocumentViewerApp
{
    using DocumentViewerApp.Models;
    using DocumentViewerApp.Services;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Bind BlobStorage options and register the BlobStorageService
            builder.Services.Configure<BlobStorageOptions>(
                builder.Configuration.GetSection("BlobStorage"));
            var options = builder.Configuration.GetSection("BlobStorage").Get<BlobStorageOptions>()
                ?? new BlobStorageOptions();
            builder.Services.AddSingleton(new BlobStorageService(options));

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
                pattern: "{controller=Document}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
