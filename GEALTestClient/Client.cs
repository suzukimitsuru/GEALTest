using System;

namespace GEALTest
{
    public class Client : IDisposable
    {
        /// <summary>
        /// UDP�|�[�g
        /// </summary>
        private UDPPort _port = null;

        /// <summary>
        /// �v���H��
        /// </summary>
        RequestFactory _factory;

#region �v���p�e�B
        public bool IsOpened { get { return this._port.IsOpened; } }
#endregion �v���p�e�B

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
        public void Operation(RequestBase request)
        {
            this._port.Send(request.GetBytes());
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

#endregion �v������
    }
}
