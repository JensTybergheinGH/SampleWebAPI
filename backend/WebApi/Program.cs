using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using WebApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
builder.Services.AddSingleton<WebSocketManager>();

var app = builder.Build();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
	app.UseWebSockets();

	app.Map("/ws", HandleWebSocket);
}

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
{
	// Logic to handle WebSocket connections
	// For simplicity, you can store the WebSocket instances somewhere to broadcast messages later

	// Example:
	// StoreWebSocketInstance(webSocket);

	// You may want to handle disconnections and clean up stored WebSocket instances
}