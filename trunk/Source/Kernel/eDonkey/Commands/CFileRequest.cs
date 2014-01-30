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
	internal class CFileRequest
	{
		public byte[] FileHash;
		public byte[] Partes;

		public CFileRequest(MemoryStream buffer)
		{
			if (buffer.Length >= 16)
			{
				BinaryReader reader = new BinaryReader(buffer);
				FileHash = reader.ReadBytes(16);
				if (buffer.Length > 16)
				{
					Partes = ReadFileStatus(reader);
				}
				reader.Close();
				buffer.Close();
				buffer = null;
			}
		}

		public CFileRequest(byte[] Hash, byte[] Partes, MemoryStream buffer)
		{
			uint startpos = (uint)buffer.Position;
			BinaryWriter writer = new BinaryWriter(buffer);
			DonkeyHeader header = new DonkeyHeader((byte)Protocol.ClientCommand.FileRequest, writer);
			writer.Write(Hash, 0, 16);
			WriteFileStatus(Partes, writer);
			header.Packetlength = (uint)buffer.Position - startpos - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}

		public static byte[] ReadFileStatus(BinaryReader reader)
		{
			byte[] Partes = null;
			ushort nChunks = reader.ReadUInt16();
			if (nChunks > 0)
			{
				Partes = new byte[nChunks];
				short processedBits = 0;
				byte bitArray;
				short i;
				while (processedBits != nChunks)
				{
					bitArray = reader.ReadByte();
					i = 0;
					do //for (i=0;i!=8;i++)
					{
						Partes[processedBits] = (((bitArray >> i) & 1) == 1) ? (byte)Protocol.ChunkState.Complete : (byte)Protocol.ChunkState.Empty;
						processedBits++;
						i++;
					}
					while ((i != 8) & (processedBits != nChunks));
				}
			}
			return Partes;
		}

		public static void WriteFileStatus(byte[] Partes, BinaryWriter writer)
		{
			if ((Partes != null) && (Partes.Length > 0))
			{
				ushort nPartes = (ushort)Partes.Length;
				writer.Write(nPartes);
				if (nPartes > 0)
				{
					short bitProcesados = 0;
					byte bitArray;
					short i;
					while (bitProcesados != nPartes)
					{
						bitArray = 0;
						i = 0;
						do
						{
							if ((Protocol.ChunkState)Partes[bitProcesados] == Protocol.ChunkState.Complete) bitArray |= (byte)(1 << i);
							i++;
							bitProcesados++;
						}
						while ((i != 8) & (bitProcesados != nPartes));
						writer.Write(bitArray);
					}
				}
			}
		}
	}

}
