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
using System.Text.RegularExpressions;
//using System.IO;
namespace Hathi.UI.Winform
{
	/// <summary>
	/// Summary description for NameCleaner.
	/// </summary>
	public class CNameCleaner
	{
		public CNameCleaner()
		{
		}
		public static string Clean(string fileName)
		{
			try
			{
				string org = fileName;
				fileName = Regex.Replace(fileName, @"[_\.-]", " ");
				Regex r;
				do
				{
					r = new Regex(@"(?<name1>.*)(\[|DVD|VHS|KVCD|xvid|CD1|CD2|Screener|Spanish|German|French|Español|italian|divx|CVCD|.*RIP|\()"
												, RegexOptions.Compiled | RegexOptions.IgnoreCase);
					if (r.IsMatch(fileName))
						fileName = r.Match(fileName).Result("${name1}");
				}
				while (r.IsMatch(fileName));
				fileName = Regex.Replace(fileName, @"[^\w]", " ");
				if (fileName.Length == 0) fileName = org;
			}
			catch { }
			return fileName.Trim();
		}

	}
}
