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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using Hathi.Classes;

namespace Hathi.UI.Winform
{
/// <summary>
/// Starting form
/// </summary>
public class StartForm : System.Windows.Forms.Form
{
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton HathiLocal;
    private System.Windows.Forms.RadioButton HathiService;
    private System.Windows.Forms.RadioButton RemoteConnector;
    private System.Windows.Forms.TextBox RemoteIP;
    private System.Windows.Forms.Label IP;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox RemoteIP04;
    private System.Windows.Forms.TextBox RemoteIP03;
    private System.Windows.Forms.TextBox RemoteIP02;
    private System.Windows.Forms.TextBox pw;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox RemotePort;
    private CedonkeyCRemote cRemote;

    private System.ComponentModel.Container components = null;

    private static Mutex m_Mutex;
    private static string m_elink;

    public StartForm(string in_elink,Mutex in_mutex)
    {
        m_Mutex=in_mutex;
        m_elink=in_elink;
        InitializeComponent();
        this.RemotePort.Enabled=false;
        this.pw.Enabled=false;
        this.RemoteIP.Enabled=false;
        this.RemoteIP02.Enabled=false;
        this.RemoteIP03.Enabled=false;
        this.RemoteIP04.Enabled=false;
    }

    /// <summary>
    /// Clean system resources
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

