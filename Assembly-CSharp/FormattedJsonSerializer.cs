using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public static class FormattedJsonSerializer
{
	private static JsonSerializer s_serializer;

	public static JsonSerializer Get()
	{
		if (FormattedJsonSerializer.s_serializer == null)
		{
			FormattedJsonSerializer.s_serializer = new JsonSerializer();
			FormattedJsonSerializer.s_serializer.NullValueHandling = NullValueHandling.Ignore;
			FormattedJsonSerializer.s_serializer.Formatting = Formatting.Indented;
			FormattedJsonSerializer.s_serializer.Converters.Add(new StringEnumConverter());
		}
		return FormattedJsonSerializer.s_serializer;
	}

	public static string Serialize(object o)
	{
		StringWriter stringWriter = new StringWriter();
		FormattedJsonSerializer.Get().Serialize(stringWriter, o);
		return stringWriter.ToString();
	}

	public static T Deserialize<T>(string json)
	{
		JsonTextReader reader = new JsonTextReader(new StringReader(json));
		return FormattedJsonSerializer.Get().Deserialize<T>(reader);
	}
}
