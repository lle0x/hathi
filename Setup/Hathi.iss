; Copyright (c)2009 Hathi Project < http://hathi.sourceforge.net >
;
; This file is part of Hathi Project
; Hathi Developers Team:
; andrewdev, beckman16, biskvit, elnomade_devel, ershyams, grefly, jpierce420, 
; knocte, kshah05, manudenfer, palutz, ramone_hamilton, soudamini, writetogupta
; 
; Hathi is a fork of Lphant version 1.0 GPL
; Lphant Team
; Juanjo, 70n1, toertchn, FeuerFrei, mimontyf, finrold, jicxicmic, bladmorv, 
; andrerib, arcange|, montagu, wins, RangO, FAV, roytam1, Jesse
; 
; This program is free software; you can redistribute it and/or
; modify it under the terms of the GNU General Public License
; as published by the Free Software Foundation; either
; version 2 of the License, or (at your option) any later version.
; 
; This program is distributed in the hope that it will be useful,
; but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
; GNU General Public License for more details.
; 
; You should have received a copy of the GNU General Public License
; along with this program; if not, write to the Free Software
; Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.


[Setup]
;Never change the value AppName=lphant : read AppId in InnoSetup help
AppName=Hathi

;----------------- TO UPDATE FOR A NEW VERSION ----------------------
AppVerName=Hathi v0.1.2.0
AppVersion=v0.1.2.0
VersionInfoVersion=0.1.2.0
;--------------------------------------------------------------------

AppPublisher=Hathi Project
AppPublisherURL=http://hathi.sourceforge.net
AppSupportURL=http://hathi.sourceforge.net
AppUpdatesURL=http://hathi.sourceforge.net
;AppMutex={lphant-DD14EC11-CB90-4956-B8F4-F5D6D708DC33}
DefaultDirName={pf}\Hathi
DefaultGroupName=Hathi
DisableProgramGroupPage=true
AllowNoIcons=true
LicenseFile=..\license.txt
AppCopyright=Copyright © Hathi Project
ShowLanguageDialog=auto
InfoBeforeFile=..\readme.txt
Compression=lzma
SolidCompression=true
UninstallDisplayIcon={app}\Hathi.UI.Winform.exe
VersionInfoCompany=Hathi Project
VersionInfoDescription=Hathi : P2P client
OutputDir=..\Setup

