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
        private class GetTokenResponse
        {
            public string access_token { get; set; }
            public string username { get; set; }
        }

        public Client()
        {
            hubConnection = new HubConnectionBuilder().WithUrl(Config.Get().LighthouseHost + "/api/lighthouse/hub",
                (options) => options.AccessTokenProvider = GetAccessToken)
                .WithAutomaticReconnect()
                .Build();
            hubConnection.On<int>("Call", Call);
        }

        public async Task ConnectToHub()
        {
            await hubConnection.StartAsync();
            Console.WriteLine("Connected to hub.");
        }

        public async Task DisconnectFromHub()
        {
            await hubConnection.StopAsync();
            Console.WriteLine("Disconnected");
        }

        public async Task Test(int id)
        {
            await hubConnection.InvokeAsync("Test", id);
        }

        public void Call(int beaconId)
        {
            Console.WriteLine(Config.Get().GetBeaconIP(beaconId));
            Console.Beep(beaconId * 1000, 1000);
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
    }
}
