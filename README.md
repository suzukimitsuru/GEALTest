# GEALTest

GEALのUIテストをPCとの通信で行います。<br/>
(GEAL is product of IT Access.,Co.Ltd.)

~~~
+-------------------+  +-------------------------+
| Target Hardware   |  | PC                      |
|+-----------------+|  |+-----------------------+|
|| Target Project  ||  || Test Project       C# ||
|| *.c             ||  ||                       ||
||                 ||  ||                       ||
||           +-----+|  |+-----------------------+|
|+-----------| *.c ||  |+-----------------------+|
|+-----------+     ||  ||                       ||
|| GEALTest Server <----> GEALTest Client    C# ||
|+-----------------+|  |+-----------------------+|
+-------------------+  +-------------------------+
~~~

## 1.フォルダ構成

ここでは、サンプルアプリケーション(C:\GEAL\projects\SampleDev)に GEALTest Server を組み込みます。<br/>
対象ハードウェアは Windows 10 で WinSock2 を使っています。<br/>
対象ハードウェアに合せて GtUDPPort.c を書き換えてください。

~~~
root
  +-Application          サンプルアプリケーションのソース
  | +-SampleDev.c        サンプルアプリケーション
  | +-GealRsx*.*         GEAL Editor 出力リソースファイル
  | +-GealRsxEnum.cs     GealRsxEnum.h を C# 用に書き換えます。
  | +-DynRscBitmap/      動的ビットマップ処理
  | +-Target*/           動作環境対応ソース
  | +-GEALTestServer/    GEALTest Server
  |   +-GtEvent.c/h      ・イベント処理: GEAL Target API をフックしてUIテストを行います。
  |   +-GtOptions.h      ・動作オプション定義
  |   +-GtRequest.c/h    ・要求操作: GEALTest Client からの要求を解析します。
  |   +-GtPort.h         ・通信ポート定義
  |   +-GtUDPPort.c/h    ・UDPポート 
  +-Bitmaps              GEAL Editor 画像ファイル
  +-Fonts                GEAL Editor フォントファイル
  +-SampleDev.c          サンプルアプリケーション: GEALTest Server が動作する様に書き換えます。
  +-SampleDev.geproj     GEAL Editor プロジェクト
  +-SampleDev.gestrl     GEAL Editor 文字列定義
  +-GEALTestClient       GEALTest Client
  | +-Client.cs          GEALTest Client 本体
  | +-UDPPort.cs         UDP通信機能
  | +-GEALTestClient.csproj GEALTest Client プロジェクト
  +-TestProject          Test Project
  | +-Command.cs         コマンド処理
  | +-TestSample.cs      テストプログラムのサンプル
  | +-TestProject.csproj Test Project ファイル
  +-GEALTest.sln         GEAL Test ソリューション
~~~

## 2.使用リソース

GEALTest は GEAL Timer API で ID:7 のタイマーを使います。

## 3.Target Project 修正部分

### 3-1.Visual Studio プロジェクトのプロパティに以下を追加してください。

* インクルード: 「構成プロパティ」「C/C++」「全般」「追加のインクルード ディレクトリ」
  * ./GEALTestServer;C:\Program Files (x86)\Windows Kits\10\Include\10.0.17134.0\um;
* プリプロセッサ: 「構成プロパティ」「C/C++」「プリプロセッサ」「プリプロセッサの定義」
  * _GEAL_TEST_SERVER;
* ライブラリ: 「構成プロパティ」「リンカー」「入力」「追加の依存ファイル」
  * C:\Program Files (x86)\Windows Kits\10\Lib\10.0.17134.0\um\x86\WS2_32.Lib;

### 3-2.GEAL Target API を変更してください。

~~~
SampleDev.c        サンプルアプリケーション
  #include <GtEvent.h> を追加
  UGxAppInitialize()  -> UGtAppInitialize()
  UGxAppProcess()     -> UGtAppProcess()
  UGxStageEnter()     -> UGtStageEnter()
  UGxStageExit()      -> UGtStageExit()
  UGxLayerRender()    -> UGtLayerRender()
  UGxWidgetRender()   -> UGtWidgetRender()
~~~

### 3-3.動作オプションの指定を追加してください。

