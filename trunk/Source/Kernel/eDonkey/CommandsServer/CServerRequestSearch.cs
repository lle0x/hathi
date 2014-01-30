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
	class CServerRequestSearch
	{
		const byte typeNemonic1 = 0x03;
		const short typeNemonic2 = 0x0001;
		const uint extensionNemonic = 0x00040001;
		const uint avaibilityNemonic = 0x15000101;
		const uint minNemonic = 0x02000101;
		const uint maxNemonic = 0x02000102;
		const byte stringParameter = 1;
		const byte typeParameter = 2;
		const byte numericParameter = 3;
		const short andParameter = 0x0000;
		const short orParameter = 0x0100;
		const short notParameter = 0x0200;

		public CServerRequestSearch(MemoryStream buffer, string searchString, bool matchAnyWords, string type, uint maxSize, uint minSize, uint avaibility, string exclude, bool esUDP)
		{
			DonkeyHeader header = new DonkeyHeader((byte)Protocol.ServerCommand.SearchRequest);
			BinaryWriter writer = new BinaryWriter(buffer);
			if (esUDP)
			{
				writer.Write((byte)Protocol.Header.eDonkey);
				//writer.Write((byte)Protocol.ServerCommandsUDP.GlobalSearchRequest);
				writer.Write((byte)Protocol.ServerCommandUDP.GlobalSearchRequest2);
			}
			else
				header.Serialize(writer);
			if (type == "Any") type = "";
			//header
			int parametercount = 0; //must be written parametercount-1 parmeter headers
			if (exclude.Length > 0)
				writer.Write(notParameter);
			if (searchString.Length > 0)
			{
				parametercount++;
				if (parametercount > 1)
					writer.Write(andParameter);
			}
			if (type.Length > 0)
			{
				parametercount++;
				if (parametercount > 1)
					writer.Write(andParameter);
			}
			if (minSize > 0)
			{
				parametercount++;
				if (parametercount > 1)
					writer.Write(andParameter);
			}
			if (maxSize > 0)
			{
				parametercount++;
				if (parametercount > 1)
					writer.Write(andParameter);
			}
			if (avaibility > 0)
			{
				parametercount++;
				if (parametercount > 1)
					writer.Write(andParameter);
			}
			//              if (extension.GetLength()>0)
			//              {
			//              parametercount++;
			//              if (parametercount>1)
			//                  writer.Write(andParameter);
			//              }
			//body
			if (searchString.Length > 0) //search a string
			{
				//writer.Write(notParameter);
				string[] list = searchString.Split(" ".ToCharArray());
				byte[] searchbyte;
				if ((matchAnyWords) && (list.Length > 1))
				{
					for (int i = 0; i < list.Length; i++)
					{
						if (i != list.Length - 1) writer.Write(orParameter);
						writer.Write(stringParameter); //write the parameter type
						searchbyte = Encoding.Default.GetBytes(list[i]);
						writer.Write((ushort)searchbyte.Length);
						writer.Write(searchbyte);
					}
				}
				else
				{
					writer.Write(stringParameter); //write the parameter type
					searchbyte = Encoding.Default.GetBytes(searchString);
					writer.Write((ushort)searchbyte.Length);
					writer.Write(searchbyte);
				}
			}
			if (type.Length > 0)
			{
				writer.Write(typeParameter); //write the parameter type
				byte[] searchbyte = Encoding.Default.GetBytes(type);
				writer.Write((ushort)searchbyte.Length);
				writer.Write(searchbyte);
				writer.Write(typeNemonic2);
				writer.Write(typeNemonic1);
			}
			if (minSize > 0)
			{
				writer.Write(numericParameter); //write the parameter type
				writer.Write(minSize);         //write the parameter
				writer.Write(minNemonic);    //nemonic for this kind of parameter
			}
			if (maxSize > 0)
			{
				writer.Write(numericParameter); //write the parameter type
				writer.Write(maxSize);         //write the parameter
				writer.Write(maxNemonic);    //nemonic for this kind of parameter
			}
			if (avaibility > 0)
			{
				writer.Write(numericParameter); //write the parameter type
				writer.Write(avaibility);    //write the parameter
				writer.Write(avaibilityNemonic);  //nemonic for this kind of parameter
			}
			//              if (extension.GetLength()>0)
			//              {
			//                  data.Write(&stringParameter,1); //write the parameter type
			//                  nSize=extension.GetLength();
			//                  data.Write(&nSize,2);   //write the length
			//                  formatC=extension.GetBuffer();
			//                  data.Write(formatC,nSize); //write parameter
			//                  data.Write(&extensionNemonic,3); //nemonic for this kind of parameter (only 3 bytes!!)
			//              }
			if (exclude.Length > 0)
			{
				writer.Write(stringParameter); //write the parameter type
				byte[] searchbyte2 = Encoding.Default.GetBytes(exclude);
				writer.Write((ushort)searchbyte2.Length);
				writer.Write(searchbyte2);
			}
			if (!esUDP)
			{
				header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
				writer.Seek(0, SeekOrigin.Begin);
				header.Serialize(writer);
			}
		}
	}
}
