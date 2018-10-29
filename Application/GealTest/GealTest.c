/*
  GEAL Test Server
  Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <winsock2.h>
#include "GealTest.h"

#define ID_GEAL_TEST_TIMER	(7)	// 通信タイマー

/*
	<summary>アプリケーション初期化</summary>
*/
GE_VOID UGxAppInitialize() {
	UGtAppInitialize();
}

/*
	<summary>アプリケーション処理</summary>
	<parameter name="psMsg">メッセージ</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {
	return UGtAppProcess(psMsg);
}

/*
	<summary>アプリケーション終了</summary>
*/
GE_VOID UGxAppFinalize() {
  UGtAppFinalize();
}

/*
	<summary>ステージ開始</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {
  UGtStageEnter(eStageID);
}

/*
	<summary>ステージ終了</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {
  UGtStageExit(eStageID);
}

/*
	<summary>レイヤ描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="eLayerID">レイヤID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {
  return UGtLayerRender(hTarget, eLayerID);
}

/*
	<summary>ウィジェット描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="sOffset">描画位置</parameter>
	<parameter name="eWidgetID">ウィジェットID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {
  return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}