# GEALTest
GEALのUIテストをPCとの通信で行います。
(GEAL is product of IT Access.,Co.Ltd.)

~~~
+-------------------+  +-------------------------+
| Target Hardware   |  | PC                      |
|+-----------------+|  |+-----------------------+|
|| Target Project  ||  || Test Project       C# ||
|| *.c             ||  |+-----------------------+|
||                 ||  |+-----------------------+|
||           +-----+|  || GEALTest Framework C# ||
|+-----------| *.c ||  |+-----------------------+|
|+-----------+     ||  |+-----------------------+|
|| GEALTest Server <----> GEALTest Client    C# ||
|+-----------------+|  |+-----------------------+|
+-------------------+  +-------------------------+
~~~

■ フォルダ構成

C:\GEAL\projects\SampleDev を基にします。

~~~
  root
  +-Application GEALアドオンモジュールのソース
  | +-SampleDev.c        サンプルアプリケーション
  | +-GealRsx*.*         GEAL Editor 出力リソースファイル
  | +-DynRscBitmap/      動的ビットマップ処理
  | +-Target*/           動作環境対応ソース
  | +-GealTest/          GEALTest Server
  +-Bitmaps              GEAL Editor 画像ファイル
  +-Fonts                GEAL Editor フォントファイル
  +-SampleDev.geproj     GEAL Editor プロジェクト
  +-SampleDev.gestrl     GEAL Editor 文字列定義
~~~

修正部分

Visual Studio プロジェクトのプロパティに以下を追加してください。
* インクルード： 「構成プロパティ」「C/C++」「全般」「追加のインクルード ディレクトリ」
*  -> ./GealTest;C:\Program Files (x86)\Windows Kits\10\Include\10.0.17134.0\um;
* ライブラリ： 「構成プロパティ」「リンカー」「入力」「追加の依存ファイル」
*  -> C:\Program Files (x86)\Windows Kits\10\Lib\10.0.17134.0\um\x86\WS2_32.Lib;

既存ソースの以下を変更してください。
~~~
SampleDev.c        サンプルアプリケーション
  UGxAppInitialize()  -> UGtAppInitialize()
  UGxAppProcess()     -> UGtAppProcess()
  UGxStageEnter()     -> UGtStageEnter()
  UGxStageExit()      -> UGtStageExit()
  UGxLayerRender()    -> UGtLayerRender()
  UGtWidgetRender()   -> UGtWidgetRender()
~~~
  GEALTest では、GEAL Timer API で ID 7 のタイマーを使います。
