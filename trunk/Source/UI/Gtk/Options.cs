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
using Hathi.Types;
using Hathi.eDonkey;
using Hathi.Classes;

namespace HathiGTK
{
public class OptionsDialog
{
    [Glade.Widget] Gtk.Window dlgOptions;
    [Glade.Widget] Gtk.Entry txtNick;
    [Glade.Widget] Gtk.Entry txtMaxDown;
    [Glade.Widget] Gtk.Entry txtMaxUp;
    [Glade.Widget] Gtk.Entry txtMaxConnections;
    [Glade.Widget] Gtk.Entry txtMaxSources;
    [Glade.Widget] Gtk.Entry txtTcpPort;
    [Glade.Widget] Gtk.Entry txtUdpPort;
    [Glade.Widget] Gtk.Entry txtIncomingFolder;
    [Glade.Widget] Gtk.Entry txtServerList;

    [Glade.Widget] Gtk.CheckButton chbEnableICH;
    [Glade.Widget] Gtk.CheckButton chbAutoConnect;
    [Glade.Widget] Gtk.CheckButton chbIpFilter;
    [Glade.Widget] Gtk.CheckButton chbServerList;
    [Glade.Widget] Gtk.CheckButton chbMinimize;

    [Glade.Widget] Gtk.Button btnOptAccept;
    [Glade.Widget] Gtk.Button btnOptCancel;
    [Glade.Widget] Gtk.Button btnBrowseIncoming;
    [Glade.Widget] Gtk.Button btnAddTempFolder;
    [Glade.Widget] Gtk.Button btnAddSharedFolder;
    [Glade.Widget] Gtk.Button btnRemoveTempFolder;
    [Glade.Widget] Gtk.Button btnRemoveSharedFolder;

    [Glade.Widget] Gtk.RadioButton radioShareAll;
    [Glade.Widget] Gtk.RadioButton radioShareFriends;
    [Glade.Widget] Gtk.RadioButton radioShareNobody;

    [Glade.Widget] Gtk.TreeView tvSharedFolders;
    [Glade.Widget] Gtk.TreeView tvTempFolders;

    public Hathi.eDonkey.CInterfaceGateway krnGateway;
    public Gtk.TreeStore stSharedFolders;
    public Gtk.TreeStore stTempFolders;

    public OptionsDialog (CInterfaceGateway in_krnGateway)
    {
        Glade.XML gxml = new Glade.XML (null, "elephant.glade", "dlgOptions", null);
        gxml.Autoconnect (this);
        krnGateway=in_krnGateway;
        stSharedFolders = new TreeStore (typeof (string));
        tvSharedFolders.Model = stSharedFolders;
        Gtk.TreeViewColumn tvc = new TreeViewColumn ("Path", new CellRendererText (), "text", 0);
        tvSharedFolders.AppendColumn(tvc);
        stTempFolders = new TreeStore (typeof (string));
        tvTempFolders.Model = stTempFolders;
        tvc = new TreeViewColumn ("Path", new CellRendererText (), "text", 0);
        tvTempFolders.AppendColumn(tvc);
    }

    public void Run ()
    {
        dlgOptions.Icon = new Gdk.Pixbuf (null, "lPhant.png");
        DisplayPreferences (krnGateway.GetConfig());
        dlgOptions.ShowAll ();
    }

    private void on_dlgOptions_delete_event (object o, EventArgs args)
    {
        dlgOptions.Destroy ();
    }

    private void on_btnOptAccept_clicked (object sender, EventArgs args)
    {
        SetPreferences();
        dlgOptions.Destroy ();
    }

    private void on_btnOptCancel_clicked (object sender, EventArgs args)
    {
        dlgOptions.Destroy ();
    }

    private void on_btnBrowseIncoming_clicked (object sender, EventArgs args)
    {
        SelectFolder dlgSelectFolder = new SelectFolder();
        ResponseType result = (ResponseType)dlgSelectFolder.dlgSelectFolder.Run();
        if (result==ResponseType.Ok)
        {
            string folder = dlgSelectFolder.dlgSelectFolder.SelectionText.Text;
            int delimitador = folder.IndexOf (":")+2;
            folder = folder.Substring(delimitador);
            txtIncomingFolder.Text = folder;
        }
        dlgSelectFolder.dlgSelectFolder.Destroy();
    }

