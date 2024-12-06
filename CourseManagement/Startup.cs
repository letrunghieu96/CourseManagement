using CourseManagement.Domain;
using CourseManagement.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using System.Data;

/// <summary>
/// Startup
/// </summary>
public class Startup
{
    #region +Fields
    /// <summary>Configuration interface</summary>
    public IConfiguration _configuration { get; }
    #endregion

    #region +Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">Configuration interface</param>
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    #endregion

    #region +ConfigureServices
    /// <summary>
    /// Service collection interface
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">Service collection interface</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        // Connection data
        services.AddScoped((service) => new SqlConnection(_configuration.GetConnectionString("DbConnection")));
        services.AddScoped<IDbTransaction>(service =>
        {
            var connection = service.GetRequiredService<SqlConnection>();
            connection.Open();

            return connection.BeginTransaction(IsolationLevel.ReadCommitted);
        });
        services.RegisterServices();

        // Add distributed memory cache
        services.AddDistributedMemoryCache();
        services.AddSession(option =>
        {
            option.Cookie.Name = "CourseManagement.AspNetCore.Cookies";
            option.IdleTimeout = TimeSpan.FromMinutes(_configuration.GetValue<int>("WebTimeOut"));
            option.Cookie.HttpOnly = true;
            option.Cookie.IsEssential = true;
        });
        services.AddAuthentication(option =>
        {
            option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(option =>
        {
            option.LoginPath = "/";
            option.LogoutPath = "/Index/Logout";
        });
    }
    #endregion

    #region +Configure
    /// <summary>
    /// Configure
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder interface</param>
    /// <param name="env">Web host environment interface</param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        // Middleware
        app.UseMiddleware<SessionTimeoutMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Index}/{action=Index}/{id?}");
        });
    }
    #endregion
}