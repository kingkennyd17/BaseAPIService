using Fintrak.Data.Interface;
using Fintrak.Data.SystemCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fintrak.Host.ServicePortal;
using Fintrak.Service.SystemCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Host.ServicePortal2._0.Middleware;
using Fintrak.Shared.Common.Base;
using Fintrak.Shared.Common.Interface;
using Fintrak.Data.SystemCore.Interface;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Fintrak.Shared.Common.Helper;
using Fintrak.Host.ServicePortal2._0;
using Fintrak.Model.SystemCore.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<SystemCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging()
       .LogTo(Console.WriteLine, LogLevel.Information));
//builder.Services.AddDbContext<IdentityDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserSetup, Roles>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<SystemCoreDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Service API", Version = "v2", Description = "Fintrak Host Service Portal API" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
});

builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped(typeof(IDataRepository<>), typeof(DataRepositoryBase<>));
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISystemCoreManager, SystemCoreManager>();
builder.Services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();
builder.Services.AddScoped<ISystemCoreManager, SystemCoreManager>();
builder.Services.AddScoped<IAudittrailRepository, AudittrailRepository>();
builder.Services.AddScoped<IUserSetupRepository, UserSetupRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
//builder.Services.AddScoped<IMenuRoleRepository, MenuRoleRepository>();
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddScoped<UserManager<UserSetup>, ApplicationUserManager>();
builder.Services.AddScoped<SignInManager<UserSetup>, ApplicationSignInManager>();
builder.Services.AddScoped<RoleManager<Roles>, ApplicationRoleManager>();
builder.Services.AddScoped<UserContextService>();

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Fintrak Service"));
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


// Add global exception handling middleware
app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<UserSetup>>();
        var roleManager = services.GetRequiredService<RoleManager<Roles>>();
        var seeder = new DataSeeder(userManager, roleManager);
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
