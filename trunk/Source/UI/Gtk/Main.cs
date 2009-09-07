#region Copyright (c)2009 Hathi Project < http://hathi.sourceforge.net >
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
using Gtk;
using Glade;
using Gdk;
using Hathi.eDonkey;
using Hathi.Classes;
using System.Collections;

namespace HathiGTK
{
public class HathiGTK
{
    [Glade.Widget("mainwindow")] Gtk.Window mainwindow;

    [Glade.Widget] Gtk.Button btnDownloads;
    [Glade.Widget] Gtk.Button btnUploads;
    [Glade.Widget] Gtk.Button btnSearch;
    [Glade.Widget] Gtk.Button btnServers;
    [Glade.Widget] Gtk.Button btnFriends;
    [Glade.Widget] Gtk.Button btnPreferences;
    [Glade.Widget] Gtk.Button btnConnect;
    [Glade.Widget] Gtk.Button btnAbout;
    [Glade.Widget] Gtk.Button btnReloadShFiles;
    [Glade.Widget] Gtk.Button btnBeginSearch;
    [Glade.Widget] Gtk.Button btnDownloadServers;
    [Glade.Widget] Gtk.Button btnAddServer;

    [Glade.Widget] Gtk.Image imgDownloads;
    [Glade.Widget] Gtk.Image imgUploads;
    [Glade.Widget] Gtk.Image imgSearch;
    [Glade.Widget] Gtk.Image imgServers;
    [Glade.Widget] Gtk.Image imgFriends;
    [Glade.Widget] Gtk.Image imgPreferences;
    [Glade.Widget] Gtk.Image imgConnect;
    [Glade.Widget] Gtk.Image imgAbout2;

    [Glade.Widget] Gtk.Notebook notebook;
    [Glade.Widget] Dialog dlgAbout;
    [Glade.Widget] Gtk.Image imgAbout;
    [Glade.Widget] Gtk.TextView txtServerLog;

    [Glade.Widget] Gtk.Window dlgOptions;

    [Glade.Widget] Gtk.TreeView tvDownloads;
    [Glade.Widget] Gtk.TreeView tvComments;
    [Glade.Widget] Gtk.TreeView tvFileNames;
    [Glade.Widget] Gtk.TreeView tvSources;
    [Glade.Widget] Gtk.TreeView tvUploads;
    [Glade.Widget] Gtk.TreeView tvSharedFiles;
    [Glade.Widget] Gtk.TreeView tvServers;
    [Glade.Widget] Gtk.TreeView tvFriends;
    [Glade.Widget] Gtk.TreeView tvSearch;

    [Glade.Widget] Gtk.Entry txtSearch;
    [Glade.Widget] Gtk.Entry txtServerIp;
    [Glade.Widget] Gtk.Entry txtServerPort;
    [Glade.Widget] Gtk.Entry txtDownloadServers;

    [Glade.Widget] Gtk.HBox hboxSearch;
    [Glade.Widget] Gtk.ProgressBar progressbar;

    public Hathi.eDonkey.CKernel HathiKernel;
    public Hathi.eDonkey.CInterfaceGateway krnGateway;
    internal static Config preferences;
    private Gtk.Menu menu;
    public Glade.XML gxml;
    private TDownloads downloads;
    private TUploads uploads;
    private TServers servers;
    private TSharedFiles sharedFiles;
    private TComments comments;
    private TIcon trayicon;
    private TSearch search;

