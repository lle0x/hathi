using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Hathi.UI.Winform
{
	/// <summary>
	/// This is a control deriving from DockContent and acts like a tab page
	/// </summary>
	public class TabPage : DockContent
	{
		public TabPage()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TabPage"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public TabPage(string text)
		{
			this.Text = text;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TabPage"/> class.
		/// </summary>
		/// <param name="Text">The text.</param>
		/// <param name="control">The control.</param>
		public TabPage(string Text, Control control)
		{
			control.Dock = DockStyle.Fill;
			this.Controls.Add(control);
		}
		/// <summary>
		/// Shows the close button.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public void ShowCloseButton(bool value)
		{
			this.CloseButtonVisible = value;
		}
		/// <summary>
		/// Gets the control.
		/// </summary>
		/// <value>The control.</value>
		public Control Control
		{
			get
			{
				if (Controls.Count > 0)
					return
						this.Controls[0];
				else
					return null;
			}
		}
	}
}
