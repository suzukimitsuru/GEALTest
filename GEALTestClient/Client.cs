using GEALTest.Request;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GEALTest
{
    /// <summary>
    /// 受信した要求
    /// </summary>
    public class ReceivedRequest
    {
        /// <summary>
        /// 受信した要求
        /// </summary>
        public readonly RequestBase Request;
        /// <summary>
        /// 経過ミリ秒
        /// </summary>
        public readonly long Milliseconds;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="request">要求</param>
        /// <param name="milliseconds">経過ミリ秒</param>
        public ReceivedRequest(RequestBase request, long milliseconds)
        {
            this.Request = request;
            this.Milliseconds = milliseconds;
        }
    }
    /// <summary>
    /// 実行結果
    /// </summary>
    public class ExecuteResult
    {
        /// <summary>
        /// 結果待ち
        /// </summary>
        public readonly List<RequestBase> Waits;
        /// <summary>
        /// 返事がない要求
        /// </summary>
        public List<RequestBase> NoReplys = new List<RequestBase>();
        /// <summary>
        /// 受信済み
        /// </summary>
        public List<ReceivedRequest> Receiveds = new List<ReceivedRequest>();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="waits">結果待ち</param>
        public ExecuteResult(List<RequestBase> waits)
        {
            // 返事がない要求の準備
            this.Waits = waits;
            foreach (var wait in waits)
                this.NoReplys.Add(wait);
        }
        /// <summary>
        /// 完了済みフラグ
        /// </summary>
        public bool IsCompleted { get { return this.NoReplys.Count <= 0; } }
        /// <summary>
        /// 受信を追加
        /// </summary>
        /// <param name="receiving">受信要求</param>
        /// <param name="milliseconds">経過ミリ秒</param>
        public void AddReceive(RequestBase receiving, long milliseconds)
        {
            this.Receiveds.Add(new ReceivedRequest(receiving, milliseconds));

            // 待っている要求なら、返事がない要求から削除
            var found = -1;
            for (var index = 0; (found < 0) && (index < this.NoReplys.Count); index++)
            {
                if (this.NoReplys[index].Equals(receiving))
                    found = index;
            }
            if (found >= 0)
                this.NoReplys.RemoveAt(found);
        }
        /// <summary>
        /// 結果待ち文字列
        /// </summary>
        public string WaitsText { get { return this.requestsToText(this.Waits); } }
        /// <summary>
        /// 返事がない要求文字列
        /// </summary>
        public string NoReplysText { get { return this.requestsToText(this.NoReplys); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        private string requestsToText(List<RequestBase> requests)
        {
            var result = "";
            foreach (var request in requests)
                result += "\n\t" + request.ToLogText();
            return result.Trim();
        }
    }
    /// <summary>
    /// GEALTest Client
    /// </summary>
    public class Client : IDisposable
    {
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
        /// UDPポート
        /// </summary>
        private UDPPort _port = null;

        /// <summary>
        /// 要求工場
        /// </summary>
        RequestFactory _factory;

        public bool IsOpened { get { return this._port.IsOpened; } }

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
        /// <returns>要求</returns>
        public RequestBase Operation(RequestBase request)
        {
            this._port.Send(request.GetBytes());
            return request;
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
            RequestBase likes = null;
            var end = DateTime.Now + TimeSpan.FromMilliseconds(milliseconds);
            while ((DateTime.Now.CompareTo(end) < 0) && this.IsOpened && (!wait.Equals(received)))
            {
                var receiving = this.Receive();
                received = (receiving != null) ? receiving : received;
                likes = ((receiving != null) && receiving.Likes(wait)) ? receiving : likes;
            }
            var result = wait.Equals(received) ? received : likes;
            return (result == null) ? new NoOperation() : result;
        }

        /// <summary>
        /// 要求を待つ
        /// </summary>
        /// <param name="waits">結果待ち</param>
        /// <param name="milliseconds">待ち時間</param>
        /// <returns>実行結果</returns>
        public ExecuteResult Wait(List<RequestBase> waits, long milliseconds)
        {
            var result = new ExecuteResult(waits);


            // 結果を待つ
            var receiveds = new List<ReceivedRequest>();
            var start = DateTime.Now;
            var end = start + TimeSpan.FromMilliseconds(milliseconds);
            while ((DateTime.Now.CompareTo(end) < 0) && this.IsOpened && (!result.IsCompleted))
            {
                // 受信したら、受信済みに追加
                var receiving = this.Receive();
                if (receiving != null)
                    result.AddReceive(receiving, (long)(DateTime.Now - start).TotalMilliseconds);
            }
            return result;
        }

        #endregion 要求操作

#region テスト操作
        /// <summary>
        /// テスト件数
        /// </summary>
        private class _testCount
        {
            public int Count { set; get; }
            public int NG { set; get; }
        }
        /// <summary>
        /// 総件数
        /// </summary>
        private _testCount _total = new _testCount();
        /// <summary>
        /// クラス件数
        /// </summary>
        private _testCount _clasz = new _testCount();
        /// <summary>
        /// クラス件数
        /// </summary>
        private _testCount _method = new _testCount();

        public void TotalStart(List<Type> classes)
        {
            this._total = new _testCount();
        }
        public void ClassStart(Type clasz)
        {
            this._clasz = new _testCount();
            Console.WriteLine("{0} Start", clasz.Name);
        }
        public void MethodStart(Type clasz, MethodInfo method)
        {
            this._method = new _testCount();
            Console.WriteLine("{0}.{1}() Start", clasz.Name, method.Name);
        }
        public void Assert(string message, RequestBase operation)
        {
            Console.WriteLine("{0,-2} {1}", "op", message);
            Console.WriteLine("\t{0}", operation.ToLogText());
        }
        public void Assert(string message, ExecuteResult result)
        {
            Console.WriteLine("{0,-2} {1}", result.IsCompleted ? "OK" : "NG", message);
            foreach (var no_reply in result.NoReplys)
                Console.WriteLine("\t{0}", no_reply.ToLogText());
            _method.NG += result.IsCompleted ? 0 : 1;
            _method.Count++;
        }
        public void MethodEnd(Type clasz, MethodInfo method)
        {
            Console.WriteLine("{0}.{1}() End {2}/{3}", clasz.Name, method.Name, _method.NG, _method.Count);
            _clasz.NG += _method.NG;
            _clasz.Count += _method.Count;
        }
        public void ClassEnd(Type clasz)
        {
            Console.WriteLine("{0} End {1}/{2}", clasz.Name, _clasz.NG, _clasz.Count);
            Console.WriteLine();
            _total.NG += _clasz.NG;
            _total.Count += _clasz.Count;
        }
        public void TotalEnd(List<Type> classes)
        {
            Console.WriteLine("Result {0}/{1}", _total.NG, _total.Count);
        }
        #endregion テスト操作
    }
}
