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
using System.IO;
using System.Collections;
using System.Text;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Hathi.Types;
using Hathi.eDonkey.Commands;

namespace Hathi.eDonkey.CommandsServer
{
	class CServerAskSourcesUDP
	{
		public CServerAskSourcesUDP(byte[] FileHash, MemoryStream buffer)
		{
			BinaryWriter writer = new BinaryWriter(buffer);
			writer.Write((byte)Protocol.Header.eDonkey);
			writer.Write((byte)Protocol.ServerCommandUDP.GlobalGetSources);
			writer.Write(FileHash);
		}

		public CServerAskSourcesUDP(ArrayList FilesHash, MemoryStream buffer)
		{
			BinaryWriter writer = new BinaryWriter(buffer);
			writer.Write((byte)Protocol.Header.eDonkey);
			writer.Write((byte)Protocol.ServerCommandUDP.GlobalGetSources);
			foreach (byte[] fileHash in FilesHash)
			{
				writer.Write(fileHash);
			}
		}
	}
}
