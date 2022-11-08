using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repository;
using UKParliament.CodeTest.Data.Repository.Interfaces;
using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Web.Middleware;

namespace UKParliament.CodeTest.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            builder.Services.AddDbContext<PersonManagerContext>(op =>
            {
                op.UseInMemoryDatabase("PersonManager");
                op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddSingleton<CurrentTime>(() => DateTime.Now);
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IPersonValidator, PersonValidator>();
            builder.Services.AddScoped<IPersonMapper, PersonMapper>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorResponseWrapper>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}