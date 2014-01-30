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
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Hathi.UI.Winform
{
/// <summary>
/// Cache of the system icons associated to file extensions
/// </summary>
public class CSystemIconsList
{
    public ImageList list;
    private Hashtable m_table;
    public CSystemIconsList()
    {
        list=new ImageList();
        m_table=new Hashtable();
    }
    public int GetIconIndexOf(string filename)
    {
        string fileExtension=CUtils.GetExtension(filename);
        //patch that fixes a crash on search with .mdf and .mds results if Alcohol 120% is sinstalled
        if (fileExtension==".mds" || fileExtension==".mdf")
        {
            filename+=".iso";
            fileExtension=".iso";
        }
        if (m_table[fileExtension]!=null) return (int)m_table[fileExtension];
        Win32.SHFILEINFO shinfo = new Win32.SHFILEINFO();
        IntPtr hImgSmall; //the handle to the system image list
        System.Drawing.Icon myIcon;
        try
        {
            hImgSmall = Win32.SHGetFileInfo(filename, Win32.FILE_ATTRIBUTE_NORMAL, ref shinfo,(uint)Marshal.SizeOf(shinfo),
                                            Win32.SHGFI_USEFILEATTRIBUTES | Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }
        catch
        {
            return -1;
        }
        list.Images.Add(myIcon);
        m_table.Add(fileExtension,list.Images.Count-1);
        return list.Images.Count-1;
    }
    public Image GetIconImageOf(string filename)
    {
        int index=GetIconIndexOf(filename);
        return list.Images[index];
    }
}
}
