using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class HttpSocket
{
	private HttpRequestEventArgs m_requestArgs;

	private JsonSerializer m_serializer;

	public WebSocketMessage m_requestMessage;

	private ManualResetEvent m_event;

	private object m_lock;

	private bool m_responseSent;

	private int m_timeoutMs;

	public HttpSocket(WebSocketMessageFactory factory)
	{
		this.m_event = new ManualResetEvent(false);
		this.m_lock = new object();
		this.m_timeoutMs = 0xEA60;
		this.m_serializer = DefaultJsonSerializer.Get();
		this.MaxMessageSize = 0x100000;
		this.MessageFactory = factory;
	}

	public WebSocketMessage Message
	{
		get
		{
			return this.m_requestMessage;
		}
	}

	public int MaxMessageSize { get; set; }

	public HttpListenerRequest Request
	{
		get
		{
			return this.m_requestArgs.Request;
		}
	}

	public HttpListenerResponse Response
	{
		get
		{
			return this.m_requestArgs.Response;
		}
	}

	public string ConnectionAddress { get; private set; }

	public string ForwardedConnectionAddress { get; private set; }

	public WebSocketMessageFactory MessageFactory { get; private set; }

	public void HandleRequest(HttpRequestEventArgs requestArgs)
	{
		int num = (int)requestArgs.Request.ContentLength64;
		if (this.MaxMessageSize > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HttpSocket.HandleRequest(HttpRequestEventArgs)).MethodHandle;
			}
			if (num > this.MaxMessageSize)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				throw new Exception("Invalid HTTP request input stream length");
			}
		}
		this.m_requestArgs = requestArgs;
		string[] array = requestArgs.Request.Url.Query.Split(new char[]
		{
			'?',
			'&'
		});
		string text = "{\n";
		string text2 = null;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string text3 in array)
		{
			if (text3.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				string[] array3 = text3.Split(new char[]
				{
					'='
				});
				string text4 = array3[0];
				string text5;
				if (array3.Length > 1)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					text5 = array3[1];
				}
				else
				{
					text5 = "True";
				}
				string text6 = text5;
				text += string.Format("\"{0}\" : \"{1}\", ", text4, text6);
				if (text4 == "messageType")
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					text2 = text6;
				}
				if (text4 == "formatted")
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (text6.ToLower() == "true")
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_serializer = FormattedJsonSerializer.Get();
					}
				}
				if (text4 == "timeoutMs")
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_timeoutMs = Convert.ToInt32(text6);
					if (this.m_timeoutMs > 0xEA60)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_timeoutMs = 0xEA60;
					}
					if (this.m_timeoutMs < -1)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_timeoutMs = -1;
					}
				}
				dictionary.Add(text4, text6);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		text += "\n}";
		this.ConnectionAddress = requestArgs.Request.RemoteEndPoint.ToString();
		this.ForwardedConnectionAddress = requestArgs.Request.Headers["X-Forwarded-For"];
		string text7 = new StreamReader(requestArgs.Request.InputStream).ReadToEnd();
		if (text7.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			text7 = text;
		}
		if (!text2.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_requestMessage = this.MessageFactory.DeserializeFromText(text2, text7);
		}
		else
		{
			this.m_requestMessage = new HttpRequestMessage
			{
				Method = requestArgs.Request.HttpMethod,
				Url = requestArgs.Request.Url,
				QueryParams = dictionary,
				RequestData = text7,
				Serializer = this.m_serializer
			};
		}
	}

	public void Send(object message)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_responseSent)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(HttpSocket.Send(object)).MethodHandle;
				}
			}
			else
			{
				HttpResponseMessage httpResponseMessage = message as HttpResponseMessage;
				if (httpResponseMessage != null)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(httpResponseMessage.ResponseData);
					this.Response.ContentType = httpResponseMessage.ContentType;
					this.Response.StatusCode = httpResponseMessage.StatusCode;
					this.Response.ContentLength64 = (long)bytes.Length;
					this.Response.OutputStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					WebSocketMessage webSocketMessage = message as WebSocketMessage;
					string s;
					if (webSocketMessage != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						StringWriter stringWriter = new StringWriter();
						this.m_serializer.Serialize(stringWriter, webSocketMessage);
						s = stringWriter.ToString();
					}
					else
					{
						s = message.ToString();
					}
					byte[] bytes2 = Encoding.UTF8.GetBytes(s);
					this.Response.ContentType = "text/json";
					this.Response.StatusCode = 0xC8;
					this.Response.ContentLength64 = (long)bytes2.Length;
					this.Response.OutputStream.Write(bytes2, 0, bytes2.Length);
				}
				this.Response.OutputStream.Close();
				this.m_responseSent = true;
				this.m_event.Set();
			}
		}
	}

	public void WaitForSend()
	{
		bool flag = !this.m_event.WaitOne(this.m_timeoutMs);
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HttpSocket.WaitForSend()).MethodHandle;
			}
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (!this.m_responseSent)
				{
					this.Response.StatusCode = 0x198;
					this.m_responseSent = true;
				}
			}
		}
	}
}
