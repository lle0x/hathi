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
using Egg;
using Hathi.eDonkey;

namespace HathiGTK
{
public class TIcon
{
    private Gtk.Menu menu;
    private TrayIcon t;
    private Hathi.eDonkey.CInterfaceGateway krnGateway;
    private Gtk.Window mainwindow;

    public TIcon (CInterfaceGateway in_krnGateway, Gtk.Window mwindow)
    {
        krnGateway = in_krnGateway;
        mainwindow = mwindow;
        menu = new Gtk.Menu ();
        EventBox eb = new EventBox ();
        eb.ButtonPressEvent += new ButtonPressEventHandler (TIconClicked);
        eb.Add (new Gtk.Image (new Gdk.Pixbuf (null, "lPhant.png")));
        MenuItem it_show = new MenuItem ("Show");
        it_show.Activated += new EventHandler (TIconShow);
        MenuItem it_options = new MenuItem ("Options");
        it_options.Activated += new EventHandler (TIconOptions);
        ImageMenuItem it_quit = new ImageMenuItem("Quit");
        it_quit.Activated += new EventHandler (TIconQuit);
        menu.Append (it_show);
        menu.Append (it_options);
        menu.Append (it_quit);
        t = new TrayIcon ("HathiGTK");
        t.Add (eb);
        t.ShowAll ();
    }

    private void TIconClicked (object sender, ButtonPressEventArgs args)
    {
        Gdk.EventButton eb = args.Event;
        if (eb.Button == 3)
        {
            menu.ShowAll ();
            menu.Popup (null, null, null, IntPtr.Zero, args.Event.Button, args.Event.Time);
        }
    }

    private void TIconQuit (object sender, EventArgs args)
    {
        krnGateway.CloseKernel ();
        Application.Quit ();
    }

    private void TIconShow (object sender, EventArgs args)
    {
        mainwindow.Show();
    }

    private void TIconOptions (object sender, EventArgs args)
    {
        OptionsDialog options = new OptionsDialog (krnGateway);
        options.Run();
    }
}
}