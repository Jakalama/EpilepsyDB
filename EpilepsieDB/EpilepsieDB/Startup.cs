using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EpilepsieDB.Data;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Repositories;
using EpilepsieDB.Repositories.Impl;
using EpilepsieDB.Models;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using EpilepsieDB.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using EpilepsieDB.Source.Wrapper;

namespace EpilepsieDB
{
    public class Startup
    {
        private IWebHostEnvironment currentEnvironment;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Use Release Configuration");

            SetCookiePolicies(services);
            InjectServices(services);

            SetDB(services);

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            ConfigureAuthorization(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            Console.WriteLine("Use Development Configuration");

            SetCookiePolicies(services);
            InjectServices(services);

            SetDB(services);
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Epilepsy EDF-File API",
                    Description = "An ASP.NET Core API for accessing epilepsy scan files",
                    TermsOfService = new Uri("https://example.com/terms")
                });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            // maybe add a cors policy
            //services.AddCors(o => o.AddPolicy("Policy", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //}));

            ConfigureAuthorization(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            currentEnvironment = env;

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                // swaggger default rout is /swagger/index.html
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseCors("Policy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                // add streaming endpoint later here
            });
        }

        private void SetCookiePolicies(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict;

                // important!
                // ensures that accessDenied page can be found!
                services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = $"/Identity/Account/Logout";
                    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                });
            });
        }

        private void InjectServices(IServiceCollection services)
        {
            // inject DbContext
            services.AddScoped<IAppDbContext>(provider => provider.GetService<EpilepsieDBContext>());

            // inject IPrincipal
            services.AddHttpContextAccessor();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            // jwt tokenservice
            services.AddTransient<ITokenService, JwtService>();

            // inject data repositories
            services.AddTransient<IRepository<Patient>, PatientRepository>();
            services.AddTransient<IRepository<Recording>, RecordingRepository>();
            services.AddTransient<IRepository<Scan>, ScanRepository>();
            services.AddTransient<IRepository<Block>, BlockRepository>();
            services.AddTransient<IRepository<Signal>, SignalRepository>();
            services.AddTransient<IRepository<Annotation>, AnnotationRepository>();

            // inject app services
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IScanService, ScanService>();
            services.AddTransient<IBlockService, BlockService>();
            services.AddTransient<ISignalService, SignalService>();
            services.AddTransient<IRecordingService, RecordingService>();
            services.AddTransient<IAnnotationService, AnnotationService>();

            services.AddTransient<IFileService, FileService>(x => 
                new FileService(
                    new FileSystemWrapper(),
                    AppDomain.CurrentDomain.BaseDirectory,
                    currentEnvironment.WebRootPath));
            services.AddTransient<IDownloadService, DownloadService>(x => 
                new DownloadService(
                    x.GetService<IRepository<Patient>>(),
                    x.GetService<IRepository<Recording>>(),
                    x.GetService<IRepository<Scan>>(),
                    x.GetService<IFileService>(),
                    new FileSystemWrapper(),
                    currentEnvironment.WebRootPath));
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IEdfService, EdfService>();
            services.AddTransient<IUsersService, UsersService>();

            // email service
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IEmailService, EmailService>();
        }

        private void SetDB(IServiceCollection services)
        {
            services.AddDbContext<EpilepsieDBContext>(options => options.UseNpgsql(GetConnectionString()));
        }

        private string GetConnectionString()
        {
            string host = Configuration["DB_HOST"];
            string port = Configuration["DB_PORT"];
            string name = Configuration["DB_NAME"];
            string user = Configuration["DB_USER"];
            string pass = Configuration["DB_PASS"];

            return $"Server={host};Port={port};Database={name};User Id={user};Password={pass};";
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            // configure identity settings
            services.AddDefaultIdentity<IdentityUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                    /* set password requirements here */
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<EpilepsieDBContext>()
                .AddDefaultTokenProviders();

            // configure jwt token usage
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])
                        )
                    };
                });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(1));
        }
    }
}
