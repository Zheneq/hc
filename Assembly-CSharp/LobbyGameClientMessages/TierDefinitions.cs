using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct TierDefinitions
	{
		public LocalizationPayload NameLocalization;

		public string IconResource;

		public bool IsRachet;
	}
}
