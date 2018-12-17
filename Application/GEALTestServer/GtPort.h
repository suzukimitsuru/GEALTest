/*	GEAL Test Server: 通信ポート定義
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_PORT_H
#define	_INC_GEAL_TEST_PORT_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>通信ポート構造体</summary>
*/
typedef struct {

	/*	<summary>初期化</summary>
		<parameter name="parameter">引数</parameter>
		<return>エラーコード</return>
	*/
	int (*Initialize)(void *parameter);

	/*	<summary>終了処理</summary>
		<return>エラーコード</return>
	*/
	int (*Terminate)(void);

	/*	<summary>ポートを開く</summary>
		<return>エラーコード</return>
	*/
	int (*Open)(void);

	/*	<summary>ポート閉じる</summary>
		<return>エラーコード</return>
	*/
	int (*Close)(void);

	/*	<summary>要求を受信</summary>
		<parameter name="buffer">受信バッファ</parameter>
		<parameter name="maxBytes">最大受信バイト数</parameter>
		<return>受信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
	*/
	int (*Receive)(unsigned char* buffer, int maxBytes);

	/*	<summary>パケットを送信</summary>
		<parameter name="packet">送信パケット</parameter>
		<parameter name="bytes">送信バイト数</parameter>
		<return>送信バイト数(0:なし, ＞0:あり, ＜0:エラーコード)</return>
	*/
	int (*Send)(unsigned char* packet, int bytes);
} GT_PORT;

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_PORT_H
