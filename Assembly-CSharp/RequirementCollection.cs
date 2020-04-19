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
		uint num;
		List<byte[]>.Enumerator enumerator;
		switch (num)
		{
		case 0U:
			if (this.RequirementsAsBinaryData.IsNullOrEmpty<byte[]>())
			{
				goto IL_13A;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.<InternalEnumerator>c__Iterator0.MoveNext()).MethodHandle;
			}
			enumerator = this.RequirementsAsBinaryData.GetEnumerator();
			break;
		case 1U:
			break;
		default:
			yield break;
		}
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (arg is QueueRequirement)
					{
						yield return arg as QueueRequirement;
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = true;
					}
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (flag)
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
			}
			else
			{
				((IDisposable)enumerator).Dispose();
			}
		}
		IL_13A:
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.Add(QueueRequirement)).MethodHandle;
				}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.AddRange(IEnumerable<QueueRequirement>)).MethodHandle;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.Exists(Predicate<QueueRequirement>)).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.Where(Predicate<QueueRequirement>)).MethodHandle;
				}
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.GenerateFailure(IQueueRequirementSystemInfo, IQueueRequirementApplicant, GameType, GameSubType, RequirementMessageContext)).MethodHandle;
					}
					return queueRequirement.GenerateFailure(systemInfo, applicant, context);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (enumerator != null)
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.Create(IEnumerable<QueueRequirement>)).MethodHandle;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.ToList()).MethodHandle;
			}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.get_InternalSerializer()).MethodHandle;
				}
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)).MethodHandle;
				}
				return null;
			}
			if (reader.TokenType != JsonToken.StartArray)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (reader.TokenType != JsonToken.EndArray)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RequirementCollection.JsonConverter.WriteJson(JsonWriter, object, JsonSerializer)).MethodHandle;
				}
			}
			finally
			{
				if (enumerator != null)
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
					enumerator.Dispose();
				}
			}
			writer.WriteEndArray();
		}
	}
}
