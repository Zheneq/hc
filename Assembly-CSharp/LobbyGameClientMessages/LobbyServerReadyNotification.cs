using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbyServerReadyNotification : WebSocketResponseMessage
	{
		public PersistedAccountData AccountData;

		public LobbyStatusNotification Status;

		public List<PersistedCharacterData> CharacterDataList;

		public LobbyPlayerGroupInfo GroupInfo;

		public FriendStatusNotification FriendStatus;

		public FactionCompetitionNotification FactionCompetitionStatus;

		public ServerQueueConfigurationUpdateNotification ServerQueueConfiguration;

		public string CommerceURL;

		public EnvironmentType EnvironmentType;

		public LobbyAlertMissionDataNotification AlertMissionData;

		public LobbySeasonQuestDataNotification SeasonChapterQuests;
	}
}