    public void InitInterface()
    {
        mainwindow.Icon = new Gdk.Pixbuf (null, "lPhant.png");
        mainwindow.Maximize ();
        imgDownloads.Pixbuf = new Gdk.Pixbuf (null, "down.gif");
        imgUploads.Pixbuf = new Gdk.Pixbuf (null, "up.gif");
        imgSearch.Pixbuf = new Gdk.Pixbuf (null, "viewmag.gif");
        imgServers.Pixbuf = new Gdk.Pixbuf (null, "network.gif");
        imgFriends.Pixbuf = new Gdk.Pixbuf (null, "kdmconfig.gif");
        imgPreferences.Pixbuf = new Gdk.Pixbuf (null, "configure.gif");
        imgConnect.Pixbuf = new Gdk.Pixbuf (null, "connect_no.gif");
        imgAbout.Pixbuf = new Gdk.Pixbuf (null, "help.gif");
        downloads = new TDownloads (tvDownloads,krnGateway);
        uploads = new TUploads (tvUploads,krnGateway);
        comments = new TComments (tvComments,krnGateway);
        servers = new TServers (krnGateway,gxml,txtServerLog);
        sharedFiles = new TSharedFiles (tvSharedFiles,krnGateway);
        search = new TSearch (krnGateway,gxml);
        trayicon = new TIcon (krnGateway,mainwindow);
        tvFileNames.AppendColumn ("File name", new CellRendererText(),"text",0);
        tvFileNames.AppendColumn ("Sources", new CellRendererText(),"text",1);
        tvSources.AppendColumn ("Name",new CellRendererText(),"text",0);
        tvSources.AppendColumn ("File name",new CellRendererText(),"text",1);
        tvSources.AppendColumn ("Speed",new CellRendererText(),"text",2);
        tvSources.AppendColumn ("Status",new CellRendererText(),"text",3);
        tvSources.AppendColumn ("Position",new CellRendererText(),"text",4);
        tvSources.AppendColumn ("Downloaded",new CellRendererText(),"text",5);
        tvSources.AppendColumn ("Uploaded",new CellRendererText(),"text",6);
        tvSources.AppendColumn ("Progress",new CellRendererText(),"text",7);
        tvSources.AppendColumn ("Software",new CellRendererText(),"text",8);
        tvSources.AppendColumn ("Version",new CellRendererText(),"text",9);
        tvFriends.AppendColumn ("Friend", new CellRendererText(),"text",0);
    }

    public void PopupMenuEvent (object sender, PopupMenuArgs args)
    {
        //Gdk.EventButton eb = args.Event;
        //if (eb.Button == 3) { // Right click
        // pop up menu here...
        Console.WriteLine ("popup");
        //}
    }

    public void jodete (object sender, EventArgs args)
    {
        //Gdk.EventButton eb = args.Event;
        //if (eb.Button == 3) { // Right click
        // pop up menu here...
        Console.WriteLine ("Evento columna");
        //}
    }

    public static void Main (string[] args)
    {
        new HathiGTK (args);
    }

    public HathiGTK (string[] args)
    {
        Application.Init();
        gxml = new Glade.XML (null, "elephant.glade", "mainwindow", null);
        gxml.Autoconnect (this);
        HathiKernel=new CKernel(); //creates and starts the kernel
        krnGateway=CKernel.InterfaceGateway[0]; //gateway to talk with the kernel
        InitInterface();
        //preferences = new Config(CKernel.DllDirectory, "configInterface.xml", "0.01", "HathiInterface");
        //m_Preferences=krnGateway.GetConfig(); //do I need it?
        mainwindow.Title = "HathiGTK " + CKernel.Version.ToString();
        mainwindow.Show();
        Application.Run();
    }

    public void OnWindowDeleteEvent (object o, DeleteEventArgs args)
    {
        MessageDialog dlgExit = new MessageDialog (mainwindow,
                DialogFlags.DestroyWithParent,
                MessageType.Warning,
                ButtonsType.YesNo, "Are you sure you want to quit?");
        ResponseType result = (ResponseType)dlgExit.Run ();
        if (result == ResponseType.Yes)
        {
            krnGateway.CloseKernel();
            Application.Quit();
            args.RetVal = false;
        }
        else
        {
            dlgExit.Destroy();
            args.RetVal = true;
        }
    }

    public void on_unmap_event (object sender, EventArgs args)
    {
        mainwindow.Hide ();
    }

    public void on_btnAbout_clicked (object sender, EventArgs args)
    {
        AboutDialog about = new AboutDialog();
        about.Run();
    }

    public void on_btnPreferences_clicked (object sender,EventArgs args)
    {
        OptionsDialog options = new OptionsDialog (krnGateway);
        options.Run();
    }

    private void on_btnDownloads_clicked (object sender, EventArgs args)
    {
        notebook.Page = 0;
    }

    private void on_btnUploads_clicked (object sender, EventArgs args)
    {
        notebook.Page = 1;
    }

    private void on_btnSearch_clicked (object o,EventArgs args)
    {
        notebook.Page = 2;
    }

    private void on_btnServers_clicked (object o,EventArgs args)
    {
        notebook.Page = 3;
    }

    private void on_btnFriends_clicked (object o,EventArgs args)
    {
        notebook.Page = 4;
    }

    private void on_btnConnect_clicked (object o,EventArgs args)
    {
        krnGateway.ConnectToaServer();
    }

    private void on_btnAddServer_clicked (object sender, EventArgs args)
    {
        servers.AddServer();
    }

    private void on_btnDownloadServers_clicked (object sender, EventArgs args)
    {
        servers.DownloadServers();
    }
}
}