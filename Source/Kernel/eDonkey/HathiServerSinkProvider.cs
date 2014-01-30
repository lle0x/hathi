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
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Serialization.Formatters;

namespace Hathi.eDonkey
{
	[Serializable]
	internal class HathiServerSinkProvider : IServerChannelSinkProvider
	{
		private IServerChannelSinkProvider m_nextIServerChannelSink = null;
		private Hashtable m_Properties = new Hashtable();
		private Hashtable m_ProviderData = new Hashtable();
		private TypeFilterLevel m_TypeFilterLevel = TypeFilterLevel.Full;
		private IChannelReceiver m_ChannelReceiver = null;
		private IServerChannelSink m_nextSink = null;
		private IChannelDataStore m_ChannelData = null;

		#region constructor
		public HathiServerSinkProvider()
		{
			m_Properties.Add("includeVersions", true);
			this.m_nextIServerChannelSink = new BinaryServerFormatterSinkProvider(m_Properties, m_ProviderData);
		}
		public HathiServerSinkProvider(Hashtable Properties, Hashtable ProviderData)
		{
			if (Properties != null) m_Properties = Properties;
			if (ProviderData != null) m_ProviderData = ProviderData;
			this.m_nextIServerChannelSink = new BinaryServerFormatterSinkProvider(m_Properties, m_ProviderData);
		}
		#endregion

		#region Miembros de IServerChannelSinkProvider

		public IServerChannelSinkProvider Next
		{
			get
			{
				return m_nextIServerChannelSink;
			}
			set
			{
				m_nextIServerChannelSink = value;
			}
		}

		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			this.m_ChannelReceiver = channel;
			IServerChannelSink nextSink = null;
			if (m_nextIServerChannelSink == null)
				this.Next = new BinaryServerFormatterSinkProvider(m_Properties, m_ProviderData);
			nextSink = this.m_nextIServerChannelSink.CreateSink(channel);
			if (this.m_nextSink == null)
			{
				this.m_nextSink = new HathiServerChannelSink(this, channel, nextSink);
				return this.m_nextSink;
			}
			else
			{
				return new HathiServerChannelSink(this, channel, nextSink);
			}
		}

		public void GetChannelData(IChannelDataStore channelData)
		{
			m_ChannelData = channelData;
		}

		#endregion

		#region Metodos Publicos

		public IChannelDataStore GetChannelData()
		{
			return this.m_ChannelData;
		}

		public TypeFilterLevel TypeFilterLevel
		{
			get
			{
				return m_TypeFilterLevel;
			}
			set
			{
				m_TypeFilterLevel = value;
			}
		}

		#endregion
	}
}
