using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using UiIoT.Hubs;
using UiIoT.Models;
using static UiIoT.Models.BufferExtensions;

namespace UiIoT
{
    public class Program
	{
		public static void Main(string[] args)
		{
			
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddSignalR();
            builder.Services.AddSingleton(_ => {
                var buffer = new Buffer<Point>(10);
                // start with something that can grow
                for (var i = 0; i < 7; i++)
                    buffer.AddNewRandomPoint();

                return buffer;
            });

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
			
			app.MapHub<RandomDataHub>("/iot/robots", options =>
            {
                options.Transports =
                    HttpTransportType.WebSockets |
                    HttpTransportType.LongPolling;
            });
			app.Run();
		}
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}