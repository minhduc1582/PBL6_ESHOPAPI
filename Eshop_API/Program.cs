using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eshop_api.Services.Identities;
using eshop_api.Helpers;
using eshop_api.Service.Products;
using eshop_api.Services.Products;
using eshop_api.Services.Images;
using eshop_api.Services.Orders;
using eshop_pbl6.Authorization;
using eshop_pbl6.Services.Identities;
using eshop_api.Authorization;
using eshop_pbl6.Helpers.Identities;
using System.Text.Json.Serialization;
using eshop_pbl6.Services.Hub;
using Serilog;
using System.Reflection;
using eshop_pbl6.Services.Addresses;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("Default");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
// Add services to the container.
// builder.Services.AddControllersWithViews();
// services.AddDbContext<DataContext>(options =>
//     options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Eshop Electronic API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);

                // Config JWT Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                  new OpenApiSecurityScheme
                                  {
                                      Reference = new OpenApiReference
                                      {
                                          Type = ReferenceType.SecurityScheme,
                                          Id = "Bearer"
                                      }
                                  },
                                 new string[] {}
                            }
                 });
                //
            }
);

services.AddDbContext<DataContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
// services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = false,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]))
//         };
//     });

// Add Depedency
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<IImageService, ImageService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddTransient<IOrderService, OderService>();
services.AddTransient<IOderDetailService, OderDetailService>();
services.AddTransient<IAddressService, AddressService>();
services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.WriteIndented = true;
               options.JsonSerializerOptions.Converters.Add(new CustomJsonConverterForType());
           });

services.AddCors(o =>
                o.AddPolicy("CorsPolicy", policy =>
                    policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/logs.log")
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();


// var loggerFactory = app.Services.GetService<ILoggerFactory>();
// loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString()); 

// Set Port
#nullable enable annotations 
string? PORT = Environment.GetEnvironmentVariable("PORT");
if(!string.IsNullOrWhiteSpace(PORT)) {app.Urls.Add("http://*:"+PORT); }   

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

//app.UseMiddleware<ApiKeyMiddleware>();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/notification");
    endpoints.MapHub<MessageHub>("/notification");
});

app.MapGet("/", () => "ESHOP WEB API PLEASE ACCESS http://localhost:23016/swagger/index.html");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
