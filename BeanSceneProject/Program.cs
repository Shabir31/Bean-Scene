using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson.Serialization.Conventions;
using System.Globalization;
using System.Text;

namespace BeanSceneProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-AU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-AU");

            // Configure default MongoDB settings
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            });

            builder.Services.AddControllersWithViews();

            // Adds both cookie and JWT Bearer token based authentication, so that you can still sign in using the website.
            // The policy scheme is used to determine which authentication scheme should be used so that both will work.
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = "JWT_OR_COOKIE";
                o.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,

                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                            // Prevents tokens without an expiry from ever working, as that would be a security vulnerability.
                            RequireExpirationTime = true,

                            // ClockSkew generally exists to account for potential clock difference between issuer and consumer
                            // But we are both, so we don't need to account for it.
                            // For all intents and purposes, this is optional
                            ClockSkew = TimeSpan.Zero
                        };
                    })
                    .AddPolicyScheme("JWT_OR_COOKIE", null, o =>
                    {
                        o.ForwardDefaultSelector = c =>
                        {
                            string auth = c.Request.Headers[HeaderNames.Authorization];
                            if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer "))
                            {
                                return JwtBearerDefaults.AuthenticationScheme;
                            }

                            return IdentityConstants.ApplicationScheme;
                        };
                    });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Turning this off for now for testing purposes
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();


        }
    }
}