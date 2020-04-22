using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct BalanceTeamSlot
	{
		public Team Team;

		public int PlayerId;

		public long AccountId;

		public CharacterType SelectedCharacter;

		public BotDifficulty BotDifficulty;
	}
}
