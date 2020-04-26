using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_TakeSnapshotResponse : WebSocketResponseMessage
	{
		public string SnapshotId;
	}
}
