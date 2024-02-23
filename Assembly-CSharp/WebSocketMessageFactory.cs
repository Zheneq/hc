using NetSerializer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public class WebSocketMessageFactory
{
	public static readonly WebSocketMessageFactory Empty;

	private Dictionary<string, Type> m_typesByName;
	private string m_md5Sum;

	public Serializer BinarySerializer{ get; private set;}
	public string ProtocolVersion
	{
		get { return GetMD5Sum(); }
	}

	static WebSocketMessageFactory()
	{
		Empty = new WebSocketMessageFactory();
		Empty.AddMessageTypes(Type.EmptyTypes);
	}

	public WebSocketMessageFactory()
	{
		m_typesByName = new Dictionary<string, Type>();
		BinarySerializer = new Serializer();
	}

	public void AddMessageTypes(IEnumerable<Type> types)
	{
		foreach (Type type in types)
		{
			m_typesByName.Add(type.Name, type);
		}
		BinarySerializer.AddTypes(types);
		m_md5Sum = null;
	}

	public string GetMD5Sum()
	{
		if (m_md5Sum != null)
		{
			return m_md5Sum;
		}
		using (MD5 mD = MD5.Create())
		{
			HashSet<Type> hashedTypes = new HashSet<Type>();
			foreach (Type type in m_typesByName.Values.OrderBy((Type type) => type.Name))
			{
				AddMD5Sum(mD, type, hashedTypes);
			}
			byte[] inputBuffer = new byte[0];
			mD.TransformFinalBlock(inputBuffer, 0, 0);
			byte[] hash = mD.Hash;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			m_md5Sum = stringBuilder.ToString();
		}
		return m_md5Sum;
	}

	public void AddMD5Sum(MD5 md5, string value)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		md5.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
	}

	private void AddMD5Sum(MD5 md5, Type type, HashSet<Type> hashedTypes)
	{
		AddMD5Sum(md5, type.Name);
		if (type.IsGenericType)
		{
			foreach (Type argType in type.GetGenericArguments())
			{
				AddMD5Sum(md5, argType, hashedTypes);
			}
		}
		if (hashedTypes.Contains(type))
		{
			return;
		}
		hashedTypes.Add(type);
		if (!IsCustomSerialized(type))
		{
			foreach (FieldInfo field in GetFieldInfos(type))
			{
				AddMD5Sum(md5, field.Name);
				AddMD5Sum(md5, field.FieldType, hashedTypes);
			}
		}
	}

	private bool IsCustomSerialized(Type type)
	{
		return type == typeof(DateTime)
		       || type.IsGenericType &&
		       (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
		        type.GetGenericTypeDefinition() == typeof(List<>) ||
		        type.GetGenericTypeDefinition() == typeof(HashSet<>) ||
		        type.GetGenericTypeDefinition() == typeof(Nullable<>));
	}

	private static IEnumerable<FieldInfo> GetFieldInfos(Type type)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		
		IEnumerable<FieldInfo> source = fields.Where(((FieldInfo fi) => (fi.Attributes & FieldAttributes.NotSerialized) == 0));
		
		IOrderedEnumerable<FieldInfo> orderedEnumerable = source.OrderBy(((FieldInfo f) => f.Name), StringComparer.Ordinal);
		if (type.BaseType == null)
		{
			return orderedEnumerable;
		}
		IEnumerable<FieldInfo> fieldInfos = GetFieldInfos(type.BaseType);
		return fieldInfos.Concat(orderedEnumerable);
	}

	public string SerializeToText(WebSocketMessage message)
	{
		string name = message.GetType().Name;
		StringWriter stringWriter = new StringWriter();
		stringWriter.WriteLine(name);
		try
		{
			JsonSerializer jsonSerializer = DefaultJsonSerializer.Get();
			jsonSerializer.Serialize(stringWriter, message);
			return stringWriter.ToString();
		}
		catch (Exception arg)
		{
			Log.Exception(new StringBuilder().Append("Failed to serialize message type ").Append(message.GetType().Name).Append(" : ").Append(arg).ToString());
			throw;
		}
	}

	public WebSocketMessage DeserializeFromText(string text)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		StringReader stringReader = new StringReader(text);
		string text2 = stringReader.ReadLine();
		Type messageType = GetMessageType(text2);
		if (messageType == null)
		{
			if (!text2.IsNullOrEmpty() && text2.IndexOfAny(new char[]{ '{', ' ', '}' }) < 0)
			{
				throw new Exception(new StringBuilder().Append("Message type ").Append(text2).Append(" not found").ToString());
			}
			throw new Exception(new StringBuilder().Append("Message type not parsed").ToString());
		}
		try
		{
			WebSocketMessage webSocketMessage = (WebSocketMessage)DefaultJsonSerializer.Get().Deserialize(new JsonTextReader(stringReader), messageType);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = text.Length;
			return webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(new StringBuilder().Append("Failed to deserialize message type ").Append(messageType.Name).Append(" : ").Append(arg).Append(" \n").Append(text).ToString());
			throw;
		}
	}

	public WebSocketMessage DeserializeFromText(string messageTypeName, string text)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		StringReader reader = new StringReader(text);
		Type messageType = GetMessageType(messageTypeName);
		if (messageType == null)
		{
			throw new Exception(new StringBuilder().Append("Message type ").Append(messageTypeName).Append(" not found").ToString());
		}
		try
		{
			WebSocketMessage webSocketMessage = (WebSocketMessage)DefaultJsonSerializer.Get().Deserialize(new JsonTextReader(reader), messageType);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = text.Length;
			return webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(new StringBuilder().Append("Failed to deserialize message type ").Append(messageType.Name).Append(" : ").Append(arg).Append(" \n").Append(text).ToString());
			throw;
		}
	}

	public byte[] SerializeToBytes(WebSocketMessage message)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			BinarySerializer.Serialize(memoryStream, message);
			return memoryStream.ToArray();
		}
		catch (Exception arg)
		{
			Log.Exception(new StringBuilder().Append("Failed to serialize message type ").Append(message.GetType().Name).Append(" : ").Append(arg).ToString());
			throw;
		}
	}

	public WebSocketMessage DeserializeFromBytes(byte[] bytes)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			MemoryStream stream = new MemoryStream(bytes);
			WebSocketMessage webSocketMessage = (WebSocketMessage)BinarySerializer.Deserialize(stream);
			webSocketMessage.DeserializationTicks = stopwatch.ElapsedTicks;
			webSocketMessage.SerializedLength = bytes.Length;
			return webSocketMessage;
		}
		catch (Exception arg)
		{
			Log.Exception(new StringBuilder().Append("Failed to deserialize message: ").Append(arg).Append(" \n").Append(bytes.ToHexString()).ToString());
			throw;
		}
	}

	public Type GetMessageType(string name)
	{
		return m_typesByName.TryGetValue(name);
	}
}
