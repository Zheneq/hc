using I2.Loc;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

public static class StringUtil
{
	private const uint TWN_DEFAULT_FNV32_HASH_PRIME = 16777619u;
	private const uint TWN_DEFAULT_FNV32_HASH_BASIS = 2166136261u;

	private static Regex m_loadoutNameRegex = new Regex("^Loadout #\\d+");

	internal static uint CaseInsensitiveHash(string name)
	{
		return CaseInsensitiveHash(name, 16777619u, 2166136261u);
	}

	private static uint CaseInsensitiveHash(string name, uint prime, uint basis)
	{
		uint num = basis;
		if (!string.IsNullOrEmpty(name))
		{
			foreach (char c in name)
			{
				num = (num * prime) ^ Convert.ToUInt32(char.ToLower(c));
			}
		}
		return num;
	}

	public static string GetTimeDifferenceText(TimeSpan difference, bool full = false)
	{
		string text = string.Empty;
		int elements = 0;
		int years = (int)difference.TotalDays / 365;
		if (years > 0)
		{
			text = years <= 1
				? text + TR("Year", "TimeSpan")
				: text + string.Format(TR("Years", "TimeSpan"), years);
			elements++;
			difference -= TimeSpan.FromDays(years * 365);
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
				text += string.Format(TR("Days", "TimeSpan"), days);
			}
			else
			{
				text += TR("Day", "TimeSpan");
			}
			elements++;
			if (!full || elements > 1)
			{
				return text;
			}
		}
		int hours = difference.Hours;
		if (hours > 0)
		{
			if (!text.IsNullOrEmpty())
			{
				text += " ";
			}
			if (hours > 1)
			{
				text += string.Format(TR("Hours", "TimeSpan"), hours);
			}
			else
			{
				text += TR("Hour", "TimeSpan");
			}
			elements++;
			if (!full || elements > 1)
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
				text += string.Format(TR("Minutes", "TimeSpan"), minutes);
			}
			else
			{
				text += TR("Minute", "TimeSpan");
			}
			elements++;
			if (!full || elements > 1)
			{
				return text;
			}
		}
		int seconds = difference.Seconds;
		if (!text.IsNullOrEmpty())
		{
			text += " ";
		}
		if (seconds > 1)
		{
			text += string.Format(TR("Seconds", "TimeSpan"), seconds);
		}
		else if (seconds == 1)
		{
			text += TR("Second", "TimeSpan");
		}
		return text;
	}

	public static string GetTimeDifferenceTextAbbreviated(TimeSpan difference)
	{
		float years = (int)difference.TotalDays / 365f;
		if (years > 1f)
		{
			return string.Format(TR("Years", "TimeSpan"), (int)years);
		}
		float days = (float)(difference.TotalHours / 24.0);
		if (days >= 1.1f)
		{
			return string.Format(TR("Days", "TimeSpan"), (int)days);
		}
		float hours = (float)(difference.TotalMinutes / 60.0);
		if (hours >= 1.1f)
		{
			return string.Format(TR("NumHrs", "Global"), (int)hours);
		}
		float minutes = (float)(difference.TotalSeconds / 60.0);
		if (minutes >= 1.1f)
		{
			return string.Format(TR("Minutes", "TimeSpan"), (int)minutes);
		}
		float seconds = (float)(difference.TotalMilliseconds / 1000.0);
		if (seconds >= 1.1f)
		{
			return string.Format(TR("SecondsTimer", "Global"), (int)seconds);
		}
		return TR("Second", "TimeSpan");
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
		string currentLanguagecode = GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		return floatNumber.ToString(format, cultureInfo.NumberFormat);
	}

	public static string GetLocalizedDouble(double doubleNumber, string format = "##,#.##")
	{
		string currentLanguagecode = GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		return doubleNumber.ToString(format, cultureInfo.NumberFormat);
	}

	public static string PathRelativeTo(string sourcePath, string destinationPath)
	{
		Uri srcUri = new Uri(sourcePath);
		Uri dstUri = new Uri(destinationPath);
		return srcUri.MakeRelativeUri(dstUri).OriginalString;
	}

	public static string PathChangeExtension(string path, string extension)
	{
		return $"{Path.GetDirectoryName(path)}/{Path.GetFileNameWithoutExtension(path)}{extension}";
	}

	public static string RemoveOptionalSuffix(string path, string extension)
	{
		return path.EndsWith(extension)
			? path.Substring(0, path.Length - extension.Length)
			: path;
	}

	public static bool IsHexString(string hex)
	{
		return int.TryParse(hex, NumberStyles.HexNumber, null, out _);
	}

	public static string TR(string term, string context)
	{
		string text = term;
		if (!context.IsNullOrEmpty())
		{
			text = text + "@" + context;
		}
		string empty = ScriptLocalization.Get(text);
		if (empty.IsNullOrEmpty())
		{
			empty = $"[{text}]#NotLocalized";
		}
		return empty;
	}

	public static string TR(string textDescription)
	{
		string[] array = textDescription.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			return TR(array[0], array[1]);
		}
		return $"[{textDescription}]#NotLocalized";
	}

	public static string TR_IfHasContext(string textDescription)
	{
		string[] array = textDescription.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			return TR(array[0], array[1]);
		}
		return textDescription;
	}

	public static string TR_GetLoadingHeader(int setIndex, int displayIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingSubtypeHeader(setIndex, displayIndex));
	}

	public static string TR_GetLoadingTooltip(int setIndex, int displayIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingSubtypeTooltip(setIndex, displayIndex));
	}

	public static string TR_QuestName(int questId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestNameKey(questId));
	}

	public static string TR_QuestDescription(int questId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestDescription(questId));
	}

	public static string TR_QuestFlavorText(int questId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestFlavorTextKey(questId));
	}

	public static string TR_QuestLongDescription(int questId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestLongDescriptionKey(questId));
	}

	public static string TR_QuestTypeDisplayName(int questId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestTypeDisplayNameKey(questId));
	}

	public static string TR_QuestObjective(int questId, int objectiveId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetQuestObjectiveKey(questId, objectiveId));
	}

	public static string TR_InventoryItemName(int invItemId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryItemNameKey(invItemId));
	}

	public static string TR_InventoryItemDescription(int invItemId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryItemDescriptionKey(invItemId));
	}

	public static string TR_InventoryObtainedDescription(int invItemId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryObtainedDescriptionKey(invItemId));
	}

	public static string TR_InventoryFlavorText(int invItemId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetInventoryFlavorTextKey(invItemId));
	}

	public static string TR_LootTableName(int lootId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLootTableNameKey(lootId));
	}

	public static string TR_LootTableDescription(int lootId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLootTableDescriptionKey(lootId));
	}

	public static string TR_KarmaName(int karmaId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetKarmaNameKey(karmaId));
	}

	public static string TR_KarmaDescription(int karmaId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetKarmaDescriptionKey(karmaId));
	}

	public static string TR_LoreTitle(int loreId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoreTitleKey(loreId));
	}

	public static string TR_LoreArticleText(int loreId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoreArticleTextKey(loreId));
	}

	public static string TR_SeasonName(int seasonId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonNameKey(seasonId));
	}

	public static string TR_SeasonSubTitle(int seasonId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonSubTitleKey(seasonId));
	}

	public static string TR_SeasonEndHeader(int seasonId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonEndHeaderKey(seasonId));
	}

	public static string TR_SeasonChapterName(int seasonId, int chapterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonChapterNameKey(seasonId, chapterId));
	}

	public static string TR_SeasonChapterUnlock(int seasonId, int chapterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonChapterUnlockKey(seasonId, chapterId));
	}

	public static string TR_SeasonStorytimeHeader(int seasonId, int chapterId, int storyId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeHeaderKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeLongBody(int seasonId, int chapterId, int storyId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeLongBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeBody(int seasonId, int chapterId, int storyId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSeasonStorytimeBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_PlayerTitle(int titleId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerTitleKey(titleId));
	}

	public static string TR_PlayerTitleUnlockCondition(int titleId, int unlockConditionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerTitleUnlockConditionKey(titleId, unlockConditionId));
	}

	public static string TR_BannerName(int bannerId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerBannerKey(bannerId));
	}

	public static string TR_BannerUnlockCondition(int bannerId, int unlockConditionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerBannerUnlockConditionKey(bannerId, unlockConditionId));
	}

	public static string TR_BannerObtainedDescription(int bannerId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBannerObtainedDescriptionKey(bannerId));
	}

	public static string TR_TitleObtainedDescription(int titleId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetTitleObtainedDescriptionKey(titleId));
	}

	public static string TR_RibbonName(int ribbonId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPlayerRibbonKey(ribbonId));
	}

	public static string TR_RibbonObtainedDescription(int ribbonId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetRibbonObtainedDescriptionKey(ribbonId));
	}

	public static string TR_EmojiName(int emojiId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiNameKey(emojiId));
	}

	public static string TR_EmojiObtainedDescription(int emojiId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiObtainedDescriptionKey(emojiId));
	}

	public static string TR_EmojiPurchaseDescription(int emojiId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiPurchaseDescriptionKey(emojiId));
	}

	public static string TR_EmojiTag(int emojiId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiTagKey(emojiId));
	}

	public static string TR_EmojiUnlock(int emojiId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetEmojiUnlockKey(emojiId));
	}

	public static string TR_MapName(string mapId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetMapNameKey(mapId));
	}

	public static string TR_GetMatrixPackEventText(int matrixId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetMatrixPackEventTextKey(matrixId));
	}

	public static string TR_GetMatrixPackDescription(int matrixId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetMatrixPackDescriptionKey(matrixId));
	}

	public static string TR_GamePackEditionName(int packId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetGamePackNameKey(packId));
	}

	public static string TR_GamePackDescription(int packId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetGamePackDescKey(packId));
	}

	public static string TR_CharacterName(string characterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterNameKey(characterId));
	}

	public static string TR_CharacterSelectTooltip(string characterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSelectTooltipKey(characterId));
	}

	public static string TR_CharacterSelectAboutDesc(string characterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSelectAboutKey(characterId));
	}

	public static string TR_CharacterBio(string characterId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterBioKey(characterId));
	}

	public static string TR_CharacterSkinName(string characterId, int skinId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinNameKey(characterId, skinId));
	}

	public static string TR_CharacterSkinDescription(string characterId, int skinId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinDescriptionKey(characterId, skinId));
	}

	public static string TR_CharacterSkinFlavor(string characterId, int skinId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterSkinFlavorKey(characterId, skinId));
	}

	public static string TR_CharacterPatternName(string characterId, int skinId, int patternId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterPatternNameKey(characterId, skinId, patternId));
	}

	public static string TR_CharacterPatternColorName(string characterId, int skinId, int patternId, int colorId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorNameKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorFlavor(string characterId, int skinId, int patternId, int colorId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorFlavorKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorObtainedDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorObtainedDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorPurchaseDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPatternColorPurchaseDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterTauntName(string characterId, int tauntId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntNameKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntObtainedDescription(string characterId, int tauntId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntObtainedDescKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntPurchaseDescription(string characterId, int tauntId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterTauntPurchaseDescKey(characterId, tauntId));
	}

	public static string TR_GetCharacterVFXSwapName(string characterId, int abilityId, int vfxSwapId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapNameKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapObtainedDescription(string characterId, int abilityId, int vfxSwapId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapObtainedDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapPurchaseDescription(string characterId, int abilityId, int vfxSwapId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCharacterVFXSwapPurchaseDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_LoadingScreenTip(int tipId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenTipKey(tipId));
	}

	public static string TR_KeyBindCommand(string keyBindId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetKeyBindCommandNameKey(keyBindId));
	}

	public static string TR_CardDisplayName(CardType m_cardType)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetCardDisplayNameKey(m_cardType));
	}

	public static string TR_AbilityName(string abilityType, string abilityId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityNameKey(abilityType, abilityId));
	}

	public static string TR_AbilityFinalFullTooltip(string abilityType, string abilityId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityFinalFullTooltipKey(abilityType, abilityId));
	}

	public static string TR_AbilityReward(string abilityType, string abilityId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityRewardKey(abilityType, abilityId));
	}

	public static string TR_AbilityModName(string abilityType, string modId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityModNameKey(abilityType, modId));
	}

	public static string TR_AbilityModFinalTooltip(string abilityType, string modId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetAbilityModFinalTooltipKey(abilityType, modId));
	}

	public static string TR_FactionGroupName(int factionGroupId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFactionGroupNameKey(factionGroupId));
	}

	public static string TR_FactionName(int factionCompletionId, int factionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFactionNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLongName(int factionCompletionId, int factionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFactionLongNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLoreDescription(int factionCompletionId, int factionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFactionLoreDescriptionKey(factionCompletionId, factionId));
	}

	public static string GetStatusIconPopupText(int statusIconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconPopupTextKey(statusIconId));
	}

	public static string GetStatusIconBuffName(int statusIconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconBuffNameKey(statusIconId));
	}

	public static string GetStatusIconBuffDesc(int statusIconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetStatusIconBuffDescKey(statusIconId));
	}

	public static string GetSpectatorToggleOptionName(UISpectatorHUD.SpectatorToggleOption option)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetSpectatorToggleOptionKey(option));
	}

	public static string TR_GetOverconDisplayName(int overconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetOverconNameKey(overconId));
	}

	public static string TR_GetOverconCommandName(int overconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetOverconCommandKey(overconId));
	}

	public static string TR_GetOverconObtainedDesc(int overconId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetOverconObtainedDescKey(overconId));
	}

	public static string TR_GetOverconUnlockCondition(int overconId, int unlockConditionId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetOverconUnlockConditionKey(overconId, unlockConditionId));
	}

	public static string TR_GetLoadingScreenBackgroundName(int loadingScreenBgId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundNameKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundObtainedDescription(int loadingScreenBgId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundObtainedDescriptionKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundPurchaseDescription(int loadingScreenBgId)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetLoadingScreenBackgroundPurchaseDescriptionKey(loadingScreenBgId));
	}

	public static string TR_GetLoadoutName(string loadoutName)
	{
		string result = loadoutName;
		Match match = m_loadoutNameRegex.Match(loadoutName);
		if (match.Success)
		{
			Regex regex = new Regex("\\d+");
			Match match2 = regex.Match(match.Value);
			if (match2.Success)
			{
				int num = int.Parse(match2.Value);
				result = string.Format(TR("LoadoutNumber", "Global"), num);
			}
		}
		return result;
	}

	public static string TR_StatDescription(StatDisplaySettings.StatType statType)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetStatDescriptionKey(statType));
	}

	public static string TR_StatName(StatDisplaySettings.StatType statType)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetStatNameKey(statType));
	}

	public static string TR_BadgeGroupName(int groupIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupName(groupIndex));
	}

	public static string TR_BadgeGroupDescription(int groupIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupDescription(groupIndex));
	}

	public static string TR_BadgeDescription(int badgeID)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeDescriptionKey(badgeID));
	}

	public static string TR_BadgeGroupRequirementDescriptionKey(int badgeID)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeGroupRequirementDescriptionKey(badgeID));
	}

	public static string TR_BadgeName(int badgeID)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetBadgeNameKey(badgeID));
	}

	public static string TR_FreelancerStatDescription(string freelancerType, int statIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFreelancerStatDescriptionKey(freelancerType, statIndex));
	}

	public static string TR_FreelancerStatName(string freelancerType, int statIndex)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetFreelancerStatNameKey(freelancerType, statIndex));
	}

	public static string TR_PersistedStatBucketName(PersistedStatBucket bucket)
	{
		return ScriptLocalization.Get(ScriptLocalization.GetPersistedStatBucketNameKey(bucket));
	}

	public static string FormatTime(int seconds)
	{
		return $"{seconds / 60}:{seconds % 60:00}";
	}
}
