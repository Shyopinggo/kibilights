using System;
using System.Threading.Tasks;
using System.IO;

namespace Spark
{
    class Program
    {
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
                    Console.WriteLine(ex.ToString());
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
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }
            var client = new Client();
            await client.ConnectToHub();
            await client.Test(2);
            await client.DisconnectFromHub();
            Console.ReadLine();
        }
    }
}