~~~
SampleDev.c        サンプルアプリケーション
  #include <GtOptions.h>
  /*  <summary>動作オプション設定</summary>
      <return>動作オプション</return>
  */
  GT_OPTIONS* UGtSetOptions(GE_VOID) {
      static GT_OPTIONS options;
      options.udp.WaitPort = *(int*)"Gt"; // 受信待ちポート番号(29767:0x7447)
      options.udp.ToHost = "127.0.0.1";   // 送信ホスト名
      options.udp.ToPort = *(int*)"GT";   // 送信ポート番号(21575:0x5447)
      options.timerId = 7;                // 通信タイマーID
      options.RecoardMode = GE_TRUE;      // 記録モード
      return &options;
  }
~~~

## 4.Test Project の準備

GEAL Editor 出力リソースファイルの GealRsxEnum.h をコピーして C# 用に書き換えて下さい。<br/>
Application/GealRsxEnum.cs

### 4-1.重複インクルード対策を名前空間に変更してください。

* 修正前

~~~
#ifndef _INC_GEAL_RSXENUM_H
#define _INC_GEAL_RSXENUM_H
  ：
#endif
~~~

* 修正後

~~~
namespace GealRsxEnum
{
  ：
}
~~~

### 4-2.列挙型を C# 用に変更してください。

* 修正前

~~~
typedef enum _eGE_BITMAP_ID { ... } eGE_BITMAP_ID;
typedef enum _eGE_FONT_ID { ... } eGE_FONT_ID;
typedef enum _eGE_STRING_ID { ... } eGE_STRING_ID;
typedef enum _eGE_LANGUAGE_ID { ... } eGE_LANGUAGE_ID;
typedef enum _eGE_EVENT_ID { ... } eGE_EVENT_ID;
typedef enum _eGE_BORDER_ID { ... } eGE_BORDER_ID;
typedef enum _eGE_STAGE_ID { ... } eGE_STAGE_ID;
typedef enum _eGE_LAYER_ID { ... } eGE_LAYER_ID;
~~~

* 修正後

~~~
enum eGE_BITMAP_ID { ... }
enum eGE_FONT_ID { ... }
enum eGE_STRING_ID { ... }
enum eGE_LANGUAGE_ID { ... }
enum eGE_EVENT_ID { ... }
enum eGE_BORDER_ID { ... }
enum eGE_STAGE_ID { ... }
enum eGE_LAYER_ID { ... }
~~~

### 4-3.Widget ID の定数定義を C# の列挙定義に変更してください。

* 修正前

~~~
typedef unsigned int eGE_WIDGET_ID;
#define eWGTIDRECT   0x2000  /* Widget Rect */
#define eWGTID_00_Rect_Bg eWGTIDRECT+0x0001
#define eWGTID_00_Rect1 eWGTIDRECT+0x0002
  ：
  ：
#define eWGTID_08_Poly_BgPage eWGTIDFIG+0x001E
~~~

* 修正後

~~~
enum eGE_WIDGET_ID
{
    eWGTIDRECT = 0x2000,  /* Widget Rect */
    eWGTID_00_Rect_Bg = eWGTIDRECT + 0x0001,
    eWGTID_00_Rect1 = eWGTIDRECT + 0x0002,
      ：
      ：
    eWGTID_08_Poly_BgPage = eWGTIDFIG + 0x001E,
}
~~~

## 5.使い方

### 5-1.テストプログラムの記録

* a) Test Project を記録モードで起動します。
  * TestProject -record TestScreen.cs
* b) Target Project を起動します。
* c) Target Project を操作します。
* d) 操作が終了したら、Test Project を Ctrl+C キーで終了します。
* e) 記録したテストプログラム TestScreen.cs を Test Project に追加します。

### 5-2.テストプログラムの確認

* 実行するメソッドを表示します。

~~~
C:\TestProject>TestProject -list
GEALTestClient 1.0.0.0  server localhost:29767  client localhost:21575

TestSample
TestSample.TestRun()

~~~

### 5-2.テストプログラムの実行

* a) Test Project を実行モードで起動します。
* * TestProject -run
* b) Target Project を起動します。
  
~~~
C:\TestProject>TestProject -run
GEALTestClient 1.0.0.0  server localhost:29767  client localhost:21575

TestSample Start
TestSample.TestRun() Start
OK ステージ000の開始待ち
op ステージ001へ移る
        eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_00_NextBtn)
OK ステージ001待ち
op ステージ002へ移る
        eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_01_NextBtn)
NG ステージ003は待っても来ない
        UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage003)
TestSample.TestRun() End 1/3
TestSample End 1/3

Result 1/3

~~~

## 6.バージョン

### 2019-04-10 1.0.0.0 最初のリリース