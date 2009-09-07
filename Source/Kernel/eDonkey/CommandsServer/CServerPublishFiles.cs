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

using System.IO;
using System.Collections;
using System.Text;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Hathi.Types;
using Hathi.eDonkey.Commands;
using System;

namespace Hathi.eDonkey.CommandsServer
{
	internal class CServerPublishFiles
	{
		public uint LastFilePublished;
		//constructor to publish a list of files
		public CServerPublishFiles(MemoryStream buffer, CFilesList FilesList, uint startIndex)
		{
			DonkeyHeader header;
			LastFilePublished = startIndex;
			BinaryWriter writer = new BinaryWriter(buffer);
			header = new DonkeyHeader((byte)Protocol.ServerCommand.OfferFiles, writer);
			writer.Write((uint)FilesList.Count);
			uint nfiles = 0;
			int fileIndex = 0;
			foreach (CElement element in FilesList.Values)
			{
				fileIndex++;
				if (fileIndex < startIndex) continue;
				if (m_AddFileToPacket(writer, element)) nfiles++;
				LastFilePublished++;
				if (nfiles >= 200) break; //TODO: check server soft limits
			}
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
			writer.Write(nfiles);
			CLog.Log(Types.Constants.Log.Info, "FIL_PUBLISHED", nfiles);
		}
		//constructor to publish one file
		public CServerPublishFiles(MemoryStream buffer, CElement element)
		{
			DonkeyHeader header;
			BinaryWriter writer = new BinaryWriter(buffer);
			header = new DonkeyHeader((byte)Protocol.ServerCommand.OfferFiles, writer);
			writer.Write(1);
			m_AddFileToPacket(writer, element);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
			writer.Write(1);
			CLog.Log(Types.Constants.Log.Info, "FIL_PUBLISHED", 1);
		}
		private bool m_AddFileToPacket(BinaryWriter writer, CElement element)
		{
			try
			{
				if (!element.File.Empty)
				{
					writer.Write(element.File.FileHash);
					if (element.File.Completed)
					{
						writer.Write((uint)0xfbfbfbfb);
						writer.Write((ushort)0xfbfb);
					}
					else
					{
						writer.Write((uint)0xfcfcfcfc);
						writer.Write((ushort)0xfcfc);
					}
					uint nParameters = 2;
					if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.Length)) nParameters++;
					if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.VBitrate)) nParameters++;
					if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.VCodec)) nParameters++;
					writer.Write(nParameters);
					// name
					new ParameterWriter((byte)Protocol.FileTag.Name, element.File.FileName, writer);
					// size
					new ParameterWriter((byte)Protocol.FileTag.Size, element.File.FileSize, writer);
					if (element.File.Details.Type == (byte)Constants.FileType.Avi)
					{
						if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.Length))
							new ParameterWriter(Protocol.FileExtraTags.length.ToString(), (string)element.File.Details.ListDetails[Constants.Avi.Length], writer);
						if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.VBitrate))
							new ParameterWriter(Protocol.FileExtraTags.bitrate.ToString(), Convert.ToUInt32(((string)element.File.Details.ListDetails[Constants.Avi.VBitrate]).Replace(" Kbps", "")), writer);
						if (element.File.Details.ListDetails.ContainsKey(Constants.Avi.VCodec))
							new ParameterWriter(Protocol.FileExtraTags.codec.ToString(), (string)element.File.Details.ListDetails[Constants.Avi.VCodec], writer);
					}
					return true;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
			return false;
		}
	}
}
