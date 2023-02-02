using Microsoft.AspNetCore.SignalR.Client;

namespace AN016;

internal class Program
{
    static async Task Main(string[] args)
    {
        #region 建立與產生 SignalR Client 端的物件
        HubConnection connection = new HubConnectionBuilder()
            // 配置 HubConnection 以使用基於 HTTP 的傳輸連接到指定的 URL
            .WithUrl("https://localhost:7153/myChatHub")
            .Build();
        #endregion

        #region 綁定 SignalR 相關事件
        #region 當要結束或者網路發生異常，導致與後端 SignalR 伺服器連線中斷時機，將會觸發此事件
        connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);

            #region 若發生網路異常，需要重新建立連線
            try
            {
                // 如何判斷是正常要結束，還是網路突然斷線
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                // Todo : 如何進行再次嘗試連線，直到成功為止呢？
                Console.WriteLine(ex.Message);
            }
            #endregion
        };
        #endregion

        #region 綁定伺服器上有特定訊息要被發送出來的時機，將會觸發此事件
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            var newMessage = $"來自伺服器的訊息> {user}: {message}";
            Console.WriteLine(newMessage);
        });
        #endregion
        #endregion

        #region 啟動與後端 SignalR 伺服器進行連線
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connection started");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        #endregion

        #region 開始對 SignalR 伺服器送出訊息
        try
        {
            // SignalR 伺服器上需要能夠提供 SendMessage 這個服務方法
            await connection.InvokeAsync("SendMessage",
                "Vulcan", "Hello everyone");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        #endregion


        Console.WriteLine("Press any key for continuing...");
        Console.ReadKey();
    }
}