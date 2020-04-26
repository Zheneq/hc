using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class EnterFreelancerResolutionPhaseNotification : WebSocketMessage
	{
		public FreelancerResolutionPhaseSubType SubPhase;

		public RankedResolutionPhaseData? RankedData;
	}
}
