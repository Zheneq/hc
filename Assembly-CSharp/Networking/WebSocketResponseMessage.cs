using System;

[Serializable]
public abstract class WebSocketResponseMessage : WebSocketMessage
{
	public bool Success;

	public string ErrorMessage;
}
