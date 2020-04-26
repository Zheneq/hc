using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FactionLoginRewardNotification : WebSocketMessage
	{
		public FactionRewards LogInRewardsGiven;
	}
}
