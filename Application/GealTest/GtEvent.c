/*	GEAL Test Server: イベント処理
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <memory.h>
#include "GealTimerAPI.h"
#include "GtEvent.h"
#include "GtPort.h"
#include "GtUDPPort.h"
#include "GtRequest.h"

extern GT_PORT GtUDPPort;
static GT_PORT* _port = &GtUDPPort;
static unsigned char _buffer[128];

/*	<summary>アプリケーション初期化</summary>
*/
GE_VOID UGxAppInitialize() {

	// アプリケーション初期化
	UGtAppInitialize();

	// 通信ポートを起動
	GT_UDP_PARAMETER param;
	param.WaitPort = *(int*)"Gt"; // 29767:0x7447
	param.ToHost = "127.0.0.1";
	param.ToPort = *(int*)"GT"; // 21575:0x5447;
	int error = _port->Initialize(&param);
	if (error == 0) {
		error = _port->Open();
		if (error == 0) {
			GtRequetInitialize();
			GxTimerStart(ID_GEAL_TEST_TIMER, 100L);
		}
	}
}

/*	<summary>アプリケーション処理</summary>
	<parameter name="psMsg">メッセージ</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {

	// 通信タイマーなら
	switch (psMsg->wMsg) {
	case eGEMSG_TIMER_UPDATE:
		switch (psMsg->wParam) {
		case ID_GEAL_TEST_TIMER: {

			// 受信したら、要求に蓄積する
			memset(_buffer, 0, sizeof(_buffer));
			int receives = _port->Receive(_buffer, sizeof(_buffer));
			if (receives > 0) {
				GtRequetPut(_buffer, receives);
			}

			// 要求が在れば、要求を実行する
			short button_id;
			int requests = GtRequetGet((unsigned char*)&button_id, sizeof(button_id));
			if (requests > 0) {
				psMsg->wMsg = eGEMSG_BUTTON_CLICK;
				psMsg->wParam = button_id;
			}
			break;
		}
		default:
			break;
		}
		break;
	}

	// アプリケーション処理
	return UGtAppProcess(psMsg);
}

/*	<summary>アプリケーション終了</summary>
*/
GE_VOID UGxAppFinalize() {

	// 通信ポートを停止
	GxTimerStop(ID_GEAL_TEST_TIMER);
	int error = _port->Close();
	error = _port->Terminate();

	// アプリケーション終了
	UGtAppFinalize();
}

/*	<summary>ステージ開始</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {
	_port->Send((unsigned char*)&eStageID, sizeof(eStageID));
	UGtStageEnter(eStageID);
}

/*	<summary>ステージ終了</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {
	UGtStageExit(eStageID);
}

/*	<summary>レイヤ描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="eLayerID">レイヤID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {
	return UGtLayerRender(hTarget, eLayerID);
}

/*	<summary>ウィジェット描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="sOffset">描画位置</parameter>
	<parameter name="eWidgetID">ウィジェットID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {
	return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}