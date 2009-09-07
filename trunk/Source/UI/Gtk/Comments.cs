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
using Gtk;
using Glade;
using Gdk;
using Hathi.eDonkey;

namespace HathiGTK
{
public class TComments
{
    public Hathi.eDonkey.CInterfaceGateway krnGateway;

    public TComments (Gtk.TreeView tvComments, CInterfaceGateway in_krnGateway)
    {
        krnGateway = in_krnGateway;
        Gtk.TreeViewColumn tvc = new TreeViewColumn ("Name", new CellRendererText(),"text",0);
        tvComments.AppendColumn (tvc);
        tvc.SortColumnId = 0;
        tvc = new TreeViewColumn ("Rating", new CellRendererText(),"text",1);
        tvComments.AppendColumn (tvc);
        tvc.SortColumnId = 1;
        tvc = new TreeViewColumn ("Comment", new CellRendererText(),"text",2);
        tvComments.AppendColumn (tvc);
        tvc.SortColumnId = 2;
    }
}
}