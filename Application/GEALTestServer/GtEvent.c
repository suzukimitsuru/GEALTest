/*	GEAL Test Server: イベント処理
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <memory.h>
#include "GealAPI.h"
#include "GealTimerAPI.h"
#include "GtOptions.h"
#include "GtEvent.h"
#include "GtPort.h"
#include "GtUDPPort.h"
#include "GtRequest.h"

GT_OPTIONS* _options = GE_NULL;	// 動作オプション
extern GT_PORT GtUDPPort;			// UDPポート定義
static GT_PORT* _port = &GtUDPPort;	// 通信ポート設定

/* 受信リングバッファ */
static unsigned char _ringBuffer[256];
static int _ringGet = 0;
static int _ringPut = 0;
static int _ringCount = 0;

static unsigned char _buffer[128];	// 通信バッファ

/*	<summary>受信バッファの初期化</summary>
*/
static void ringInitialize(void) {
	memset(_ringBuffer, 0, sizeof(_ringBuffer));
	_ringGet = 0;
	_ringPut = 0;
	_ringCount = 0;
}

/*	<summary>受信バッファへの蓄積</summary>
	<parameter name="receiveData">受信データ</parameter>
	<parameter name="receiveBytes">受信バイト数</parameter>
	<return>オーバーフロバイト数(0:なし)</return>
*/
static int ringPut(unsigned char* receiveData, int receiveBytes) {
	int overflow = 0;
	if ((receiveData != NULL) && (receiveBytes > 0)) {

		// 受信データを全て
		for (int index = 0; index < receiveBytes; index++) {

			// 最大パケット長以下なら
			if (_ringCount < sizeof(_ringBuffer)) {

				// 受信データを蓄積する
				_ringBuffer[_ringPut] = receiveData[index];
				_ringPut++; _ringPut = (_ringPut < sizeof(_ringBuffer)) ? _ringPut : 0;
				_ringCount++;
			} else {
				overflow++;
			}
		}
	}
	return overflow;
}

/*	<summary>受信バッファから要求バイト数の覗き見</summary>
	<return>要求バイト数(0:なし)</return>
*/
static int ringBytesPeek() {
	int result = 0;
	if (_ringCount >= 0) {
		int requresultest_bytes = ((GT_REQUEST_BASE*)&_ringBuffer[_ringGet])->bytes;
	}
	return result;
}

/*	<summary>受信バッファからの取り出し</summary>
	<parameter name="request">要求データ</parameter>
	<parameter name="maxBytes">最大要求バイト数</parameter>
	<return>要求バイト数(0:なし)</return>
*/
static int ringGet(unsigned char* request, int maxBytes) {
	int result = 0;
	if (_ringCount >= 0) {
		int request_bytes = ((GT_REQUEST_BASE*)&_ringBuffer[_ringGet])->bytes;
		if (_ringCount >= request_bytes) {
			for (int index = 0; (index < request_bytes) && (index < maxBytes); index++) {
				request[index] = _ringBuffer[_ringGet];
				_ringGet++; _ringGet = (_ringGet < sizeof(_ringBuffer)) ? _ringGet : 0;
				_ringCount--;
				result++;
			}
		}
	}
	return result;
}

/*	<summary>アプリケーション初期化</summary>
*/
GE_VOID UGxAppInitialize() {
	_options = UGtSetOptions();

	// アプリケーション初期化
	UGtAppInitialize();

	// 通信ポートを起動
	int error = _port->Initialize(&_options->udp);
	if (error == 0) {
		error = _port->Open();
		if (error == 0) {
			ringInitialize();
			GxTimerStart(_options->timerId, 100L);

			// 記録メッセージ送信
			if (_options->RecoardMode) {
				GT_REQUEST_PARAMTER req;
				GtRequetParameterSet(&req, oteEvent_UGxAppInitialize, tteNothing, 0, GEAL_VERSION);
				_port->Send((unsigned char*)&req, sizeof(req));
			}
		}
	}
}

