using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Spark
{
    public class Client
    {
        private HubConnection hubConnection;
        private bool isConnected;
        private int reconnectTime;
        private int attemptTime;
        private class GetTokenResponse
        {
            public string access_token { get; set; }
            public string username { get; set; }
        }

        public Client()
        {
            isConnected = false;
            reconnectTime = 5000;
            attemptTime = 10000;
            hubConnection = new HubConnectionBuilder().WithUrl(Config.Get().LighthouseHost + "/api/lighthouse/hub",
                (options) => options.AccessTokenProvider = GetAccessToken)
                .Build();
            hubConnection.Closed += Reconnect;
            hubConnection.On<int>("Call", Call);
        }

        public async Task ConnectToHub()
        {
            await hubConnection.StartAsync();
            Console.WriteLine("Connected to hub.");
            isConnected = true;
        }

        public async Task DisconnectFromHub()
        {
            if (!isConnected) return;
            isConnected = false;
            await hubConnection.StopAsync();
            Console.WriteLine("Disconnected");
        }

        public async Task Test(int id)
        {
            await hubConnection.InvokeAsync("Test", id);
        }

        public void Call(int beaconId)
        {
            string ip;
            try
            {
                ip = Config.Get().GetBeaconIP(beaconId);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Unknown beacon id {beaconId}");
                return;
            }
            Console.WriteLine($"Calling {ip}");
            try
            {
                string url = $"http://{ip}/call";
                Console.WriteLine(HttpRequest(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to {ip}");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string> GetAccessToken()
        {
            var conf = Config.Get();
            string url = $"{conf.LighthouseHost}/account/gettoken?name={conf.UserName}&password={conf.Password}";
            string json = await Task.Run(() => HttpRequest(url));
            var response = JsonSerializer.Deserialize<GetTokenResponse>(json);
            return response.access_token;
        }

        public string HttpRequest(string url)
        {
            var request = HttpWebRequest.Create(url);
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
            response.Close();
        }

        public async Task Reconnect(Exception ex)
        {
            if (ex == null) return;
            Console.WriteLine("Failed to connect.");
            Console.WriteLine(ex.Message);
            isConnected = false;
            int timeOut = reconnectTime;
            while (true)
            {
                try
                {
                    Console.WriteLine($"Reconnect in {timeOut / 1000} seconds.");
                    await Task.Delay(timeOut);
                    await ConnectToHub();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to reconnect.");
                    Console.WriteLine(e.Message);
                    timeOut += attemptTime;
                }
            }
        }
    }
}
