using CreateCaptcha;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using note_backend.Helpers;
using note_backend.Models;
using note_backend.Services;
using StackExchange.Redis;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<VerificationCode>();
//JWT
builder.Services.Configure<JWTOption>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    JWTOption jwtObj = builder.Configuration.GetSection("JWT").Get<JWTOption>()!;
    byte[] ketBytes = Encoding.UTF8.GetBytes(jwtObj.SigningKey);
    var secKey = new SymmetricSecurityKey(ketBytes);
    x.TokenValidationParameters = new TokenValidationParameters()
    {

        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secKey
    };
});
//httpcontext
builder.Services.AddHttpContextAccessor();
//Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod());
});
//memory
builder.Services.AddMemoryCache();
builder.Services.Configure<MvcOptions>(option =>
{
    option.Filters.Add<RequestFrequencyFilter>();
    //option.Filters.Add<RegistrationFilter>();
}
);
builder.Services.AddScoped<RegistrationFilter>();
//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var cfig = builder.Configuration.GetSection("redis:localConnection").Value;
    return ConnectionMultiplexer.Connect(cfig);
});
//DbContent Identity
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    var myCon = builder.Configuration.GetConnectionString("localConnection");
    opt.UseMySql(myCon, new MySqlServerVersion(new Version(8, 0, 39)));
});
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 6;
    opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});

var ideBuilder = new IdentityBuilder(typeof(User), typeof(note_backend.Models.Role), builder.Services);
ideBuilder.AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders().AddRoleManager<RoleManager<note_backend.Models.Role>>().AddUserManager<UserManager<User>>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//use cors
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();
