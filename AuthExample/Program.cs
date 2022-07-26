using AuthExample.API.Middlewares;
using AuthExample.API.Responses;
using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using AuthExample.Infrastructure.Exceptions;
using AuthExample.Infrastructure.Features.CarFeatures;
using AuthExample.Infrastructure.Repositories;
using AuthExample.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Authorization & Authentication
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters.ValidateIssuer = true;
    o.TokenValidationParameters.ValidIssuer = builder.Configuration.GetValue<string>("iss");
    o.TokenValidationParameters.ValidateIssuerSigningKey = true;
    o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSecret")));
    o.TokenValidationParameters.ValidateAudience = false;
    o.TokenValidationParameters.ValidateLifetime = true;
    o.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    //o.MetadataAddress = $"{authority}/v2.0/.well-known/openid-configuration";
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder(new string[] { JwtBearerDefaults.AuthenticationScheme })
        .RequireAuthenticatedUser().Build());
});

// Interfaces
builder.Services.AddScoped<ICarsRepository, CarsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

//Settings
builder.Services.Configure<JwtSettings>(builder.Configuration);

// Libraries
builder.Services.AddMediatR(typeof(GetAllCarsQuery));

builder.Services.AddApiVersioning();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is UserBlockedException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
            {
                ErrorMessage = exceptionHandlerPathFeature.Error.Message,
                Code = StatusCodes.Status403Forbidden
            }));

        }

        else if(exceptionHandlerPathFeature?.Error is ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            if (exceptionHandlerPathFeature?.Error != null)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                {
                    ErrorMessage = exceptionHandlerPathFeature.Error.Message,
                    Code = StatusCodes.Status400BadRequest
                }));
            }
        }
        else
        {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
            {
                ErrorMessage = "An exception was thrown.",
                Code = StatusCodes.Status500InternalServerError
            }));
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    });
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestCulture();
app.MapControllers();

app.Run();
