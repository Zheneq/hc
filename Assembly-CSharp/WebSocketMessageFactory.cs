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

	public Serializer BinarySerializer
	{
		get;
		private set;
	}

	public string ProtocolVersion => GetMD5Sum();

	static WebSocketMessageFactory()
	{
		Empty = new WebSocketMessageFactory();
		Empty.AddMessageTypes(new Type[0]);
	}

	public WebSocketMessageFactory()
	{
		m_typesByName = new Dictionary<string, Type>();
		BinarySerializer = new Serializer();
	}

	public void AddMessageTypes(IEnumerable<Type> types)
	{
		IEnumerator<Type> enumerator = types.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Type current = enumerator.Current;
				m_typesByName.Add(current.Name, current);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_0007;
				}
			}
			end_IL_0007:;
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0043;
					}
				}
			}
			end_IL_0043:;
		}
		BinarySerializer.AddTypes(types);
		m_md5Sum = null;
	}

	public string GetMD5Sum()
	{
		if (m_md5Sum != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_md5Sum;
				}
			}
		}
		MD5 mD = MD5.Create();
		try
		{
			HashSet<Type> hashedTypes = new HashSet<Type>();
			IEnumerator<Type> enumerator = m_typesByName.Values.OrderBy((Type type) => type.Name).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.Current;
					AddMD5Sum(mD, current, hashedTypes);
				}
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_008d;
						}
					}
				}
				end_IL_008d:;
			}
			byte[] inputBuffer = new byte[0];
			mD.TransformFinalBlock(inputBuffer, 0, 0);
			byte[] hash = mD.Hash;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				stringBuilder.AppendFormat("{0:x2}", hash[i]);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_md5Sum = stringBuilder.ToString();
					goto end_IL_0028;
				}
			}
			end_IL_0028:;
		}
		finally
		{
			if (mD != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						((IDisposable)mD).Dispose();
						goto end_IL_010a;
					}
				}
			}
			end_IL_010a:;
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
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type type2 in genericArguments)
			{
				AddMD5Sum(md5, type2, hashedTypes);
			}
		}
		if (hashedTypes.Contains(type))
		{
			return;
		}
		while (true)
		{
			hashedTypes.Add(type);
			if (!IsCustomSerialized(type))
			{
				while (true)
				{
					IEnumerator<FieldInfo> enumerator = GetFieldInfos(type).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							FieldInfo current = enumerator.Current;
							AddMD5Sum(md5, current.Name);
							AddMD5Sum(md5, current.FieldType, hashedTypes);
						}
						while (true)
						{
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_00d2;
								}
							}
						}
						end_IL_00d2:;
					}
				}
			}
			return;
		}
	}

	private bool IsCustomSerialized(Type type)
	{
		if (type == typeof(DateTime))
		{
			while (true)
			{
				return true;
			}
		}
		if (type.IsGenericType)
		{
			if (type.GetGenericTypeDefinition() != typeof(Dictionary<, >) && type.GetGenericTypeDefinition() != typeof(List<>))
			{
				if (type.GetGenericTypeDefinition() != typeof(HashSet<>) && type.GetGenericTypeDefinition() != typeof(Nullable<>))
				{
					goto IL_0096;
				}
			}
			return true;
		}
		goto IL_0096;
		IL_0096:
		return false;
	}

	private static IEnumerable<FieldInfo> GetFieldInfos(Type type)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = ((FieldInfo fi) => (fi.Attributes & FieldAttributes.NotSerialized) == 0);
		}
		IEnumerable<FieldInfo> source = fields.Where(_003C_003Ef__am_0024cache1);
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = ((FieldInfo f) => f.Name);
		}
		IOrderedEnumerable<FieldInfo> orderedEnumerable = source.OrderBy(_003C_003Ef__am_0024cache2, StringComparer.Ordinal);
		if (type.BaseType == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return orderedEnumerable;
				}
			}
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
			Log.Exception($"Failed to serialize message type {message.GetType().Name} : {arg}");
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (!text2.IsNullOrEmpty())
					{
						if (text2.IndexOfAny(new char[3]
						{
							'{',
							' ',
							'}'
						}) < 0)
						{
							throw new Exception($"Message type {text2} not found");
						}
					}
					throw new Exception($"Message type not parsed");
				}
			}
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
			Log.Exception($"Failed to deserialize message type {messageType.Name} : {arg} \n{text}");
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					throw new Exception($"Message type {messageTypeName} not found");
				}
			}
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
			Log.Exception($"Failed to deserialize message type {messageType.Name} : {arg} \n{text}");
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
			Log.Exception($"Failed to serialize message type {message.GetType().Name} : {arg}");
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
			Log.Exception($"Failed to deserialize message: {arg} \n{bytes.ToHexString()}");
			throw;
		}
	}

	public Type GetMessageType(string name)
	{
		return m_typesByName.TryGetValue(name);
	}
}
