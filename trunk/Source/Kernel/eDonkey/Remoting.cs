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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Collections;
using System.Runtime.Serialization.Formatters;

//sinks
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using Hathi.eDonkey.InterfaceGateway;


namespace Hathi.eDonkey
{
/// <summary>
/// Descripción breve de CRemoting.
/// Servidor remoto, registrar el canal, y publicar la clase.
/// Previsto algun tipo de autentificación.
/// </summary>
internal class CRemoting
{
    private TcpServerChannel m_lPhantChannel;
    private int puerto;
    private string[] IPPermitida;
    public string[] GetIPPermitida() {
        return IPPermitida;
    }
    public CRemoting()
    {
        //
        // TODO: agregar aquí la lógica del constructor
        //
        puerto = CKernel.Preferences.GetInt("RemoteControlPort",4670);
        IPPermitida = CKernel.Preferences.GetStringArray("AllowedIP");
        Hashtable props = new Hashtable();
        props.Add("name","HathiService");
        props.Add("priority","10"); //en la ayuda pone que son enteros, pero con ellos da error de conversion.
        props.Add("port", puerto);
        props.Add("supressChannelData",true);
        props.Add("useIpAddress",true);
        props.Add("rejectRemoteRequests",false);
#if (!COMPRESSION)
        BinaryServerFormatterSinkProvider provider =
            new BinaryServerFormatterSinkProvider();
        provider.TypeFilterLevel =
            System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
        m_lPhantChannel = new TcpServerChannel(props, provider);
        ChannelServices.RegisterChannel(m_lPhantChannel, true);
#else
        //Iniciacion
        Hashtable propsinks = new Hashtable();
        propsinks.Add("includeVersions",true);
        propsinks.Add("typeFilterLevel","Full");
        Hashtable datasinks = new Hashtable();
        //2ª Opcion
        HathiServerSinkProvider provider =
            new HathiServerSinkProvider(propsinks,datasinks);
        //Creacion
        m_lPhantChannel = new TcpServerChannel(props, provider);
        ChannelServices.RegisterChannel(m_lPhantChannel, true);
#endif
        RemotingConfiguration.RegisterWellKnownServiceType(
            typeof(CInterfaceGateway),
            "InterfazRemota",
            WellKnownObjectMode.Singleton);
    }

    ~CRemoting()
    {
        ChannelServices.UnregisterChannel(m_lPhantChannel);
        RemotingConfiguration.Configure(null,true);
    }
}
}