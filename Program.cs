using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpClientProject
{
    internal class Program
    {
        static int portRemote = 11111;
        static int portLocal = 11111;
        static IPAddress addressRemote = IPAddress.Parse("239.255.255.255");
        static string? nameLocal;

        static void SendUdpMessage()
        {
            UdpClient clientSend = new UdpClient();
            IPEndPoint pointRemote = new(addressRemote, portRemote);
            try
            {
                while (true)
                {
                    string? message = nameLocal + " >>> ";
                    Console.Write($"\nInput message: ");
                    message += Console.ReadLine();

                    byte[] buffer = Encoding.Default.GetBytes(message!);
                    clientSend.Send(buffer, buffer.Length, pointRemote);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                clientSend.Close();
            }
        }

        static void ReceiveUdpMessage()
        {
            UdpClient clientReceive = new UdpClient(portLocal);
            clientReceive.JoinMulticastGroup(addressRemote);

            IPEndPoint? ipRemote = null;
            try
            {
                while (true)
                {
                    byte[] buffer = clientReceive.Receive(ref ipRemote);
                    string message = Encoding.Default.GetString(buffer);
                    Console.WriteLine($"\n>>> {ipRemote.Address} {message}");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                clientReceive.Close();
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Input Name: ");
            nameLocal = Console.ReadLine();

            //Console.Write("Input Local Port: ");
            //portLocal = Int32.Parse(Console.ReadLine());

            //Console.Write("Input Remote Port: ");
            //portRemote = Int32.Parse(Console.ReadLine());

            Thread treadReceive = new(ReceiveUdpMessage);
            treadReceive.Start();
            SendUdpMessage();
        }
    }
}