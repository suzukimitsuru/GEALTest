# Microsoft Developer Studio Project File - Name="SampleDev" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** �ҏW���Ȃ��ł������� **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=SampleDev - Win32 Debug
!MESSAGE ����͗L����Ҳ�̧�قł͂���܂���B ������ۼު�Ă�����ނ��邽�߂ɂ� NMAKE ���g�p���Ă��������B
!MESSAGE [Ҳ�̧�ق̴���߰�] ����ނ��g�p���Ď��s���Ă�������
!MESSAGE 
!MESSAGE NMAKE /f "SampleDev.mak".
!MESSAGE 
!MESSAGE NMAKE �̎��s���ɍ\�����w��ł��܂�
!MESSAGE ����� ײݏ��ϸۂ̐ݒ���`���܂��B��:
!MESSAGE 
!MESSAGE NMAKE /f "SampleDev.mak" CFG="SampleDev - Win32 Debug"
!MESSAGE 
!MESSAGE �I���\������� Ӱ��:
!MESSAGE 
!MESSAGE "SampleDev - Win32 Release" ("Win32 (x86) Application" �p)
!MESSAGE "SampleDev - Win32 Debug" ("Win32 (x86) Application" �p)
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "SampleDev - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /MT /W3 /GX /O2 /I "$(GEAL_PATH)/include" /I "./" /I "./TargetWin32" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x411 /d "NDEBUG"
# ADD RSC /l 0x411 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /machine:I386
# ADD LINK32 Comdlg32.lib kernel32.lib user32.lib gdi32.lib comdlg32.lib libGealEngine.lib /nologo /subsystem:windows /machine:I386 /libpath:"$(GEAL_PATH)\lib\Win32"

!ELSEIF  "$(CFG)" == "SampleDev - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /I "$(GEAL_PATH)/include" /I "./" /I "./TargetWin32" /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x411 /d "_DEBUG"
# ADD RSC /l 0x411 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib comdlg32.lib libGealEngine.lib /nologo /subsystem:windows /debug /machine:I386 /nodefaultlib:"LIBCMT" /pdbtype:sept /libpath:"$(GEAL_PATH)\lib\Win32"

!ENDIF 

# Begin Target

# Name "SampleDev - Win32 Release"
# Name "SampleDev - Win32 Debug"
# Begin Group "GealResources"

# PROP Default_Filter ""
# Begin Source File

SOURCE=.\GealRsxBitmap.c
# End Source File
# Begin Source File

SOURCE=.\GealRsxConfig.c
# End Source File
# Begin Source File

SOURCE=.\GealRsxConfig.h
# End Source File
# Begin Source File

SOURCE=.\GealRsxEnum.h
# End Source File
# Begin Source File

SOURCE=.\GealRsxFont.c
# End Source File
# Begin Source File

SOURCE=.\GealRsxString.c
# End Source File
# Begin Source File

SOURCE=.\GealRsxUniconv.c
# End Source File
# Begin Source File

SOURCE=.\GealRsxWidget.c
# End Source File
# End Group
# Begin Group "TargetWin32"

# PROP Default_Filter ""
# Begin Source File

SOURCE=.\TargetWin32\TargetDrawCLUT.c
# End Source File
# Begin Source File

SOURCE=.\TargetWin32\TargetDrawRGB.c
# End Source File
# Begin Source File

SOURCE=.\TargetWin32\TargetMain.c
# End Source File
# Begin Source File

SOURCE=.\TargetWin32\TargetSystem.c
# End Source File
# Begin Source File

SOURCE=.\TargetWin32\TargetTypes.h
# End Source File
# End Group
# Begin Group "GealAPI"

# PROP Default_Filter ""
# Begin Source File

SOURCE=..\..\..\include\GealAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealCommon.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealConfigAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealFigureAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealLayerAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealTargetAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealTimerAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealTypes.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealUtilAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWButtonAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWFigureAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWGaugeAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWidgetAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWListAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWMenuAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWPictureAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWRectAPI.h
# End Source File
# Begin Source File

SOURCE=..\..\..\include\GealWTextAPI.h
# End Source File
# End Group
# Begin Source File

SOURCE=.\SampleDev.c
# End Source File
# End Target
# End Project
