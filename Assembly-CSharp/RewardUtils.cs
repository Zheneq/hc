using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardUtils
{
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
		case PurchaseType.Character:
			text = text + StringUtil.TR("Freelancer", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterDisplayName((CharacterType)rewardData[0]);
			break;
		case PurchaseType.Skin:
			text = text + StringUtil.TR("Skin", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetSkinName(rewardData[1]);
			break;
		case PurchaseType.Texture:
		case PurchaseType.ModToken:
			throw new Exception("Invalid unlock type");
		case PurchaseType.Tint:
			text = text + StringUtil.TR("Skin", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetPatternColorName(rewardData[1], rewardData[2], rewardData[3]);
			break;
		case PurchaseType.Taunt:
			text = text + StringUtil.TR("Taunt", "Rewards") + " ";
			text += GameWideData.Get().GetCharacterResourceLink((CharacterType)rewardData[0]).GetTauntName(rewardData[1]);
			break;
		case PurchaseType.Title:
			text = text + StringUtil.TR("Title", "Rewards") + " ";
			text += GameBalanceVars.Get().GetTitle(rewardData[0], string.Empty, -1);
			break;
		case PurchaseType.BannerBackground:
			text = text + StringUtil.TR("Banner", "Rewards") + " ";
			text += GameBalanceVars.Get().GetBannerName(rewardData[0], string.Empty);
			break;
		case PurchaseType.BannerForeground:
			text = text + StringUtil.TR("Emblem", "Rewards") + " ";
			text += GameBalanceVars.Get().GetBannerName(rewardData[0], string.Empty);
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
		}
		return text;
	}

	public static List<RewardUtils.RewardData> GetNextAccountRewards(int currentLevel)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			return new List<RewardUtils.RewardData>();
		}
		int num = currentLevel + 1;
		if (num > gameBalanceVars.MaxPlayerLevel)
		{
			return new List<RewardUtils.RewardData>();
		}
		return RewardUtils.GetAccountRewards(new List<int>
		{
			num
		});
	}

	public static List<RewardUtils.RewardData> GetAccountRewards(List<int> filterLevels = null)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			return list;
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
				list.Add(new RewardUtils.RewardData
				{
					Level = level,
					Amount = 1,
					Type = RewardUtils.RewardType.Title,
					Name = gameBalanceVars.PlayerTitles[j].GetTitleText(-1),
					SpritePath = "QuestRewards/titleIcon"
				});
			}
		}
		for (int k = 0; k < gameBalanceVars.ChatEmojis.Length; k++)
		{
			level = gameBalanceVars.ChatEmojis[k].GetUnlockPlayerLevel();
			if (filterLevels.Exists((int x) => x == level))
			{
				if (!gameBalanceVars.ChatEmojis[k].m_isHidden)
				{
					list.Add(new RewardUtils.RewardData
					{
						Level = level,
						Amount = 1,
						Type = RewardUtils.RewardType.ChatEmoji,
						Name = gameBalanceVars.ChatEmojis[k].GetEmojiName(),
						SpritePath = "QuestRewards/general"
					});
				}
			}
		}
		for (int l = 0; l < gameBalanceVars.Overcons.Length; l++)
		{
			level = gameBalanceVars.Overcons[l].GetUnlockPlayerLevel();
			if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.Overcons[l].m_isHidden)
			{
				list.Add(new RewardUtils.RewardData
				{
					Level = level,
					Amount = 1,
					Type = RewardUtils.RewardType.Overcon,
					Name = gameBalanceVars.Overcons[l].GetOverconName(),
					SpritePath = "QuestRewards/general"
				});
			}
		}
		for (int m = 0; m < gameBalanceVars.LoadingScreenBackgrounds.Length; m++)
		{
			level = gameBalanceVars.LoadingScreenBackgrounds[m].GetUnlockPlayerLevel();
			if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.LoadingScreenBackgrounds[m].m_isHidden)
			{
				list.Add(new RewardUtils.RewardData
				{
					Level = level,
					Amount = 1,
					Type = RewardUtils.RewardType.LoadingScreenBackground,
					Name = gameBalanceVars.LoadingScreenBackgrounds[m].GetLoadingScreenBackgroundName(),
					SpritePath = gameBalanceVars.LoadingScreenBackgrounds[m].GetSpritePath()
				});
			}
		}
		for (int n = 0; n < gameBalanceVars.PlayerBanners.Length; n++)
		{
			level = gameBalanceVars.PlayerBanners[n].GetUnlockPlayerLevel();
			if (filterLevels.Exists((int x) => x == level) && !gameBalanceVars.PlayerBanners[n].m_isHidden)
			{
				RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
				rewardData.Level = level;
				rewardData.Amount = 1;
				if (gameBalanceVars.PlayerBanners[n].m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
				{
					rewardData.Type = RewardUtils.RewardType.Banner;
				}
				else
				{
					rewardData.Type = RewardUtils.RewardType.Emblem;
				}
				rewardData.Name = gameBalanceVars.PlayerBanners[n].GetBannerName();
				rewardData.SpritePath = gameBalanceVars.PlayerBanners[n].m_iconResourceString;
				list.Add(rewardData);
			}
		}
		foreach (int num in filterLevels)
		{
			CurrencyData[] currencyRewards = gameBalanceVars.PlayerProgressInfo[num - 2].CurrencyRewards;
			for (int num2 = 0; num2 < currencyRewards.Length; num2++)
			{
				RewardUtils.RewardData rewardData2 = new RewardUtils.RewardData();
				rewardData2.Amount = currencyRewards[num2].m_Amount;
				rewardData2.Name = string.Empty;
				rewardData2.SpritePath = RewardUtils.GetCurrencyIconPath(currencyRewards[num2].Type, out rewardData2.Type);
				rewardData2.Level = num;
				list.Add(rewardData2);
			}
		}
		List<RewardUtils.RewardData> list2 = list;
		
		list2.Sort(((RewardUtils.RewardData first, RewardUtils.RewardData second) => first.Level - second.Level));
		return list;
	}

	public static RewardUtils.RewardData GetSeasonsUnlockedReward()
	{
		return new RewardUtils.RewardData
		{
			Amount = 1,
			Type = RewardUtils.RewardType.System,
			Name = StringUtil.TR("SeasonsUnlocked", "Rewards"),
			SpritePath = "QuestRewards/contract"
		};
	}

	public static List<RewardUtils.RewardData> GetNextCharacterRewards(CharacterResourceLink charLink, int currentLevel)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			return new List<RewardUtils.RewardData>();
		}
		int num = currentLevel + 1;
		if (num > gameBalanceVars.MaxCharacterLevelForRewards)
		{
			return new List<RewardUtils.RewardData>();
		}
		return RewardUtils.GetCharacterRewards(charLink, new List<int>
		{
			num
		});
	}

	public static RewardUtils.RewardType GetRewardTypeFromInventoryItem(InventoryItemTemplate template)
	{
		RewardUtils.RewardType result = RewardUtils.RewardType.ISO;
		switch (template.Type)
		{
		case InventoryItemType.TitleID:
			return RewardUtils.RewardType.Title;
		case InventoryItemType.BannerID:
			return RewardUtils.RewardType.Banner;
		case InventoryItemType.Skin:
			return RewardUtils.RewardType.Skin;
		case InventoryItemType.Style:
			return RewardUtils.RewardType.Style;
		case InventoryItemType.Taunt:
			return RewardUtils.RewardType.Taunt;
		case InventoryItemType.Mod:
			return RewardUtils.RewardType.Mod;
		case InventoryItemType.Lockbox:
			return RewardUtils.RewardType.Lockbox;
		case InventoryItemType.Currency:
		{
			CurrencyType currencyType = (CurrencyType)template.TypeSpecificData[0];
			if (currencyType != CurrencyType.ModToken)
			{
				if (currencyType != CurrencyType.GGPack)
				{
					if (currencyType != CurrencyType.ISO)
					{
					}
					else
					{
						result = RewardUtils.RewardType.ISO;
					}
				}
				else
				{
					result = RewardUtils.RewardType.GGBoost;
				}
			}
			else
			{
				result = RewardUtils.RewardType.ModToken;
			}
			return result;
		}
		case InventoryItemType.ChatEmoji:
			return RewardUtils.RewardType.ChatEmoji;
		case InventoryItemType.Material:
			return RewardUtils.RewardType.Material;
		case InventoryItemType.AbilityVfxSwap:
			return RewardUtils.RewardType.AbilityVfxSwap;
		case InventoryItemType.Overcon:
			return RewardUtils.RewardType.Overcon;
		case InventoryItemType.Faction:
			return RewardUtils.RewardType.Faction;
		case InventoryItemType.Unlock:
			return RewardUtils.RewardType.System;
		case InventoryItemType.Conveyance:
			return RewardUtils.RewardType.System;
		case InventoryItemType.FreelancerExpBonus:
			return RewardUtils.RewardType.FreelancerExpBonus;
		case InventoryItemType.LoadingScreenBackground:
			return RewardUtils.RewardType.LoadingScreenBackground;
		}
		throw new Exception("Not supported: " + template.Type);
	}

	public static List<RewardUtils.RewardData> GetCharacterRewards(CharacterResourceLink charLink, List<int> filterLevels = null)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
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
			level = gameBalanceVars.PlayerTitles[j].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!gameBalanceVars.PlayerTitles[j].m_isHidden)
			{
				if (filterLevels.Exists((int x) => x == level))
				{
					list.Add(new RewardUtils.RewardData
					{
						Level = level,
						Amount = 1,
						Type = RewardUtils.RewardType.Title,
						Name = gameBalanceVars.PlayerTitles[j].GetTitleText(-1),
						SpritePath = "QuestRewards/titleIcon"
					});
				}
			}
		}
		for (int k = 0; k < gameBalanceVars.ChatEmojis.Length; k++)
		{
			level = gameBalanceVars.ChatEmojis[k].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!gameBalanceVars.ChatEmojis[k].m_isHidden && filterLevels.Exists((int x) => x == level))
			{
				list.Add(new RewardUtils.RewardData
				{
					Level = level,
					Amount = 1,
					Type = RewardUtils.RewardType.ChatEmoji,
					Name = gameBalanceVars.ChatEmojis[k].GetEmojiName(),
					SpritePath = "QuestRewards/general"
				});
			}
		}
		for (int l = 0; l < gameBalanceVars.Overcons.Length; l++)
		{
			level = gameBalanceVars.Overcons[l].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!gameBalanceVars.Overcons[l].m_isHidden && filterLevels.Exists((int x) => x == level))
			{
				list.Add(new RewardUtils.RewardData
				{
					Level = level,
					Amount = 1,
					Type = RewardUtils.RewardType.Overcon,
					Name = gameBalanceVars.Overcons[l].GetOverconName(),
					SpritePath = "QuestRewards/general"
				});
			}
		}
		for (int m = 0; m < gameBalanceVars.LoadingScreenBackgrounds.Length; m++)
		{
			level = gameBalanceVars.LoadingScreenBackgrounds[m].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!gameBalanceVars.LoadingScreenBackgrounds[m].m_isHidden)
			{
				if (filterLevels.Exists((int x) => x == level))
				{
					list.Add(new RewardUtils.RewardData
					{
						Level = level,
						Amount = 1,
						Type = RewardUtils.RewardType.LoadingScreenBackground,
						Name = gameBalanceVars.LoadingScreenBackgrounds[m].GetLoadingScreenBackgroundName(),
						SpritePath = gameBalanceVars.LoadingScreenBackgrounds[m].GetSpritePath()
					});
				}
			}
		}
		for (int n = 0; n < gameBalanceVars.PlayerBanners.Length; n++)
		{
			level = gameBalanceVars.PlayerBanners[n].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!gameBalanceVars.PlayerBanners[n].m_isHidden)
			{
				if (filterLevels.Exists((int x) => x == level))
				{
					RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
					rewardData.Level = level;
					rewardData.Amount = 1;
					if (gameBalanceVars.PlayerBanners[n].m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
					{
						rewardData.Type = RewardUtils.RewardType.Banner;
					}
					else
					{
						rewardData.Type = RewardUtils.RewardType.Emblem;
					}
					rewardData.Name = gameBalanceVars.PlayerBanners[n].GetBannerName();
					rewardData.SpritePath = gameBalanceVars.PlayerBanners[n].m_iconResourceString;
					list.Add(rewardData);
				}
			}
		}
		GameBalanceVars.CharacterUnlockData characterUnlockData = gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType);
		for (int num = 0; num < characterUnlockData.tauntUnlockData.Length; num++)
		{
			level = characterUnlockData.tauntUnlockData[num].GetUnlockCharacterLevel(charLink.m_characterType, false);
			if (!charLink.m_taunts[num].m_isHidden)
			{
				if (filterLevels.Exists((int x) => x == level))
				{
					RewardUtils.RewardData rewardData2 = new RewardUtils.RewardData();
					rewardData2.Level = level;
					rewardData2.Amount = 1;
					rewardData2.Type = RewardUtils.RewardType.Taunt;
					rewardData2.Name = charLink.GetTauntName(num);
					rewardData2.SpritePath = "QuestRewards/taunt";
					AbilityData component = charLink.ActorDataPrefab.GetComponent<AbilityData>();
					if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
					{
						rewardData2.Foreground = component.m_sprite0;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
					{
						rewardData2.Foreground = component.m_sprite1;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
					{
						rewardData2.Foreground = component.m_sprite2;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
					{
						rewardData2.Foreground = component.m_sprite3;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
					{
						rewardData2.Foreground = component.m_sprite4;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_5)
					{
						rewardData2.Foreground = component.m_sprite5;
					}
					else if (charLink.m_taunts[num].m_actionForTaunt == AbilityData.ActionType.ABILITY_6)
					{
						rewardData2.Foreground = component.m_sprite6;
					}
					list.Add(rewardData2);
				}
			}
		}
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
		int num2 = 0;
		while (num2 < characterUnlockData.skinUnlockData.Length)
		{
			GameBalanceVars.SkinUnlockData skinUnlockData = characterUnlockData.skinUnlockData[num2];
			PlayerSkinData skin = playerCharacterData.CharacterComponent.GetSkin(num2);
			if (!charLink.m_skins[num2].m_isHidden)
			{
				if (!skin.Unlocked)
				{
				}
				else
				{
					level = skinUnlockData.GetUnlockCharacterLevel(charLink.m_characterType, false);
					if (filterLevels.Exists((int x) => x == level))
					{
						list.Add(new RewardUtils.RewardData
						{
							Level = level,
							Amount = 1,
							Type = RewardUtils.RewardType.Skin,
							Name = charLink.GetSkinName(num2),
							SpritePath = charLink.m_skins[num2].m_skinSelectionIconPath
						});
					}
					for (int num3 = 0; num3 < skinUnlockData.patternUnlockData.Length; num3++)
					{
						for (int num4 = 0; num4 < skinUnlockData.patternUnlockData[num3].colorUnlockData.Length; num4++)
						{
							GameBalanceVars.ColorUnlockData playerUnlockable = skinUnlockData.patternUnlockData[num3].colorUnlockData[num4];
							CharacterColor characterColor = charLink.m_skins[num2].m_patterns[num3].m_colors[num4];
							if (characterColor.m_isHidden)
							{
							}
							else
							{
								level = playerUnlockable.GetUnlockCharacterLevel(charLink.m_characterType, false);
								if (filterLevels.Exists((int x) => x == level))
								{
									list.Add(new RewardUtils.RewardData
									{
										Level = level,
										Amount = 1,
										Type = RewardUtils.RewardType.Style,
										Name = characterColor.m_name,
										SpritePath = characterColor.m_iconResourceString
									});
								}
							}
						}
					}
				}
			}
			IL_8CF:
			num2++;
			continue;
			goto IL_8CF;
		}
		List<int> list2 = filterLevels;
		
		if (list2.Exists(((int x) => x == GameWideData.Get().AdvancedSkinUnlockLevel)))
		{
			list.Add(new RewardUtils.RewardData
			{
				Level = GameWideData.Get().AdvancedSkinUnlockLevel,
				Amount = 1,
				Type = RewardUtils.RewardType.System,
				Name = RewardUtils.GetStyleLevelTypeStr(StyleLevelType.Advanced),
				SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Advanced)
			});
		}
		List<int> list3 = filterLevels;
		
		if (list3.Exists(((int x) => x == GameWideData.Get().ExpertSkinUnlockLevel)))
		{
			list.Add(new RewardUtils.RewardData
			{
				Level = GameWideData.Get().ExpertSkinUnlockLevel,
				Amount = 1,
				Type = RewardUtils.RewardType.System,
				Name = RewardUtils.GetStyleLevelTypeStr(StyleLevelType.Expert),
				SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Expert)
			});
		}
		if (filterLevels.Exists((int x) => x == GameWideData.Get().MasterySkinUnlockLevel))
		{
			list.Add(new RewardUtils.RewardData
			{
				Level = GameWideData.Get().MasterySkinUnlockLevel,
				Amount = 1,
				Type = RewardUtils.RewardType.System,
				Name = RewardUtils.GetStyleLevelTypeStr(StyleLevelType.Mastery),
				SpritePath = CharacterColor.GetIconResourceStringForStyleLevelType(StyleLevelType.Mastery)
			});
		}
		foreach (int num5 in filterLevels)
		{
			if (-1 < num5 - 2)
			{
				CurrencyData[] currencyRewards;
				if (num5 - 2 < gameBalanceVars.CharacterProgressInfo.Length)
				{
					currencyRewards = gameBalanceVars.CharacterProgressInfo[num5 - 2].CurrencyRewards;
				}
				else
				{
					currencyRewards = gameBalanceVars.RepeatingCharacterProgressInfo.CurrencyRewards;
				}
				for (int num6 = 0; num6 < currencyRewards.Length; num6++)
				{
					RewardUtils.RewardData rewardData3 = new RewardUtils.RewardData();
					rewardData3.Amount = currencyRewards[num6].m_Amount;
					rewardData3.Name = string.Empty;
					rewardData3.SpritePath = RewardUtils.GetCurrencyIconPath(currencyRewards[num6].Type, out rewardData3.Type);
					rewardData3.Level = num5;
					list.Add(rewardData3);
				}
			}
		}
		for (int num7 = 0; num7 < gameBalanceVars.RepeatingCharacterLevelRewards.Length; num7++)
		{
			if (gameBalanceVars.RepeatingCharacterLevelRewards[num7].charType == (int)charLink.m_characterType)
			{
				int num8 = gameBalanceVars.RepeatingCharacterLevelRewards[num7].startLevel;
				while (filterLevels.Contains(num8))
				{
					RewardUtils.RewardData rewardData4 = new RewardUtils.RewardData();
					rewardData4.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[num7].reward.Amount;
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[num7].reward.ItemTemplateId);
					rewardData4.Name = itemTemplate.GetDisplayName();
					rewardData4.SpritePath = itemTemplate.IconPath;
					rewardData4.Level = num8;
					rewardData4.InventoryTemplate = itemTemplate;
					rewardData4.Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate);
					list.Add(rewardData4);
					if (gameBalanceVars.RepeatingCharacterLevelRewards[num7].repeatingLevel <= 0)
					{
						goto IL_C87;
					}
					num8 += gameBalanceVars.RepeatingCharacterLevelRewards[num7].repeatingLevel;
				}
			}
			IL_C87:;
		}
		List<RewardUtils.RewardData> list4 = list;
		
		list4.Sort(((RewardUtils.RewardData first, RewardUtils.RewardData second) => first.Level - second.Level));
		return list;
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
						bool result2;
						if (p.Requirement != QueueRequirement.RequirementType.TotalLevel)
						{
							result2 = (p.Requirement == QueueRequirement.RequirementType.TotalMatches);
						}
						else
						{
							result2 = true;
						}
						return result2;
					});
				}
			}
			IQueueRequirementSystemInfo queueRequirementSystemInfo = ClientGameManager.Get().QueueRequirementSystemInfo;
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
			IQueueRequirementApplicant queueRequirementApplicant = ClientGameManager.Get().QueueRequirementApplicant;
			bool isInTutorial;
			if (seasonTemplate != null)
			{
				isInTutorial = seasonTemplate.IsTutorial;
			}
			else
			{
				isInTutorial = false;
			}
			RewardUtils.TmpRequirementCheckApplicant tmpRequirementCheckApplicant = new RewardUtils.TmpRequirementCheckApplicant(queueRequirementApplicant, isInTutorial);
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

	public static List<RewardUtils.RewardData> GetNextSeasonLevelRewards(int currentSeasonLevel)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		list.AddRange(RewardUtils.GetAccountLevelRewards(currentSeasonLevel + 1));
		list.AddRange(RewardUtils.GetSeasonLevelRewards(currentSeasonLevel + 1));
		return list;
	}

	public static List<RewardUtils.RewardData> GetAccountLevelRewards(int seasonLevel)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		if (RewardUtils.DidPlayerUnlockPvPQueue(seasonLevel, true))
		{
			list.Add(new RewardUtils.RewardData
			{
				Level = seasonLevel,
				Amount = 1,
				Type = RewardUtils.RewardType.System,
				Name = StringUtil.TR("PvPQueueUnlocked", "Rewards"),
				SpritePath = "QuestRewards/contract"
			});
		}
		return list;
	}

	public static List<RewardUtils.RewardData> GetSeasonLevelRewards(int onlyForLevel = -1)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
		if (seasonTemplate == null)
		{
			return list;
		}
		List<SeasonReward> allRewards = seasonTemplate.Rewards.GetAllRewards();
		int i = 0;
		while (i < allRewards.Count)
		{
			if (onlyForLevel <= 0)
			{
				goto IL_146;
			}
			if (allRewards[i].repeatEveryXLevels == 0)
			{
				if (allRewards[i].level != onlyForLevel)
				{
					goto IL_37C;
				}
			}
			else if ((onlyForLevel - allRewards[i].level) % allRewards[i].repeatEveryXLevels != 0)
			{
				goto IL_37C;
			}
			if (!(allRewards[i] is SeasonItemReward))
			{
				goto IL_146;
			}
			SeasonItemReward seasonItemReward = allRewards[i] as SeasonItemReward;
			bool tryUseCharDataOnInitialLoad = playerAccountData.SeasonLevel <= onlyForLevel;
			if (seasonItemReward.Conditions.IsNullOrEmpty<QuestCondition>())
			{
				goto IL_146;
			}
			if (QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement, tryUseCharDataOnInitialLoad))
			{
				goto IL_146;
			}
			IL_37C:
			i++;
			continue;
			IL_146:
			if (allRewards[i] is SeasonCurrencyReward)
			{
				SeasonCurrencyReward seasonCurrencyReward = allRewards[i] as SeasonCurrencyReward;
				InventoryItemTemplate itemTemplate = seasonCurrencyReward.GetItemTemplate();
				RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
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
				case CurrencyType.ISO:
					rewardData.Type = RewardUtils.RewardType.ISO;
					break;
				case CurrencyType.ModToken:
					rewardData.Type = RewardUtils.RewardType.ModToken;
					break;
				default:
					if (type != CurrencyType.FreelancerCurrency)
					{
						if (type == CurrencyType.UnlockFreelancerToken)
						{
							rewardData.Type = RewardUtils.RewardType.UnlockFreelancerToken;
						}
					}
					else
					{
						rewardData.Type = RewardUtils.RewardType.FreelancerCurrency;
					}
					break;
				case CurrencyType.GGPack:
					rewardData.Type = RewardUtils.RewardType.GGBoost;
					break;
				}
				list.Add(rewardData);
				goto IL_37C;
			}
			if (allRewards[i] is SeasonItemReward)
			{
				InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate((allRewards[i] as SeasonItemReward).ItemReward.ItemTemplateId);
				list.Add(new RewardUtils.RewardData
				{
					Amount = 1,
					Name = itemTemplate2.DisplayName,
					Level = allRewards[i].level,
					SpritePath = InventoryWideData.GetSpritePath(itemTemplate2),
					Foreground = InventoryWideData.GetItemFg(itemTemplate2),
					InventoryTemplate = itemTemplate2,
					Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate2),
					isRepeating = (allRewards[i].repeatEveryXLevels != 0),
					repeatLevels = allRewards[i].repeatEveryXLevels
				});
				goto IL_37C;
			}
			goto IL_37C;
		}
		list.Sort((RewardUtils.RewardData first, RewardUtils.RewardData second) => first.Level - second.Level);
		return list;
	}

	public static List<RewardUtils.RewardData> GetAvailableSeasonEndRewards(SeasonTemplate season)
	{
		int endLevel = QuestWideData.GetEndLevel(season.Prerequisites, season.Index);
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		List<SeasonTemplate.SeasonEndRewards> availableEndRewards = SeasonWideData.Get().GetAvailableEndRewards(season);
		for (int i = 0; i < availableEndRewards.Count; i++)
		{
			for (int j = 0; j < availableEndRewards[i].CurrencyRewards.Count; j++)
			{
				QuestCurrencyReward questCurrencyReward = availableEndRewards[i].CurrencyRewards[j];
				RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
				rewardData.Amount = questCurrencyReward.Amount;
				rewardData.Level = endLevel;
				rewardData.SpritePath = RewardUtils.GetCurrencyIconPath(questCurrencyReward.Type, out rewardData.Type);
				rewardData.Name = RewardUtils.GetDisplayString(rewardData, false);
				list.Add(rewardData);
			}
			for (int k = 0; k < availableEndRewards[i].ItemRewards.Count; k++)
			{
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(availableEndRewards[i].ItemRewards[k].ItemTemplateId);
				list.Add(new RewardUtils.RewardData
				{
					Amount = 1,
					Name = itemTemplate.GetDisplayName(),
					Level = endLevel,
					SpritePath = InventoryWideData.GetSpritePath(itemTemplate),
					Foreground = InventoryWideData.GetItemFg(itemTemplate),
					InventoryTemplate = itemTemplate,
					Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate)
				});
			}
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

	public static string GetTypeDisplayString(RewardUtils.RewardType type, bool isPlural)
	{
		switch (type)
		{
		case RewardUtils.RewardType.FreelancerCurrency:
			return StringUtil.TR("FreelancerCurrency", "Global");
		case RewardUtils.RewardType.ModToken:
			return (!isPlural) ? StringUtil.TR("ModToken", "Rewards") : StringUtil.TR("ModTokens", "Rewards");
		case RewardUtils.RewardType.GGBoost:
		{
			string result;
			if (isPlural)
			{
				result = StringUtil.TR("GGBoosts", "Rewards");
			}
			else
			{
				result = StringUtil.TR("GGBoost", "Rewards");
			}
			return result;
		}
		case RewardUtils.RewardType.ISO:
			return StringUtil.TR("ISO", "Rewards");
		case RewardUtils.RewardType.Style:
			return (!isPlural) ? StringUtil.TR("Skin", "Rewards") : StringUtil.TR("Skins", "Rewards");
		case RewardUtils.RewardType.System:
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
		case RewardUtils.RewardType.Lockbox:
		{
			string result3;
			if (isPlural)
			{
				result3 = StringUtil.TR("LootMatrices", "Rewards");
			}
			else
			{
				result3 = StringUtil.TR("LootMatrix", "Rewards");
			}
			return result3;
		}
		case RewardUtils.RewardType.AbilityVfxSwap:
		{
			string result4;
			if (isPlural)
			{
				result4 = StringUtil.TR("AbilityEffects", "Rewards");
			}
			else
			{
				result4 = StringUtil.TR("AbilityEffect", "Rewards");
			}
			return result4;
		}
		case RewardUtils.RewardType.Overcon:
			return StringUtil.TR("Overcon", "Inventory");
		case RewardUtils.RewardType.Faction:
			return StringUtil.TR("Faction", "Inventory");
		case RewardUtils.RewardType.LoadingScreenBackground:
			return StringUtil.TR("LoadingScreenBackground", "Inventory");
		case RewardUtils.RewardType.FreelancerExpBonus:
			return StringUtil.TR("FreelancerExpBonus", "Global");
		}
		string text = type.ToString();
		string str = text;
		string str2;
		if (isPlural)
		{
			str2 = "s";
		}
		else
		{
			str2 = string.Empty;
		}
		string term = str + str2;
		return StringUtil.TR(term, "Rewards");
	}

	public static string GetDisplayString(RewardUtils.RewardData reward, bool ignoreType = false)
	{
		RewardUtils.RewardType type = reward.Type;
		switch (type)
		{
		case RewardUtils.RewardType.FreelancerCurrency:
		case RewardUtils.RewardType.ModToken:
		case RewardUtils.RewardType.GGBoost:
		case RewardUtils.RewardType.ISO:
			break;
		default:
			if (type == RewardUtils.RewardType.System)
			{
				return reward.Name;
			}
			if (type != RewardUtils.RewardType.UnlockFreelancerToken)
			{
				if (ignoreType)
				{
					return reward.Name;
				}
				return RewardUtils.GetTypeDisplayString(reward.Type, reward.Amount > 1) + ": " + reward.Name;
			}
			break;
		}
		return reward.Amount + " " + RewardUtils.GetTypeDisplayString(reward.Type, reward.Amount > 1);
	}

	public static string GetCurrencyIconPath(CurrencyType currencyType, out RewardUtils.RewardType rewardType)
	{
		switch (currencyType)
		{
		case CurrencyType.ISO:
			rewardType = RewardUtils.RewardType.ISO;
			return "QuestRewards/iso_01";
		case CurrencyType.ModToken:
			rewardType = RewardUtils.RewardType.ModToken;
			return "QuestRewards/modToken";
		case CurrencyType.GGPack:
			rewardType = RewardUtils.RewardType.GGBoost;
			return "QuestRewards/ggPack_01";
		case CurrencyType.Experience:
			rewardType = RewardUtils.RewardType.System;
			return "Localization/" + StringUtil.TR("EXP", "TEXTURE");
		case CurrencyType.RankedCurrency:
			rewardType = RewardUtils.RewardType.RankedCurrency;
			return "QuestRewards/rankedCurrency";
		case CurrencyType.FreelancerCurrency:
			rewardType = RewardUtils.RewardType.FreelancerCurrency;
			return "QuestRewards/freelancerCurrency_01";
		case CurrencyType.UnlockFreelancerToken:
			rewardType = RewardUtils.RewardType.UnlockFreelancerToken;
			return "QuestRewards/FreelancerCoin";
		}
		throw new Exception("Invalid currency type: " + currencyType);
	}

	public static string GetCurrencyIconPath(CurrencyType currencyType)
	{
		RewardUtils.RewardType rewardType;
		return RewardUtils.GetCurrencyIconPath(currencyType, out rewardType);
	}

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

		public RewardUtils.RewardType Type;

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

		public TmpRequirementCheckApplicant(IQueueRequirementApplicant baseApplicant, bool isInTutorial)
		{
			this.IsInTutorial = isInTutorial;
			this.m_baseApplicant = baseApplicant;
			this.SeasonLevel = this.m_baseApplicant.SeasonLevel;
			this.InitialSeasonLevel = this.SeasonLevel;
		}

		public int CharacterMatches
		{
			get
			{
				return this.m_baseApplicant.CharacterMatches;
			}
		}

		public int VsHumanMatches
		{
			get
			{
				return this.m_baseApplicant.VsHumanMatches;
			}
		}

		public ClientAccessLevel AccessLevel
		{
			get
			{
				return this.m_baseApplicant.AccessLevel;
			}
		}

		public int SeasonLevel { get; set; }

		public CharacterType CharacterType
		{
			get
			{
				return this.m_baseApplicant.CharacterType;
			}
		}

		public LocalizationArg_Handle LocalizedHandle
		{
			get
			{
				return this.m_baseApplicant.LocalizedHandle;
			}
		}

		public float GameLeavingPoints
		{
			get
			{
				return this.m_baseApplicant.GameLeavingPoints;
			}
		}

		public IEnumerable<CharacterType> AvailableCharacters
		{
			get
			{
				return this.m_baseApplicant.AvailableCharacters;
			}
		}

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			return this.m_baseApplicant.IsCharacterTypeAvailable(ct);
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			int num = this.m_baseApplicant.GetReactorLevel(seasons);
			if (!this.IsInTutorial)
			{
				num += this.SeasonLevel - this.InitialSeasonLevel;
			}
			return num;
		}

		public int TotalMatches
		{
			get
			{
				if (this.IsInTutorial)
				{
					return this.SeasonLevel - 1;
				}
				return this.m_baseApplicant.TotalMatches;
			}
		}
	}
}
