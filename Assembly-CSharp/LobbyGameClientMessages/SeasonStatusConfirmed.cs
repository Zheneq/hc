using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SeasonStatusConfirmed : WebSocketResponseMessage
	{
		public SeasonStatusConfirmed.DialogType Dialog;

		public enum DialogType
		{
			symbol_001D,
			symbol_000E
		}
	}
}
