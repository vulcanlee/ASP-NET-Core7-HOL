using Microsoft.AspNetCore.SignalR;

namespace AN015.Hubs;

public class MyChatHub : Hub
{
    // 提供 SignalR 用戶端可以對此伺服器呼叫的方法
    public async Task SendMessage(string user, string message)
    {
        // 使用 Clients.All 屬性，對所有連線上的 SignalR 用戶，發出訊息
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
