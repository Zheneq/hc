using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ActivateQuestTriggerRequest : WebSocketMessage
	{
		public QuestTriggerType TriggerType;

		public int ActivationCount;

		public int QuestId;

		public int QuestBonusCount;

		public int ItemTemplateId;

		public CharacterType charType;
	}
}
