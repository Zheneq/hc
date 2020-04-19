using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonStatusConfirmed : WebSocketResponseMessage
	{
		public SeasonStatusConfirmed.DialogType Dialog;

		public enum DialogType
		{
			\u001D,
			\u000E
		}
	}
}
