using System.Net;
using ConsoleProject.Extensions;

namespace ConsoleProject
{
    class Program
    {
        #region Static properties

        public static string? FileLog { get; set; }
        public static string? FileOutput { get; set; }
        public static string? AddressStart { get; set; }
        public static string? AddressMask { get; set; }

        public static IPAddress? StartIpAddress { get; set; }
        
        public static IPAddress? SubnetMaskIpAddress { get; set; }

        #endregion


        #region Programm Entry

        private static async Task Main(string[] args)
        { 
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            
            
            Console.WriteLine("E - get parameters from environment file");
            Console.WriteLine("C - get parameters from configuration file");
            Console.WriteLine("Your choice: ");
            string? userChoice = Console.ReadLine();

            if (userChoice != null)
            {
                if (userChoice == "E")
                {
                    SetParametersFromEnvironment();
                }
                else if (userChoice == "C")
                {
                    SetParametersFromConfiguration(config);
                }
                else
                {
                    Console.WriteLine("you choose not existing option");
                }

                var allRecords = await ReadFromFile.ReadAllRecordsFromFile();

                await WriteIntoFile.ArrangeList(allRecords);

            }
            else
            {
                Console.WriteLine("you didn't choose any option");
            }
        }

        #endregion

        #region Set Parameters

        private static void SetParametersFromConfiguration(IConfiguration configuration)
        {
            FileLog = configuration["Configs:FileLog"];
            FileOutput = configuration["Configs:FileOutput"];
            AddressStart = configuration["Configs:AddressStart"];
            AddressMask = configuration["Configs:AddressMask"];

            IPAddress? start;
            IPAddress? mask;
            if (IPAddress.TryParse(AddressStart, out start) && IPAddress.TryParse(AddressMask, out mask))
            {
                StartIpAddress = start;
                SubnetMaskIpAddress = mask;
            }
        }

        private static void SetParametersFromEnvironment()
        {
            FileLog = "fileLog".Env();
            FileOutput = "fileOutput".Env();
            AddressStart = "addressStart".Env();
            AddressMask = "addressMask".Env();
            
            IPAddress? start;
            IPAddress? mask;
            if (IPAddress.TryParse(AddressStart, out start) && IPAddress.TryParse(AddressMask, out mask))
            {
                StartIpAddress = start;
                SubnetMaskIpAddress = mask;
            }
        }

        #endregion
    }
}
