using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

public static class FormattedJsonSerializer
{
	private static JsonSerializer s_serializer;

	public static JsonSerializer Get()
	{
		if (s_serializer == null)
		{
			s_serializer = new JsonSerializer();
			s_serializer.NullValueHandling = NullValueHandling.Ignore;
			s_serializer.Formatting = Formatting.Indented;
			s_serializer.Converters.Add(new StringEnumConverter());
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
}
