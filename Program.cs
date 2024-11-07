using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BoxingAppDiploma.Data;
using BoxingAppDiploma.Models;

var builder = WebApplication.CreateBuilder(args);

// ���������� connection string �� �������������� (�������� �� appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// �������� ��������� �� ������ ����� � ��������� �� EF Core � �������� ���������� ��� ������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(  // ��������� �� ��������� �� �������� �����
            maxRetryCount: 5,             // ���������� ���� �����
            maxRetryDelay: TimeSpan.FromSeconds(10),  // ���������� ����� �� ��������� ����� �������
            errorNumbersToAdd: null)      // ����� �� ������� ���������� ������, ��� � ���������� (�� ������� �������� null)
    )
);

// �������� �������� �� ��������� �� ������ � ������ �� ����������
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// �������� ������ �� ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// �������� ���������� � ������� � Razor ����������
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

app.UseRouting();  // ������������ �� HTTP ������

app.UseAuthentication();  // �������������� (Authentication)
app.UseAuthorization();  // ����������� (Authorization)

// �������� �������� �� ���������� �� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// �������� Razor Pages (��� ��������� Razor Pages, � �� ���� MVC ����������)
app.MapRazorPages();

// ���������� ������������
app.Run();