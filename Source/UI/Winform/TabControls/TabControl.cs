using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Hathi.UI.Winform
{
	/// <summary>
	/// The Tab Control using the docking panels
	/// </summary>
	public partial class TabControl : UserControl
	{
		private DockPanel mDockPanel = new DockPanel();
		private TabPage mActiveTabPage;
		private List<TabPage> mTabPages = new List<TabPage>();

		public event EventHandler ClosePressed;
		public event EventHandler SelectionChanged;

		public IEnumerable<TabPage> TabPages
		{
			get { return mTabPages; }
		}

		public int Count
		{
			get { return mTabPages.Count; }
		}

		/// <summary>
		/// Gets the active tab page.
		/// </summary>
		/// <value>The active tab page.</value>
		public TabPage ActiveTabPage
		{
			get { return mActiveTabPage; }
		}

		/// <summary>
		/// Gets the index of the Active Tab page.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get
			{
				for (int i = 0; i < mTabPages.Count; i++)
				{
					if (mTabPages[i].Equals(mActiveTabPage))
						return i;
				}
				return -1;
			}
			set
			{
				mActiveTabPage = mTabPages[value];
				FireSelectionChanged();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TabControl"/> class.
		/// </summary>
		public TabControl()
		{
			InitializeComponent();
			mDockPanel.Dock = DockStyle.Fill;
			mDockPanel.DocumentStyle = DocumentStyle.DockingWindow;
			this.Controls.Add(mDockPanel);
		}
		/// <summary>
		/// Adds the specified tab page.
		/// </summary>
		/// <param name="tabPage">The tab page.</param>
		public void Add(TabPage tabPage)
		{
			tabPage.Enter += SetActiveTabPage;
			tabPage.Activated += SetActiveTabPage;
			tabPage.FormClosed += RemoveTabPage;
			tabPage.Show(mDockPanel);
			mTabPages.Add(tabPage);
			mActiveTabPage = tabPage;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="tabPages">The tab pages.</param>
		public void AddRange(TabPage[] tabPages)
		{
			foreach (TabPage tabPage in tabPages)
			{
				Add(tabPage);
			}
		}

		/// <summary>
		/// Gets the tab page at a specified index
		/// </summary>
		/// <param name="index">The index.</param>
		public TabPage GetTabPageAt(int index)
		{
			return mTabPages[index];
		}

		private void RemoveTabPage(object sender, FormClosedEventArgs e)
		{
			if (ClosePressed != null)
				ClosePressed(this, EventArgs.Empty);

			mTabPages.Remove(sender as TabPage);
			if (mTabPages.Count > 0)
			{
				mActiveTabPage = mTabPages[mTabPages.Count - 1];
				mActiveTabPage.Activate();
			}
			else
				mActiveTabPage = null;
			FireSelectionChanged();
		}

		private void SetActiveTabPage(object sender, EventArgs e)
		{
			mActiveTabPage = sender as TabPage;
			FireSelectionChanged();
		}
		private void FireSelectionChanged()
		{
			if (SelectionChanged != null)
				SelectionChanged(this, EventArgs.Empty);
		}
	}

}
