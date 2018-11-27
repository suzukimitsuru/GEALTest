/*	GEAL Test Server: 要求操作
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
	eGEWTXT_STRING	文字列リソースを表示するテキストウィシ?ェットを表します。
	eGEWTXT_NUMBER	32bit 整数を表示するテキストウィシ?ェットを表します。
	eGEWTXT_MULTI	複数の文字列リソースを切り替えて表示するテキストウィシ?ェ ットを表します。
	eGEWTXT_DYNAMIC	動的な文字列を表示するテキストウィシ?ェットを表します。
#define eWGTIDBTN   0x4000  Widget Button
	eGEWBTN_PUSH	クリック可能なホ?タンウィシ?ェットを表します。
	eGEWBTN_RADIO	複数のホ?タンのいす?れか1つた?け選択することか?出来るホ? タンウィシ?ェットを表します。
	eGEWBTN_TOGGLE	クリックする毎に選択、通常/選択の状態か?切り替わるホ? タンウィシ?ェットを表します。
#define eWGTIDPICT  0x5000  Widget Picture
#define eWGTIDGAUGE 0x6000  Widget Gauge
	eGEGAUGE_ORI_HORIZONTAL	横向きを表します。
	eGEGAUGE_ORI_VERTICAL	縦向きを表します。
	横向きのケ?ーシ?の数値か?増加する方向
	eGEGAUGE_DIR_TORIGHT	左から右への方向を表します。
	eGEGAUGE_DIR_TOLEFT		右から左への方向を表します。
	縦向きのケ?ーシ?の数値か?増加する方向
	eGEGAUGE_DIR_TOTOP		下から上への方向を表します。
	eGEGAUGE_DIR_TOBOTTOM	上から下への方向を表します。
#define eWGTIDMENU  0x7000  Widget Menu
#define eWGTIDLIST  0x8000  Widget List
	eGEWLIST_RSCSTRG	リストのアイテムには文字列リソースの ID か?格納されます。
	eGEWLIST_DYNAMIC	リストのアイテムには動的文字列(GE_TCHAR)のアト?レスか? 格納されます。
	eGEWLIST_BARMODE_HIDE	スクロールハ?ーを表示しません。
	eGEWLIST_BARMODE_SHOW	スクロールハ?ーを常に表示します。
	eGEWLIST_BARMODE_AUTO	スクロール可能な場合のみスクロールハ?ーを表示します。
#define eWGTIDFIG  0x9000   Widget Fig
	eGEWFIG_LINE	図形タイフ?か?線分て?あることを表します。
	eGEWFIG_POLYGON	図形タイフ?か?多角形て?あることを表します。
	eGEWFIG_ELLIPSE	図形タイフ?か?楕円て?あることを表します。
	eGEWFIG_ARC		図形タイフ?か?円弧て?あることを表します。
*/
#ifndef _INC_GEAL_TEST_REQUEST_H
#define	_INC_GEAL_TEST_REQUEST_H

#ifdef __cplusplus
extern "C" {		/* Assume C declarations for C++ */
#endif				/* __cplusplus */

/*	<summary>要求構造体	</summary>
*/
typedef struct {
	int number;
	int target;
	int operation;
}	GT_REQUEST;

/*	<summary>要求の初期化</summary>
*/
extern void GtRequetInitialize(void);

/*	<summary>要求の蓄積</summary>
	<parameter name="receiveData">受信データ</parameter>
	<parameter name="receiveBytes">受信バイト数</parameter>
	<return>オーバーフロバイト数(0:なし)</return>
*/
extern int GtRequetPut(unsigned char* receiveData, int receiveBytes);

/*	<summary>要求の取り出し</summary>
	<parameter name="receiveData">要求データ</parameter>
	<parameter name="receiveBytes">最大要求バイト数</parameter>
	<return>要求バイト数(0:なし)</return>
*/
extern int GtRequetGet(unsigned char* request, int maxBytes);


#ifdef __cplusplus
}				/* Assume C declarations for C++ */
#endif  			/* __cplusplus */
#endif	_INC_GEAL_TEST_REQUEST_H
