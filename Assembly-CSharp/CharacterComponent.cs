using System;
using System.Collections.Generic;

[Serializable]
public class CharacterComponent : ICloneable
{
	public CharacterComponent()
	{
		this.Skins = new List<PlayerSkinData>();
		this.Taunts = new List<PlayerTauntData>();
		this.Mods = new List<PlayerModData>();
		this.AbilityVfxSwaps = new List<PlayerAbilityVfxSwapData>();
		this.LastSkin = default(CharacterVisualInfo);
		this.LastCards = default(CharacterCardInfo);
		this.LastMods = default(CharacterModInfo);
		this.LastRankedMods = default(CharacterModInfo);
		this.LastAbilityVfxSwaps = default(CharacterAbilityVfxSwapInfo);
		this.CharacterLoadouts = new List<CharacterLoadout>();
		this.CharacterLoadoutsRanked = new List<CharacterLoadout>();
		this.NumCharacterLoadouts = 0;
		this.LastSelectedLoadout = -1;
		this.LastSelectedRankedLoadout = -1;
		this.ResetSelections();
	}

	public bool Unlocked { get; set; }

	public CharacterVisualInfo LastSkin { get; set; }

	public CharacterCardInfo LastCards { get; set; }

	public CharacterModInfo LastMods { get; set; }

	public CharacterModInfo LastRankedMods { get; set; }

	public CharacterAbilityVfxSwapInfo LastAbilityVfxSwaps { get; set; }

	public List<CharacterLoadout> CharacterLoadouts { get; set; }

	public List<CharacterLoadout> CharacterLoadoutsRanked { get; set; }

	public int LastSelectedLoadout { get; set; }

	public int LastSelectedRankedLoadout { get; set; }

	public int NumCharacterLoadouts { get; set; }

	public List<PlayerSkinData> Skins { get; set; }

	public List<PlayerTauntData> Taunts { get; set; }

	public List<PlayerModData> Mods { get; set; }

	public List<PlayerAbilityVfxSwapData> AbilityVfxSwaps { get; set; }

	public int ResetSelectionVersion { get; set; }

	public PlayerSkinData GetSkin(int i)
	{
		while (this.Skins.Count <= i)
		{
			this.Skins.Add(new PlayerSkinData());
		}
		return this.Skins[i];
	}

	public PlayerTauntData GetTaunt(int i)
	{
		while (this.Taunts.Count <= i)
		{
			this.Taunts.Add(new PlayerTauntData());
		}
		return this.Taunts[i];
	}

	public PlayerAbilityVfxSwapData GetAbilityVfxSwap(int i)
	{
		while (this.AbilityVfxSwaps.Count <= i)
		{
			this.AbilityVfxSwaps.Add(default(PlayerAbilityVfxSwapData));
		}
		return this.AbilityVfxSwaps[i];
	}

	public void ResetSelections()
	{
		this.LastCards = default(CharacterCardInfo);
		this.LastMods = default(CharacterModInfo);
		this.LastRankedMods = default(CharacterModInfo);
		this.LastAbilityVfxSwaps = default(CharacterAbilityVfxSwapInfo);
		this.LastSkin = default(CharacterVisualInfo);
	}

	public bool IsStyleUnlocked(int skinIndex, int patternIndex, int colorIndex)
	{
		return skinIndex < this.Skins.Count && patternIndex < this.Skins[skinIndex].Patterns.Count && colorIndex < this.Skins[skinIndex].Patterns[patternIndex].Colors.Count && this.Skins[skinIndex].Patterns[patternIndex].Colors[colorIndex].Unlocked;
	}

	public bool IsModUnlocked(int abilityId, int abilityModId)
	{
		using (List<PlayerModData>.Enumerator enumerator = this.Mods.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerModData playerModData = enumerator.Current;
				if (playerModData.AbilityId == abilityId)
				{
					if (playerModData.AbilityModID == abilityModId)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsAbilityVfxSwapUnlocked(int abilityId, int abilityVfxSwapId)
	{
		using (List<PlayerAbilityVfxSwapData>.Enumerator enumerator = this.AbilityVfxSwaps.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerAbilityVfxSwapData playerAbilityVfxSwapData = enumerator.Current;
				if (playerAbilityVfxSwapData.AbilityId == abilityId && playerAbilityVfxSwapData.AbilityVfxSwapID == abilityVfxSwapId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
