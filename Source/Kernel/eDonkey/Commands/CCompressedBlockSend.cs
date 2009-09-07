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
using System.Collections;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Diagnostics;

namespace Hathi.eDonkey.Commands
{
	internal class CCompressedBlockSend
	{
		//cualquiera de las dos funciones de comprimir causa un leak de memoria enorme
		public static int ComprimirBuffer(byte[] in_buffer, ref byte[] out_buffer)
		{
			out_buffer = new byte[in_buffer.Length + 300];
			Deflater compresor = new Deflater();
			compresor.SetInput(in_buffer);
			compresor.Flush();
			compresor.Finish();
			int compressedsize = compresor.Deflate(out_buffer, 0, (int)(in_buffer.Length) + 300);
			compresor = null;
			return compressedsize;
		}

		public static int ComprimirBuffer2(byte[] in_buffer, ref byte[] out_buffer)
		{
			try
			{
				MemoryStream ms = new MemoryStream();
				Stream s = new DeflaterOutputStream(ms);
				s.Write(in_buffer, 0, in_buffer.Length);
				s.Close();
				out_buffer = (byte[])ms.ToArray();
				return out_buffer.Length;
			}
			catch
			{
				return in_buffer.Length;
			}
		}

		public CCompressedBlockSend(byte[] data, uint start, uint end, byte[] FileHash, ref ArrayList UploadDataPackets)
		{
			//          byte[] buffercomp=new byte[final-inicio+300];
			//          Deflater descompresor=new Deflater();
			//          descompresor.SetInput(datos);
			//          descompresor.Flush();
			//          int compressedsize=descompresor.Deflate(buffercomp,0,(int)(final-inicio)+300);
			//          descompresor.Finish();
			byte[] buffercomp = null;
			int compressedsize = ComprimirBuffer(data, ref buffercomp);
			if (compressedsize >= end - start)
			{
				buffercomp = null;
				MemoryStream strmdatos = new MemoryStream(data);
				CSendBlock EnvioBloque = new CSendBlock(strmdatos, start, end, FileHash, ref UploadDataPackets);
				return;
			}
			Debug.Write("Compressed comp:" + compressedsize.ToString() + " win: " + Convert.ToString(end - start - compressedsize) + "\n");
			MemoryStream datosComp = new MemoryStream(buffercomp);
			end = start + (uint)compressedsize;
			MemoryStream buffer;
			uint size;
			BinaryReader reader = new BinaryReader(datosComp);
			BinaryWriter writer;
			byte[] aux_buffer;
			while (start != end)
			{
				buffer = new MemoryStream();
				writer = new BinaryWriter(buffer);
				DonkeyHeader header = new DonkeyHeader((byte)Protocol.ClientCommandExt.CompressedPart, writer, Protocol.ProtocolType.eMule);
				writer.Write(FileHash);
				if (end - start > 10240) size = 10240;
				else size = end - start;
				writer.Write(start);
				writer.Write(compressedsize);
				aux_buffer = reader.ReadBytes((int)size);
				writer.Write(aux_buffer);
				start += size;
				header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
				writer.Seek(0, SeekOrigin.Begin);
				header.Serialize(writer);
				UploadDataPackets.Add(buffer);
				writer = null;
				buffer = null;
			}
			reader.Close();
			datosComp.Close();
			datosComp = null;
			reader = null;
		}
	}

}
