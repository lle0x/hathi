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

namespace Hathi.eDonkey
{
internal class CGapList : SortedList
{
    private uint filesize;

    public CGapList(uint filesize)
    {
        this.filesize = filesize - 1;
        base.Add(0, this.filesize);
    }

    // add an empty gap
    public void Add(uint start, uint end)
    {
        if (end > filesize) end = filesize;
        for (int n = Count - 1; n >= 0; n--)
        {
            uint gapStart = (uint)GetKey(n); // Keys[n];
            uint gapEnd = (uint)GetByIndex(n); // Values[n];
            if ((gapStart >= start) && (gapEnd <= end))
            {
                // this gap is inside the new gap - delete
                RemoveAt(n);
                continue;
            }
            if ((gapStart >= start) && (gapStart <= end))
            {
                // a part of this gap is in the new gap - extend limit and delete
                end = gapEnd;
                RemoveAt(n);
                continue;
            }
            if ((gapEnd <= end) && (gapEnd >= start))
            {
                // a part of this gap is in the new gap - extend limit and delete
                start = gapStart;
                RemoveAt(n);
                continue;
            }
            if ((start >= gapStart) && (end <= gapEnd))
            {
                // new gap is already inside this gap - return
                return;
            }
        }
        int frontGap = IndexOfValue(start - 1);
        if (frontGap != -1)
        {
            // we had an neighbor in front - merge and delete
            start = (uint)GetKey(frontGap); // Keys[frontGap];
            RemoveAt(frontGap);
        }
        int nextGap = IndexOfKey(end + 1);
        if (nextGap != -1)
        {
            // we had an neighbor at the end - merge and delete
            end = (uint)GetByIndex(nextGap); // Values[nextGap];
            RemoveAt(nextGap);
        }
        // finally add the gap
        base.Add(start, end);
    }

    // substract an gap from list
    public void Fill(uint start, uint end)
    {
        if (end > filesize) end = filesize;
        for (int n = Count - 1; n >= 0; n--)
        {
            uint gapStart = (uint)GetKey(n); // Keys[n];
            uint gapEnd = (uint)GetByIndex(n); // Values[n];
            if ((gapStart >= start) && (gapEnd <= end))
            {
                // our part fills this gap completly
                RemoveAt(n);
            }
            else if ((gapStart >= start) && (gapStart <= end))
            {
                // a part of this gap is in the part - set limit
                RemoveAt(n);
                base.Add(end + 1, gapEnd);
            }
            else if ((gapEnd <= end) && (gapEnd >= start))
            {
                // a part of this gap is in the part - set limit
                RemoveAt(n);
                base.Add(gapStart, start - 1);
            }
            else if ((start >= gapStart) && (end <= gapEnd))
            {
                RemoveAt(n);
                base.Add(gapStart, start - 1);
                base.Add(end + 1, gapEnd);
                break;
            }
        }
    }

    public uint RemainingBytes()
    {
        uint bytes = 0;
        for (int i = 0; i != Count; i++)
        {
            bytes += ((uint)GetByIndex(i) - (uint)GetKey(i) + 1);
        }
        return bytes;
    }
}
}
