using NewStudentAttendanceAPI.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ClassService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7169/");
    // Configure other HttpClient settings if needed
});
builder.Services.AddHttpClient<StudentServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7169/");
    // Configure other HttpClient settings if needed
});
builder.Services.AddHttpClient<AttendanceService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7169/");
    // Configure other HttpClient settings if needed
});

builder.Services.AddHttpClient<StudentServices>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<AttendanceService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
      name: "patch",
        pattern: "Class/Patch/{id}",
        defaults: new { controller = "Class", action = "Patch" });
app.Run();





