using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentRegistration.Application.Courses;
using StudentRegistration.Application.Registrations;
using StudentRegistration.Application.Students;
using StudentRegistration.Application.Users;
using StudentRegistration.Data.EF;
using StudentRegistration.Data.Entities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<StudentDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddDbContext<StudentDbContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDb")));
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<UserManager<User>, UserManager<User>>();
//builder.Services.AddTransient<RoleManager<User>, RoleManager<User>>();
builder.Services.AddTransient<SignInManager<User>, SignInManager<User>>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<ICourseService, CourseService>();


// Add Swagger to the services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentRegistrationSystem API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Add JWT authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.RequireHttpsMetadata = false;
//            options.SaveToken = true;
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                ValidAudience = builder.Configuration["Jwt:Issuer"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//            };
//        });

//builder.Services.AddAuthorization(options =>
//{
//    options.DefaultPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();

//    // Thêm các chính sách xác thực khác ...
//});

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// cấu hình middleware

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentRegistrationSystem API");
});
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(enpoints =>
{
    enpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
