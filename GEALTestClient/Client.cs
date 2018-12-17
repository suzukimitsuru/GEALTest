using System;

namespace GEALTest
{
    public class Client : IDisposable
    {
        /// <summary>
        /// UDPポート
        /// </summary>
        private UDPPort _port = null;

#region クラス操作

        public UDPPort port { get { return this._port; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="port">通信ポート</param>
        public Client(UDPPort port)
        {
            // 通信開始
            this._port = port;
            this._port.Open();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    this._port = null;
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~Client() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        void IDisposable.Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion IDisposable Support

        #endregion クラス操作

#region 要求操作

        /// <summary>
        /// ボタンを押す
        /// </summary>
        /// <param name="button">ボタン</param>
        public void ButtonPush(uint button)
        {
            this._port.Send(BitConverter.GetBytes((Int16)button));
        }

        /// <summary>
        /// ステージ開始を待つ
        /// </summary>
        /// <param name="stage">開始待ちステージ</param>
        /// <param name="milliseconds">待ちミリ秒</param>
        /// /// <returns>開始したステージ</returns>
        public uint StageWait(uint stage, long milliseconds)
        {
            uint started = 0;
            long wait_ticks = milliseconds * TimeSpan.TicksPerMillisecond;
            for (var begin = DateTime.Now; (DateTime.Now - begin).Ticks < wait_ticks;)
            {
                var received = this._port.Receive();
                if (received.Length > 0)
                {
                    started = BitConverter.ToUInt16(received, 0);
                    begin = DateTime.Now - new TimeSpan(wait_ticks);
                }
            }
            return started;
        }

        #endregion 要求操作
    }
}
