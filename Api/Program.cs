using System.Reflection;
using System.Text;
using Api;
using Api.Extensions;
using FluentValidation;
using Infraestructure.Context;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Notification;
using Services.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    //TODO add support for versions
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "MotoRent Api",
                        Contact = new OpenApiContact() { Name = "Tiago Cirelli", Email = "tiagocirelli@hotmail.com" },
                        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/mit/") }
                    });

                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Insert the JWT token in this way: Bearer {your token}",
                        Name = "Authorization",
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    //opt.IncludeXmlComments(xmlPath);
                })

                .AddDbContext<DataContext>(options =>
                {
                    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new Exception("ConnectionString \"DefaultConnection\" not found!");

                    connectionString = Environment.ExpandEnvironmentVariables(connectionString);

                    options
#if DEBUG
                        .UseLoggerFactory(LoggerFactory.Create(p => p.AddConsole()))
                        .EnableSensitiveDataLogging()
#endif
                        .UseNpgsql(connectionString);
                })

                .AddAutoMapper(typeof(AutoMapperProfiles.Profiles))

                .Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)))

                .AddScoped<CurrentUser>(s => new CurrentUserModelBinder(s.GetService<IHttpContextAccessor>()).BindModel())

                .AddScoped<IRepositoryWrapper, RepositoryWrapper>()
                .AddScoped<AuthService>()
                .AddScoped<DriverService>()
                .AddScoped<MotorbikeService>()
                .AddScoped<OrderService>()
                .AddScoped<RentPlanService>()
                .AddScoped<RentService>()
                .AddScoped<Services.NotificationService>()
                .AddScoped<Notification.NotificationService>()

                .AddValidatorsFromAssemblyContaining<MotorbikeValidator>()

                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

builder.Services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)));

IConfigurationSection tokenSettingsSection = builder.Configuration.GetSection(nameof(TokenSettings));
builder.Services.Configure<TokenSettings>(tokenSettingsSection);
TokenSettings tokenSettings = tokenSettingsSection.Get<TokenSettings>()!;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Key!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = tokenSettings.Audience,
        ValidIssuer = tokenSettings.Issuer
    };
});

ValidatorOptions.Global.LanguageManager = new CustomLanguageManager
{
    Culture = new System.Globalization.CultureInfo("en")
};

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
