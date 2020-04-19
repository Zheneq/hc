using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SendRAFReferralEmailsRequest : WebSocketMessage
	{
		public List<string> Emails;
	}
}
