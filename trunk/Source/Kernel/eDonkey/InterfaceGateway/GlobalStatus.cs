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
using System.Net;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
//using System.Runtime.Remoting.Lifetime;

using Hathi.Types;
using Hathi.Classes;

namespace Hathi.eDonkey.InterfaceGateway
{
	[Serializable]
	public class GlobalStatus
	{
		public string ServerName;
		public ushort ActiveConnections;
		public uint UserID;
		public bool IsHighID;
		public uint ServerFiles;
		public uint ServerUsers;
		public float DowloadSpeed;
		public float UploadSpeed;
		public float AvgUpSpeed;
		public float AvgDownSpeed;
		public ulong SessionUpload;
		public ulong SessionDownload;
	}
}