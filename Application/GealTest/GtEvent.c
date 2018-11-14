/*	GEAL Test Server: �C�x���g����
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include "GealTypes.h"
#include "GealTimerAPI.h"
#include "GtEvent.h"
#include "GtPort.h"

extern GT_PORT GtUDPPort;
static GT_PORT* _port = &GtUDPPort;

/*	<summary>�A�v���P�[�V����������</summary>
*/
GE_VOID UGxAppInitialize() {

	// �A�v���P�[�V����������
	UGtAppInitialize();

	// �ʐM�|�[�g���N��
	int port = 12345;
	int error = _port->Initialize();
	if (error == 0) {
		error = _port->Open(&port);
		if (error == 0) {
			GxTimerStart(ID_GEAL_TEST_TIMER, 100L);
		}
	}
}

/*	<summary>�A�v���P�[�V��������</summary>
	<parameter name="psMsg">���b�Z�[�W</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {

	// �ʐM�^�C�}�[�Ȃ�
	switch (psMsg->wMsg) {
	case eGEMSG_TIMER_UPDATE:
		switch (psMsg->wParam) {
		case ID_GEAL_TEST_TIMER: {

			// �v������M������
			_port->Receive();
			break;
		}
		default:
			break;
		}
		break;
	}

	// �A�v���P�[�V��������
	return UGtAppProcess(psMsg);
}

/*	<summary>�A�v���P�[�V�����I��</summary>
*/
GE_VOID UGxAppFinalize() {

	// �ʐM�|�[�g���~
	GxTimerStop(ID_GEAL_TEST_TIMER);
	int error = _port->Close();
	error = _port->Terminate();

	// �A�v���P�[�V�����I��
	UGtAppFinalize();
}

/*	<summary>�X�e�[�W�J�n</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {
	UGtStageEnter(eStageID);
}

/*	<summary>�X�e�[�W�I��</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {
	UGtStageExit(eStageID);
}

/*	<summary>���C���`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="eLayerID">���C��ID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {
	return UGtLayerRender(hTarget, eLayerID);
}

/*	<summary>�E�B�W�F�b�g�`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="sOffset">�`��ʒu</parameter>
	<parameter name="eWidgetID">�E�B�W�F�b�gID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {
	return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}