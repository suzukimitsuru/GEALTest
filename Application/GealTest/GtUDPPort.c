/*	GEAL Test Server: UDPポート
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <stdio.h>
#include <winsock2.h>
#include "GtPort.h"

static WSADATA _wsaData;
static SOCKET _sock;
static char _receive[2048];

/*	<summary>初期化</summary>
	<return>エラーコード</return>
*/
static int _Initialize(void) {
	int error = 0;

	// Windowsソケットを準備
	if (WSAStartup(MAKEWORD(2,0), &_wsaData) != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>終了処理</summary>
	<return>エラーコード</return>
*/
static int _Terminate(void) {
	int error = 0;

	// Windowsソケットを一掃
	if (WSACleanup() != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>ポートを開く</summary>
	<parameter name="parameter">引数</parameter>
	<return>エラーコード</return>
*/
static int _Open(void *parameter) {
	int error = 0;
	int port = ((int*)parameter)[0];

	// ソケットを開く
	_sock = socket(AF_INET, SOCK_DGRAM, 0);
	if (_sock == INVALID_SOCKET) {
		error = -WSAGetLastError();
	} else {

		// IPアドレスと結び付ける
		struct sockaddr_in addr;
		addr.sin_family = AF_INET;
		addr.sin_port = htons(port);
		addr.sin_addr.S_un.S_addr = INADDR_ANY;
		if (bind(_sock, (struct sockaddr *)&addr, sizeof(addr)) != 0) {
			error = -WSAGetLastError();
		} else {

			// ブロック無しモードに移行
			u_long non_blocking = 1;
			if (ioctlsocket(_sock, FIONBIO, &non_blocking) != 0) {
				error = -WSAGetLastError();
			}
		}
	}
	return error;
}

/*	<summary>ポート閉じる</summary>
	<return>エラーコード</return>
*/
static int _Close(void) {
	int error = 0;

	// ソケットを閉じる
	if (closesocket(_sock) != 0) {
		error = -WSAGetLastError();
	}
	return error;
}

/*	<summary>要求を受信</summary>
	<return>受信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
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

/*	<summary>受信データを返す</summary>
	<return>受信データ</return>
*/
char* _ReceiveData(void) {
	return &_receive[0];
}

/*	<summary>パケットを送信</summary>
	<parameter name="packet">送信パケット</parameter>
	<parameter name="bytes">送信バイト数</parameter>
	<return>送信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
*/
int _Send(char *packet, int bytes) {
	int sended_bytes = 0;
	sended_bytes = send(_sock, packet, bytes, 0);
	if (sended_bytes < 1) {
		sended_bytes = -WSAGetLastError();
	}
	return sended_bytes;
}

/*	<summary>UDPポート構造体</summary>
*/
GT_PORT GtUDPPort = { _Initialize, _Terminate, _Open, _Close, _Receive, _ReceiveData, _Send };