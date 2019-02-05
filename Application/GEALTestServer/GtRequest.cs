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
    enum OperationEnum {
		NoOperation = 0,
        // Event       S->C
        UGxAppInitialize = 0x0101,  // GE_VOID UGtAppInitialize(GE_VOID);
        UGxAppFinalize = 0x0102,    // GE_VOID UGtAppFinalize(GE_VOID);
        UGxStageEnter = 0x0103,     // GE_VOID UGtStageEnter(GE_ID eStageID);
        UGxStageExit = 0x0104,      // GE_VOID UGtStageExit(GE_ID eStageID);
        UGxLayerRender = 0x0105,    // GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);
        UGxWidgetRender = 0x0106,   // GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);
        // Message     S<->C           GE_BOOL UGtAppProcess(GE_MSG* psMsg);
        eGEMSG_MOUSEDOWN = 0x0201,      // eGEMSG_MOUSEDOWN                             dwParam ���16bit:�������W ����16bit:�������W
        eGEMSG_MOUSEUP = 0x0202,        // eGEMSG_MOUSEUP                               dwParam ���16bit:�������W ����16bit:�������W
        eGEMSG_TIMER_UPDATE = 0x0203,   // eGEMSG_TIMER_UPDATE   wParam �^�C�}�[ID
        eGEMSG_BUTTON_DOWN = 0x0204,    // eGEMSG_BUTTON_DOWN    wParam �E�B�W�F�b�g ID
        eGEMSG_BUTTON_CLICK = 0x0205,   // eGEMSG_BUTTON_CLICK   wParam �E�B�W�F�b�g ID
        eGEMSG_LISTITEM_DOWN = 0x0206,  // eGEMSG_LISTITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���16bit:�s�ԍ� ����16bit:�A�C�e���ԍ�
        eGEMSG_LISTBAR_DOWN = 0x0207,   // eGEMSG_LISTBAR_DOWN   wParam �E�B�W�F�b�g ID dwParam                  ����16bit:�X�N���[���o�[�R�}���h
        eGEMSG_MENUITEM_DOWN = 0x0208,  // eGEMSG_MENUITEM_DOWN  wParam �E�B�W�F�b�g ID dwParam ���j���[�A�C�e���ԍ�
        eGEMSG_USEREVENT = 0x0209,		// eGEMSG_USEREVENT      wParam �C�x���gID(eGE_EVENT_ID) dwParam �J�X�^������
		// WaitFlag    S<-C
		//WaitFlag = 0x0501,			// Wait flag	�E�B�W�F�b�g ID mask  value
	};

    /// <summary>
    /// �Ώی^�񋓌^
    /// </summary>
    enum TargetTypeEnum {
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
	public delegate string TargetToString(byte type, ushort id);

    /// <summary>
    /// �v���H��
    /// </summary>
    public class RequestFactory
    {
        private TargetToString targetToString;
        public RequestFactory(TargetToString targetToString)
        {
            this.targetToString = targetToString;
        }
        public RequestBase GetRequest(byte[] received)
        {
            RequestBase result = null;
            if (received.Length >= 6)
            {
                var analize = new RequestBase(received);
                switch (analize.Operation)
                {
                    case (ushort)OperationEnum.UGxAppInitialize: result = new UGxAppInitialize(received); break;
                    case (ushort)OperationEnum.UGxAppFinalize: result = new UGxAppFinalize(); break;
                    case (ushort)OperationEnum.UGxStageEnter: result = new UGxStageEnter(this.targetToString, received); break;
                    case (ushort)OperationEnum.UGxStageExit: result = new UGxStageExit(this.targetToString, received); break;
                    case (ushort)OperationEnum.UGxLayerRender: result = new UGxLayerRender(this.targetToString, received); break;
                    case (ushort)OperationEnum.UGxWidgetRender: result = new UGxWidgetRender(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_MOUSEDOWN: result = new MouseDown(received); break;
                    case (ushort)OperationEnum.eGEMSG_MOUSEUP: result = new MouseUp(received); break;
                    case (ushort)OperationEnum.eGEMSG_TIMER_UPDATE: result = new TimerUpdate(received); break;
                    case (ushort)OperationEnum.eGEMSG_BUTTON_DOWN: result = new ButtonDown(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_BUTTON_CLICK: result = new ButtonClick(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_LISTITEM_DOWN: result = new ListItemDown(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_LISTBAR_DOWN: result = new ListBarDown(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_MENUITEM_DOWN: result = new MenuItemDown(this.targetToString, received); break;
                    case (ushort)OperationEnum.eGEMSG_USEREVENT: result = new UserEvent(this.targetToString, received); break;
                    default: result = analize; break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// ��{�v��
    /// </summary>
    public class RequestBase
    {
        private ushort operation;
        public ushort Operation { get { return this.operation; } set { this.operation = value; } }
        public RequestBase(ushort operation) { this.operation = operation; }
        public RequestBase(byte[] request)
        {
            if (request.Length >= 3)
            {
                int bytes = request[0];
                if (bytes >= (3 - 1))
                {
                    this.operation = (ushort)((request[2] << 8) | request[1]);
                }
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var target = (obj is RequestBase) ? (RequestBase)obj : null;
            return (target != null) 
                && (this.operation == target.operation);
        }
        virtual public byte[] GetBytes()
        {
            var length = 3 - 1;
            var operation = (int)this.operation;
            return new byte[] {
                (byte)length,
                (byte)((operation >> 0) & 0xff),
                (byte)((operation >> 8) & 0xff),
            };
        }
        virtual public string ToLogText()
        {
            return string.Format("{0}();", this.operation.ToString());
        }
    }

    /// <summary>
    /// �Ώۗv��
    /// </summary>
    public class RequestTarget : RequestBase {
        protected TargetToString targetToString;

		private byte targetType;
		public byte TargetType { get { return this.targetType; } set { this.targetType = value; } }

		private ushort targetId;
		public ushort TargetId { get { return this.targetId; } set { this.targetId = value; } }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="targetToString">�Ώۂ𕶎���ɕϊ�</param>
        /// <param name="operation">����</param>
        /// <param name="targetType">�Ώی^</param>
        /// <param name="targetId">�Ώ�ID</param>
        public RequestTarget(TargetToString targetToString, ushort operation, byte targetType, ushort targetId)
            : base (operation)
        {
            this.targetToString = targetToString;
            this.targetType = targetType;
            this.targetId = targetId;
        }
        public RequestTarget(TargetToString targetToString, byte[] request)
            : base(request)
        {
            this.targetToString = targetToString;
            if (request.Length >= 6)
            {
                int bytes = request[0];
                if (bytes >= (6 - 1))
                {
                    this.targetType = request[3];
                    this.targetId = (ushort)((request[5] << 8) | request[4]);
                }
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var target = (obj is RequestTarget) ? (RequestTarget)obj : null;
            return base.Equals(target)
                && (this.targetType == target.targetType)
                && (this.targetId == target.targetId);

        }
        public override byte[] GetBytes()
        {
            var length = 6 - 1;
            var operation = (int)this.Operation;
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
        public override string ToLogText()
        {
            return string.Format("{0}({1});", this.Operation.ToString(), this.targetToString(this.TargetType, this.TargetId));
        }
    }

    /// <summary>
    /// �����v��
    /// </summary>
    public class RequestParameter : RequestTarget {
		private uint parameter;
		public uint Parameter { get { return this.parameter; } set { this.parameter = value; } }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="targetToString">�Ώۂ𕶎���ɕϊ�</param>
        /// <param name="operation">����</param>
        /// <param name="targetType">�Ώی^</param>
        /// <param name="targetId">�Ώ�ID</param>
        /// <param name="parameter">����</param>
        public RequestParameter(TargetToString targetToString, ushort operation, byte targetType, ushort targetId, uint parameter) 
            : base(targetToString, operation, targetType, targetId)
        {
            this.parameter = parameter;
        }
        public RequestParameter(TargetToString targetToString, byte[] request)
            : base(targetToString, request)
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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var target = (obj is RequestParameter) ? (RequestParameter)obj : null;
            return base.Equals(target)
                && (this.parameter == target.parameter);
        }
        public override byte[] GetBytes()
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
        public override string ToLogText()
        {
            return string.Format("{0}({1}, 0x{2:X8});",
                this.Operation.ToString(), this.targetToString(this.TargetType, this.TargetId),
                this.Parameter);
        }
    }

    public class NoOperation : RequestBase
    {
        public NoOperation() : base((ushort)OperationEnum.NoOperation) { }
    }
    public class UGxAppInitialize : RequestParameter
    {
        public UGxAppInitialize(byte[] request) : base(null, request) { }
        public UGxAppInitialize(uint version)
        : base(null, (ushort)OperationEnum.UGxAppInitialize, (byte)TargetTypeEnum.tteNothing, 0, version) { }
        public override string ToLogText()
        {
            return string.Format("{0}();\t// Version={1}", this.Operation.ToString(), this.Parameter.ToString());
        }
    }
    public class UGxAppFinalize : RequestBase
    {
        public UGxAppFinalize() : base((ushort)OperationEnum.UGxAppFinalize) { }
    }
    public class UGxStageEnter : RequestTarget
    {
        public UGxStageEnter(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public UGxStageEnter(TargetToString targetToString, ushort stageId)
            : base(targetToString, (ushort)OperationEnum.UGxStageEnter, (byte)TargetTypeEnum.tteSTAGE, stageId) { }
    }
    public class UGxStageExit : RequestTarget
    {
        public UGxStageExit(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public UGxStageExit(TargetToString targetToString, ushort stageId)
            : base(targetToString, (ushort)OperationEnum.UGxStageExit, (byte)TargetTypeEnum.tteSTAGE, stageId) { }
    }
    public class UGxLayerRender : RequestTarget
    {
        public UGxLayerRender(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public UGxLayerRender(TargetToString targetToString, ushort layerId)
             : base(targetToString, (ushort)OperationEnum.UGxLayerRender, (byte)TargetTypeEnum.tteLAYER, layerId) { }
    }
    public class UGxWidgetRender : RequestParameter
    {
        public UGxWidgetRender(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public UGxWidgetRender(TargetToString targetToString, ushort widgetId, ushort x, ushort y)
        : base(targetToString, (ushort)OperationEnum.UGxWidgetRender, (byte)TargetTypeEnum.tteWIDGET, widgetId, (uint)(y << 16) | x) { }
        public override string ToLogText()
        {
            return string.Format("{0}({1}, GE_POINT(x={2}, y={3}));", 
                this.Operation.ToString(), this.targetToString(this.TargetType, this.TargetId), 
                (this.Parameter >> 0) & 0xFFFF, (this.Parameter >> 16) & 0xFFFF);
        }
    }
    public class MouseDown : RequestParameter
    {
        public MouseDown(byte[] request) : base(null, request) { }
        public MouseDown(ushort x, ushort y)
        : base(null, (ushort)OperationEnum.eGEMSG_MOUSEDOWN, (byte)TargetTypeEnum.tteNothing, 0, (uint)(y << 16) | x) { }
        public override string ToLogText()
        {
            return string.Format("{0}(x={1}, y={2});", this.Operation.ToString(),
                (this.Parameter >> 0) & 0xFFFF, (this.Parameter >> 16) & 0xFFFF);
        }
    }
    public class MouseUp : RequestParameter
    {
        public MouseUp(byte[] request) : base(null, request) { }
        public MouseUp(ushort x, ushort y)
        : base(null, (ushort)OperationEnum.eGEMSG_MOUSEUP, (byte)TargetTypeEnum.tteNothing, 0, (uint)(y << 16) | x) { }
        public override string ToLogText()
        {
            return string.Format("{0}(x={1}, y={2});", this.Operation.ToString(),
                (this.Parameter >> 0) & 0xFFFF, (this.Parameter >> 16) & 0xFFFF);
        }
    }
    public class TimerUpdate : RequestTarget
    {
        public TimerUpdate(byte[] request) : base(null, request) { }
        public TimerUpdate(ushort timerId)
            : base(null, (ushort)OperationEnum.eGEMSG_TIMER_UPDATE, (byte)TargetTypeEnum.tteNothing, timerId) { }
        public override string ToLogText()
        {
            return string.Format("{0}({1});", this.Operation.ToString(), this.TargetId);
        }
    }
    public class ButtonDown : RequestTarget
    {
        public ButtonDown(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public ButtonDown(TargetToString targetToString, ushort widgetId)
            : base(targetToString, (ushort)OperationEnum.eGEMSG_BUTTON_DOWN, (byte)TargetTypeEnum.tteWIDGET, widgetId) { }
    }
    public class ButtonClick : RequestTarget
    {
        public ButtonClick(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public ButtonClick(TargetToString targetToString, ushort widgetId)
            : base(targetToString, (ushort)OperationEnum.eGEMSG_BUTTON_CLICK, (byte)TargetTypeEnum.tteWIDGET, widgetId) { }
    }
    public class ListItemDown : RequestParameter
    {
        public ListItemDown(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public ListItemDown(TargetToString targetToString, ushort widgetId, ushort line, ushort item)
        : base(targetToString, (ushort)OperationEnum.eGEMSG_LISTITEM_DOWN, (byte)TargetTypeEnum.tteWIDGET, widgetId, (uint)(line << 16) | item) { }
        public override string ToLogText()
        {
            return string.Format("{0}({1}, line={2}, item={3});",
                this.Operation.ToString(), this.targetToString(this.TargetType, this.TargetId),
                (this.Parameter >> 0) & 0xFFFF, (this.Parameter >> 16) & 0xFFFF);
        }
    }
    public class ListBarDown : RequestParameter
    {
        public ListBarDown(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public ListBarDown(TargetToString targetToString, ushort widgetId, ushort command)
        : base(targetToString, (ushort)OperationEnum.eGEMSG_LISTBAR_DOWN, (byte)TargetTypeEnum.tteWIDGET, widgetId, command) { }
        public override string ToLogText()
        {
            return string.Format("{0}({1}, command={3});",
                this.Operation.ToString(), this.targetToString(this.TargetType, this.TargetId),
                (this.Parameter >> 0) & 0xFFFF, (this.Parameter >> 16) & 0xFFFF);
        }
    }
    public class MenuItemDown : RequestParameter
    {
        public MenuItemDown(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public MenuItemDown(TargetToString targetToString, ushort widgetId, uint parameter)
        : base(targetToString, (ushort)OperationEnum.eGEMSG_MENUITEM_DOWN, (byte)TargetTypeEnum.tteWIDGET, widgetId, parameter) { }
    }
    public class UserEvent : RequestParameter
    {
        public UserEvent(TargetToString targetToString, byte[] request) : base(targetToString, request) { }
        public UserEvent(TargetToString targetToString, ushort eventId, uint parameter)
        : base(targetToString, (ushort)OperationEnum.eGEMSG_USEREVENT, (byte)TargetTypeEnum.tteEVENT, eventId, parameter) { }
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