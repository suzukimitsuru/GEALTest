using GEALTest.Request;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GEALTest
{
    /// <summary>
    /// ��M�����v��
    /// </summary>
    public class ReceivedRequest
    {
        /// <summary>
        /// ��M�����v��
        /// </summary>
        public readonly RequestBase Request;
        /// <summary>
        /// �o�߃~���b
        /// </summary>
        public readonly long Milliseconds;
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="request">�v��</param>
        /// <param name="milliseconds">�o�߃~���b</param>
        public ReceivedRequest(RequestBase request, long milliseconds)
        {
            this.Request = request;
            this.Milliseconds = milliseconds;
        }
    }
    /// <summary>
    /// ���s����
    /// </summary>
    public class ExecuteResult
    {
        /// <summary>
        /// ���ʑ҂�
        /// </summary>
        public readonly List<RequestBase> Waits;
        /// <summary>
        /// �Ԏ����Ȃ��v��
        /// </summary>
        public List<RequestBase> NoReplys = new List<RequestBase>();
        /// <summary>
        /// ��M�ς�
        /// </summary>
        public List<ReceivedRequest> Receiveds = new List<ReceivedRequest>();
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="waits">���ʑ҂�</param>
        public ExecuteResult(List<RequestBase> waits)
        {
            // �Ԏ����Ȃ��v���̏���
            this.Waits = waits;
            foreach (var wait in waits)
                this.NoReplys.Add(wait);
        }
        /// <summary>
        /// �����ς݃t���O
        /// </summary>
        public bool IsCompleted { get { return this.NoReplys.Count <= 0; } }
        /// <summary>
        /// ��M��ǉ�
        /// </summary>
        /// <param name="receiving">��M�v��</param>
        /// <param name="milliseconds">�o�߃~���b</param>
        public void AddReceive(RequestBase receiving, long milliseconds)
        {
            this.Receiveds.Add(new ReceivedRequest(receiving, milliseconds));

            // �҂��Ă���v���Ȃ�A�Ԏ����Ȃ��v������폜
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
        /// ���ʑ҂�������
        /// </summary>
        public string WaitsText { get { return this.requestsToText(this.Waits); } }
        /// <summary>
        /// �Ԏ����Ȃ��v��������
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
#region �N���X����

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="port">�ʐM�|�[�g</param>
        /// <param name="factory">�v���H��</param>
        public Client(UDPPort port, RequestFactory factory)
        {
            this._factory = factory;
            this._port = port;
            this._port.Open();
        }

        #region IDisposable Support
        private bool disposedValue = false; // �d������Ăяo�������o����ɂ�

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // �}�l�[�W�h��Ԃ�j�����܂� (�}�l�[�W�h �I�u�W�F�N�g)�B
                    this._port = null;
                    this._factory = null;
                }

                // �A���}�l�[�W�h ���\�[�X (�A���}�l�[�W�h �I�u�W�F�N�g) ��������A���̃t�@�C�i���C�U�[���I�[�o�[���C�h���܂��B
                // �傫�ȃt�B�[���h�� null �ɐݒ肵�܂��B

                disposedValue = true;
            }
        }

        // ��� Dispose(bool disposing) �ɃA���}�l�[�W�h ���\�[�X���������R�[�h���܂܂��ꍇ�ɂ̂݁A�t�@�C�i���C�U�[���I�[�o�[���C�h���܂��B
        // ~Client() {
        //   // ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(bool disposing) �ɋL�q���܂��B
        //   Dispose(false);
        // }

        // ���̃R�[�h�́A�j���\�ȃp�^�[���𐳂��������ł���悤�ɒǉ�����܂����B
        void IDisposable.Dispose()
        {
            // ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(bool disposing) �ɋL�q���܂��B
            Dispose(true);
            // TODO: ��̃t�@�C�i���C�U�[���I�[�o�[���C�h�����ꍇ�́A���̍s�̃R�����g���������Ă��������B
            // GC.SuppressFinalize(this);
        }
        #endregion IDisposable Support

        #endregion �N���X����

#region �v������

        /// <summary>
        /// UDP�|�[�g
        /// </summary>
        private UDPPort _port = null;

        /// <summary>
        /// �v���H��
        /// </summary>
        RequestFactory _factory;

        public bool IsOpened { get { return this._port.IsOpened; } }

        /// <summary>
        /// �v������M����
        /// </summary>
        /// <returns></returns>
        public RequestBase Receive()
        {
            var received = this._port.Receive();
            return this._factory.GetRequest(received);
        }

        /// <summary>
        /// �v�����s��
        /// </summary>
        /// <param name="request">�v��</param>
        /// <returns>�v��</returns>
        public RequestBase Operation(RequestBase request)
        {
            this._port.Send(request.GetBytes());
            return request;
        }

        /// <summary>
        /// �v����҂�
        /// </summary>
        /// <param name="wait">�҂v��</param>
        /// <param name="milliseconds">�҂��~���b</param>
        /// /// <returns>��M�����v��</returns>
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
        /// �v����҂�
        /// </summary>
        /// <param name="waits">���ʑ҂�</param>
        /// <param name="milliseconds">�҂�����</param>
        /// <returns>���s����</returns>
        public ExecuteResult Wait(List<RequestBase> waits, long milliseconds)
        {
            var result = new ExecuteResult(waits);


            // ���ʂ�҂�
            var receiveds = new List<ReceivedRequest>();
            var start = DateTime.Now;
            var end = start + TimeSpan.FromMilliseconds(milliseconds);
            while ((DateTime.Now.CompareTo(end) < 0) && this.IsOpened && (!result.IsCompleted))
            {
                // ��M������A��M�ς݂ɒǉ�
                var receiving = this.Receive();
                if (receiving != null)
                    result.AddReceive(receiving, (long)(DateTime.Now - start).TotalMilliseconds);
            }
            return result;
        }

        #endregion �v������

#region �e�X�g����
        /// <summary>
        /// �e�X�g����
        /// </summary>
        private class _testCount
        {
            public int Count { set; get; }
            public int NG { set; get; }
        }
        /// <summary>
        /// ������
        /// </summary>
        private _testCount _total = new _testCount();
        /// <summary>
        /// �N���X����
        /// </summary>
        private _testCount _clasz = new _testCount();
        /// <summary>
        /// �N���X����
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
        #endregion �e�X�g����
    }
}
