using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
[JsonConverter(typeof(JsonConverter))]
public class EloValues : ICloneable
{
	private class JsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(EloValues);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			EloValues eloValues = (EloValues)value;
			serializer.Serialize(writer, eloValues.Values);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			EloValues eloValues = (existingValue == null) ? new EloValues() : ((EloValues)existingValue);
			serializer.Populate(reader, eloValues.Values);
			return eloValues;
		}
	}

	public Dictionary<string, EloDatum> Values
	{
		get;
		private set;
	}

	[JsonIgnore]
	public float PlayerFacingElo
	{
		get
		{
			GetElo(ELOPlayerKey.PublicFacingKey.KeyText, out float elo, out int _);
			return elo;
		}
		set
		{
			UpdateElo(ELOPlayerKey.PublicFacingKey.KeyText, value, 0);
		}
	}

	[JsonIgnore]
	public float InternalElo
	{
		get
		{
			GetElo(ELOPlayerKey.MatchmakingEloKey.KeyText, out float elo, out int _);
			return elo;
		}
		set
		{
			UpdateElo(ELOPlayerKey.MatchmakingEloKey.KeyText, value, 0);
		}
	}

	public EloValues()
	{
		Values = new Dictionary<string, EloDatum>();
	}

	public void UpdateElo(string key, float value, int countDelta)
	{
		if (Values.TryGetValue(key, out EloDatum value2))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					value2.Elo = Math.Max(1f, value);
					value2.Count = Math.Max(0, value2.Count + countDelta);
					return;
				}
			}
		}
		countDelta = Math.Max(0, countDelta);
		Values.Add(key, new EloDatum
		{
			Elo = value,
			Count = countDelta
		});
	}

	public void ApplyDelta(string key, float eloDelta, int countDelta)
	{
		if (Values.TryGetValue(key, out EloDatum value))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					value.Elo = Math.Max(1f, value.Elo + eloDelta);
					value.Count = Math.Max(0, value.Count + countDelta);
					return;
				}
			}
		}
		countDelta = Math.Max(0, countDelta);
		Values.Add(key, new EloDatum
		{
			Elo = 1500f + eloDelta,
			Count = countDelta
		});
	}

	public void GetElo(string key, out float elo, out int count)
	{
		if (Values.TryGetValue(key, out EloDatum value))
		{
			elo = value.Elo;
			count = value.Count;
		}
		else
		{
			elo = 1500f;
			count = 0;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
