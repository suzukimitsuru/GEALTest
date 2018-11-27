/*	GEAL Test Server: 要求操作
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <memory.h>
#include "GtRequest.h"

static unsigned char _ringBuffer[256];
static int _get = 0;
static int _put = 0;
static int _count = 0;

/*	<summary>要求 の初期化</summary>
*/
void GtRequetInitialize(void) {
	memset(_ringBuffer, 0, sizeof(_ringBuffer));
	_get = 0;
	_put = 0;
	_count = 0;
}

/*	<summary>要求の蓄積</summary>
	<parameter name="receiveData">受信データ</parameter>
	<parameter name="receiveBytes">受信バイト数</parameter>
	<return>オーバーフロバイト数(0:なし)</return>
*/
int GtRequetPut(unsigned char* receiveData, int receiveBytes) {
	int overflow = 0;
	if ((receiveData != NULL) && (receiveBytes > 0)) {

		// 受信データを全て
		for (int index = 0; index < receiveBytes; index++) {

			// 最大パケット長以下なら
			if (_count < sizeof(_ringBuffer)) {

				// 受信データを蓄積する
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

/*	<summary>要求の取り出し</summary>
	<parameter name="receiveData">要求データ</parameter>
	<parameter name="receiveBytes">最大要求バイト数</parameter>
	<return>要求バイト数(0:なし)</return>
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
