using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FriendStatusSyncNotification : SyncNotification
	{
		public long AccountId;

		public FriendInfo FriendInfo;
	}
}
