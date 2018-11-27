/*	GEAL Test Server: UDPポート
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_UDP_PORT_H
#define	_INC_GEAL_TEST_UDP_PORT_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>UDPポート引数</summary>
*/
typedef struct {
	int		WaitPort;	// 受信待ちポート
	char*	ToHost;		// 送信先ホスト
	int		ToPort;		// 送信先ポート
} GT_UDP_PARAMETER;

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_UDP_PORT_H