/*	<summary>アプリケーション処理</summary>
	<parameter name="psMsg">メッセージ</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {
	enum OperationEnum operation = oteNothing;
	enum TargetTypeEnum target = tteWIDGET;
	int request = 0;

	// 通信タイマーなら
	switch (psMsg->wMsg) {
	case eGEMSG_BUTTON_DOWN:	operation = oteMessage_BUTTON_DOWN;		target = tteWIDGET;	request = 1;	break;
	case eGEMSG_BUTTON_CLICK:	operation = oteMessage_BUTTON_CLICK;	target = tteWIDGET;	request = 1;	break;
	case eGEMSG_LISTITEM_DOWN:	operation = oteMessage_LISTITEM_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_LISTBAR_DOWN:	operation = oteMessage_LISTBAR_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_MENUITEM_DOWN:	operation = oteMessage_MENUITEM_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_USEREVENT:		operation = oteMessage_USEREVENT;		target = tteEVENT;	request = 2;	break;
	case eGEMSG_TIMER_UPDATE:
		if (psMsg->wParam == _options->timerId) {

			// 受信したら、要求に蓄積する
			memset(_buffer, 0, sizeof(_buffer));
			int receives = _port->Receive(_buffer, sizeof(_buffer));
			if (receives > 0) {
				ringPut(_buffer, receives);
			}

			// 要求を取り出す
			int requests = 0;
			GT_REQUEST_PARAMTER request;
			request.parameter = 0L;
			int request_bytes = ringBytesPeek();
			if (request_bytes >= sizeof(GT_REQUEST_PARAMTER)) {
				requests = ringGet((unsigned char*)&request, sizeof(request));
			} else {
				if (request_bytes >= sizeof(GT_REQUEST_BASE)) {
					requests = ringGet((unsigned char*)&request.base, sizeof(request.base));
				}
			}

			// 要求が在れば、要求を実行する
			if (requests > 0) {
				unsigned short message = 0;
				enum OperationEnum operation = request.base.operation;
				switch (operation) {
					case oteMessage_BUTTON_DOWN:	message = oteMessage_BUTTON_DOWN;	break;
					case oteMessage_BUTTON_CLICK:	message = eGEMSG_BUTTON_CLICK;		break;
					case oteMessage_LISTITEM_DOWN:	message = eGEMSG_LISTITEM_DOWN;		break;
					case oteMessage_LISTBAR_DOWN:	message = eGEMSG_LISTBAR_DOWN;		break;
					case oteMessage_MENUITEM_DOWN:	message = eGEMSG_MENUITEM_DOWN;		break;
					case oteMessage_USEREVENT:		message = eGEMSG_USEREVENT;			break;
					default:						message = 0;						break;
				}
				if (message > 0) {
					psMsg->wMsg = message;
					psMsg->wParam = request.base.targetId;
					psMsg->dwParam = request.parameter;
				}
			}
		}
		break;
	}

	// 記録メッセージ送信
	if (_options->RecoardMode && (operation != oteNothing)) {
		switch (request) {
			case 1: {
				GT_REQUEST_BASE req;
				GtRequetBaseSet(&req, operation, target, psMsg->wParam);
				_port->Send((unsigned char*)&req, sizeof(req));
				break;
			}
			case 2: {
				GT_REQUEST_PARAMTER req;
				GtRequetParameterSet(&req, operation, target, psMsg->wParam, psMsg->dwParam);
				_port->Send((unsigned char*)&req, sizeof(req));
				break;
			}
		}
	}

	// アプリケーション処理
	return UGtAppProcess(psMsg);
}

/*	<summary>アプリケーション終了</summary>
*/
GE_VOID UGxAppFinalize() {

	// 記録メッセージ送信
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxAppFinalize, tteNothing, 0);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// 通信ポートを停止
	GxTimerStop(_options->timerId);
	int error = _port->Close();
	error = _port->Terminate();

	// アプリケーション終了
	UGtAppFinalize();
}

/*	<summary>ステージ開始</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {

	// 記録メッセージ送信
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxStageEnter, tteSTAGE, eStageID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}
	// ステージ開始
	UGtStageEnter(eStageID);
}

/*	<summary>ステージ終了</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {

	// 記録メッセージ送信
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxStageExit, tteSTAGE, eStageID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// ステージ終了
	UGtStageExit(eStageID);
}

/*	<summary>レイヤ描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="eLayerID">レイヤID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {

	// 記録メッセージ送信
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxLayerRender, tteLAYER, eLayerID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// レイヤ描画
	return UGtLayerRender(hTarget, eLayerID);
}

/*	<summary>ウィジェット描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="sOffset">描画位置</parameter>
	<parameter name="eWidgetID">ウィジェットID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {

	// 記録メッセージ送信
	if (_options->RecoardMode) {
		GT_REQUEST_PARAMTER req;
		unsigned int param = (sOffset.y << 16) + sOffset.x;
		GtRequetParameterSet(&req, oteEvent_UGxWidgetRender, tteWIDGET, eWidgetID, param);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// ウィジェット描画
	return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}