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
using System.Collections;

namespace Hathi.eDonkey.Commands
{
	internal class CSourceExchangeResponse
	{
		public byte[] FileHash;
		public ushort nSources;
		public stDatosFuente[] Sources;

		public CSourceExchangeResponse(MemoryStream buffer, int version)
		{
			BinaryReader reader = new BinaryReader(buffer);
			FileHash = reader.ReadBytes(16);
			nSources = reader.ReadUInt16();
			Sources = new stDatosFuente[nSources];
			for (int i = 0; i != nSources; i++)
			{
				Sources[i].IP = reader.ReadUInt32();
				Sources[i].Port = reader.ReadUInt16();
				Sources[i].ServerIP = reader.ReadUInt32();
				Sources[i].ServerPort = reader.ReadUInt16();
				if (version > 1) reader.ReadBytes(16);
			}
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}

		public CSourceExchangeResponse(MemoryStream buffer, byte[] FileHash, uint RequesterID, ushort RequesterPort, int version)
		{
			//Hashtable IPList=new Hashtable();
			ArrayList clientsList = new ArrayList();
			BinaryWriter writer = new BinaryWriter(buffer);
			DonkeyHeader header = new DonkeyHeader((byte)Protocol.ClientCommandExt.SourcesResult, writer, Protocol.ProtocolType.eMule);
			CElement Element = (CElement)CKernel.FilesList[FileHash];
			nSources = 0;
			writer.Write(FileHash);
			writer.Write(nSources);
			uint myID = CKernel.Preferences.GetUInt("ID");
			if ((Element != null) && (Element.SourcesList != null))
			{
				lock (Element.SourcesList.SyncRoot())
				{
					foreach (CClient Client in Element.SourcesList)
					{
						if ((Client.DownloadState != Protocol.DownloadState.None) &&
										(Client.UserID > Protocol.LowIDLimit) &&
										(Client.UserHash != null) &&
										(Client.UserID != RequesterID) &&
										(Client.UserID != myID))
						{
							//IPList[((ulong)Client.UserID << 16) + Client.Port]=((ulong)Client.ServerIP<<16) + Client.ServerPort;
							clientsList.Add(Client);
							nSources++;
						}
						if (nSources >= 200) break;
					}
				}
				if (nSources < 200)
				{
					int i = 0;
					CClient QueueClient;
					while (i < CKernel.Queue.Count)
					{
						QueueClient = (CClient)CKernel.Queue[i];
						if ((QueueClient.UploadElement == Element) &&
										(QueueClient.UserID > Protocol.LowIDLimit) &&
										(QueueClient.DownloadState == Protocol.DownloadState.OnQueue) &&
										(QueueClient.UserHash != null) &&
										(QueueClient.UserID != RequesterID) &&
										(QueueClient.UserID != myID))
						{
							//IPList[((ulong)QueueClient.UserID<<16)+QueueClient.Port]=((ulong)QueueClient.ServerIP<<16)+QueueClient.ServerPort;
							if (!clientsList.Contains(QueueClient)) clientsList.Add(QueueClient);
							nSources++;
						}
						if (nSources >= 200) break;
						i++;
					}
				}
			}
			/*          //do not send oursef
									IPList.Remove(((ulong)CKernel.Preferences.GetUInt("ID")<<16)+CKernel.Preferences.GetUShort("TCPPort"));
									//do not send himself
									IPList.Remove(((ulong)RequesterID<<16)+RequesterPort);
									foreach (ulong ipPort in IPList.Keys)
									{
											uint IP=(uint)(ipPort>>16);
											ushort Port=(ushort)(ipPort&0x00000000FFFF);
											//ushort Port=(ushort)(ipPort-((ulong)IP<<16));

											ulong ipServerPort=(ulong)IPList[ipPort];
											uint ServerIP=(uint)(ipServerPort>>16);
											ushort ServerPort=(ushort)(ipServerPort&0x00000000FFFF);

											writer.Write(IP);
											writer.Write(Port);
											writer.Write(ServerIP);
											writer.Write(ServerPort);
									}
			*/
			foreach (CClient client in clientsList)
			{
				writer.Write(client.UserID);
				writer.Write(client.Port);
				writer.Write(client.ServerIP);
				writer.Write(client.ServerPort);
				if (version > 1)
					writer.Write(client.UserHash);
			}
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
			writer.Seek(16, SeekOrigin.Current);
			writer.Write((ushort)clientsList.Count);
		}
	}

}
