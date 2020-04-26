using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedResolutionPlayerState
	{
		public enum ReadyState
		{
			_001D,
			_000E,
			_0012
		}

		public int PlayerId;

		public CharacterType Intention;

		public ReadyState OnDeckness;
	}
}
