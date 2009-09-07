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
using System.Collections;
using System.Collections.Specialized;
using Hathi.eDonkey.Commands;

namespace Hathi.eDonkey
{
	/// <summary>
	/// Summary description for CFileFound.
	/// </summary>
	internal class CFileFound
	{
		public string Hash;
		public string Name;
		public uint Size;
		public uint Avaibility;
		public string Codec;
		public uint BitRate;
		public string Length;
		public bool Complete;
		public Hashtable Sources;

		public Types.Constants.SearchResultState ResultState;
		public StringCollection OtherNames;

		public CFileFound(string in_Hash, string in_Name, uint in_Size, uint in_Avaibility, string in_codec, string in_length, uint in_bitrate, bool in_complete, uint in_ip, ushort in_port)
		{
			this.Hash = in_Hash;
			this.Name = in_Name;
			this.Size = in_Size;
			this.Avaibility = in_Avaibility;
			Codec = in_codec;
			BitRate = in_bitrate;
			Length = in_length;
			Complete = in_complete;
			this.OtherNames = new StringCollection();
			this.OtherNames.Add(Name);
			CElement element = CKernel.FilesList[CKernel.StringToHash(in_Hash)];
			if (element == null)
				ResultState = Types.Constants.SearchResultState.New;
			else if (element.File.FileStatus == Protocol.FileState.Complete)
				ResultState = Types.Constants.SearchResultState.AlreadyDownloaded;
			else
				ResultState = Types.Constants.SearchResultState.AlreadyDownloading;
			if ((in_ip > Protocol.LowIDLimit) && (in_port > 0) && (in_port < ushort.MaxValue))
			{
				Sources = new Hashtable();
				Sources.Add(in_ip, in_port);
				//Debug.WriteLine(in_ip.ToString()+":"+in_port.ToString());
				if ((element != null) && (element.File.FileStatus == Protocol.FileState.Ready))
					CKernel.ClientsList.AddClientToFile(in_ip, in_port, 0, 0, element.File.FileHash);
			}
		}

		public void UpdateFile(uint in_Avaibility, string in_Name, string in_codec, string in_length, uint in_bitrate, bool in_complete, uint in_ip, ushort in_port)
		{
			this.Avaibility += in_Avaibility;
			Complete = Complete || in_complete;
			if (!this.OtherNames.Contains(in_Name))
			{
				this.OtherNames.Add(in_Name);
			}
			if (((Length == null) || (Length.Length == 0)) && (in_length.Length > 0))
				Length = in_length;
			if ((Codec.Length == 0) && (in_codec.Length > 0))
				Codec = in_codec;
			if ((BitRate == 0) && (in_bitrate > 0))
				BitRate = in_bitrate;
			if ((in_ip > Protocol.LowIDLimit) && (in_port > 0) && (in_port < ushort.MaxValue))
			{
				if (Sources == null) Sources = new Hashtable();
				if (!Sources.Contains(in_ip)) Sources.Add(in_ip, in_port);
				//Debug.WriteLine(in_ip.ToString()+":"+in_port.ToString());
				CElement element = CKernel.FilesList[CKernel.StringToHash(Hash)];
				if ((element != null) && (element.File.FileStatus == Protocol.FileState.Ready))
				{
					CKernel.ClientsList.AddClientToFile(in_ip, in_port, 0, 0, element.File.FileHash);
				}
			}
		}

		public void AddSourcesToFile()
		{
			if ((Sources == null) || (Sources.Count == 0)) return;
			CElement element = CKernel.FilesList[CKernel.StringToHash(this.Hash)];
			int i;
			if ((element != null) && (element.File.FileStatus == Protocol.FileState.Ready))
			{
				stDatosFuente[] sourcesList = new stDatosFuente[Sources.Count];
				i = 0;
				foreach (uint ip in Sources.Keys)
				{
					sourcesList[i].IP = ip;
					sourcesList[i].Port = (ushort)Sources[ip];
					i++;
				}
				CKernel.ClientsList.AddClientsToFile(sourcesList, CKernel.StringToHash(this.Hash));
			}
		}

		public string ToLink()
		{
			return "ed2k://|file|" + this.Name + "|" + this.Size.ToString() + "|" + this.Hash + "|/";
		}
	}
}
