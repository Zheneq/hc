using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class HttpRequestMessage : WebSocketMessage
{
	public string Method;

	public Uri Url;

	public Dictionary<string, string> QueryParams;

	public string RequestData;

	public JsonSerializer Serializer;
}
