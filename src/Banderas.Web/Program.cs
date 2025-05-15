using Banderas.Web.Business.UseCases;
using Banderas.Web.Business.UseCases.Flags;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//ASP.NET CORE IDENTITY
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IFlagUserDetails, FlagUserDetails>();
builder.Services.AddScoped<FlagsUseCases>();

builder.Services.AddScoped<AddFlagUseCase>();
builder.Services.AddScoped<GetPaginatedFlagsUseCase>();
builder.Services.AddScoped<GetSingleFlagUseCase>();
builder.Services.AddScoped<UpdateFlagUseCase>();
builder.Services.AddScoped<DeleteFlagUseCase>();


var app = builder.Build();

//ACTUALITZAR LA BASE DE DADES
using (var scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
