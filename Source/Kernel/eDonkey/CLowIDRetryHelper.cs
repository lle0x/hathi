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

namespace Hathi.eDonkey
{
	/// <summary>
	/// CLowIDRetry  helps the server connection deciding when it should retry
	///  in order to get a high id
	/// </summary>
	internal class CLowIDRetryHelper
	{
		private byte m_numRetriesLowID;    //The counter of retries left
		private ushort m_tcpPort;          //Last tcpPort we got a HighID
		private ushort m_udpPort;          //Last udpPort we got a HighID

		public CLowIDRetryHelper()
		{
			uint lastId = (uint)CKernel.Preferences.GetUInt("ID");
			//Check if the program finished last run with a High id
			if (lastId > Protocol.LowIDLimit)
			{
				m_numRetriesLowID = Protocol.MaxRetriesLowID;
				m_tcpPort = (ushort)CKernel.Preferences.GetProperty("TCPPort");
				m_udpPort = (ushort)CKernel.Preferences.GetProperty("UDPPort");
			}
			else
			{
				m_numRetriesLowID = 0;
				m_tcpPort = 0;
				m_tcpPort = 0;
			}
		}

		/// <summary>
		/// This method is called to get advice about retrying the connection
		/// </summary>
		public bool ShouldRetry()
		{
			bool result = false;
			if (m_numRetriesLowID > 0)
			{
				lock (this)
				{
					//Check if the port configuration has been changed
					if (m_tcpPort != (ushort)CKernel.Preferences.GetProperty("TCPPort") ||
									m_udpPort != (ushort)CKernel.Preferences.GetProperty("UDPPort"))
					{
						m_numRetriesLowID = 0;
						result = false;
					}
					else
					{
						m_numRetriesLowID--;
						result = true;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// This method should be called when a high ID connection is got
		/// </summary>
		public void GotHighID()
		{
			lock (this)
			{
				m_numRetriesLowID = Protocol.MaxRetriesLowID;
				m_tcpPort = (ushort)CKernel.Preferences.GetProperty("TCPPort");
				m_udpPort = (ushort)CKernel.Preferences.GetProperty("UDPPort");
			}
		}
	}
}
