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
	/// <summary>
	/// CServerDescription process description send by CServer.
	/// </summary>
	internal class CServerDescription
	{
		public string Name;
		public string Description;

		public CServerDescription(MemoryStream buffer)
		{
			BinaryReader reader = new BinaryReader(buffer);
			reader.ReadBytes(30);
			short length;
			length = reader.ReadInt16();
			Name = Encoding.ASCII.GetString(reader.ReadBytes(length));
			reader.ReadBytes(4);
			length = reader.ReadInt16();
			Description = Encoding.ASCII.GetString(reader.ReadBytes(length));
			reader.Close();
			buffer.Close();
			reader = null;
			buffer = null;
		}
	}
}
//using System.IO;
//using System.Collections;
//using System.Text;
//using System.Diagnostics;
//using ICSharpCode.SharpZipLib.Zip.Compression;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
//using Hathi.Types;
//using Hathi.eDonkey.Commands;

//namespace Hathi.eDonkey.CommandsServer
//{
//    /// <summary>
//    /// CServerDescription process description send by CServer.
//    /// </summary>
//    internal class CServerDescription
//    {
//        public string Name;
//        public string Description;

//        public CServerDescription(MemoryStream buffer)
//        {
//            BinaryReader reader = new BinaryReader(buffer);
//            reader.ReadBytes(30);
//            short length;
//            length = reader.ReadInt16();
//            Name = Encoding.ASCII.GetString(reader.ReadBytes(length));
//            reader.ReadBytes(4);
//            length = reader.ReadInt16();
//            Description = Encoding.ASCII.GetString(reader.ReadBytes(length));
//            reader.Close();
//            buffer.Close();
//            reader = null;
//            buffer = null;
//        }
//    }
//}
