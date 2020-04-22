using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FreelancerUnavailableNotification : WebSocketMessage
	{
		public CharacterType oldCharacterType;

		public CharacterType newCharacterType;

		public string thiefName;

		public bool ItsTooLateToChange;
	}
}
