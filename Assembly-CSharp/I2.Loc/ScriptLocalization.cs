using System;
using System.Text;

namespace I2.Loc
{
	public static class ScriptLocalization
	{
		public static string Get(string Term)
		{
			return Get(Term, false, 0);
		}

		public static string Get(string Term, bool FixForRTL)
		{
			return Get(Term, FixForRTL, 0);
		}

		public static string Get(string Term, bool FixForRTL, int maxLineLengthForRTL)
		{
			return LocalizationManager.GetTermTranslation(Term, FixForRTL, maxLineLengthForRTL);
		}

		public static string GetLoadingSubtypeHeader(int setIndex, int displayIndex)
		{
			return new StringBuilder().Append("GameSubType_").Append(setIndex).Append("_DisplayIndex_").Append(displayIndex).Append("_Header@GameSubTypeData").ToString();
		}

		public static string GetLoadingSubtypeTooltip(int setIndex, int displayIndex)
		{
			return new StringBuilder().Append("GameSubType_").Append(setIndex).Append("_DisplayIndex_").Append(displayIndex).Append("_Tooltip@GameSubTypeData").ToString();
		}

		public static string GetPersistedStatBucketNameKey(PersistedStatBucket bucket)
		{
			return new StringBuilder().Append("PersistedStatBucket_").Append(bucket.ToString()).Append("@Global").ToString();
		}

		public static string GetQuestNameKey(int questId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_Name@QuestWideData").ToString();
		}

		public static string GetQuestDescription(int questId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_Desc@QuestWideData").ToString();
		}

		public static string GetQuestFlavorTextKey(int questId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_Flavor@QuestWideData").ToString();
		}

		public static string GetQuestLongDescriptionKey(int questId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_LongDesc@QuestWideData").ToString();
		}

		public static string GetQuestTypeDisplayNameKey(int questId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_TypeDisplayName@QuestWideData").ToString();
		}

		public static string GetQuestObjectiveKey(int questId, int objectiveId)
		{
			return new StringBuilder().Append("Quest_").Append(Convert.ToString(questId)).Append("_Obj_").Append(Convert.ToString(objectiveId)).Append("@QuestWideData").ToString();
		}

		public static string GetInventoryItemNameKey(int invItemId)
		{
			return new StringBuilder().Append("InventoryItem_").Append(Convert.ToString(invItemId)).Append("_Name@InventoryWideData").ToString();
		}

		public static string GetInventoryItemDescriptionKey(int invItemId)
		{
			return new StringBuilder().Append("InventoryItem_").Append(Convert.ToString(invItemId)).Append("_Desc@InventoryWideData").ToString();
		}

		public static string GetInventoryObtainedDescriptionKey(int invItemId)
		{
			return new StringBuilder().Append("InventoryItem_").Append(Convert.ToString(invItemId)).Append("_Obtained@InventoryWideData").ToString();
		}

		public static string GetInventoryFlavorTextKey(int invItemId)
		{
			return new StringBuilder().Append("InventoryItem_").Append(Convert.ToString(invItemId)).Append("_Flavor@InventoryWideData").ToString();
		}

		public static string GetLootTableNameKey(int lootId)
		{
			return new StringBuilder().Append("LootTable_").Append(Convert.ToString(lootId)).Append("_Name@InventoryWideData").ToString();
		}

		public static string GetLootTableDescriptionKey(int lootId)
		{
			return new StringBuilder().Append("LootTable_").Append(Convert.ToString(lootId)).Append("_Desc@InventoryWideData").ToString();
		}

		public static string GetKarmaNameKey(int karmaId)
		{
			return new StringBuilder().Append("Karma_").Append(Convert.ToString(karmaId)).Append("_Name@InventoryWideData").ToString();
		}

		public static string GetKarmaDescriptionKey(int karmaId)
		{
			return new StringBuilder().Append("Karma_").Append(Convert.ToString(karmaId)).Append("_Desc@InventoryWideData").ToString();
		}

		public static string GetLoreTitleKey(int loreId)
		{
			return new StringBuilder().Append("Lore_").Append(Convert.ToString(loreId)).Append("_Title@LoreWideData").ToString();
		}

		public static string GetLoreArticleTextKey(int loreId)
		{
			return new StringBuilder().Append("Lore_").Append(Convert.ToString(loreId)).Append("_ArticleText@LoreWideData").ToString();
		}

		public static string GetSeasonNameKey(int seasonId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Name@SeasonWideData").ToString();
		}

