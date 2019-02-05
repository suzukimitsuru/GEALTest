using System.Linq;

/// <summary>
/// GEAL Test Server: 要求操作
/// </summary>
/// <author>
/// Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
/// </author>
namespace GEALTest
{
    /// <summary>
    /// 操作型列挙型
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
        eGEMSG_MOUSEDOWN = 0x0201,      // eGEMSG_MOUSEDOWN                             dwParam 上位16bit:垂直座標 下位16bit:水平座標
        eGEMSG_MOUSEUP = 0x0202,        // eGEMSG_MOUSEUP                               dwParam 上位16bit:垂直座標 下位16bit:水平座標
        eGEMSG_TIMER_UPDATE = 0x0203,   // eGEMSG_TIMER_UPDATE   wParam タイマーID
        eGEMSG_BUTTON_DOWN = 0x0204,    // eGEMSG_BUTTON_DOWN    wParam ウィジェット ID
        eGEMSG_BUTTON_CLICK = 0x0205,   // eGEMSG_BUTTON_CLICK   wParam ウィジェット ID
        eGEMSG_LISTITEM_DOWN = 0x0206,  // eGEMSG_LISTITEM_DOWN  wParam ウィジェット ID dwParam 上位16bit:行番号 下位16bit:アイテム番号
        eGEMSG_LISTBAR_DOWN = 0x0207,   // eGEMSG_LISTBAR_DOWN   wParam ウィジェット ID dwParam                  下位16bit:スクロールバーコマンド
        eGEMSG_MENUITEM_DOWN = 0x0208,  // eGEMSG_MENUITEM_DOWN  wParam ウィジェット ID dwParam メニューアイテム番号
        eGEMSG_USEREVENT = 0x0209,		// eGEMSG_USEREVENT      wParam イベントID(eGE_EVENT_ID) dwParam カスタム引数
		// WaitFlag    S<-C
		//WaitFlag = 0x0501,			// Wait flag	ウィジェット ID mask  value
	};

    /// <summary>
    /// 対象型列挙型
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
    /// 対象を文字列に変換
    /// </summary>
    /// <param name="type">対象型</param>
    /// <param name="id">対象ID</param>
    /// <returns>文字列</returns>
	public delegate string TargetToString(byte type, ushort id);

    /// <summary>
    /// 要求工場
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
    /// 基本要求
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
    /// 対象要求
    /// </summary>
    public class RequestTarget : RequestBase {
        protected TargetToString targetToString;

		private byte targetType;
		public byte TargetType { get { return this.targetType; } set { this.targetType = value; } }

		private ushort targetId;
		public ushort TargetId { get { return this.targetId; } set { this.targetId = value; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetToString">対象を文字列に変換</param>
        /// <param name="operation">操作</param>
        /// <param name="targetType">対象型</param>
        /// <param name="targetId">対象ID</param>
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
    /// 引数要求
    /// </summary>
    public class RequestParameter : RequestTarget {
		private uint parameter;
		public uint Parameter { get { return this.parameter; } set { this.parameter = value; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetToString">対象を文字列に変換</param>
        /// <param name="operation">操作</param>
        /// <param name="targetType">対象型</param>
        /// <param name="targetId">対象ID</param>
        /// <param name="parameter">引数</param>
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
				eGEWTXT_STRING	文字列リソースを表示するテキストウィジェットを表します。
				eGEWTXT_NUMBER	32bit 整数を表示するテキストウィジェットを表します。
				eGEWTXT_MULTI	複数の文字列リソースを切り替えて表示するテキストウィジェットを表します。
				eGEWTXT_DYNAMIC	動的な文字列を表示するテキストウィジェットを表します。
		 3:	eWGTIDBTN   0x4000  * Widget Button
				eGEWBTN_PUSH	クリック可能なホ?タンウィジェットを表します。
				eGEWBTN_RADIO	複数のボタンのいずれか1つだけ選択することが出来るボタンウィジェットを表します。
				eGEWBTN_TOGGLE	クリックする毎に選択、通常/選択の状態か?切り替わるボタンウィジェットを表します。
		 4:	eWGTIDPICT  0x5000  * Widget Picture
		 5:	eWGTIDGAUGE 0x6000  * Widget Gauge
				eGEGAUGE_ORI_HORIZONTAL	横向きを表します。
				eGEGAUGE_ORI_VERTICAL	縦向きを表します。
				横向きのゲージの数値が増加する方向
				eGEGAUGE_DIR_TORIGHT	左から右への方向を表します。
				eGEGAUGE_DIR_TOLEFT		右から左への方向を表します。
				縦向きのゲージの数値が増加する方向
				eGEGAUGE_DIR_TOTOP		下から上への方向を表します。
				eGEGAUGE_DIR_TOBOTTOM	上から下への方向を表します。
		 6:	eWGTIDMENU  0x7000  * Widget Menu
		 7: eWGTIDLIST  0x8000  * Widget List
				eGEWLIST_RSCSTRG	リストのアイテムには文字列リソースの ID が格納されます。
				eGEWLIST_DYNAMIC	リストのアイテムには動的文字列(GE_TCHAR)のアドレスが格納されます。
				eGEWLIST_BARMODE_HIDE	スクロールバーを表示しません。
				eGEWLIST_BARMODE_SHOW	スクロールバーを常に表示します。
				eGEWLIST_BARMODE_AUTO	スクロール可能な場合のみスクロールバーを表示します。
		 8: eWGTIDFIG  0x9000  * Widget Figure
				eGEWFIG_LINE	図形タイプが線分であることを表します。
				eGEWFIG_POLYGON	図形タイプが多角形であることを表します。
				eGEWFIG_ELLIPSE	図形タイプが楕円であることを表します。
				eGEWFIG_ARC		図形タイプが円弧であることを表します。
	*/
}