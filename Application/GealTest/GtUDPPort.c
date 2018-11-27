/*	GEAL Test Server: UDP�|�[�g
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
//�x��	C4996	'inet_addr': Use inet_pton() or InetPton() instead or define _WINSOCK_DEPRECATED_NO_WARNINGS to disable deprecated API warnings	SampleDev	z : \�h�L�������g\github\gealtest\application\gealtest\gtudpport.c	56
#define _WINSOCK_DEPRECATED_NO_WARNINGS 1

#include <WinSock2.h>
#include "GtPort.h"
#include "GtUDPPort.h"

static WSADATA _wsa;	// Windows Socket
static SOCKET _sock;	// �\�P�b�g
static struct sockaddr_in _wait;	// ��M�҂��A�h���X
static struct sockaddr_in _to;		// ���M��A�h���X

/*	<summary>������</summary>
	<parameter name="parameter">����</parameter>
	<return>�G���[�R�[�h</return>
*/
static int _Initialize(void *parameter) {
	int error = 0;

	// Windows�\�P�b�g������
	if (WSAStartup(MAKEWORD(2,0), &_wsa) != 0) {
		error = -WSAGetLastError();
	} else {
		// �A�h���X�̐ݒ�
		GT_UDP_PARAMETER* param = (GT_UDP_PARAMETER*)parameter;
		_wait.sin_family = AF_INET;
		_wait.sin_port = htons(param->WaitPort);
		_wait.sin_addr.S_un.S_addr = INADDR_ANY;
		_to.sin_family = AF_INET;
		_to.sin_port = htons(param->ToPort);
		_to.sin_addr.S_un.S_addr = inet_addr(param->ToHost);
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
	<return>�G���[�R�[�h</return>
*/
static int _Open(void) {
	int error = 0;

	// �\�P�b�g���J��
	_sock = socket(AF_INET, SOCK_DGRAM, 0);
	if (_sock == INVALID_SOCKET) {
		error = -WSAGetLastError();
	} else {
		
		// IP�A�h���X�ƌ��ѕt����
		if (bind(_sock, (struct sockaddr *)&_wait, sizeof(_wait)) != 0) {
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
	<parameter name="buffer">��M�o�b�t�@</parameter>
	<parameter name="maxBytes">�ő��M�o�C�g��</parameter>
	<return>��M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
*/
static int _Receive(unsigned char* buffer, int maxBytes) {
	int bytes = recv(_sock, buffer, maxBytes, 0);
	if (bytes < 1) {
		int last_error = WSAGetLastError();
		if (last_error == WSAEWOULDBLOCK) {
			bytes = 0;
		} else {
			bytes =  -last_error;
		}
	} else {
		bytes = bytes;
	}
	return bytes;
}

/*	<summary>�p�P�b�g�𑗐M</summary>
	<parameter name="packet">���M�p�P�b�g</parameter>
	<parameter name="bytes">���M�o�C�g��</parameter>
	<return>���M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
*/
int _Send(unsigned char* packet, int bytes) {
	int sended_bytes = 0;
	sended_bytes = sendto(_sock, packet, bytes, 0, (struct sockaddr *)&_to, sizeof(_to));
	if (sended_bytes < 1) {
		sended_bytes = -WSAGetLastError();
	}
	return sended_bytes;
}

/*	<summary>UDP�|�[�g�\����</summary>
*/
GT_PORT GtUDPPort = { _Initialize, _Terminate, _Open, _Close, _Receive, _Send };
