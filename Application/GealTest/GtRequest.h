/*	GEAL Test Server: ��M�v����`
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_REQUEST_H
#define	_INC_GEAL_TEST_REQUEST_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>������</summary>
	<return>�G���[�R�[�h</return>
*/
extern void GtRequestInitialize(void);

/*	<summary>�v���̎擾</summary>
	<return>�v��(NULL:�Ȃ�)</return>
*/
extern void GtRequestSet(char* packet);

/*	<summary>�v���̎擾</summary>
	<return>�v��(NULL:�Ȃ�)</return>
*/
extern char* GtRequestGet(void);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_REQUEST_H
