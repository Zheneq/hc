using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CreateGameRequest : WebSocketMessage
	{
		public LobbyGameConfig GameConfig;

		public ReadyState ReadyState;

		public string ProcessCode;

		public BotDifficulty SelectedBotSkillTeamA;

		public BotDifficulty SelectedBotSkillTeamB;
	}
}
