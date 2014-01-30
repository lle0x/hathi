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
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Hathi.eDonkey
{
/// <summary>
/// debug only class
/// </summary>
[Serializable]
public class CLogClient:StringCollection
{
    private FileStream m_File;
    private BinaryWriter m_writer;

    public CLogClient()
    {
        m_File=new FileStream(Application.StartupPath + "\\log.txt",FileMode.Create,FileAccess.Write,FileShare.Read);
        m_writer=new BinaryWriter(m_File);
    }

    ~CLogClient()
    {
        m_File.Close();
    }

    public void AddLog(string logstring)
    {
        m_writer.Write(DateTime.Now.ToString()+":"+logstring+"\r\n");
    }

    public void AddLog(string logstring,uint clientID)
    {
        m_writer.Write(DateTime.Now.ToString()+":"+clientID+" : "+logstring+"\r\n");
    }
}
}