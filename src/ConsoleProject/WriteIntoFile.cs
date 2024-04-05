
using System.Runtime.InteropServices.JavaScript;

namespace ConsoleProject;

public record ReadyListToWrite
{
    public string? IpAddress { get; set; }
    public int Count { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public static class WriteIntoFile
{

    public static async Task WriteReadyListIntoFile(List<ReadyListToWrite> list)
    {
        if (Program.FileOutput != null)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Program.FileOutput))
                {
                    await writer.WriteLineAsync($"Start writing new records for {DateTime.Now}");
                    foreach (var row in list)
                    {
                        writer.WriteLine($"IP: {row.IpAddress} Request Count: {row.Count} In time from: {row.StartTime}  till: {row.EndTime}");
                    }
                    
                    Console.WriteLine("All data written into file successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Programm cannot open file");
                Environment.Exit(1);
            }
        }
        else
        {
            Console.WriteLine("Output file doesn't exist");
            Environment.Exit(1);
        }
    }

    public static async Task ArrangeList(List<Tuple<string, DateTime>> allRecord)
    {
        List<ReadyListToWrite> readyList = new List<ReadyListToWrite>();

        foreach (var data in allRecord)
        {
            if (readyList.Any(l => l.IpAddress == data.Item1))
            {
                var item = readyList.FirstOrDefault(l => l.IpAddress == data.Item1);

                if (item != null)
                {
                    item.Count++;
                    
                    if (item.StartTime > data.Item2)
                        item.StartTime = data.Item2;

                    if (item.EndTime < data.Item2)
                        item.EndTime = data.Item2;
                }
            }
            else
            {
                ReadyListToWrite item = new ReadyListToWrite()
                {
                    IpAddress = data.Item1,
                    Count = 1,
                    StartTime = data.Item2,
                    EndTime = data.Item2
                };
                readyList.Add(item);
            }
            
        }

        await WriteReadyListIntoFile(readyList);
    }
}