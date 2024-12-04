using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using BEBoxchat.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connections = new ConcurrentDictionary<string, WebSocket>();

var app = builder.Build();

//configuration web socket
app.UseWebSockets();

app.Map("/chat", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest) return;

    var userId = context.Request.Query["userId"];
    var socket = await context.WebSockets.AcceptWebSocketAsync();
    connections[userId] = socket;

    while (socket.State == WebSocketState.Open)
    {
        var buffer = new byte[1024];
        var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Text)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var targetUser = Util.ExtractTargetUser(message); // Hàm để lấy ID của user nhận
            if (connections.TryGetValue(targetUser, out var targetSocket))
            {
                await targetSocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        else if (result.MessageType == WebSocketMessageType.Close)
        {
            connections.TryRemove(userId, out _);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by user", CancellationToken.None);
        }
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
