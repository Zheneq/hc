using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FriendListSyncNotification : SyncNotification
	{
		public long AccountId;

		public FriendInfo FriendInfo;
	}
}
