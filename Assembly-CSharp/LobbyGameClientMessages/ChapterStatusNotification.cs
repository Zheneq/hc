using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ChapterStatusNotification : WebSocketResponseMessage
	{
		public int SeasonIndex;

		public int ChapterIndex;

		public bool IsCompleted;

		public bool IsUnlocked;
	}
}
