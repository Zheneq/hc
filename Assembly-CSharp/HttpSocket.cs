using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

	public WebSocketMessage Message
	{
		get { return m_requestMessage; }
	}

	public int MaxMessageSize
	{
		get;
		set;
	}

	public HttpListenerRequest Request
	{
		get { return m_requestArgs.Request; }
	}

	public HttpListenerResponse Response
	{
		get { return m_requestArgs.Response; }
	}

	public string ConnectionAddress
	{
		get;
		private set;
	}

	public string ForwardedConnectionAddress
	{
		get;
		private set;
	}

	public WebSocketMessageFactory MessageFactory
	{
		get;
		private set;
	}

	public HttpSocket(WebSocketMessageFactory factory)
	{
		m_event = new ManualResetEvent(false);
		m_lock = new object();
		m_timeoutMs = 60000;
		m_serializer = DefaultJsonSerializer.Get();
		MaxMessageSize = 1048576;
		MessageFactory = factory;
	}

	public void HandleRequest(HttpRequestEventArgs requestArgs)
	{
		int num = (int)requestArgs.Request.ContentLength64;
		if (MaxMessageSize > 0)
		{
			if (num > MaxMessageSize)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						throw new Exception("Invalid HTTP request input stream length");
					}
				}
			}
		}
		m_requestArgs = requestArgs;
		string[] array = requestArgs.Request.Url.Query.Split('?', '&');
		string str = "{\n";
		string text = null;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2.IsNullOrEmpty())
			{
				continue;
			}
			string[] array3 = text2.Split('=');
			string text3 = array3[0];
			object obj;
			if (array3.Length > 1)
			{
				obj = array3[1];
			}
			else
			{
				obj = "True";
			}
			string text4 = (string)obj;
			str += new StringBuilder().Append("\"").Append(text3).Append("\" : \"").Append(text4).Append("\", ").ToString();
			if (text3 == "messageType")
			{
				text = text4;
			}
			if (text3 == "formatted")
			{
				if (text4.ToLower() == "true")
				{
					m_serializer = FormattedJsonSerializer.Get();
				}
			}
			if (text3 == "timeoutMs")
			{
				m_timeoutMs = Convert.ToInt32(text4);
				if (m_timeoutMs > 60000)
				{
					m_timeoutMs = 60000;
				}
				if (m_timeoutMs < -1)
				{
					m_timeoutMs = -1;
				}
			}
			dictionary.Add(text3, text4);
		}
		while (true)
		{
			str += "\n}";
			ConnectionAddress = requestArgs.Request.RemoteEndPoint.ToString();
			ForwardedConnectionAddress = requestArgs.Request.Headers["X-Forwarded-For"];
			string text5 = new StreamReader(requestArgs.Request.InputStream).ReadToEnd();
			if (text5.IsNullOrEmpty())
			{
				text5 = str;
			}
			if (!text.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_requestMessage = MessageFactory.DeserializeFromText(text, text5);
						return;
					}
				}
			}
			m_requestMessage = new HttpRequestMessage
			{
				Method = requestArgs.Request.HttpMethod,
				Url = requestArgs.Request.Url,
				QueryParams = dictionary,
				RequestData = text5,
				Serializer = m_serializer
			};
			return;
		}
	}

	public void Send(object message)
	{
		lock (m_lock)
		{
			if (m_responseSent)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			HttpResponseMessage httpResponseMessage = message as HttpResponseMessage;
			if (httpResponseMessage != null)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(httpResponseMessage.ResponseData);
				Response.ContentType = httpResponseMessage.ContentType;
				Response.StatusCode = httpResponseMessage.StatusCode;
				Response.ContentLength64 = bytes.Length;
				Response.OutputStream.Write(bytes, 0, bytes.Length);
			}
			else
			{
				WebSocketMessage webSocketMessage = message as WebSocketMessage;
				string s;
				if (webSocketMessage != null)
				{
					StringWriter stringWriter = new StringWriter();
					m_serializer.Serialize(stringWriter, webSocketMessage);
					s = stringWriter.ToString();
				}
				else
				{
					s = message.ToString();
				}
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				Response.ContentType = "text/json";
				Response.StatusCode = 200;
				Response.ContentLength64 = bytes2.Length;
				Response.OutputStream.Write(bytes2, 0, bytes2.Length);
			}
			Response.OutputStream.Close();
			m_responseSent = true;
			m_event.Set();
		}
	}

	public void WaitForSend()
	{
		if (m_event.WaitOne(m_timeoutMs))
		{
			return;
		}
		while (true)
		{
			lock (m_lock)
			{
				if (!m_responseSent)
				{
					Response.StatusCode = 408;
					m_responseSent = true;
				}
			}
			return;
		}
	}
}
