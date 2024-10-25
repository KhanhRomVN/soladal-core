global using soladal_core.Data;
// using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
// using soladal_core.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this line
using Microsoft.IdentityModel.Tokens; // Add this line
using System.Text; // Add this line

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    new MySqlServerVersion(new Version(8, 0, 21)))); 

// builder.Services.AddRateLimiter(options =>
// {
//     options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext>(context =>
//         RateLimitPartition.GetFixedWindowLimiter("global", TimeSpan.FromMinutes(1), 10));
// });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        // Ensure this key is at least 16 characters long (128 bits)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_here_12345")), // Example key 
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(); 

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

app.UseAuthentication(); 
app.UseAuthorization(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.MapControllers(); 

app.Run();
