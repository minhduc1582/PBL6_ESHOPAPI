using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Eshop_API.Repositories.Generics;
using Eshop_API.Repositories.Orders;
using Eshop_API.Services.VNPAY;
using Eshop_API.Repositories.VnPays;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Eshop_API;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using eshop_pbl6.Helpers.Common;
using Eshop_API.Repositories.Addresses;
using Eshop_API.Repositories.Identities;
using Eshop_API.Repositories.Images;
using Eshop_API.Repositories.Products;
using Eshop_API.Helpers.Profiles;
using Eshop_API.Services.Comments;

var builder = WebApplication.CreateBuilder(args);
// configur log
ElasticsearchExtensions.ConfigureLogging();
builder.Host.UseSerilog();
// add sentry
var sentryDsn = builder.Configuration["Sentry:Dsn"]; // <--
builder.WebHost.UseSentry(sentryDsn); 

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
                // using System.Reflection;
               var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //     c.IncludeXmlComments(xmlPath);
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
#region Services
services.AddScoped<IProductService, ProductService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<IImageService, ImageService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddTransient<IOrderService, OderService>();
services.AddTransient<IOderDetailService, OderDetailService>();
services.AddTransient<ICommentService, CommentService>();
services.AddTransient<IAddressService, AddressService>();
services.AddTransient<IVnPayService, VnPayService>();
services.AddAutoMapper(typeof(MapperProfiles).Assembly);
#endregion

#region Repositories
services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddTransient<IOrderRepository, OrderRepository>();
services.AddTransient<IBillPaymentRepository, BillPaymentRepository>();
services.AddTransient<IAddressRepository, AddressRepository>();
services.AddTransient<IUserRepository, UserRepository>();
services.AddTransient<IImageRepository, ImageRepository>();
services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
services.AddTransient<ICategoryRepository, CategoryRepository>();
services.AddTransient<IProductRepository, ProductRepository>();
#endregion
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

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.File("Logs/logs.log")
//     .CreateLogger();
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog();
// 

// add elastic search
services.AddElasticsearch(builder.Configuration);

// Edit reponse annotation
services.AddMvcCore().ConfigureApiBehaviorOptions(options => {
            options.InvalidModelStateResponseFactory = (errorContext) =>
            {
                var errors = errorContext.ModelState.Values.SelectMany(e => e.Errors.Select(m => new
                {
                    ErrorMessage = m.ErrorMessage
                })).ToList();
                // var result = new
                // {
                //     Metadata = new
                //     {
                //         Code = (int)HttpStatusCode.BadRequest,
                //         Message = errors.Select(e => e.ErrorMessage).ToList()
                //     }
                // };
                string messageError = string.Join( "| ", errors.Select(e => e.ErrorMessage).ToList());
                return new OkObjectResult(CommonReponse.CreateResponse(ResponseCodes.BadRequest,messageError,null));
            };
        });

var app = builder.Build();
// tracing sentry
app.UseSentryTracing();

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


app.UseHttpsRedirection();

app.MapGet("/", () => "ESHOP WEB API PLEASE ACCESS http://localhost:23016/swagger/index.html");

app.UseAuthorization();
// app.UseAuthentication();

app.MapControllers();

app.Run();



// void ConfigureLogging()
// {
//     var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//     var configuration = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//         .AddJsonFile(
//             $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
//             optional: true)
//         .Build();

//     Log.Logger = new LoggerConfiguration()
//         .Enrich.FromLogContext()
//         .Enrich.WithMachineName()
//         .WriteTo.Debug()
//         .WriteTo.Console()
//         .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
//         .WriteTo.File("Logs/logs.log")
//         .Enrich.WithProperty("Environment", environment)
//         .ReadFrom.Configuration(configuration)
//         .CreateLogger();
// }

// ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
// {
//     return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
//     {
//         AutoRegisterTemplate = true,
//         IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
//     };
// }