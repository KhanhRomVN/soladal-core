using DotNetEnv; // Thêm namespace này
using Microsoft.Extensions.Configuration; // Thêm namespace này
using Microsoft.Extensions.DependencyInjection; // Thêm namespace này
using Microsoft.Extensions.Hosting; // Thêm namespace này

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Env.Load();

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables(); 

        Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}