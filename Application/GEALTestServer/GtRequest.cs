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
        eGEMSG_MOUSEDOWN = 0x0201,      // eGEMSG_MOUSEDOWN                             dwParam 上位16bit:垂直座標 下位16bit:水平座標
        eGEMSG_MOUSEUP = 0x0202,        // eGEMSG_MOUSEUP                               dwParam 上位16bit:垂直座標 下位16bit:水平座標
        eGEMSG_TIMER_UPDATE = 0x0203,   // eGEMSG_TIMER_UPDATE   wParam タイマーID
        eGEMSG_BUTTON_DOWN = 0x0204,    // eGEMSG_BUTTON_DOWN    wParam ウィジェット ID
        eGEMSG_BUTTON_CLICK = 0x0205,   // eGEMSG_BUTTON_CLICK   wParam ウィジェット ID
        eGEMSG_LISTITEM_DOWN = 0x0206,  // eGEMSG_LISTITEM_DOWN  wParam ウィジェット ID dwParam 上位16bit:行番号 下位16bit:アイテム番号
        eGEMSG_LISTBAR_DOWN = 0x0207,   // eGEMSG_LISTBAR_DOWN   wParam ウィジェット ID dwParam                  下位16bit:スクロールバーコマンド
        eGEMSG_MENUITEM_DOWN = 0x0208,  // eGEMSG_MENUITEM_DOWN  wParam ウィジェット ID dwParam メニューアイテム番号
        eGEMSG_USEREVENT = 0x0209,		// eGEMSG_USEREVENT      wParam イベントID(eGE_EVENT_ID) dwParam カスタム引数
		// WaitEvent   S<-C
		oteWaitEvent_AppInitialize = 0x0301,	// GE_VOID UGtAppInitialize(GE_VOID);
		oteWaitEvent_AppFinalize = 0x0302,		// GE_VOID UGtAppFinalize(GE_VOID);
		oteWaitEvent_StageEnter = 0x0303,		// GE_VOID UGtStageEnter(GE_ID eStageID);
		oteWaitEvent_StageExit = 0x0304,		// GE_VOID UGtStageExit(GE_ID eStageID);
		oteWaitEvent_LayerRender = 0x0305,		// GE_BOOL UGtLayerRender(HTARGET hTarget, GE_ID eLayerID);
		oteWaitEvent_WidgetRender = 0x0306, 	// GE_BOOL UGtWidgetRender(HTARGET hTarget, GE_POINT sOffset, GE_ID eWidgetID);
		// WaitMessage S<-C
		oteWaitMessage_MOUSEDOWN = 0x0401,		// eGEMSG_MOUSEDOWN                             dwParam 上位16bit:垂直座標 下位16bit:水平座標
		oteWaitMessage_MOUSEUP = 0x0402,		// eGEMSG_MOUSEUP                               dwParam 上位16bit:垂直座標 下位16bit:水平座標
		oteWaitMessage_TIMER_UPDATE = 0x0403,	// eGEMSG_TIMER_UPDATE   wParam タイマーID
		oteWaitMessage_BUTTON_DOWN = 0x0404,	// eGEMSG_BUTTON_DOWN    wParam ウィジェット ID
		oteWaitMessage_BUTTON_CLICK = 0x0405,	// eGEMSG_BUTTON_CLICK   wParam ウィジェット ID
		oteWaitMessage_LISTITEM_DOWN = 0x0406,	// eGEMSG_LISTITEM_DOWN  wParam ウィジェット ID dwParam 上位16bit:行番号 下位 16bit:アイテム番号
		oteWaitMessage_LISTBAR_DOWN = 0x0407,	// eGEMSG_LISTBAR_DOWN   wParam ウィジェット ID dwParam                  下位16bit:スクロールバーコマンド
		oteWaitMessage_MENUITEM_DOWN = 0x0408,	// eGEMSG_MENUITEM_DOWN  wParam ウィジェット ID dwParam メニューアイテム番号
		oteWaitMessage_USEREVENT = 0x0409,		// eGEMSG_USEREVENT      wParam イベントID(eGE_EVENT_ID) dwParam カスタム引数
		// WaitFlag    S<-C
		oteWaitFlag = 0x0501,					// Wati flag	ウィジェット ID mask  value
	};

    /// <summary>
    /// 対象型列挙型
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
    /// 対象を文字列に変換
    /// </summary>
    /// <param name="type">対象型</param>
    /// <param name="id">対象ID</param>
    /// <returns>文字列</returns>
	public delegate string TargetToString(TargetTypeEnum type, ushort id);

    /// <summary>
    /// 基本要求
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
        /// コンストラクタ
        /// </summary>
        /// <param name="operation">操作</param>
        /// <param name="targetType">対象型</param>
        /// <param name="targetId">対象ID</param>
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
    /// 引数要求
    /// </summary>
    public class RequestParameter : RequestBase {
		private uint parameter;
		public uint Parameter { get { return this.parameter; } set { this.parameter = value; } }

        public RequestParameter() : base()
        {
            this.parameter = 0;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="operation">操作</param>
        /// <param name="targetType">対象型</param>
        /// <param name="targetId">対象ID</param>
        /// <param name="parameter">引数</param>
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