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
	internal class CFileStatus
	{
		public byte[] FileHash;
		public ushort nChunks;
		public byte[] Chunks;

		public CFileStatus(MemoryStream buffer, bool readHash)
		{
			BinaryReader reader = new BinaryReader(buffer);
			if (readHash) FileHash = reader.ReadBytes(16);
			if (reader.PeekChar() == -1)
				nChunks = 0;
			else
				nChunks = reader.ReadUInt16();
			if (nChunks > 0)
			{
				Chunks = new byte[nChunks];
				short bitProcesados = 0;
				byte bitArray;
				short i;
				while (bitProcesados != nChunks)
				{
					bitArray = reader.ReadByte();
					i = 0;
					do //for (i=0;i!=8;i++)
					{
						Chunks[bitProcesados] = (((bitArray >> i) & 1) == 1) ? (byte)Protocol.ChunkState.Complete : (byte)Protocol.ChunkState.Empty;
						bitProcesados++;
						i++;
					}
					while ((i != 8) & (bitProcesados != nChunks));
				}
			}
			//          reader.Close();
			//          buffer.Close();
			//          reader=null;
			//          buffer=null;
		}

		public CFileStatus(byte[] in_FileHash, byte[] in_Partes, MemoryStream buffer)
		{
			BinaryWriter writer = new BinaryWriter(buffer);
			DonkeyHeader header = new DonkeyHeader((byte)Protocol.ClientCommand.FileState, writer);
			writer.Write(in_FileHash, 0, 16);
			WriteFileStatus(writer, in_Partes);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}

		public static void WriteFileStatus(BinaryWriter writer, byte[] in_Partes)
		{
			ushort nChunks;
			if ((in_Partes == null) || (in_Partes.Length == 0)) nChunks = 0;
			else nChunks = (ushort)(in_Partes.Length);
			writer.Write(nChunks);
			if (nChunks > 0)
			{
				short bitProcesados = 0;
				byte bitArray;
				short i;
				while (bitProcesados != nChunks)
				{
					bitArray = 0;
					i = 0;
					do
					{
						if ((Protocol.ChunkState)in_Partes[bitProcesados] == Protocol.ChunkState.Complete)
							bitArray |= (byte)(1 << i);
						i++;
						bitProcesados++;
					}
					while ((i != 8) & (bitProcesados != nChunks));
					writer.Write(bitArray);
				}
			}
		}
	}
}
