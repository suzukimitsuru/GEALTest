/*	GEAL Test Server: UDPポート
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
//警告	C4996	'inet_addr': Use inet_pton() or InetPton() instead or define _WINSOCK_DEPRECATED_NO_WARNINGS to disable deprecated API warnings	SampleDev	z : \ドキュメント\github\gealtest\application\gealtest\gtudpport.c	56
#define _WINSOCK_DEPRECATED_NO_WARNINGS 1

#include <WinSock2.h>
#include "GtPort.h"
#include "GtUDPPort.h"

static WSADATA _wsa;	// Windows Socket
static SOCKET _sock;	// ソケット
static struct sockaddr_in _wait;	// 受信待ちアドレス
static struct sockaddr_in _to;		// 送信先アドレス

/*	<summary>初期化</summary>
	<parameter name="parameter">引数</parameter>
	<return>エラーコード</return>
*/
static int _Initialize(void *parameter) {
	int error = 0;

	// Windowsソケットを準備
	if (WSAStartup(MAKEWORD(2,0), &_wsa) != 0) {
		error = -WSAGetLastError();
	} else {
		// アドレスの設定
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
	<return>エラーコード</return>
*/
static int _Open(void) {
	int error = 0;

	// ソケットを開く
	_sock = socket(AF_INET, SOCK_DGRAM, 0);
	if (_sock == INVALID_SOCKET) {
		error = -WSAGetLastError();
	} else {
		
		// IPアドレスと結び付ける
		if (bind(_sock, (struct sockaddr *)&_wait, sizeof(_wait)) != 0) {
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
	<parameter name="buffer">受信バッファ</parameter>
	<parameter name="maxBytes">最大受信バイト数</parameter>
	<return>受信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
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

/*	<summary>パケットを送信</summary>
	<parameter name="packet">送信パケット</parameter>
	<parameter name="bytes">送信バイト数</parameter>
	<return>送信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
*/
int _Send(unsigned char* packet, int bytes) {
	int sended_bytes = 0;
	sended_bytes = sendto(_sock, packet, bytes, 0, (struct sockaddr *)&_to, sizeof(_to));
	if (sended_bytes < 1) {
		sended_bytes = -WSAGetLastError();
	}
	return sended_bytes;
}

/*	<summary>UDPポート構造体</summary>
*/
GT_PORT GtUDPPort = { _Initialize, _Terminate, _Open, _Close, _Receive, _Send };
