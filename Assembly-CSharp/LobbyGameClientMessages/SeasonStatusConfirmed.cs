using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonStatusConfirmed : WebSocketResponseMessage
	{
		public enum DialogType
		{
			_001D,
			_000E
		}

		public DialogType Dialog;
	}
}
