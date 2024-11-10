using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.IRepository;
using DataAccess.Repository;
using DataAccess.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using CapstoneAuctioneerAPI;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Unicode;
using System.Globalization;
using Net.payOS;
using Hangfire;
using CapstoneAuctioneerAPI.Controller;

/// <summary>
/// Initializes a new instance of the <see cref="$Program" /> class.
/// </summary>
// config PayOS
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
string connectionString = builder.Configuration.GetConnectionString("Capstone");
builder.Services.AddHangfire(configuration =>
    configuration.UseSqlServerStorage(connectionString));
builder.Services.AddCors();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddSingleton(payOS);
builder.Services.AddSession();
builder.Services.AddControllers();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] { }
                        }
                    });
});
builder.Services.AddHttpClient<VerificationController>(client =>
{
    client.BaseAddress = new Uri("https://api.fpt.ai/vision/idr/vnm");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddControllers().AddOData(option => option.Select().Filter().Count().OrderBy().Expand()
.SetMaxTop(100).AddRouteComponents("odata", GetEdmModel()));
builder.Services.AddDbContext<ConnectDB>();
builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<ConnectDB>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AuctionService>();
builder.Services.AddScoped<DigitalSignatureHelper>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<BatchService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAuctioneerRepository, AuctioneerRepository>();
builder.Services.AddScoped<IUserReponsitory, UserReponsitory>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddSignalR();
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOptions:AppId"],
    builder.Configuration["PaypalOptions:AppSecret"],
    builder.Configuration["PaypalOptions:Mode"]
    ));
builder.Services.AddSingleton<IUploadRepository>(sp => new UploadRepository(builder.Environment.ContentRootPath));
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        string validIssuer = builder.Configuration["JWT:ValidIssuer"];
        string validAudience = builder.Configuration["JWT:ValidAudience"];
        string secret = builder.Configuration["JWT:Secret"];

        if (string.IsNullOrEmpty(validIssuer) || string.IsNullOrEmpty(validAudience) || string.IsNullOrEmpty(secret))
        {
            throw new ApplicationException("JWT configuration values cannot be null or empty.");
        }

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero // Loại bỏ thời gian chênh lệch để tránh các vấn đề liên quan đến thời gian sống của token
        };
    });
builder.Services.AddHangfireServer();
// Cấu hình Authorization với các policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN", policy => policy.RequireRole("admin"));
    options.AddPolicy("USER", policy => policy.RequireRole("user"));
});

builder.Services.AddMvc();

var app = builder.Build();
var cultureInfo = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHangfireDashboard();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Project v1"));
app.UseHttpsRedirection();
app.UseODataBatching();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseWebSockets();
app.UseRouting();
app.Use(next => context =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
    {
        return next(context);
    }
    IEnumerable<string> templates;
    IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }
    return next(context);
});
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(enpoints =>
{
    enpoints.MapControllers();
});

app.Run();
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Account>("Account");

    builder.EntityType<Account>().HasKey(e => e.Id);
    return builder.GetEdmModel();
}