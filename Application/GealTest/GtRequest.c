/*	GEAL Test Server: �v������
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <memory.h>
#include "GtRequest.h"

static unsigned char _ringBuffer[256];
static int _get = 0;
static int _put = 0;
static int _count = 0;

/*	<summary>�v�� �̏�����</summary>
*/
void GtRequetInitialize(void) {
	memset(_ringBuffer, 0, sizeof(_ringBuffer));
	_get = 0;
	_put = 0;
	_count = 0;
}

/*	<summary>�v���̒~��</summary>
	<parameter name="receiveData">��M�f�[�^</parameter>
	<parameter name="receiveBytes">��M�o�C�g��</parameter>
	<return>�I�[�o�[�t���o�C�g��(0:�Ȃ�)</return>
*/
int GtRequetPut(unsigned char* receiveData, int receiveBytes) {
	int overflow = 0;
	if ((receiveData != NULL) && (receiveBytes > 0)) {

		// ��M�f�[�^��S��
		for (int index = 0; index < receiveBytes; index++) {

			// �ő�p�P�b�g���ȉ��Ȃ�
			if (_count < sizeof(_ringBuffer)) {

				// ��M�f�[�^��~�ς���
				_ringBuffer[_put] = receiveData[index];
				_put++; _put = (_put < sizeof(_ringBuffer)) ? _put : 0;
				_count++;
			} else {
				overflow++;
			}
		}
	}
	return overflow;
}

/*	<summary>�v���̎��o��</summary>
	<parameter name="receiveData">�v���f�[�^</parameter>
	<parameter name="receiveBytes">�ő�v���o�C�g��</parameter>
	<return>�v���o�C�g��(0:�Ȃ�)</return>
*/
extern int GtRequetGet(unsigned char* request, int maxBytes) {
	int bytes = 0;
	if (_count >= 2) {
		for (int index = 0; (index < 2) && (index < maxBytes); index++) {
			request[index] = _ringBuffer[_get];
			_get++; _get = (_get < sizeof(_ringBuffer)) ? _get : 0;
			_count--;
			bytes++;
		}
	} else {
		bytes = 0;
	}
	return bytes;
}
