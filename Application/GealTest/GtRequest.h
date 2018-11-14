/*	GEAL Test Server: 受信要求定義
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_REQUEST_H
#define	_INC_GEAL_TEST_REQUEST_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>初期化</summary>
	<return>エラーコード</return>
*/
extern void GtRequestInitialize(void);

/*	<summary>要求の取得</summary>
	<return>要求(NULL:なし)</return>
*/
extern void GtRequestSet(char* packet);

/*	<summary>要求の取得</summary>
	<return>要求(NULL:なし)</return>
*/
extern char* GtRequestGet(void);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_REQUEST_H
