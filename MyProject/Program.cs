using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.Extensions.Options;
using MyProject.Context;
using MyProject.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(Options =>
Options/*.UseLazyLoadingProxies()*/.UseSqlServer(builder.Configuration.GetConnectionString("MyProjectContext")));
builder.Services.AddScoped<EmployeeRepository>(); //untuk menghubungkan main program dengan Employee Repository jika ada Repository bary tinggal tambahin AddScoped lagi.
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<AccountRepository>();

//IMPLEMENTASI CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options
     .AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod());
});

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); // options added on 17-4-2023
    o.RoutePrefix = string.Empty;
});*/

//IMPLEMENTASI CORS
app.UseCors(options => options
.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod());
//--------------------------------

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//app.UseDeveloperExceptionPage();   //Menampilkan error pada saat develop

app.Run();
