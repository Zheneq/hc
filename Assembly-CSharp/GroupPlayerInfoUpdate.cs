using System;

[Serializable]
public class GroupPlayerInfoUpdate
{
	public ReadyState? ReadyState;

	public CharacterType? CharacterType;

	public CharacterVisualInfo? CharacterSkin;

	public CharacterCardInfo? CharacterCards;

	public CharacterModInfo? CharacterMods;

	public CharacterAbilityVfxSwapInfo? CharacterAbilityVfxSwaps;

	public BotDifficulty? Difficulty;
}
