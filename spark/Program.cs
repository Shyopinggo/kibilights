using System;
using System.Threading.Tasks;
using System.IO;

namespace Spark
{
    class Program
    {
        private static Client client;
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Spark v 1");
            if (!File.Exists(Config.Path))
            {
                try
                {
                    Config.SaveDefault();
                    Console.WriteLine("Please, edit a config file and restart the application.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save default config file.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
            }
            else
            {
                try
                {
                    Config.Load();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load config file.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
            }
            client = new Client();
            try
            {
                await client.ConnectToHub();
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
                client.Reconnect(ex);
            }
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    await client.DisconnectFromHub();
                    break;
                }
                else
                {
                    Console.WriteLine("Command not found.");
                    continue;
                }
            }
        }

        
    }
}
