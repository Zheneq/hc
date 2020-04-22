using NetSerializer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable]
[JsonConverter(typeof(JsonConverter))]
public class RequirementCollection : IEnumerable<QueueRequirement>, IEnumerable
{
	private class JsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(RequirementCollection);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						throw new Exception($"Bad JSON definition of RequirementCollection, expected '[' not {reader.TokenType}='{reader.Value}'");
					}
				}
			}
			reader.Read();
			RequirementCollection requirementCollection = Create();
			while (reader.TokenType != JsonToken.EndArray)
			{
				QueueRequirement item = QueueRequirement.ExtractRequirementFromReader(reader);
				requirementCollection.Add(item);
				reader.Read();
			}
			while (true)
			{
				if (reader.TokenType != JsonToken.EndArray)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							throw new Exception($"Bad JSON definition of RequirementCollection, expected ']' not {reader.TokenType}='{reader.Value}'");
						}
					}
				}
				return requirementCollection;
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			RequirementCollection requirementCollection = value as RequirementCollection;
			writer.WriteStartArray();
			IEnumerator<QueueRequirement> enumerator = requirementCollection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					QueueRequirement current = enumerator.Current;
					writer.WriteStartObject();
					writer.WritePropertyName(current.Requirement.ToString());
					writer.WriteStartObject();
					current.WriteToJson(writer);
					writer.WriteEndObject();
					writer.WriteEndObject();
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_0014;
					}
				}
				end_IL_0014:;
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
							goto end_IL_0075;
						}
					}
				}
				end_IL_0075:;
			}
			writer.WriteEndArray();
		}
	}

	public List<byte[]> RequirementsAsBinaryData;

	[NonSerialized]
	private bool m_dirty = true;

	private List<QueueRequirement> m_queueRequirementAsList = new List<QueueRequirement>();

	private static Serializer s_serializer;

	private static Serializer InternalSerializer
	{
		get
		{
			if (s_serializer == null)
			{
				s_serializer = new Serializer();
				s_serializer.AddTypes(QueueRequirement.MessageTypes);
			}
			return s_serializer;
		}
	}

	private IEnumerator<QueueRequirement> InternalEnumerator()
	{
		if (RequirementsAsBinaryData.IsNullOrEmpty())
		{
			yield break;
		}
		while (true)
		{
			using (List<byte[]>.Enumerator enumerator = RequirementsAsBinaryData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					byte[] data = enumerator.Current;
					MemoryStream stream = new MemoryStream(data);
					InternalSerializer.Deserialize(stream, out object arg);
					if (arg != null)
					{
						if (arg is QueueRequirement)
						{
							yield return arg as QueueRequirement;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				while (true)
				{
					switch (5)
					{
					default:
						yield break;
					case 0:
						break;
					}
				}
			}
		}
	}

	public void Add(QueueRequirement item)
	{
		if (item != null)
		{
			MemoryStream memoryStream = new MemoryStream();
			InternalSerializer.Serialize(memoryStream, item);
			if (RequirementsAsBinaryData == null)
			{
				RequirementsAsBinaryData = new List<byte[]>();
			}
			RequirementsAsBinaryData.Add(memoryStream.ToArray());
		}
		m_dirty = true;
	}

	public void AddRange(IEnumerable<QueueRequirement> collection)
	{
		using (IEnumerator<QueueRequirement> enumerator = collection.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement current = enumerator.Current;
				Add(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_0009;
				}
			}
			end_IL_0009:;
		}
		m_dirty = true;
	}

	public bool Exists(Predicate<QueueRequirement> match)
	{
		using (IEnumerator<QueueRequirement> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement current = enumerator.Current;
				if (match(current))
				{
					return true;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_0009;
				}
			}
			end_IL_0009:;
		}
		return false;
	}

	public RequirementCollection Where(Predicate<QueueRequirement> match)
	{
		RequirementCollection requirementCollection = Create();
		IEnumerator<QueueRequirement> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement current = enumerator.Current;
				if (match(current))
				{
					requirementCollection.Add(current);
				}
			}
			return requirementCollection;
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0036;
					}
				}
			}
			end_IL_0036:;
		}
	}

	public bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return !Exists((QueueRequirement p) => !p.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType));
	}

	public LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType, RequirementMessageContext context)
	{
		IEnumerator<QueueRequirement> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement current = enumerator.Current;
				if (!current.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return current.GenerateFailure(systemInfo, applicant, context);
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_005b;
					}
				}
			}
			end_IL_005b:;
		}
		return null;
	}

	public IEnumerator<QueueRequirement> GetEnumerator()
	{
		return InternalEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return InternalEnumerator();
	}

	public static RequirementCollection Create()
	{
		return new RequirementCollection();
	}

	public static RequirementCollection Create(IEnumerable<QueueRequirement> requirements)
	{
		List<byte[]> list = null;
		if (!requirements.IsNullOrEmpty())
		{
			list = new List<byte[]>();
			using (IEnumerator<QueueRequirement> enumerator = requirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QueueRequirement current = enumerator.Current;
					MemoryStream memoryStream = new MemoryStream();
					InternalSerializer.Serialize(memoryStream, current);
					list.Add(memoryStream.ToArray());
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_001b;
					}
				}
				end_IL_001b:;
			}
		}
		RequirementCollection requirementCollection = new RequirementCollection();
		requirementCollection.RequirementsAsBinaryData = list;
		return requirementCollection;
	}

	public List<QueueRequirement> ToList()
	{
		if (m_dirty)
		{
			m_dirty = false;
			m_queueRequirementAsList.Clear();
			using (IEnumerator<QueueRequirement> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QueueRequirement current = enumerator.Current;
					m_queueRequirementAsList.Add(current);
				}
			}
		}
		return m_queueRequirementAsList;
	}
}
