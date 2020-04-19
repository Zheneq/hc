using System;

[Serializable]
public class RankedDataChange : WebSocketMessage
{
	public string Removal;

	public RankedData Update;
}
