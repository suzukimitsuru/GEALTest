/*	GEAL Test Server: UDP�|�[�g
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <stdio.h>
#include <winsock2.h>
#include "GtPort.h"

static WSADATA _wsaData;
static SOCKET _sock;
static char _receive[2048];

/*	<summary>������</summary>
	<return>�G���[�R�[�h</return>
*/
static int _Initialize(void) {
	int error = 0;

	// Windows�\�P�b�g������
	if (WSAStartup(MAKEWORD(2,0), &_wsaData) != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>�I������</summary>
	<return>�G���[�R�[�h</return>
*/
static int _Terminate(void) {
	int error = 0;

	// Windows�\�P�b�g����|
	if (WSACleanup() != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>�|�[�g���J��</summary>
	<parameter name="parameter">����</parameter>
	<return>�G���[�R�[�h</return>
*/
static int _Open(void *parameter) {
	int error = 0;
	int port = ((int*)parameter)[0];

	// �\�P�b�g���J��
	_sock = socket(AF_INET, SOCK_DGRAM, 0);
	if (_sock == INVALID_SOCKET) {
		error = -WSAGetLastError();
	} else {

		// IP�A�h���X�ƌ��ѕt����
		struct sockaddr_in addr;
		addr.sin_family = AF_INET;
		addr.sin_port = htons(port);
		addr.sin_addr.S_un.S_addr = INADDR_ANY;
		if (bind(_sock, (struct sockaddr *)&addr, sizeof(addr)) != 0) {
			error = -WSAGetLastError();
		} else {

			// �u���b�N�������[�h�Ɉڍs
			u_long non_blocking = 1;
			if (ioctlsocket(_sock, FIONBIO, &non_blocking) != 0) {
				error = -WSAGetLastError();
			}
		}
	}
	return error;
}

/*	<summary>�|�[�g����</summary>
	<return>�G���[�R�[�h</return>
*/
static int _Close(void) {
	int error = 0;

	// �\�P�b�g�����
	if (closesocket(_sock) != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>�v������M</summary>
	<return>��M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
*/
static int _Receive(void) {
	int bytes = 0;
	memset(_receive, 0, sizeof(_receive));
	bytes = recv(_sock, _receive, sizeof(_receive), 0);
	if (bytes < 1) {
		int last_error = WSAGetLastError();
		if (last_error == WSAEWOULDBLOCK) {
			bytes = 0;
		} else {
			bytes =  -last_error;
		}
	}
	return bytes;
}

/*	<summary>��M�f�[�^��Ԃ�</summary>
	<return>��M�f�[�^</return>
*/
char* _ReceiveData(void) {
	return &_receive[0];
}

/*	<summary>�p�P�b�g�𑗐M</summary>
	<parameter name="packet">���M�p�P�b�g</parameter>
	<parameter name="bytes">���M�o�C�g��</parameter>
	<return>���M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
*/
int _Send(char *packet, int bytes) {
	int sended_bytes = 0;
	sended_bytes = send(_sock, packet, bytes, 0);
	if (sended_bytes < 1) {
		sended_bytes = -WSAGetLastError();
	}
	return sended_bytes;
}

/*	<summary>UDP�|�[�g�\����</summary>
*/
GT_PORT GtUDPPort = { _Initialize, _Terminate, _Open, _Close, _Receive, _ReceiveData, _Send };