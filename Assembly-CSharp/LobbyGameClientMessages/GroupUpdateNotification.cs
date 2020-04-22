using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GroupUpdateNotification : WebSocketMessage
	{
		public List<UpdateGroupMemberData> Members;

		public GameType GameType;

		public ushort SubTypeMask;

		public BotDifficulty AllyDifficulty;

		public BotDifficulty EnemyDifficulty;

		public long GroupId;
	}
}
