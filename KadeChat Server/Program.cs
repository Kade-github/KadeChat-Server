using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KadeChat_Server
{
    class Program
    {
        // Using a TCP Server do not edit the code if you don't know what your doing.
        // Also, if you do get someones ip that is fine just don't use it in a malicious way.
        // Or you can get banned.

        static string ChatMsgs = "[KadeChat Server]";

        // Edit these
        static int MaxLength = 255;
        static string ServerName = "KadeChat Server";

        static void Main(string[] args)
        {
            // Lets assign a TCPListner to listen on port 4000. (Servers can only be on port 4000!)
            TcpListener server = new TcpListener(IPAddress.Any, 4000);
            server.Start();
            Console.Write("[Server Started]");
            while (true)
            {
                // This is how a client is gonna be handled.
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }

        static void HandleClient(object obj)
        {
            var client = (TcpClient)obj;
            try
            {
                // Lets define some bytes and decode the byte the client sent.
                ASCIIEncoding Enc = new ASCIIEncoding();
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                // dataReceived is the string to what the client sent.
                string user = dataReceived.Split(',')[0];
                string data = dataReceived.Split(',')[1];
                // Commands/Functions
                if (data.Equals("chat"))
                {
                    // This prints what ever the client said.
                    var dataReceive = dataReceived.Replace(user + "," + data + ",", string.Empty);
                    string chat = dataReceive;
                    if (chat.Length >= MaxLength)
                    {
                        // Send back the error 'bad length'
                        buffer = Enc.GetBytes("bad length");
                        nwStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        // Returns the chat message into the console.
                        ChatMsgs = ChatMsgs + "\n[" + user + "]: " + chat;
                        Console.Write("\n[" + user + "]: " + chat);
                    }
                }
                else if (data.Equals("gchat"))
                {
                    // This returns the chat to the clients server messages.
                    buffer = Enc.GetBytes(ChatMsgs);
                    nwStream.Write(buffer, 0, buffer.Length);
                }
                else if (data.Equals("ping"))
                {
                    // pings back that its online.
                    buffer = Enc.GetBytes("good");
                    nwStream.Write(buffer, 0, buffer.Length);
                }
                else if (data.Equals("gserver"))
                {
                    // Get the server name
                    buffer = Enc.GetBytes(ServerName);
                    nwStream.Write(buffer, 0, buffer.Length);
                    Console.Write("\n[" + user + "] Connected!");
                }
                client.Close();
            }
            catch (Exception exc)
            {
                // We write an exception because otherwise it will crash the server. (As try and catch abuse is amazing)
                Console.Write("\nA Client failed to connect! Maybe on the servers fault.\nException:\n" + exc + "\n");
                client.Close();
            }
        }
    }
}
