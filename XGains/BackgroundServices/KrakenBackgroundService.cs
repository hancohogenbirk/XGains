using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using XGains.Hubs;
using XGains.Models.Request.Kraken;

namespace XGains.BackgroundServices
{
    public class KrakenBackgroundService : BackgroundService
    {
        private readonly IHubContext<BalanceHub> _balanceHub;
        private readonly ClientWebSocket _webSocket;

        public KrakenBackgroundService(IHubContext<BalanceHub> balanceHub)
        {
            _balanceHub = balanceHub;
            _webSocket = new ClientWebSocket();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await SubscribeToPairs(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var receivedData = await Receive(_webSocket, cancellationToken);
                    if (receivedData != null && !receivedData.Contains("heartbeat"))
                        await _balanceHub.Clients.All.SendAsync("BroadcastData", receivedData, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR - {ex.Message}");
                }
            }
        }

        private async Task SubscribeToPairs(CancellationToken cancellationToken)
        {
            try
            {
                var subscriptionModel = new SubscribeMarketPairModel
                {
                    Event = "subscribe",
                    Pair = new[]
                    {
                        "XBT/USD",
                        "XBT/EUR"
                    },
                    Subscription = new SubscriptionModel
                    {
                        Name = "ticker"
                    }
                };
                var subscribeMessage = JsonSerializer.Serialize(subscriptionModel, options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
                var messageData = Encoding.UTF8.GetBytes(subscribeMessage);

                await _webSocket.ConnectAsync(
                            new Uri(
                                "wss://ws.kraken.com"),
                            cancellationToken);

                if (!_webSocket.CloseStatus.HasValue)
                {
                    await _webSocket.SendAsync(
                        new ArraySegment<byte>(messageData, 0, messageData.Length),
                        WebSocketMessageType.Text,
                        true,
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
            }
        }

        private static async Task<string> Receive(WebSocket socket, CancellationToken cancellationToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);

            while (!cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                await using var stream = new MemoryStream();
                do
                {
                    result = await socket.ReceiveAsync(buffer, cancellationToken);
                    stream.Write(buffer.Array ?? throw new InvalidOperationException(), buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                stream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(stream, Encoding.UTF8);

                return await reader.ReadToEndAsync();
            }
            return null;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _webSocket.Dispose();
            return Task.CompletedTask;
        }
    }
}
