using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false; // Set to true in production
        options.TokenValidationParameters.ValidateAudience = false; // Disable audience validation for simplicity
        options.TokenValidationParameters.NameClaimType = "username";

    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder.Configuration["ClientApp"]!);
    });
});

var app = builder.Build();

app.UseCors();

app.MapReverseProxy();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
    await next();
});
app.UseAuthentication();
app.UseAuthorization();


app.Run();
