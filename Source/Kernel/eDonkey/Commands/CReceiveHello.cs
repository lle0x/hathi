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
	public class CReceiveHello
	{
		public byte HashSize;
		public byte[] Hash;
		public uint UserID;
		public ushort UserPort;
		public string UserName;
		public uint Version;
		public uint ServerIP;
		public ushort ServerPort;
		public byte software;

		public uint VersioneMuleMajor;
		public uint VersioneMuleMinor;
		public uint VersioneMuleRevision;
		//public byte VersionEmuleProtocol;
		public byte VersionCompression;
		public byte VersionSourceExchange;
		public byte VersionUDP;
		public byte VersionComments;
		public byte VersionExtendedRequests;
		public uint VersionLphant;
		public uint IDClientCompatible;
		public bool AllowViewSharedFiles;
		public ushort PortUDP;
		public bool ExtendedInfoComplete;
		public bool SupportsPartner;
		public bool PartnerAllowed;

		public CReceiveHello(bool bRespuesta, MemoryStream buffer)
		{
			int emuleinfo = 0;
			AllowViewSharedFiles = true;
			IDClientCompatible = 0;
			VersionUDP = 0;
			VersionComments = 0;
			VersionCompression = 0;
			VersioneMuleMinor = 0;
			VersioneMuleRevision = 0;
			VersioneMuleMajor = 0;
			VersionExtendedRequests = 0;
			VersionLphant = 0;
			VersionSourceExchange = 0;
			PortUDP = 0;
			PartnerAllowed = false;
			SupportsPartner = false;
			BinaryReader reader = new BinaryReader(buffer);
			ExtendedInfoComplete = false;
			if (bRespuesta)
				HashSize = 16;
			else
				HashSize = reader.ReadByte();
			Hash = reader.ReadBytes(HashSize);
			software = (byte)Protocol.Client.eDonkey;
			if ((Hash[5] == 14) && (Hash[14] == 111))
			{
				software = (byte)Protocol.Client.eMule;
			}
			UserID = reader.ReadUInt32();
			UserPort = reader.ReadUInt16();
			uint nParametros = reader.ReadUInt32();
			UserName = "";
			Version = 0;
			for (int i = 0; i != nParametros; i++)
			{
				CParameterReader ParameterReader = new CParameterReader(reader);
				switch ((Protocol.ClientParameter)ParameterReader.id)
				{
					case Protocol.ClientParameter.Name:
						UserName = ParameterReader.valorString;
						break;
					case Protocol.ClientParameter.Version:
						Version = ParameterReader.valorNum;
						break;
					case Protocol.ClientParameter.Port:
						UserPort = (ushort)ParameterReader.valorNum;
						break;
					case Protocol.ClientParameter.EmuleVersion:
						VersioneMuleMajor = ((ParameterReader.valorNum & 0x00FFFFFF) >> 17) & 0x7F;
						VersioneMuleMinor = ((ParameterReader.valorNum & 0x00FFFFFF) >> 10) & 0x7F;
						VersioneMuleRevision = ((ParameterReader.valorNum & 0x00FFFFFF) >> 7) & 0x07;
						//Revision=( (ParameterReader.valorNum & 0x00FFFFFF) >> 7) & 0x07;
						IDClientCompatible = (ParameterReader.valorNum >> 24);
						emuleinfo |= 4;
						break;
					case Protocol.ClientParameter.EmuleUDPPort:
						PortUDP = (ushort)ParameterReader.valorNum;
						emuleinfo |= 1;
						break;
					case Protocol.ClientParameter.Emule_MiscOptions1:
						VersionUDP = (byte)((ParameterReader.valorNum >> 4 * 6) & 0x0f);
						VersionCompression = (byte)((ParameterReader.valorNum >> 4 * 5) & 0x0f);
						//m_bySupportSecIdent       = ((ParameterReader.valorNum >> 4*4) & 0x0f);
						VersionSourceExchange = (byte)((ParameterReader.valorNum >> 4 * 3) & 0x0f);
						VersionExtendedRequests = (byte)((ParameterReader.valorNum >> 4 * 2) & 0x0f);
						VersionComments = (byte)((ParameterReader.valorNum >> 4 * 1) & 0x0f);
						AllowViewSharedFiles = ((ParameterReader.valorNum >> 1 * 2) & 0x01) > 0;
						//m_bMultiPacket            = (ParameterReader.valorNum >> 1*1) & 0x01;
						//m_fSupportsPreview        = (ParameterReader.valorNum >> 1*0) & 0x01;
						emuleinfo |= 2;
						break;
					case Protocol.ClientParameter.EmuleCompatOptions:
						if ((ParameterReader.valorNum & 0x00000001) > 0) SupportsPartner = true;
						if ((ParameterReader.valorNum & 0x00000002) > 0) PartnerAllowed = true;
						break;
				}
			}
			ServerIP = reader.ReadUInt32();
			ServerPort = reader.ReadUInt16();
			if ((emuleinfo & 4) == 4) ExtendedInfoComplete = true;
			if (buffer.Length - buffer.Position >= 3)
			{
				software = (byte)Protocol.Client.eDonkeyHybrid;
				uint extra = reader.ReadUInt32();
				if (extra == 1262767181)
					software = (byte)Protocol.Client.mlDonkey;
			}
			if ((IDClientCompatible == (byte)Protocol.Client.Hathi) && (VersioneMuleMajor > 0)) VersioneMuleMajor--;
			if (Version > 10000 && Version < 100000)
				Version = Version - (Version / 10000) * 10000;
			if (Version > 1000)
				Version = Version - (Version / 1000) * 1000;
			if (Version < 100)
				Version *= 10;
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}
	}
}
