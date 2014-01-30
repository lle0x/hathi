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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Windows.Forms;

using Hathi.Classes;
using Hathi.eDonkey;
using Hathi.eDonkey.InterfaceGateway;
using Hathi.eDonkey.Utils;
using Hathi.Types;

using ICSharpCode.SharpZipLib;

namespace Hathi.UI.Winform
{
/// <summary>
/// Brief description of eDonkeyRemoting
/// </summary>
public class CedonkeyCRemote
{
    private TcpClientChannel m_lPhantChannel;

    private IPAddress m_ip;
    public IPAddress ip() {
        return m_ip;
    }

    private int m_Port=4670;
    public int Port() {
        return m_Port;
    }

    private string m_url;
    public string url() {
        return m_url;
    }

    private int m_Port2;
    public int Port2() {
        return m_Port2;
    }

    public CInterfaceGateway  remoteInterface;
    private Hashtable props ;
    private IClientChannelSinkProvider provider;

    public CedonkeyCRemote()
    {
        props = new Hashtable();
        props.Add("name","HathiClient");
        props.Add("priority","10");
        props.Add("port",0);
#if (!COMPRESSION)
        props.Add("supressChannelData",true);
        props.Add("useIpAddress",true);
        props.Add("rejectRemoteRequests",false);
        provider = new BinaryClientFormatterSinkProvider();
#else
        //iniciacion
        Hashtable propsinks = new Hashtable();
        propsinks.Add("includeVersions",true);
        Hashtable datasinks = new Hashtable();
        //2ª Opcion
        provider = new HathiClientSinkProvider(propsinks,datasinks);
#endif
    }
    ~CedonkeyCRemote()
    {
        RemotingConfiguration.Configure(null, true);
        ChannelServices.UnregisterChannel(m_lPhantChannel);
    }
    public void DisConnect()
    {
        try
        {
            RemotingConfiguration.Configure(null, true);
            ChannelServices.UnregisterChannel(m_lPhantChannel);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }
    public bool Connect(string IP,string key,int port2)
    {
        m_ip = IPAddress.Parse(IP);
        if (port2!=0)
            this.m_Port=port2;
        this.m_url="tcp://" + IP + ":" + this.m_Port + "/RemoteInterface";
        this.m_Port2=port2;
        System.Security.Cryptography.MD5 cripto=System.Security.Cryptography.MD5.Create();
        bool valor=false;
        try
        {
            m_lPhantChannel = new TcpClientChannel(props, provider);
            ChannelServices.RegisterChannel(m_lPhantChannel, true);
        }
        catch
        {
            DisConnect();
            if (m_lPhantChannel!=null)
                ChannelServices.RegisterChannel(m_lPhantChannel, true);
            else
                m_lPhantChannel = new TcpClientChannel(props, provider);
        }
        remoteInterface = (CInterfaceGateway) Activator.GetObject(typeof(CInterfaceGateway),
                         this.m_url);
        if (remoteInterface == null)
            Debug.Write("Could not locate the server");
        else
        {
            try
            {
                valor = remoteInterface.CheckPw( key );
            }
            catch
            {
                Debug.Write("\nCould not locate the server\n");
            }
        }
        return valor;
    }
    public bool Connect(string IP,int port3,string key,int port2)
    {
        m_ip = IPAddress.Parse(IP);
        if (port2!=0)
            this.m_Port=port2;
        this.m_url="tcp://" + IP + ":" + this.m_Port + "/RemoteInterface";
        this.m_Port2=port2;
        System.Security.Cryptography.MD5 cripto=System.Security.Cryptography.MD5.Create();
        bool valor=false;
        try
        {
            m_lPhantChannel = new TcpClientChannel(props, provider);
            ChannelServices.RegisterChannel(m_lPhantChannel, true);
        }
        catch
        {
            DisConnect();
            if (m_lPhantChannel!=null)
                ChannelServices.RegisterChannel(m_lPhantChannel, true);
            else
                m_lPhantChannel = new TcpClientChannel(props, provider);
        }
        remoteInterface = (CInterfaceGateway) Activator.GetObject(typeof(CInterfaceGateway),
                         this.m_url);
        if (remoteInterface == null)
            Debug.Write("Could not locate the server");
        else
        {
            //c = new byte[key.Length];
            //for (int i=0;i<key.Length;i++) c[i]=System.Convert.ToByte(key[i]);
            //cripto.ComputeHash(c);
            //try
            //{
            valor = remoteInterface.CheckPw( key );
            /*}
            catch
            {
                ChannelServices.UnregisterChannel(this.canalCeLephant);
                this.canalCeLephant = null;
                Debug.Write("\nNo se pudo encontrar el servidor\n");
            }*/
        }
        return valor;
    }
}

[Serializable]
public class HathiClientSinkProvider : IClientChannelSinkProvider, IClientFormatterSinkProvider
{
    private IClientChannelSinkProvider m_NextChannelSinkProvider=null;
    private IClientChannelSink m_NextChannelSink=null;
    private Hashtable m_Properties=new Hashtable();
    private Hashtable m_ProviderData=new Hashtable();

