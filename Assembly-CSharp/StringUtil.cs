using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using I2.Loc;

public static class StringUtil
{
	private const uint TWN_DEFAULT_FNV32_HASH_PRIME = 0x1000193U;

	private const uint TWN_DEFAULT_FNV32_HASH_BASIS = 0x811C9DC5U;

	private static Regex m_loadoutNameRegex = new Regex("^Loadout #\\d+");

	internal static uint CaseInsensitiveHash(string name)
	{
		return StringUtil.CaseInsensitiveHash(name, 0x1000193U, 0x811C9DC5U);
	}

	private static uint CaseInsensitiveHash(string name, uint prime, uint basis)
	{
		uint num = basis;
		if (!string.IsNullOrEmpty(name))
		{
			for (int i = 0; i < name.Length; i++)
			{
				num = (num * prime ^ Convert.ToUInt32(char.ToLower(name[i])));
			}
		}
		return num;
	}

	public static string GetTimeDifferenceText(TimeSpan difference, bool full = false)
	{
		string text = string.Empty;
		int num = 0;
		int num2 = (int)difference.TotalDays;
		int num3 = num2 / 0x16D;
		if (num3 > 0)
		{
			if (num3 > 1)
			{
				text += string.Format(StringUtil.TR("Years", "TimeSpan"), num3);
			}
			else
			{
				text += StringUtil.TR("Year", "TimeSpan");
			}
			num++;
			difference -= TimeSpan.FromDays((double)(num3 * 0x16D));
			if (!full)
			{
				return text;
			}
		}
		int days = difference.Days;
		if (days > 0)
		{
			if (!text.IsNullOrEmpty())
			{
				text += " ";
			}
			if (days > 1)
			{
				text += string.Format(StringUtil.TR("Days", "TimeSpan"), days);
			}
			else
			{
				text += StringUtil.TR("Day", "TimeSpan");
			}
			num++;
			if (full)
			{
				if (num <= 1)
				{
					goto IL_14D;
				}
			}
			return text;
		}
		IL_14D:
		int hours = difference.Hours;
		if (hours > 0)
		{
			if (!text.IsNullOrEmpty())
			{
				text += " ";
			}
			if (hours > 1)
			{
				text += string.Format(StringUtil.TR("Hours", "TimeSpan"), hours);
			}
			else
			{
				text += StringUtil.TR("Hour", "TimeSpan");
			}
			num++;
			if (!full || num > 1)
			{
				return text;
			}
		}
		int minutes = difference.Minutes;
		if (minutes > 0)
		{
			if (!text.IsNullOrEmpty())
			{
				text += " ";
			}
			if (minutes > 1)
			{
				text += string.Format(StringUtil.TR("Minutes", "TimeSpan"), minutes);
			}
			else
			{
				text += StringUtil.TR("Minute", "TimeSpan");
			}
			num++;
			if (full)
			{
				if (num <= 1)
				{
					goto IL_275;
				}
			}
			return text;
		}
		IL_275:
		int seconds = difference.Seconds;
		if (!text.IsNullOrEmpty())
		{
			text += " ";
		}
		if (seconds > 1)
		{
			text += string.Format(StringUtil.TR("Seconds", "TimeSpan"), seconds);
		}
		else if (seconds == 1)
		{
			text += StringUtil.TR("Second", "TimeSpan");
		}
		return text;
	}

	public static string GetTimeDifferenceTextAbbreviated(TimeSpan difference)
	{
		int num = (int)difference.TotalDays;
		float num2 = (float)num / 365f;
		if (num2 > 1f)
		{
			return string.Format(StringUtil.TR("Years", "TimeSpan"), (int)num2);
		}
		float num3 = (float)(difference.TotalHours / 24.0);
		if (num3 >= 1.1f)
		{
			return string.Format(StringUtil.TR("Days", "TimeSpan"), (int)num3);
		}
		float num4 = (float)(difference.TotalMinutes / 60.0);
		if (num4 >= 1.1f)
		{
			return string.Format(StringUtil.TR("NumHrs", "Global"), (int)num4);
		}
		float num5 = (float)(difference.TotalSeconds / 60.0);
		if (num5 >= 1.1f)
		{
			return string.Format(StringUtil.TR("Minutes", "TimeSpan"), (int)num5);
		}
		float num6 = (float)(difference.TotalMilliseconds / 1000.0);
		if (num6 >= 1.1f)
		{
			return string.Format(StringUtil.TR("SecondsTimer", "Global"), (int)num6);
		}
		return StringUtil.TR("Second", "TimeSpan");
	}

