using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BoxingAppDiploma.Data;
using BoxingAppDiploma.Models;

var builder = WebApplication.CreateBuilder(args);

// �������� ��������� �� ������ ����� � �������� �� Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// �������� ������ �� Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// �������� ������������ � ������� � Razor ����������
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// ������������� �� HTTP ��������
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();  // �������� � ����� �� ����������
}
else
{
    app.UseExceptionHandler("/Home/Error");  // ����������� �� ������ � �������������� �����
    app.UseHsts();  // �������� HTTP Strict Transport Security
}

app.UseHttpsRedirection();  // ������������ ��� HTTPS
app.UseStaticFiles();  // �������� ������� (CSS, JS, �����������)

app.UseRouting();  // ������������ �� ������

app.UseAuthentication();  // ��������������
app.UseAuthorization();  // �����������

// �������� �������� �� ���������� �� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// �������� Razor Pages, ��� �� �����
app.MapRazorPages();

// ���������� ������������
app.Run();