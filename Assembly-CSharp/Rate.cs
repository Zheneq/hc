using Newtonsoft.Json;
using System;

[Serializable]
[JsonConverter(typeof(JsonConverter))]
public struct Rate
{
	private class JsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Rate);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((Rate)value).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return (Rate)reader.Value.ToString();
		}
	}

	public double Amount;

	public TimeSpan Period;

	public double AmountPerSecond
	{
		get
		{
			if (Period == TimeSpan.Zero)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return 0.0;
					}
				}
			}
			return Amount / Period.TotalSeconds;
		}
	}

	public Rate(double amount, TimeSpan period)
	{
		Amount = amount;
		Period = period;
	}

	public static implicit operator Rate(string rate)
	{
		string[] array = rate.Split(new string[1]
		{
			" per "
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					throw new Exception("Failed to parse rate");
				}
			}
		}
		return new Rate(double.Parse(array[0]), TimeSpan.Parse(array[1]));
	}

	public override string ToString()
	{
		return $"{Amount} per {Period}";
	}
}
