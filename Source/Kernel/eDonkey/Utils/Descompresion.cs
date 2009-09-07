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
using System.Diagnostics;

//Compresion
using System.IO;
using ICSharpCode.SharpZipLib;
using System.Runtime.Serialization;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Hathi.Types;

namespace Hathi.eDonkey.Utils
{
	public class Descompresion
	{
		private static CompressionType m_CompressionProvider = CompressionType.Zip;
		private Stream m_streaminput = null;
		private Stream m_streamoutput = null;
		private byte[] m_byteinput = new byte[10];
		private byte[] m_byteoutput = new byte[10];

		#region constructors
		public Descompresion()
		{
		}

		public Descompresion(Stream Input)
		{
			if (Input != null)
				this.SetStream = Input;
		}
		public Descompresion(Stream Input, CompressionType CompressionProvider)
		{
			this.CompressionProvider = CompressionProvider;
			if (Input != null)
				this.SetStream = Input;
		}
		public Descompresion(byte[] DataInput)
		{
			if (DataInput != null)
				this.SetArray = DataInput;
		}
		public Descompresion(byte[] DataInput, CompressionType CompressionProvider)
		{
			this.CompressionProvider = CompressionProvider;
			if (DataInput != null)
				this.SetArray = DataInput;
		}

		#endregion

		#region propiedades
		public Stream SetStream
		{
			set
			{
				Stream aux = value;
				this.m_byteinput = null;
				this.m_byteinput = new byte[aux.Length];
				aux.Seek(0, SeekOrigin.Begin);
				aux.Read(this.m_byteinput, 0, (int)aux.Length);
				this.m_streaminput = null;
				this.m_streaminput = new MemoryStream(this.m_byteinput);
				if (this.CompressionProvider == CompressionType.Zip)
					this.DecompresNowZip();
				else if (this.CompressionProvider == CompressionType.GZip)
					this.DecompresNowGZip();
			}
		}

		public byte[] SetArray
		{
			set
			{
				this.m_byteinput = null;
				this.m_byteinput = new byte[value.Length];
				this.m_byteinput = value;
				this.m_streaminput = new MemoryStream(value);
				if (this.CompressionProvider == CompressionType.Zip)
					this.DecompresNowZip();
				else if (this.CompressionProvider == CompressionType.GZip)
					this.DecompresNowGZip();
			}
		}

		public Stream ToStream
		{
			get
			{
				return this.m_streamoutput;
			}
		}

		public byte[] ToArray
		{
			get
			{
				return this.m_byteoutput;
			}
		}

		public CompressionType CompressionProvider
		{
			get
			{
				return m_CompressionProvider;
			}
			set
			{
				m_CompressionProvider = value;
			}
		}
		#endregion

		#region Funciones privadas

		private void DecompresNowZip()
		{
			ICSharpCode.SharpZipLib.Zip.Compression.Inflater def =
					new ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true);
			byte[] data = new byte[50000];
			int size = 0;
			try
			{
				def.SetInput(this.m_byteinput);
				size = def.Inflate(data);
				if (size == 0)
					while (def.IsFinished == false)
					{
						if (def.IsNeedingInput)
						{
							Exception e = new Exception("Se necesitan mas datos, se han perdido?");
							break;
						}
						else
						{
							if (def.IsNeedingDictionary)
							{
								Exception e = new Exception("Falta el diccionario");
								break;
							}
							else
							{
								System.Threading.Thread.Sleep(2000);
								size = def.Inflate(data);
							}
						}
					}
			}
			catch (ZipException e)
			{
				this.m_byteoutput = null;
				this.m_streamoutput = null;
				Debug.WriteLine(e.Message);
			}
			finally
			{
				this.m_byteoutput = null;
				this.m_byteoutput = new byte[size];
				this.m_streamoutput = null;
				this.m_streamoutput = new MemoryStream(size);
				this.m_streamoutput.Write(data, 0, size);
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
				this.m_streamoutput.Read(this.m_byteoutput, 0, size);
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
			}
		}

		private void DecompresNowGZip()
		{
			int restan = 0;
			int size = 0;
			byte[] BytesDecompressed = new byte[50000];
			this.m_streaminput.Seek(0, SeekOrigin.Begin);
			Stream s = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(this.m_streaminput);
			try
			{
				while (true)
				{
					size = s.Read(BytesDecompressed, 0, (int)BytesDecompressed.Length);
					if (size > 0)
					{
						restan += size;
						size = restan;
					}
					else break;
				}
			}
			catch (ZipException e)
			{
				size = 0;
				Debug.WriteLine(e.Message);
			}
			finally
			{
				s.Read(BytesDecompressed, 0, restan);
				s.Flush();
				this.m_streamoutput = null;
				this.m_streamoutput = new MemoryStream(restan);
				this.m_streamoutput.Write(BytesDecompressed, 0, restan);
				this.m_byteoutput = null;
				this.m_byteoutput = new byte[restan];
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
				this.m_streamoutput.Read(this.m_byteoutput, 0, restan);
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
				//s.Close();
			}
		}
		#endregion

	}

}
