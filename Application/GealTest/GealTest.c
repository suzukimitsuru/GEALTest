/*
  GEAL Test Server
  Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <winsock2.h>
#include "GealTest.h"

#define ID_GEAL_TEST_TIMER	(7)	// �ʐM�^�C�}�[

/*
	<summary>�A�v���P�[�V����������</summary>
*/
GE_VOID UGxAppInitialize() {
	UGtAppInitialize();
}

/*
	<summary>�A�v���P�[�V��������</summary>
	<parameter name="psMsg">���b�Z�[�W</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {
	return UGtAppProcess(psMsg);
}

/*
	<summary>�A�v���P�[�V�����I��</summary>
*/
GE_VOID UGxAppFinalize() {
  UGtAppFinalize();
}

/*
	<summary>�X�e�[�W�J�n</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {
  UGtStageEnter(eStageID);
}

/*
	<summary>�X�e�[�W�I��</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {
  UGtStageExit(eStageID);
}

/*
	<summary>���C���`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="eLayerID">���C��ID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {
  return UGtLayerRender(hTarget, eLayerID);
}

/*
	<summary>�E�B�W�F�b�g�`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="sOffset">�`��ʒu</parameter>
	<parameter name="eWidgetID">�E�B�W�F�b�gID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {
  return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}