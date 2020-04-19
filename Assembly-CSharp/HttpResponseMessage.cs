using System;

public class HttpResponseMessage : WebSocketMessage
{
	public int StatusCode = 0xC8;

	public string ResponseData = string.Empty;

	public string ContentType = "text/plain";
}
