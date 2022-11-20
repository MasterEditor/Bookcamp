using System.Reflection;
using Bookstore.Domain.Models;
using Bookstore.Infrustructure;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Bookstore.API;
using Bookstore.API.Converters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Bookstore.Domain.Mongo.Maps;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Bookstore.API.Middlewares;
using Microsoft.AspNetCore.CookiePolicy;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

MongoClassMaps.RegisterMaps();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddInfrustructure();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddServices();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.Configure<BookSettings>(builder.Configuration.GetSection("BookSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["TokenSettings:Audience"],
        ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
        RequireExpirationTime = true,
        ValidateLifetime = true,
        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) => {
            var clonedParameters = validationParameters.Clone();
            clonedParameters.LifetimeValidator = null;
            try
            {
                Validators.ValidateLifetime(notBefore, expires, securityToken, clonedParameters);
            }
            catch
            {
                return false;
            }

            return DateTime.UtcNow < expires && DateTime.UtcNow > notBefore;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
    });

    options.AddPolicy("User", policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypes.Role, "User");
    });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFluentValidationAutoValidation(fv =>
{
    fv.DisableDataAnnotationsValidation = true;
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Bookstore",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{new OpenApiSecurityScheme{Reference = new OpenApiReference
    {
    Type = ReferenceType.SecurityScheme,
    Id = "Bearer"
    }}, new List<string>()}
    });
});

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Bookstore");
    });
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseCors();
app.UseAuthCookieMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();