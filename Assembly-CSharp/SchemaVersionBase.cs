using System;
using System.Globalization;
using Newtonsoft.Json;

[JsonConverter(typeof(SchemaVersionBase.JsonConverter))]
[Serializable]
public class SchemaVersionBase
{
	public ulong IntValue;

	public SchemaVersionBase(string stringValue)
	{
		this.StringValue = stringValue;
	}

	public SchemaVersionBase(ulong intValue = 0UL)
	{
		this.IntValue = intValue;
	}

	public string StringValue
	{
		get
		{
			return string.Format("0x{0:x}", this.IntValue);
		}
		set
		{
			if (value.StartsWith("0x"))
			{
				value = value.Substring(2);
			}
			this.IntValue = ulong.Parse(value, NumberStyles.HexNumber);
		}
	}

	public void Clear()
	{
		this.IntValue = 0UL;
	}

	public override string ToString()
	{
		return this.StringValue;
	}

	private class JsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(SchemaVersionBase);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SchemaVersionBase schemaVersionBase = (SchemaVersionBase)value;
			writer.WriteValue(schemaVersionBase.StringValue);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			SchemaVersionBase schemaVersionBase = (SchemaVersionBase)Activator.CreateInstance(objectType);
			schemaVersionBase.StringValue = reader.Value.ToString();
			return schemaVersionBase;
		}
	}
}
