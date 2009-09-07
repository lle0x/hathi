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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Hathi.eDonkey.Commands
{
	internal class CFileBlockRequest
	{
		public ArrayList RequestedBlocks;

		public CFileBlockRequest(MemoryStream buffer)
		{
			byte[] FileHash;
			if (buffer.Length != 40) return;
			CFileBlock block1 = new CFileBlock();
			CFileBlock block2 = new CFileBlock();
			CFileBlock block3 = new CFileBlock();
			RequestedBlocks = new ArrayList();
			BinaryReader reader = new BinaryReader(buffer);
			//FileHash=new Byte[16];
			FileHash = reader.ReadBytes(16);
			block1.start = reader.ReadUInt32();
			block2.start = reader.ReadUInt32();
			block3.start = reader.ReadUInt32();
			block1.end = reader.ReadUInt32();
			block2.end = reader.ReadUInt32();
			block3.end = reader.ReadUInt32();
			block1.FileHash = FileHash;
			block2.FileHash = FileHash;
			block3.FileHash = FileHash;
			if (block1.end > block1.start) RequestedBlocks.Add(block1);
			if (block2.end > block2.start) RequestedBlocks.Add(block2);
			if (block3.end > block3.start) RequestedBlocks.Add(block3);
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}

		public CFileBlockRequest(MemoryStream buffer, byte[] FileHash, ref CFileBlock block1, ref CFileBlock block2, ref CFileBlock block3)
		{
			BinaryWriter writer = new BinaryWriter(buffer);
			DonkeyHeader header = new DonkeyHeader((byte)Protocol.ClientCommand.RequestParts, writer);
			writer.Write(FileHash);
			writer.Write(block1.start);
			writer.Write(block2.start);
			writer.Write(block3.start);
			writer.Write(block1.end + 1);
			writer.Write(block2.end + 1);
			writer.Write(block3.end + 1);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}
	}

}
