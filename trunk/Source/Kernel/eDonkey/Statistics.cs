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

namespace Hathi.eDonkey
{
/// <summary>
/// Summary description for CStatistics.
/// </summary>
public class CStatistics
{
    private ulong m_SessionDown;
    private ulong m_SessionUp;
    private DateTime m_StartTime;

    public ulong GetSesionDown() {
        return m_SessionDown;
    }
    public ulong GetSessionUp() {
        return m_SessionUp;
    }

    public float GetAvgDown()
    {
        TimeSpan dif=DateTime.Now-m_StartTime;
        return (float)(m_SessionDown/1024F)/((float)dif.TotalSeconds+1);
    }

    public float GetAvgUp()
    {
        TimeSpan dif=DateTime.Now-m_StartTime;
        return (float)(m_SessionUp/1024F)/((float)dif.TotalSeconds+1);
    }

    public CStatistics()
    {
        m_StartTime=DateTime.Now;
        m_SessionDown=0;
        m_SessionDown=0;
        m_SessionUp=0;
    }

    public void IncSessionDown(int nbytes)
    {
        m_SessionDown+=(ulong)nbytes;
    }

    public void IncSessionUp(int nbytes)
    {
        m_SessionUp+=(ulong)nbytes;
    }
}
}