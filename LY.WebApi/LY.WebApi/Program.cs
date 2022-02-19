using Autofac.Extensions.DependencyInjection;
using LY.Common.Helper;
using LY.Model.Share;
using LY.WebApi.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var appsettingsHelper = new AppsettingsHelper();
var jwtConfig = appsettingsHelper.Get<JwtConfigModel>("JWT");
var appTitle = appsettingsHelper.Get<string>("AppTitle");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
string apiVersion = "v1";
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc(apiVersion, new OpenApiInfo { Title = appTitle, Version = apiVersion });
    o.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "Bearer token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = ""
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
            },
            new List<string>()
         }
     });
});

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;//是否需要HTTPS
    o.SaveToken = true;//是否将信息存储在token中

    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateLifetime = true,//是否验证过期时间
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            bool isExpires = DateTime.UtcNow < expires;
            return isExpires;
        },

        ValidateAudience = false,//是否验证订阅人
        //ValidAudience = "",//验证订阅人

        ValidateIssuer = true,//是否验证发布人
        ValidIssuer = jwtConfig.Issuer,//验证发布人

        ValidateIssuerSigningKey = true,//是否验证签名(建议true，不验证会被篡改，失去安全性)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),//验证签名
    };
});

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// IOC
builder.AddAppServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();