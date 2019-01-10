using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NFineCore.Support;
using NFineCore.EntityFramework;
using NFineCore.EntityFramework.Models;
using NFineCore.Service.SystemManage;
using SharpRepository.Ioc.Microsoft.DependencyInjection;
using NFineCore.Web.Filters;
using Hangfire;
using Hangfire.MySql;
using System.Transactions;

namespace NFineCore.Web
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddStaticHttpContextAccessor();
            
            services.AddDistributedRedisCache(options => { options.Configuration = "localhost"; options.InstanceName = "NFineCore"; });
            //services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(7200);//Session过期时间为1小时(60*60)
            });
            services.AddAutoMapper();
            // 添加Cookie服务
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => false;     //这里改成false
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddMvc(option =>
            {
                option.Filters.Add(new LoginAuthFilter());
                //option.Filters.Add(new OperateLogFilter());
            });
            services.AddHangfire(x => x.UseStorage(
                new MySqlStorage(
                    "Server=localhost;Database=nfinecorebase;User Id=root;Password=123456;Allow User Variables=True;",
                    new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "hangfire_"
                    })));
            services.AddTransient(typeof(LoginLogService));
            services.AddTransient(typeof(OperateLogService));

            services.AddDbContext<NFineCoreDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            //services.AddDbContext<NFineDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            return services.UseSharpRepository(Configuration.GetSection("sharpRepository"), "efCore");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NFineCoreDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }            

            app.UseAuthentication();//使用Cookie的中间件
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "SystemManage",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "WeixinManage",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "ExampleManage",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            #region Hangfire 定时任务 
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<OperateLogService>(a => a.TestAsync1(), "*/1 * * * *");  //间隔1分钟执行
            RecurringJob.AddOrUpdate<OperateLogService>(a => a.TestAsync2(), "*/2 * * * *");  //间隔2分钟执行
            RecurringJob.AddOrUpdate<OperateLogService>(a => a.TestAsync3(), "*/3 * * * *");  //间隔3分钟执行
            RecurringJob.AddOrUpdate<OperateLogService>(a => a.TestAsync4(), "*/4 * * * *");  //间隔4分钟执行
            RecurringJob.AddOrUpdate<OperateLogService>(a => a.TestAsync5(), "*/5 * * * *");  //间隔5分钟执行
            #endregion

            app.UseStaticHttpContext();
            AutoMapperConfig.RegisterMappings();
        }
    }
}
