using Newtonsoft.Json;
using System;
using System.Globalization;

[Serializable]
[JsonConverter(typeof(JsonConverter))]
public class SchemaVersionBase
{
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

	public ulong IntValue;

	public string StringValue
	{
		get
		{
			return $"0x{IntValue:x}";
		}
		set
		{
			if (value.StartsWith("0x"))
			{
				value = value.Substring(2);
			}
			IntValue = ulong.Parse(value, NumberStyles.HexNumber);
		}
	}

	public SchemaVersionBase(string stringValue)
	{
		StringValue = stringValue;
	}

	public SchemaVersionBase(ulong intValue = 0uL)
	{
		IntValue = intValue;
	}

	public void Clear()
	{
		IntValue = 0uL;
	}

	public override string ToString()
	{
		return StringValue;
	}
}
