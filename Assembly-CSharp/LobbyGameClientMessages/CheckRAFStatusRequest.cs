using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CheckRAFStatusRequest : WebSocketMessage
	{
		public bool GetReferralCode;
	}
}
