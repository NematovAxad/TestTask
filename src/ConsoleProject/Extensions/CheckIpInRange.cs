using System.Net;
using System.Net.NetworkInformation;

namespace ConsoleProject.Extensions;

public static class CheckIpInRange
{
    public static bool CheckInRange(string ip)
    {
        // check one more time IpAddresses because we use IPAddress = IPAddress? in 14/15  lines
        if (Program.StartIpAddress == null || Program.SubnetMaskIpAddress == null) return false;
        
        // convert string format ip adresses to IPAddress
        IPAddress startAddress = Program.StartIpAddress; 
        IPAddress subnetMaskAddress = Program.SubnetMaskIpAddress;

        if (!IPAddress.TryParse(ip, out var ipToCheck))
            return false;
        
        // convert ip addresses to byte
        byte[] startBytes = startAddress.GetAddressBytes();
        byte[] maskBytes = subnetMaskAddress.GetAddressBytes();
        byte[] ipToCheckBytes = ipToCheck.GetAddressBytes();
        
        byte[] networkBytes = new byte[4];
        byte[] broadcastBytes = new byte[4];

        // calculate every byte of network and broadcast and check ip ip matchs in range
        for (int i = 0; i < 4; i++)
        {
            networkBytes[i] = (byte)(startBytes[i] & maskBytes[i]);
            broadcastBytes[i] = (byte)(startBytes[i] | ~maskBytes[i]);
            if (ipToCheckBytes[i] < networkBytes[i] || ipToCheckBytes[i] > broadcastBytes[i])
                return false;
        }
        return true;
    }
}