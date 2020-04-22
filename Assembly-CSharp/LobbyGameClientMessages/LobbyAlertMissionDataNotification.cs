using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class LobbyAlertMissionDataNotification : WebSocketMessage
	{
		public bool AlertMissionsEnabled;

		public ActiveAlertMission CurrentAlert;

		public DateTime? NextAlert;

		public List<float> ReminderHours;
	}
}
