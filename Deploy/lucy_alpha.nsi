; lucy_full.nsi
;
; This script create an installer for Lucy;
; The installation copy file to %programFiles%\Lucy

;-------------------------------

; The name of the installer
Name "Lucy"

; The file to write
OutFile "lucy_alpha.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Lucy

; Request application privileges for Windows Vista
RequestExecutionLevel admin

VIProductVersion "0.1.0.0"
LicenseData ..\LICENSE
;--------------------------------

; Pages

Page license
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles
;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File ..\deploy_tmp\*.*
  
  WriteUninstaller "lucy-uninst.exe"  
  
  CreateDirectory $SMPROGRAMS\Lucy
  CreateShortCut $SMPROGRAMS\Lucy\Lucy.lnk $INSTDIR\Lucy.exe 
  CreateShortCut "$SMPROGRAMS\Lucy\Unistall Lucy.lnk" $INSTDIR\lucy-uninst.exe

SectionEnd ; end the section

UninstallIcon "${NSISDIR}\Contrib\Graphics\Icons\nsis1-uninstall.ico"

Section "Uninstall"

  Delete "$SMPROGRAMS\Lucy\*.*"
  Delete $INSTDIR
  
SectionEnd