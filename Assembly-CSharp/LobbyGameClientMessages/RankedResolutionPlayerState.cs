using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedResolutionPlayerState
	{
		public int PlayerId;

		public CharacterType Intention;

		public RankedResolutionPlayerState.ReadyState OnDeckness;

		public enum ReadyState
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}
