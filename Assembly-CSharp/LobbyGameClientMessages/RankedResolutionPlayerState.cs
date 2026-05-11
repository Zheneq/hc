using System;

namespace LobbyGameClientMessages
{
    [Serializable]
    public struct RankedResolutionPlayerState
    {
        public enum ReadyState
        {
            None,
            Unselected,
            Selected
        }

        public int PlayerId;
        public CharacterType Intention;
        public ReadyState OnDeckness;
    }
}