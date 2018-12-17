/*	GEAL Test Server: UDP�|�[�g
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_UDP_PORT_H
#define	_INC_GEAL_TEST_UDP_PORT_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>UDP�|�[�g����</summary>
*/
typedef struct {
	int		WaitPort;	// ��M�҂��|�[�g
	char*	ToHost;		// ���M��z�X�g
	int		ToPort;		// ���M��|�[�g
} GT_UDP_PARAMETER;

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_UDP_PORT_H
