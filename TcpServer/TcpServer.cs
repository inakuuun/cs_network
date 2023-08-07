using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//// TODO:ssh接続を行う
//using System.Security.Cryptography;
//using Renci.SshNet;

namespace TcpServer
{
    class TcpServer
    {
        /// <summary>
        /// TCPリスナー
        /// </summary>
        /// <remarks>クライアントからの接続要求を待機時に利用</remarks>
        private TcpListener? _listener;

        /// <summary>
        /// TCPクライアント
        /// </summary>
        /// <remarks>応答電文を送信するために利用</remarks>
        private TcpClient? _client;


        /// <summary>
        /// 接続開始
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNum"></param>
        public void ConnectStart(IPAddress ipAddress, int portNum)
        {
            // コネクションの確立に成功した場合に以降の処理を実施
            if (Connection(ipAddress, portNum))
            {
                // クライアントからの接続要求待ち
                _client = _listener?.AcceptTcpClient();
                if(_client != null)
                {
                    Console.WriteLine("A client connected.");

                    // データを読み書きするインスタンスを取得
                    NetworkStream netStream = _client.GetStream();
                    // 受信するデータのバッファサイズを指定して初期化
                    byte[] receiveBytes = new byte[_client.ReceiveBufferSize];

                    // クライアントからデータの送信があるまで処理を待機
                    int bytesRead = netStream.Read(receiveBytes, 0, _client.ReceiveBufferSize);
                    // 取得したデータを文字列に変換
                    string receivedData = Encoding.UTF8.GetString(receiveBytes, 0, bytesRead);
                    Console.WriteLine($"Received Data: {receivedData}");

                    // クライアントへ送信するデータ
                    string sendData = "Hello, Client!";
                    byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                    // このタイミングでクライアントへデータを送信
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                    Console.WriteLine($"Sent Data: {sendData}");

                    // 接続解除
                    Close();
                }
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
                // クライアントとTCP接続確立
                // 指定のポート番号で接続待機
                // -------------------------------------------------
                _listener = new TcpListener(ipAddress, portNum);
                _listener.Start();
                Console.WriteLine($"Server is listening on {ipAddress}:{portNum}");
                result = true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Server is not listening on {ipAddress}:{portNum}");
                Console.WriteLine($"{e}");
            }
            return result;
        }

        /// <summary>
        /// 接続解除
        /// </summary>
        public void Close()
        {
            _client?.Close();
            _listener?.Stop();
        }

        //// 公開鍵暗号方式を用いて共通鍵を暗号化し、暗号化した共通鍵を用いて通信を行う
        //// TODO：https://chat.openai.com/share/84345757-ad02-4685-91d5-a8249c6a02bf => chatgpt
        //// https://techlive.tokyo/archives/12206 => sshライブラリのインストール
        //// https://nugetmusthaves.com/tag/SSH => sshのライブラリとして安定
        //public void HyblidCipher()
        //{
        //    string host = "サーバーのIPアドレス";
        //    string userName = "ユーザー名";
        //    string password = "パスワード";
        //    string privateKeyFilePath = "秘密鍵ファイルのパス";
        //    string remoteDirectory = "リモートディレクトリのパス";
        //    string encryptedKeyFilePath = remoteDirectory + "/key.bin";
        //    string keyFilePath = remoteDirectory + "/key.bin";

        //    // 秘密鍵を使用してファイルをサーバーからダウンロード
        //    using (var client = new SftpClient(host, userName, password))
        //    {
        //        client.Connect();
        //        client.ChangeDirectory(remoteDirectory);
        //        //client.DownloadFile(privateKeyFilePath, privateKeyFilePath);
        //        client.Disconnect();
        //    }

        //    // 共通鍵を暗号化された形でダウンロード
        //    byte[] encryptedKey = System.IO.File.ReadAllBytes(encryptedKeyFilePath);
        //}
    }
}
