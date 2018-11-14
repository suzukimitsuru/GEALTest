/*	GEAL Test Server: イベント処理
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include "GealTypes.h"
#include "GealTimerAPI.h"
#include "GtEvent.h"
#include "GtPort.h"

extern GT_PORT GtUDPPort;
static GT_PORT* _port = &GtUDPPort;

/*	<summary>アプリケーション初期化</summary>
*/
GE_VOID UGxAppInitialize() {

	// アプリケーション初期化
	UGtAppInitialize();

	// 通信ポートを起動
	int port = 12345;
	int error = _port->Initialize();
	if (error == 0) {
		error = _port->Open(&port);
		if (error == 0) {
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

			// 要求を受信したら
			_port->Receive();
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