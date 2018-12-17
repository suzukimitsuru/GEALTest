/*	GEAL Test Server: �C�x���g����
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <memory.h>
#include "GealAPI.h"
#include "GealTimerAPI.h"
#include "GtOptions.h"
#include "GtEvent.h"
#include "GtPort.h"
#include "GtUDPPort.h"
#include "GtRequest.h"

GT_OPTIONS* _options = GE_NULL;	// ����I�v�V����
extern GT_PORT GtUDPPort;			// UDP�|�[�g��`
static GT_PORT* _port = &GtUDPPort;	// �ʐM�|�[�g�ݒ�

/* ��M�����O�o�b�t�@ */
static unsigned char _ringBuffer[256];
static int _ringGet = 0;
static int _ringPut = 0;
static int _ringCount = 0;

static unsigned char _buffer[128];	// �ʐM�o�b�t�@

/*	<summary>��M�o�b�t�@�̏�����</summary>
*/
static void ringInitialize(void) {
	memset(_ringBuffer, 0, sizeof(_ringBuffer));
	_ringGet = 0;
	_ringPut = 0;
	_ringCount = 0;
}

/*	<summary>��M�o�b�t�@�ւ̒~��</summary>
	<parameter name="receiveData">��M�f�[�^</parameter>
	<parameter name="receiveBytes">��M�o�C�g��</parameter>
	<return>�I�[�o�[�t���o�C�g��(0:�Ȃ�)</return>
*/
static int ringPut(unsigned char* receiveData, int receiveBytes) {
	int overflow = 0;
	if ((receiveData != NULL) && (receiveBytes > 0)) {

		// ��M�f�[�^��S��
		for (int index = 0; index < receiveBytes; index++) {

			// �ő�p�P�b�g���ȉ��Ȃ�
			if (_ringCount < sizeof(_ringBuffer)) {

				// ��M�f�[�^��~�ς���
				_ringBuffer[_ringPut] = receiveData[index];
				_ringPut++; _ringPut = (_ringPut < sizeof(_ringBuffer)) ? _ringPut : 0;
				_ringCount++;
			} else {
				overflow++;
			}
		}
	}
	return overflow;
}

/*	<summary>��M�o�b�t�@����v���o�C�g���̔`����</summary>
	<return>�v���o�C�g��(0:�Ȃ�)</return>
*/
static int ringBytesPeek() {
	int result = 0;
	if (_ringCount >= 0) {
		int requresultest_bytes = ((GT_REQUEST_BASE*)&_ringBuffer[_ringGet])->bytes;
	}
	return result;
}

/*	<summary>��M�o�b�t�@����̎��o��</summary>
	<parameter name="request">�v���f�[�^</parameter>
	<parameter name="maxBytes">�ő�v���o�C�g��</parameter>
	<return>�v���o�C�g��(0:�Ȃ�)</return>
*/
static int ringGet(unsigned char* request, int maxBytes) {
	int result = 0;
	if (_ringCount >= 0) {
		int request_bytes = ((GT_REQUEST_BASE*)&_ringBuffer[_ringGet])->bytes;
		if (_ringCount >= request_bytes) {
			for (int index = 0; (index < request_bytes) && (index < maxBytes); index++) {
				request[index] = _ringBuffer[_ringGet];
				_ringGet++; _ringGet = (_ringGet < sizeof(_ringBuffer)) ? _ringGet : 0;
				_ringCount--;
				result++;
			}
		}
	}
	return result;
}

/*	<summary>�A�v���P�[�V����������</summary>
*/
GE_VOID UGxAppInitialize() {
	_options = UGtSetOptions();

	// �A�v���P�[�V����������
	UGtAppInitialize();

	// �ʐM�|�[�g���N��
	int error = _port->Initialize(&_options->udp);
	if (error == 0) {
		error = _port->Open();
		if (error == 0) {
			ringInitialize();
			GxTimerStart(_options->timerId, 100L);

			// �L�^���b�Z�[�W���M
			if (_options->RecoardMode) {
				GT_REQUEST_PARAMTER req;
				GtRequetParameterSet(&req, oteEvent_UGxAppInitialize, tteNothing, 0, GEAL_VERSION);
				_port->Send((unsigned char*)&req, sizeof(req));
			}
		}
	}
}

