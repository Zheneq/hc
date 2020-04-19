using System;

[Serializable]
public class LobbyPlayerInfoUpdate
{
	public int PlayerId;

	public ContextualReadyState? ContextualReadyState;

	public CharacterType? CharacterType;

	public CharacterVisualInfo? CharacterSkin;

	public CharacterCardInfo? CharacterCards;

	public CharacterModInfo? CharacterMods;

	public CharacterAbilityVfxSwapInfo? CharacterAbilityVfxSwaps;

	public CharacterLoadoutUpdate? CharacterLoadoutChanges;

	public int? LastSelectedLoadout;

	public BotDifficulty? AllyDifficulty;

	public BotDifficulty? EnemyDifficulty;

	public bool RankedLoadoutMods;
}
