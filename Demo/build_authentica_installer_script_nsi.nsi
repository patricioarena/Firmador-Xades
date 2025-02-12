# Ruta base al directorio del script
!define BASE_PATH "${__FILEDIR__}"

# Rutas específicas
!define POSTINSTALL_PATH "${BASE_PATH}\PostInstallTask\bin\Release\net8.0-windows"
!define DEMO_RELEASE_PATH "${BASE_PATH}\Demo\bin\Release\net8.0-windows"

# Nombre del instalador
Outfile "Authentica_Installer.exe"

# Establecer el título de la ventana del instalador
Caption "Instalador de Authentica"

# Solicitar permisos de ejecución
RequestExecutionLevel admin  # Necesario para instalar en "Program Files (x86)"

# Establecer icono del instalador y de la ventana
Icon "${DEMO_RELEASE_PATH}\Resources\favicon.ico"

# Establecemos la ruta de instalación personalizada
InstallDir "C:\Program Files (x86)\Fiscalia de Estado\Authentica"

# Guardar la ruta de instalación en el registro (si es necesario)
InstallDirRegKey HKCU "Software\Authentica" "Install_Dir"

# Sección para preparar los archivos
Section "Preparar Archivos" SecPreparar

  # Copiar archivos desde PostInstallTask\bin\Release\net8.0-windows hacia Demo\bin\Release\net8.0-windows
  SetOutPath "${DEMO_RELEASE_PATH}"
  File /r "${POSTINSTALL_PATH}\*.*"

SectionEnd

# Sección de instalación principal
Section "Instalar" SecInstalar

  # Establecer la ruta de instalación
  SetOutPath $INSTDIR  # Los archivos se copiarán en la ruta de instalación

  # Copiar los archivos combinados desde Demo\bin\Release\net8.0-windows a la carpeta de instalación
  File /r "${DEMO_RELEASE_PATH}\*.*"

  # Crear un acceso directo en el escritorio con el icono
  CreateShortCut "$DESKTOP\Authentica.lnk" "$INSTDIR\Authentica.exe" "" "$INSTDIR\Resources\favicon.ico"

  # Copiar el desinstalador a la carpeta de instalación (uninstall.exe)
  WriteUninstaller "$INSTDIR\Uninstaller.exe"  # Esto crea el desinstalador

  # Registrar el desinstalador en la lista de programas
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica" "DisplayName" "Authentica"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica" "UninstallString" "$INSTDIR\Uninstaller.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica" "DisplayIcon" "$INSTDIR\Resources\favicon.ico"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica" "DisplayVersion" "1.0"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica" "Publisher" "Fiscalia de Estado"

SectionEnd

# Sección para ejecutar el PostInstallTask.exe al final
Section "PostInstall" SecPostInstall

  # Ejecutar el archivo PostInstallTask.exe
  ExecWait '"$INSTDIR\PostInstallTask.exe"'

SectionEnd

# Sección de desinstalación
Section "Uninstall" SecUninstall

  # Eliminar los archivos de la instalación
  RMDir /r "$INSTDIR"  # Eliminar la carpeta de instalación y todos sus archivos

  # Eliminar el acceso directo en el escritorio
  Delete "$DESKTOP\Authentica.lnk"

  # Eliminar el registro de la instalación
  DeleteRegKey HKCU "Software\Authentica"

  # Eliminar el icono de la instalación
  Delete "$INSTDIR\Resources\favicon.ico"

  # Eliminar el desinstalador (uninstall.exe)
  Delete "$INSTDIR\Uninstaller.exe"

  # Eliminar la entrada del registro de la lista de programas
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Authentica"

SectionEnd
