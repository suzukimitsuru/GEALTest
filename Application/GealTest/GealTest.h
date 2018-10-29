/*
  GEAL Test Server: �Ώۃv���W�F�N�g�ł̒�`
  Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_TARGET_H
#define	_INC_GEAL_TEST_TARGET_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

#include "GealTypes.h"

/*
	<summary>�A�v���P�[�V����������</summary>
*/
GE_VOID UGtAppInitialize(GE_VOID);

/*
	<summary>�A�v���P�[�V��������</summary>
	<parameter name="psMsg">���b�Z�[�W</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGtAppProcess(GE_MSG* psMsg);

/*
	<summary>�A�v���P�[�V�����I��</summary>
*/
GE_VOID UGtAppFinalize(GE_VOID);

/*
	<summary>�X�e�[�W�J�n</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGtStageEnter(GE_ID eStageID);

/*
	<summary>�X�e�[�W�I��</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGtStageExit(GE_ID eStageID);

/*
	<summary>���C���`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="eLayerID">���C��ID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);

/*
	<summary>�E�B�W�F�b�g�`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="sOffset">�`��ʒu</parameter>
	<parameter name="eWidgetID">�E�B�W�F�b�gID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_TARGET_H
