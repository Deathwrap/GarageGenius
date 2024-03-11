using System.Text;
using System.Text.Json.Serialization;
using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Repository.Cars;
using Deathwrap.GarageGenius.Repository.Clients;
using Deathwrap.GarageGenius.Repository.RefreshTokens;
using Deathwrap.GarageGenius.Repository.ServiceCategories;
using Deathwrap.GarageGenius.Repository.Services;
using Deathwrap.GarageGenius.Service.Cars;
using Deathwrap.GarageGenius.Service.Clients;
using Deathwrap.GarageGenius.Service.Email;
using Deathwrap.GarageGenius.Service.RefreshTokens;
using Deathwrap.GarageGenius.Service.Services;
using Deathwrap.GarageGenius.Service.Token;
using Deathwrap.GarageGenius.Service.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDataAccess, DataAccess>();
builder.Services.AddTransient<IServiceCategoryRepository, ServiceCategoryRepository>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();
builder.Services.AddTransient<IServicesService, ServicesService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IClientsService, ClientsService>();
builder.Services.AddTransient<IClientsRepository, ClientsRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IRefreshTokensRepository, RefreshTokensRepository>();
builder.Services.AddTransient<ICarsService, CarsService>();
builder.Services.AddTransient<ICarsRepository, CarsRepository>();
builder.Services.AddTransient<IRefreshTokensService, RefreshTokensService>();

builder.Services.AddHttpContextAccessor();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
            ValidAudience = builder.Configuration["Jwt:Audience"]!,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ClockSkew = TimeSpan.Zero,
        };
   
    });
builder.Services.AddAuthorization(options => 
                      options.DefaultPolicy = new AuthorizationPolicyBuilder
                              (JwtBearerDefaults.AuthenticationScheme)
                          .RequireAuthenticatedUser()
                          .Build());
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "GarageGenius", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Необходим Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
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
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();