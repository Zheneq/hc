using System;
using Newtonsoft.Json;

[JsonConverter(typeof(Rate.JsonConverter))]
[Serializable]
public struct Rate
{
	public double Amount;

	public TimeSpan Period;

	public Rate(double amount, TimeSpan period)
	{
		this.Amount = amount;
		this.Period = period;
	}

	public static implicit operator Rate(string rate)
	{
		string[] array = rate.Split(new string[]
		{
			" per "
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 2)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(string).MethodHandle;
			}
			throw new Exception("Failed to parse rate");
		}
		return new Rate(double.Parse(array[0]), TimeSpan.Parse(array[1]));
	}

	public override string ToString()
	{
		return string.Format("{0} per {1}", this.Amount, this.Period);
	}

	public double AmountPerSecond
	{
		get
		{
			if (this.Period == TimeSpan.Zero)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Rate.get_AmountPerSecond()).MethodHandle;
				}
				return 0.0;
			}
			return this.Amount / this.Period.TotalSeconds;
		}
	}

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
			return reader.Value.ToString();
		}
	}
}
