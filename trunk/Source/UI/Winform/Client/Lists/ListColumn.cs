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
using System.Windows.Forms;

namespace Hathi.UI.Winform
{
/// <summary>
/// Summary description for ListColumn.
/// </summary>
public class ListColumn
{
    private System.ComponentModel.Container components = null;
    int imageIndex;
    bool ownerDraw;
    bool imageOnRight;
    bool m_subitemOwnerDraw;
    HorizontalAlignment textAlign;
    int width;
    string text;

    public ListColumn()
    {
        InitializeComponent();
        //Default values
        imageIndex = -1;
        subItemOwnerDraw=false;
        ownerDraw = false;
        imageOnRight = true;
        textAlign = HorizontalAlignment.Left;
        width = 60;
        text = "ColumnHeader";
    }

    #region Public Properties

    public bool subItemOwnerDraw
    {
        get {
            return m_subitemOwnerDraw;
        }
        set {
            m_subitemOwnerDraw=value;
        }
    }
    public int ImageIndex
    {
        get {
            return imageIndex;
        }
        set {
            imageIndex = value;
        }
    }

    public bool OwnerDraw
    {
        get {
            return ownerDraw;
        }
        set {
            ownerDraw = value;
        }
    }

    public bool ImageOnRight
    {
        get {
            return imageOnRight;
        }
        set {
            imageOnRight = value;
        }
    }

    public string Text
    {
        get {
            return text;
        }
        set {
            text = value;
        }
    }

    public HorizontalAlignment TextAlign
    {
        get {
            return textAlign;
        }
        set {
            textAlign = value;
        }
    }

    public int Width
    {
        get {
            return width;
        }
        set {
            width = value;
        }
    }

    #endregion

    #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
    }
    #endregion

}
}
