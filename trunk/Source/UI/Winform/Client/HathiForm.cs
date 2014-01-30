#region Copyright (c) 2013 Hathi Project < http://hathi.sourceforge.net >
/*
* This file is part of Hathi Project
* Hathi Developers Team:
* andrewdev, beckman16, biskvit, elnomade_devel, ershyams, grefly, jpierce420,
* knocte, kshah05, manudenfer, palutz, ramone_hamilton, soudamini, writetogupta
*
* Hathi is a fork of Lphant Version 1.0 GPL
* Lphant Team
* Juanjo, 70n1, toertchn, FeuerFrei, mimontyf, finrold, jicxicmic, bladmorv,
* andrerib, arcange|, montagu, wins, RangO, FAV, roytam1, Jesse
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either
* version 2 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;
using Hathi.eDonkey;
using Hathi.UI.Winform.Controls;
using Hathi.Classes;
using Hathi.eDonkey.InterfaceGateway;


namespace Hathi.UI.Winform
{
/// <summary>
/// Summary description for HathiForm.
/// </summary>
public class HathiForm : System.Windows.Forms.Form
{
    private Hathi.UI.Winform.Controls.HathiToggleButton btnDownloads;
    private System.Windows.Forms.Panel panelExtensible;
    private System.Windows.Forms.Panel panelButtons;
    private Hathi.UI.Winform.Controls.HathiToggleButton btnServers;
    private Hathi.UI.Winform.Controls.HathiToggleButton btnSearch;
    private Hathi.UI.Winform.Controls.HathiToggleButton btnUploads;
    private Hathi.UI.Winform.Controls.HathiButton buttonOptions;
    private System.Windows.Forms.ImageList imageListOptions;
    private System.Windows.Forms.ImageList imageListHelp;
    private System.ComponentModel.IContainer components;

    private CkernelGateway krnGateway;
    private System.Windows.Forms.Panel panelContent;

    private FormDownloads FDownloads;
    private FormUploads FUploads;
    private FormServers FServers;
    private FormSearch FSearchs;
    public FormFriends FFriends;
    private System.Windows.Forms.ToolTip toolTipMain;
    private System.Windows.Forms.Panel panel1;
    private Hathi.UI.Winform.Controls.HathiButton HathiButtonConnect;
    private System.Windows.Forms.ImageList imageListServerDiscon;
    private System.Windows.Forms.ImageList imageListServerCon;
    private Hathi.UI.Winform.Controls.HathiGraphic graphics;
    private Form activeForm;

    private bool m_KernelLoaded;
    private string m_link;
    private InterfacePreferences m_Preferences;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.ImageList imageListIcons;
    TaskbarNotifier taskbarNotifier1;

    private CInterfaceGateway m_remoteGateway;
    private Mutex m_Mutex; //used to mantain a reference to the mutex to ensure it is not released
    private Icon m_ico;
    private FormWindowState m_PreviousState;
    private Hathi.UI.Winform.Controls.HathiToggleButton btnFriends;
    private System.Windows.Forms.Label labelSessionUP;
    private System.Windows.Forms.Label labelSessionDO;
    private string m_LastChatMessage;
    public static Classes.Globalization Globalization;
    public static CSkin Skin;
    public static CSystemIconsList SystemIconsList;
    internal static Config preferences;
    private System.Windows.Forms.Panel panelGraphic;
    private System.Windows.Forms.Panel panelToolbarRight;
    private System.Windows.Forms.Panel panelStatusBar;
    private System.Windows.Forms.Panel panelStatusBarLeft;
    private System.Windows.Forms.Panel panelStatusBarRight;
    private System.Windows.Forms.Panel panelStatusBarExtensible;
    private System.Windows.Forms.Label labelStatusServer;
    private System.Windows.Forms.Label labelStatusAvgDOSpeed;
    private System.Windows.Forms.Label labelStatusDOSpeed;
    private System.Windows.Forms.Label labelStatusUPSpeed;
    private System.Windows.Forms.Label labelStatusMsg;

    private Graphics m_gr;
    private System.Windows.Forms.Button buttonAboutMenu;
    private System.Windows.Forms.ContextMenu contextMenuAbout;
    private System.Windows.Forms.Button buttonAbout;
    private System.Windows.Forms.MenuItem menuItemWeb;
    private System.Windows.Forms.MenuItem menuItemSupport;
    private System.Windows.Forms.ContextMenu contextMenuNotifyIcon;
    private System.Windows.Forms.MenuItem menuItemNotifyIconOpen;
    private System.Windows.Forms.MenuItem menuItemNotifyIconQuit;
    private System.Windows.Forms.MenuItem menuItemNotifyIconOptions;
    private System.Windows.Forms.MenuItem menuItemSkins;

    private byte m_LastIcon;

    public delegate void TextEvent(string text);

    [STAThread]
    static void Main(string[] args)
    {
        CMain doStartJob=new CMain(args);
    }

    public HathiForm(string elink, Mutex in_Mutex)
    {
        m_Mutex=in_Mutex;
        m_link=elink;
        m_remoteGateway=null;
        InitializeComponent();
        Initialize();
    }
    public HathiForm(string elink,Mutex in_Mutex,CInterfaceGateway remoteGateway)
    {
        m_Mutex=in_Mutex;
        m_link=elink;
        m_remoteGateway=remoteGateway;
        InitializeComponent();
        Initialize();
    }
    protected override void WndProc(ref Message m)
    {
        if (m.Msg==Win32.WM_COPYDATA)
        {
            Win32.COPYDATASTRUCT stMsg=new Win32.COPYDATASTRUCT();
            stMsg= (Win32.COPYDATASTRUCT)m.GetLParam(stMsg.GetType());
            byte[] lpStr=new byte[stMsg.cbData];
            IntPtr lpB=new IntPtr(stMsg.lpData);
            Marshal.Copy(lpB,lpStr,0,stMsg.cbData);
            string parameter=Encoding.Default.GetString(lpStr);
            if ((parameter!=null)&&(parameter!="")) krnGateway.DownloadElink(parameter,preferences.GetBool("eLinkFromBrowserStopped",false));
            if (notifyIcon1.Visible==false)
            {
                Activate();
            }
        }
        base.WndProc(ref m);
    }

    private void Initialize()
    {
        m_PreviousState=FormWindowState.Normal;
        SystemIconsList=new CSystemIconsList();
        Skin=new CSkin();
        m_LastIcon=10;
        preferences = new Config(Application.StartupPath, "configInterface.xml", "0.01", "HathiInterface");
        preferences.PropertyChanged += new Config.PropertyEventHandler(m_OnPropertyChanged);
        preferences.PropertyDefaults += new Config.PropertyDefaultHandler(OnGetDefaultProperty);
        preferences.LoadProperties();
        Globalization=new Classes.Globalization(Application.StartupPath+Path.DirectorySeparatorChar+"language", "interface_", "xml",preferences.GetString("Language"));
        m_Globalize();
        m_KernelLoaded=false;
        toolTipMain.SetToolTip(HathiButtonConnect,Globalization["LBL_CONNDISC"]);
        toolTipMain.SetToolTip(buttonOptions,Globalization["LBL_OPTIONS"]);
        taskbarNotifier1=new TaskbarNotifier();
        taskbarNotifier1.SetBackgroundBitmap(new Bitmap(GetType(),"Client.Resources.Classic.PopUpSkin.bmp"),Color.FromArgb(255,0,0));
        taskbarNotifier1.SetCloseBitmap(new Bitmap(typeof(HathiForm),"Client.Resources.Classic.close.bmp"),Color.FromArgb(255,0,255),new Point(127,8));
        taskbarNotifier1.TitleRectangle=new Rectangle(50,65,100,70);
        taskbarNotifier1.ContentRectangle=new Rectangle(8,75,133,100);
        taskbarNotifier1.NormalContentColor = Color.White;
        //taskbarNotifier1.TitleClick+=new EventHandler(TitleClick);
        taskbarNotifier1.ContentClick+=new EventHandler(m_BallonClicked);
        //taskbarNotifier1.CloseClick+=new EventHandler(CloseClick);
        Skin.CurrentSkin=preferences.GetString("Skin","default");
        m_ApplySkin();
        graphics.AutoGraphicScale=preferences.GetBool("AutoGraphicScale",false);
        graphics.GraphicScale=preferences.GetInt("GraphicScale",1);
        FDownloads=new FormDownloads();
        FDownloads.TopLevel=false;
        panelContent.Controls.Add(FDownloads);
        FDownloads.Dock = DockStyle.Fill;
        FDownloads.ApplySkin();
        FUploads=new FormUploads();
        FUploads.TopLevel=false;
        FUploads.Dock = DockStyle.Fill;
        panelContent.Controls.Add(FUploads);
        FUploads.Dock = DockStyle.Fill;
        FUploads.ApplySkin();
        FServers=new FormServers();
        FServers.TopLevel=false;
        FServers.Dock = DockStyle.Fill;
        panelContent.Controls.Add(FServers);
        FServers.Dock = DockStyle.Fill;
        FServers.labelmsg=this.labelStatusMsg;
        FServers.ApplySkin();
        FSearchs=new FormSearch();
        FSearchs.TopLevel=false;
        FSearchs.Dock = DockStyle.Fill;
        panelContent.Controls.Add(FSearchs);
        FSearchs.Dock = DockStyle.Fill;
        FSearchs.ApplySkin();
        FFriends=new FormFriends();
        FFriends.TopLevel=false;
        FFriends.Dock = DockStyle.Fill;
        panelContent.Controls.Add(FFriends);
        FFriends.Dock = DockStyle.Fill;
        FFriends.ApplySkin();
    }

    private void m_ApplySkin()
    {
        BackColor=Skin.GetColor("defaultBackColor");
        taskbarNotifier1.SetBackgroundBitmap(Skin.GetBitmap("PopUpSkin"),Color.FromArgb(255,0,0));
        //backgrounds
        panelButtons.BackgroundImage=Skin.GetBitmap("ToolBarButtons");
        panelGraphic.BackgroundImage=Skin.GetBitmap("SmallGraphic");
        panelExtensible.BackgroundImage=Skin.GetBitmap("ToolBarExtensible");
        panelToolbarRight.BackgroundImage=Skin.GetBitmap("ToolBarRight");
        panelStatusBarLeft.BackgroundImage=Skin.GetBitmap("StatusBarLeft");
        panelStatusBarExtensible.BackgroundImage=Skin.GetBitmap("StatusBarExtensible");
        panelStatusBarRight.BackgroundImage=Skin.GetBitmap("StatusBarRight");
        //buttons
        btnSearch.Image=Skin.GetBitmap("BtnSearch");
        btnServers.Image=Skin.GetBitmap("BtnServers");
        btnDownloads.Image=Skin.GetBitmap("BtnDownload");
        btnUploads.Image=Skin.GetBitmap("BtnUpload");
        btnFriends.Image=Skin.GetBitmap("BtnFriends");
        btnSearch.InactiveBackColor=Skin.GetColor("ToolBarButtonInactiveBackColor");
        btnSearch.MouseUpBackColor=Skin.GetColor("ToolBarButtonHoverColor");
        btnSearch.SelectedBackColor=Skin.GetColor("ToolBarButtonSelectedBackColor");
        btnSearch.TextColor=Skin.GetColor("ToolBarButtonTextColor");
        btnServers.InactiveBackColor=Skin.GetColor("ToolBarButtonInactiveBackColor");
        btnServers.MouseUpBackColor=Skin.GetColor("ToolBarButtonHoverColor");
        btnServers.SelectedBackColor=Skin.GetColor("ToolBarButtonSelectedBackColor");
        btnServers.TextColor=Skin.GetColor("ToolBarButtonTextColor");
        btnFriends.InactiveBackColor=Skin.GetColor("ToolBarButtonInactiveBackColor");
        btnFriends.MouseUpBackColor=Skin.GetColor("ToolBarButtonHoverColor");
        btnFriends.SelectedBackColor=Skin.GetColor("ToolBarButtonSelectedBackColor");
        btnFriends.TextColor=Skin.GetColor("ToolBarButtonTextColor");
        btnUploads.InactiveBackColor=Skin.GetColor("ToolBarButtonInactiveBackColor");
        btnUploads.MouseUpBackColor=Skin.GetColor("ToolBarButtonHoverColor");
        btnUploads.SelectedBackColor=Skin.GetColor("ToolBarButtonSelectedBackColor");
        btnUploads.TextColor=Skin.GetColor("ToolBarButtonTextColor");
        btnDownloads.InactiveBackColor=Skin.GetColor("ToolBarButtonInactiveBackColor");
        btnDownloads.MouseUpBackColor=Skin.GetColor("ToolBarButtonHoverColor");
        btnDownloads.SelectedBackColor=Skin.GetColor("ToolBarButtonSelectedBackColor");
        btnDownloads.TextColor=Skin.GetColor("ToolBarButtonTextColor");
        imageListServerCon.Images.Clear();
        imageListServerCon.ImageSize=Skin.GetBitmap("BtnConnect").Size;
        imageListServerCon.Images.Add(Skin.GetBitmap("BtnConnect"));
        imageListServerCon.Images.Add(Skin.GetBitmap("BtnConnectMousePressed"));
        imageListServerCon.Images.Add(Skin.GetBitmap("BtnConnectMouseUp"));
        imageListServerDiscon.Images.Clear();
        imageListServerDiscon.ImageSize=Skin.GetBitmap("BtnDisconnect").Size;
        imageListServerDiscon.Images.Add(Skin.GetBitmap("BtnDisconnect"));
        imageListServerDiscon.Images.Add(Skin.GetBitmap("BtnDisconnectMousePressed"));
        imageListServerDiscon.Images.Add(Skin.GetBitmap("BtnDisconnectMouseUp"));
        imageListOptions.Images.Clear();
        imageListOptions.ImageSize=Skin.GetBitmap("BtnOptions").Size;
        imageListOptions.Images.Add(Skin.GetBitmap("BtnOptions"));
        imageListOptions.Images.Add(Skin.GetBitmap("BtnOptionsMousePressed"));
        imageListOptions.Images.Add(Skin.GetBitmap("BtnOptionsMouseUp"));
        //labels
        labelStatusMsg.ForeColor=Skin.GetColor("StatusBarMessageTextColor");
        labelStatusDOSpeed.ForeColor=Skin.GetColor("StatusBarDownSpeedTextColor");
        labelSessionDO.ForeColor=Skin.GetColor("StatusBarDownSpeedTextColor");
        labelSessionUP.ForeColor=Skin.GetColor("StatusBarUpSpeedTextColor");
        labelStatusUPSpeed.ForeColor=Skin.GetColor("StatusBarUpSpeedTextColor");
        labelStatusAvgDOSpeed.ForeColor=Skin.GetColor("StatusBarAvgTextColor");
        labelStatusServer.ForeColor=Skin.GetColor("StatusBarServerTextColor");
        //graphic
        graphics.GridColor=Skin.GetColor("GraphicGridColor");
        graphics.DownColor=Skin.GetColor("GraphicDownLineColor");
        graphics.UpColor=Skin.GetColor("GraphicUpLineColor");
        int x=0;
        int y=0;
        string ID;
        if (panelGraphic.Height<100)
            ID="SmallGraphic";
        else
            ID="BigGraphic";
        Skin.GetSize(ID+"Size",ref x,ref y);
        graphics.Height=y;
        graphics.Width=x;
        Skin.GetPosition(ID+"Position",ref x, ref y);
        graphics.Left=x;
        graphics.Top=y;
        Skin.GetSize("GraphicLinesWidth",ref x, ref y);
        graphics.GraphicLinesWidth=x;
        //positions
        Skin.GetPosition("BtnOptionsPosition",ref x,ref y);
        buttonOptions.Left=x;
        buttonOptions.Top=y;
        Skin.GetPosition("BtnConnectionPosition",ref x,ref y);
        HathiButtonConnect.Left=x;
        HathiButtonConnect.Top=y;
        //sizes
        Skin.GetSize("BtnOptionsSize",ref x,ref y);
        buttonOptions.Width=x;
        buttonOptions.Height=y;
        imageListOptions.ImageSize=imageListOptions.Images[0].Size;
        Skin.GetSize("BtnConnectionSize",ref x,ref y);
        HathiButtonConnect.Width=x;
        HathiButtonConnect.Height=y;
        imageListServerCon.ImageSize=imageListServerCon.Images[0].Size;
        imageListServerDiscon.ImageSize=imageListServerDiscon.Images[0].Size;
        buttonAbout.BackColor=Skin.GetColor("ButtonBackColor");
        buttonAbout.ForeColor=Skin.GetColor("ButtonForeColor");
        buttonAboutMenu.BackColor=Skin.GetColor("ButtonBackColor");
        buttonAboutMenu.ForeColor=Skin.GetColor("ButtonForeColor");
        if (contextMenuAbout.MenuItems.Count>3)
        {
            contextMenuAbout.MenuItems.RemoveAt(contextMenuAbout.MenuItems.Count-1);
            contextMenuAbout.MenuItems.RemoveAt(contextMenuAbout.MenuItems.Count-1);
        }
        if ((Skin.LinkText.Length>0)&&(Skin.LinkUrl.Length>0))
        {
            contextMenuAbout.MenuItems.Add("-");
            contextMenuAbout.MenuItems.Add(Skin.LinkText,new System.EventHandler(this.menuItemAbout_click));
        }
    }
    internal void OnGetDefaultProperty(object sender, PropertyDefaultArgs e)
    {
        switch (e.Key)
        {
        case "MinimizeToTray":
            e.Value=true;
            break;
        case "TaskBarNotifier":
            e.Value=true;
            break;
        case "PreviewPlayer":
            e.Value="";
            break;
        case "StartupConnectionSelect":
            e.Value=false;
            break;
        case "Language":
            e.Value=CultureInfo.CurrentCulture.Name;
            break;
        case "eLinkFromBrowserStopped":
            e.Value=false;
            break;
        case "FilterShowCount":
            e.Value=true;
            break;
        case "FilterShowTotalSizes":
            e.Value=true;
            break;
        case "AutoSort":
            e.Value=false;
            break;
        }
    }

    private void Connect()
    {
        FServers.Connect(krnGateway);
        FDownloads.Connect(krnGateway);
        FUploads.Connect(krnGateway);
        FSearchs.Connect(krnGateway);
        FFriends.Connect(krnGateway);
        m_Preferences=krnGateway.GetConfig();
        graphics.SetMinValue(0);
        if (m_Preferences.maxDownloadRate>m_Preferences.maxUploadRate)
            graphics.SetMaxValue((int)m_Preferences.maxDownloadRate);
        else
            graphics.SetMaxValue((int)m_Preferences.maxUploadRate);
        activeForm=FDownloads;
        krnGateway.OnRefreshStatus+=new RefreshEvent(m_InvokeOnRefreshStatus);
        krnGateway.OnNewChatMessage+=new SourceEvent(m_InvokeOnNewChatMessage);
        krnGateway.OnStartChatSession+=new ClientEvent(m_InvokeOnStartChatSession);
        krnGateway.OnAddingFriend+=new ClientEvent(m_InvokeOnAddingFriend);
        krnGateway.OnDeleteFriend+=new ClientEvent(m_InvokeOnDeleteFriend);
    }
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HathiForm));
        this.panelStatusBar = new System.Windows.Forms.Panel();
        this.panelStatusBarExtensible = new System.Windows.Forms.Panel();
        this.panelStatusBarRight = new System.Windows.Forms.Panel();
        this.panelStatusBarLeft = new System.Windows.Forms.Panel();
        this.labelStatusMsg = new System.Windows.Forms.Label();
        this.labelStatusAvgDOSpeed = new System.Windows.Forms.Label();
        this.labelStatusDOSpeed = new System.Windows.Forms.Label();
        this.labelStatusUPSpeed = new System.Windows.Forms.Label();
        this.labelStatusServer = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panelContent = new System.Windows.Forms.Panel();
        this.panelExtensible = new System.Windows.Forms.Panel();
        this.buttonAboutMenu = new System.Windows.Forms.Button();
        this.buttonAbout = new System.Windows.Forms.Button();
        this.labelSessionDO = new System.Windows.Forms.Label();
        this.labelSessionUP = new System.Windows.Forms.Label();
        this.graphics = new Hathi.UI.Winform.Controls.HathiGraphic(this.components);
        this.panelToolbarRight = new System.Windows.Forms.Panel();
        this.panelButtons = new System.Windows.Forms.Panel();
        this.HathiButtonConnect = new Hathi.UI.Winform.Controls.HathiButton(this.components);
        this.imageListServerCon = new System.Windows.Forms.ImageList(this.components);
        this.buttonOptions = new Hathi.UI.Winform.Controls.HathiButton(this.components);
        this.imageListOptions = new System.Windows.Forms.ImageList(this.components);
        this.btnFriends = new Hathi.UI.Winform.Controls.HathiToggleButton(this.components);
        this.btnServers = new Hathi.UI.Winform.Controls.HathiToggleButton(this.components);
        this.btnSearch = new Hathi.UI.Winform.Controls.HathiToggleButton(this.components);
        this.btnUploads = new Hathi.UI.Winform.Controls.HathiToggleButton(this.components);
        this.btnDownloads = new Hathi.UI.Winform.Controls.HathiToggleButton(this.components);
        this.imageListServerDiscon = new System.Windows.Forms.ImageList(this.components);
        this.imageListHelp = new System.Windows.Forms.ImageList(this.components);
        this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
        this.panelGraphic = new System.Windows.Forms.Panel();
        this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
        this.contextMenuNotifyIcon = new System.Windows.Forms.ContextMenu();
        this.menuItemNotifyIconOpen = new System.Windows.Forms.MenuItem();
        this.menuItemNotifyIconQuit = new System.Windows.Forms.MenuItem();
        this.menuItemNotifyIconOptions = new System.Windows.Forms.MenuItem();
        this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
        this.contextMenuAbout = new System.Windows.Forms.ContextMenu();
        this.menuItemWeb = new System.Windows.Forms.MenuItem();
        this.menuItemSupport = new System.Windows.Forms.MenuItem();
        this.menuItemSkins = new System.Windows.Forms.MenuItem();
        this.panelStatusBar.SuspendLayout();
        this.panelStatusBarLeft.SuspendLayout();
        this.panelExtensible.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.graphics)).BeginInit();
        this.panelButtons.SuspendLayout();
        this.panelGraphic.SuspendLayout();
        this.SuspendLayout();
        // 
        // panelStatusBar
        // 
        this.panelStatusBar.Controls.Add(this.panelStatusBarExtensible);
        this.panelStatusBar.Controls.Add(this.panelStatusBarRight);
        this.panelStatusBar.Controls.Add(this.panelStatusBarLeft);
        this.panelStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelStatusBar.ForeColor = System.Drawing.Color.Transparent;
        this.panelStatusBar.Location = new System.Drawing.Point(0, 444);
        this.panelStatusBar.Name = "panelStatusBar";
        this.panelStatusBar.Size = new System.Drawing.Size(880, 22);
        this.panelStatusBar.TabIndex = 11;
        // 
        // panelStatusBarExtensible
        // 
        this.panelStatusBarExtensible.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelStatusBarExtensible.BackgroundImage")));
        this.panelStatusBarExtensible.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelStatusBarExtensible.Location = new System.Drawing.Point(771, 0);
        this.panelStatusBarExtensible.Name = "panelStatusBarExtensible";
        this.panelStatusBarExtensible.Size = new System.Drawing.Size(95, 22);
        this.panelStatusBarExtensible.TabIndex = 10;
        // 
        // panelStatusBarRight
        // 
        this.panelStatusBarRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelStatusBarRight.BackgroundImage")));
        this.panelStatusBarRight.Dock = System.Windows.Forms.DockStyle.Right;
        this.panelStatusBarRight.Location = new System.Drawing.Point(866, 0);
        this.panelStatusBarRight.Name = "panelStatusBarRight";
        this.panelStatusBarRight.Size = new System.Drawing.Size(14, 22);
        this.panelStatusBarRight.TabIndex = 9;
        // 
        // panelStatusBarLeft
        // 
        this.panelStatusBarLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelStatusBarLeft.BackgroundImage")));
        this.panelStatusBarLeft.Controls.Add(this.labelStatusMsg);
        this.panelStatusBarLeft.Controls.Add(this.labelStatusAvgDOSpeed);
        this.panelStatusBarLeft.Controls.Add(this.labelStatusDOSpeed);
        this.panelStatusBarLeft.Controls.Add(this.labelStatusUPSpeed);
        this.panelStatusBarLeft.Controls.Add(this.labelStatusServer);
        this.panelStatusBarLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelStatusBarLeft.Location = new System.Drawing.Point(0, 0);
        this.panelStatusBarLeft.Name = "panelStatusBarLeft";
        this.panelStatusBarLeft.Size = new System.Drawing.Size(771, 22);
        this.panelStatusBarLeft.TabIndex = 8;
        // 
        // labelStatusMsg
        // 
        this.labelStatusMsg.BackColor = System.Drawing.Color.Transparent;
        this.labelStatusMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(69)))), ((int)(((byte)(151)))));
        this.labelStatusMsg.Location = new System.Drawing.Point(3, 5);
        this.labelStatusMsg.Name = "labelStatusMsg";
        this.labelStatusMsg.Size = new System.Drawing.Size(320, 13);
        this.labelStatusMsg.TabIndex = 11;
        this.labelStatusMsg.Text = "Hathi v";
        this.labelStatusMsg.TextChanged += new System.EventHandler(this.labelStatusMsg_TextChanged);
        // 
        // labelStatusAvgDOSpeed
        // 
        this.labelStatusAvgDOSpeed.BackColor = System.Drawing.Color.Transparent;
        this.labelStatusAvgDOSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(69)))), ((int)(((byte)(151)))));
        this.labelStatusAvgDOSpeed.Location = new System.Drawing.Point(456, 5);
        this.labelStatusAvgDOSpeed.Name = "labelStatusAvgDOSpeed";
        this.labelStatusAvgDOSpeed.Size = new System.Drawing.Size(72, 11);
        this.labelStatusAvgDOSpeed.TabIndex = 10;
        this.labelStatusAvgDOSpeed.Text = "AVGDO:0";
        // 
        // labelStatusDOSpeed
        // 
        this.labelStatusDOSpeed.BackColor = System.Drawing.Color.Transparent;
        this.labelStatusDOSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.labelStatusDOSpeed.ForeColor = System.Drawing.Color.Green;
        this.labelStatusDOSpeed.Location = new System.Drawing.Point(400, 5);
        this.labelStatusDOSpeed.Name = "labelStatusDOSpeed";
        this.labelStatusDOSpeed.Size = new System.Drawing.Size(48, 11);
        this.labelStatusDOSpeed.TabIndex = 9;
        this.labelStatusDOSpeed.Text = "DO:0";
        // 
        // labelStatusUPSpeed
        // 
        this.labelStatusUPSpeed.BackColor = System.Drawing.Color.Transparent;
        this.labelStatusUPSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.labelStatusUPSpeed.ForeColor = System.Drawing.Color.Red;
        this.labelStatusUPSpeed.Location = new System.Drawing.Point(344, 5);
        this.labelStatusUPSpeed.Name = "labelStatusUPSpeed";
        this.labelStatusUPSpeed.Size = new System.Drawing.Size(46, 12);
        this.labelStatusUPSpeed.TabIndex = 8;
        this.labelStatusUPSpeed.Text = "UP:0";
        // 
        // labelStatusServer
        // 
        this.labelStatusServer.BackColor = System.Drawing.Color.Transparent;
        this.labelStatusServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.labelStatusServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(69)))), ((int)(((byte)(151)))));
        this.labelStatusServer.Location = new System.Drawing.Point(537, 6);
        this.labelStatusServer.Name = "labelStatusServer";
        this.labelStatusServer.Size = new System.Drawing.Size(216, 11);
        this.labelStatusServer.TabIndex = 6;
        this.labelStatusServer.Text = "Not connected";
        this.toolTipMain.SetToolTip(this.labelStatusServer, "Users:0 Files:0");
        // 
        // panel1
        // 
        this.panel1.Location = new System.Drawing.Point(0, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(200, 100);
        this.panel1.TabIndex = 0;
        // 
        // panelContent
        // 
        this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.panelContent.Location = new System.Drawing.Point(0, 63);
        this.panelContent.Name = "panelContent";
        this.panelContent.Size = new System.Drawing.Size(880, 381);
        this.panelContent.TabIndex = 10;
        // 
        // panelExtensible
        // 
        this.panelExtensible.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.panelExtensible.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelExtensible.BackgroundImage")));
        this.panelExtensible.Controls.Add(this.buttonAboutMenu);
        this.panelExtensible.Controls.Add(this.buttonAbout);
        this.panelExtensible.Controls.Add(this.labelSessionDO);
        this.panelExtensible.Controls.Add(this.labelSessionUP);
        this.panelExtensible.Location = new System.Drawing.Point(656, 0);
        this.panelExtensible.Name = "panelExtensible";
        this.panelExtensible.Size = new System.Drawing.Size(216, 58);
        this.panelExtensible.TabIndex = 9;
        // 
        // buttonAboutMenu
        // 
        this.buttonAboutMenu.BackColor = System.Drawing.Color.Transparent;
        this.buttonAboutMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.buttonAboutMenu.Location = new System.Drawing.Point(127, 3);
        this.buttonAboutMenu.Name = "buttonAboutMenu";
        this.buttonAboutMenu.Size = new System.Drawing.Size(17, 19);
        this.buttonAboutMenu.TabIndex = 10;
        this.buttonAboutMenu.Text = ">";
        this.buttonAboutMenu.UseVisualStyleBackColor = false;
        this.buttonAboutMenu.Click += new System.EventHandler(this.buttonAboutMenu_Click);
        // 
        // buttonAbout
        // 
        this.buttonAbout.BackColor = System.Drawing.Color.Transparent;
        this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.buttonAbout.ForeColor = System.Drawing.Color.Black;
        this.buttonAbout.Location = new System.Drawing.Point(58, 3);
        this.buttonAbout.Name = "buttonAbout";
        this.buttonAbout.Size = new System.Drawing.Size(70, 19);
        this.buttonAbout.TabIndex = 9;
        this.buttonAbout.TabStop = false;
        this.buttonAbout.Text = "About ...";
        this.buttonAbout.UseVisualStyleBackColor = false;
        this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
        // 
        // labelSessionDO
        // 
        this.labelSessionDO.AutoSize = true;
        this.labelSessionDO.BackColor = System.Drawing.Color.Transparent;
        this.labelSessionDO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.labelSessionDO.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelSessionDO.ForeColor = System.Drawing.Color.Green;
        this.labelSessionDO.Location = new System.Drawing.Point(56, 24);
        this.labelSessionDO.Name = "labelSessionDO";
        this.labelSessionDO.Size = new System.Drawing.Size(104, 13);
        this.labelSessionDO.TabIndex = 8;
        this.labelSessionDO.Text = "Downloaded: 00.0 B";
        // 
        // labelSessionUP
        // 
        this.labelSessionUP.AutoSize = true;
        this.labelSessionUP.BackColor = System.Drawing.Color.Transparent;
        this.labelSessionUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.labelSessionUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelSessionUP.ForeColor = System.Drawing.Color.Red;
        this.labelSessionUP.Location = new System.Drawing.Point(56, 40);
        this.labelSessionUP.Name = "labelSessionUP";
        this.labelSessionUP.Size = new System.Drawing.Size(89, 13);
        this.labelSessionUP.TabIndex = 7;
        this.labelSessionUP.Text = "Uploaded: 00.0 B";
        // 
        // graphics
        // 
        this.graphics.AutoGraphicScale = false;
        this.graphics.BackColor = System.Drawing.Color.Transparent;
        this.graphics.GraphicScale = 0;
        this.graphics.Location = new System.Drawing.Point(7, 6);
        this.graphics.Name = "graphics";
        this.graphics.Size = new System.Drawing.Size(296, 38);
        this.graphics.TabIndex = 0;
        this.graphics.TabStop = false;
        this.graphics.MouseUp += new System.Windows.Forms.MouseEventHandler(this.graphics_Click);
        // 
        // panelToolbarRight
        // 
        this.panelToolbarRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.panelToolbarRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelToolbarRight.BackgroundImage")));
        this.panelToolbarRight.Location = new System.Drawing.Point(872, 0);
        this.panelToolbarRight.Name = "panelToolbarRight";
        this.panelToolbarRight.Size = new System.Drawing.Size(8, 58);
        this.panelToolbarRight.TabIndex = 8;
        // 
        // panelButtons
        // 
        this.panelButtons.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelButtons.BackgroundImage")));
        this.panelButtons.Controls.Add(this.HathiButtonConnect);
        this.panelButtons.Controls.Add(this.buttonOptions);
        this.panelButtons.Controls.Add(this.btnFriends);
        this.panelButtons.Controls.Add(this.btnServers);
        this.panelButtons.Controls.Add(this.btnSearch);
        this.panelButtons.Controls.Add(this.btnUploads);
        this.panelButtons.Controls.Add(this.btnDownloads);
        this.panelButtons.Location = new System.Drawing.Point(0, 0);
        this.panelButtons.Name = "panelButtons";
        this.panelButtons.Size = new System.Drawing.Size(400, 58);
        this.panelButtons.TabIndex = 7;
        // 
        // HathiButtonConnect
        // 
        this.HathiButtonConnect.BackColor = System.Drawing.Color.Transparent;
        this.HathiButtonConnect.Cursor = System.Windows.Forms.Cursors.Hand;
        this.HathiButtonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.HathiButtonConnect.ImageIndex = 1;
        this.HathiButtonConnect.ImageList = this.imageListServerCon;
        this.HathiButtonConnect.Location = new System.Drawing.Point(360, 8);
        this.HathiButtonConnect.Name = "HathiButtonConnect";
        this.HathiButtonConnect.Size = new System.Drawing.Size(40, 40);
        this.HathiButtonConnect.TabIndex = 6;
        this.HathiButtonConnect.UseVisualStyleBackColor = false;
        this.HathiButtonConnect.Click += new System.EventHandler(this.HathiButton1_Click);
        // 
        // imageListServerCon
        // 
        this.imageListServerCon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListServerCon.ImageStream")));
        this.imageListServerCon.TransparentColor = System.Drawing.Color.Transparent;
        this.imageListServerCon.Images.SetKeyName(0, "");
        this.imageListServerCon.Images.SetKeyName(1, "");
        this.imageListServerCon.Images.SetKeyName(2, "");
        // 
        // buttonOptions
        // 
        this.buttonOptions.Cursor = System.Windows.Forms.Cursors.Hand;
        this.buttonOptions.ImageIndex = 0;
        this.buttonOptions.ImageList = this.imageListOptions;
        this.buttonOptions.Location = new System.Drawing.Point(320, 8);
        this.buttonOptions.Name = "buttonOptions";
        this.buttonOptions.Size = new System.Drawing.Size(40, 40);
        this.buttonOptions.TabIndex = 5;
        this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
        // 
        // imageListOptions
        // 
        this.imageListOptions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListOptions.ImageStream")));
        this.imageListOptions.TransparentColor = System.Drawing.Color.Transparent;
        this.imageListOptions.Images.SetKeyName(0, "");
        this.imageListOptions.Images.SetKeyName(1, "");
        this.imageListOptions.Images.SetKeyName(2, "");
        // 
        // btnFriends
        // 
        this.btnFriends.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnFriends.BackColor = System.Drawing.Color.Transparent;
        this.btnFriends.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnFriends.Location = new System.Drawing.Point(232, 0);
        this.btnFriends.Name = "btnFriends";
        this.btnFriends.Size = new System.Drawing.Size(56, 56);
        this.btnFriends.TabIndex = 4;
        this.btnFriends.Tag = "Friends";
        this.btnFriends.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.btnFriends.UseVisualStyleBackColor = false;
        this.btnFriends.CheckedChanged += new System.EventHandler(this.btnShared_CheckedChanged);
        // 
        // btnServers
        // 
        this.btnServers.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnServers.BackColor = System.Drawing.Color.Transparent;
        this.btnServers.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnServers.Location = new System.Drawing.Point(176, 0);
        this.btnServers.Name = "btnServers";
        this.btnServers.Size = new System.Drawing.Size(56, 56);
        this.btnServers.TabIndex = 3;
        this.btnServers.Tag = "Servers";
        this.btnServers.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.btnServers.UseVisualStyleBackColor = false;
        this.btnServers.CheckedChanged += new System.EventHandler(this.btnServers_CheckedChanged);
        // 
        // btnSearch
        // 
        this.btnSearch.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnSearch.BackColor = System.Drawing.Color.Transparent;
        this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnSearch.Location = new System.Drawing.Point(120, 0);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(56, 56);
        this.btnSearch.TabIndex = 2;
        this.btnSearch.Tag = "Search";
        this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.btnSearch.UseVisualStyleBackColor = false;
        this.btnSearch.CheckedChanged += new System.EventHandler(this.btnSearch_CheckedChanged);
        // 
        // btnUploads
        // 
        this.btnUploads.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnUploads.BackColor = System.Drawing.Color.Transparent;
        this.btnUploads.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnUploads.Location = new System.Drawing.Point(64, 0);
        this.btnUploads.Name = "btnUploads";
        this.btnUploads.Size = new System.Drawing.Size(56, 56);
        this.btnUploads.TabIndex = 1;
        this.btnUploads.Tag = "Upload";
        this.btnUploads.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.btnUploads.UseVisualStyleBackColor = false;
        this.btnUploads.CheckedChanged += new System.EventHandler(this.btnUploads_CheckedChanged);
        // 
        // btnDownloads
        // 
        this.btnDownloads.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnDownloads.BackColor = System.Drawing.Color.White;
        this.btnDownloads.Checked = true;
        this.btnDownloads.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnDownloads.Location = new System.Drawing.Point(8, 0);
        this.btnDownloads.Name = "btnDownloads";
        this.btnDownloads.Size = new System.Drawing.Size(56, 56);
        this.btnDownloads.TabIndex = 0;
        this.btnDownloads.TabStop = true;
        this.btnDownloads.Tag = "Download";
        this.btnDownloads.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.btnDownloads.UseVisualStyleBackColor = false;
        this.btnDownloads.CheckedChanged += new System.EventHandler(this.btnDownloads_CheckedChanged);
        // 
        // imageListServerDiscon
        // 
        this.imageListServerDiscon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListServerDiscon.ImageStream")));
        this.imageListServerDiscon.TransparentColor = System.Drawing.Color.Transparent;
        this.imageListServerDiscon.Images.SetKeyName(0, "");
        this.imageListServerDiscon.Images.SetKeyName(1, "");
        this.imageListServerDiscon.Images.SetKeyName(2, "");
        // 
        // imageListHelp
        // 
        this.imageListHelp.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListHelp.ImageStream")));
        this.imageListHelp.TransparentColor = System.Drawing.Color.Transparent;
        this.imageListHelp.Images.SetKeyName(0, "");
        this.imageListHelp.Images.SetKeyName(1, "");
        this.imageListHelp.Images.SetKeyName(2, "");
        // 
        // panelGraphic
        // 
        this.panelGraphic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelGraphic.BackgroundImage")));
        this.panelGraphic.Controls.Add(this.graphics);
        this.panelGraphic.Location = new System.Drawing.Point(400, 0);
        this.panelGraphic.Name = "panelGraphic";
        this.panelGraphic.Size = new System.Drawing.Size(312, 58);
        this.panelGraphic.TabIndex = 12;
        // 
        // notifyIcon1
        // 
        this.notifyIcon1.ContextMenu = this.contextMenuNotifyIcon;
        this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
        this.notifyIcon1.Text = "Hathi";
        this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
        // 
        // contextMenuNotifyIcon
        // 
        this.contextMenuNotifyIcon.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNotifyIconOpen,
            this.menuItemNotifyIconQuit,
            this.menuItemNotifyIconOptions});
        // 
        // menuItemNotifyIconOpen
        // 
        this.menuItemNotifyIconOpen.DefaultItem = true;
        this.menuItemNotifyIconOpen.Index = 0;
        this.menuItemNotifyIconOpen.Text = "Open";
        this.menuItemNotifyIconOpen.Click += new System.EventHandler(this.notifyIcon1_DoubleClick);
        // 
        // menuItemNotifyIconQuit
        // 
        this.menuItemNotifyIconQuit.Index = 1;
        this.menuItemNotifyIconQuit.Text = "Quit";
        this.menuItemNotifyIconQuit.Click += new System.EventHandler(this.menuItemQuitNotifyIcon_click);
        // 
        // menuItemNotifyIconOptions
        // 
        this.menuItemNotifyIconOptions.Index = 2;
        this.menuItemNotifyIconOptions.Text = "Options";
        this.menuItemNotifyIconOptions.Click += new System.EventHandler(this.menuItemOptionsNotifyIcon_click);
        // 
        // imageListIcons
        // 
        this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
        this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
        this.imageListIcons.Images.SetKeyName(0, "");
        this.imageListIcons.Images.SetKeyName(1, "");
        this.imageListIcons.Images.SetKeyName(2, "");
        // 
        // contextMenuAbout
        // 
        this.contextMenuAbout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemWeb,
            this.menuItemSupport,
            this.menuItemSkins});
        this.contextMenuAbout.Popup += new System.EventHandler(this.contextMenuAbout_Popup);
        // 
        // menuItemWeb
        // 
        this.menuItemWeb.Index = 0;
        this.menuItemWeb.Text = "Hathi Web";
        this.menuItemWeb.Click += new System.EventHandler(this.menuItemAbout_click);
        // 
        // menuItemSupport
        // 
        this.menuItemSupport.Index = 1;
        this.menuItemSupport.Text = "Hathi support";
        this.menuItemSupport.Click += new System.EventHandler(this.menuItemAbout_click);
        // 
        // menuItemSkins
        // 
        this.menuItemSkins.Index = 2;
        this.menuItemSkins.Text = "Hathi skins";
        this.menuItemSkins.Click += new System.EventHandler(this.menuItemAbout_click);
        // 
        // HathiForm
        // 
        this.AllowDrop = true;
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        this.ClientSize = new System.Drawing.Size(880, 466);
        this.Controls.Add(this.panelGraphic);
        this.Controls.Add(this.panelStatusBar);
        this.Controls.Add(this.panelContent);
        this.Controls.Add(this.panelExtensible);
        this.Controls.Add(this.panelToolbarRight);
        this.Controls.Add(this.panelButtons);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "HathiForm";
        this.Text = "Hathi v";
        this.Load += new System.EventHandler(this.HathiForm_Load);
        this.Activated += new System.EventHandler(this.HathiForm_Activated);
        this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HathiForm_DragDrop);
        this.Closing += new System.ComponentModel.CancelEventHandler(this.HathiForm_Closing);
        this.Resize += new System.EventHandler(this.HathiForm_Resize);
        this.DragOver += new System.Windows.Forms.DragEventHandler(this.HathiForm_DragOver);
        this.panelStatusBar.ResumeLayout(false);
        this.panelStatusBarLeft.ResumeLayout(false);
        this.panelExtensible.ResumeLayout(false);
        this.panelExtensible.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.graphics)).EndInit();
        this.panelButtons.ResumeLayout(false);
        this.panelGraphic.ResumeLayout(false);
        this.ResumeLayout(false);

    }
    #endregion


    private void m_Close_eLephant()
    {
        krnGateway.NotRefresh=true;
        Thread.Sleep(1000);
        FDownloads.SaveListsSettings();
        FUploads.SaveListsSettings();
        FServers.SaveListsSettings();
        FSearchs.SaveListsSettings();
        preferences.SetProperty("AutoGraphicScale",graphics.AutoGraphicScale);
        preferences.SetProperty("GraphicScale",graphics.GraphicScale);
        if (this.WindowState!=FormWindowState.Minimized)
        {
            int margin=0;
            if (this.WindowState==FormWindowState.Maximized)
                margin=1;
            preferences.SetProperty("WindowWidth", this.Width-margin*2);
            preferences.SetProperty("WindowHeight", this.Height-margin*2);
            preferences.SetProperty("WindowLocationX", this.Left+margin);
            preferences.SetProperty("WindowLocationY", this.Top+margin);
        }
        preferences.SaveProperties();
        krnGateway.OnRefreshStatus-=new RefreshEvent(m_InvokeOnRefreshStatus);
        krnGateway.OnNewChatMessage-=new SourceEvent(m_InvokeOnNewChatMessage);
        krnGateway.OnStartChatSession-=new ClientEvent(m_InvokeOnStartChatSession);
        krnGateway.OnAddingFriend-=new ClientEvent(m_InvokeOnAddingFriend);
        krnGateway.OnDeleteFriend-=new ClientEvent(m_InvokeOnDeleteFriend);
        krnGateway.CloseKernel();
        Application.Exit();
    }

    private void HathiForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (MessageBox.Show(Globalization["MSG_EXIT"],"Hathi",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
        {
            m_Close_eLephant();
        }
        else e.Cancel=true;
    }

    private void btnUploads_CheckedChanged(object sender, System.EventArgs e)
    {
        if (btnUploads.Checked)
        {
            FUploads.Show();
        }
        else FUploads.Hide();
    }

    private void btnDownloads_CheckedChanged(object sender, System.EventArgs e)
    {
        if (btnDownloads.Checked)
            FDownloads.Show();
        else FDownloads.Hide();
    }
    private void btnServers_CheckedChanged(object sender, System.EventArgs e)
    {
        if (btnServers.Checked)
            FServers.Show();
        else FServers.Hide();
    }
    private void btnSearch_CheckedChanged(object sender, System.EventArgs e)
    {
        if (btnSearch.Checked)
            FSearchs.Show();
        else FSearchs.Hide();
    }
    private void btnShared_CheckedChanged(object sender, System.EventArgs e)
    {
        if (btnFriends.Checked)
            FFriends.Show();
        else FFriends.Hide();
    }
    private void buttonOptions_Click(object sender, System.EventArgs e)
    {
        FormOptions FOptions=new FormOptions(krnGateway);
        FOptions.ShowDialog();
        m_Preferences=krnGateway.GetConfig();
        if (m_Preferences.maxDownloadRate>m_Preferences.maxUploadRate)
            graphics.SetMaxValue((int)m_Preferences.maxDownloadRate);
        else
            graphics.SetMaxValue((int)m_Preferences.maxUploadRate);
        if (preferences.GetString("Skin")!=Skin.CurrentSkin)
        {
            Skin.CurrentSkin=preferences.GetString("Skin");
            m_ApplySkin();
            FDownloads.ApplySkin();
            FUploads.ApplySkin();
            FSearchs.ApplySkin();
            FServers.ApplySkin();
            FFriends.ApplySkin();
        }
        if (preferences.GetString("Language")!=Globalization.Language)
        {
            Globalization.Language = preferences.GetString("Language");
            m_Globalize();
            FDownloads.Globalize();
            FUploads.Globalize();
            FSearchs.Globalize();
            FServers.Globalize();
            FFriends.Globalize();
            FDownloads.downloadList.Globalize();
            FDownloads.sourcesList.Globalize();
            FDownloads.listViewComments.Globalize();
            FDownloads.listViewNames.Globalize();
            FUploads.sharedListView.Globalize();
            FUploads.uploadsList.Globalize();
            FServers.serversList.Globalize();
        }
    }

    private void m_InvokeOnRefreshStatus(CkernelGateway in_krnGateway)
    {
        try
        {
            this.Invoke(new RefreshEvent(m_OnRefreshStatus),new object[] {in_krnGateway});
        }
        catch {}
    }

    private void m_OnRefreshStatus(CkernelGateway in_krnGateway)
    {
        GlobalStatus status=krnGateway.GetGlobalStatus();
        byte IconNumber;
        labelStatusUPSpeed.Text=Globalization["LBL_UP"]+status.UploadSpeed.ToString("###0.#");
        labelStatusDOSpeed.Text=Globalization["LBL_DO"]+status.DowloadSpeed.ToString("###0.#");
        labelStatusAvgDOSpeed.Text=Globalization["LBL_AVGDO"]+status.AvgDownSpeed.ToString();
        labelSessionUP.Text=Globalization["LBL_UPLOADED"]+":"+HathiListView.SizeToString(status.SessionUpload);
        labelSessionDO.Text=Globalization["LBL_DOWNLOADED"]+":"+HathiListView.SizeToString(status.SessionDownload);
        notifyIcon1.Text=labelStatusUPSpeed.Text+" "+labelStatusDOSpeed.Text+" "+labelStatusAvgDOSpeed.Text;
        this.Text= String.Format("Hathi v{0} ( {1} )", in_krnGateway.Version, notifyIcon1.Text);
#if DEBUG
        labelStatusServer.Text="Cnx: "+ status.ActiveConnections.ToString()+" ";
        labelStatusServer.Text+=status.ServerName;
#else
        labelStatusServer.Text=status.ServerName;
#endif
        toolTipMain.SetToolTip(labelStatusServer,Globalization["LBL_USERS"]+":"+status.ServerUsers+" - "+Globalization["LBL_FILES"]+":"+status.ServerFiles.ToString()+" - ID:" + status.UserID.ToString());
        graphics.AddValue(status.DowloadSpeed,status.UploadSpeed);
        if (status.UserID==0)
            IconNumber = 1;
        else
        {
            if (status.IsHighID) IconNumber = 0;
            else IconNumber = 2;
        }
        if (m_LastIcon!=IconNumber)
        {
            m_UpdateStatusIcon(imageListIcons.Images[IconNumber]);
            m_LastIcon=IconNumber;
        }
        if (notifyIcon1.Visible)
        {
            m_DrawNotifyIcon(imageListIcons.Images[IconNumber],status.DowloadSpeed/m_Preferences.maxDownloadRate,status.UploadSpeed/m_Preferences.maxUploadRate);
        }
        if (status.UserID!=0)
            HathiButtonConnect.ImageList=this.imageListServerDiscon;
        else
            HathiButtonConnect.ImageList=this.imageListServerCon;
    }

    private void HathiButton1_Click(object sender, System.EventArgs e)
    {
        krnGateway.ConnectToaServer();
    }


    private void m_StartKernel()
    {
        if (!m_KernelLoaded)
        {
            FDownloads.Show();
            m_KernelLoaded=true;
            if (m_remoteGateway==null)
                krnGateway=new CkernelGateway();
            else
                krnGateway=new CkernelGateway(m_remoteGateway);
            Connect();
            if (m_link!=null) krnGateway.DownloadElink(m_link,false);
        }
    }

    private void HathiForm_Activated(object sender, System.EventArgs e)
    {
        /*          if (!m_KernelLoaded)
                    {
                        FDownloads.Show();
                        m_KernelLoaded=true;
                        if (m_remoteGateway==null)
                            krnGateway=new CkernelGateway();
                        else
                            krnGateway=new CkernelGateway(m_remoteGateway);
                        Connect();
                        if (m_link!=null) krnGateway.DownloadElink(m_link,false);
                    }
        */      }

    private void HathiForm_Resize(object sender, System.EventArgs e)
    {
        if (WindowState!=FormWindowState.Minimized) m_PreviousState=WindowState;
        if (krnGateway!=null)
            krnGateway.NotRefresh=(WindowState==FormWindowState.Minimized);
        if ((WindowState==FormWindowState.Minimized)&&(preferences!=null)&&(preferences.GetBool("MinimizeToTray",true)))
        {
            FDownloads.HideDetails();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HathiForm));
            notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            notifyIcon1.Visible=true;
            Hide();
        }
        else
        {
            notifyIcon1.Visible=false;
        }
    }

    private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
    {
        Show();
        Activate();
        WindowState=m_PreviousState;
    }

    private void m_DrawNotifyIcon(Image img, float DownloadPerCent,float UploadPerCent)
    {
        m_gr = Graphics.FromImage(img);
        int limitdownload=(int)((1F-DownloadPerCent)*16F);
        int limitupload=(int)((1F-UploadPerCent)*16F);
        m_gr.DrawLine(new Pen(Brushes.Blue,1),0,limitdownload,0,15);
        m_gr.DrawLine(new Pen(Brushes.Red,1),2,limitupload,2,15);
        System.IntPtr oldhandle=notifyIcon1.Icon.Handle;
        System.IntPtr handle=((Bitmap)img).GetHicon();
        m_ico =Icon.FromHandle(handle);
        notifyIcon1.Icon=m_ico;
        Win32.DestroyIcon(oldhandle);
    }


    private void m_UpdateStatusIcon(Image img)
    {
        System.IntPtr handle=((Bitmap)img).GetHicon();
        m_ico =Icon.FromHandle(handle);
        System.IntPtr oldhandle=this.Icon.Handle;
        this.Icon=m_ico;
        Win32.DestroyIcon(oldhandle);
    }

    private void HathiForm_Load(object sender, System.EventArgs e)
    {
        Win32.SetProp(this.Handle,InterfaceConstants.GUID,1);
        m_StartKernel();
        Size m_windowSize=this.Size;
        Point m_windowLocation=this.Location;
        try
        {
            m_windowSize.Width=preferences.GetInt("WindowWidth");
            m_windowSize.Height=preferences.GetInt("WindowHeight");
            m_windowLocation.X=preferences.GetInt("WindowLocationX");
            m_windowLocation.Y=preferences.GetInt("WindowLocationY");
        }
        catch
        {
            // not fatal, just rewrite default values
            m_windowSize=this.Size;
            m_windowLocation=this.Location;
        }
        // write to the current window the read values from the xml config file
        this.Size=m_windowSize;
        this.Location=m_windowLocation;
        this.FDownloads.HideDetails();
    }

    private void labelStatusMsg_TextChanged(object sender, System.EventArgs e)
    {
        if ((WindowState==FormWindowState.Minimized)&&(notifyIcon1.Visible==true)&&(preferences.GetBool("TaskBarNotifier",true)))
        {
//              taskbarNotifier1.CloseClickable=true;
//              taskbarNotifier1.TitleClickable=false;
//              taskbarNotifier1.ContentClickable=true;
//              taskbarNotifier1.EnableSelectionRectangle=true;
//              taskbarNotifier1.KeepVisibleOnMousOver=true;
//              taskbarNotifier1.ReShowOnMouseOver=true;
//              taskbarNotifier1.Show("",labelStatusMsg.Text,500,3000,500);
            //MethodInvoker mi = new MethodInvoker(m_TaskBarNotifier1Show);
            Invoke(new TextEvent(m_TaskBarNotifier1Show),new object[] {labelStatusMsg.Text});
        }
    }

    private void m_TaskBarNotifier1Show(string text)
    {
        taskbarNotifier1.CloseClickable=true;
        taskbarNotifier1.TitleClickable=false;
        taskbarNotifier1.ContentClickable=true;
        taskbarNotifier1.EnableSelectionRectangle=true;
        taskbarNotifier1.KeepVisibleOnMousOver=true;
        taskbarNotifier1.ReShowOnMouseOver=true;
        taskbarNotifier1.Show("",text,500,3000,500);
    }

    private void m_BallonClicked(object sender, System.EventArgs e)
    {
        notifyIcon1_DoubleClick(sender,e);
    }

    /// <summary>
    /// Event handler for preferences class if any property was changed.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">changed property arguments</param>
    private void m_OnPropertyChanged(object sender, PropertyEventArgs e)
    {
        //TO DO
    }
    private void m_StartCharSession()
    {
        if (!btnFriends.Checked)
        {
            btnFriends.Checked=true;
            if ((preferences.GetBool("MinimizeToTray",true))&&(WindowState==FormWindowState.Minimized))
            {
                if (preferences.GetBool("TaskBarNotifier",true))
                {
                    /*taskbarNotifier1.CloseClickable=true;
                    taskbarNotifier1.TitleClickable=false;
                    taskbarNotifier1.ContentClickable=true;
                    taskbarNotifier1.EnableSelectionRectangle=true;
                    taskbarNotifier1.KeepVisibleOnMousOver=true;
                    taskbarNotifier1.ReShowOnMouseOver=true;
                    taskbarNotifier1.Show("",Globalization["MSG_NEWCHAT"]+m_LastChatMessage,500,3000,500);
                    */
                    Invoke(new TextEvent(m_TaskBarNotifier1Show),new object[] {Globalization["MSG_NEWCHAT"]+m_LastChatMessage});
                }
            }
            else
            {
                Win32.SetForegroundWindow(Handle);
            }
        }
    }
    private void m_AddingFriend()
    {
        FFriends.LoadFriendsList();
        if (!btnFriends.Checked)
        {
            btnFriends.Checked=true;
        }
    }
    private void m_DeleteFriend()
    {
        FFriends.LoadFriendsList();
        if (!btnFriends.Checked)
        {
            btnFriends.Checked=true;
        }
    }

    private void m_InvokeOnNewChatMessage(InterfaceClient client, string message)
    {
        m_LastChatMessage=client.Name+":"+message;
        MethodInvoker mi = new MethodInvoker(this.m_StartCharSession);
        this.BeginInvoke(mi);
    }
    private void m_InvokeOnStartChatSession(InterfaceClient client)
    {
        m_LastChatMessage=client.Name;
        MethodInvoker mi = new MethodInvoker(this.m_StartCharSession);
        this.BeginInvoke(mi);
    }
    private void m_InvokeOnAddingFriend(InterfaceClient client)
    {
        m_LastChatMessage=client.Name;
        MethodInvoker mi = new MethodInvoker(this.m_AddingFriend);
        this.BeginInvoke(mi);
    }
    private void m_InvokeOnDeleteFriend(InterfaceClient client)
    {
        MethodInvoker mi = new MethodInvoker(this.m_DeleteFriend);
        this.BeginInvoke(mi);
    }
    private void m_Globalize()
    {
        btnDownloads.Tag=Globalization["LBL_DOWNLOADS"];
        btnUploads.Tag=Globalization["LBL_UPLOADS"];
        btnServers.Tag=Globalization["LBL_SERVERS"];
        btnSearch.Tag=Globalization["LBL_SEARCH"];
        btnFriends.Tag=Globalization["LBL_FRIENDS"];
        labelStatusAvgDOSpeed.Text=Globalization["LBL_AVGDO"];
        labelStatusDOSpeed.Text=Globalization["LBL_DO"];
        labelStatusUPSpeed.Text=Globalization["LBL_UP"];
        labelSessionDO.Text=Globalization["LBL_DOWNLOADED"] + ":";
        labelSessionUP.Text=Globalization["LBL_UPLOADED"] + ":";
        graphics.strSeconds=Globalization["LBL_SECONDS"];
        graphics.strMinutes=Globalization["LBL_MINUTES"];
        graphics.strHours=Globalization["LBL_HOURS"];
        graphics.strAuto=Globalization["LBL_AUTOMATIC"];
        buttonAbout.Text=Globalization["LBL_ABOUT"];
        contextMenuAbout.MenuItems[0].Text=Globalization["LBL_LPHANTWEB"];
        contextMenuAbout.MenuItems[1].Text=Globalization["LBL_HELP"]+"/"+Globalization["LBL_SUPPORT"];
        contextMenuNotifyIcon.MenuItems[0].Text=Globalization["LBL_OPEN"];
        contextMenuNotifyIcon.MenuItems[1].Text=Globalization["LBL_QUIT"];
        contextMenuNotifyIcon.MenuItems[2].Text=Globalization["LBL_OPTIONS"];
    }

    private void graphics_Click(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) return;
        if (panelGraphic.Height<100)
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HathiForm));
            panelGraphic.BackgroundImage=Skin.GetBitmap("BigGraphic");
            panelGraphic.Height=160;
            int x=0;
            int y=0;
            Skin.GetSize("BigGraphicSize",ref x,ref y);
            graphics.Height=y;
            Skin.GetPosition("BigGraphicPosition",ref x, ref y);
            graphics.Left=x;
            graphics.Top=y;
        }
        else
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HathiForm));
            panelGraphic.BackgroundImage=Skin.GetBitmap("SmallGraphic");
            panelGraphic.Height=58;
            int x=0;
            int y=0;
            Skin.GetSize("SmallGraphicSize",ref x,ref y);
            graphics.Height=y;
            Skin.GetPosition("SmallGraphicPosition",ref x, ref y);
            graphics.Left=x;
            graphics.Top=y;
        }
    }

    private void buttonAbout_Click(object sender, System.EventArgs e)
    {
        FormAbout frmAbout=new FormAbout();
        frmAbout.ShowDialog(this);
        frmAbout.Dispose();
    }

    private void buttonAboutMenu_Click(object sender, System.EventArgs e)
    {
        contextMenuAbout.Show(buttonAbout,new Point(0,buttonAbout.Height));
    }

    private void menuItemAbout_click(object sender, System.EventArgs e)
    {
        try
        {
            int itemNumber=contextMenuAbout.MenuItems.IndexOf((MenuItem)sender);
            switch (itemNumber)
            {
            case 0:
                System.Diagnostics.Process.Start("http://hathi.sourceforge.net");
                break;
            case 1:
                System.Diagnostics.Process.Start("http://sourceforge.net/forum/forum.php?forum_id=962503");
                break;
            case 2:
                System.Diagnostics.Process.Start("http://hathi.sourceforge.net/skins");
                break;
            default:
                if (itemNumber==this.contextMenuAbout.MenuItems.Count-1)
                    System.Diagnostics.Process.Start(Skin.LinkUrl);
                break;
            }
        }
        catch
        {
            //can not start process.... ignore error
        }
    }


    private void menuItemQuitNotifyIcon_click(object sender, System.EventArgs e)
    {
        if (MessageBox.Show(Globalization["MSG_EXIT"],"Hathi",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
        {
            notifyIcon1.Visible=false;
            m_Close_eLephant();
        }
    }

    private void menuItemOptionsNotifyIcon_click(object sender, System.EventArgs e)
    {
        menuItemNotifyIconOptions.Enabled=false;
        menuItemNotifyIconOpen.Enabled=false;
        menuItemNotifyIconQuit.Enabled=false;
        buttonOptions_Click(sender, e);
        menuItemNotifyIconOptions.Enabled=true;
        menuItemNotifyIconOpen.Enabled=true;
        menuItemNotifyIconQuit.Enabled=true;
    }

    private void HathiForm_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.Text))
        {
            string txtLink=(string)e.Data.GetData(DataFormats.Text,true);
            if (txtLink.IndexOf("ed2k://")>=0)
                e.Effect=DragDropEffects.Link;
            else
                e.Effect=DragDropEffects.None;
        }
        else
            e.Effect=DragDropEffects.None;
    }

    private void HathiForm_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.Text))
        {
            string txtLink=(string)e.Data.GetData(DataFormats.Text,true);
            if (txtLink.IndexOf("ed2k://")==0)
            {
                if (krnGateway!=null) krnGateway.DownloadElink(txtLink,false);
            }
        }
    }

    private void contextMenuAbout_Popup(object sender, EventArgs e)
    {

    }

}
}
