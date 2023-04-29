using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnector
{
    class TcpConnector
    {
        private TcpClient client;

        /// <summary>
        /// 接続開始
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNum"></param>
        public void ConnectStart(IPAddress ipAddress, int portNum)
        {
            if (Connection(ipAddress, portNum))
            {
                // データを読み書きするインスタンスを取得
                NetworkStream netStream = client.GetStream();

                // サーバーへ送信するデータ
                string sendData = "Hello, Server!";
                byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                // このタイミングでサーバへデータを送信処理
                netStream.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine($"Sent Data: {sendData}");

                // 受信するデータのバッファサイズを指定して初期化
                byte[] receiveBytes = new byte[client.ReceiveBufferSize];

                // サーバからデータの送信があるまで処理を待機
                int bytesRead = netStream.Read(receiveBytes, 0, client.ReceiveBufferSize);
                // 取得したデータを文字列に変換
                string receivedData = Encoding.UTF8.GetString(receiveBytes, 0, bytesRead);
                Console.WriteLine($"Received Data: {receivedData}");

                // 接続解除
                Close();
            }
        }

        /// <summary>
        /// コネクション確立
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNum"></param>
        public bool Connection(IPAddress ipAddress, int portNum)
        {
            bool result = false;
            try
            {
                // -------------------------------------------------
                // サーバーとTCP接続確立
                // サーバーへ接続開始
                // -------------------------------------------------
                client = new TcpClient();
                client.Connect(ipAddress, portNum);
                Console.WriteLine($"Server is listening on {ipAddress}:{portNum}");
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Server is not listening {ipAddress}:{portNum}");
                Console.WriteLine($"{e}");
            }
            return result;
        }

        /// <summary>
        /// 接続解除
        /// </summary>
        public void Close()
        {
            // TcpClient をクローズする
            client.Close();
        }
    }
}
