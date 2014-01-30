using System;
using System.IO;
using System.Collections;
using System.Text;
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

using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Hathi.Types;
using Hathi.eDonkey.Commands;

namespace Hathi.eDonkey.CommandsServer
{

	internal class CServerHello
	{
		public CServerHello(MemoryStream buffer)
		{
			DonkeyHeader header;
			BinaryWriter writer = new BinaryWriter(buffer);
			header = new DonkeyHeader((byte)Protocol.ServerCommand.LoginRequest, writer);
			writer.Write(CKernel.Preferences.GetByteArray("UserHash"));
			writer.Write(CKernel.Preferences.GetUInt("ID"));
			writer.Write(CKernel.Preferences.GetUShort("TCPPort"));
			uint nParameters = 5;
			writer.Write(nParameters);
			// user
			new ParameterWriter((byte)Protocol.ClientParameter.Name, CKernel.Preferences.GetString("UserName"), writer);
			// version
			new ParameterWriter((byte)Protocol.ClientParameter.Version, Protocol.EDONKEYVERSION, writer);
			// port
			new ParameterWriter((byte)Protocol.ClientParameter.Port, (uint)CKernel.Preferences.GetUShort("TCPPort"), writer);
			// compression
			new ParameterWriter((byte)Protocol.ClientParameter.Compression, 5, writer); //1 (supports compression) + 5 (supports alternative server ports)
			// emule version
			new ParameterWriter((byte)Protocol.ClientParameter.EmuleVersion, Protocol.EMULE_VERSION_COMPLEX, writer);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}
	}
}
