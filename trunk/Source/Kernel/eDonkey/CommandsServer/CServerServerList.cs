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
	/// CServerServerList process the serverlist send by/to CServer.
	/// </summary>
	internal class CServerServerList
	{
		public byte NewServers;

		public CServerServerList(MemoryStream buffer)
		{
			DonkeyHeader header;
			BinaryWriter writer = new BinaryWriter(buffer);
			header = new DonkeyHeader((byte)Protocol.ServerCommand.GetServerList, writer);
			header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
			writer.Seek(0, SeekOrigin.Begin);
			header.Serialize(writer);
		}

		public CServerServerList(MemoryStream buffer, CServersList ServerList)
		{
			BinaryReader reader = new BinaryReader(buffer);
			byte nServers = reader.ReadByte();
			uint IP;
			ushort port;
			NewServers = 0;
			try
			{
				for (int i = 0; i != nServers; i++)
				{
					IP = reader.ReadUInt32();
					port = reader.ReadUInt16();
					if (ServerList.Add(IP, port) != null) NewServers++;
				}
			}
			catch { }
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
//    /// CServerServerList process the serverlist send by/to CServer.
//    /// </summary>
//    internal class CServerServerList
//    {
//        public byte NewServers;

//        public CServerServerList(MemoryStream buffer)
//        {
//            DonkeyHeader header;
//            BinaryWriter writer = new BinaryWriter(buffer);
//            header = new DonkeyHeader((byte)Protocol.ServerCommand.GetServerList, writer);
//            header.Packetlength = (uint)writer.BaseStream.Length - header.Packetlength + 1;
//            writer.Seek(0, SeekOrigin.Begin);
//            header.Serialize(writer);
//        }

//        public CServerServerList(MemoryStream buffer, CServersList ServerList)
//        {
//            BinaryReader reader = new BinaryReader(buffer);
//            byte nServers = reader.ReadByte();
//            uint IP;
//            ushort port;
//            NewServers = 0;
//            try
//            {
//                for (int i = 0; i != nServers; i++)
//                {
//                    IP = reader.ReadUInt32();
//                    port = reader.ReadUInt16();
//                    if (ServerList.Add(IP, port) != null) NewServers++;
//                }
//            }
//            catch { }
//            reader.Close();
//            buffer.Close();
//            reader = null;
//            buffer = null;
//        }
//    }
//}
