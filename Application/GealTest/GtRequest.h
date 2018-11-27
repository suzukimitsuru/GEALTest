/*	GEAL Test Server: �v������
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
	+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
	|len|num|T|C|
	+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
	target
		resource	type
	 0:	static		0:bitmap 1:font 2:string 3:language 4:event 5:border 6:stage 7:layer
	 1:	rect
	 2:	text
	 3:	button
	 4:	picture
	 5:	gauge
	 6:	menu
	 7: list
	 8: figure
	 9:
	10:
	11:
	12:
	13:
	14:
	15:	
eGE_WIDGET_TYPE WGTID_TYPE(GE_WGTID id);

#define eWGTIDRECT  0x2000   Widget Rect
#define eWGTIDTEXT  0x3000  Widget Text
	eGEWTXT_STRING	�����񃊃\�[�X��\������e�L�X�g�E�B�V?�F�b�g��\���܂��B
	eGEWTXT_NUMBER	32bit ������\������e�L�X�g�E�B�V?�F�b�g��\���܂��B
	eGEWTXT_MULTI	�����̕����񃊃\�[�X��؂�ւ��ĕ\������e�L�X�g�E�B�V?�F �b�g��\���܂��B
	eGEWTXT_DYNAMIC	���I�ȕ������\������e�L�X�g�E�B�V?�F�b�g��\���܂��B
#define eWGTIDBTN   0x4000  Widget Button
	eGEWBTN_PUSH	�N���b�N�\�ȃz?�^���E�B�V?�F�b�g��\���܂��B
	eGEWBTN_RADIO	�����̃z?�^���̂���?�ꂩ1��?���I�����邱�Ƃ�?�o����z? �^���E�B�V?�F�b�g��\���܂��B
	eGEWBTN_TOGGLE	�N���b�N���閈�ɑI���A�ʏ�/�I���̏�Ԃ�?�؂�ւ��z? �^���E�B�V?�F�b�g��\���܂��B
#define eWGTIDPICT  0x5000  Widget Picture
#define eWGTIDGAUGE 0x6000  Widget Gauge
	eGEGAUGE_ORI_HORIZONTAL	��������\���܂��B
	eGEGAUGE_ORI_VERTICAL	�c������\���܂��B
	�������̃P?�[�V?�̐��l��?�����������
	eGEGAUGE_DIR_TORIGHT	������E�ւ̕�����\���܂��B
	eGEGAUGE_DIR_TOLEFT		�E���獶�ւ̕�����\���܂��B
	�c�����̃P?�[�V?�̐��l��?�����������
	eGEGAUGE_DIR_TOTOP		�������ւ̕�����\���܂��B
	eGEGAUGE_DIR_TOBOTTOM	�ォ�牺�ւ̕�����\���܂��B
#define eWGTIDMENU  0x7000  Widget Menu
#define eWGTIDLIST  0x8000  Widget List
	eGEWLIST_RSCSTRG	���X�g�̃A�C�e���ɂ͕����񃊃\�[�X�� ID ��?�i�[����܂��B
	eGEWLIST_DYNAMIC	���X�g�̃A�C�e���ɂ͓��I������(GE_TCHAR)�̃A�g?���X��? �i�[����܂��B
	eGEWLIST_BARMODE_HIDE	�X�N���[���n?�[��\�����܂���B
	eGEWLIST_BARMODE_SHOW	�X�N���[���n?�[����ɕ\�����܂��B
	eGEWLIST_BARMODE_AUTO	�X�N���[���\�ȏꍇ�̂݃X�N���[���n?�[��\�����܂��B
#define eWGTIDFIG  0x9000   Widget Fig
	eGEWFIG_LINE	�}�`�^�C�t?��?������?���邱�Ƃ�\���܂��B
	eGEWFIG_POLYGON	�}�`�^�C�t?��?���p�`��?���邱�Ƃ�\���܂��B
	eGEWFIG_ELLIPSE	�}�`�^�C�t?��?�ȉ~��?���邱�Ƃ�\���܂��B
	eGEWFIG_ARC		�}�`�^�C�t?��?�~�ʂ�?���邱�Ƃ�\���܂��B
*/
#ifndef _INC_GEAL_TEST_REQUEST_H
#define	_INC_GEAL_TEST_REQUEST_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>�v���\����	</summary>
*/
typedef struct {
	int number;
	int target;
	int operation;
}	GT_REQUEST;

/*	<summary>�v���̏�����</summary>
*/
extern void GtRequetInitialize(void);

/*	<summary>�v���̒~��</summary>
	<parameter name="receiveData">��M�f�[�^</parameter>
	<parameter name="receiveBytes">��M�o�C�g��</parameter>
	<return>�I�[�o�[�t���o�C�g��(0:�Ȃ�)</return>
*/
extern int GtRequetPut(unsigned char* receiveData, int receiveBytes);

/*	<summary>�v���̎��o��</summary>
	<parameter name="receiveData">�v���f�[�^</parameter>
	<parameter name="receiveBytes">�ő�v���o�C�g��</parameter>
	<return>�v���o�C�g��(0:�Ȃ�)</return>
*/
extern int GtRequetGet(unsigned char* request, int maxBytes);


#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_REQUEST_H
