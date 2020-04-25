using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NetSerializer;
using Newtonsoft.Json;

[JsonConverter(typeof(RequirementCollection.JsonConverter))]
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
		if (this.RequirementsAsBinaryData.IsNullOrEmpty<byte[]>())
		{
			yield break;
		}
		List<byte[]>.Enumerator enumerator = this.RequirementsAsBinaryData.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				byte[] data = enumerator.Current;
				MemoryStream stream = new MemoryStream(data);
				object arg;
				RequirementCollection.InternalSerializer.Deserialize(stream, out arg);
				if (arg != null)
				{
					if (arg is QueueRequirement)
					{
						yield return arg as QueueRequirement;
						flag = true;
					}
				}
			}
		}
		finally
		{
			if (flag)
			{
			}
			else
			{
				((IDisposable)enumerator).Dispose();
			}
		}
		yield break;
	}

	public void Add(QueueRequirement item)
	{
		if (item != null)
		{
			MemoryStream memoryStream = new MemoryStream();
			RequirementCollection.InternalSerializer.Serialize(memoryStream, item);
			if (this.RequirementsAsBinaryData == null)
			{
				this.RequirementsAsBinaryData = new List<byte[]>();
			}
			this.RequirementsAsBinaryData.Add(memoryStream.ToArray());
		}
		this.m_dirty = true;
	}

	public void AddRange(IEnumerable<QueueRequirement> collection)
	{
		using (IEnumerator<QueueRequirement> enumerator = collection.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement item = enumerator.Current;
				this.Add(item);
			}
		}
		this.m_dirty = true;
	}

	public bool Exists(Predicate<QueueRequirement> match)
	{
		using (IEnumerator<QueueRequirement> enumerator = this.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement obj = enumerator.Current;
				if (match(obj))
				{
					return true;
				}
			}
		}
		return false;
	}

	public RequirementCollection Where(Predicate<QueueRequirement> match)
	{
		RequirementCollection requirementCollection = RequirementCollection.Create();
		IEnumerator<QueueRequirement> enumerator = this.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement queueRequirement = enumerator.Current;
				if (match(queueRequirement))
				{
					requirementCollection.Add(queueRequirement);
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		return requirementCollection;
	}

	public bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return false == this.Exists((QueueRequirement p) => !p.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType));
	}

	public LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType, RequirementMessageContext context)
	{
		IEnumerator<QueueRequirement> enumerator = this.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				QueueRequirement queueRequirement = enumerator.Current;
				if (!queueRequirement.DoesApplicantPass(systemInfo, applicant, gameType, gameSubType))
				{
					return queueRequirement.GenerateFailure(systemInfo, applicant, context);
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		return null;
	}

	public IEnumerator<QueueRequirement> GetEnumerator()
	{
		return this.InternalEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.InternalEnumerator();
	}

	public static RequirementCollection Create()
	{
		return new RequirementCollection();
	}

	public static RequirementCollection Create(IEnumerable<QueueRequirement> requirements)
	{
		List<byte[]> list = null;
		if (!requirements.IsNullOrEmpty<QueueRequirement>())
		{
			list = new List<byte[]>();
			using (IEnumerator<QueueRequirement> enumerator = requirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QueueRequirement ob = enumerator.Current;
					MemoryStream memoryStream = new MemoryStream();
					RequirementCollection.InternalSerializer.Serialize(memoryStream, ob);
					list.Add(memoryStream.ToArray());
				}
			}
		}
		return new RequirementCollection
		{
			RequirementsAsBinaryData = list
		};
	}

	public List<QueueRequirement> ToList()
	{
		if (this.m_dirty)
		{
			this.m_dirty = false;
			this.m_queueRequirementAsList.Clear();
			foreach (QueueRequirement item in this)
			{
				this.m_queueRequirementAsList.Add(item);
			}
		}
		return this.m_queueRequirementAsList;
	}

	private static Serializer InternalSerializer
	{
		get
		{
			if (RequirementCollection.s_serializer == null)
			{
				RequirementCollection.s_serializer = new Serializer();
				RequirementCollection.s_serializer.AddTypes(QueueRequirement.MessageTypes);
			}
			return RequirementCollection.s_serializer;
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
				throw new Exception(string.Format("Bad JSON definition of RequirementCollection, expected '[' not {0}='{1}'", reader.TokenType, reader.Value));
			}
			reader.Read();
			RequirementCollection requirementCollection = RequirementCollection.Create();
			while (reader.TokenType != JsonToken.EndArray)
			{
				QueueRequirement item = QueueRequirement.ExtractRequirementFromReader(reader);
				requirementCollection.Add(item);
				reader.Read();
			}
			if (reader.TokenType != JsonToken.EndArray)
			{
				throw new Exception(string.Format("Bad JSON definition of RequirementCollection, expected ']' not {0}='{1}'", reader.TokenType, reader.Value));
			}
			return requirementCollection;
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
					QueueRequirement queueRequirement = enumerator.Current;
					writer.WriteStartObject();
					writer.WritePropertyName(queueRequirement.Requirement.ToString());
					writer.WriteStartObject();
					queueRequirement.WriteToJson(writer);
					writer.WriteEndObject();
					writer.WriteEndObject();
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			writer.WriteEndArray();
		}
	}
}