    #region Constructor
    public HathiClientSinkProvider()
    {
        m_Properties.Add("includeVersions",true);
        //this.Next=new BinaryClientFormatterSinkProvider(m_Properties,m_ProviderData);
    }
    public HathiClientSinkProvider(Hashtable Properties, Hashtable ProviderData)
    {
        this.m_Properties=Properties;
        this.m_ProviderData=ProviderData;
        //this.Next=new BinaryClientFormatterSinkProvider(m_Properties,m_ProviderData);
    }
    #endregion

    #region Implementation of IClientChannelSinkProvider

    public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
    {
        object NextSink=null;
        if (this.m_NextChannelSinkProvider == null)
            this.m_NextChannelSinkProvider=new BinaryClientFormatterSinkProvider(m_Properties,m_ProviderData);
        NextSink = this.m_NextChannelSinkProvider.CreateSink(channel,url,remoteChannelData);
        if (this.m_NextChannelSink==null)
        {
            this.m_NextChannelSink=new HathiClientSink(this,url,NextSink);
            return this.m_NextChannelSink;
        }
        else
        {
            return new HathiClientSink(this,url,NextSink);
        }
    }

    public IClientChannelSinkProvider Next
    {
        get {
            return m_NextChannelSinkProvider;
        }
        set {
            m_NextChannelSinkProvider = value;
        }
    }

    #endregion

}

[Serializable]
public class HathiClientSink:BaseChannelObjectWithProperties,IClientChannelSink,IMessageSink
{
    private string m_url=null;
    private IClientChannelSink m_NextChannelSink=null;
    private IMessageSink m_NextMessageSink=null;
    private HathiClientSinkProvider m_Provider=null;
    private Hashtable m_properties = new Hashtable();
    private Hathi.Classes.Config m_preferences;
    private CompressionType m_CompressionMethod;

    #region Constructor
    public HathiClientSink(IClientChannelSinkProvider Provider, string url)
    {
        object nextobject=new BinaryClientFormatterSink(this);
        this.m_url=url;
        this.m_Provider=Provider as HathiClientSinkProvider;
        this.m_NextChannelSink = nextobject as IClientChannelSink;
        this.m_NextMessageSink = nextobject as IMessageSink;
        m_preferences = new Config(Application.StartupPath, "config.xml", "0.02", "HathiKernel");
        this.m_CompressionMethod=(CompressionType)m_preferences.GetEnum("CompressionMethod",CompressionType.Zip);
    }

    public HathiClientSink(IClientChannelSinkProvider Provider, string url, object nextobject)
    {
        this.m_url=url;
        this.m_Provider=Provider as HathiClientSinkProvider;
        if (nextobject != null)
        {
            this.m_NextChannelSink = nextobject as IClientChannelSink;
            if (this.m_NextChannelSink ==null)
                this.m_NextChannelSink = (IClientChannelSink)new BinaryClientFormatterSink(this);
            this.m_NextMessageSink = nextobject as IMessageSink;
            if (this.m_NextMessageSink == null)
                this.m_NextMessageSink = (IMessageSink)new BinaryClientFormatterSink(this);
        }
        m_preferences = new Config(Application.StartupPath, "config.xml", "0.02", "HathiKernel");
        this.m_CompressionMethod=(CompressionType)m_preferences.GetEnum("CompressionMethod",CompressionType.Zip);
    }
    #endregion

    #region public methods
    public void SetNext(object nextobject)
    {
        this.m_NextChannelSink = nextobject as IClientChannelSink;
        this.m_NextMessageSink = nextobject as IMessageSink;
    }
    #endregion

    #region Implementation of IClientChannelSink

