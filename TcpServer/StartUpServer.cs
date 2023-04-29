// See https://aka.ms/new-console-template for more information

using System.Net;

namespace TcpServer
{
    class StartUpServer
    {
        public static void Main()
        {
            // 待ち受けるIPアドレスとポート番号を指定する
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // localhost
            int portNum = 50000;

            // TCP接続開始
            TcpServer connector = new TcpServer();
            connector.ConnectStart(ipAddress, portNum);
        }
    }
}

