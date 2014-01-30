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
using System.Diagnostics;

//Compresion
using System.IO;
using ICSharpCode.SharpZipLib;
using System.Runtime.Serialization;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Hathi.Types;

namespace Hathi.eDonkey.Utils
{
	public class Compresion
	{
		private static CompressionType m_CompressionProvider = CompressionType.Zip;
		private Stream m_streaminput = null;
		private Stream m_streamoutput = null;
		private byte[] m_byteinput = new byte[10];
		private byte[] m_byteoutput = new byte[10];

		#region constructors
		public Compresion()
		{
		}

		public Compresion(Stream Input)
		{
			if (Input != null)
				this.SetStream = Input;
		}
		public Compresion(Stream Input, CompressionType CompressionProvider)
		{
			this.CompressionProvider = CompressionProvider;
			if (Input != null)
				this.SetStream = Input;
		}
		public Compresion(byte[] DataInput)
		{
			if (DataInput != null)
				this.SetArray = DataInput;
		}
		public Compresion(byte[] DataInput, CompressionType CompressionProvider)
		{
			this.CompressionProvider = CompressionProvider;
			if (DataInput != null)
				this.SetArray = DataInput;
		}
		public Compresion(object objeto)
		{
			if (objeto != null)
				this.SetObject = objeto;
		}
		public Compresion(object objeto, CompressionType CompressionProvider)
		{
			this.CompressionProvider = CompressionProvider;
			if (objeto != null)
				this.SetObject = objeto;
		}
		#endregion

		#region propiedades
		public Stream SetStream
		{
			set
			{
				this.m_streaminput = null;
				this.m_streaminput = value;
				this.m_byteinput = null;
				this.m_byteinput = new byte[this.m_streaminput.Length];
				this.m_streaminput.Seek(0, SeekOrigin.Begin);
				this.m_streaminput.Read(this.m_byteinput, 0, (int)this.m_streaminput.Length);
				if (m_CompressionProvider == CompressionType.GZip)
					this.CompresNowGZip();
				else if (m_CompressionProvider == CompressionType.Zip)
					this.CompresNowZip();
			}
		}

		public byte[] SetArray
		{
			set
			{
				this.m_byteinput = null;
				this.m_byteinput = new byte[value.Length];
				this.m_byteinput = value;
				this.m_streaminput = null;
				this.m_streaminput = new MemoryStream(value);
				if (m_CompressionProvider == CompressionType.GZip)
					this.CompresNowGZip();
				else if (m_CompressionProvider == CompressionType.Zip)
					this.CompresNowZip();
			}
		}
		public object SetObject
		{
			set
			{
				Stream StreamToCompress = null;
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
						new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				bf.Serialize(StreamToCompress, value);
				this.SetStream = StreamToCompress;
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
				if (value == CompressionType.GZip | value == CompressionType.Zip)
					m_CompressionProvider = value;
			}
		}
		#endregion

		#region Funciones privadas

		private void CompresNowZip()
		{
			ICSharpCode.SharpZipLib.Zip.Compression.Deflater def =
					new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(
					ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION, true);
			Stream aux = new MemoryStream();
			byte[] data = new byte[this.m_streaminput.Length]; //minimo el tamaño sin comprimir.
			int size = 0;
			try
			{
				def.SetInput(this.m_byteinput);
				def.Finish();
				size = def.Deflate(data);
				if (size == 0)
					while (def.IsFinished == false)
					{
						if (def.IsNeedingInput)
						{
							Exception e = new Exception("Tamaño muy pequeño para comprimir");
							break;
						}
						else
						{
							size = def.Deflate(data);
							System.Threading.Thread.Sleep(2000);
						}
					}
				def.Flush();
			}
			catch (ZipException e)
			{
				this.m_byteoutput = null;
				this.m_streamoutput = null;
				Debug.WriteLine(e.Message);
			}
			catch (Exception e)
			{
				this.m_byteoutput = null;
				this.m_streamoutput = null;
				Debug.WriteLine(e.Message);
			}
			finally
			{
				this.m_byteoutput = null;
				this.m_byteoutput = new byte[size];
				this.m_streamoutput = new MemoryStream(size);
				this.m_streamoutput.Write(data, 0, size);
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
				this.m_streamoutput.Read(this.m_byteoutput, 0, size);
				this.m_streamoutput.Seek(0, SeekOrigin.Begin);
			}
		}


		private void CompresNowGZip()
		{
			Stream s = null;
			Stream aux = new MemoryStream();
			try
			{
				s = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(aux);
				s.Write(this.m_byteinput, 0, (int)this.m_byteinput.Length);
				(s as ICSharpCode.SharpZipLib.GZip.GZipOutputStream).Finish();
				s.Flush();
			}
			catch (ZipException e)
			{
				this.m_byteoutput = null;
				this.m_streamoutput = null;
				Debug.WriteLine(e.Message);
			}
			finally
			{
				aux.Seek(0, SeekOrigin.Begin);
				this.m_byteoutput = null;
				this.m_byteoutput = new byte[aux.Length];
				aux.Read(this.m_byteoutput, 0, (int)aux.Length);
				this.m_streamoutput = null;
				this.m_streamoutput = new MemoryStream(this.m_byteoutput);
				//s.Close();
			}
		}
		#endregion

	}
}