		public static string GetSeasonSubTitleKey(int seasonId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_SubTitle@SeasonWideData").ToString();
		}

		public static string GetSeasonEndHeaderKey(int seasonId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_EndHeader@SeasonWideData").ToString();
		}

		public static string GetSeasonChapterNameKey(int seasonId, int chapterId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Chapter_").Append(Convert.ToString(chapterId)).Append("_Name@SeasonWideData").ToString();
		}

		public static string GetSeasonChapterUnlockKey(int seasonId, int chapterId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Chapter_").Append(Convert.ToString(chapterId)).Append("_Unlock@SeasonWideData").ToString();
		}

		public static string GetSeasonStorytimeHeaderKey(int seasonId, int chapterId, int storyId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Chapter_").Append(Convert.ToString(chapterId)).Append("_Story_").Append(Convert.ToString(storyId)).Append("_Header@SeasonWideData").ToString();
		}

		public static string GetSeasonStorytimeBodyKey(int seasonId, int chapterId, int storyId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Chapter_").Append(Convert.ToString(chapterId)).Append("_Story_").Append(Convert.ToString(storyId)).Append("_Body@SeasonWideData").ToString();
		}

		public static string GetSeasonStorytimeLongBodyKey(int seasonId, int chapterId, int storyId)
		{
			return new StringBuilder().Append("Season_").Append(Convert.ToString(seasonId)).Append("_Chapter_").Append(Convert.ToString(chapterId)).Append("_Story_").Append(Convert.ToString(storyId)).Append("_LongBody@SeasonWideData").ToString();
		}

		public static string GetPlayerTitleKey(int titleId)
		{
			return new StringBuilder().Append("PlayerTitle_").Append(Convert.ToString(titleId)).Append("@GameWideData").ToString();
		}

		public static string GetPlayerTitleUnlockConditionKey(int titleId, int unlockConditionId)
		{
			return new StringBuilder().Append("PlayerTitle_").Append(Convert.ToString(titleId)).Append("_Unlock_").Append(Convert.ToString(unlockConditionId)).Append("_Condition@GameWideData").ToString();
		}

		public static string GetPlayerBannerKey(int bannerId)
		{
			return new StringBuilder().Append("PlayerBanner_").Append(Convert.ToString(bannerId)).Append("@GameWideData").ToString();
		}

		public static string GetPlayerBannerUnlockConditionKey(int bannerId, int unlockConditionId)
		{
			return new StringBuilder().Append("PlayerBanner_").Append(Convert.ToString(bannerId)).Append("_Unlock_").Append(Convert.ToString(unlockConditionId)).Append("_Condition@GameWideData").ToString();
		}

		public static string GetBannerObtainedDescriptionKey(int bannerId)
		{
			return new StringBuilder().Append("PlayerBanner_").Append(Convert.ToString(bannerId)).Append("_ObtainedDescription@GameWideData").ToString();
		}

		public static string GetTitleObtainedDescriptionKey(int titleId)
		{
			return new StringBuilder().Append("PlayerTitle_").Append(Convert.ToString(titleId)).Append("_ObtainedDescription@GameWideData").ToString();
		}

		public static string GetPlayerRibbonKey(int ribbonId)
		{
			return new StringBuilder().Append("PlayerRibbon_").Append(Convert.ToString(ribbonId)).Append("@GameWideData").ToString();
		}

		public static string GetPlayerRibbonUnlockConditionKey(int ribbonId, int unlockConditionId)
		{
			return new StringBuilder().Append("PlayerRibbon_").Append(Convert.ToString(ribbonId)).Append("_Unlock_").Append(Convert.ToString(unlockConditionId)).Append("_Condition@GameWideData").ToString();
		}

		public static string GetRibbonObtainedDescriptionKey(int ribbonId)
		{
			return new StringBuilder().Append("PlayerRibbon_").Append(Convert.ToString(ribbonId)).Append("Obtn@GameWideData").ToString();
		}

