; Script generado por el Inno Setup Script Wizard.

#define MyAppName "Authentica"
#define MyAppVersion "3.0.3"
#define MyAppPublisher "Fiscalia de Estado."
#define MyAppURL "https://www2.fepba.gov.ar/"
#define MyAppExeName "Authentica.exe"
#define MyAppAssocName MyAppName + " File"
#define MyAppAssocExt ".myp"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt
#define BasePath "D:\Codigo\Firmador Xades - copia"
#define OutputDir "C:\Users\parena\Desktop\Inno"

[Setup]
AppId={{8D63249B-E05C-4A13-B0A2-BA8DCAE7ADDC}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
OutputDir={#OutputDir}
OutputBaseFilename={#MyAppName}_Installer
SetupIconFile={#BasePath}\Demo\Demo\bin\Release\net8.0-windows\Resources\favicon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={#BasePath}\Demo\Demo\bin\Release\net8.0-windows\Resources\favicon.ico

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#BasePath}\Demo\Demo\bin\Release\net8.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BasePath}\Demo\Demo\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#BasePath}\Demo\Demo\bin\Release\net8.0-windows\Resources\NDP481-x86-x64-AllOS-ENU.exe"; DestDir: "{tmp}"; Flags: ignoreversion
Source: "{#BasePath}\Demo\Demo\bin\Release\net8.0-windows\Resources\windowsdesktop-runtime-8.0.11-win-x86.exe"; DestDir: "{tmp}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\NDP481-x86-x64-AllOS-ENU.exe"; Parameters: "/quiet /norestart"; Description: "Instalando .NET Framework 4.8.1..."; Flags: runhidden waituntilterminated
Filename: "{tmp}\windowsdesktop-runtime-8.0.11-win-x86.exe"; Parameters: "/quiet /norestart"; Description: "Instalando .NET Desktop Runtime 8.0..."; Flags: runhidden waituntilterminated
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UserChoice: Integer;
begin
  // Verifica si estamos en el paso final de la instalación
  if CurStep = ssPostInstall then
  begin
    // Muestra el mensaje al usuario
    UserChoice := MsgBox('El equipo se reiniciará en 1 minuto. Puede optar por reiniciar ahora seleccionando "Aceptar".', 
                         mbInformation, MB_OKCancel);

    // Si el usuario presiona "Aceptar", reinicia de inmediato
    if UserChoice = IDOK then
    begin
      ShellExec('', 'shutdown', '/a', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); // Cancela el reinicio programado
      ShellExec('', 'shutdown', '/r /t 0', '', SW_HIDE, ewNoWait, ResultCode); // Reinicia de inmediato
    end
    else
    begin
      // Si el usuario no presiona "Aceptar", programa el reinicio en 60 segundos
      ShellExec('', 'shutdown', '/r /t 60', '', SW_HIDE, ewNoWait, ResultCode);
    end;
  end;
end;