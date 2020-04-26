using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardUtils
{
	public enum RewardType
	{
		RankedCurrency,
		FreelancerCurrency,
		ModToken,
		GGBoost,
		ISO,
		Banner,
		Emblem,
		Title,
		Skin,
		Style,
		Taunt,
		System,
		ChatEmoji,
		Lockbox,
		Material,
		Mod,
		AbilityVfxSwap,
		Overcon,
		Faction,
		UnlockFreelancerToken,
		LoadingScreenBackground,
		FreelancerExpBonus,
		LAST
	}

	public class RewardData
	{
		public int Level;

		public int Amount;

		public string Name;

		public RewardType Type;

		public string SpritePath;

		public Sprite Foreground;

		public InventoryItemTemplate InventoryTemplate;

		public bool isRepeating;

		public int repeatLevels;
	}

	private class TmpRequirementCheckApplicant : IQueueRequirementApplicant
	{
		private bool IsInTutorial;

		private int InitialSeasonLevel;

		private IQueueRequirementApplicant m_baseApplicant;

		public int CharacterMatches => m_baseApplicant.CharacterMatches;

		public int VsHumanMatches => m_baseApplicant.VsHumanMatches;

		public ClientAccessLevel AccessLevel => m_baseApplicant.AccessLevel;

		public int SeasonLevel
		{
			get;
			set;
		}

		public CharacterType CharacterType => m_baseApplicant.CharacterType;

		public LocalizationArg_Handle LocalizedHandle => m_baseApplicant.LocalizedHandle;

		public float GameLeavingPoints => m_baseApplicant.GameLeavingPoints;

		public IEnumerable<CharacterType> AvailableCharacters => m_baseApplicant.AvailableCharacters;

		public int TotalMatches
		{
			get
			{
				if (IsInTutorial)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return SeasonLevel - 1;
						}
					}
				}
				return m_baseApplicant.TotalMatches;
			}
		}

		public TmpRequirementCheckApplicant(IQueueRequirementApplicant baseApplicant, bool isInTutorial)
		{
			IsInTutorial = isInTutorial;
			m_baseApplicant = baseApplicant;
			SeasonLevel = m_baseApplicant.SeasonLevel;
			InitialSeasonLevel = SeasonLevel;
		}

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			return m_baseApplicant.IsCharacterTypeAvailable(ct);
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			int num = m_baseApplicant.GetReactorLevel(seasons);
			if (!IsInTutorial)
			{
				num += SeasonLevel - InitialSeasonLevel;
			}
			return num;
		}
	}

	public const string GenericFallbackIconPath = "QuestRewards/general";

	private const string m_contractsIconPath = "QuestRewards/contract";

	private const string m_seasonsIconPath = "QuestRewards/contract";

	private const string m_ggIconPath = "QuestRewards/ggPack_01";

	private const string m_rotateIconPath = "QuestRewards/rotateIcon";

	private const string m_isoIconPath = "QuestRewards/iso_01";

	private const string m_modTokenIconPath = "QuestRewards/modToken";

	private const string m_tauntIconPath = "QuestRewards/taunt";

	private const string m_titleIconPath = "QuestRewards/titleIcon";

	private const string m_rankedCurrencyIconPath = "QuestRewards/rankedCurrency";

	private const string m_freelancerCurrencyIconPath = "QuestRewards/freelancerCurrency_01";

	private const string m_freelancerTokenIconPath = "QuestRewards/FreelancerCoin";

	private const string m_dustIconPath = "QuestRewards/general";

	private const string m_chatEmojiIconPath = "QuestRewards/general";

	private const string m_overconIconPath = "QuestRewards/general";

	public static string GetRewardDisplayName(PurchaseType rewardType, int[] rewardData)
	{
		string text = string.Empty;
		switch (rewardType)
		{
		case PurchaseType.BannerBackground:
			text = text + StringUtil.TR("Banner", "Rewards") + " ";
			text += GameBalanceVars.Get().GetBannerName(rewardData[0], string.Empty);
			break;
		case PurchaseType.BannerForeground:
			text = text + StringUtil.TR("Emblem", "Rewards") + " ";
			text += GameBalanceVars.Get().GetBannerName(rewardData[0], string.Empty);
			break;
		case PurchaseType.Character:
			text = text + StringUtil.TR("Freelancer", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterDisplayName((CharacterType)rewardData[0]);
			break;
		case PurchaseType.ChatEmoji:
			text = text + StringUtil.TR("ChatEmoji", "Rewards") + " ";
			text += GameBalanceVars.Get().ChatEmojis[rewardData[0]].GetEmojiName();
			break;
		case PurchaseType.InventoryItem:
			text += InventoryWideData.Get().GetItemTemplate(rewardData[0]).GetDisplayName();
			break;
		case PurchaseType.Overcon:
			text = text + StringUtil.TR("Overcon", "Inventory") + " ";
			text += GameBalanceVars.Get().Overcons[rewardData[0]].GetOverconName();
			break;
		case PurchaseType.Skin:
			text = text + StringUtil.TR("Skin", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetSkinName(rewardData[1]);
			break;
		case PurchaseType.Taunt:
			text = text + StringUtil.TR("Taunt", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetTauntName(rewardData[1]);
			break;
		case PurchaseType.Texture:
		case PurchaseType.ModToken:
			throw new Exception("Invalid unlock type");
		case PurchaseType.Tint:
			text = text + StringUtil.TR("Skin", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetPatternColorName(rewardData[1], rewardData[2], rewardData[3]);
			break;
		case PurchaseType.Title:
			text = text + StringUtil.TR("Title", "Rewards") + " ";
			text += GameBalanceVars.Get().GetTitle(rewardData[0], string.Empty);
			break;
		}
		return text;
	}

	public static List<RewardData> GetNextAccountRewards(int currentLevel)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new List<RewardData>();
				}
			}
		}
		int num = currentLevel + 1;
		if (num > gameBalanceVars.MaxPlayerLevel)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new List<RewardData>();
				}
			}
		}
		List<int> list = new List<int>();
		list.Add(num);
		return GetAccountRewards(list);
	}

	public static List<RewardData> GetAccountRewards(List<int> filterLevels = null)
	{
		List<RewardData> list = new List<RewardData>();
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
		if (filterLevels == null)
		{
			filterLevels = new List<int>();
			for (int i = 2; i <= gameBalanceVars.MaxPlayerLevel; i++)
			{
				filterLevels.Add(i);
			}
		}
		int level;
		for (int j = 0; j < gameBalanceVars.PlayerTitles.Length; j++)
		{
			level = gameBalanceVars.PlayerTitles[j].GetUnlockPlayerLevel();
			if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.PlayerTitles[j].m_isHidden)
			{
				RewardData rewardData = new RewardData();
				rewardData.Level = level;
				rewardData.Amount = 1;
				rewardData.Type = RewardType.Title;
				rewardData.Name = gameBalanceVars.PlayerTitles[j].GetTitleText();
				rewardData.SpritePath = "QuestRewards/titleIcon";
				list.Add(rewardData);
			}
		}
		while (true)
		{
			for (int k = 0; k < gameBalanceVars.ChatEmojis.Length; k++)
			{
				level = gameBalanceVars.ChatEmojis[k].GetUnlockPlayerLevel();
				if (filterLevels.Exists((int x) => x == level))
				{
					if (!gameBalanceVars.ChatEmojis[k].m_isHidden)
					{
						RewardData rewardData2 = new RewardData();
						rewardData2.Level = level;
						rewardData2.Amount = 1;
						rewardData2.Type = RewardType.ChatEmoji;
						rewardData2.Name = gameBalanceVars.ChatEmojis[k].GetEmojiName();
						rewardData2.SpritePath = "QuestRewards/general";
						list.Add(rewardData2);
					}
				}
			}
			while (true)
			{
				for (int l = 0; l < gameBalanceVars.Overcons.Length; l++)
				{
					level = gameBalanceVars.Overcons[l].GetUnlockPlayerLevel();
					if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.Overcons[l].m_isHidden)
					{
						RewardData rewardData3 = new RewardData();
						rewardData3.Level = level;
						rewardData3.Amount = 1;
						rewardData3.Type = RewardType.Overcon;
						rewardData3.Name = gameBalanceVars.Overcons[l].GetOverconName();
						rewardData3.SpritePath = "QuestRewards/general";
						list.Add(rewardData3);
					}
				}
				while (true)
				{
					for (int m = 0; m < gameBalanceVars.LoadingScreenBackgrounds.Length; m++)
					{
						level = gameBalanceVars.LoadingScreenBackgrounds[m].GetUnlockPlayerLevel();
						if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.LoadingScreenBackgrounds[m].m_isHidden)
						{
							RewardData rewardData4 = new RewardData();
							rewardData4.Level = level;
							rewardData4.Amount = 1;
							rewardData4.Type = RewardType.LoadingScreenBackground;
							rewardData4.Name = gameBalanceVars.LoadingScreenBackgrounds[m].GetLoadingScreenBackgroundName();
							rewardData4.SpritePath = gameBalanceVars.LoadingScreenBackgrounds[m].GetSpritePath();
							list.Add(rewardData4);
						}
					}
					for (int n = 0; n < gameBalanceVars.PlayerBanners.Length; n++)
					{
						level = gameBalanceVars.PlayerBanners[n].GetUnlockPlayerLevel();
						if (!filterLevels.Exists((int x) => x == level) || gameBalanceVars.PlayerBanners[n].m_isHidden)
						{
							continue;
						}
						RewardData rewardData5 = new RewardData();
						rewardData5.Level = level;
						rewardData5.Amount = 1;
						if (gameBalanceVars.PlayerBanners[n].m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
						{
							rewardData5.Type = RewardType.Banner;
						}
						else
						{
							rewardData5.Type = RewardType.Emblem;
						}
						rewardData5.Name = gameBalanceVars.PlayerBanners[n].GetBannerName();
						rewardData5.SpritePath = gameBalanceVars.PlayerBanners[n].m_iconResourceString;
						list.Add(rewardData5);
					}
					while (true)
					{
						foreach (int filterLevel in filterLevels)
						{
							CurrencyData[] currencyRewards = gameBalanceVars.PlayerProgressInfo[filterLevel - 2].CurrencyRewards;
							for (int num = 0; num < currencyRewards.Length; num++)
							{
								RewardData rewardData6 = new RewardData();
								rewardData6.Amount = currencyRewards[num].m_Amount;
								rewardData6.Name = string.Empty;
								rewardData6.SpritePath = GetCurrencyIconPath(currencyRewards[num].Type, out rewardData6.Type);
								rewardData6.Level = filterLevel;
								list.Add(rewardData6);
							}
						}
						
						list.Sort(((RewardData first, RewardData second) => first.Level - second.Level));
						return list;
					}
				}
			}
		}
	}

	public static RewardData GetSeasonsUnlockedReward()
	{
		RewardData rewardData = new RewardData();
		rewardData.Amount = 1;
		rewardData.Type = RewardType.System;
		rewardData.Name = StringUtil.TR("SeasonsUnlocked", "Rewards");
		rewardData.SpritePath = "QuestRewards/contract";
		return rewardData;
	}

	public static List<RewardData> GetNextCharacterRewards(CharacterResourceLink charLink, int currentLevel)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return new List<RewardData>();
				}
			}
		}
		int num = currentLevel + 1;
		if (num > gameBalanceVars.MaxCharacterLevelForRewards)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return new List<RewardData>();
				}
			}
		}
		return GetCharacterRewards(charLink, new List<int>
		{
			num
		});
	}

	public static RewardType GetRewardTypeFromInventoryItem(InventoryItemTemplate template)
	{
		RewardType result = RewardType.ISO;
		switch (template.Type)
		{
		case InventoryItemType.AbilityVfxSwap:
			result = RewardType.AbilityVfxSwap;
			break;
		case InventoryItemType.BannerID:
			result = RewardType.Banner;
			break;
		case InventoryItemType.ChatEmoji:
			result = RewardType.ChatEmoji;
			break;
		case InventoryItemType.Overcon:
			result = RewardType.Overcon;
			break;
		case InventoryItemType.Lockbox:
			result = RewardType.Lockbox;
			break;
		case InventoryItemType.Material:
			result = RewardType.Material;
			break;
		case InventoryItemType.Mod:
			result = RewardType.Mod;
			break;
		case InventoryItemType.Skin:
			result = RewardType.Skin;
			break;
		case InventoryItemType.Style:
			result = RewardType.Style;
			break;
		case InventoryItemType.Taunt:
			result = RewardType.Taunt;
			break;
		case InventoryItemType.TitleID:
			result = RewardType.Title;
			break;
		case InventoryItemType.Currency:
		{
			CurrencyType currencyType = (CurrencyType)template.TypeSpecificData[0];
			if (currencyType != CurrencyType.ModToken)
			{
				if (currencyType != CurrencyType.GGPack)
				{
					if (currencyType != 0)
					{
					}
					else
					{
						result = RewardType.ISO;
					}
				}
				else
				{
					result = RewardType.GGBoost;
				}
			}
			else
			{
				result = RewardType.ModToken;
			}
			break;
		}
		case InventoryItemType.Faction:
			result = RewardType.Faction;
			break;
		case InventoryItemType.Unlock:
			result = RewardType.System;
			break;
		case InventoryItemType.Conveyance:
			result = RewardType.System;
			break;
		case InventoryItemType.LoadingScreenBackground:
			result = RewardType.LoadingScreenBackground;
			break;
		case InventoryItemType.FreelancerExpBonus:
			result = RewardType.FreelancerExpBonus;
			break;
		default:
			throw new Exception("Not supported: " + template.Type);
		}
		return result;
	}

	public static List<RewardData> GetCharacterRewards(CharacterResourceLink charLink, List<int> filterLevels = null)
	{
		List<RewardData> list = new List<RewardData>();
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			return list;
		}
		if (filterLevels == null)
		{
			filterLevels = new List<int>();
			for (int i = 2; i <= gameBalanceVars.MaxCharacterLevelForRewards; i++)
			{
				filterLevels.Add(i);
			}
		}
		int level;
		for (int j = 0; j < gameBalanceVars.PlayerTitles.Length; j++)
		{
			level = gameBalanceVars.PlayerTitles[j].GetUnlockCharacterLevel(charLink.m_characterType);
			if (!gameBalanceVars.PlayerTitles[j].m_isHidden)
			{
				if (filterLevels.Exists((int x) => x == level))
				{
					RewardData rewardData = new RewardData();
					rewardData.Level = level;
					rewardData.Amount = 1;
					rewardData.Type = RewardType.Title;
					rewardData.Name = gameBalanceVars.PlayerTitles[j].GetTitleText();
					rewardData.SpritePath = "QuestRewards/titleIcon";
					list.Add(rewardData);
				}
			}
		}
		while (true)
		{
			for (int k = 0; k < gameBalanceVars.ChatEmojis.Length; k++)
			{
				level = gameBalanceVars.ChatEmojis[k].GetUnlockCharacterLevel(charLink.m_characterType);
				if (!gameBalanceVars.ChatEmojis[k].m_isHidden && filterLevels.Exists((int x) => x == level))
				{
					RewardData rewardData2 = new RewardData();
					rewardData2.Level = level;
					rewardData2.Amount = 1;
					rewardData2.Type = RewardType.ChatEmoji;
					rewardData2.Name = gameBalanceVars.ChatEmojis[k].GetEmojiName();
					rewardData2.SpritePath = "QuestRewards/general";
					list.Add(rewardData2);
				}
			}
			while (true)
			{
				for (int l = 0; l < gameBalanceVars.Overcons.Length; l++)
				{
					level = gameBalanceVars.Overcons[l].GetUnlockCharacterLevel(charLink.m_characterType);
					if (!gameBalanceVars.Overcons[l].m_isHidden && filterLevels.Exists((int x) => x == level))
					{
						RewardData rewardData3 = new RewardData();
						rewardData3.Level = level;
						rewardData3.Amount = 1;
						rewardData3.Type = RewardType.Overcon;
						rewardData3.Name = gameBalanceVars.Overcons[l].GetOverconName();
						rewardData3.SpritePath = "QuestRewards/general";
						list.Add(rewardData3);
					}
				}
				while (true)
				{
					for (int m = 0; m < gameBalanceVars.LoadingScreenBackgrounds.Length; m++)
					{
						level = gameBalanceVars.LoadingScreenBackgrounds[m].GetUnlockCharacterLevel(charLink.m_characterType);
						if (gameBalanceVars.LoadingScreenBackgrounds[m].m_isHidden)
						{
							continue;
						}
						if (filterLevels.Exists((int x) => x == level))
						{
							RewardData rewardData4 = new RewardData();
							rewardData4.Level = level;
							rewardData4.Amount = 1;
							rewardData4.Type = RewardType.LoadingScreenBackground;
							rewardData4.Name = gameBalanceVars.LoadingScreenBackgrounds[m].GetLoadingScreenBackgroundName();
							rewardData4.SpritePath = gameBalanceVars.LoadingScreenBackgrounds[m].GetSpritePath();
							list.Add(rewardData4);
						}
					}
					for (int n = 0; n < gameBalanceVars.PlayerBanners.Length; n++)
					{
						level = gameBalanceVars.PlayerBanners[n].GetUnlockCharacterLevel(charLink.m_characterType);
						if (gameBalanceVars.PlayerBanners[n].m_isHidden)
						{
							continue;
						}
						if (!filterLevels.Exists((int x) => x == level))
						{
							continue;
						}
						RewardData rewardData5 = new RewardData();
						rewardData5.Level = level;
						rewardData5.Amount = 1;
						if (gameBalanceVars.PlayerBanners[n].m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
						{
							rewardData5.Type = RewardType.Banner;
						}
						else
						{
							rewardData5.Type = RewardType.Emblem;
						}
						rewardData5.Name = gameBalanceVars.PlayerBanners[n].GetBannerName();
						rewardData5.SpritePath = gameBalanceVars.PlayerBanners[n].m_iconResourceString;
						list.Add(rewardData5);
					}
					GameBalanceVars.CharacterUnlockData characterUnlockData = gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType);
					for (int num = 0; num < characterUnlockData.tauntUnlockData.Length; num++)
					{
						level = characterUnlockData.tauntUnlockData[num].GetUnlockCharacterLevel(charLink.m_characterType);
						if (charLink.m_taunts[num].m_isHidden)
						{
							continue;
						}
						if (!filterLevels.Exists((int x) => x == level))
						{
							continue;
						}
						RewardData rewardData6 = new RewardData();
						rewardData6.Level = level;
						rewardData6.Amount = 1;
						rewardData6.Type = RewardType.Taunt;
						rewardData6.Name = charLink.GetTauntName(num);
						rewardData6.SpritePath = "QuestRewards/taunt";
						AbilityData component = charLink.ActorDataPrefab.GetComponent<AbilityData>();
						if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
						{
							rewardData6.Foreground = component.m_sprite0;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
						{
							rewardData6.Foreground = component.m_sprite1;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
						{
							rewardData6.Foreground = component.m_sprite2;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
						{
							rewardData6.Foreground = component.m_sprite3;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
						{
							rewardData6.Foreground = component.m_sprite4;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_5)
						{
							rewardData6.Foreground = component.m_sprite5;
						}
						else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_6)
						{
							rewardData6.Foreground = component.m_sprite6;
						}
						list.Add(rewardData6);
					}
					while (true)
					{
						PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
						for (int num2 = 0; num2 < characterUnlockData.skinUnlockData.Length; num2++)
						{
							GameBalanceVars.SkinUnlockData skinUnlockData = characterUnlockData.skinUnlockData[num2];
							PlayerSkinData skin = playerCharacterData.CharacterComponent.GetSkin(num2);
							if (charLink.m_skins[num2].m_isHidden)
							{
								continue;
							}
							if (!skin.Unlocked)
							{
								continue;
							}
							level = skinUnlockData.GetUnlockCharacterLevel(charLink.m_characterType);
							if (filterLevels.Exists((int x) => x == level))
							{
								RewardData rewardData7 = new RewardData();
								rewardData7.Level = level;
								rewardData7.Amount = 1;
								rewardData7.Type = RewardType.Skin;
								rewardData7.Name = charLink.GetSkinName(num2);
								rewardData7.SpritePath = charLink.m_skins[num2].m_skinSelectionIconPath;
								list.Add(rewardData7);
							}
							for (int num3 = 0; num3 < skinUnlockData.patternUnlockData.Length; num3++)
							{
								for (int num4 = 0; num4 < skinUnlockData.patternUnlockData[num3].colorUnlockData.Length; num4++)
								{
									GameBalanceVars.ColorUnlockData playerUnlockable = skinUnlockData.patternUnlockData[num3].colorUnlockData[num4];
									CharacterColor characterColor = charLink.m_skins[num2].m_patterns[num3].m_colors[num4];
									if (characterColor.m_isHidden)
									{
										continue;
									}
									level = playerUnlockable.GetUnlockCharacterLevel(charLink.m_characterType);
									if (filterLevels.Exists((int x) => x == level))
									{
										RewardData rewardData8 = new RewardData();
										rewardData8.Level = level;
										rewardData8.Amount = 1;
										rewardData8.Type = RewardType.Style;
										rewardData8.Name = characterColor.m_name;
										rewardData8.SpritePath = characterColor.m_iconResourceString;
										list.Add(rewardData8);
									}
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										goto end_IL_08a5;
									}
									continue;
									end_IL_08a5:
									break;
								}
							}
						}
						List<int> list2 = filterLevels;
						
						if (list2.Exists(((int x) => x == GameWideData.Get().AdvancedSkinUnlockLevel)))
						{
							RewardData rewardData9 = new RewardData();
							rewardData9.Level = GameWideData.Get().AdvancedSkinUnlockLevel;
							rewardData9.Amount = 1;
							rewardData9.Type = RewardType.System;
							rewardData9.Name = GetStyleLevelTypeStr(StyleLevelType.Advanced);
							rewardData9.SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Advanced);
							list.Add(rewardData9);
						}
						List<int> list3 = filterLevels;
						
						if (list3.Exists(((int x) => x == GameWideData.Get().ExpertSkinUnlockLevel)))
						{
							RewardData rewardData10 = new RewardData();
							rewardData10.Level = GameWideData.Get().ExpertSkinUnlockLevel;
							rewardData10.Amount = 1;
							rewardData10.Type = RewardType.System;
							rewardData10.Name = GetStyleLevelTypeStr(StyleLevelType.Expert);
							rewardData10.SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Expert);
							list.Add(rewardData10);
						}
						if (filterLevels.Exists((int x) => x == GameWideData.Get().MasterySkinUnlockLevel))
						{
							RewardData rewardData11 = new RewardData();
							rewardData11.Level = GameWideData.Get().MasterySkinUnlockLevel;
							rewardData11.Amount = 1;
							rewardData11.Type = RewardType.System;
							rewardData11.Name = GetStyleLevelTypeStr(StyleLevelType.Mastery);
							rewardData11.SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Mastery);
							list.Add(rewardData11);
						}
						foreach (int filterLevel in filterLevels)
						{
							CurrencyData[] array = null;
							if (-1 < filterLevel - 2)
							{
								if (filterLevel - 2 < gameBalanceVars.CharacterProgressInfo.Length)
								{
									array = gameBalanceVars.CharacterProgressInfo[filterLevel - 2].CurrencyRewards;
								}
								else
								{
									array = gameBalanceVars.RepeatingCharacterProgressInfo.CurrencyRewards;
								}
								for (int num5 = 0; num5 < array.Length; num5++)
								{
									RewardData rewardData12 = new RewardData();
									rewardData12.Amount = array[num5].m_Amount;
									rewardData12.Name = string.Empty;
									rewardData12.SpritePath = GetCurrencyIconPath(array[num5].Type, out rewardData12.Type);
									rewardData12.Level = filterLevel;
									list.Add(rewardData12);
								}
							}
						}
						for (int num6 = 0; num6 < gameBalanceVars.RepeatingCharacterLevelRewards.Length; num6++)
						{
							if (gameBalanceVars.RepeatingCharacterLevelRewards[num6].charType != (int)charLink.m_characterType)
							{
								continue;
							}
							int num7 = gameBalanceVars.RepeatingCharacterLevelRewards[num6].startLevel;
							while (true)
							{
								if (filterLevels.Contains(num7))
								{
									RewardData rewardData13 = new RewardData();
									rewardData13.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[num6].reward.Amount;
									InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[num6].reward.ItemTemplateId);
									rewardData13.Name = itemTemplate.GetDisplayName();
									rewardData13.SpritePath = itemTemplate.IconPath;
									rewardData13.Level = num7;
									rewardData13.InventoryTemplate = itemTemplate;
									rewardData13.Type = GetRewardTypeFromInventoryItem(itemTemplate);
									list.Add(rewardData13);
									if (gameBalanceVars.RepeatingCharacterLevelRewards[num6].repeatingLevel > 0)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												goto end_IL_0c4d;
											}
											continue;
											end_IL_0c4d:
											break;
										}
										num7 += gameBalanceVars.RepeatingCharacterLevelRewards[num6].repeatingLevel;
										continue;
									}
									break;
								}
								break;
							}
						}
						while (true)
						{
							
							list.Sort(((RewardData first, RewardData second) => first.Level - second.Level));
							return list;
						}
					}
				}
			}
		}
	}

	public static bool DidPlayerUnlockPvPQueue(int nextSeasonLevel, bool checkUsingPreviousLevel = false)
	{
		bool result = false;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		if (playerAccountData != null)
		{
			RequirementCollection requirementCollection = null;
			if (ClientGameManager.Get().GameTypeAvailabilies != null && ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(GameType.PvP))
			{
				RequirementCollection requirements = ClientGameManager.Get().GameTypeAvailabilies[GameType.PvP].Requirements;
				if (requirements != null)
				{
					requirementCollection = requirements.Where(delegate(QueueRequirement p)
					{
						int result2;
						if (p.Requirement != QueueRequirement.RequirementType.TotalLevel)
						{
							result2 = ((p.Requirement == QueueRequirement.RequirementType.TotalMatches) ? 1 : 0);
						}
						else
						{
							result2 = 1;
						}
						return (byte)result2 != 0;
					});
				}
			}
			IQueueRequirementSystemInfo queueRequirementSystemInfo = ClientGameManager.Get().QueueRequirementSystemInfo;
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
			IQueueRequirementApplicant queueRequirementApplicant = ClientGameManager.Get().QueueRequirementApplicant;
			int isInTutorial;
			if (seasonTemplate != null)
			{
				isInTutorial = (seasonTemplate.IsTutorial ? 1 : 0);
			}
			else
			{
				isInTutorial = 0;
			}
			TmpRequirementCheckApplicant tmpRequirementCheckApplicant = new TmpRequirementCheckApplicant(queueRequirementApplicant, (byte)isInTutorial != 0);
			if (checkUsingPreviousLevel)
			{
				tmpRequirementCheckApplicant.SeasonLevel = nextSeasonLevel - 1;
			}
			if (requirementCollection != null)
			{
				if (!requirementCollection.DoesApplicantPass(queueRequirementSystemInfo, tmpRequirementCheckApplicant, GameType.PvP, null))
				{
					tmpRequirementCheckApplicant.SeasonLevel = nextSeasonLevel;
					result = requirementCollection.DoesApplicantPass(queueRequirementSystemInfo, tmpRequirementCheckApplicant, GameType.PvP, null);
				}
			}
		}
		return result;
	}

	public static List<RewardData> GetNextSeasonLevelRewards(int currentSeasonLevel)
	{
		List<RewardData> list = new List<RewardData>();
		list.AddRange(GetAccountLevelRewards(currentSeasonLevel + 1));
		list.AddRange(GetSeasonLevelRewards(currentSeasonLevel + 1));
		return list;
	}

	public static List<RewardData> GetAccountLevelRewards(int seasonLevel)
	{
		List<RewardData> list = new List<RewardData>();
		if (DidPlayerUnlockPvPQueue(seasonLevel, true))
		{
			RewardData rewardData = new RewardData();
			rewardData.Level = seasonLevel;
			rewardData.Amount = 1;
			rewardData.Type = RewardType.System;
			rewardData.Name = StringUtil.TR("PvPQueueUnlocked", "Rewards");
			rewardData.SpritePath = "QuestRewards/contract";
			list.Add(rewardData);
		}
		return list;
	}

	public static List<RewardData> GetSeasonLevelRewards(int onlyForLevel = -1)
	{
		List<RewardData> list = new List<RewardData>();
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
		if (seasonTemplate == null)
		{
			return list;
		}
		List<SeasonReward> allRewards = seasonTemplate.Rewards.GetAllRewards();
		for (int i = 0; i < allRewards.Count; i++)
		{
			if (onlyForLevel > 0)
			{
				if (allRewards[i].repeatEveryXLevels == 0)
				{
					if (allRewards[i].level != onlyForLevel)
					{
						continue;
					}
				}
				else if ((onlyForLevel - allRewards[i].level) % allRewards[i].repeatEveryXLevels != 0)
				{
					continue;
				}
				if (allRewards[i] is SeasonItemReward)
				{
					SeasonItemReward seasonItemReward = allRewards[i] as SeasonItemReward;
					bool tryUseCharDataOnInitialLoad = playerAccountData.SeasonLevel <= onlyForLevel;
					if (!seasonItemReward.Conditions.IsNullOrEmpty())
					{
						if (!QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement, tryUseCharDataOnInitialLoad))
						{
							continue;
						}
					}
				}
			}
			if (allRewards[i] is SeasonCurrencyReward)
			{
				SeasonCurrencyReward seasonCurrencyReward = allRewards[i] as SeasonCurrencyReward;
				InventoryItemTemplate itemTemplate = seasonCurrencyReward.GetItemTemplate();
				RewardData rewardData = new RewardData();
				rewardData.Amount = seasonCurrencyReward.CurrencyReward.Amount;
				rewardData.Name = itemTemplate.GetDisplayName();
				rewardData.Level = allRewards[i].level;
				rewardData.SpritePath = InventoryWideData.GetSpritePath(itemTemplate);
				rewardData.Foreground = InventoryWideData.GetItemFg(itemTemplate);
				rewardData.InventoryTemplate = itemTemplate;
				rewardData.isRepeating = (allRewards[i].repeatEveryXLevels != 0);
				rewardData.repeatLevels = allRewards[i].repeatEveryXLevels;
				CurrencyType type = seasonCurrencyReward.CurrencyReward.Type;
				switch (type)
				{
				default:
					if (type == CurrencyType.UnlockFreelancerToken)
					{
						rewardData.Type = RewardType.UnlockFreelancerToken;
					}
					break;
				case CurrencyType.ModToken:
					rewardData.Type = RewardType.ModToken;
					break;
				case CurrencyType.GGPack:
					rewardData.Type = RewardType.GGBoost;
					break;
				case CurrencyType.ISO:
					rewardData.Type = RewardType.ISO;
					break;
				case CurrencyType.FreelancerCurrency:
					rewardData.Type = RewardType.FreelancerCurrency;
					break;
				}
				list.Add(rewardData);
			}
			else if (allRewards[i] is SeasonItemReward)
			{
				InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate((allRewards[i] as SeasonItemReward).ItemReward.ItemTemplateId);
				RewardData rewardData2 = new RewardData();
				rewardData2.Amount = 1;
				rewardData2.Name = itemTemplate2.DisplayName;
				rewardData2.Level = allRewards[i].level;
				rewardData2.SpritePath = InventoryWideData.GetSpritePath(itemTemplate2);
				rewardData2.Foreground = InventoryWideData.GetItemFg(itemTemplate2);
				rewardData2.InventoryTemplate = itemTemplate2;
				rewardData2.Type = GetRewardTypeFromInventoryItem(itemTemplate2);
				rewardData2.isRepeating = (allRewards[i].repeatEveryXLevels != 0);
				rewardData2.repeatLevels = allRewards[i].repeatEveryXLevels;
				list.Add(rewardData2);
			}
		}
		while (true)
		{
			list.Sort((RewardData first, RewardData second) => first.Level - second.Level);
			return list;
		}
	}

	public static List<RewardData> GetAvailableSeasonEndRewards(SeasonTemplate season)
	{
		int endLevel = QuestWideData.GetEndLevel(season.Prerequisites, season.Index);
		List<RewardData> list = new List<RewardData>();
		List<SeasonTemplate.SeasonEndRewards> availableEndRewards = SeasonWideData.Get().GetAvailableEndRewards(season);
		int num = 0;
		while (num < availableEndRewards.Count)
		{
			for (int i = 0; i < availableEndRewards[num].CurrencyRewards.Count; i++)
			{
				QuestCurrencyReward questCurrencyReward = availableEndRewards[num].CurrencyRewards[i];
				RewardData rewardData = new RewardData();
				rewardData.Amount = questCurrencyReward.Amount;
				rewardData.Level = endLevel;
				rewardData.SpritePath = GetCurrencyIconPath(questCurrencyReward.Type, out rewardData.Type);
				rewardData.Name = GetDisplayString(rewardData);
				list.Add(rewardData);
			}
			while (true)
			{
				for (int j = 0; j < availableEndRewards[num].ItemRewards.Count; j++)
				{
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(availableEndRewards[num].ItemRewards[j].ItemTemplateId);
					RewardData rewardData2 = new RewardData();
					rewardData2.Amount = 1;
					rewardData2.Name = itemTemplate.GetDisplayName();
					rewardData2.Level = endLevel;
					rewardData2.SpritePath = InventoryWideData.GetSpritePath(itemTemplate);
					rewardData2.Foreground = InventoryWideData.GetItemFg(itemTemplate);
					rewardData2.InventoryTemplate = itemTemplate;
					rewardData2.Type = GetRewardTypeFromInventoryItem(itemTemplate);
					list.Add(rewardData2);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						goto end_IL_018a;
					}
					continue;
					end_IL_018a:
					break;
				}
				num++;
				goto IL_0198;
			}
			IL_0198:;
		}
		return list;
	}

	public static string GetStyleLevelTypeStr(StyleLevelType styleLevelType)
	{
		string arg = string.Empty;
		if (styleLevelType != StyleLevelType.Advanced)
		{
			if (styleLevelType != StyleLevelType.Expert)
			{
				if (styleLevelType != StyleLevelType.Mastery)
				{
				}
				else
				{
					arg = StringUtil.TR("Mastery", "Global");
				}
			}
			else
			{
				arg = StringUtil.TR("Expert", "Global");
			}
		}
		else
		{
			arg = StringUtil.TR("Advanced", "Global");
		}
		return string.Format(StringUtil.TR("StyleLevelType", "Rewards"), arg);
	}

	public static string GetTypeDisplayString(RewardType type, bool isPlural)
	{
		switch (type)
		{
		case RewardType.ModToken:
			return (!isPlural) ? StringUtil.TR("ModToken", "Rewards") : StringUtil.TR("ModTokens", "Rewards");
		case RewardType.GGBoost:
		{
			string result3;
			if (isPlural)
			{
				result3 = StringUtil.TR("GGBoosts", "Rewards");
			}
			else
			{
				result3 = StringUtil.TR("GGBoost", "Rewards");
			}
			return result3;
		}
		case RewardType.ISO:
			return StringUtil.TR("ISO", "Rewards");
		case RewardType.System:
		{
			string result2;
			if (isPlural)
			{
				result2 = StringUtil.TR("SystemRewards", "Rewards");
			}
			else
			{
				result2 = StringUtil.TR("System", "Rewards");
			}
			return result2;
		}
		case RewardType.Lockbox:
		{
			string result4;
			if (isPlural)
			{
				result4 = StringUtil.TR("LootMatrices", "Rewards");
			}
			else
			{
				result4 = StringUtil.TR("LootMatrix", "Rewards");
			}
			return result4;
		}
		case RewardType.AbilityVfxSwap:
		{
			string result;
			if (isPlural)
			{
				result = StringUtil.TR("AbilityEffects", "Rewards");
			}
			else
			{
				result = StringUtil.TR("AbilityEffect", "Rewards");
			}
			return result;
		}
		case RewardType.Faction:
			return StringUtil.TR("Faction", "Inventory");
		case RewardType.Style:
			return (!isPlural) ? StringUtil.TR("Skin", "Rewards") : StringUtil.TR("Skins", "Rewards");
		case RewardType.Overcon:
			return StringUtil.TR("Overcon", "Inventory");
		case RewardType.FreelancerCurrency:
			return StringUtil.TR("FreelancerCurrency", "Global");
		case RewardType.LoadingScreenBackground:
			return StringUtil.TR("LoadingScreenBackground", "Inventory");
		case RewardType.FreelancerExpBonus:
			return StringUtil.TR("FreelancerExpBonus", "Global");
		default:
		{
			string str = type.ToString();
			object str2;
			if (isPlural)
			{
				str2 = "s";
			}
			else
			{
				str2 = string.Empty;
			}
			string term = str + (string)str2;
			return StringUtil.TR(term, "Rewards");
		}
		}
	}

	public static string GetDisplayString(RewardData reward, bool ignoreType = false)
	{
		RewardType type = reward.Type;
		switch (type)
		{
		default:
			if (type != RewardType.UnlockFreelancerToken)
			{
				if (ignoreType)
				{
					return reward.Name;
				}
				return GetTypeDisplayString(reward.Type, reward.Amount > 1) + ": " + reward.Name;
			}
			goto case RewardType.FreelancerCurrency;
		case RewardType.FreelancerCurrency:
		case RewardType.ModToken:
		case RewardType.GGBoost:
		case RewardType.ISO:
			return reward.Amount + " " + GetTypeDisplayString(reward.Type, reward.Amount > 1);
		case RewardType.System:
			return reward.Name;
		}
	}

	public static string GetCurrencyIconPath(CurrencyType currencyType, out RewardType rewardType)
	{
		switch (currencyType)
		{
		case CurrencyType.ModToken:
			rewardType = RewardType.ModToken;
			return "QuestRewards/modToken";
		case CurrencyType.GGPack:
			rewardType = RewardType.GGBoost;
			return "QuestRewards/ggPack_01";
		case CurrencyType.ISO:
			rewardType = RewardType.ISO;
			return "QuestRewards/iso_01";
		case CurrencyType.RankedCurrency:
			rewardType = RewardType.RankedCurrency;
			return "QuestRewards/rankedCurrency";
		case CurrencyType.FreelancerCurrency:
			rewardType = RewardType.FreelancerCurrency;
			return "QuestRewards/freelancerCurrency_01";
		case CurrencyType.UnlockFreelancerToken:
			rewardType = RewardType.UnlockFreelancerToken;
			return "QuestRewards/FreelancerCoin";
		case CurrencyType.Experience:
			rewardType = RewardType.System;
			return "Localization/" + StringUtil.TR("EXP", "TEXTURE");
		default:
			throw new Exception("Invalid currency type: " + currencyType);
		}
	}

	public static string GetCurrencyIconPath(CurrencyType currencyType)
	{
		RewardType rewardType;
		return GetCurrencyIconPath(currencyType, out rewardType);
	}
}
