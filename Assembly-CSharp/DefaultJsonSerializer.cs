using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public static class DefaultJsonSerializer
{
	private static JsonSerializer s_serializer;

	public static JsonSerializer Get()
	{
		if (DefaultJsonSerializer.s_serializer == null)
		{
			DefaultJsonSerializer.s_serializer = new JsonSerializer();
			DefaultJsonSerializer.s_serializer.NullValueHandling = NullValueHandling.Ignore;
			DefaultJsonSerializer.s_serializer.Converters.Add(new StringEnumConverter());
		}
		return DefaultJsonSerializer.s_serializer;
	}

	public static string Serialize(object o)
	{
		StringWriter stringWriter = new StringWriter();
		DefaultJsonSerializer.Get().Serialize(stringWriter, o);
		return stringWriter.ToString();
	}

	public static T Deserialize<T>(string json)
	{
		JsonTextReader reader = new JsonTextReader(new StringReader(json));
		return DefaultJsonSerializer.Get().Deserialize<T>(reader);
	}
}
