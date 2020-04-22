using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ChatSyncNotification : SyncNotification
	{
		public long SenderAccountId;

		public long SenderSessionToken;

		public List<string> FellowRecipientHandles;

		public List<string> BlockedRecipientHandles;

		public ChatNotification Notification;
	}
}
