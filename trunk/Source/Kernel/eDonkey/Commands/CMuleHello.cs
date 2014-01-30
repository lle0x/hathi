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
	internal class CMuleHello
	{
		public byte VersioneMule;
		public byte VersionEmuleProtocol;
		public byte VersionCompression;
		public byte VersionSourceExchange;
		public byte VersionUDP;
		public byte VersionComments;
		public byte VersionExtendedRequests;
		public uint VersionLphant;
		public uint IDClientCompatible;
		public ushort PortUDP;

		public CMuleHello(MemoryStream buffer, bool isResponse)
		{
			BinaryWriter writer = new BinaryWriter(buffer);
			DonkeyHeader header;
			if (isResponse)
				header = new DonkeyHeader((byte)Protocol.ClientCommandExt.eMuleInfoAnswer, writer, Protocol.ProtocolType.eMule);
			else
				header = new DonkeyHeader((byte)Protocol.ClientCommandExt.eMuleInfo, writer, Protocol.ProtocolType.eMule);
			writer.Write(Protocol.EMULE_VERSION);
			writer.Write(Protocol.EMULE_PROTOCOL_VERSION);
			uint nParameters = 8;
			writer.Write(nParameters);
			// compression
			new ParameterWriter(Protocol.ET_COMPRESSION, Protocol.EMULE_VERSION_COMPRESION, writer);
			// udp port
			new ParameterWriter(Protocol.ET_UDPPORT, (uint)CKernel.Preferences.GetUShort("UDPPort"), writer);
			// version udp
			new ParameterWriter(Protocol.ET_UDPVER, (uint)Protocol.EMULE_VERSION_UDP, writer);
			// version source exchange
			new ParameterWriter(Protocol.ET_SOURCEEXCHANGE, (uint)Protocol.EMULE_VERSION_SOURCEEXCHANGE, writer);
			// version comments
			new ParameterWriter(Protocol.ET_COMMENTS, (uint)Protocol.EMULE_VERSION_COMMENTS, writer);
			// version extended requests
			new ParameterWriter(Protocol.ET_EXTENDEDREQUEST, (uint)Protocol.EMULE_VERSION_EXTENDEDREQUEST, writer);
			// client compatible
			new ParameterWriter(Protocol.ET_COMPATIBLECLIENT, (byte)Protocol.Client.Hathi, writer);
			// version Hathi
			new ParameterWriter(Protocol.ET_ELEPHANTVERSION, Protocol.ELEPHANT_VERSION, writer);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}

		public CMuleHello(MemoryStream buffer)
		{
			BinaryReader reader = new BinaryReader(buffer);
			VersioneMule = reader.ReadByte();
			if (VersioneMule == 0x2B) VersioneMule = 0x22;
			VersionEmuleProtocol = reader.ReadByte();
			if (VersionEmuleProtocol == Protocol.EMULE_PROTOCOL_VERSION)
			{
				if (VersioneMule < 0x25 && VersioneMule > 0x22)
					VersionUDP = 1;
				if (VersioneMule < 0x25 && VersioneMule > 0x21)
					VersionSourceExchange = 1;
				if (VersioneMule == 0x24)
					VersionComments = 1;
			}
			PortUDP = 0;
			VersionCompression = 0;
			VersionSourceExchange = 1;
			uint nParametros = reader.ReadUInt32();
			if (VersionEmuleProtocol != Protocol.EMULE_PROTOCOL_VERSION) return;
			for (int i = 0; i != nParametros; i++)
			{
				CParameterReader ParameterReader = new CParameterReader(reader);
				switch (ParameterReader.id)
				{
					case Protocol.ET_COMPRESSION:
						VersionCompression = (byte)ParameterReader.valorNum;
						break;
					case Protocol.ET_UDPPORT:
						PortUDP = (ushort)ParameterReader.valorNum;
						break;
					case Protocol.ET_UDPVER:
						VersionUDP = (byte)ParameterReader.valorNum;
						break;
					case Protocol.ET_SOURCEEXCHANGE:
						VersionSourceExchange = (byte)ParameterReader.valorNum;
						break;
					case Protocol.ET_COMMENTS:
						VersionComments = (byte)ParameterReader.valorNum;
						break;
					case Protocol.ET_EXTENDEDREQUEST:
						VersionExtendedRequests = (byte)ParameterReader.valorNum;
						break;
					case Protocol.ET_COMPATIBLECLIENT:
						IDClientCompatible = ParameterReader.valorNum;
						break;
					case Protocol.ET_ELEPHANTVERSION:
						VersionLphant = ParameterReader.valorNum & 0x00FFFFFF;
						break;
				}
			}
			if (VersionCompression == 0)
			{
				VersionSourceExchange = 0;
				VersionExtendedRequests = 0;
				VersionComments = 0;
				PortUDP = 0;
			}
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}
	}
}
