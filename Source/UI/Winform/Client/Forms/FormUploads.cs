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
using System.IO;
using System.Diagnostics;
using Hathi.eDonkey;
using Hathi.Types;
using Hathi.eDonkey.InterfaceGateway;

namespace Hathi.UI.Winform
{
	/// <summary>
	/// Summary description for FormUploads.
	/// </summary>
	public class FormUploads : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private FormAviDetails FAviDetails;
		private System.Windows.Forms.Panel panel1;
		public uploadsListView uploadsList;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		public Hathi.UI.Winform.sharedListView sharedListView;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button buttonReload;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label labelSharedFiles;
		//private Crownwood.Magic.Controls.TabControl tabControlDetails;
		//private Crownwood.Magic.Controls.TabPage tabPageDetails;
		private TabControl tabControlDetails;
		private TabPage tabPageDetails;
		private System.Windows.Forms.TextBox tx_fileName;
		private System.Windows.Forms.TextBox tx_completeName;
		private System.Windows.Forms.Label label6;
		//private Crownwood.Magic.Controls.TabPage tabPagStats;
		private TabPage tabPagStats;
		private System.Windows.Forms.Label labelSessionUploadedRes;
		private System.Windows.Forms.Label labelSessionDownloadedRes;
		private System.Windows.Forms.Label labelSessionRequestsRes;
		private System.Windows.Forms.Label labelSessionUploaded;
		private System.Windows.Forms.Label labelSessionDownloaded;
		private System.Windows.Forms.Label labelSessionRequests;
		private System.Windows.Forms.Panel panel_fileDetails;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelComment;
		private System.Windows.Forms.TextBox textBoxComment;
		private System.Windows.Forms.Button buttonSetCommet;
		private System.Windows.Forms.Button buttonOpenFolder;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonRename;

		private CkernelGateway krnGateway;

