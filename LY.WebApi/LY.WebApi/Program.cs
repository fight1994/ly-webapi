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
    o.RequireHttpsMetadata = false;//�Ƿ���ҪHTTPS
    o.SaveToken = true;//�Ƿ���Ϣ�洢��token��

    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateLifetime = true,//�Ƿ���֤����ʱ��
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            bool isExpires = DateTime.UtcNow < expires;
            return isExpires;
        },

        ValidateAudience = false,//�Ƿ���֤������
        //ValidAudience = "",//��֤������

        ValidateIssuer = true,//�Ƿ���֤������
        ValidIssuer = jwtConfig.Issuer,//��֤������

        ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��(����true������֤�ᱻ�۸ģ�ʧȥ��ȫ��)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),//��֤ǩ��
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