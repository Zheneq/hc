using System;

[Serializable]
public abstract class WebSocketMessage
{
	public const int INVALID_ID = 0;

	public int RequestId;

	public int ResponseId;

	[NonSerialized]
	public long DeserializationTicks;

	[NonSerialized]
	public long SerializedLength;
}
