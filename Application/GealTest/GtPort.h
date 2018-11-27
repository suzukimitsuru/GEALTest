/*	GEAL Test Server: �ʐM�|�[�g��`
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_PORT_H
#define	_INC_GEAL_TEST_PORT_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>�ʐM�|�[�g�\����</summary>
*/
typedef struct {

	/*	<summary>������</summary>
		<parameter name="parameter">����</parameter>
		<return>�G���[�R�[�h</return>
	*/
	int (*Initialize)(void *parameter);

	/*	<summary>�I������</summary>
		<return>�G���[�R�[�h</return>
	*/
	int (*Terminate)(void);

	/*	<summary>�|�[�g���J��</summary>
		<return>�G���[�R�[�h</return>
	*/
	int (*Open)(void);

	/*	<summary>�|�[�g����</summary>
		<return>�G���[�R�[�h</return>
	*/
	int (*Close)(void);

	/*	<summary>�v������M</summary>
		<parameter name="buffer">��M�o�b�t�@</parameter>
		<parameter name="maxBytes">�ő��M�o�C�g��</parameter>
		<return>��M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
	*/
	int (*Receive)(unsigned char* buffer, int maxBytes);

	/*	<summary>�p�P�b�g�𑗐M</summary>
		<parameter name="packet">���M�p�P�b�g</parameter>
		<parameter name="bytes">���M�o�C�g��</parameter>
		<return>���M�o�C�g��(0:�Ȃ�, ��0:����, ��0:�G���[�R�[�h)</return>
	*/
	int (*Send)(unsigned char* packet, int bytes);
} GT_PORT;

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_PORT_H
