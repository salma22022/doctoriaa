using Project.Models;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Project.Middlewares;
using Project.Services;
using Microsoft.AspNetCore.Identity;
namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Information()
                     .Enrich.FromLogContext()
                    .WriteTo.File("accesslogs.txt") // Log to file
                    .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<VeseetaDBContext>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<VeseetaDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Local")));
            builder.Services.AddScoped<IDayRepository,DayRepository>();
            
            
            
            var app = builder.Build();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Request Completed {Protocol} {Method} {Url} - {StatusCode} in {Elapsed:0.0000} ms";
            });
            //app.Use(async (context, next) =>
            //{
            //    try
            //    {


            //        Console.WriteLine($"{context.Request.Path}");
            //        await next(context);

            //        Log.Information($"Request finished  HTTP/" + 
            //            $"{context.Response.} " +
            //            $" {context.Request.Method} {context.Request.Path}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error($"Execprion:  {ex.Message} {context.Request.Path} {context.Request.Method} {context.Response.StatusCode}");
            //    }

            //});





            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseMiddleware<RequestLogger>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            //add 404 page
            app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");
            app.Run();
        }
    }
}
