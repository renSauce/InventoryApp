using System.Net.Sockets;
using System.Text;

namespace InventoryApp.Robotics;

public class Robot
{
    public const int urscriptPort = 30002, dashboardPort = 29999;

    // GUI IP addresses (Skal teste om 2 er nødvendige eller 1 er nok)
    public string RobotIpAddress { get; set; } = "localhost";
    public string ControlBoxIpAddress { get; set; } = "localhost";

    private void SendString(int port, string message)
    {
        using var client = new TcpClient(RobotIpAddress, port);
        using var stream = client.GetStream();
        stream.Write(Encoding.ASCII.GetBytes(message));
    }

    public void SendUrscript(string urscript)
    {
        SendString(dashboardPort, "brake release\n");
        SendString(urscriptPort, urscript);
    }
}
