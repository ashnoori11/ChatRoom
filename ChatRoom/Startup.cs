using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.Services;
using ChatRoom.Data.Context;
using ChatRoom.IoC.Dependencies;
using ChatRoom.SignalRHubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ChatRoom
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChatRoomContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("chatProjectConnectionString")));
            services.ChatRoomServiceRegistery();
            services.AddSignalR();
            services.AddRazorPages();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie(options =>
            //{
            //    options.LoginPath = "/Login";
            //    options.AccessDeniedPath = "/Login";
            //});

           // services.AddAuthentication(IdentityConstants.ApplicationScheme)
           services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>
                  {
                      options.LoginPath = "/Login";
                      options.ExpireTimeSpan = new System.TimeSpan(100, 10, 0);
                      options.SlidingExpiration = true;

                      options.Cookie = new CookieBuilder
                      {
                          SameSite = SameSiteMode.Strict,
                          SecurePolicy = CookieSecurePolicy.Always,
                          IsEssential = true,
                          HttpOnly = true
                      };
                      options.Cookie.Name = "MyCookie";
                  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthorization();

            //app.UseCors(builder =>
            //{
            //    builder.WithOrigins("server address")
            //    .AllowAnyHeader()
            //    .WithMethods("GET", "POST")
            //    .AllowCredentials();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<AgentHub>("/agentHub");
            });
        }
    }
}
