using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncTCPClient
{
    static async Task Main()
    {
        TcpClient client = new();
        await client.ConnectAsync("127.0.0.1", 5000);
        
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Connected to the server.");

        while (true)
        {
            Console.Write("Enter two currencies (EURO DOLLAR LEI GRIVNA POUND) or exit: ");
            string message = Console.ReadLine();
            if (message == "exit") break;

            await SendMessageAsync(stream, message);
            string response = await ReceiveMessageAsync(stream);
            Console.WriteLine("Server response: " + response);
            if (response == "Request limit exceeded. Try again later.") return;
        }
    }

    private static async Task SendMessageAsync(NetworkStream stream, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        try
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine("Sending is imposible");
        }
    }

    private static async Task<string> ReceiveMessageAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        int received = 0;
        try
        {
            received = await stream.ReadAsync(buffer, 0, buffer.Length);
        }catch(Exception e)
        {
            Console.WriteLine("Recieve is imposible");
        }

        return Encoding.UTF8.GetString(buffer, 0, received);
    }
}