    #region Windows Form Designer Generated Code
    /// <summary>
    /// This is necessary to support the designer, do not modify the contents below
    /// </summary>
    private void InitializeComponent()
    {
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.RemotePort = new System.Windows.Forms.TextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.pw = new System.Windows.Forms.TextBox();
        this.label5 = new System.Windows.Forms.Label();
        this.RemoteIP04 = new System.Windows.Forms.TextBox();
        this.RemoteIP03 = new System.Windows.Forms.TextBox();
        this.RemoteIP02 = new System.Windows.Forms.TextBox();
        this.IP = new System.Windows.Forms.Label();
        this.RemoteIP = new System.Windows.Forms.TextBox();
        this.RemoteConnector = new System.Windows.Forms.RadioButton();
        this.HathiService = new System.Windows.Forms.RadioButton();
        this.HathiLocal = new System.Windows.Forms.RadioButton();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.groupBox1.SuspendLayout();
        this.SuspendLayout();
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.RemotePort);
        this.groupBox1.Controls.Add(this.label6);
        this.groupBox1.Controls.Add(this.pw);
        this.groupBox1.Controls.Add(this.label5);
        this.groupBox1.Controls.Add(this.RemoteIP04);
        this.groupBox1.Controls.Add(this.RemoteIP03);
        this.groupBox1.Controls.Add(this.RemoteIP02);
        this.groupBox1.Controls.Add(this.IP);
        this.groupBox1.Controls.Add(this.RemoteIP);
        this.groupBox1.Controls.Add(this.RemoteConnector);
        this.groupBox1.Controls.Add(this.HathiService);
        this.groupBox1.Controls.Add(this.HathiLocal);
        this.groupBox1.Location = new System.Drawing.Point(8, 8);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(328, 184);
        this.groupBox1.TabIndex = 1;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Interface connection:";
        // 
        // RemotePort
        // 
        this.RemotePort.Location = new System.Drawing.Point(184, 128);
        this.RemotePort.Name = "RemotePort";
        this.RemotePort.Size = new System.Drawing.Size(48, 20);
        this.RemotePort.TabIndex = 9;
        // 
        // label6
        // 
        this.label6.Location = new System.Drawing.Point(40, 128);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(88, 16);
        this.label6.TabIndex = 16;
        this.label6.Text = "Remote port:";
        // 
        // pw
        // 
        this.pw.Location = new System.Drawing.Point(112, 152);
        this.pw.Name = "pw";
        this.pw.PasswordChar = '*';
        this.pw.Size = new System.Drawing.Size(120, 20);
        this.pw.TabIndex = 10;
        // 
        // label5
        // 
        this.label5.Location = new System.Drawing.Point(40, 160);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(64, 16);
        this.label5.TabIndex = 14;
        this.label5.Text = "Password:";
        // 
        // RemoteIP04
        // 
        this.RemoteIP04.Location = new System.Drawing.Point(208, 104);
        this.RemoteIP04.Name = "RemoteIP04";
        this.RemoteIP04.Size = new System.Drawing.Size(24, 20);
        this.RemoteIP04.TabIndex = 7;
        // 
        // RemoteIP03
        // 
        this.RemoteIP03.Location = new System.Drawing.Point(176, 104);
        this.RemoteIP03.Name = "RemoteIP03";
        this.RemoteIP03.Size = new System.Drawing.Size(24, 20);
        this.RemoteIP03.TabIndex = 6;
        // 
        // RemoteIP02
        // 
        this.RemoteIP02.Location = new System.Drawing.Point(144, 104);
        this.RemoteIP02.Name = "RemoteIP02";
        this.RemoteIP02.Size = new System.Drawing.Size(24, 20);
        this.RemoteIP02.TabIndex = 5;
        // 
        // IP
        // 
        this.IP.Location = new System.Drawing.Point(40, 112);
        this.IP.Name = "IP";
        this.IP.Size = new System.Drawing.Size(16, 16);
        this.IP.TabIndex = 5;
        this.IP.Text = "IP";
        // 
        // RemoteIP
        // 
        this.RemoteIP.Location = new System.Drawing.Point(112, 104);
        this.RemoteIP.Name = "RemoteIP";
        this.RemoteIP.Size = new System.Drawing.Size(24, 20);
        this.RemoteIP.TabIndex = 4;
        // 
        // RemoteConnector
        // 
        this.RemoteConnector.Location = new System.Drawing.Point(24, 80);
        this.RemoteConnector.Name = "RemoteConnector";
        this.RemoteConnector.Size = new System.Drawing.Size(264, 16);
        this.RemoteConnector.TabIndex = 3;
        this.RemoteConnector.Text = "Remote (Connect to a remote Hathi)";
        this.RemoteConnector.CheckedChanged += new System.EventHandler(this.RemoteConnector_CheckedChanged);
        // 
        // HathiService
        // 
        this.HathiService.Enabled = false;
        this.HathiService.Location = new System.Drawing.Point(24, 56);
        this.HathiService.Name = "HathiService";
        this.HathiService.Size = new System.Drawing.Size(184, 16);
        this.HathiService.TabIndex = 2;
        this.HathiService.Text = "Local (Start Hathi service";
        // 
        // HathiLocal
        // 
        this.HathiLocal.Checked = true;
        this.HathiLocal.Location = new System.Drawing.Point(24, 32);
        this.HathiLocal.Name = "HathiLocal";
        this.HathiLocal.Size = new System.Drawing.Size(184, 16);
        this.HathiLocal.TabIndex = 1;
        this.HathiLocal.TabStop = true;
        this.HathiLocal.Text = "Local (Start Hathi).";
        this.HathiLocal.CheckedChanged += new System.EventHandler(this.HathiLocal_CheckedChanged);
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(144, 200);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(88, 24);
        this.button1.TabIndex = 11;
        this.button1.Text = "OK";
        this.button1.Click += new System.EventHandler(this.Accept_OnClick);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(240, 200);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(96, 24);
        this.button2.TabIndex = 12;
        this.button2.Text = "Cancel";
        this.button2.Click += new System.EventHandler(this.Cancel_OnClick);
        // 
        // StartForm
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(344, 226);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.groupBox1);
        this.Name = "StartForm";
        this.Text = "Start Hathi";
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.ResumeLayout(false);

    }
    #endregion

    private void Cancel_OnClick(object sender, System.EventArgs e)
    {
        Application.Exit();
    }

    private void Accept_OnClick(object sender, System.EventArgs e)
    {
        string IP=RemoteIP.Text + "." + RemoteIP02.Text + "." + RemoteIP03.Text + "." + RemoteIP04.Text;
        if (this.RemoteConnector.Checked)
        {
            if (cRemote==null)
                cRemote=new CedonkeyCRemote();
            int RemotePort;
            if (this.RemotePort.Text=="")
                RemotePort=4670;
            else
                RemotePort=System.Convert.ToInt32(this.RemotePort.Text);
            cRemote.DisConnect();
            if (cRemote.Connect(IP,pw.Text,RemotePort))
            {
                Hathi.UI.Winform.HathiForm FormHathi=new HathiForm(m_elink,m_Mutex,cRemote.remoteInterface);
                FormHathi.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Can not connect or invalid password","Hathi remote connection",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        if (this.HathiLocal.Checked)
        {
            Hathi.UI.Winform.HathiForm FormHathi=new HathiForm(m_elink,m_Mutex);
            this.Hide();
            FormHathi.Show();
        }
    }

    private void HathiLocal_CheckedChanged(object sender, System.EventArgs e)
    {
        if (this.HathiLocal.Checked)
        {
            this.RemotePort.Enabled=false;
            this.pw.Enabled=false;
            this.RemoteIP.Enabled=false;
            this.RemoteIP02.Enabled=false;
            this.RemoteIP03.Enabled=false;
            this.RemoteIP04.Enabled=false;
        }
    }

    private void RemoteConnector_CheckedChanged(object sender, System.EventArgs e)
    {
        if (this.RemoteConnector.Checked)
        {
            this.RemotePort.Enabled=true;
            this.pw.Enabled=true;
            this.RemoteIP.Enabled=true;
            this.RemoteIP02.Enabled=true;
            this.RemoteIP03.Enabled=true;
            this.RemoteIP04.Enabled=true;
        }
    }
}
}
