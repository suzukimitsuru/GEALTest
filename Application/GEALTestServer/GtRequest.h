/*	GEAL Test Server: �v������
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#ifndef _INC_GEAL_TEST_PACKET_H
#define	_INC_GEAL_TEST_PACKET_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

	/*	<summary>����^�񋓌^</summary>
	*/
	enum OperationEnum {
		opeNoOperation = 0,
		// Event       S->C
		opeUGxAppInitialize = 0x0101,	// GE_VOID UGtAppInitialize(GE_VOID);
		opeUGxAppFinalize = 0x0102,		// GE_VOID UGtAppFinalize(GE_VOID);
		opeUGxStageEnter = 0x0103,		// GE_VOID UGtStageEnter(GE_ID eStageID);
		opeUGxStageExit = 0x0104,		// GE_VOID UGtStageExit(GE_ID eStageID);
		opeUGxLayerRender = 0x0105,		// GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);
		opeUGxWidgetRender = 0x0106,	// GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);
		// Message     S<->C GE_BOOL UGtAppProcess(GE_MSG* psMsg);
		opeMOUSEDOWN = 0x0201,			// eGEMSG_MOUSEDOWN                             dwParam ���16bit:�������W ����16bit:�������W
		opeMOUSEUP = 0x0202,			// eGEMSG_MOUSEUP                               dwParam ���16bit:�������W ����16bit:�������W
		opeTIMER_UPDATE = 0x0203,		// eGEMSG_TIMER_UPDATE   wParam �^�C�}�[ID
		opeBUTTON_DOWN = 0x0204,		// eGEMSG_BUTTON_DOWN    wParam �E�B�W�F�b�g ID
		opeBUTTON_CLICK = 0x0205,		// eGEMSG_BUTTON_CLICK   wParam �E�B�W�F�b�g ID
		opeLISTITEM_DOWN = 0x0206,		// eGEMSG_LISTITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���16bit:�s�ԍ� ���� 16bit:�A�C�e���ԍ�
		opeLISTBAR_DOWN = 0x0207,		// eGEMSG_LISTBAR_DOWN   wParam �E�B�W�F�b�g ID dwParam                  ����16bit:�X�N���[���o�[�R�}���h
		opeMENUITEM_DOWN = 0x0208,		// eGEMSG_MENUITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���j���[�A�C�e���ԍ�
		opeUSEREVENT = 0x0209,			// eGEMSG_USEREVENT      wParam �C�x���gID(eGE_EVENT_ID) dwParam �J�X�^������
		// WaitFlag    S<-C
		//opeWaitFlag = 0x0501,					// Wati flag	�E�B�W�F�b�g ID mask  value
	};

	/*	<summary>�Ώی^�񋓌^</summary>
	*/
	enum TargetTypeEnum {
		tteNoTarget = 0,
		tteBITMAP = 1,		// eGE_BITMAP_ID
		tteFONT = 2,		// eGE_FONT_ID
		tteSTRING = 3,		// eGE_STRING_ID
		tteLANGUAGE = 4,	// eGE_LANGUAGE_ID
		tteEVENT = 5,		// eGE_EVENT_ID
		tteBORDER = 6,		// eGE_BORDER_ID
		tteSTAGE = 7,		// eGE_STAGE_ID
		tteLAYER = 8,		// eGE_LAYER_ID
		tteWIDGET = 9,		// eWGTID_*
	};

#pragma pack(push, 1)
	/*	<summary>�v���\����</summary>
	*/
	typedef struct {
		unsigned char	bytes;			// �o�C�g��
		unsigned short	operation;		// ����R�[�h
		unsigned char	targetType;		// �Ώی^
		unsigned short	targetId;		// �Ώ�ID Widget ID / Value
	}	GT_REQUEST_BASE;
	typedef struct {
		GT_REQUEST_BASE	base;
		unsigned int	parameter;		// �p�����[�^
	}	GT_REQUEST_PARAMTER;
