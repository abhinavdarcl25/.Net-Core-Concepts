using CollegeApp_API.Configurations;
using CollegeApp_API.Data;
using CollegeApp_API.Data.Interfaces;
using CollegeApp_API.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});

//builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using bearer scheme.Enter Bearer [space] add your token in the text input. Example Bearer fdfffe7t564tydty",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string> ()
        }
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
//CORS policy configuration
builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
{
    //Allow all origins
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    //Allow few origis
    //policy.WithOrigins("https://localhost:4200");
    //policy.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod();

}));

string Audience = builder.Configuration.GetValue<string>("Audience");
string Issuer = builder.Configuration.GetValue<string>("Issuer");

//JWT authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"))),
        //ValidateIssuer = false,
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        //ValidateAudience = false,
        ValidateAudience = true,
        ValidAudience = Audience,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyTestCORS");

app.UseAuthorization();

app.MapControllers();

app.Run();
