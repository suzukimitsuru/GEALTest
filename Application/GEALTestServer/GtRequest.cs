using System.Linq;

/// <summary>
/// GEAL Test Server: �v������
/// </summary>
/// <author>
/// Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
/// </author>
namespace GEALTest
{
    /// <summary>
    /// ����^�񋓌^
    /// </summary>
    public enum OperationEnum {
		oteNothing = 0,
        // Event       S->C
        UGxAppInitialize = 0x0101,  // GE_VOID UGtAppInitialize(GE_VOID);
        UGxAppFinalize = 0x0102,    // GE_VOID UGtAppFinalize(GE_VOID);
        UGxStageEnter = 0x0103,     // GE_VOID UGtStageEnter(GE_ID eStageID);
        UGxStageExit = 0x0104,      // GE_VOID UGtStageExit(GE_ID eStageID);
        UGxLayerRender = 0x0105,    // GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);
        UGxWidgetRender = 0x0106,   // GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);
                                    // Message     S<->C GE_BOOL UGtAppProcess(GE_MSG* psMsg);
        eGEMSG_MOUSEDOWN = 0x0201,      // eGEMSG_MOUSEDOWN                             dwParam ���16bit:�������W ����16bit:�������W
        eGEMSG_MOUSEUP = 0x0202,        // eGEMSG_MOUSEUP                               dwParam ���16bit:�������W ����16bit:�������W
        eGEMSG_TIMER_UPDATE = 0x0203,   // eGEMSG_TIMER_UPDATE   wParam �^�C�}�[ID
        eGEMSG_BUTTON_DOWN = 0x0204,    // eGEMSG_BUTTON_DOWN    wParam �E�B�W�F�b�g ID
        eGEMSG_BUTTON_CLICK = 0x0205,   // eGEMSG_BUTTON_CLICK   wParam �E�B�W�F�b�g ID
        eGEMSG_LISTITEM_DOWN = 0x0206,  // eGEMSG_LISTITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���16bit:�s�ԍ� ����16bit:�A�C�e���ԍ�
        eGEMSG_LISTBAR_DOWN = 0x0207,   // eGEMSG_LISTBAR_DOWN   wParam �E�B�W�F�b�g ID dwParam                  ����16bit:�X�N���[���o�[�R�}���h
        eGEMSG_MENUITEM_DOWN = 0x0208,  // eGEMSG_MENUITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���j���[�A�C�e���ԍ�
        eGEMSG_USEREVENT = 0x0209,		// eGEMSG_USEREVENT      wParam �C�x���gID(eGE_EVENT_ID) dwParam �J�X�^������
		// WaitEvent   S<-C
		oteWaitEvent_AppInitialize = 0x0301,	// GE_VOID UGtAppInitialize(GE_VOID);
		oteWaitEvent_AppFinalize = 0x0302,		// GE_VOID UGtAppFinalize(GE_VOID);
		oteWaitEvent_StageEnter = 0x0303,		// GE_VOID UGtStageEnter(GE_ID eStageID);
		oteWaitEvent_StageExit = 0x0304,		// GE_VOID UGtStageExit(GE_ID eStageID);
		oteWaitEvent_LayerRender = 0x0305,		// GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);
		oteWaitEvent_WidgetRender = 0x0306, 	// GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);
		// WaitMessage S<-C
		oteWaitMessage_MOUSEDOWN = 0x0401,		// eGEMSG_MOUSEDOWN                             dwParam ���16bit:�������W ����16bit:�������W
		oteWaitMessage_MOUSEUP = 0x0402,		// eGEMSG_MOUSEUP                               dwParam ���16bit:�������W ����16bit:�������W
		oteWaitMessage_TIMER_UPDATE = 0x0403,	// eGEMSG_TIMER_UPDATE   wParam �^�C�}�[ID
		oteWaitMessage_BUTTON_DOWN = 0x0404,	// eGEMSG_BUTTON_DOWN    wParam �E�B�W�F�b�g ID
		oteWaitMessage_BUTTON_CLICK = 0x0405,	// eGEMSG_BUTTON_CLICK   wParam �E�B�W�F�b�g ID
		oteWaitMessage_LISTITEM_DOWN = 0x0406,	// eGEMSG_LISTITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���16bit:�s�ԍ� ���� 16bit:�A�C�e���ԍ�
		oteWaitMessage_LISTBAR_DOWN = 0x0407,	// eGEMSG_LISTBAR_DOWN   wParam �E�B�W�F�b�g ID dwParam                  ����16bit:�X�N���[���o�[�R�}���h
		oteWaitMessage_MENUITEM_DOWN = 0x0408,	// eGEMSG_MENUITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���j���[�A�C�e���ԍ�
		oteWaitMessage_USEREVENT = 0x0409,		// eGEMSG_USEREVENT      wParam �C�x���gID(eGE_EVENT_ID) dwParam �J�X�^������
		// WaitFlag    S<-C
		oteWaitFlag = 0x0501,					// Wati flag	�E�B�W�F�b�g ID mask  value
	};

    /// <summary>
    /// �Ώی^�񋓌^
    /// </summary>
    public enum TargetTypeEnum {
		tteNothing = 0,
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

