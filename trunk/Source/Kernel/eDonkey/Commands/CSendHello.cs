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

namespace Hathi.eDonkey.Commands
{
	internal class CSendHello
	{
		//private byte opcode;
		public CSendHello(bool response, MemoryStream buffer, CServer server, bool sendCompatOptions, bool allowPartner)
		{
			DonkeyHeader header;
			BinaryWriter writer = new BinaryWriter(buffer);
			if (response)
				header = new DonkeyHeader((byte)Protocol.ClientCommand.HelloAnswer, writer);
			else
			{
				header = new DonkeyHeader((byte)Protocol.ClientCommand.Hello, writer);
				writer.Write((byte)CKernel.Preferences.GetByteArray("UserHash").Length);
			}
			writer.Write(CKernel.Preferences.GetByteArray("UserHash"));
			writer.Write(CKernel.Preferences.GetUInt("ID"));
			writer.Write(CKernel.Preferences.GetUShort("TCPPort"));
			uint nParametros = 5;
			if (sendCompatOptions) nParametros++;
			writer.Write(nParametros);
			// username
			new ParameterWriter((byte)Protocol.ClientParameter.Name, CKernel.Preferences.GetString("UserName"), writer);
			// version
			new ParameterWriter((byte)Protocol.ClientParameter.Version, Protocol.EDONKEYVERSION, writer);
			//ParameterNumericNumeric port=new ParameterWriter((byte)Protocol.ClientParameter.Port,(uint)CKernel.Preferences.GetUShort("TCPPort"),writer);
			// emule version
			new ParameterWriter((byte)Protocol.ClientParameter.EmuleVersion, Protocol.EMULE_VERSION_COMPLEX, writer);
			// upd port
			new ParameterWriter((byte)Protocol.ClientParameter.EmuleUDPPort, (uint)CKernel.Preferences.GetUShort("UDPPort"), writer);
			// emule flags
			new ParameterWriter((byte)Protocol.ClientParameter.Emule_MiscOptions1,
				//              (                       << 4*7) |
													(Protocol.EMULE_VERSION_UDP << 4 * 6) |
													(Protocol.EMULE_VERSION_COMPRESION << 4 * 5) |
													(0 /*secureID */                        << 4 * 4) |
													(Protocol.EMULE_VERSION_SOURCEEXCHANGE << 4 * 3) |
													(Protocol.EMULE_VERSION_EXTENDEDREQUEST << 4 * 2) |
													(Protocol.EMULE_VERSION_COMMENTS << 4 * 1) |
				//              (                                       << 1*3) |
													((((Types.Constants.AllowViewShared)CKernel.Preferences.GetEnum("AllowViewShared", Types.Constants.AllowViewShared.Nobody) == Types.Constants.AllowViewShared.Nobody) ? (uint)1 : (uint)0) << 1 * 2) |
													(0 /*uMultiPacket*/                     << 1 * 1) |
													(0 /*uSupportPreview*/                  << 1 * 0)
													, writer);
			uint compatValue = 1;
			if (sendCompatOptions)
			{
				if (allowPartner) compatValue = 3;
				//CLog.Log(Types.Constants.Log.Verbose,"Sent partner flag "+compatValue.ToString());
				new ParameterWriter((byte)Protocol.ClientParameter.EmuleCompatOptions, compatValue, writer);
			}
			if (server == null)
			{
				writer.Write((uint)0);
				writer.Write((ushort)0);
			}
			else
			{
				writer.Write(server.IP);
				writer.Write(server.MainPort);
			}
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}
	}

}
