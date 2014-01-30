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
using System.Xml;
using System.Windows.Forms;
using System.IO;
using Hathi.Types;

namespace Hathi.eDonkey
{
/// <summary>
/// Class for store file Category information
/// </summary>
public class CCategory
{
    private string m_Name;
    private uint m_Color;
    private uint m_ID;
    private string m_AutoString;

    public string Name
    {
        get
        {
            return m_Name;
        }
        set
        {
            m_Name=value;
        }
    }

    public string AutoString
    {
        get
        {
            return m_AutoString;
        }
        set
        {
            m_AutoString=value;
        }
    }

    public uint Color
    {
        get
        {
            return m_Color;
        }
        set
        {
            m_Color=value;
        }
    }

    public uint ID
    {
        get
        {
            return m_ID;
        }
        set
        {
            m_ID=value;
        }
    }

    public CCategory()
    {
    }
}

}