    /// <summary>
    /// �Ώۂ𕶎���ɕϊ�
    /// </summary>
    /// <param name="type">�Ώی^</param>
    /// <param name="id">�Ώ�ID</param>
    /// <returns>������</returns>
	public delegate string TargetToString(TargetTypeEnum type, ushort id);

    /// <summary>
    /// ��{�v��
    /// </summary>
    public class RequestBase {
		private OperationEnum operation;
		public OperationEnum Operation { get { return this.operation; } set { this.operation = value; } }

		private TargetTypeEnum targetType;
		public TargetTypeEnum TargetType { get { return this.targetType; } set { this.targetType = value; } }

		private ushort targetId;
		public ushort TargetId { get { return this.targetId; } set { this.targetId = value; } }

		public RequestBase () {
			this.operation = OperationEnum.oteNothing;
			this.targetType = TargetTypeEnum.tteNothing;
			this.targetId = 0;
		}
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="operation">����</param>
        /// <param name="targetType">�Ώی^</param>
        /// <param name="targetId">�Ώ�ID</param>
        public RequestBase(OperationEnum operation, TargetTypeEnum targetType, ushort targetId)
        {
            this.operation = operation;
            this.targetType = targetType;
            this.targetId = targetId;
        }
        public RequestBase(byte[] request)
        {
            if (request.Length >= 6)
            {
                int bytes = request[0];
                if (bytes >= (6 - 1))
                {
                    this.operation = (OperationEnum)((request[2] << 8) | request[1]);
                    this.targetType = (TargetTypeEnum)request[3];
                    this.targetId = (ushort)((request[5] << 8) | request[4]);
                }
            }
        }
        virtual public byte[] GetBytes()
        {
            var length = 6 - 1;
            var operation = (int)this.operation;
            var targetType = (int)this.targetType;
            return new byte[] {
                (byte)length,
                (byte)((operation >> 0) & 0xff),
                (byte)((operation >> 8) & 0xff),
                (byte)(targetType & 0xff),
                (byte)((this.targetId >> 0) & 0xff),
                (byte)((this.targetId >> 8) & 0xff),
            };
        }
        virtual public string ToString(TargetToString target)
        {
            return string.Format("{0}({1});", this.Operation.ToString(), target(this.TargetType, this.TargetId));
        }
    }

    /// <summary>
    /// �����v��
    /// </summary>
    public class RequestParameter : RequestBase {
		private uint parameter;
		public uint Parameter { get { return this.parameter; } set { this.parameter = value; } }

        public RequestParameter() : base()
        {
            this.parameter = 0;
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="operation">����</param>
        /// <param name="targetType">�Ώی^</param>
        /// <param name="targetId">�Ώ�ID</param>
        /// <param name="parameter">����</param>
        public RequestParameter(OperationEnum operation, TargetTypeEnum targetType, ushort targetId, uint parameter) : base(operation, targetType, targetId)
        {
            this.parameter = parameter;
        }
        public RequestParameter(byte[] request) : base(request)
        {
            if (request.Length >= (6 + 4))
            {
                int bytes = request[0];
                if (bytes >= (6 + 4 - 1))
                {
                    this.parameter = (uint)((request[9] << 24) | (request[8] << 16) | (request[7] << 8) | request[6]);
                }
            }
        }
        override public byte[] GetBytes()
        {
            var base_bytes = base.GetBytes();
            var parameter_bytes = new byte[] {
                (byte)((this.parameter >> 0) & 0xff),
                (byte)((this.parameter >> 8) & 0xff),
                (byte)((this.parameter >> 16) & 0xff),
                (byte)((this.parameter >> 24) & 0xff),
            };
            return base_bytes.Concat(parameter_bytes).ToArray();
        }
        override public string ToString(TargetToString target)
        {
            var param = "";
            var comment = "";
            var target_name = target(this.TargetType, this.TargetId);
            switch (this.Operation)
            {
                case OperationEnum.UGxAppInitialize: comment = "Version=" + this.Parameter.ToString(); break;
                case OperationEnum.eGEMSG_MOUSEDOWN:
                case OperationEnum.eGEMSG_MOUSEUP:
                case OperationEnum.UGxWidgetRender: param = string.Format("{0}, GE_POINT(x={1}, y={2})", target_name, (this.parameter >> 0) & 0xFFFF, (this.parameter >> 16) & 0xFFFF); break;
                case OperationEnum.eGEMSG_LISTITEM_DOWN: param = string.Format("{0}, line={1}, item={2})", target_name, (this.parameter >> 0) & 0xFFFF, (this.parameter >> 16) & 0xFFFF); break;
                case OperationEnum.eGEMSG_LISTBAR_DOWN: param = string.Format("{0}, command={2})", target_name, (this.parameter >> 0) & 0xFFFF, (this.parameter >> 16) & 0xFFFF); break;

                default: param = string.Format("{0}, 0x{1:X8}", target_name, this.Parameter); break;
            }
            return string.Format("{0}({1});{2}", this.Operation.ToString(), param, (comment.Length > 0) ? "\t// " + comment : "");
        }
    }

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
}