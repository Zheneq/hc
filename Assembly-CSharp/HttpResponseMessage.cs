public class HttpResponseMessage : WebSocketMessage
{
	public int StatusCode = 200;

	public string ResponseData = string.Empty;

	public string ContentType = "text/plain";
}
