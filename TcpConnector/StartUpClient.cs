// See https://aka.ms/new-console-template for more information

using System.Net;

namespace TcpConnector
{
    class StartUpClient
    {
        public static void Main()
        {
            // 待ち受けるIPアドレスとポート番号を指定する
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // localhost
            int portNum = 50000;

            TcpConnector connector = new TcpConnector();
            connector.ConnectStart(ipAddress, portNum);
        }
    }
}