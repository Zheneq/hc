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
			\u001D,
			\u000E,
			\u0012
		}
	}
}
