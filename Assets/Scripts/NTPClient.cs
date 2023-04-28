using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public static class NTPClient
{
    private static DateTime syncedTime;
    private static TimeSpan timeSinceSync;
    private static DateTime networkDateTime;
    private static bool isSynced = false;

    public static void SyncTime()
    {
        try
        {
            IPAddress[] addresses = Dns.GetHostEntry("pool.ntp.org").AddressList;
            // Pick the first one.
            IPEndPoint endPoint = new IPEndPoint(addresses[0], 123);

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.ReceiveTimeout = 3000;
                socket.SendTimeout = 3000;
                socket.Connect(endPoint);

                byte[] ntpData = new byte[48];
                ntpData[0] = 0x1B; // Set the Leap Indicator, Version Number, and Mode

                socket.Send(ntpData);
                socket.Receive(ntpData);

                ulong intPart = BitConverter.ToUInt32(ntpData, 40);
                ulong fractPart = BitConverter.ToUInt32(ntpData, 44);

                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
                DateTime networkDateTime = new DateTime(1900, 1, 1) + timeSpan; // time since 1900 on server



                double networkDateTimeEpoch = (networkDateTime - new DateTime(1970, 1, 1)).TotalSeconds;

                syncedTime = networkDateTime.ToUniversalTime();
                double syncedTimeEpoch = (syncedTime - new DateTime(1970, 1, 1)).TotalSeconds;
                timeSinceSync = DateTime.UtcNow - syncedTime;
            }
        }
        catch (Exception e)
        {
            Logger.Log("Error: " + e.Message);
        }
    }

    public static double GetCurrentEpoch()
    {
        if (!isSynced)
        {
            throw new InvalidOperationException("Time has not been synced yet. Call SyncTime() first.");
        }

        DateTime currentTime = DateTime.UtcNow - syncedTime;
        return currentTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                      ((x & 0x0000ff00) << 8) +
                      ((x & 0x00ff0000) >> 8) +
                      ((x & 0xff000000) >> 24));
    }
}


//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading.Tasks;

//public static class NTPClient
//{
//    private const string DefaultNtpServer = "pool.ntp.org";
//    private static DateTime syncedTime;
//    private static bool isSynced = false;

//    public static async Task SyncTimeAsync(string ntpServer = DefaultNtpServer)
//    {
//        IPAddress[] addresses = await Dns.GetHostEntryAsync(ntpServer);
//        IPEndPoint endPoint = new IPEndPoint(addresses[0], 123);

//        using (UdpClient client = new UdpClient())
//        {
//            client.Connect(endPoint);
//            byte[] ntpData = new byte[48];
//            ntpData[0] = 0x1B;
//            await client.SendAsync(ntpData, ntpData.Length);
//            ntpData = (await client.ReceiveAsync()).Buffer;

//            ulong intPart = BitConverter.ToUInt32(ntpData, 40);
//            ulong fractPart = BitConverter.ToUInt32(ntpData, 44);
//            intPart = SwapEndianness(intPart);
//            fractPart = SwapEndianness(fractPart);

//            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
//            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
//            DateTime networkDateTime = new DateTime(1900, 1, 1) + timeSpan;
//            syncedTime = networkDateTime.ToLocalTime();
//            isSynced = true;
//        }
//    }

//    private static uint SwapEndianness(ulong x)
//    {
//        return (uint)(((x & 0x000000ff) << 24) +
//                      ((x & 0x0000ff00) << 8) +
//                      ((x & 0x00ff0000) >> 8) +
//                      ((x & 0xff000000) >> 24));
//    }

//    public static double GetCurrentEpoch()
//    {
//        if (!isSynced)
//        {
//            throw new InvalidOperationException("Time has not been synced yet. Call SyncTimeAsync() first.");
//        }

//        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//        TimeSpan timeSinceSync = DateTime.UtcNow - syncedTime.ToUniversalTime();
//        DateTime currentTime = syncedTime.ToUniversalTime().Add(timeSinceSync);
//        TimeSpan timeSinceUnixEpoch = currentTime - unixEpoch;
//        return timeSinceUnixEpoch.TotalSeconds;
//    }
//}