    private void on_btnAddTempFolder_clicked (object sender, EventArgs args)
    {
        SelectFolder dlgSelectFolder = new SelectFolder();
        ResponseType result = (ResponseType)dlgSelectFolder.dlgSelectFolder.Run();
        if (result==ResponseType.Ok)
        {
            string folder = dlgSelectFolder.dlgSelectFolder.SelectionText.Text;
            int delimitador = folder.IndexOf (":")+2;
            folder = folder.Substring(delimitador);
            stTempFolders.AppendValues(folder);
        }
        dlgSelectFolder.dlgSelectFolder.Destroy();
    }

    private void on_btnAddSharedFolder_clicked (object sender, EventArgs args)
    {
        SelectFolder dlgSelectFolder = new SelectFolder();
        ResponseType result = (ResponseType)dlgSelectFolder.dlgSelectFolder.Run();
        if (result==ResponseType.Ok)
        {
            string folder = dlgSelectFolder.dlgSelectFolder.SelectionText.Text;
            int delimitador = folder.IndexOf (":")+2;
            folder = folder.Substring(delimitador);
            stSharedFolders.AppendValues(folder);
        }
        dlgSelectFolder.dlgSelectFolder.Destroy();
    }

    private void on_btnRemoveSharedFolder_clicked (object sender, EventArgs args)
    {
        Gtk.TreeIter iter;
        Gtk.TreePath path;
        Gtk.TreeModel model;
        if (tvSharedFolders.Selection.CountSelectedRows() == 0) return;
        tvSharedFolders.Selection.GetSelected (out model, out iter);
        path = model.GetPath (iter);
        stSharedFolders.Remove (ref iter);
    }

    private void on_btnRemoveTempFolder_clicked (object sender, EventArgs args)
    {
        Gtk.TreeIter iter;
        Gtk.TreePath path;
        Gtk.TreeModel model;
        if (tvTempFolders.Selection.CountSelectedRows() == 0) return;
        tvTempFolders.Selection.GetSelected (out model, out iter);
        path = model.GetPath (iter);
        stTempFolders.Remove (ref iter);
    }

    public void DisplayPreferences (InterfacePreferences preferences)
    {
        txtNick.Text = preferences.UserName;
        txtMaxConnections.Text = preferences.maxConnexions.ToString();
        txtMaxDown.Text = preferences.maxDownloadRate.ToString();
        txtMaxUp.Text = preferences.maxUploadRate.ToString();
        txtMaxSources.Text = preferences.maxSources.ToString();
        txtTcpPort.Text = preferences.TCPPort.ToString();
        txtUdpPort.Text = preferences.UDPPort.ToString();
        foreach (string sharedFolder in preferences.sharedFolders)
        {
            if (sharedFolder==preferences.sharedFolders[0])
            {
                txtIncomingFolder.Text = sharedFolder;
            }
            else
            {
                stSharedFolders.AppendValues (sharedFolder);
            }
        }
        foreach (string tmpFolder in preferences.TempFolders)
        {
            stTempFolders.AppendValues (tmpFolder);
        }
        chbEnableICH.Active = preferences.ICHEnabled;
        chbAutoConnect.Active = preferences.Autoreconect;
        chbIpFilter.Active = preferences.IPFilterEnabled;
        radioShareAll.Active = (preferences.AllowViewShared==Constants.AllowViewShared.All);
        radioShareFriends.Active = (preferences.AllowViewShared==Constants.AllowViewShared.Friends);
        radioShareNobody.Active = (preferences.AllowViewShared==Constants.AllowViewShared.Nobody);
        txtServerList.Text = preferences.ServerMetUri;
        chbServerList.Active = preferences.DownloadServerMet;
    }