/*	<summary>�A�v���P�[�V��������</summary>
	<parameter name="psMsg">���b�Z�[�W</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxAppProcess(GE_MSG* psMsg) {
	enum OperationEnum operation = oteNothing;
	enum TargetTypeEnum target = tteWIDGET;
	int request = 0;

	// �ʐM�^�C�}�[�Ȃ�
	switch (psMsg->wMsg) {
	case eGEMSG_BUTTON_DOWN:	operation = oteMessage_BUTTON_DOWN;		target = tteWIDGET;	request = 1;	break;
	case eGEMSG_BUTTON_CLICK:	operation = oteMessage_BUTTON_CLICK;	target = tteWIDGET;	request = 1;	break;
	case eGEMSG_LISTITEM_DOWN:	operation = oteMessage_LISTITEM_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_LISTBAR_DOWN:	operation = oteMessage_LISTBAR_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_MENUITEM_DOWN:	operation = oteMessage_MENUITEM_DOWN;	target = tteWIDGET;	request = 2;	break;
	case eGEMSG_USEREVENT:		operation = oteMessage_USEREVENT;		target = tteEVENT;	request = 2;	break;
	case eGEMSG_TIMER_UPDATE:
		if (psMsg->wParam == _options->timerId) {

			// ��M������A�v���ɒ~�ς���
			memset(_buffer, 0, sizeof(_buffer));
			int receives = _port->Receive(_buffer, sizeof(_buffer));
			if (receives > 0) {
				ringPut(_buffer, receives);
			}

			// �v�������o��
			int requests = 0;
			GT_REQUEST_PARAMTER request;
			request.parameter = 0L;
			int request_bytes = ringBytesPeek();
			if (request_bytes >= sizeof(GT_REQUEST_PARAMTER)) {
				requests = ringGet((unsigned char*)&request, sizeof(request));
			} else {
				if (request_bytes >= sizeof(GT_REQUEST_BASE)) {
					requests = ringGet((unsigned char*)&request.base, sizeof(request.base));
				}
			}

			// �v�����݂�΁A�v�������s����
			if (requests > 0) {
				unsigned short message = 0;
				enum OperationEnum operation = request.base.operation;
				switch (operation) {
					case oteMessage_BUTTON_DOWN:	message = oteMessage_BUTTON_DOWN;	break;
					case oteMessage_BUTTON_CLICK:	message = eGEMSG_BUTTON_CLICK;		break;
					case oteMessage_LISTITEM_DOWN:	message = eGEMSG_LISTITEM_DOWN;		break;
					case oteMessage_LISTBAR_DOWN:	message = eGEMSG_LISTBAR_DOWN;		break;
					case oteMessage_MENUITEM_DOWN:	message = eGEMSG_MENUITEM_DOWN;		break;
					case oteMessage_USEREVENT:		message = eGEMSG_USEREVENT;			break;
					default:						message = 0;						break;
				}
				if (message > 0) {
					psMsg->wMsg = message;
					psMsg->wParam = request.base.targetId;
					psMsg->dwParam = request.parameter;
				}
			}
		}
		break;
	}

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode && (operation != oteNothing)) {
		switch (request) {
			case 1: {
				GT_REQUEST_BASE req;
				GtRequetBaseSet(&req, operation, target, psMsg->wParam);
				_port->Send((unsigned char*)&req, sizeof(req));
				break;
			}
			case 2: {
				GT_REQUEST_PARAMTER req;
				GtRequetParameterSet(&req, operation, target, psMsg->wParam, psMsg->dwParam);
				_port->Send((unsigned char*)&req, sizeof(req));
				break;
			}
		}
	}

	// �A�v���P�[�V��������
	return UGtAppProcess(psMsg);
}

/*	<summary>�A�v���P�[�V�����I��</summary>
*/
GE_VOID UGxAppFinalize() {

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxAppFinalize, tteNothing, 0);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// �ʐM�|�[�g���~
	GxTimerStop(_options->timerId);
	int error = _port->Close();
	error = _port->Terminate();

	// �A�v���P�[�V�����I��
	UGtAppFinalize();
}

/*	<summary>�X�e�[�W�J�n</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageEnter(GE_ID eStageID) {

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxStageEnter, tteSTAGE, eStageID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}
	// �X�e�[�W�J�n
	UGtStageEnter(eStageID);
}

/*	<summary>�X�e�[�W�I��</summary>
	<parameter name="eStageID">�X�e�[�WID</parameter>
*/
GE_VOID UGxStageExit(GE_ID eStageID) {

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxStageExit, tteSTAGE, eStageID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// �X�e�[�W�I��
	UGtStageExit(eStageID);
}

/*	<summary>���C���`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="eLayerID">���C��ID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxLayerRender(HTARGET hTarget, GE_ID eLayerID) {

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode) {
		GT_REQUEST_BASE req;
		GtRequetBaseSet(&req, oteEvent_UGxLayerRender, tteLAYER, eLayerID);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// ���C���`��
	return UGtLayerRender(hTarget, eLayerID);
}

/*	<summary>�E�B�W�F�b�g�`��</summary>
	<parameter name="hTarget">��ʃn���h��</parameter>
	<parameter name="sOffset">�`��ʒu</parameter>
	<parameter name="eWidgetID">�E�B�W�F�b�gID</parameter>
	<return>�����ς݃t���O</return>
*/
GE_BOOL UGxWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID) {

	// �L�^���b�Z�[�W���M
	if (_options->RecoardMode) {
		GT_REQUEST_PARAMTER req;
		unsigned int param = (sOffset.y << 16) + sOffset.x;
		GtRequetParameterSet(&req, oteEvent_UGxWidgetRender, tteWIDGET, eWidgetID, param);
		_port->Send((unsigned char*)&req, sizeof(req));
	}

	// �E�B�W�F�b�g�`��
	return UGtWidgetRender(hTarget, sOffset, eWidgetID);
}