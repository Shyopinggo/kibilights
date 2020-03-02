using System;
using System.IO;

namespace Spark
{
    class Program
    {
        static void Main(string[] args)
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
            Console.WriteLine(Config.Get().GetBeaconIP(1));
        }
    }
}