    private void m_CheckPreferences()
    {
        Random rnd=new Random();
        if (txtTcpPort.Text.Equals("0")) txtTcpPort.Text="4662";
        if (txtUdpPort.Text.Equals("0")) txtUdpPort.Text="4672";
        if (txtTcpPort.Text.Length==0) txtTcpPort.Text="4662";
        if (txtUdpPort.Text.Length==0) txtUdpPort.Text="4672";
        if (txtMaxDown.Text.Length==0) txtMaxDown.Text="50";
        if (txtMaxUp.Text.Length==0) txtMaxUp.Text="16";
        if (txtMaxConnections.Text.Length==0) txtMaxConnections.Text="180";
        if (txtMaxSources.Text.Length==0) txtMaxSources.Text="900";
        if (txtTcpPort.Text.Equals(txtUdpPort.Text)) txtUdpPort.Text=rnd.Next(1000,65535).ToString();
    }

    public void SetPreferences()
    {
        int i, num_rows;
        bool exists_iter;
        Gtk.TreeIter iter;
        Gtk.TreeView tv;
        m_CheckPreferences();
        InterfacePreferences preferences = new InterfacePreferences();
        exists_iter = stSharedFolders.GetIterFirst (out iter);
        num_rows=0;
        /* Get the number of rows */
        while (exists_iter==true)
        {
            exists_iter = stSharedFolders.IterNext (ref iter);
            num_rows++;
        }
        preferences.sharedFolders = new string[num_rows+1];
        preferences.TempFolders = new string[1];
        preferences.sharedFolders[0] = txtIncomingFolder.Text;
        /* Save the shared folders */
        stSharedFolders.GetIterFirst (out iter);
        for (i=1; i<=num_rows; i++)
        {
            preferences.sharedFolders[i] = tvSharedFolders.Model.GetValue(iter,0).ToString();
            stSharedFolders.IterNext (ref iter);
        }
        exists_iter = stTempFolders.GetIterFirst (out iter);
        num_rows=0;
        /* Get the number of rows */
        while (exists_iter==true)
        {
            exists_iter = stTempFolders.IterNext (ref iter);
            num_rows++;
        }
        preferences.TempFolders = new string[num_rows];
        /* Save the temp folders */
        stTempFolders.GetIterFirst (out iter);
        for (i=0; i<num_rows; i++)
        {
            preferences.TempFolders[i] = tvTempFolders.Model.GetValue(iter,0).ToString();
            stTempFolders.IterNext (ref iter);
        }
        preferences.maxConnexions = Convert.ToInt32(txtMaxConnections.Text);
        preferences.maxDownloadRate = (float)Convert.ToDouble(txtMaxDown.Text);
        preferences.maxSources = Convert.ToInt32(txtMaxSources.Text);
        preferences.maxUploadRate = (float)Convert.ToDouble(txtMaxUp.Text);
        preferences.UserName = txtNick.Text;
        preferences.TCPPort = Convert.ToUInt16(txtTcpPort.Text);
        preferences.UDPPort = Convert.ToUInt16(txtUdpPort.Text);
        preferences.ICHEnabled = chbEnableICH.Active;
        preferences.Autoreconect = chbAutoConnect.Active;
        preferences.maxServerFails = 2;
        preferences.DownloadServerMet = chbServerList.Active;
        preferences.ServerMetUri = txtServerList.Text;
        preferences.RemoteControlEnabled=false;
        preferences.RemoteControlPort=Convert.ToInt32("4771");
        preferences.RemoteControlPassword="prueba";
        preferences.Language="en-US";
        preferences.OSigEnabled=true;
        if (radioShareAll.Active) preferences.AllowViewShared=Constants.AllowViewShared.All;
        else if (radioShareFriends.Active) preferences.AllowViewShared=Constants.AllowViewShared.Friends;
        else if (radioShareNobody.Active) preferences.AllowViewShared=Constants.AllowViewShared.Nobody;
        preferences.IPFilterEnabled=chbIpFilter.Active;
        krnGateway.SetConfig(preferences);
    }

}
}