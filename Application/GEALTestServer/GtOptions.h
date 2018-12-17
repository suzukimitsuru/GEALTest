/*	GEAL Test Server: 動作オプション定義
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_PARAMETER_H
#define	_INC_GEAL_TEST_PARAMETER_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

#include "GealTypes.h"
#include "GtUDPPort.h"

typedef struct {
	GT_UDP_PARAMETER	udp;	// UDPポート引数
	GE_ID	timerId;		// 通信タイマーID
	GE_BOOL	RecoardMode;	// 記録モード
} GT_OPTIONS;

/*	<summary>動作オプション設定</summary>
	<return>動作オプション</return>
*/
extern GT_OPTIONS *UGtSetOptions(GE_VOID);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_PARAMETER_H
