using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StudentRegistration.AdminApp.Services;
using StudentRegistration.AdminApp.Services.Course;
using StudentRegistration.AdminApp.Services.Registration;
using StudentRegistration.ViewModel.Validator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IStudentApiClient, StudentApiClient>();
builder.Services.AddTransient<ICourseApiClient, CourseApiClient>();
builder.Services.AddTransient<IRegistrationApiClient, RegistrationApiClient>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireUppercase = false;
});

// Add FluentValidation
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateStudentRequestValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<UpdateStudentRequestValidator>();
});

// Add Validator

// cấu hình HttpClient
builder.Services.AddHttpClient<IUserApiClient, UserApiClient>((httpClient) =>
{
    // thiết lập địa chỉ uri nơi để gọi api
    httpClient.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
});

builder.Services.AddHttpClient<IStudentApiClient, StudentApiClient>((httpClient) =>
{
    // thiết lập địa chỉ uri nơi để gọi api
    httpClient.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
});

builder.Services.AddHttpClient<ICourseApiClient, CourseApiClient>((httpClient) =>
{
    // thiết lập địa chỉ uri nơi để gọi api
    httpClient.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
});

builder.Services.AddHttpClient<IRegistrationApiClient, RegistrationApiClient>((httpClient) =>
{
    // thiết lập địa chỉ uri nơi để gọi api
    httpClient.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
});


// cấu hình HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// cấu hình authentication và authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Thêm các chính sách xác thực khác ...
});

// cấu hình session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
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

app.UseRouting();
app.UseSession();

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
