﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NetSerializer;
using Newtonsoft.Json;

public class WebSocketMessageFactory
{
	public static readonly WebSocketMessageFactory Empty = new WebSocketMessageFactory();

	private Dictionary<string, Type> m_typesByName;

	private string m_md5Sum;

	static WebSocketMessageFactory()
	{
		WebSocketMessageFactory.Empty.AddMessageTypes(new Type[0]);
	}

	public WebSocketMessageFactory()
	{
		this.m_typesByName = new Dictionary<string, Type>();
		this.BinarySerializer = new Serializer();
	}

	public Serializer BinarySerializer { get; private set; }

	public void AddMessageTypes(IEnumerable<Type> types)
	{
		IEnumerator<Type> enumerator = types.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				this.m_typesByName.Add(type.Name, type);
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		this.BinarySerializer.AddTypes(types);
		this.m_md5Sum = null;
	}

	public string ProtocolVersion
	{
		get
		{
			return this.GetMD5Sum();
		}
	}

	public string GetMD5Sum()
	{
		if (this.m_md5Sum != null)
		{
			return this.m_md5Sum;
		}
		MD5 md = MD5.Create();
		try
		{
			HashSet<Type> hashedTypes = new HashSet<Type>();
			IEnumerator<Type> enumerator = (from type in this.m_typesByName.Values
			orderby type.Name
			select type).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Type type2 = enumerator.Current;
					this.AddMD5Sum(md, type2, hashedTypes);
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			byte[] inputBuffer = new byte[0];
			md.TransformFinalBlock(inputBuffer, 0, 0);
			byte[] hash = md.Hash;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				stringBuilder.AppendFormat("{0:x2}", hash[i]);
			}
			this.m_md5Sum = stringBuilder.ToString();
		}
		finally
		{
			if (md != null)
			{
				((IDisposable)md).Dispose();
			}
		}
		return this.m_md5Sum;
	}

	public void AddMD5Sum(MD5 md5, string value)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		md5.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
	}

	private void AddMD5Sum(MD5 md5, Type type, HashSet<Type> hashedTypes)
	{
		this.AddMD5Sum(md5, type.Name);
		if (type.IsGenericType)
		{
			foreach (Type type2 in type.GetGenericArguments())
			{
				this.AddMD5Sum(md5, type2, hashedTypes);
			}
		}
		if (!hashedTypes.Contains(type))
		{
			hashedTypes.Add(type);
			if (!this.IsCustomSerialized(type))
			{
				IEnumerator<FieldInfo> enumerator = WebSocketMessageFactory.GetFieldInfos(type).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						FieldInfo fieldInfo = enumerator.Current;
						this.AddMD5Sum(md5, fieldInfo.Name);
						this.AddMD5Sum(md5, fieldInfo.FieldType, hashedTypes);
					}
				}
				finally
				{
					if (enumerator != null)
					{
						enumerator.Dispose();
					}
				}
			}
		}
	}

	private bool IsCustomSerialized(Type type)
	{
		if (type == typeof(DateTime))
		{
			return true;
		}
		if (type.IsGenericType)
		{
			if (type.GetGenericTypeDefinition() != typeof(Dictionary<, >) && type.GetGenericTypeDefinition() != typeof(List<>))
			{
				if (type.GetGenericTypeDefinition() != typeof(HashSet<>) && type.GetGenericTypeDefinition() != typeof(Nullable<>))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private static IEnumerable<FieldInfo> GetFieldInfos(Type type)
	{
		IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		
		IEnumerable<FieldInfo> source = fields.Where(((FieldInfo fi) => (fi.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope));
		
		IOrderedEnumerable<FieldInfo> orderedEnumerable = source.OrderBy(((FieldInfo f) => f.Name), StringComparer.Ordinal);
		if (type.BaseType == null)
		{
			return orderedEnumerable;
		}
		IEnumerable<FieldInfo> fieldInfos = WebSocketMessageFactory.GetFieldInfos(type.BaseType);
		return fieldInfos.Concat(orderedEnumerable);
	}

	public string SerializeToText(WebSocketMessage message)
	{
		string name = message.GetType().Name;
		StringWriter stringWriter = new StringWriter();
		stringWriter.WriteLine(name);
		string result;
		try
		{
			JsonSerializer jsonSerializer = DefaultJsonSerializer.Get();
			jsonSerializer.Serialize(stringWriter, message);
			result = stringWriter.ToString();
		}
		catch (Exception arg)
		{
			Log.Exception(string.Format("Failed to serialize message type {0} : {1}", message.GetType().Name, arg), new object[0]);
			throw;
		}
		return result;
	}

	public WebSocketMessage DeserializeFromText(string text)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		StringReader stringReader = new StringReader(text);
		string text2 = stringReader.ReadLine();
		Type messageType = this.GetMessageType(text2);
		if (messageType == null)
		{
			if (!text2.IsNullOrEmpty())
			{
				if (text2.IndexOfAny(new char[]
				{
					'{',
					' ',
					'}'
				}) < 0)
				{
					throw new Exception(string.Format("Message type {0} not found", text2));
				}
			}
			throw new Exception(string.Format("Message type not parsed", new object[0]));
		}
		WebSocketMessage result;
		try
		{
			WebSocketMessage webSocketMessage = (WebSocketMessage)DefaultJsonSerializer.Get().Deserialize(new JsonTextReader(stringReader), messageType);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = (long)text.Length;
			result = webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(string.Format("Failed to deserialize message type {0} : {1} \n{2}", messageType.Name, arg, text), new object[0]);
			throw;
		}
		return result;
	}

	public WebSocketMessage DeserializeFromText(string messageTypeName, string text)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		StringReader reader = new StringReader(text);
		Type messageType = this.GetMessageType(messageTypeName);
		if (messageType == null)
		{
			throw new Exception(string.Format("Message type {0} not found", messageTypeName));
		}
		WebSocketMessage result;
		try
		{
			WebSocketMessage webSocketMessage = (WebSocketMessage)DefaultJsonSerializer.Get().Deserialize(new JsonTextReader(reader), messageType);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = (long)text.Length;
			result = webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(string.Format("Failed to deserialize message type {0} : {1} \n{2}", messageType.Name, arg, text), new object[0]);
			throw;
		}
		return result;
	}

	public byte[] SerializeToBytes(WebSocketMessage message)
	{
		byte[] result;
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			this.BinarySerializer.Serialize(memoryStream, message);
			result = memoryStream.ToArray();
		}
		catch (Exception arg)
		{
			Log.Exception(string.Format("Failed to serialize message type {0} : {1}", message.GetType().Name, arg), new object[0]);
			throw;
		}
		return result;
	}

	public WebSocketMessage DeserializeFromBytes(byte[] bytes)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		WebSocketMessage result;
		try
		{
			MemoryStream stream = new MemoryStream(bytes);
			WebSocketMessage webSocketMessage = (WebSocketMessage)this.BinarySerializer.Deserialize(stream);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = (long)bytes.Length;
			result = webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(string.Format("Failed to deserialize message: {0} \n{1}", arg, bytes.ToHexString()), new object[0]);
			throw;
		}
		return result;
	}

	public Type GetMessageType(string name)
	{
		return this.m_typesByName.TryGetValue(name);
	}
}
