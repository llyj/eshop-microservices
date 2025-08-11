using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Duende.IdentityModel;
using Identity.API.Extensions;
using Identity.API.Extensions.AuthenticationExtensions;
using Identity.API.Extensions.AuthorizationExtensions;
using Identity.API.Infrastructure;
using Identity.API.Middleware;
using Identity.Domain;
using Identity.Infrastructure;
using MicroServiceDemo.Infrastructure.Core.Extensions;
using MicroServiceDemo.Infrastructure.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCommon.Core.Extensions;
using NetCommon.FreeRedis.Config;
using NetCommon.FreeRedis.Extensions;
using NetCommon.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.AllowTrailingCommas = JsonExtension.DefaultJsonSerializerOptions.AllowTrailingCommas;
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonExtension.DefaultJsonSerializerOptions.DefaultIgnoreCondition;
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    //opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    //opt.JsonSerializerOptions.PropertyNameCaseInsensitive=
    opt.JsonSerializerOptions.Encoder = JsonExtension.DefaultJsonSerializerOptions.Encoder;
    foreach (var jsonConverter in JsonExtension.DefaultJsonSerializerOptions.Converters)
    {
        opt.JsonSerializerOptions.Converters.Add(jsonConverter);
    }
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        //In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description = "JWT Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {new OpenApiSecurityScheme(){Reference = new OpenApiReference(){Id = "bearer",Type = ReferenceType.SecurityScheme}},new List<string>()}
    });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(SysUser).Assembly, typeof(Program).Assembly);

    cfg.AddOpenBehavior(typeof(IdentityDbContextTransactionBehavior<,>));
});

builder.Services.AddCap(options =>
{
    //options.UseEntityFramework<IdentityDbContext>();

    options.UsePostgreSql(builder.Configuration.GetSection("Database:ConnectionString").Value!);

    options.UseRabbitMQ(opt =>
    {
        builder.Configuration.GetSection("RabbitMQ").Bind(opt);
    });
});

builder.Services.AddDbContextPool<IdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

builder.Services.AddRepositories();

builder.Services.AddJwtConfig(builder.Configuration.GetSection("JwtOption"));

var jwtOption = builder.Configuration.GetSection("JwtOption").Get<JwtOption>();

// 禁止 Claims 类型默认映射
//JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
//JsonWebTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(options =>
{
    //options.DefaultAuthenticateScheme = CustomAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.AddScheme<CustomAuthenticationHandler>(CustomAuthenticationDefaults.AuthenticationScheme, "");
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    // 清除 Claims 类型映射
    //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    //JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
    //JsonWebTokenHandler.DefaultMapInboundClaims = false;
    // 使用默认的token校验
    //opt.UseSecurityTokenValidators = true;
    // 禁止 Claims 类型默认映射
    opt.MapInboundClaims = false;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        NameClaimType = JwtClaimTypes.Id,
        RoleClaimType = JwtClaimTypes.Role,
        ValidateIssuer = true,
        ValidIssuer = jwtOption.Issuer,
        ValidateAudience = jwtOption.AudienceList != null,
        ValidAudiences = jwtOption.AudienceList,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SignatureKey)),
        ValidateLifetime = true,
    };
    opt.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Query.TryGetValue(jwtOption.AccessTokenQueryName, out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        },

        //OnAuthenticationFailed = async context =>
        //{
        //    var redisRepository = context.HttpContext.RequestServices.GetService<IRedisRepository>();

        //    redisRepository.

        //    foreach (var tokenHandler in context.Options.TokenHandlers)
        //    {
        //        await tokenHandler.ValidateTokenAsync("", context.Options.TokenValidationParameters);
        //    }
        //    //context.Result =
        //    //return Task.CompletedTask;
        //},

        //OnChallenge = async context =>
        //{
        //    if (context.AuthenticateFailure is SecurityTokenExpiredException)
        //    {
        //        (context.AuthenticateFailure as SecurityTokenExpiredException)!.Expires =
        //            (context.AuthenticateFailure as SecurityTokenExpiredException)!.Expires.ToLocalTime();
        //    }
        //    //await context.Response.WriteAsJsonAsync(new { code = 100 });
        //}
    };
});

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(PermissionAuthorization.AuthPolicy, opt => opt.AddRequirements(new PermissionAuthorizationRequirement()));
});

builder.Services.AddFreeRedis(new RedisConfig()
{
    ConnectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value!,
    JsonDeserialize = (s, type) => JsonSerializer.Deserialize(s, type, JsonExtension.DefaultJsonSerializerOptions),
    JsonSerialize = o => JsonSerializer.Serialize(o, JsonExtension.DefaultJsonSerializerOptions)
});

builder.Services.AddMigration<IdentityDbContext, IdentityDbContextSeed>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var identityDbContext = scope.ServiceProvider.GetService<IdentityDbContext>();

    identityDbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandleMiddleware>();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();