[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: ..\Source\UI\Winform\bin\Release\Hathi.UI.Winform.exe; DestDir: {app}; Flags: ignoreversion
Source: ..\Source\UI\Winform\bin\Release\Hathi.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Source\UI\Winform\bin\Release\Hathi.UI.Winform.Controls.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Source\UI\Winform\bin\Release\WeifenLuo.WinFormsUI.Docking.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Source\UI\Winform\bin\Release\ICSharpCode.SharpZipLib.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Source\UI\Winform\bin\Release\server.met; DestDir: {app}; Flags: ignoreversion skipifsourcedoesntexist
;Source: ..\Source\Client\bin\Release\ipfilter.dat; DestDir: {app}; Flags: ignoreversion skipifsourcedoesntexist
Source: ..\Source\UI\Winform\bin\Release\webSearchs.xml; DestDir: {app}; Flags: ignoreversion
; Check: BackupFile({app}\webSearchs.xml)
Source: ..\Source\UI\Winform\bin\Release\Language\*; DestDir: {app}\Language; Flags: ignoreversion recursesubdirs
Source: ..\Source\UI\Winform\bin\Release\Skins\*; DestDir: {app}\Skins; Flags: ignoreversion recursesubdirs
Source: ..\changelog.txt; DestDir: {app}; Flags: ignoreversion isreadme
Source: ..\readme.txt; DestDir: {app}; Flags: ignoreversion
Source: ..\license.txt; DestDir: {app}; Flags: ignoreversion

;Used only for custom messages
Source: Language\Custom-*; DestDir: {tmp}; Flags: dontcopy

[INI]
Filename: {app}\Hathi.url; Section: InternetShortcut; Key: URL; String: http://hathi.sourceforge.net
Filename: {app}\dotnet.url; Section: InternetShortcut; Key: URL; String: http://www.microsoft.com/downloads/details.aspx?FamilyID=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5&DisplayLang=en

[Icons]
Name: {group}\Hathi; Filename: {app}\Hathi.UI.Winform.exe
Name: {group}\Readme; Filename: {app}\readme.txt
; NOTE: The following entry contains an English phrase ("on the Web"). You are free to translate it into another language if required.
Name: {group}\Hathi on the Web; Filename: {app}\Hathi.url
Name: {group}\Install .NET 2.0 Framework; Filename: {app}\dotnet.url
; NOTE: The following entry contains an English phrase ("Uninstall"). You are free to translate it into another language if required.
Name: {group}\Uninstall Hathi; Filename: {uninstallexe}
Name: {userdesktop}\Hathi; Filename: {app}\Hathi.UI.Winform.exe; Tasks: desktopicon

[Run]
; NOTE: The following entry contains an English phrase ("Launch"). You are free to translate it into another language if required.
Filename: {app}\Hathi.UI.Winform.exe; Description: {code:GetSectionMessages|LaunchHathi}; Flags: nowait postinstall skipifsilent unchecked

[UninstallDelete]
Type: files; Name: {app}\Hathi.url
Type: files; Name: {app}\dotnet.url

[Tasks]
; NOTE: The following entry contains English phrases ("Create a desktop icon" and "Additional icons"). You are free to translate them into another language if required.
Name: desktopicon; Description: {code:GetSectionMessages|CreateDesktopIcon}; GroupDescription: Additional icons:; Flags: unchecked

[Languages]
Name: en_US; MessagesFile: compiler:Default.isl
Name: es_ES; MessagesFile: Language\Spanish.isl
Name: fr_FR; MessagesFile: Language\French.isl
Name: de_DE; MessagesFile: Language\German.isl

[_ISTool]
UseAbsolutePaths=false

[Code]
var
	CustomMessagesFile : String;
	DefaultMessagesFile : String;

function GetValue(Section, Msg : String) : String;
var
	Value: String;
begin
	Value:=GetIniString(Section,Msg,'????',CustomMessagesFile);
	if Value='????' then
		Value:=GetIniString(Section,Msg,'????',DefaultMessagesFile);
	Result:=Value;
end;

function GetSectionMessages(Msg : String) : String;
begin
	Result:=GetValue('Messages', Msg);
end;

function BackupFile(FileName:String): Boolean;
begin
	if FileExists(FileName) then
		FileCopy(FileName,FileName+'.backup',False);
	Result:=True;
end;


function CheckdotNet(): Boolean;
var
  Dummy: Integer;
  LanguageCode: String;
begin
	LanguageCode := Copy(ActiveLanguage(),0,2);
	if not RegValueExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v2.0','50727') then begin
		MsgBox(GetValue('Messages','WarningDotNet1')+#13#13+GetValue('Messages','WarningDotNet2'), mbError, MB_OK);
		ShellExec('open', 'http://www.microsoft.com/downloads/details.aspx?FamilyID=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5&DisplayLang='+LanguageCode, '', '', SW_SHOWNORMAL, ewNoWait, Dummy);
		Result:=False;
	end else
		Result:=True;
end;

procedure URLLabelOnClick(Sender: TObject);
var
  Dummy: Integer;
begin
  ShellExec('open', 'http://hathi.sourceforge.net', '', '', SW_SHOWNORMAL, ewNoWait, Dummy);
end;

procedure InitializeWizard();
var
  URLLabel: TNewStaticText;
begin
  URLLabel := TNewStaticText.Create(WizardForm);
  URLLabel.Top := 333;
  URLLabel.Left := 20;
  URLLabel.Caption := 'hathi.sourceforge.net';
  URLLabel.Font.Style := URLLabel.Font.Style + [fsUnderLine];
  URLLabel.Font.Color := clBlue;
  URLLabel.Cursor := crHand;
  URLLabel.OnClick := @URLLabelOnClick;
  URLLabel.Parent := WizardForm;
end;

function InitializeSetup() : Boolean;
var
	DefaultFile:String;
begin
	DefaultFile:='Custom-en_US.txt';
	CustomMessagesFile:='Custom-'+ActiveLanguage()+'.txt';

	ExtractTemporaryFile(CustomMessagesFile);
	ExtractTemporaryFile(DefaultFile);

	CustomMessagesFile:=ExpandConstant('{tmp}\'+CustomMessagesFile);
	DefaultMessagesFile:=ExpandConstant('{tmp}\'+DefaultFile);

	Result:=True;
end;

function NextButtonClick(CurPage: Integer): Boolean;
// return False to surpress the click on the Next button
// return False to abort Setup
begin
	if CurPage=wpWelcome then
		CheckdotNet();
	Result:=true;
end;
