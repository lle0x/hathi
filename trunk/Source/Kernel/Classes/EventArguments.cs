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

namespace Hathi.Classes
{
	#region Property Event Arguments
	public class PropertyEventArgs : EventArgs
	{
		string _key;
		object _newValue;
		object _oldValue;

		/// <returns>
		/// The key of the changed property
		/// </returns>
		public string Key
		{
			get
			{
				return _key;
			}
		}

		/// <returns>
		/// The new value of the property
		/// </returns>
		public object NewValue
		{
			get
			{
				return _newValue;
			}
			set
			{
				_newValue = value;
			}
		}

		/// <returns>
		/// The old value of the property
		/// </returns>
		public object OldValue
		{
			get
			{
				return _oldValue;
			}
		}

		public PropertyEventArgs(string key, object oldValue, object newValue)
		{
			this._key = key;
			this._oldValue = oldValue;
			this._newValue = newValue;
		}
	}

	public class PropertyDefaultArgs : EventArgs
	{
		string _key;
		object _value;

		/// <returns>
		/// The key of the changed property
		/// </returns>
		public string Key
		{
			get
			{
				return _key;
			}
		}

		/// <returns>
		/// The new value of the property
		/// </returns>
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				this._value = value;
			}
		}

		public PropertyDefaultArgs(string key, object value)
		{
			this._key = key;
			this._value = value;
		}
	}

	public class PropertyLoadedArgs : EventArgs
	{
		string _version;

		/// <returns>
		/// The version of loaded file
		/// </returns>
		public string Version
		{
			get
			{
				return _version;
			}
		}

		public PropertyLoadedArgs(string version)
		{
			this._version = version;
		}
	}
	#endregion
}