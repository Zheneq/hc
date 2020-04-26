using System;
using System.Collections.Generic;

[Serializable]
public class LobbyCharacterInfo
{
	public CharacterType CharacterType;

	public CharacterVisualInfo CharacterSkin = default(CharacterVisualInfo);

	public CharacterCardInfo CharacterCards = default(CharacterCardInfo);

	public CharacterModInfo CharacterMods = default(CharacterModInfo);

	public CharacterAbilityVfxSwapInfo CharacterAbilityVfxSwaps = default(CharacterAbilityVfxSwapInfo);

	public List<PlayerTauntData> CharacterTaunts = new List<PlayerTauntData>();

	public List<CharacterLoadout> CharacterLoadouts = new List<CharacterLoadout>();

	public int CharacterMatches;

	public int CharacterLevel;

	public LobbyCharacterInfo Clone()
	{
		return (LobbyCharacterInfo)MemberwiseClone();
	}
}
