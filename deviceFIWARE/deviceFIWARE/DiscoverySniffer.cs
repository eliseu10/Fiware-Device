using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace device_robot
{
    public class DiscoverySniffer
    {
        private static String BROADCAST_IP = "255.255.255.255";
        private static int PORT = 8888;

        public string discoverIP()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(BROADCAST_IP), PORT);
            String msgString = "IPREQUEST";
            byte[] msg = Encoding.ASCII.GetBytes(msgString);

            UdpClient udp = new UdpClient();
            udp.EnableBroadcast = true;
            udp.Send(msg, msg.Length, endpoint);

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, PORT);
            byte[] rcv = udp.Receive(ref ep);
            udp.Close();

            String rcvString = Encoding.ASCII.GetString(rcv);
            Console.WriteLine("Message received: " + rcvString);
            char[] c = new char[1];
            c[0] = ':';
            String[] split = rcvString.Split(c);
            if (split[0] == "IPRESPONSE")
            {
                return split[1];
            }
            else
            {
                return null;
            }
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
