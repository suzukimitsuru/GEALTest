/*	GEAL Test Server: ����I�v�V������`
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
	GT_UDP_PARAMETER	udp;	// UDP�|�[�g����
	GE_ID	timerId;		// �ʐM�^�C�}�[ID
	GE_BOOL	RecoardMode;	// �L�^���[�h
} GT_OPTIONS;

/*	<summary>����I�v�V�����ݒ�</summary>
	<return>����I�v�V����</return>
*/
extern GT_OPTIONS *UGtSetOptions(GE_VOID);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_PARAMETER_H
