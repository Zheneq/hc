using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class DefaultJsonSerializer
{
	private static JsonSerializer s_serializer;

	public static JsonSerializer Get()
	{
		if (s_serializer == null)
		{
			s_serializer = new JsonSerializer();
			s_serializer.NullValueHandling = NullValueHandling.Ignore;
			s_serializer.Converters.Add(new StringEnumConverter());
			s_serializer.Converters.Add(new VectorJsonConverter());
			s_serializer.Converters.Add(new Vector2JsonConverter());
			s_serializer.Converters.Add(new QuatJsonConverter());
			s_serializer.Converters.Add(new ActorJsonConverter());
			s_serializer.Converters.Add(new BoundsJsonConverter());
			s_serializer.Converters.Add(new AbilityJsonConverter());
			s_serializer.Converters.Add(new GameObjectJsonConverter());
			s_serializer.Converters.Add(new SpriteJsonConverter()); 
		}
		return s_serializer;
	}

	public static string Serialize(object o)
	{
		StringWriter stringWriter = new StringWriter();
		Get().Serialize(stringWriter, o);
		return stringWriter.ToString();
	}

	public static T Deserialize<T>(string json)
	{
		JsonTextReader reader = new JsonTextReader(new StringReader(json));
		return Get().Deserialize<T>(reader);
	}

	public abstract class JsonWriterConverter : JsonConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
		}

		public override bool CanRead
		{
			get { return false; }
		}
	}

	public class VectorJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Vector3 v = (Vector3)value;
			new JArray(new float[] { v.x, v.y, v.z }).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Vector3) == objectType;
		}
	}

	public class Vector2JsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Vector2 v = (Vector2)value;
			new JArray(new float[] { v.x, v.y }).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Vector2) == objectType;
		}
	}

	public class QuatJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Quaternion q = (Quaternion)value;
			new JArray(new float[] { q.x, q.y, q.z, q.w }).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Quaternion) == objectType;
		}
	}

	public class ActorJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ActorData actor = (ActorData)value;
			JToken.FromObject($"{actor.DisplayName} [{actor.ActorIndex}]").WriteTo(writer);
		}


		public override bool CanConvert(Type objectType)
		{
			return typeof(ActorData) == objectType;
		}
	}

	public class BoundsJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Bounds bounds = (Bounds)value;
			JToken.FromObject(new Dictionary<String, float[]>
			{
				{"center", new float[] { bounds.center.x, bounds.center.y, bounds.center.z } },
				{"size", new float[] { bounds.size.x, bounds.size.y, bounds.size.z } }
			}).WriteTo(writer);
		}


		public override bool CanConvert(Type objectType)
		{
			return typeof(Bounds) == objectType;
		}
	}

	public class AbilityJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Ability ability = (Ability)value;
			JToken.FromObject(ability.m_abilityName).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(Ability));
		}
	}

	public class GameObjectJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken.FromObject("GameObject").WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(GameObject) == objectType;
		}
	}

	public class SpriteJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken.FromObject("Sprite").WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Sprite) == objectType;
		}
	}




}
