# GEALTest
GEALのUIテストをPCとの通信で行います。

+------------------+ +-----------------+
| Target Hardware  | | PC              |
|+----------------+| |+---------------+|
|| Target Project || || Test Project
|+----------------+| |+---------------+|
+------------------+ +-----------------+


■ フォルダ構成
C:\GEAL\projects\SampleDev

  root
  +-Application GEALアドオンモジュールのソース
  | +-GealAppFramework.c/h GEALアドオンモジュールのコア
  | +-GealModuleFade.c/h GEALモジュール(フェード)
  | +-GealModuleScroll.c/h GEALモジュール(スクロール)
  +-Bitmaps GEALアドオンモジュールを使用する最小構成サンプル
  +-Fonts スクロールの最小構成サンプル
  +-SampleDev.geproj フェードの最小構成サンプル
  +-SampleDev.gestrl サンプルを組み合わせたデモ
