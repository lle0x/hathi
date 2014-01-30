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
using System.IO;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Net;

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Hathi.eDonkey.Commands
{
/// <summary>
/// Summary description for edonkeyCommands.
/// </summary>
internal class DonkeyHeader
{
    public Protocol.ProtocolType eDonkeyID;
    public uint Packetlength;
    public byte Command;

    public DonkeyHeader(byte command)
    {
        eDonkeyID=Protocol.ProtocolType.eDonkey;
        Command=command;
        Packetlength=6;
    }

    public DonkeyHeader(byte command,Protocol.ProtocolType protocol)
    {
        eDonkeyID=protocol;
        Command=command;
        Packetlength=6;
    }

    public DonkeyHeader(byte command,BinaryWriter writer)
    {
        eDonkeyID=Protocol.ProtocolType.eDonkey;
        Command=command;
        Packetlength=6;
        Serialize(writer);
    }

    public DonkeyHeader(byte command,BinaryWriter writer,Protocol.ProtocolType protocol)
    {
        eDonkeyID=protocol;
        Command=command;
        Packetlength=6;
        Serialize(writer);
    }

    public DonkeyHeader(MemoryStream buffer)
    {
        BinaryReader reader=new BinaryReader(buffer);
        eDonkeyID=(Protocol.ProtocolType)reader.ReadByte();
        Packetlength=(uint)reader.ReadInt32();
        Command=reader.ReadByte();
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)eDonkeyID);
        writer.Write(Packetlength);
        writer.Write(Command);
    }
}

#region Parameter Reader & Writer
/*
#define TAGTYPE_HASH            0x01
#define TAGTYPE_STRING          0x02
#define TAGTYPE_UINT32          0x03
#define TAGTYPE_FLOAT32         0x04
#define TAGTYPE_BOOL            0x05
#define TAGTYPE_BOOLARRAY       0x06
#define TAGTYPE_BLOB            0x07
#define TAGTYPE_UINT16          0x08
#define TAGTYPE_UINT8           0x09
*/

internal class CParameterReader
{
    public byte type;
    public byte id;
    public string nombreParam;
    public string valorString;
    public uint valorNum;

    public CParameterReader(BinaryReader reader)
    {
        ushort longitud;
        type=reader.ReadByte();
        longitud=reader.ReadUInt16();
        //Console.WriteLine("type: " + type.ToString());
        if (longitud==1)
            id=reader.ReadByte();
        else if (longitud>0) nombreParam=new string(reader.ReadChars(longitud));
        if (type==2)
        {
            longitud=reader.ReadUInt16();
            //valorString=new string(reader.ReadChars(longitud));
            //valorString=Encoding.UTF8.GetString(reader.ReadBytes(longitud));
            byte [] buf;//=new byte[longitud];
            buf=reader.ReadBytes(longitud);
            valorString=Encoding.Default.GetString(buf);
        }
        else if (type==3) valorNum=reader.ReadUInt32();
        else if (type==4) reader.ReadBytes(4);
        else if (type==1) reader.ReadBytes(16);
        else if (type==5) reader.ReadByte();
    }
}

internal class CParametersSaver
{
    public CParametersSaver(byte type, byte id, string name, string valueString, uint valueNum, BinaryWriter writer)
    {
        switch (type)
        {
        case 2:
            if (name!=null)
                new ParameterWriter(name,valueString,writer);
            else
                new ParameterWriter(id,valueString,writer);
            break;
        case 3:
            if (name!=null)
                new ParameterWriter(name,valueNum,writer);
            else
                new ParameterWriter(id,valueNum,writer);
            break;
        }
    }
}

internal struct ParameterWriter
{
    private const short typeParameter = 1;

    private byte m_type;
    private object m_id;
    private object m_value;

    public ParameterWriter(byte id, uint value, BinaryWriter writer)
    {
        m_type = 3;
        m_id = id;
        m_value = value;
        Serialize(writer);
    }

    public ParameterWriter(byte id, uint value)
    {
        m_type = 3;
        m_id = id;
        m_value = value;
    }

    public ParameterWriter(byte id, string value, BinaryWriter writer)
    {
        m_type = 2;
        m_id = id;
        m_value = value;
        Serialize(writer);
    }

    public ParameterWriter(byte id, string value)
    {
        m_type = 2;
        m_id = id;
        m_value = value;
    }

    public ParameterWriter(string id, uint value, BinaryWriter writer)
    {
        m_type = 3;
        m_id = id;
        m_value = value;
        Serialize(writer);
    }

    public ParameterWriter(string id, uint value)
    {
        m_type = 3;
        m_id = id;
        m_value = value;
    }

    public ParameterWriter(string id, string value, BinaryWriter writer)
    {
        m_type = 2;
        m_id = id;
        m_value = value;
        Serialize(writer);
    }

    public ParameterWriter(string id, string value)
    {
        m_type = 2;
        m_id = id;
        m_value = value;
    }

    public void Serialize(BinaryWriter writer)
    {
        if (m_id is string)
            SerializeString(writer);
        else
            SerializeNumeric(writer);
    }

    internal void SerializeNumeric(BinaryWriter writer)
    {
        byte id = (byte)m_id;
        writer.Write(m_type);
        writer.Write(typeParameter);
        writer.Write(id);
        if (m_value is string)
        {
            string value = (string)m_value;
            byte[] buffer = Encoding.Default.GetBytes(value);
            writer.Write((ushort)buffer.Length);
            writer.Write(buffer);
        }
        else
        {
            uint value = (uint)m_value;
            writer.Write(value);
        }
    }

    internal void SerializeString(BinaryWriter writer)
    {
        string id = (string)m_id;
        writer.Write(m_type);
        writer.Write((ushort)id.ToCharArray().Length);
        writer.Write(id.ToCharArray());
        if (m_value is string)
        {
            string value = (string)m_value;
            writer.Write((ushort)value.ToCharArray().Length);
            writer.Write(value.ToCharArray());
        }
        else
        {
            uint value = (uint)m_value;
            writer.Write(value);
        }
    }
}

#endregion

public struct stDatosFuente
{
    public uint IP;
    public ushort Port;
    public uint ServerIP;
    public ushort ServerPort;
}
	
}
