using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using inmobiliaria.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>//la api web valida con token
	{
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = configuration["TokenAuthentication:Issuer"],
			ValidAudience = configuration["TokenAuthentication:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
				configuration["TokenAuthentication:SecretKey"])),
		};
		// opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				// Leer el token desde el query string
				var accessToken = context.Request.Query["access_token"];
				// Si el request es para el Hub u otra ruta seleccionada...
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(path.StartsWithSegments("/chatsegurohub") ||
					path.StartsWithSegments("/api/propietarios/reset") ||
					path.StartsWithSegments("/api/propietarios/token")))
				{//reemplazar las urls por las necesarias ruta ⬆
					context.Token = accessToken;
				}
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(
    options => options.UseMySql(
        configuration["ConnectionStrings:MySql"],
        ServerVersion.AutoDetect(configuration["ConnectionStrings:MySql"]))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Usuarios/Login";
    options.LogoutPath = "/Usuarios/Logout";
    options.AccessDeniedPath = "/Usuarios/Restringido";
});

builder.WebHost.UseUrls("http://localhost:5029", "http://*:5500", "http://192.168.0.15:5029");

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("Operador", policy => policy.RequireClaim(ClaimTypes.Name, "Administrador", "Operador"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    //options.AddPolicy("Operador", policy => policy.RequireRole("Operador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
