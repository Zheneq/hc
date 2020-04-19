using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CheckRAFStatusResponse : WebSocketResponseMessage
	{
		public string ReferralCode;
	}
}
