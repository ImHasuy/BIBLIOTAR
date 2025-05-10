using BiblioTar.Context;
using BiblioTar.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{

    //optionsBuilder.UseSqlServer("Server=adatb-mssql.mik.uni-pannon.hu,2019;Database=h13_rd7nam;User ID=h13_rd7nam;Password=wfaMpR5+=H;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    optionsBuilder.UseSqlServer("Server=localhost;Database=Bibliotar;Trusted_Connection=True;TrustServerCertificate=True;");
    //optionsBuilder.UseSqlServer("Server=desktop-lln5qik\\SQLEXPRESS;Database=Bibliotar;Trusted_Connection=True;TrustServerCertificate=True;");
    //optionsBuilder.UseSqlServer("Server=Mati\\SQLEXPRESS;Database=Bibliotar;Trusted_Connection=True;TrustServerCertificate=True;");

});

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFineService, FineService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var Jwt = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Jwt["Issuer"],
            ValidAudience = Jwt["Audience"],
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt["SecretKey"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("StaffPolicy", policy => policy.RequireRole("Librarian", "Administrator"));
    options.AddPolicy("AllUserPolicy", policy => policy.RequireRole("Customer", "Librarian", "Administrator"));
    
});

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetpincerApp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
});

app.MapControllers();

app.Run();
