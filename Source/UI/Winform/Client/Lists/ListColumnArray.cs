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

namespace Hathi.UI.Winform
{
/// <summary>
/// Summary description for ListColumnArray.
/// </summary>
public class ListColumnArray: CollectionBase
{
    public event EventHandler ColumnAdded;
    public event EventHandler ColumnRemoved;
    public ListColumn Add(ListColumn value)
    {
        base.List.Add(value as object);
        if (ColumnAdded != null)
            ColumnAdded(value, new EventArgs());
        return value;
    }
    public void Add(string title, int width,bool subItemOwnerDraw)
    {
        ListColumn column=new ListColumn();
        column.Width=width;
        column.Text=title;
        column.OwnerDraw=true;
        column.subItemOwnerDraw=subItemOwnerDraw;
        Add(column);
    }
    public void AddRange(ListColumn[] values)
    {
        foreach (ListColumn ip in values)
            Add(ip);
    }

    public void Remove(ListColumn value)
    {
        base.List.Remove(value as object);
        if (ColumnRemoved != null)
            ColumnRemoved(value, new EventArgs());
    }

    public void Insert(int index, ListColumn value)
    {
        base.List.Insert(index, value as object);
        if (ColumnAdded != null)
            ColumnAdded(this, new EventArgs());
    }

    public bool Contains(ListColumn value)
    {
        return base.List.Contains(value as object);
    }

    public ListColumn this[int index]
    {
        get {
            return (base.List[index] as ListColumn);
        }
    }

    public int IndexOf(ListColumn value)
    {
        return base.List.IndexOf(value);
    }
}
}
