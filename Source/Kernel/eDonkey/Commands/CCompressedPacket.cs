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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hathi.eDonkey.Commands
{
	internal class CCompressedPacket
	{
		public CCompressedPacket(ref MemoryStream packet)
		{
			byte[] compressedBuffer = null;
			BinaryReader reader = new BinaryReader(packet);
			MemoryStream compressedPacket = new MemoryStream();
			packet.Seek(0, SeekOrigin.Begin);
			BinaryWriter writer = new BinaryWriter(compressedPacket);
			writer.Write(reader.ReadBytes(5));
			byte opcode = reader.ReadByte();
			writer.Write(opcode);
			byte[] uncompressedBuffer = reader.ReadBytes((int)packet.Length - 6);
			int compressedsize = CCompressedBlockSend.ComprimirBuffer(uncompressedBuffer, ref compressedBuffer);
			if (compressedsize + 6 >= packet.Length)
				return;
			writer.Write(compressedBuffer);
			compressedPacket.Seek(0, SeekOrigin.Begin);
			DonkeyHeader header = new DonkeyHeader(opcode, writer, Protocol.ProtocolType.Packet);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
			packet.Close();
			packet = compressedPacket;
		}
	}

}
