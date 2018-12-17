using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GEALTest
{
    public class UDPPort
    {
        /// <summary>
        /// 受信待ちポート番号
        /// </summary>
        private int _waitPort = 0x5447; // 21575:"GT";

        /// <summary>
        /// 送信先ホスト名
        /// </summary>
        private string _toHost = "";

        /// <summary>
        /// 送信先ポート番号
        /// </summary>
        private int _toPort = 0x7447; // 29767:"Gt"

        /// <summary>
        /// UDPポート
        /// </summary>
        private UdpClient _client = null;

        /// <summary>
        /// 受信リスト
        /// </summary>
        private List<byte[]> _receiveList = new List<byte[]>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="waitPort">受信待ちポート番号</param>
        /// <param name="toHost">送信先ホスト名</param>
        /// <param name="toPort">送信先ポート番号</param>
        public UDPPort(int waitPort, string toHost, int toPort)
        {
            // 引数を保存
            this._waitPort = waitPort;
            this._toHost = toHost;
            this._toPort = toPort;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~UDPPort()
        {
            // ポートを閉じる
            this.Close();
        }

        /// <summary>
        /// ポートを開く
        /// </summary>
        public void Open()
        {
            // 受信を待機
            this._client = new UdpClient(new IPEndPoint(IPAddress.Any, this._waitPort));
            this._client.BeginReceive(this._receiveCallback, this._client);
        }

        /// <summary>
        /// ポートを閉じる
        /// </summary>
        public void Close()
        {
            // UDPポートを閉じる
            if (this._client != null)
            {
                this._client.Close();
                this._client = null;
            }
        }

        /// <summary>
        /// 開き済み
        /// </summary>
        public bool IsOpened { get { return this._client != null; } }

        /// <summary>
        /// データを送信
        /// </summary>
        /// <param name="sendData">送信データ</param>
        public void Send(byte[] sendData)
        {
            // ポートが無かったら、ポートを作る
            this._client = (this._client == null) ? new UdpClient() : this._client;

            // 非同期にデータを送信する
            this._client.BeginSend(sendData, sendData.Length, this._toHost, this._toPort, (IAsyncResult ar) =>
            {
                // 非同期送信を終了する
                var udp = (UdpClient)ar.AsyncState;
                try
                {
                    udp.EndSend(ar);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Send(): 送信エラー({0}/{1})", ex.Message, ex.ErrorCode);
                }
                catch (ObjectDisposedException ex)
                {
                    // すでに閉じている時は終了
                    Console.WriteLine("Send(): {0}は閉じられています。", ex.ObjectName);
                    this._client = null;
                }
            }, this._client);
        }

        /// <summary>
        /// データ受信処理
        /// </summary>
        /// <param name="ar">非同期対象</param>
        private void _receiveCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;

            // 非同期受信を終了する
            IPEndPoint from = null;
            byte[] receive_data = { };
            try
            {
                receive_data = udp.EndReceive(ar, ref from);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("_receiveCallback(): 受信エラー({0}/{1})", ex.Message, ex.ErrorCode);
            }
            catch (ObjectDisposedException ex)
            {
                // すでに閉じている時は終了
                Console.WriteLine("_receiveCallback(): {0}は閉じられています。", ex.ObjectName);
                this._client = null;
            }
            if (receive_data.Length > 0)
            {
                // 受信データを蓄積
                this._receiveList.Add(receive_data);
            }

            // 再びデータ受信を開始する
            if (this._client != null)
            {
                udp.BeginReceive(this._receiveCallback, udp);
            }
        }

        /// <summary>
        /// データを受信
        /// </summary>
        /// <returns>受信データ</returns>
        public byte[] Receive()
        {
            byte[] result = { };
            if (this._receiveList.Count > 0)
            {
                result = this._receiveList[0];
                this._receiveList.RemoveAt(0);
                result = (result is null) ? new byte[] { } : result;
            }
            return result;
        }
    }
}
