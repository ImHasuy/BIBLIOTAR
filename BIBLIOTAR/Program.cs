using BiblioTar.Context;
using BiblioTar.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer("Server=adatb-mssql.mik.uni-pannon.hu,2019;Database=h13_rd7nam;User ID=h13_rd7nam;Password=wfaMpR5+=H;MultipleActiveResultSets=true;TrustServerCertificate=True;");
});

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFineService, FineService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
