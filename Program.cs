using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncSocketClient
{
    static async Task Main()
    {
        Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 5000));
        Console.WriteLine("Connected to the server.");

        while (true)
        {
            Console.Write("Enter two currencies (EURO DOLLAR LEI GRIVNA POUND) or exit: ");
            string message = Console.ReadLine();
            if (message == "exit") break;

            await SendMessageAsync(clientSocket, message);
            string response = await ReceiveMessageAsync(clientSocket);
            Console.WriteLine("Server response: " + response);
            if (response == "Request limit exceeded. Try again later.") return;
        }

        clientSocket.Close();
    }

    private static async Task SendMessageAsync(Socket clientSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        try
        {
            await clientSocket.SendAsync(buffer, SocketFlags.None);
        }
        catch (Exception)
        {
            Console.WriteLine("Sending is impossible");
        }
    }

    private static async Task<string> ReceiveMessageAsync(Socket clientSocket)
    {
        byte[] buffer = new byte[1024];
        int received = 0;
        try
        {
            received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
        }
        catch (Exception)
        {
            Console.WriteLine("Receive is impossible");
        }

        return Encoding.UTF8.GetString(buffer, 0, received);
    }
}
