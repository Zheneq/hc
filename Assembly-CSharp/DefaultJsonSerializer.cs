using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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
			
			// custom
			s_serializer.Converters.Add(new VectorJsonConverter());
			s_serializer.Converters.Add(new Vector2JsonConverter());
			s_serializer.Converters.Add(new QuatJsonConverter());
			s_serializer.Converters.Add(new ColorJsonConverter());
			s_serializer.Converters.Add(new ActorJsonConverter());
			s_serializer.Converters.Add(new BoundsJsonConverter());
			s_serializer.Converters.Add(new AbilityJsonConverter());
			s_serializer.Converters.Add(new GameObjectJsonConverter());
			s_serializer.Converters.Add(new MovementPathStartJsonConverter());
			s_serializer.Converters.Add(new SpriteJsonConverter());
			s_serializer.Converters.Add(new BoardSquareJsonConverter());
			s_serializer.Converters.Add(new BoardSquarePathInfoJsonConverter());
			s_serializer.Converters.Add(new ClientEffectDataJsonConverter());
			s_serializer.Converters.Add(new NetworkConnectionJsonConverter());
			// end custom
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

	// custom
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

	// custom
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

	// custom
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

	// custom
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

	// custom
	public class ColorJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Color q = (Color)value;
			new JArray(new float[] { q.r, q.g, q.b, q.a }).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Color) == objectType;
		}
	}

	// custom
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

	// custom
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

	// custom
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

	/*	
	// custom
	public class AbilityRequestDataJsonConverter : JsonWriterConverter
		{
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				ActorTargeting.AbilityRequestData ability = (ActorTargeting.AbilityRequestData)value;


				List<BoardSquarePathInfo> path = new List<BoardSquarePathInfo>();

				for (BoardSquarePathInfo node = (BoardSquarePathInfo)value; node != null; node = node.next)
				{
					path.Add(node);
				}

				JsonSerializer serializer2 = new JsonSerializer();
				serializer2.NullValueHandling = NullValueHandling.Ignore;
				serializer2.Converters.Add(new BoardSquareJsonConverter());
				serializer2.Serialize(writer, path);

				JToken.FromObject(new Dictionary<String, float[]>
				{
					{"actionType", ability.m_actionType.ToString() },
					{"size", new float[] { bounds.size.x, bounds.size.y, bounds.size.z } }
				}).WriteTo(writer);
			}

			public override bool CanConvert(Type objectType)
			{
				return objectType.IsSubclassOf(typeof(ActorTargeting.AbilityRequestData));
			}
		}*/

	// custom
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

	// custom
	public class MovementPathStartJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken.FromObject("MovementPathStart").WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(MovementPathStart) == objectType;
		}
	}

	// custom
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

	// custom
	public class BoardSquarePathInfoJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			List<BoardSquarePathInfo> path = new List<BoardSquarePathInfo>();

			for (BoardSquarePathInfo node = (BoardSquarePathInfo)value; node != null; node = node.next)
			{
				path.Add(node);
			}

			JsonSerializer serializer2 = new JsonSerializer();
			serializer2.NullValueHandling = NullValueHandling.Ignore;
			serializer2.Converters.Add(new BoardSquareJsonConverter());
			serializer2.Serialize(writer, path);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(BoardSquarePathInfo) == objectType;
		}
	}

	// custom
	public class BoardSquareJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BoardSquare square = (BoardSquare)value;
			JToken.FromObject(new int[] { square.x, square.y }).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(BoardSquare) == objectType;
		}
	}

	// custom
	public class ClientEffectDataJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ClientEffectData v = (ClientEffectData)value;

			JObject res = new JObject
			{
				{ "sequences", JToken.FromObject(v.m_sequences.Select((Sequence x) => x.Id)) },
				{ "target", v.m_target ? JToken.FromObject(v.m_target.DebugNameString()) : null },
				{ "statuses", JToken.FromObject(v.m_statuses.Select((StatusType x) => x.ToString())) }
			};
			res.WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(ClientEffectData) == objectType;
		}
	}

	// custom
	public class NetworkConnectionJsonConverter : JsonWriterConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			NetworkConnection v = (NetworkConnection)value;

			JObject res = new JObject
			{
				{ "hostId", JToken.FromObject(v.hostId) },
				{ "connectionId", JToken.FromObject(v.connectionId) },
				{ "isReady", JToken.FromObject(v.isReady) },
				{ "address", JToken.FromObject(v.address) },
				{ "lastMessageTime", JToken.FromObject(v.lastMessageTime) },
				{ "closeStatusCode", JToken.FromObject(v.closeStatusCode) },
				{ "isConnected", JToken.FromObject(v.isConnected) },
				{ "lastError", JToken.FromObject(v.lastError) }
			};
			res.WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(NetworkConnection) == objectType;
		}
	}
}
