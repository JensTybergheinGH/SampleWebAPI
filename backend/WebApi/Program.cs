using Microsoft.EntityFrameworkCore;
using WebApi.Model;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(builder =>
{
	builder
		.WithOrigins("http://localhost:4200")
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowCredentials();
});

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//	endpoints.MapHub<NotificationMessageModel>("/notify");
//});
app.MapHub<NotificationMessageModel>("/notify");

app.Run();