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
	class CServerSearchResults
	{
		public CServerSearchResults(MemoryStream buffer, CSearcher search, bool esUDP)
		{
			BinaryReader reader = new BinaryReader(buffer);
			if (!esUDP)
			{
				uint nResultados = reader.ReadUInt32();
				for (uint i = 0; i < nResultados; i++)
				{
					m_ExtractResult(reader, search);
				}
				search.OnTCPSearchEnded();
			}
			else
			{
				m_ExtractResult(reader, search);
				while ((reader.PeekChar() != 0) && (reader.PeekChar() != -1))
				{
					Debug.WriteLine("MoreUDP results in one packet");
					if ((Protocol.ProtocolType)reader.ReadByte() != Protocol.ProtocolType.eDonkey) break;
					if ((reader.PeekChar() == -1) || (reader.ReadByte() != (byte)Protocol.ServerCommandUDP.GlobalSearchResult))
						break;
					m_ExtractResult(reader, search);
				}
			}
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}

		private uint m_ExtractResult(BinaryReader reader, CSearcher search)
		{
			CParameterReader parameterReader;
			byte[] HashEncontrado = reader.ReadBytes(16);
			uint ip = reader.ReadUInt32();
			ushort port = reader.ReadUInt16();
			uint nParametros = reader.ReadUInt32();
			string fileName = "?";
			uint fileSize = 0;
			uint nSources = 1;
			string codec = "";
			uint bitrate = 0;
			string length = "";
			bool complete = false;
			for (uint param = 0; param != nParametros; param++)
			{
				parameterReader = new CParameterReader(reader);
				switch ((Protocol.FileTag)parameterReader.id)
				{
					case Protocol.FileTag.Name:
						fileName = parameterReader.valorString;
						break;
					case Protocol.FileTag.Size:
						fileSize = parameterReader.valorNum;
						break;
					case Protocol.FileTag.Sources:
						nSources = parameterReader.valorNum;
						break;
					case Protocol.FileTag.Completed:
						complete = parameterReader.valorNum > 0;
						break;
					default:
						if (parameterReader.nombreParam == Protocol.FileExtraTags.codec.ToString())
							codec = parameterReader.valorString;
						else if (parameterReader.nombreParam == Protocol.FileExtraTags.length.ToString())
							length = parameterReader.valorString;
						else if (parameterReader.nombreParam == Protocol.FileExtraTags.bitrate.ToString())
							bitrate = parameterReader.valorNum;
						//Debug.WriteLine(parameterReader.id+" name: "+parameterReader.nombreParam+" valString:"+parameterReader.valorString+" valNum: "+parameterReader.valorNum);
						break;
				}
			}
			if (fileSize < Protocol.PartSize) complete = true;
			search.AddFileFound(HashEncontrado, fileName, fileSize, nSources, codec, length, bitrate, complete, ip, port);
			return nSources;
		}
	}
}