    public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
    {
        object state = null;
        //requestStream esta cerrado, sustitucion por otra.
        if (!stream.CanWrite)
            OpenStream(ref stream,
                        System.Convert.ToInt32((string)headers["CompressedSize"]) );
        //Procesar el mensaje
        ProcessRequest(msg, headers, ref stream, ref state);
        try
        {
            if ( (bool)state)
                this.m_NextChannelSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
    }

    public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
    {
        // Initialize the variable, the response is not intialized, view the documentation for this function
        responseHeaders = null;
        responseStream = null;
        object state = null;
        //The requestStream is closed so we need to open it
        if (!requestStream.CanWrite)
            OpenStream(ref requestStream);
        //Process the message
        ProcessRequest(msg, requestHeaders, ref requestStream, ref state);
        // Send it through the channel.
        try
        {
            if ( (bool)state)
                this.m_NextChannelSink.ProcessMessage(msg, requestHeaders, requestStream,
                                                      out responseHeaders, out responseStream);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
        //The requestStream is closed so we need to open it
        if (!responseStream.CanWrite)
            OpenStream(ref responseStream,
                        System.Convert.ToInt32( (string)responseHeaders["CompressedSize"]) );
        //Post processing message
        ProcessResponse(null, responseHeaders, ref responseStream, state);
        //Depuracion
        /*if (msg.Properties["__MethodName"]==null)
            Console.WriteLine("Peticion desconocida");
        else
            Console.WriteLine("Peticion:" + (string)msg.Properties["__MethodName"]);
        Console.WriteLine("Datos enviados:" +
            System.Convert.ToString(requestHeaders["Tamaño"]) + "/" +
            System.Convert.ToString(requestHeaders["TamañoComprimido"]) );
        Console.WriteLine("Datos recibidos:" +
            (string)responseHeaders["Tamaño"] + "/" +
            (string)responseHeaders["TamañoComprimido"] );
        *///fin depuracion
        //Se debe devolver los datos en la variable original que debe salir de esta funcion para que la
        //interprete el sistema.
        //responseStream=null;
        //responseStream=my_responseStream;
    }

    public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
    {
        if (!stream.CanWrite)
        {
            try
            {
                OpenStream(ref stream);
            }
            catch
            {
                OpenStream(ref stream,
                            System.Convert.ToInt32( (string)headers["CompressedSize"]) );
            }
        }
        ProcessResponse(null, headers, ref stream, state);
        try
        {
            sinkStack.AsyncProcessResponse(headers, stream);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
    }

    public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
    {
        return this.m_NextChannelSink.GetRequestStream(msg, headers);
    }

    public IClientChannelSink NextChannelSink
    {
        get {
            return this.m_NextChannelSink;
        }
    }

    #endregion

    #region Implementation of IChannelSinkBase

    IDictionary IChannelSinkBase.Properties
    {
        get {
            return m_properties;
        }
    }

    #endregion

    #region Implementation of IMessageSink

    //IMesageSink may process messages but they contain no headers.  There is no implementation
    //for IMessageSink on the server
    public IMessage SyncProcessMessage(IMessage msg)
    {
        IMessage respMsg = null;
        try
        {
            respMsg = this.m_NextMessageSink.SyncProcessMessage(msg);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
        return respMsg;
    }

    public IMessageSink NextSink
    {
        get {
            return this.m_NextMessageSink;
        }
    }

    public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
    {
        IMessageCtrl respMsg = null;
        try
        {
            respMsg = this.m_NextMessageSink.AsyncProcessMessage(msg, replySink);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
        return respMsg;
    }

    #endregion

    #region Process the message
    private void ProcessRequest(IMessage message, ITransportHeaders headers, ref Stream stream, ref object state)
    {
        state = true;
        if (headers!=null)
        {
            //Compare the requests
            Compresion compressor=new Compresion(stream,  m_CompressionMethod  );
            Stream compressed=compressor.ToStream;
            if (compressed != null)
            {
                if (compressed.Length < stream.Length)
                {
                    headers["edonkeyCompress"] = "Yes";
                    headers["CompressedSize"]=compressed.Length;
                    headers["Compressed"]=stream.Length;
                    headers["CompressionType"]= (int)compressor.CompressionProvider;
                    stream=compressed;
                }
            }
        }
    }
    private void ProcessResponse(IMessage message, ITransportHeaders headers, ref Stream stream, object state)
    {
        if (headers != null)
        {
            if ((string)headers["edonkeyCompress"] == "Yes")
            {
                //Descomprimir respuesta
                CompressionType c=CompressionType.Zip;
                int com=System.Convert.ToInt32(headers["CompressionType"]);
                if (Enum.GetName(typeof(CompressionType),com) != null)
                {
                    c =(CompressionType)com;
                    Descompresion descompressor =new Descompresion(stream,c );
                    Stream decompressed = descompressor.ToStream;
                    if (decompressed != null)
                    {
                        headers["CompressionType"] = null;
                        headers["edonkeyCompress"] = null;
                        stream = decompressed;
                    }
                }
                else
                {
                    stream=null;
                }
            }
        }
    }
    #endregion

    #region Other Functions
    private void OpenStream(ref Stream stream)
    {
        OpenStream(ref stream,(int)stream.Length);
    }
    private void OpenStream(ref Stream stream,int compression)
    {
        MemoryStream ms = new MemoryStream(compression);
        byte[] Data = new byte[compression];
        stream.Read(Data,0,compression);
        ms.Write(Data,0,compression);
        stream=null;
        stream=ms;
    }

    #endregion
}
}
