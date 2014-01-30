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
using System.Threading;

namespace Hathi.UI.Winform
{
/// <summary>
/// Summary description for FormAbout.
/// </summary>
public class FormAbout : System.Windows.Forms.Form
{
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Timer timer1;
    private Hathi.UI.Winform.Controls.ScrollingCredits scrollingCredits;
    private System.ComponentModel.IContainer components;
    private double m_dblOpacityIncrement = .1;
    private double m_dblOpacityDecrement = .1;
    private const int TIMER_INTERVAL = 50;

    public FormAbout()
    {
        //
        // Required for Windows Form Designer support
        //
        InitializeComponent();
        Opacity = .0;
        timer1.Interval = TIMER_INTERVAL;
        timer1.Start();
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
        this.label5 = new System.Windows.Forms.Label();
        this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.scrollingCredits = new Hathi.UI.Winform.Controls.ScrollingCredits();
        this.SuspendLayout();
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.BackColor = System.Drawing.Color.Transparent;
        this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label5.ForeColor = System.Drawing.Color.White;
        this.label5.Location = new System.Drawing.Point(13, 233);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(149, 13);
        this.label5.TabIndex = 4;
        this.label5.Text = "Copyright (C)2009 Hathi Team";
        this.label5.Click += new System.EventHandler(this.FormAbout_Click);
        // 
        // linkLabel1
        // 
        this.linkLabel1.ActiveLinkColor = System.Drawing.Color.White;
        this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
        this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Transparent;
        this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.linkLabel1.ForeColor = System.Drawing.Color.White;
        this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
        this.linkLabel1.LinkColor = System.Drawing.Color.White;
        this.linkLabel1.Location = new System.Drawing.Point(11, 9);
        this.linkLabel1.Name = "linkLabel1";
        this.linkLabel1.Size = new System.Drawing.Size(300, 26);
        this.linkLabel1.TabIndex = 5;
        this.linkLabel1.TabStop = true;
        this.linkLabel1.Text = "http://hathi.sourceforge.net";
        this.linkLabel1.VisitedLinkColor = System.Drawing.Color.White;
        this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        // 
        // timer1
        // 
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // scrollingCredits
        // 
        this.scrollingCredits.BackColor = System.Drawing.Color.Transparent;
        this.scrollingCredits.Location = new System.Drawing.Point(-7, 62);
        this.scrollingCredits.Name = "scrollingCredits";
        this.scrollingCredits.Size = new System.Drawing.Size(355, 158);
        this.scrollingCredits.TabIndex = 0;
        this.scrollingCredits.Text = "scrollingCredits";
        this.scrollingCredits.Click += new System.EventHandler(this.scrollingCredits_Click);
        // 
        // FormAbout
        // 
        this.BackColor = System.Drawing.Color.Gray;
        this.ClientSize = new System.Drawing.Size(349, 258);
        this.Controls.Add(this.linkLabel1);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.scrollingCredits);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FormAbout";
        this.Opacity = 0;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "About Hathi";
        this.TransparencyKey = System.Drawing.Color.Transparent;
        this.Load += new System.EventHandler(this.FormAbout_Load);
        this.Click += new System.EventHandler(this.FormAbout_Click);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
    #endregion

    private void FormAbout_Click(object sender, System.EventArgs e)
    {
        m_dblOpacityIncrement = -m_dblOpacityDecrement;
    }

    private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
        System.Diagnostics.Process.Start("http://hathi.sourceforge.net");
    }

    private void FormAbout_Load(object sender, System.EventArgs e)
    {
    }

    private void timer1_Tick(object sender, System.EventArgs e)
    {
        if ( m_dblOpacityIncrement > 0 )
        {
            if ( this.Opacity < 1 )
                this.Opacity += m_dblOpacityIncrement;
        }
        else
        {
            if ( this.Opacity > 0 )
                this.Opacity += m_dblOpacityIncrement;
            else
                this.Close();
        }
    }

    private void scrollingCredits_Click(object sender, System.EventArgs e)
    {
        m_dblOpacityIncrement = -m_dblOpacityDecrement;
    }
}
}
