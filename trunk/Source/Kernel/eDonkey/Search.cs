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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Hathi.eDonkey.Commands;
using Hathi.eDonkey.CommandsServer;

namespace Hathi.eDonkey
{
	internal class CSearcher : Hashtable
	{
		private int m_ServerIndex;
		private MemoryStream m_TCPPacket;
		private MemoryStream m_UDPPacket;
		private uint m_sources;
		private bool m_SearchCanceled;
		private bool m_IsClientSearch;
		private bool m_Searching;

		public void CancelSearch()
		{
			m_SearchCanceled = true;
		}

		/// <summary>
		/// Constructor used in common client server searchs
		/// </summary>
		/// <param name="searchString">searchString</param>
		/// <param name="matchAnyWords">matchAnyWords</param>
		/// <param name="type">type</param>
		/// <param name="maxSize">maxSize</param>
		/// <param name="minSize">minSize</param>
		/// <param name="avaibility">avaibility</param>
		public CSearcher(string searchString, bool matchAnyWords, string type, uint maxSize, uint minSize, uint avaibility, string exclude)
			: base()
		{
			// buscamos primero en el servidor en el que estamos
			m_TCPPacket = new MemoryStream();
			m_UDPPacket = new MemoryStream();
			m_sources = 0;
			m_IsClientSearch = false;
			m_SearchCanceled = false;
			m_Searching = true;
			CServerRequestSearch ServerRequestSearchTCP = new CServerRequestSearch(m_TCPPacket, searchString, matchAnyWords, type, maxSize, minSize, avaibility, exclude, false);
			CServerRequestSearch ServerRequestSearchUDP = new CServerRequestSearch(m_UDPPacket, searchString, matchAnyWords, type, maxSize, minSize, avaibility, exclude, true);
			CKernel.ServersList.ActiveServer.SendTCPSearch(m_TCPPacket, this);
		}

		/// <summary>
		/// Constructor used when the search is a shard file list request
		/// </summary>
		public CSearcher()
		{
			m_IsClientSearch = true;
			m_Searching = false;
		}

		public void ExtendSearch()
		{
			if ((CKernel.ServersList.Count > m_ServerIndex)
							&& (CServer)(CKernel.ServersList[m_ServerIndex]) != CKernel.ServersList.ActiveServer)
			{
				CServer nextServer;
				nextServer = (CServer)CKernel.ServersList[m_ServerIndex];
				nextServer.SendUDPSearch(m_UDPPacket, this);
			}
			m_ServerIndex++;
			if (m_ServerIndex >= CKernel.ServersList.Count)
			{
				m_ServerIndex = 0;
			}
		}

		public void AddFileFound(byte[] Hash, string name, uint size, uint avaibility, string codec, string length, uint bitrate, bool complete, uint ip, ushort port)
		{
			try
			{
				string strHash = CKernel.HashToString(Hash);
				m_sources += avaibility;
				if (ContainsKey(strHash))
				{
					CFileFound fileFound = (CFileFound)this[strHash];
					fileFound.UpdateFile(avaibility, name, codec, length, bitrate, complete, ip, port);
					CKernel.SearchFileModified(fileFound, (int)CKernel.Searchs.GetKey(CKernel.Searchs.IndexOfValue(this)));
				}
				else
				{
					CFileFound fileFound = new CFileFound(strHash, name, size, avaibility, codec, length, bitrate, complete, ip, port);
					Add(strHash, fileFound);
					CKernel.NewFileFound(fileFound, (int)CKernel.Searchs.GetKey(CKernel.Searchs.IndexOfValue(this)));
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("Search error");
				Debug.WriteLine(e.ToString());
				CKernel.SearchEnded((int)CKernel.Searchs.GetKey(CKernel.Searchs.IndexOfValue(this)));
			}
		}

		public void OnTCPSearchEnded()
		{
			if ((CKernel.Preferences.GetBool("AutoExtendSearch"))
							&& (m_sources < Protocol.MaxSearchResults)
							&& (!m_IsClientSearch) && (m_Searching))
			{
				Debug.Write("Autoextending search\n");
				m_ServerIndex = 0;
				for (uint i = 0; i < CKernel.ServersList.Count; i++)
				{
					if ((m_sources > Protocol.MaxSearchResults) || (m_SearchCanceled)) break;
					ExtendSearch();
					Thread.Sleep(250);
					CKernel.NewSearchProgress((int)(((float)i / (float)CKernel.ServersList.Count) * 100.0F), (int)CKernel.Searchs.GetKey(CKernel.Searchs.IndexOfValue(this)));
				}
				//m_UDPPacket.Close();
				//m_UDPPacket=null;
				if (m_ServerIndex >= CKernel.ServersList.Count)
				{
					m_ServerIndex = 0;
				}
			}
			//          else
			//          {
			//              if ((!m_IsClientSearch)&&(m_Searching)&&(CKernel.ServersList.ActiveServer!=null))
			//                  CKernel.ServersList.ActiveServer.QueryMoreResults();
			//
			//          }
			m_Searching = false;
			if (CKernel.Searchs.IndexOfValue(this) >= 0)
				CKernel.SearchEnded((int)CKernel.Searchs.GetKey(CKernel.Searchs.IndexOfValue(this)));
		}
	}
}