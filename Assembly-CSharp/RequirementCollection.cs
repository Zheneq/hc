using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NetSerializer;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonConverter))]
[Serializable]
public class RequirementCollection : IEnumerable<QueueRequirement>, IEnumerable
{
	public List<byte[]> RequirementsAsBinaryData;

	[NonSerialized]
	private bool m_dirty = true;
	private List<QueueRequirement> m_queueRequirementAsList = new List<QueueRequirement>();
	private static Serializer s_serializer;

	private IEnumerator<QueueRequirement> InternalEnumerator()
	{
		bool flag = false;
		if (RequirementsAsBinaryData.IsNullOrEmpty())
		{
			yield break;
		}
		
		foreach (byte[] data in RequirementsAsBinaryData)
		{
			MemoryStream stream = new MemoryStream(data);
			object arg;
			InternalSerializer.Deserialize(stream, out arg);
			QueueRequirement req = arg as QueueRequirement;
			if (arg != null && req != null)
			{
				yield return req;
				flag = true;
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
		foreach (QueueRequirement item in collection)
		{
			Add(item);
		}
		m_dirty = true;
	}

	public bool Exists(Predicate<QueueRequirement> match)
	{
		foreach (QueueRequirement obj in this)
		{
			if (match(obj))
			{
				return true;
			}
		}
		return false;
	}

	public RequirementCollection Where(Predicate<QueueRequirement> match)
	{
		RequirementCollection requirementCollection = Create();
		foreach (QueueRequirement queueRequirement in this)
		{
			if (match(queueRequirement))
			{
				requirementCollection.Add(queueRequirement);
			}
		}
		return requirementCollection;
	}

	public bool DoesApplicantPass(
		IQueueRequirementSystemInfo systemInfo,
		IQueueRequirementApplicant applicant,
		GameType gameType,
		GameSubType gameSubType)
	{
		return false == Exists(p => !p.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType));
	}

	public LocalizationPayload GenerateFailure(
		IQueueRequirementSystemInfo systemInfo,
		IQueueRequirementApplicant applicant,
		GameType gameType,
		GameSubType gameSubType,
		RequirementMessageContext context)
	{
		foreach (QueueRequirement queueRequirement in this)
		{
			if (!queueRequirement.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType))
			{
				return queueRequirement.GenerateFailure(systemInfo, applicant, context);
			}
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
			foreach (QueueRequirement ob in requirements)
			{
				MemoryStream memoryStream = new MemoryStream();
				InternalSerializer.Serialize(memoryStream, ob);
				list.Add(memoryStream.ToArray());
			}
		}
		return new RequirementCollection
		{
			RequirementsAsBinaryData = list
		};
	}

	public List<QueueRequirement> ToList()
	{
		if (m_dirty)
		{
			m_dirty = false;
			m_queueRequirementAsList.Clear();
			foreach (QueueRequirement item in this)
			{
				m_queueRequirementAsList.Add(item);
			}
		}
		return m_queueRequirementAsList;
	}

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
				return null;
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception(new StringBuilder().Append("Bad JSON definition of RequirementCollection, expected '[' not ").Append(reader.TokenType).Append("='").Append(reader.Value).Append("'").ToString());
			}
			reader.Read();
			RequirementCollection requirementCollection = Create();
			while (reader.TokenType != JsonToken.EndArray)
			{
				QueueRequirement item = QueueRequirement.ExtractRequirementFromReader(reader);
				requirementCollection.Add(item);
				reader.Read();
			}
			if (reader.TokenType != JsonToken.EndArray)
			{
				throw new Exception(new StringBuilder().Append("Bad JSON definition of RequirementCollection, expected ']' not ").Append(reader.TokenType).Append("='").Append(reader.Value).Append("'").ToString());
			}
			return requirementCollection;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			RequirementCollection requirementCollection = value as RequirementCollection;
			writer.WriteStartArray();
			foreach (QueueRequirement queueRequirement in requirementCollection)
			{
				writer.WriteStartObject();
				writer.WritePropertyName(queueRequirement.Requirement.ToString());
				writer.WriteStartObject();
				queueRequirement.WriteToJson(writer);
				writer.WriteEndObject();
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}