		public static string GetEmojiNameKey(int emojiId)
		{
			return new StringBuilder().Append("Emoji_").Append(Convert.ToString(emojiId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetEmojiObtainedDescriptionKey(int emojiId)
		{
			return new StringBuilder().Append("Emoji_").Append(Convert.ToString(emojiId)).Append("_ObtainedDescription@GameWideData").ToString();
		}

		public static string GetEmojiPurchaseDescriptionKey(int emojiId)
		{
			return new StringBuilder().Append("Emoji_").Append(Convert.ToString(emojiId)).Append("_PurchaseDescription@GameWideData").ToString();
		}

		public static string GetEmojiTagKey(int emojiId)
		{
			return new StringBuilder().Append("Emoji_").Append(Convert.ToString(emojiId)).Append("_Tag@GameWideData").ToString();
		}

		public static string GetEmojiUnlockKey(int emojiId)
		{
			return new StringBuilder().Append("Emoji_").Append(Convert.ToString(emojiId)).Append("_Unlock@GameWideData").ToString();
		}

		public static string GetMapNameKey(string mapId)
		{
			return new StringBuilder().Append("Map_").Append(mapId).Append("_Name@GameWideData").ToString();
		}

		public static string GetMatrixPackEventTextKey(int matrixId)
		{
			return new StringBuilder().Append("MatrixPack_").Append(matrixId).Append("_EventText@GameWideData").ToString();
		}

		public static string GetMatrixPackDescriptionKey(int matrixId)
		{
			return new StringBuilder().Append("MatrixPack_").Append(matrixId).Append("_Description@GameWideData").ToString();
		}

		public static string GetGamePackNameKey(int packId)
		{
			return new StringBuilder().Append("GamePack_").Append(packId).Append("_Name@GameWideData").ToString();
		}

		public static string GetGamePackDescKey(int packId)
		{
			return new StringBuilder().Append("GamePack_").Append(packId).Append("_Desc@GameWideData").ToString();
		}

		public static string GetCharacterNameKey(string characterId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Name@GameWideData").ToString();
		}

		public static string GetCharacterSelectTooltipKey(string characterId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_SelectTooltip@GameWideData").ToString();
		}

		public static string GetCharacterSelectAboutKey(string characterId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_AboutDesc@GameWideData").ToString();
		}

		public static string GetCharacterBioKey(string characterId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Bio@GameWideData").ToString();
		}

		public static string GetCharacterSkinNameKey(string characterId, int skinId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetCharacterSkinDescriptionKey(string characterId, int skinId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Desc@GameWideData").ToString();
		}

		public static string GetCharacterSkinFlavorKey(string characterId, int skinId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Flavor@GameWideData").ToString();
		}

		public static string GetCharacterPatternNameKey(string characterId, int skinId, int patternId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetPatternColorNameKey(string characterId, int skinId, int patternId, int colorId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Color_").Append(Convert.ToString(colorId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetPatternColorDescKey(string characterId, int skinId, int patternId, int colorId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Color_").Append(Convert.ToString(colorId)).Append("_Desc@GameWideData").ToString();
		}

		public static string GetPatternColorFlavorKey(string characterId, int skinId, int patternId, int colorId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Color_").Append(Convert.ToString(colorId)).Append("_Flavor@GameWideData").ToString();
		}

		public static string GetPatternColorObtainedDescKey(string characterId, int skinId, int patternId, int colorId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Color_").Append(Convert.ToString(colorId)).Append("_Obtn@GameWideData").ToString();
		}

		public static string GetPatternColorPurchaseDescKey(string characterId, int skinId, int patternId, int colorId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Skin_").Append(Convert.ToString(skinId)).Append("_Pattern_").Append(Convert.ToString(patternId)).Append("_Color_").Append(Convert.ToString(colorId)).Append("_Pur@GameWideData").ToString();
		}

		public static string GetCharacterTauntNameKey(string characterId, int tauntId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Taunt_").Append(Convert.ToString(tauntId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetCharacterTauntObtainedDescKey(string characterId, int tauntId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Taunt_").Append(Convert.ToString(tauntId)).Append("_Obtn@GameWideData").ToString();
		}

		public static string GetCharacterTauntPurchaseDescKey(string characterId, int tauntId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Taunt_").Append(Convert.ToString(tauntId)).Append("_Pur@GameWideData").ToString();
		}

		public static string GetCharacterVFXSwapNameKey(string characterId, int abilityId, int vfxSwapId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Ability_").Append(Convert.ToString(abilityId)).Append("_VFX_").Append(Convert.ToString(vfxSwapId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetCharacterVFXSwapObtainedDescKey(string characterId, int abilityId, int vfxSwapId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Ability_").Append(Convert.ToString(abilityId)).Append("_VFX_").Append(Convert.ToString(vfxSwapId)).Append("_Obtn@GameWideData").ToString();
		}

		public static string GetCharacterVFXSwapPurchaseDescKey(string characterId, int abilityId, int vfxSwapId)
		{
			return new StringBuilder().Append("Character_").Append(characterId).Append("_Ability_").Append(Convert.ToString(abilityId)).Append("_VFX_").Append(Convert.ToString(vfxSwapId)).Append("_Pur@GameWideData").ToString();
		}

		public static string GetLoadingScreenTipKey(int tipId)
		{
			return new StringBuilder().Append("LoadingScreenTip_").Append(Convert.ToString(tipId)).Append("@GameWideData").ToString();
		}

		public static string GetKeyBindCommandNameKey(string keyBindId)
		{
			return new StringBuilder().Append("KeyBindCommand_").Append(keyBindId).Append("_Name@GameWideData").ToString();
		}

		public static string GetCardDisplayNameKey(CardType m_cardType)
		{
			return new StringBuilder().Append("Catalyst_").Append(m_cardType.ToString()).Append("_Name@CatalystData").ToString();
		}

		public static string GetAbilityNameKey(string abilityType, string abilityId)
		{
			return new StringBuilder().Append("Ability_").Append(abilityType).Append("_").Append(abilityId).Append("_Name@AbilitiesData").ToString();
		}

		public static string GetAbilityFinalFullTooltipKey(string abilityType, string abilityId)
		{
			return new StringBuilder().Append("Ability_").Append(abilityType).Append("_").Append(abilityId).Append("_Tooltip@AbilitiesData").ToString();
		}

		public static string GetAbilityRewardKey(string abilityType, string abilityId)
		{
			return new StringBuilder().Append("Ability_").Append(abilityType).Append("_").Append(abilityId).Append("_Reward@AbilitiesData").ToString();
		}

		public static string GetAbilityModNameKey(string abilityType, string modId)
		{
			return new StringBuilder().Append("Mod_").Append(abilityType).Append("_").Append(modId).Append("_Name@AbilitiesData").ToString();
		}

		public static string GetAbilityModFinalTooltipKey(string abilityType, string modId)
		{
			return new StringBuilder().Append("Mod_").Append(abilityType).Append("_").Append(modId).Append("_Tooltip@AbilitiesData").ToString();
		}

		public static string GetFreelancerStatNameKey(string freelancerType, int statIndex)
		{
			return new StringBuilder().Append("FreelancerStat_").Append(freelancerType).Append("_").Append(statIndex).Append("_Name@FreelancerStats").ToString();
		}

		public static string GetFreelancerStatDescriptionKey(string freelancerType, int statIndex)
		{
			return new StringBuilder().Append("FreelancerStat_").Append(freelancerType).Append("_").Append(statIndex).Append("_Description@FreelancerStats").ToString();
		}

		public static string GetBadgeGroupName(int badgeGroupIndex)
		{
			return new StringBuilder().Append("BadgeGroup_").Append(badgeGroupIndex).Append("_Name@Badges").ToString();
		}

		public static string GetBadgeGroupDescription(int badgeGroupIndex)
		{
			return new StringBuilder().Append("BadgeGroup_").Append(badgeGroupIndex).Append("_Description@Badges").ToString();
		}

		public static string GetStatNameKey(StatDisplaySettings.StatType statType)
		{
			return new StringBuilder().Append("Stat_").Append(statType.ToString()).Append("_Name@Badges").ToString();
		}

		public static string GetStatDescriptionKey(StatDisplaySettings.StatType statType)
		{
			return new StringBuilder().Append("Stat_").Append(statType.ToString()).Append("_Description@Badges").ToString();
		}

		public static string GetBadgeNameKey(int badgeIndex)
		{
			return new StringBuilder().Append("Badge_").Append(badgeIndex).Append("_Name@Badges").ToString();
		}

		public static string GetBadgeDescriptionKey(int badgeIndex)
		{
			return new StringBuilder().Append("Badge_").Append(badgeIndex).Append("_Description@Badges").ToString();
		}

		public static string GetBadgeGroupRequirementDescriptionKey(int badgeIndex)
		{
			return new StringBuilder().Append("Badge_").Append(badgeIndex).Append("_GroupRequirementDescription@Badges").ToString();
		}

		public static string GetStoreItemOverlayKey(int itemId)
		{
			return new StringBuilder().Append("StoreItem_").Append(itemId).Append("_Overlay@GameWideData").ToString();
		}

		public static string GetSlashCommandKey(string slashcommand)
		{
			return new StringBuilder().Append(slashcommand).Append("@SlashCommand").ToString();
		}

		public static string GetSlashCommandDescKey(string slashcommand)
		{
			return new StringBuilder().Append(slashcommand).Append("@SlashCommandDesc").ToString();
		}

		public static string GetSlashCommandAliasKey(string slashcommand, int aliasId)
		{
			return new StringBuilder().Append(slashcommand).Append("@SlashCommandAlias").Append(Convert.ToString(aliasId)).ToString();
		}

		public static string GetFactionGroupNameKey(int factionGroupId)
		{
			return new StringBuilder().Append("FactionGroup_").Append(Convert.ToString(factionGroupId)).Append("_").Append(Convert.ToString(factionGroupId)).Append("_Name@FactionWideData").ToString();
		}

		public static string GetFactionNameKey(int factionCompletionId, int factionId)
		{
			return new StringBuilder().Append("FactionCompletion_").Append(Convert.ToString(factionCompletionId)).Append("_Faction_").Append(Convert.ToString(factionId)).Append("_Name@FactionWideData").ToString();
		}

		public static string GetFactionLongNameKey(int factionCompletionId, int factionId)
		{
			return new StringBuilder().Append("FactionCompletion_").Append(Convert.ToString(factionCompletionId)).Append("_Faction_").Append(Convert.ToString(factionId)).Append("_Long@FactionWideData").ToString();
		}

		public static string GetFactionLoreDescriptionKey(int factionCompletionId, int factionId)
		{
			return new StringBuilder().Append("FactionCompletion_").Append(Convert.ToString(factionCompletionId)).Append("_Faction_").Append(Convert.ToString(factionId)).Append("_Lore@FactionWideData").ToString();
		}

		public static string GetStatusIconPopupTextKey(int statusIconId)
		{
			return new StringBuilder().Append("StatusIcon_").Append(Convert.ToString(statusIconId)).Append("_PopupText@AbilitiesData").ToString();
		}

		public static string GetStatusIconBuffNameKey(int statusIconId)
		{
			return new StringBuilder().Append("StatusIcon_").Append(Convert.ToString(statusIconId)).Append("_BuffName@AbilitiesData").ToString();
		}

		public static string GetStatusIconBuffDescKey(int statusIconId)
		{
			return new StringBuilder().Append("StatusIcon_").Append(Convert.ToString(statusIconId)).Append("_BuffDesc@AbilitiesData").ToString();
		}

		public static string GetSpectatorToggleOptionKey(UISpectatorHUD.SpectatorToggleOption option)
		{
			return new StringBuilder().Append("SpectatorTogOp_").Append((int)option).Append("@AbilitiesData").ToString();
		}

		public static string GetOverconNameKey(int overconId)
		{
			return new StringBuilder().Append("Overcon_").Append(Convert.ToString(overconId)).Append("_Name@UIOverconData").ToString();
		}

		public static string GetOverconCommandKey(int overconId)
		{
			return new StringBuilder().Append("Overcon_").Append(Convert.ToString(overconId)).Append("_Command@UIOverconData").ToString();
		}

		public static string GetOverconObtainedDescKey(int overconId)
		{
			return new StringBuilder().Append("Overcon_").Append(Convert.ToString(overconId)).Append("_ObtainedDesc@UIOverconData").ToString();
		}

		public static string GetOverconUnlockConditionKey(int overconId, int unlockConditionId)
		{
			return new StringBuilder().Append("Overcon_").Append(Convert.ToString(overconId)).Append("_Unlock_").Append(Convert.ToString(unlockConditionId)).Append("_Condition@UIOverconData").ToString();
		}

		public static string GetLoadingScreenBackgroundNameKey(int loadingScreenBgId)
		{
			return new StringBuilder().Append("LoadingScreenBg_").Append(Convert.ToString(loadingScreenBgId)).Append("_Name@GameWideData").ToString();
		}

		public static string GetLoadingScreenBackgroundObtainedDescriptionKey(int loadingScreenBgId)
		{
			return new StringBuilder().Append("LoadingScreenBg_").Append(Convert.ToString(loadingScreenBgId)).Append("_ObtainedDesc@GameWideData").ToString();
		}

		public static string GetLoadingScreenBackgroundPurchaseDescriptionKey(int loadingScreenBgId)
		{
			return new StringBuilder().Append("LoadingScreenBg_").Append(Convert.ToString(loadingScreenBgId)).Append("_PurchaseDesc@GameWideData").ToString();
		}
	}
}