		public FormUploads()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Initialize();
			//this.tabControlDetails.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox;
			//this.tabControlDetails.PositionTop = true;
			//tabPageQueue.Visible=false;
			m_Globalize();
		}

		private void m_Globalize()
		{
			labelSharedFiles.Text = HathiForm.Globalization["LBL_SHAREDFILES"];
			buttonReload.Text = HathiForm.Globalization["LBL_RELOADSHAREDFILES"];
			tabPageDetails.Text = HathiForm.Globalization["LBL_DETAILS"];
			//tabPageQueue.Title=HathiForm.Globalization["LBL_QUEUE"];
			label6.Text = HathiForm.Globalization["LBL_PATH"] + ":";
			label5.Text = HathiForm.Globalization["LBL_FILE"] + ":";
			labelSessionDownloaded.Text = HathiForm.Globalization["LBL_DOWNLOADED"];
			labelSessionRequests.Text = HathiForm.Globalization["LBL_REQUESTS"];
			labelSessionUploaded.Text = HathiForm.Globalization["LBL_UPLOADED"];
			labelComment.Text = HathiForm.Globalization["LBL_COMMENT"];
			buttonSetCommet.Text = HathiForm.Globalization["LBL_SETCOMMENT"];
			toolTip1.SetToolTip(this.buttonOpenFolder, HathiForm.Globalization["LBL_OPENFOLDER"]);
			buttonRename.Text = HathiForm.Globalization["LBL_RENAME"];
		}

		public void Globalize()
		{
			m_Globalize();
		}

		private void Initialize()
		{
			FAviDetails = new FormAviDetails();
			FAviDetails.TopLevel = false;
			FAviDetails.Dock = DockStyle.Fill;
			panel_fileDetails.Controls.Add(FAviDetails);
			FAviDetails.Dock = DockStyle.Fill;
			FAviDetails.ApplySkin();
		}

		public void Connect(CkernelGateway in_krnGateway)
		{
			krnGateway = in_krnGateway;
			uploadsList.Initilize(krnGateway);
			sharedListView.Initilize(krnGateway);
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormUploads));
			this.panel1 = new System.Windows.Forms.Panel();
			this.uploadsList = new Hathi.UI.Winform.uploadsListView(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();			
			this.tabControlDetails = new TabControl();
			this.tabPageDetails = new TabPage();
			this.buttonOpenFolder = new System.Windows.Forms.Button();
			this.buttonSetCommet = new System.Windows.Forms.Button();
			this.textBoxComment = new System.Windows.Forms.TextBox();
			this.labelComment = new System.Windows.Forms.Label();
			this.tx_fileName = new System.Windows.Forms.TextBox();
			this.tx_completeName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.panel_fileDetails = new System.Windows.Forms.Panel();
			this.tabPagStats = new TabPage();
			this.labelSessionUploadedRes = new System.Windows.Forms.Label();
			this.labelSessionDownloadedRes = new System.Windows.Forms.Label();
			this.labelSessionRequestsRes = new System.Windows.Forms.Label();
			this.labelSessionUploaded = new System.Windows.Forms.Label();
			this.labelSessionDownloaded = new System.Windows.Forms.Label();
			this.labelSessionRequests = new System.Windows.Forms.Label();
			this.buttonReload = new System.Windows.Forms.Button();
			this.labelSharedFiles = new System.Windows.Forms.Label();
			this.sharedListView = new Hathi.UI.Winform.sharedListView(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.buttonRename = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.tabPageDetails.SuspendLayout();
			this.tabPagStats.SuspendLayout();
			this.SuspendLayout();
			//
			// panel1
			//
			this.panel1.Controls.Add(this.uploadsList);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(776, 96);
			this.panel1.TabIndex = 0;
			//
			// uploadsList
			//
			this.uploadsList.AllowColumnReorder = true;
			this.uploadsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
																 | System.Windows.Forms.AnchorStyles.Left)
																 | System.Windows.Forms.AnchorStyles.Right)));
			this.uploadsList.AutoArrange = false;
			this.uploadsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uploadsList.DefaultCustomDraw = true;
			this.uploadsList.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(69)), ((System.Byte)(151)));
			this.uploadsList.FullRowSelect = true;
			this.uploadsList.FullyCustomHeader = false;
			this.uploadsList.HideSelection = false;
			this.uploadsList.IncreaseHeaderHeight = 0;
			this.uploadsList.Location = new System.Drawing.Point(8, 8);
			this.uploadsList.Name = "uploadsList";
			this.uploadsList.Size = new System.Drawing.Size(760, 88);
			this.uploadsList.TabIndex = 1;
			this.uploadsList.View = System.Windows.Forms.View.Details;
			//
			// splitter1
			//
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 96);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(776, 8);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			//
			// panel2
			//
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 104);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(776, 296);
			this.panel2.TabIndex = 2;
			//
			// panel3
			//
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
														| System.Windows.Forms.AnchorStyles.Left)
														| System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(235)), ((System.Byte)(241)), ((System.Byte)(250)));
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Controls.Add(this.buttonReload);
			this.panel3.Controls.Add(this.labelSharedFiles);
			this.panel3.Controls.Add(this.sharedListView);
			this.panel3.Location = new System.Drawing.Point(8, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(760, 288);
			this.panel3.TabIndex = 3;
			//
			// panel4
			//
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
														| System.Windows.Forms.AnchorStyles.Left)
														| System.Windows.Forms.AnchorStyles.Right)));
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel4.Controls.Add(this.tabControlDetails);
			this.panel4.Location = new System.Drawing.Point(408, 32);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(344, 248);
			this.panel4.TabIndex = 15;
			//
			// tabControlDetails
			//
			this.tabControlDetails.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(235)), ((System.Byte)(241)), ((System.Byte)(250)));
			this.tabControlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			//this.tabControlDetails.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
			this.tabControlDetails.Location = new System.Drawing.Point(0, 0);
			this.tabControlDetails.Name = "tabControlDetails";
			//this.tabControlDetails.PositionTop = true;
			//this.tabControlDetails.SelectedIndex = 0;
			//this.tabControlDetails.SelectedTab = this.tabPageDetails;
			tabPageDetails.Activate();
			this.tabControlDetails.Size = new System.Drawing.Size(342, 246);
			this.tabControlDetails.TabIndex = 0;
			
			this.tabPageDetails.ShowCloseButton(false);
			this.tabPagStats.ShowCloseButton(false);
			this.tabControlDetails.AddRange(new TabPage[] {
            tabPageDetails,
            tabPagStats
        });
			//this.tabControlDetails.SelectionChanged += new System.EventHandler(this.tabControlDetails_SelectionChanged);
			//
			// tabPageDetails
			//
			this.tabPageDetails.Controls.Add(this.buttonRename);
			this.tabPageDetails.Controls.Add(this.buttonOpenFolder);
			this.tabPageDetails.Controls.Add(this.buttonSetCommet);
			this.tabPageDetails.Controls.Add(this.textBoxComment);
			this.tabPageDetails.Controls.Add(this.labelComment);
			this.tabPageDetails.Controls.Add(this.tx_fileName);
			this.tabPageDetails.Controls.Add(this.tx_completeName);
			this.tabPageDetails.Controls.Add(this.label6);
			this.tabPageDetails.Controls.Add(this.label5);
			this.tabPageDetails.Controls.Add(this.panel_fileDetails);
			this.tabPageDetails.Location = new System.Drawing.Point(0, 0);
			this.tabPageDetails.Name = "tabPageDetails";
			this.tabPageDetails.Size = new System.Drawing.Size(342, 221);
			this.tabPageDetails.TabIndex = 0;
			this.tabPageDetails.Text = "Details";
			//
			// buttonOpenFolder
			//
			this.buttonOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpenFolder.Image")));
			this.buttonOpenFolder.Location = new System.Drawing.Point(314, 20);
			this.buttonOpenFolder.Name = "buttonOpenFolder";
			this.buttonOpenFolder.Size = new System.Drawing.Size(29, 19);
			this.buttonOpenFolder.TabIndex = 36;
			this.toolTip1.SetToolTip(this.buttonOpenFolder, "Open folder");
			this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
			//
			// buttonSetCommet
			//
			this.buttonSetCommet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSetCommet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonSetCommet.Location = new System.Drawing.Point(206, 38);
			this.buttonSetCommet.Name = "buttonSetCommet";
			this.buttonSetCommet.Size = new System.Drawing.Size(137, 19);
			this.buttonSetCommet.TabIndex = 34;
			this.buttonSetCommet.Text = "Set my comment";
			this.buttonSetCommet.Click += new System.EventHandler(this.buttonSetCommet_Click);
			//
			// textBoxComment
			//
			this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																		| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxComment.Font = new System.Drawing.Font("Tahoma", 7F);
			this.textBoxComment.Location = new System.Drawing.Point(84, 38);
			this.textBoxComment.Name = "textBoxComment";
			this.textBoxComment.Size = new System.Drawing.Size(123, 20);
			this.textBoxComment.TabIndex = 33;
			this.textBoxComment.Text = "";
			//
			// labelComment
			//
			this.labelComment.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.labelComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelComment.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelComment.Location = new System.Drawing.Point(4, 38);
			this.labelComment.Name = "labelComment";
			this.labelComment.Size = new System.Drawing.Size(81, 19);
			this.labelComment.TabIndex = 32;
			this.labelComment.Text = "Comment:";
			//
			// tx_fileName
			//
			this.tx_fileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																 | System.Windows.Forms.AnchorStyles.Right)));
			this.tx_fileName.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(235)), ((System.Byte)(241)), ((System.Byte)(250)));
			this.tx_fileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tx_fileName.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tx_fileName.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(69)), ((System.Byte)(151)));
			this.tx_fileName.Location = new System.Drawing.Point(84, 2);
			this.tx_fileName.Name = "tx_fileName";
			this.tx_fileName.Size = new System.Drawing.Size(123, 20);
			this.tx_fileName.TabIndex = 30;
			this.tx_fileName.Text = "";
			//
			// tx_completeName
			//
			this.tx_completeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																		 | System.Windows.Forms.AnchorStyles.Right)));
			this.tx_completeName.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(235)), ((System.Byte)(241)), ((System.Byte)(250)));
			this.tx_completeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tx_completeName.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tx_completeName.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(69)), ((System.Byte)(151)));
			this.tx_completeName.Location = new System.Drawing.Point(84, 20);
			this.tx_completeName.Name = "tx_completeName";
			this.tx_completeName.ReadOnly = true;
			this.tx_completeName.Size = new System.Drawing.Size(232, 20);
			this.tx_completeName.TabIndex = 29;
			this.tx_completeName.Text = "";
			//
			// label6
			//
			this.label6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.Black;
			this.label6.Location = new System.Drawing.Point(4, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 19);
			this.label6.TabIndex = 28;
			this.label6.Text = "File:";
			//
			// label5
			//
			this.label5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(4, 2);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(81, 19);
			this.label5.TabIndex = 27;
			this.label5.Text = "File Name:";
			//
			// panel_fileDetails
			//
			this.panel_fileDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
																			 | System.Windows.Forms.AnchorStyles.Right)));
			this.panel_fileDetails.AutoScroll = true;
			this.panel_fileDetails.Font = new System.Drawing.Font("Tahoma", 7F);
			this.panel_fileDetails.Location = new System.Drawing.Point(4, 58);
			this.panel_fileDetails.Name = "panel_fileDetails";
			this.panel_fileDetails.Size = new System.Drawing.Size(379, 171);
			this.panel_fileDetails.TabIndex = 31;
			//
			// tabPagStats
			//
			this.tabPagStats.Controls.Add(this.labelSessionUploadedRes);
			this.tabPagStats.Controls.Add(this.labelSessionDownloadedRes);
			this.tabPagStats.Controls.Add(this.labelSessionRequestsRes);
			this.tabPagStats.Controls.Add(this.labelSessionUploaded);
			this.tabPagStats.Controls.Add(this.labelSessionDownloaded);
			this.tabPagStats.Controls.Add(this.labelSessionRequests);
			this.tabPagStats.Location = new System.Drawing.Point(0, 0);
			this.tabPagStats.Name = "tabPagStats";
			//this.tabPagStats.Selected = false;
			this.tabPagStats.Size = new System.Drawing.Size(342, 221);
			this.tabPagStats.TabIndex = 2;
			this.tabPagStats.Text = "Stats";
			//
			// labelSessionUploadedRes
			//
			this.labelSessionUploadedRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionUploadedRes.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionUploadedRes.Location = new System.Drawing.Point(194, 44);
			this.labelSessionUploadedRes.Name = "labelSessionUploadedRes";
			this.labelSessionUploadedRes.Size = new System.Drawing.Size(123, 19);
			this.labelSessionUploadedRes.TabIndex = 16;
			this.labelSessionUploadedRes.Text = "0";
			//
			// labelSessionDownloadedRes
			//
			this.labelSessionDownloadedRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionDownloadedRes.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionDownloadedRes.Location = new System.Drawing.Point(194, 26);
			this.labelSessionDownloadedRes.Name = "labelSessionDownloadedRes";
			this.labelSessionDownloadedRes.Size = new System.Drawing.Size(123, 19);
			this.labelSessionDownloadedRes.TabIndex = 15;
			this.labelSessionDownloadedRes.Text = "0";
			//
			// labelSessionRequestsRes
			//
			this.labelSessionRequestsRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionRequestsRes.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionRequestsRes.Location = new System.Drawing.Point(194, 8);
			this.labelSessionRequestsRes.Name = "labelSessionRequestsRes";
			this.labelSessionRequestsRes.Size = new System.Drawing.Size(123, 19);
			this.labelSessionRequestsRes.TabIndex = 14;
			this.labelSessionRequestsRes.Text = "0";
			//
			// labelSessionUploaded
			//
			this.labelSessionUploaded.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.labelSessionUploaded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionUploaded.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionUploaded.Location = new System.Drawing.Point(10, 44);
			this.labelSessionUploaded.Name = "labelSessionUploaded";
			this.labelSessionUploaded.Size = new System.Drawing.Size(185, 19);
			this.labelSessionUploaded.TabIndex = 13;
			this.labelSessionUploaded.Text = "Session Uploaded:";
			//
			// labelSessionDownloaded
			//
			this.labelSessionDownloaded.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.labelSessionDownloaded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionDownloaded.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionDownloaded.Location = new System.Drawing.Point(10, 26);
			this.labelSessionDownloaded.Name = "labelSessionDownloaded";
			this.labelSessionDownloaded.Size = new System.Drawing.Size(185, 19);
			this.labelSessionDownloaded.TabIndex = 12;
			this.labelSessionDownloaded.Text = "Session Downloaded:";
			//
			// labelSessionRequests
			//
			this.labelSessionRequests.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.labelSessionRequests.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSessionRequests.Font = new System.Drawing.Font("Tahoma", 7F);
			this.labelSessionRequests.Location = new System.Drawing.Point(10, 8);
			this.labelSessionRequests.Name = "labelSessionRequests";
			this.labelSessionRequests.Size = new System.Drawing.Size(185, 19);
			this.labelSessionRequests.TabIndex = 11;
			this.labelSessionRequests.Text = "Session Requests:";
			//
			// buttonReload
			//
			this.buttonReload.BackColor = System.Drawing.SystemColors.Control;
			this.buttonReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonReload.Location = new System.Drawing.Point(152, 4);
			this.buttonReload.Name = "buttonReload";
			this.buttonReload.Size = new System.Drawing.Size(182, 24);
			this.buttonReload.TabIndex = 14;
			this.buttonReload.Text = "Reload shared list";
			this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
			//
			// labelSharedFiles
			//
			this.labelSharedFiles.Location = new System.Drawing.Point(10, 6);
			this.labelSharedFiles.Name = "labelSharedFiles";
			this.labelSharedFiles.Size = new System.Drawing.Size(140, 20);
			this.labelSharedFiles.TabIndex = 0;
			this.labelSharedFiles.Text = "Shared Files";
			this.labelSharedFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// sharedListView
			//
			this.sharedListView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
			this.sharedListView.AllowColumnReorder = true;
			this.sharedListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
																		| System.Windows.Forms.AnchorStyles.Left)));
			this.sharedListView.AutoArrange = false;
			this.sharedListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sharedListView.DefaultCustomDraw = true;
			this.sharedListView.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(69)), ((System.Byte)(151)));
			this.sharedListView.FullRowSelect = true;
			this.sharedListView.FullyCustomHeader = false;
			this.sharedListView.HideSelection = false;
			this.sharedListView.IncreaseHeaderHeight = 0;
			this.sharedListView.Location = new System.Drawing.Point(8, 32);
			this.sharedListView.Name = "sharedListView";
			this.sharedListView.Size = new System.Drawing.Size(392, 248);
			this.sharedListView.TabIndex = 2;
			this.sharedListView.View = System.Windows.Forms.View.Details;
			this.sharedListView.SelectedIndexChanged += new System.EventHandler(this.sharedListView_SelectedIndexChanged);
			//
			// buttonRename
			//
			this.buttonRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRename.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRename.Location = new System.Drawing.Point(206, 2);
			this.buttonRename.Name = "buttonRename";
			this.buttonRename.Size = new System.Drawing.Size(137, 19);
			this.buttonRename.TabIndex = 37;
			this.buttonRename.Text = "Rename";
			this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
			//
			// FormUploads
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(222)), ((System.Byte)(231)), ((System.Byte)(247)));
			this.ClientSize = new System.Drawing.Size(776, 400);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormUploads";
			this.Text = "FormUploads";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.tabPageDetails.ResumeLayout(false);
			this.tabPagStats.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		private void buttonReload_Click(object sender, System.EventArgs e)
		{
			sharedListView.ReloadList(true);
		}
		public void SaveListsSettings()
		{
			uploadsList.SavePreferences();
			sharedListView.SavePreferences();
		}

		private void m_RefreshStatistics(InterfaceFile file)
		{
			FileStatistics fstatistics = krnGateway.GetFileStatistics(file.strHash);
			labelSessionDownloadedRes.Text = HathiListView.SizeToString((uint)fstatistics.SessionDownload);
			labelSessionUploadedRes.Text = HathiListView.SizeToString((uint)fstatistics.SessionUpload);
			labelSessionRequestsRes.Text = fstatistics.SessionRequests.ToString();
			byte rating = 0;
			string comment = "";
			krnGateway.GetFileComment(file.strHash, ref comment, ref rating);
			this.textBoxComment.Text = comment;
		}
		private void sharedListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sharedListView.SelectedItems.Count > 0)
			{
				m_RefreshStatistics((InterfaceFile)sharedListView.SelectedItems[0].Tag);
				showInfo(((InterfaceFile)sharedListView.SelectedItems[0].Tag));
			}
		}

		private void buttonSetCommet_Click(object sender, System.EventArgs e)
		{
			if ((sharedListView.SelectedItems.Count > 0) && (textBoxComment.Text.Length > 0))
			{
				krnGateway.SetFileComment(((InterfaceFile)sharedListView.SelectedItems[0].Tag).strHash, textBoxComment.Text, 0);
			}
		}

		private void showInfo(InterfaceFile file)
		{
			sFileDetails FileInfo = (sFileDetails)krnGateway.GetFileDetails(file.strHash);
			this.tx_fileName.Text = file.Name;
			this.tx_completeName.Text = file.CompleteName;
			switch ((byte)FileInfo.Type)
			{
				case ((byte)Constants.FileType.Avi):
					FAviDetails.ShowData((Hashtable)FileInfo.ListDetails);
					FAviDetails.Show();
					break;
				default:
					FAviDetails.Hide();
					break;
			}
		}
		public void ApplySkin()
		{
			BackColor = HathiForm.Skin.GetColor("defaultBackColor");
			panel1.BackColor = HathiForm.Skin.GetColor("panelsBackColor");
			panel2.BackColor = HathiForm.Skin.GetColor("panelsBackColor");
			panel3.BackColor = HathiForm.Skin.GetColor("panelsBackColor");
			panel_fileDetails.BackColor = HathiForm.Skin.GetColor("panelsBackColor");
			splitter1.BackColor = HathiForm.Skin.GetColor("SplittersBackColor");
			buttonReload.BackColor = HathiForm.Skin.GetColor("ButtonBackColor");
			buttonReload.ForeColor = HathiForm.Skin.GetColor("ButtonForeColor");
			buttonSetCommet.BackColor = HathiForm.Skin.GetColor("ButtonBackColor");
			buttonSetCommet.ForeColor = HathiForm.Skin.GetColor("ButtonForeColor");
			buttonRename.BackColor = HathiForm.Skin.GetColor("ButtonBackColor");
			buttonRename.ForeColor = HathiForm.Skin.GetColor("ButtonForeColor");
			labelSharedFiles.ForeColor = HathiForm.Skin.GetColor("labelsForeColor");
			labelSharedFiles.BackColor = HathiForm.Skin.GetColor("labelsBackColor");
			sharedListView.ForeColor = HathiForm.Skin.GetColor("listsForeColor");
			sharedListView.BackColor = HathiForm.Skin.GetColor("listsBackColor");
			sharedListView.headerBackColor = HathiForm.Skin.GetColor("listsHeaderBackColor");
			sharedListView.ScrollBarBKColor = HathiForm.Skin.GetColor("listsScrollBarBackColor").ToArgb();
			sharedListView.headerForeColor = HathiForm.Skin.GetColor("listsHeaderForeColor");
			uploadsList.ForeColor = HathiForm.Skin.GetColor("listsForeColor");
			uploadsList.BackColor = HathiForm.Skin.GetColor("listsBackColor");
			uploadsList.headerBackColor = HathiForm.Skin.GetColor("listsHeaderBackColor");
			uploadsList.ScrollBarBKColor = HathiForm.Skin.GetColor("listsScrollBarBackColor").ToArgb();
			uploadsList.headerForeColor = HathiForm.Skin.GetColor("listsHeaderForeColor");
			tabControlDetails.BackColor = HathiForm.Skin.GetColor("panelsBackColor");
			tabControlDetails.ForeColor = HathiForm.Skin.GetColor("labelsForeColor");
			//tabControlDetails.TextInactiveColor = HathiForm.Skin.GetColor("tabsInactiveForeColor");
			label5.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			label5.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			label6.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			label6.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			tx_fileName.BackColor = HathiForm.Skin.GetColor("readOnlyTextBoxBackColor");
			tx_fileName.ForeColor = HathiForm.Skin.GetColor("readOnlyTextBoxForeColor");
			tx_completeName.BackColor = HathiForm.Skin.GetColor("readOnlyTextBoxBackColor");
			tx_completeName.ForeColor = HathiForm.Skin.GetColor("readOnlyTextBoxForeColor");
			textBoxComment.ForeColor = HathiForm.Skin.GetColor("TextBoxForeColor");
			textBoxComment.BackColor = HathiForm.Skin.GetColor("TextBoxBackColor");
			labelSessionRequests.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			labelSessionRequests.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			labelSessionDownloaded.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			labelSessionDownloaded.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			labelSessionUploaded.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			labelSessionUploaded.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			labelComment.ForeColor = HathiForm.Skin.GetColor("SquaredLabelsForeColor");
			labelComment.BackColor = HathiForm.Skin.GetColor("SquaredLabelsBackColor");
			labelSessionRequestsRes.ForeColor = HathiForm.Skin.GetColor("readOnlyTextBoxForeColor");
			labelSessionRequestsRes.BackColor = HathiForm.Skin.GetColor("readOnlyTextBoxBackColor");
			labelSessionDownloadedRes.ForeColor = HathiForm.Skin.GetColor("readOnlyTextBoxForeColor");
			labelSessionDownloadedRes.BackColor = HathiForm.Skin.GetColor("readOnlyTextBoxBackColor");
			labelSessionUploadedRes.ForeColor = HathiForm.Skin.GetColor("readOnlyTextBoxForeColor");
			labelSessionUploadedRes.BackColor = HathiForm.Skin.GetColor("readOnlyTextBoxBackColor");
			FAviDetails.ApplySkin();
			this.Refresh();
		}		

		private void buttonOpenFolder_Click(object sender, System.EventArgs e)
		{
			if (tx_completeName.Text.Length > 0)
			{
				FileInfo finfo = new FileInfo(this.tx_completeName.Text);
				Process.Start(finfo.DirectoryName);
			}
		}

		private void buttonRename_Click(object sender, System.EventArgs e)
		{
			krnGateway.SetFileName(((InterfaceFile)sharedListView.SelectedItems[0].Tag).strHash, tx_fileName.Text);
			sharedListView.SelectedItems[0].Text = tx_fileName.Text;
		}
	}
}
