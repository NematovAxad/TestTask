using System.Globalization;
using System.Text.RegularExpressions;
using ConsoleProject.Extensions;

namespace ConsoleProject;

public static class ReadFromFile
{
    public static Task<List<Tuple<string, DateTime>>> ReadAllRecordsFromFile()
    {
        List<Tuple<string, DateTime>> allRecords = new List<Tuple<string, DateTime>>();
        
        if (Program.FileLog == null) return Task.FromResult(allRecords);
        
        try
        {
            using StreamReader reader = new StreamReader(Program.FileLog);
            while ( reader.ReadLine() is { } line && line.Length>0)
            {
                Regex ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                Match matchIp = ipRegex.Match(line ?? string.Empty);
                    
                Regex dateTimeRegex = new Regex(@"\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2}");
                Match matchDateTime = dateTimeRegex.Match(line ?? string.Empty);

                if(matchIp.Success && matchDateTime.Success && DateTime.TryParseExact(matchDateTime.Value, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    string ip = matchIp.Value;
                    DateTime dt = dateTime;

                    if (Program.AddressStart != null && Program.AddressMask != null)
                    {
                        if(CheckIpInRange.CheckInRange(ip))
                            allRecords.Add(new Tuple<string, DateTime>(ip, dt));
                    }
                    else
                    {
                        allRecords.Add(new Tuple<string, DateTime>(ip, dt));
                    }
                }
            }

            return Task.FromResult(allRecords);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Programm cannot open file");
            Environment.Exit(1);
        }

        return Task.FromResult(allRecords);
    }
}