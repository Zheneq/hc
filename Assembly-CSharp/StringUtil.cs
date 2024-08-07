using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using I2.Loc;
using UnityEngine;

public static class StringUtil
{
#if EVOS
	private static Dictionary<string, Dictionary<string, string>> _locOverrides;

	private static Dictionary<string, Dictionary<string, string>> LocOverrides
	{
		get
		{
			if (_locOverrides == null)
			{
				try
				{
					string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "Evos/loc.json"));
					Debug.Log($"Loading localization patch from {fullPath}");
					string data = File.ReadAllText(fullPath);
					_locOverrides = DefaultJsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(data);
				}
				catch (Exception e)
				{
					Debug.LogError($"Failed to load localization patch: {e}");
					_locOverrides = new Dictionary<string, Dictionary<string, string>>();
				}
			}

			return _locOverrides;
		}
	}
	

#endif
	
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
		string empty = GetLocalization(text);
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
		return GetLocalization(ScriptLocalization.GetLoadingSubtypeHeader(setIndex, displayIndex));
	}

	public static string TR_GetLoadingTooltip(int setIndex, int displayIndex)
	{
		return GetLocalization(ScriptLocalization.GetLoadingSubtypeTooltip(setIndex, displayIndex));
	}

	public static string TR_QuestName(int questId)
	{
		return GetLocalization(ScriptLocalization.GetQuestNameKey(questId));
	}

	public static string TR_QuestDescription(int questId)
	{
		return GetLocalization(ScriptLocalization.GetQuestDescription(questId));
	}

	public static string TR_QuestFlavorText(int questId)
	{
		return GetLocalization(ScriptLocalization.GetQuestFlavorTextKey(questId));
	}

	public static string TR_QuestLongDescription(int questId)
	{
		return GetLocalization(ScriptLocalization.GetQuestLongDescriptionKey(questId));
	}

	public static string TR_QuestTypeDisplayName(int questId)
	{
		return GetLocalization(ScriptLocalization.GetQuestTypeDisplayNameKey(questId));
	}

	public static string TR_QuestObjective(int questId, int objectiveId)
	{
		return GetLocalization(ScriptLocalization.GetQuestObjectiveKey(questId, objectiveId));
	}

	public static string TR_InventoryItemName(int invItemId)
	{
		return GetLocalization(ScriptLocalization.GetInventoryItemNameKey(invItemId));
	}

	public static string TR_InventoryItemDescription(int invItemId)
	{
		return GetLocalization(ScriptLocalization.GetInventoryItemDescriptionKey(invItemId));
	}

	public static string TR_InventoryObtainedDescription(int invItemId)
	{
		return GetLocalization(ScriptLocalization.GetInventoryObtainedDescriptionKey(invItemId));
	}

	public static string TR_InventoryFlavorText(int invItemId)
	{
		return GetLocalization(ScriptLocalization.GetInventoryFlavorTextKey(invItemId));
	}

	public static string TR_LootTableName(int lootId)
	{
		return GetLocalization(ScriptLocalization.GetLootTableNameKey(lootId));
	}

	public static string TR_LootTableDescription(int lootId)
	{
		return GetLocalization(ScriptLocalization.GetLootTableDescriptionKey(lootId));
	}

	public static string TR_KarmaName(int karmaId)
	{
		return GetLocalization(ScriptLocalization.GetKarmaNameKey(karmaId));
	}

	public static string TR_KarmaDescription(int karmaId)
	{
		return GetLocalization(ScriptLocalization.GetKarmaDescriptionKey(karmaId));
	}

	public static string TR_LoreTitle(int loreId)
	{
		return GetLocalization(ScriptLocalization.GetLoreTitleKey(loreId));
	}

	public static string TR_LoreArticleText(int loreId)
	{
		return GetLocalization(ScriptLocalization.GetLoreArticleTextKey(loreId));
	}

	public static string TR_SeasonName(int seasonId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonNameKey(seasonId));
	}

	public static string TR_SeasonSubTitle(int seasonId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonSubTitleKey(seasonId));
	}

	public static string TR_SeasonEndHeader(int seasonId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonEndHeaderKey(seasonId));
	}

	public static string TR_SeasonChapterName(int seasonId, int chapterId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonChapterNameKey(seasonId, chapterId));
	}

	public static string TR_SeasonChapterUnlock(int seasonId, int chapterId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonChapterUnlockKey(seasonId, chapterId));
	}

	public static string TR_SeasonStorytimeHeader(int seasonId, int chapterId, int storyId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonStorytimeHeaderKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeLongBody(int seasonId, int chapterId, int storyId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonStorytimeLongBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_SeasonStorytimeBody(int seasonId, int chapterId, int storyId)
	{
		return GetLocalization(ScriptLocalization.GetSeasonStorytimeBodyKey(seasonId, chapterId, storyId));
	}

	public static string TR_PlayerTitle(int titleId)
	{
		return GetLocalization(ScriptLocalization.GetPlayerTitleKey(titleId));
	}

	public static string TR_PlayerTitleUnlockCondition(int titleId, int unlockConditionId)
	{
		return GetLocalization(ScriptLocalization.GetPlayerTitleUnlockConditionKey(titleId, unlockConditionId));
	}

	public static string TR_BannerName(int bannerId)
	{
		return GetLocalization(ScriptLocalization.GetPlayerBannerKey(bannerId));
	}

	public static string TR_BannerUnlockCondition(int bannerId, int unlockConditionId)
	{
		return GetLocalization(ScriptLocalization.GetPlayerBannerUnlockConditionKey(bannerId, unlockConditionId));
	}

	public static string TR_BannerObtainedDescription(int bannerId)
	{
		return GetLocalization(ScriptLocalization.GetBannerObtainedDescriptionKey(bannerId));
	}

	public static string TR_TitleObtainedDescription(int titleId)
	{
		return GetLocalization(ScriptLocalization.GetTitleObtainedDescriptionKey(titleId));
	}

	public static string TR_RibbonName(int ribbonId)
	{
		return GetLocalization(ScriptLocalization.GetPlayerRibbonKey(ribbonId));
	}

	public static string TR_RibbonObtainedDescription(int ribbonId)
	{
		return GetLocalization(ScriptLocalization.GetRibbonObtainedDescriptionKey(ribbonId));
	}

	public static string TR_EmojiName(int emojiId)
	{
		return GetLocalization(ScriptLocalization.GetEmojiNameKey(emojiId));
	}

	public static string TR_EmojiObtainedDescription(int emojiId)
	{
		return GetLocalization(ScriptLocalization.GetEmojiObtainedDescriptionKey(emojiId));
	}

	public static string TR_EmojiPurchaseDescription(int emojiId)
	{
		return GetLocalization(ScriptLocalization.GetEmojiPurchaseDescriptionKey(emojiId));
	}

	public static string TR_EmojiTag(int emojiId)
	{
		return GetLocalization(ScriptLocalization.GetEmojiTagKey(emojiId));
	}

	public static string TR_EmojiUnlock(int emojiId)
	{
		return GetLocalization(ScriptLocalization.GetEmojiUnlockKey(emojiId));
	}

	public static string TR_MapName(string mapId)
	{
		return GetLocalization(ScriptLocalization.GetMapNameKey(mapId));
	}

	public static string TR_GetMatrixPackEventText(int matrixId)
	{
		return GetLocalization(ScriptLocalization.GetMatrixPackEventTextKey(matrixId));
	}

	public static string TR_GetMatrixPackDescription(int matrixId)
	{
		return GetLocalization(ScriptLocalization.GetMatrixPackDescriptionKey(matrixId));
	}

	public static string TR_GamePackEditionName(int packId)
	{
		return GetLocalization(ScriptLocalization.GetGamePackNameKey(packId));
	}

	public static string TR_GamePackDescription(int packId)
	{
		return GetLocalization(ScriptLocalization.GetGamePackDescKey(packId));
	}

	public static string TR_CharacterName(string characterId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterNameKey(characterId));
	}

	public static string TR_CharacterSelectTooltip(string characterId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterSelectTooltipKey(characterId));
	}

	public static string TR_CharacterSelectAboutDesc(string characterId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterSelectAboutKey(characterId));
	}

	public static string TR_CharacterBio(string characterId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterBioKey(characterId));
	}

	public static string TR_CharacterSkinName(string characterId, int skinId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterSkinNameKey(characterId, skinId));
	}

	public static string TR_CharacterSkinDescription(string characterId, int skinId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterSkinDescriptionKey(characterId, skinId));
	}

	public static string TR_CharacterSkinFlavor(string characterId, int skinId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterSkinFlavorKey(characterId, skinId));
	}

	public static string TR_CharacterPatternName(string characterId, int skinId, int patternId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterPatternNameKey(characterId, skinId, patternId));
	}

	public static string TR_CharacterPatternColorName(string characterId, int skinId, int patternId, int colorId)
	{
		return GetLocalization(ScriptLocalization.GetPatternColorNameKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return GetLocalization(ScriptLocalization.GetPatternColorDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorFlavor(string characterId, int skinId, int patternId, int colorId)
	{
		return GetLocalization(ScriptLocalization.GetPatternColorFlavorKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorObtainedDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return GetLocalization(ScriptLocalization.GetPatternColorObtainedDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterPatternColorPurchaseDescription(string characterId, int skinId, int patternId, int colorId)
	{
		return GetLocalization(ScriptLocalization.GetPatternColorPurchaseDescKey(characterId, skinId, patternId, colorId));
	}

	public static string TR_CharacterTauntName(string characterId, int tauntId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterTauntNameKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntObtainedDescription(string characterId, int tauntId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterTauntObtainedDescKey(characterId, tauntId));
	}

	public static string TR_CharacterTauntPurchaseDescription(string characterId, int tauntId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterTauntPurchaseDescKey(characterId, tauntId));
	}

	public static string TR_GetCharacterVFXSwapName(string characterId, int abilityId, int vfxSwapId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterVFXSwapNameKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapObtainedDescription(string characterId, int abilityId, int vfxSwapId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterVFXSwapObtainedDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_GetCharacterVFXSwapPurchaseDescription(string characterId, int abilityId, int vfxSwapId)
	{
		return GetLocalization(ScriptLocalization.GetCharacterVFXSwapPurchaseDescKey(characterId, abilityId, vfxSwapId));
	}

	public static string TR_LoadingScreenTip(int tipId)
	{
		return GetLocalization(ScriptLocalization.GetLoadingScreenTipKey(tipId));
	}

	public static string TR_KeyBindCommand(string keyBindId)
	{
		return GetLocalization(ScriptLocalization.GetKeyBindCommandNameKey(keyBindId));
	}

	public static string TR_CardDisplayName(CardType m_cardType)
	{
		return GetLocalization(ScriptLocalization.GetCardDisplayNameKey(m_cardType));
	}

	public static string TR_AbilityName(string abilityType, string abilityId)
	{
		return GetLocalization(ScriptLocalization.GetAbilityNameKey(abilityType, abilityId));
	}

	public static string TR_AbilityFinalFullTooltip(string abilityType, string abilityId)
	{
		return GetLocalization(ScriptLocalization.GetAbilityFinalFullTooltipKey(abilityType, abilityId));
	}

	public static string TR_AbilityReward(string abilityType, string abilityId)
	{
		return GetLocalization(ScriptLocalization.GetAbilityRewardKey(abilityType, abilityId));
	}

	public static string TR_AbilityModName(string abilityType, string modId)
	{
		return GetLocalization(ScriptLocalization.GetAbilityModNameKey(abilityType, modId));
	}

	public static string TR_AbilityModFinalTooltip(string abilityType, string modId)
	{
		return GetLocalization(ScriptLocalization.GetAbilityModFinalTooltipKey(abilityType, modId));
	}

	public static string TR_FactionGroupName(int factionGroupId)
	{
		return GetLocalization(ScriptLocalization.GetFactionGroupNameKey(factionGroupId));
	}

	public static string TR_FactionName(int factionCompletionId, int factionId)
	{
		return GetLocalization(ScriptLocalization.GetFactionNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLongName(int factionCompletionId, int factionId)
	{
		return GetLocalization(ScriptLocalization.GetFactionLongNameKey(factionCompletionId, factionId));
	}

	public static string TR_FactionLoreDescription(int factionCompletionId, int factionId)
	{
		return GetLocalization(ScriptLocalization.GetFactionLoreDescriptionKey(factionCompletionId, factionId));
	}

	public static string GetStatusIconPopupText(int statusIconId)
	{
		return GetLocalization(ScriptLocalization.GetStatusIconPopupTextKey(statusIconId));
	}

	public static string GetStatusIconBuffName(int statusIconId)
	{
		return GetLocalization(ScriptLocalization.GetStatusIconBuffNameKey(statusIconId));
	}

	public static string GetStatusIconBuffDesc(int statusIconId)
	{
		return GetLocalization(ScriptLocalization.GetStatusIconBuffDescKey(statusIconId));
	}

	public static string GetSpectatorToggleOptionName(UISpectatorHUD.SpectatorToggleOption option)
	{
		return GetLocalization(ScriptLocalization.GetSpectatorToggleOptionKey(option));
	}

	public static string TR_GetOverconDisplayName(int overconId)
	{
		return GetLocalization(ScriptLocalization.GetOverconNameKey(overconId));
	}

	public static string TR_GetOverconCommandName(int overconId)
	{
		return GetLocalization(ScriptLocalization.GetOverconCommandKey(overconId));
	}

	public static string TR_GetOverconObtainedDesc(int overconId)
	{
		return GetLocalization(ScriptLocalization.GetOverconObtainedDescKey(overconId));
	}

	public static string TR_GetOverconUnlockCondition(int overconId, int unlockConditionId)
	{
		return GetLocalization(ScriptLocalization.GetOverconUnlockConditionKey(overconId, unlockConditionId));
	}

	public static string TR_GetLoadingScreenBackgroundName(int loadingScreenBgId)
	{
		return GetLocalization(ScriptLocalization.GetLoadingScreenBackgroundNameKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundObtainedDescription(int loadingScreenBgId)
	{
		return GetLocalization(ScriptLocalization.GetLoadingScreenBackgroundObtainedDescriptionKey(loadingScreenBgId));
	}

	public static string TR_GetLoadingScreenBackgroundPurchaseDescription(int loadingScreenBgId)
	{
		return GetLocalization(ScriptLocalization.GetLoadingScreenBackgroundPurchaseDescriptionKey(loadingScreenBgId));
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
		return GetLocalization(ScriptLocalization.GetStatDescriptionKey(statType));
	}

	public static string TR_StatName(StatDisplaySettings.StatType statType)
	{
		return GetLocalization(ScriptLocalization.GetStatNameKey(statType));
	}

	public static string TR_BadgeGroupName(int groupIndex)
	{
		return GetLocalization(ScriptLocalization.GetBadgeGroupName(groupIndex));
	}

	public static string TR_BadgeGroupDescription(int groupIndex)
	{
		return GetLocalization(ScriptLocalization.GetBadgeGroupDescription(groupIndex));
	}

	public static string TR_BadgeDescription(int badgeID)
	{
		return GetLocalization(ScriptLocalization.GetBadgeDescriptionKey(badgeID));
	}

	public static string TR_BadgeGroupRequirementDescriptionKey(int badgeID)
	{
		return GetLocalization(ScriptLocalization.GetBadgeGroupRequirementDescriptionKey(badgeID));
	}

	public static string TR_BadgeName(int badgeID)
	{
		return GetLocalization(ScriptLocalization.GetBadgeNameKey(badgeID));
	}

	public static string TR_FreelancerStatDescription(string freelancerType, int statIndex)
	{
		return GetLocalization(ScriptLocalization.GetFreelancerStatDescriptionKey(freelancerType, statIndex));
	}

	public static string TR_FreelancerStatName(string freelancerType, int statIndex)
	{
		return GetLocalization(ScriptLocalization.GetFreelancerStatNameKey(freelancerType, statIndex));
	}

	public static string TR_PersistedStatBucketName(PersistedStatBucket bucket)
	{
		return GetLocalization(ScriptLocalization.GetPersistedStatBucketNameKey(bucket));
	}

	public static string FormatTime(int seconds)
	{
		return $"{seconds / 60}:{seconds % 60:00}";
	}
	
	// inlined in reactor/rogues
	private static string GetLocalization(string term)
	{
#if EVOS
		var locOverrides = LocOverrides;
		if (!locOverrides.IsNullOrEmpty())
		{
			string currentLanguage = LocalizationManager.CurrentLanguage;
			if (locOverrides.TryGetValue(currentLanguage, out var strings)
			    && strings.TryGetValue(term, out string result)
			    && !result.IsNullOrEmpty())
			{
				return result;
			}
		}
#endif
		return ScriptLocalization.Get(term);
	}
}
