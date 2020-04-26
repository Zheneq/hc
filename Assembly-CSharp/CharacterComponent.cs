using System;
using System.Collections.Generic;

[Serializable]
public class CharacterComponent : ICloneable
{
	public bool Unlocked
	{
		get;
		set;
	}

	public CharacterVisualInfo LastSkin
	{
		get;
		set;
	}

	public CharacterCardInfo LastCards
	{
		get;
		set;
	}

	public CharacterModInfo LastMods
	{
		get;
		set;
	}

	public CharacterModInfo LastRankedMods
	{
		get;
		set;
	}

	public CharacterAbilityVfxSwapInfo LastAbilityVfxSwaps
	{
		get;
		set;
	}

	public List<CharacterLoadout> CharacterLoadouts
	{
		get;
		set;
	}

	public List<CharacterLoadout> CharacterLoadoutsRanked
	{
		get;
		set;
	}

	public int LastSelectedLoadout
	{
		get;
		set;
	}

	public int LastSelectedRankedLoadout
	{
		get;
		set;
	}

	public int NumCharacterLoadouts
	{
		get;
		set;
	}

	public List<PlayerSkinData> Skins
	{
		get;
		set;
	}

	public List<PlayerTauntData> Taunts
	{
		get;
		set;
	}

	public List<PlayerModData> Mods
	{
		get;
		set;
	}

	public List<PlayerAbilityVfxSwapData> AbilityVfxSwaps
	{
		get;
		set;
	}

	public int ResetSelectionVersion
	{
		get;
		set;
	}

	public CharacterComponent()
	{
		Skins = new List<PlayerSkinData>();
		Taunts = new List<PlayerTauntData>();
		Mods = new List<PlayerModData>();
		AbilityVfxSwaps = new List<PlayerAbilityVfxSwapData>();
		LastSkin = default(CharacterVisualInfo);
		LastCards = default(CharacterCardInfo);
		LastMods = default(CharacterModInfo);
		LastRankedMods = default(CharacterModInfo);
		LastAbilityVfxSwaps = default(CharacterAbilityVfxSwapInfo);
		CharacterLoadouts = new List<CharacterLoadout>();
		CharacterLoadoutsRanked = new List<CharacterLoadout>();
		NumCharacterLoadouts = 0;
		LastSelectedLoadout = -1;
		LastSelectedRankedLoadout = -1;
		ResetSelections();
	}

	public PlayerSkinData GetSkin(int i)
	{
		while (Skins.Count <= i)
		{
			Skins.Add(new PlayerSkinData());
		}
		while (true)
		{
			return Skins[i];
		}
	}

	public PlayerTauntData GetTaunt(int i)
	{
		while (Taunts.Count <= i)
		{
			Taunts.Add(new PlayerTauntData());
		}
		while (true)
		{
			return Taunts[i];
		}
	}

	public PlayerAbilityVfxSwapData GetAbilityVfxSwap(int i)
	{
		while (AbilityVfxSwaps.Count <= i)
		{
			AbilityVfxSwaps.Add(default(PlayerAbilityVfxSwapData));
		}
		return AbilityVfxSwaps[i];
	}

	public void ResetSelections()
	{
		LastCards = default(CharacterCardInfo);
		LastMods = default(CharacterModInfo);
		LastRankedMods = default(CharacterModInfo);
		LastAbilityVfxSwaps = default(CharacterAbilityVfxSwapInfo);
		LastSkin = default(CharacterVisualInfo);
	}

	public bool IsStyleUnlocked(int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex < Skins.Count && patternIndex < Skins[skinIndex].Patterns.Count && colorIndex < Skins[skinIndex].Patterns[patternIndex].Colors.Count)
		{
			return Skins[skinIndex].Patterns[patternIndex].Colors[colorIndex].Unlocked;
		}
		return false;
	}

	public bool IsModUnlocked(int abilityId, int abilityModId)
	{
		using (List<PlayerModData>.Enumerator enumerator = Mods.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerModData current = enumerator.Current;
				if (current.AbilityId == abilityId)
				{
					if (current.AbilityModID == abilityModId)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public bool IsAbilityVfxSwapUnlocked(int abilityId, int abilityVfxSwapId)
	{
		using (List<PlayerAbilityVfxSwapData>.Enumerator enumerator = AbilityVfxSwaps.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerAbilityVfxSwapData current = enumerator.Current;
				if (current.AbilityId == abilityId && current.AbilityVfxSwapID == abilityVfxSwapId)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