	public static string GetCurrentLanguagecode()
	{
		string text = LocalizationManager.CurrentLanguageCode;
		if (text.Equals("zh", StringComparison.OrdinalIgnoreCase))
		{
			text = "zh-CN";
		}
		return text;
	}

	public static string GetLocalizedFloat(float floatNumber, string format = "##,#.##")
	{
		string currentLanguagecode = StringUtil.GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		return floatNumber.ToString(format, cultureInfo.NumberFormat);
	}

	public static string GetLocalizedDouble(double doubleNumber, string format = "##,#.##")
	{
		string currentLanguagecode = StringUtil.GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		return doubleNumber.ToString(format, cultureInfo.NumberFormat);
	}

	public static string PathRelativeTo(string sourcePath, string destinationPath)
	{
		Uri uri = new Uri(sourcePath);
		Uri uri2 = new Uri(destinationPath);
		Uri uri3 = uri.MakeRelativeUri(uri2);
		return uri3.OriginalString;
	}

	public static string PathChangeExtension(string path, string extension)
	{
		return string.Format("{0}/{1}{2}", Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), extension);
	}

	public static string RemoveOptionalSuffix(string path, string extension)
	{
		if (path.EndsWith(extension))
		{
			return path.Substring(0, path.Length - extension.Length);
		}
		return path;
	}

	public static bool IsHexString(string hex)
	{
		int num;
		return int.TryParse(hex, NumberStyles.HexNumber, null, out num);
	}

	public static string TR(string term, string context)
	{
		string text = string.Empty;
		string text2 = term;
		if (!context.IsNullOrEmpty())
		{
			text2 = text2 + "@" + context;
		}
		text = ScriptLocalization.Get(text2);
		if (text.IsNullOrEmpty())
		{
			text = string.Format("[{0}]#NotLocalized", text2);
		}
		return text;
	}

	public static string TR(string textDescription)
	{
		string[] array = textDescription.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			return StringUtil.TR(array[0], array[1]);
		}
		return string.Format("[{0}]#NotLocalized", textDescription);
	}

	public static string TR_IfHasContext(string textDescription)
	{
		string[] array = textDescription.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			return StringUtil.TR(array[0], array[1]);
		}
		return textDescription;
	}

	public static string TR_GetLoadingHeader(int setIndex, int displayIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingSubtypeHeader(setIndex, displayIndex));
	}

	public static string TR_GetLoadingTooltip(int setIndex, int displayIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingSubtypeTooltip(setIndex, displayIndex));
	}

	public static string TR_QuestName(int questId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestNameKey(questId));
	}

	public static string TR_QuestDescription(int questId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestDescription(questId));
	}

	public static string TR_QuestFlavorText(int questId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestFlavorTextKey(questId));
	}

	public static string TR_QuestLongDescription(int questId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestLongDescriptionKey(questId));
	}

	public static string TR_QuestTypeDisplayName(int questId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestTypeDisplayNameKey(questId));
	}

	public static string TR_QuestObjective(int questId, int objectiveId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetQuestObjectiveKey(questId, objectiveId));
	}

	public static string TR_InventoryItemName(int invItemId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryItemNameKey(invItemId));
	}

	public static string TR_InventoryItemDescription(int invItemId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryItemDescriptionKey(invItemId));
	}

	public static string TR_InventoryObtainedDescription(int invItemId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryObtainedDescriptionKey(invItemId));
	}

	public static string TR_InventoryFlavorText(int invItemId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryFlavorTextKey(invItemId));
	}

	public static string TR_LootTableName(int lootId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLootTableNameKey(lootId));
	}

	public static string TR_LootTableDescription(int lootId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLootTableDescriptionKey(lootId));
	}

	public static string TR_KarmaName(int karmaId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetKarmaNameKey(karmaId));
	}

	public static string TR_KarmaDescription(int karmaId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetKarmaDescriptionKey(karmaId));
	}

	public static string TR_LoreTitle(int loreId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoreTitleKey(loreId));
	}

	public static string TR_LoreArticleText(int loreId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoreArticleTextKey(loreId));
	}

	public static string TR_SeasonName(int seasonId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonNameKey(seasonId));
	}

	public static string TR_SeasonSubTitle(int seasonId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonSubTitleKey(seasonId));
	}

	public static string TR_SeasonEndHeader(int seasonId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonEndHeaderKey(seasonId));
	}

	public static string TR_SeasonChapterName(int seasonId, int chapterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonChapterNameKey(seasonId, chapterId));
	}

	public static string TR_SeasonChapterUnlock(int seasonId, int chapterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonChapterUnlockKey(seasonId, chapterId));
	}

	public static string TR_SeasonStorytimeHeader(int seasonId, int chapterId, int storyId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeHeaderKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeLongBody(int seasonId, int chapterId, int storyId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeLongBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeBody(int seasonId, int chapterId, int storyId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_PlayerTitle(int titleId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerTitleKey(titleId));
	}

	public static string TR_PlayerTitleUnlockCondition(int titleId, int unlockConditionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerTitleUnlockConditionKey(titleId, unlockConditionId));
	}

	public static string TR_BannerName(int bannerId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerBannerKey(bannerId));
	}

	public static string TR_BannerUnlockCondition(int bannerId, int unlockConditionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerBannerUnlockConditionKey(bannerId, unlockConditionId));
	}

	public static string TR_BannerObtainedDescription(int bannerId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBannerObtainedDescriptionKey(bannerId));
	}

	public static string TR_TitleObtainedDescription(int titleId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetTitleObtainedDescriptionKey(titleId));
	}

	public static string TR_RibbonName(int ribbonId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerRibbonKey(ribbonId));
	}

	public static string TR_RibbonObtainedDescription(int ribbonId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetRibbonObtainedDescriptionKey(ribbonId));
	}

	public static string TR_EmojiName(int emojiId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiNameKey(emojiId));
	}

	public static string TR_EmojiObtainedDescription(int emojiId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiObtainedDescriptionKey(emojiId));
	}

	public static string TR_EmojiPurchaseDescription(int emojiId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiPurchaseDescriptionKey(emojiId));
	}

	public static string TR_EmojiTag(int emojiId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiTagKey(emojiId));
	}

	public static string TR_EmojiUnlock(int emojiId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiUnlockKey(emojiId));
	}

	public static string TR_MapName(string mapId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetMapNameKey(mapId));
	}

	public static string TR_GetMatrixPackEventText(int matrixId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetMatrixPackEventTextKey(matrixId));
	}

	public static string TR_GetMatrixPackDescription(int matrixId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetMatrixPackDescriptionKey(matrixId));
	}

	public static string TR_GamePackEditionName(int packId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetGamePackNameKey(packId));
	}

	public static string TR_GamePackDescription(int packId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetGamePackDescKey(packId));
	}

	public static string TR_CharacterName(string characterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterNameKey(characterId));
	}

	public static string TR_CharacterSelectTooltip(string characterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSelectTooltipKey(characterId));
	}

	public static string TR_CharacterSelectAboutDesc(string characterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSelectAboutKey(characterId));
	}

	public static string TR_CharacterBio(string characterId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterBioKey(characterId));
	}

	public static string TR_CharacterSkinName(string characterId, int skinId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinNameKey(characterId, skinId));
	}

	public static string TR_CharacterSkinDescription(string characterId, int skinId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinDescriptionKey(characterId, skinId));
	}

	public static string TR_CharacterSkinFlavor(string characterId, int skinId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinFlavorKey(characterId, skinId));
	}

	public static string TR_CharacterPatternName(string characterId, int skinId, int patternId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterPatternNameKey(characterId, skinId, patternId));
	}

	public static string TR_CharacterPatternColorName(string characterId, int skinId, int patternId, int colorId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorNameKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorDescription(string characterId, int skinId, int patternId, int colorId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorFlavor(string characterId, int skinId, int patternId, int colorId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorFlavorKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorObtainedDescription(string characterId, int skinId, int patternId, int colorId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorObtainedDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorPurchaseDescription(string characterId, int skinId, int patternId, int colorId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorPurchaseDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterTauntName(string characterId, int tauntId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntNameKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntObtainedDescription(string characterId, int tauntId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntObtainedDescKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntPurchaseDescription(string characterId, int tauntId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntPurchaseDescKey(characterId, tauntId));
	}

	public static string TR_GetCharacterVFXSwapName(string characterId, int abilityId, int vfxSwapId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapNameKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapObtainedDescription(string characterId, int abilityId, int vfxSwapId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapObtainedDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapPurchaseDescription(string characterId, int abilityId, int vfxSwapId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapPurchaseDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_LoadingScreenTip(int tipId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenTipKey(tipId));
	}

	public static string TR_KeyBindCommand(string keyBindId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetKeyBindCommandNameKey(keyBindId));
	}

	public static string TR_CardDisplayName(CardType m_cardType)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetCardDisplayNameKey(m_cardType));
	}

	public static string TR_AbilityName(string abilityType, string abilityId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityNameKey(abilityType, abilityId));
	}

	public static string TR_AbilityFinalFullTooltip(string abilityType, string abilityId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityFinalFullTooltipKey(abilityType, abilityId));
	}

	public static string TR_AbilityReward(string abilityType, string abilityId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityRewardKey(abilityType, abilityId));
	}

	public static string TR_AbilityModName(string abilityType, string modId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityModNameKey(abilityType, modId));
	}

	public static string TR_AbilityModFinalTooltip(string abilityType, string modId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityModFinalTooltipKey(abilityType, modId));
	}

	public static string TR_FactionGroupName(int factionGroupId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFactionGroupNameKey(factionGroupId));
	}

	public static string TR_FactionName(int factionCompletionId, int factionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFactionNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLongName(int factionCompletionId, int factionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFactionLongNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLoreDescription(int factionCompletionId, int factionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFactionLoreDescriptionKey(factionCompletionId, factionId));
	}

	public static string GetStatusIconPopupText(int statusIconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconPopupTextKey(statusIconId));
	}

	public static string GetStatusIconBuffName(int statusIconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconBuffNameKey(statusIconId));
	}

	public static string GetStatusIconBuffDesc(int statusIconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconBuffDescKey(statusIconId));
	}

	public static string GetSpectatorToggleOptionName(UISpectatorHUD.SpectatorToggleOption option)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetSpectatorToggleOptionKey(option));
	}

	public static string TR_GetOverconDisplayName(int overconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetOverconNameKey(overconId));
	}

	public static string TR_GetOverconCommandName(int overconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetOverconCommandKey(overconId));
	}

	public static string TR_GetOverconObtainedDesc(int overconId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetOverconObtainedDescKey(overconId));
	}

	public static string TR_GetOverconUnlockCondition(int overconId, int unlockConditionId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetOverconUnlockConditionKey(overconId, unlockConditionId));
	}

	public static string TR_GetLoadingScreenBackgroundName(int loadingScreenBgId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundNameKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundObtainedDescription(int loadingScreenBgId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundObtainedDescriptionKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundPurchaseDescription(int loadingScreenBgId)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundPurchaseDescriptionKey(loadingScreenBgId));
	}

	public static string TR_GetLoadoutName(string loadoutName)
	{
		string result = loadoutName;
		Match match = StringUtil.m_loadoutNameRegex.Match(loadoutName);
		if (match.Success)
		{
			Regex regex = new Regex("\\d+");
			Match match2 = regex.Match(match.Value);
			if (match2.Success)
			{
				int num = int.Parse(match2.Value);
				result = string.Format(StringUtil.TR("LoadoutNumber", "Global"), num);
			}
		}
		return result;
	}

	public static string TR_StatDescription(StatDisplaySettings.StatType statType)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetStatDescriptionKey(statType));
	}

	public static string TR_StatName(StatDisplaySettings.StatType statType)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetStatNameKey(statType));
	}

	public static string TR_BadgeGroupName(int groupIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupName(groupIndex));
	}

	public static string TR_BadgeGroupDescription(int groupIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupDescription(groupIndex));
	}

	public static string TR_BadgeDescription(int badgeID)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeDescriptionKey(badgeID));
	}

	public static string TR_BadgeGroupRequirementDescriptionKey(int badgeID)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupRequirementDescriptionKey(badgeID));
	}

	public static string TR_BadgeName(int badgeID)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeNameKey(badgeID));
	}

	public static string TR_FreelancerStatDescription(string freelancerType, int statIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFreelancerStatDescriptionKey(freelancerType, statIndex));
	}

	public static string TR_FreelancerStatName(string freelancerType, int statIndex)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetFreelancerStatNameKey(freelancerType, statIndex));
	}

	public static string TR_PersistedStatBucketName(PersistedStatBucket bucket)
	{
		string empty = string.Empty;
		return ScriptLocalization.Get(ScriptLocalization.GetPersistedStatBucketNameKey(bucket));
	}

	public static string FormatTime(int seconds)
	{
		return string.Format("{0}:{1:00}", seconds / 0x3C, seconds % 0x3C);
	}
}