#pragma pack(pop)

	/*	<summary>��{�v���̐ݒ�</summary>
		<parameter name="request">�v��</parameter>
		<parameter name="operation">����</parameter>
		<parameter name="targetType">�Ώی^</parameter>
		<parameter name="targetId">�Ώ�ID</parameter>
	*/
	extern void GtRequetBaseSet(GT_REQUEST_BASE* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId);

	/*	<summary>�����v���̐ݒ�</summary>
		<parameter name="request">�v��</parameter>
		<parameter name="operation">����</parameter>
		<parameter name="targetType">�Ώی^</parameter>
		<parameter name="targetId">�Ώ�ID</parameter>
		<parameter name="parameter">����</parameter>
	*/
	extern void GtRequetParameterSet(GT_REQUEST_PARAMTER* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId, unsigned int parameter);

	/*	 1:	eWGTIDRECT  0x2000  * Widget Rect
		 2:	eWGTIDTEXT  0x3000  * Widget Text
				eGEWTXT_STRING	�����񃊃\�[�X��\������e�L�X�g�E�B�W�F�b�g��\���܂��B
				eGEWTXT_NUMBER	32bit ������\������e�L�X�g�E�B�W�F�b�g��\���܂��B
				eGEWTXT_MULTI	�����̕����񃊃\�[�X��؂�ւ��ĕ\������e�L�X�g�E�B�W�F�b�g��\���܂��B
				eGEWTXT_DYNAMIC	���I�ȕ������\������e�L�X�g�E�B�W�F�b�g��\���܂��B
		 3:	eWGTIDBTN   0x4000  * Widget Button
				eGEWBTN_PUSH	�N���b�N�\�ȃz?�^���E�B�W�F�b�g��\���܂��B
				eGEWBTN_RADIO	�����̃{�^���̂����ꂩ1�����I�����邱�Ƃ��o����{�^���E�B�W�F�b�g��\���܂��B
				eGEWBTN_TOGGLE	�N���b�N���閈�ɑI���A�ʏ�/�I���̏�Ԃ�?�؂�ւ��{�^���E�B�W�F�b�g��\���܂��B
		 4:	eWGTIDPICT  0x5000  * Widget Picture
		 5:	eWGTIDGAUGE 0x6000  * Widget Gauge
				eGEGAUGE_ORI_HORIZONTAL	��������\���܂��B
				eGEGAUGE_ORI_VERTICAL	�c������\���܂��B
				�������̃Q�[�W�̐��l�������������
				eGEGAUGE_DIR_TORIGHT	������E�ւ̕�����\���܂��B
				eGEGAUGE_DIR_TOLEFT		�E���獶�ւ̕�����\���܂��B
				�c�����̃Q�[�W�̐��l�������������
				eGEGAUGE_DIR_TOTOP		�������ւ̕�����\���܂��B
				eGEGAUGE_DIR_TOBOTTOM	�ォ�牺�ւ̕�����\���܂��B
		 6:	eWGTIDMENU  0x7000  * Widget Menu
		 7: eWGTIDLIST  0x8000  * Widget List
				eGEWLIST_RSCSTRG	���X�g�̃A�C�e���ɂ͕����񃊃\�[�X�� ID ���i�[����܂��B
				eGEWLIST_DYNAMIC	���X�g�̃A�C�e���ɂ͓��I������(GE_TCHAR)�̃A�h���X���i�[����܂��B
				eGEWLIST_BARMODE_HIDE	�X�N���[���o�[��\�����܂���B
				eGEWLIST_BARMODE_SHOW	�X�N���[���o�[����ɕ\�����܂��B
				eGEWLIST_BARMODE_AUTO	�X�N���[���\�ȏꍇ�̂݃X�N���[���o�[��\�����܂��B
		 8: eWGTIDFIG  0x9000  * Widget Figure
				eGEWFIG_LINE	�}�`�^�C�v�������ł��邱�Ƃ�\���܂��B
				eGEWFIG_POLYGON	�}�`�^�C�v�����p�`�ł��邱�Ƃ�\���܂��B
				eGEWFIG_ELLIPSE	�}�`�^�C�v���ȉ~�ł��邱�Ƃ�\���܂��B
				eGEWFIG_ARC		�}�`�^�C�v���~�ʂł��邱�Ƃ�\���܂��B
	*/

#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_PACKET_H
