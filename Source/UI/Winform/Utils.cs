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

namespace Hathi.UI.Winform
{
/// <summary>
/// General interface static functions should go here
/// </summary>
public class CUtils
{
    public CUtils()
    {
    }
    static public bool IsVideo(string fileName)
    {
        string fileExtension=GetExtension(fileName);
        string videoExtensions=".asf.avi.divx.m1v.m2v.mkv.mov.mp1v.mp2v.mpe.mpeg.mpg.mps.mpv.mpv1.mpv2.ogm.qt.ram.rm.rv.vivo.vob.wmv.rv9";
        return (videoExtensions.IndexOf(fileExtension)>0);
    }
    static public bool IsAudio(string fileName)
    {
        string fileExtension=GetExtension(fileName);
        string audioExtensions=".669.aac.aif.aiff.amf.ams.ape.au.dbm.dmf.dsm.far.flac.it.mdl.med.mid.midi.mod.mol.mp1.mp2.mp3.mp4.mpa.mpc.mpp.mtm.nst.ogg.okt.psm.ptm.ra.rmi.s3m.stm.ult.umx.wav.wma.wow.xm";
        return (audioExtensions.IndexOf(fileExtension)>0);
    }
    static public bool IsFile(string fileName)
    {
        return (!IsAudio(fileName)&&(!IsVideo(fileName)));
    }
    static public string GetExtension(string fileName)
    {
        //dont use Path.GetExtension to avoid exceptions if fileName contains invalid characters
        int location=fileName.LastIndexOf(".");
        string fileExtension="";
        if (location>0)
            fileExtension=fileName.Substring(location);
        return fileExtension.ToLower();
    }
}
public class CFilterSummary
{
    public ulong TotalSize;
    public uint Items;
}
}
