using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UseOverconSyncNotification : SyncNotification
	{
		public int OverconId;

		public long SenderAccountId;

		public int ActorId;

		public List<string> FellowRecipientHandles;
	}
}
