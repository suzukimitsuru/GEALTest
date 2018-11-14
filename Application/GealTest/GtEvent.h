/*	GEAL Test Server: GEALユーザイベント定義
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_EVENT_H
#define	_INC_GEAL_TEST_EVENT_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

#include "GealTypes.h"

#define ID_GEAL_TEST_TIMER	(7)	// 通信タイマー

/*	<summary>アプリケーション初期化</summary>
*/
GE_VOID UGtAppInitialize(GE_VOID);

/*	<summary>アプリケーション処理</summary>
	<parameter name="psMsg">メッセージ</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGtAppProcess(GE_MSG* psMsg);

/*	<summary>アプリケーション終了</summary>
*/
GE_VOID UGtAppFinalize(GE_VOID);

/*	<summary>ステージ開始</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGtStageEnter(GE_ID eStageID);

/*	<summary>ステージ終了</summary>
	<parameter name="eStageID">ステージID</parameter>
*/
GE_VOID UGtStageExit(GE_ID eStageID);

/*	<summary>レイヤ描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="eLayerID">レイヤID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);

/*	<summary>ウィジェット描画</summary>
	<parameter name="hTarget">画面ハンドル</parameter>
	<parameter name="sOffset">描画位置</parameter>
	<parameter name="eWidgetID">ウィジェットID</parameter>
	<return>処理済みフラグ</return>
*/
GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_EVENT_H
