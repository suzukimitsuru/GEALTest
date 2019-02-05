using System;

namespace GEALTest
{
    public class Client : IDisposable
    {
        /// <summary>
        /// UDPポート
        /// </summary>
        private UDPPort _port = null;

        /// <summary>
        /// 要求工場
        /// </summary>
        RequestFactory _factory;

#region プロパティ
        public bool IsOpened { get { return this._port.IsOpened; } }
#endregion プロパティ

        #region クラス操作

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="port">通信ポート</param>
        /// <param name="factory">要求工場</param>
        public Client(UDPPort port, RequestFactory factory)
        {
            this._factory = factory;
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
                    // マネージド状態を破棄します (マネージド オブジェクト)。
                    this._port = null;
                    this._factory = null;
                }

                // アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
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
        /// 要求を受信する
        /// </summary>
        /// <returns></returns>
        public RequestBase Receive()
        {
            var received = this._port.Receive();
            return this._factory.GetRequest(received);
        }

        /// <summary>
        /// 要求を行う
        /// </summary>
        /// <param name="request">要求</param>
        public void Operation(RequestBase request)
        {
            this._port.Send(request.GetBytes());
        }

        /// <summary>
        /// 要求を待つ
        /// </summary>
        /// <param name="wait">待つ要求</param>
        /// <param name="milliseconds">待ちミリ秒</param>
        /// /// <returns>受信した要求</returns>
        public RequestBase Wait(RequestBase wait, long milliseconds)
        {
            RequestBase received = null;
            RequestBase waited = null;
            long wait_ticks = milliseconds * TimeSpan.TicksPerMillisecond;
            var begin = DateTime.Now;
            while (((DateTime.Now - begin).Ticks < wait_ticks) && this.IsOpened && (!wait.Equals(received)))
            {
                var receiving = this.Receive();
                received = (receiving != null) ? receiving : received;
                waited = ((receiving != null) && (receiving.Operation == wait.Operation)) ? receiving : waited;
/*                if (received != null)
                    Console.WriteLine("received {0}", received.ToLogText());
                if (waited != null)
                    Console.WriteLine("waited {0}", waited.ToLogText());*/
            }
            var result = (waited != null) ? waited : received;
            return (result == null) ? new NoOperation() : result;
        }

#endregion 要求操作
    }
}
