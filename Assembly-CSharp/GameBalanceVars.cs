using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBalanceVars
{
	[Serializable]
	public class LobbyStatSettings
	{
		public StatDisplaySettings.StatType m_StatType;

		public float LowWatermark = float.MinValue;

		public bool LowerIsBetter;
	}

	[Serializable]
	public class GameResultBadge : IUniqueID
	{
		public enum BadgeQuality
		{
			Bronze,
			Silver,
			Gold
		}

		public enum BadgeRole
		{
			General,
			Frontliner,
			Firepower,
			Support
		}

		public enum ComparisonType
		{
			None,
			Global,
			Game,
			Freelancer
		}

		[Serializable]
		public class BadgeCondition
		{
			public enum BadgeFunctionType
			{
				None,
				LessThan,
				GreaterThan,
				LessThanOrEqual,
				GreaterThanOrEqual
			}

			public StatDisplaySettings.StatType[] StatsToSum;

			public BadgeFunctionType FunctionType;

			public float ValueToCompare;

			public bool DoesValueMeetConditions(float value)
			{
				if (StatsToSum.Length == 0)
				{
					return true;
				}
				if (FunctionType == BadgeFunctionType.GreaterThan)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return value > ValueToCompare;
						}
					}
				}
				if (FunctionType == BadgeFunctionType.GreaterThanOrEqual)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return value >= ValueToCompare;
						}
					}
				}
				if (FunctionType == BadgeFunctionType.LessThan)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return value < ValueToCompare;
						}
					}
				}
				if (FunctionType == BadgeFunctionType.LessThanOrEqual)
				{
					return value <= ValueToCompare;
				}
				Log.Error("Attempting to sum stats but did not specify compare function");
				return false;
			}
		}

		public string DisplayName;

		public string BadgeDescription;

		public string BadgeGroupRequirementDescription;

		[AssetFileSelector("", "", "")]
		public string BadgeIconString;

		public bool DisplayEvenIfConsolidated;

		public int UniqueBadgeID;

		public BadgeCondition[] MinimumConditions;

		public BadgeQuality Quality;

		public BadgeRole Role;

		public StatDisplaySettings.StatType BadgePointCalcType;

		public List<StatDisplaySettings.StatType> StatsToHighlight = new List<StatDisplaySettings.StatType>();

		public ComparisonType ComparisonGroup;

		public int GlobalPercentileToObtain;

		public bool UsesFreelancerStats;

		public float GetQualityWorth()
		{
			switch (Quality)
			{
			case BadgeQuality.Bronze:
				return 5f;
			case BadgeQuality.Silver:
				return 8f;
			case BadgeQuality.Gold:
				return 11f;
			default:
				throw new Exception("bad quality");
			}
		}

		public int GetID()
		{
			return UniqueBadgeID;
		}

		public void SetID(int newID)
		{
			UniqueBadgeID = newID;
		}

		public bool CouldRecieveBothBadgesInOneGame(GameResultBadge other)
		{
			if (ComparisonGroup != other.ComparisonGroup)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			if (UsesFreelancerStats != other.UsesFreelancerStats)
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
			switch (ComparisonGroup)
			{
			case ComparisonType.Global:
			case ComparisonType.Game:
				return BadgePointCalcType != other.BadgePointCalcType;
			case ComparisonType.Freelancer:
				return other.ComparisonGroup != ComparisonType.Freelancer;
			case ComparisonType.None:
			{
				if (MinimumConditions.Length != other.MinimumConditions.Length)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				for (int i = 0; i < MinimumConditions.Length; i++)
				{
					if (MinimumConditions[i].StatsToSum.IsNullOrEmpty() != other.MinimumConditions[i].StatsToSum.IsNullOrEmpty())
					{
						return true;
					}
					if (MinimumConditions[i].StatsToSum.Length != other.MinimumConditions[i].StatsToSum.Length)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					for (int j = 0; j < MinimumConditions[i].StatsToSum.Length; j++)
					{
						if (MinimumConditions[i].StatsToSum[j] == other.MinimumConditions[i].StatsToSum[j])
						{
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							return true;
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							goto end_IL_0141;
						}
						continue;
						end_IL_0141:
						break;
					}
				}
				return false;
			}
			default:
				throw new Exception("Strange comparison group");
			}
		}

		public bool DoStatsMeetMinimumRequirementsForBadge(StatDisplaySettings.IPersistatedStatValueSupplier StatSupplier)
		{
			for (int i = 0; i < MinimumConditions.Length; i++)
			{
				if (MinimumConditions[i].StatsToSum.IsNullOrEmpty())
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				float num = 0f;
				for (int j = 0; j < MinimumConditions[i].StatsToSum.Length; j++)
				{
					float? stat = StatSupplier.GetStat(MinimumConditions[i].StatsToSum[j]);
					if (stat.HasValue)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						num += stat.Value;
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (MinimumConditions[i].DoesValueMeetConditions(num))
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return false;
				}
			}
			return true;
		}
	}

	[Serializable]
	public class CharacterLevelReward
	{
		public int charType;

		public int startLevel;

		public int repeatingLevel;

		public QuestItemReward reward;
	}

	[Serializable]
	public class RAFReward
	{
		public int pointsRequired;

		public int questId;

		public bool isEnabled = true;

		public bool isRepeating;
	}

	[Serializable]
	public class RankedTierLockedItems
	{
		public int MaxTier;

		public int MinTier;

		public PlayerUnlockableReference[] Items;
	}

	[Serializable]
	public class CharacterUnlockData : PlayerUnlockable
	{
		public CharacterType character;

		public SkinUnlockData[] skinUnlockData;

		public TauntUnlockData[] tauntUnlockData;

		public AbilityVfxUnlockData[] abilityVfxUnlockData;

		public void CopyValuesToCharLinkUnlockData(CharResourceLinkCharUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class TauntUnlockData : PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void CopyValuesTo(TauntUnlockData other)
		{
			CopyValuesToBase(other);
		}

		public TauntUnlockData Clone()
		{
			TauntUnlockData tauntUnlockData = new TauntUnlockData();
			CopyValuesToBase(tauntUnlockData);
			return tauntUnlockData;
		}
	}

	[Serializable]
	public class SkinUnlockData : PlayerUnlockable
	{
		public PatternUnlockData[] patternUnlockData;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void CopyValuesToCharLinkUnlockData(CharResourceLinkSkinUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class PatternUnlockData : PlayerUnlockable
	{
		public ColorUnlockData[] colorUnlockData;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return Index2;
		}

		public void CopyValuesToCharLinkUnlockData(CharResourceLinkPatternUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class ColorUnlockData : PlayerUnlockable
	{
		public bool UnlockAutomaticallyWithParentSkin;

		public bool UsableByBots;

		public float UsableByBotsWeight;

		public bool IsGoldenAge;

		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return Index2;
		}

		public void SetPatternIndex(int patternIndex)
		{
			Index3 = patternIndex;
		}

		public int GetPatternIndex()
		{
			return Index3;
		}

		public void CopyValuesTo(ColorUnlockData other)
		{
			CopyValuesToBase(other);
			other.UnlockAutomaticallyWithParentSkin = UnlockAutomaticallyWithParentSkin;
			other.UsableByBots = UsableByBots;
			other.UsableByBotsWeight = UsableByBotsWeight;
			other.IsGoldenAge = IsGoldenAge;
		}
	}

	[Serializable]
	public class AbilityVfxUnlockData : PlayerUnlockable
	{
		public override string ToString()
		{
			return "Ability[" + Index2 + "]->VfxSwap[" + ID + "]";
		}

		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharacterTypeInt()
		{
			return Index1;
		}

		public void SetSwapAbilityId(int swapAbilityId)
		{
			Index2 = swapAbilityId;
		}

		public int GetSwapAbilityId()
		{
			return Index2;
		}

		public void CopyValuesTo(AbilityVfxUnlockData other)
		{
			CopyValuesToBase(other);
		}

		public AbilityVfxUnlockData Clone()
		{
			AbilityVfxUnlockData abilityVfxUnlockData = new AbilityVfxUnlockData();
			CopyValuesToBase(abilityVfxUnlockData);
			return abilityVfxUnlockData;
		}
	}

	[Serializable]
	public class CharResourceLinkCharUnlockData : PlayerUnlockable
	{
		public void CopyValuesTo(CharacterUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class CharResourceLinkSkinUnlockData : PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void CopyValuesTo(SkinUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class CharResourceLinkPatternUnlockData : PlayerUnlockable
	{
		public void SetCharacterTypeInt(int charTypeInt)
		{
			Index1 = charTypeInt;
		}

		public int GetCharTypeInt()
		{
			return Index1;
		}

		public void SetSkinIndex(int skinIndex)
		{
			Index2 = skinIndex;
		}

		public int GetSkinIndex()
		{
			return Index2;
		}

		public void CopyValuesTo(PatternUnlockData other)
		{
			CopyValuesToBase(other);
		}
	}

	[Serializable]
	public class AbilityModUnlockData : PlayerUnlockable
	{
	}

	[Serializable]
	public class LevelProgressInfo
	{
		public int ExperienceToNextLevel;

		public CurrencyData[] CurrencyRewards;
	}

	public enum UnlockableType
	{
		None,
		Banner,
		Title,
		Character,
		Taunt,
		Skin,
		Pattern,
		Color,
		AbilityVfx,
		AbilityMod,
		Emoji,
		Overcon,
		LoadingScreenBackground
	}

	[Serializable]
	public class UnlockData
	{
		public enum UnlockType
		{
			CharacterLevel,
			PlayerLevel,
			Purchase,
			ELO,
			Custom,
			Quest,
			HasDateTimePassed,
			FactionTierReached,
			TitleLevelReached,
			CurrentSeason
		}

		public string LogicStatement;

		public UnlockCondition[] UnlockConditions;

		public string VisibilityLogicStatement;

		public UnlockCondition[] VisibilityConditions;

		public string PurchaseableLogicStatement;

		public UnlockCondition[] PurchaseableConditions;

		public void InitValues()
		{
			LogicStatement = string.Empty;
			UnlockConditions = new UnlockCondition[0];
			VisibilityLogicStatement = string.Empty;
			VisibilityConditions = new UnlockCondition[0];
			PurchaseableLogicStatement = string.Empty;
			PurchaseableConditions = new UnlockCondition[0];
		}

		public UnlockData Clone()
		{
			UnlockData unlockData = new UnlockData();
			unlockData.LogicStatement = LogicStatement;
			unlockData.VisibilityLogicStatement = VisibilityLogicStatement;
			unlockData.PurchaseableLogicStatement = PurchaseableLogicStatement;
			unlockData.UnlockConditions = null;
			if (UnlockConditions != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				unlockData.UnlockConditions = new UnlockCondition[UnlockConditions.Length];
				for (int i = 0; i < UnlockConditions.Length; i++)
				{
					UnlockCondition[] unlockConditions = unlockData.UnlockConditions;
					int num = i;
					object obj;
					if (UnlockConditions[i] != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						obj = UnlockConditions[i].Clone();
					}
					else
					{
						obj = null;
					}
					unlockConditions[num] = (UnlockCondition)obj;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			unlockData.VisibilityConditions = null;
			if (VisibilityConditions != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				unlockData.VisibilityConditions = new UnlockCondition[VisibilityConditions.Length];
				for (int j = 0; j < VisibilityConditions.Length; j++)
				{
					UnlockCondition[] visibilityConditions = unlockData.VisibilityConditions;
					int num2 = j;
					object obj2;
					if (VisibilityConditions[j] != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						obj2 = VisibilityConditions[j].Clone();
					}
					else
					{
						obj2 = null;
					}
					visibilityConditions[num2] = (UnlockCondition)obj2;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			unlockData.PurchaseableConditions = null;
			if (PurchaseableConditions != null)
			{
				unlockData.PurchaseableConditions = new UnlockCondition[PurchaseableConditions.Length];
				for (int k = 0; k < PurchaseableConditions.Length; k++)
				{
					UnlockCondition[] purchaseableConditions = unlockData.PurchaseableConditions;
					int num3 = k;
					object obj3;
					if (PurchaseableConditions[k] != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						obj3 = PurchaseableConditions[k].Clone();
					}
					else
					{
						obj3 = null;
					}
					purchaseableConditions[num3] = (UnlockCondition)obj3;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return unlockData;
		}

		public static UnlockCondition[] CloneUnlockConditionArray(UnlockCondition[] input)
		{
			UnlockCondition[] array = null;
			if (input != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				array = new UnlockCondition[input.Length];
				for (int i = 0; i < input.Length; i++)
				{
					array[i] = ((input[i] == null) ? null : input[i].Clone());
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return array;
		}
	}

	[Serializable]
	public class UnlockCondition
	{
		public UnlockData.UnlockType ConditionType;

		public int typeSpecificData;

		public int typeSpecificData2;

		public int typeSpecificData3;

		public List<int> typeSpecificDate;

		public string typeSpecificString;

		public UnlockCondition Clone()
		{
			UnlockCondition unlockCondition = (UnlockCondition)MemberwiseClone();
			if (typeSpecificDate != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				unlockCondition.typeSpecificDate = new List<int>(typeSpecificDate);
			}
			return unlockCondition;
		}
	}

	[Serializable]
	public class UnlockConditionValue
	{
		public UnlockData.UnlockType ConditionType;

		public int typeSpecificData;

		public int typeSpecificData2;

		public int typeSpecificData3;

		public List<int> typeSpecificDate;

		public string typeSpecificString;
	}

	public interface IUniqueID
	{
		int GetID();

		void SetID(int newID);
	}

	[Serializable]
	public class PlayerUnlockable : IUniqueID
	{
		[HideInInspector]
		public int ID;

		public string Name;

		public int m_sortOrder;

		public string ObtainedDescription;

		public string PurchaseDescription;

		public InventoryItemRarity Rarity;

		public CountryPrices Prices;

		public int Index1;

		public int Index2;

		public int Index3;

		public UnlockData m_unlockData;

		public bool m_isHidden;

		public int GetID()
		{
			return ID;
		}

		public void SetID(int newID)
		{
			ID = newID;
		}

		public string GetObtainedDescription()
		{
			if (this is ChatEmoticon)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return StringUtil.TR_EmojiObtainedDescription(ID);
					}
				}
			}
			if (this is ColorUnlockData)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						CharacterType index = (CharacterType)Index1;
						return StringUtil.TR_CharacterPatternColorObtainedDescription(index.ToString(), Index2 + 1, Index3 + 1, ID + 1);
					}
					}
				}
			}
			if (this is TauntUnlockData)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						CharacterType index2 = (CharacterType)Index1;
						return StringUtil.TR_CharacterTauntObtainedDescription(index2.ToString(), ID + 1);
					}
					}
				}
			}
			if (this is AbilityVfxUnlockData)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						CharacterType index3 = (CharacterType)Index1;
						return StringUtil.TR_GetCharacterVFXSwapObtainedDescription(index3.ToString(), Index2 + 1, ID);
					}
					}
				}
			}
			if (this is PlayerRibbon)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return StringUtil.TR_RibbonObtainedDescription(ID);
					}
				}
			}
			if (this is PlayerBanner)
			{
				return StringUtil.TR_BannerObtainedDescription(ID);
			}
			if (this is PlayerTitle)
			{
				return StringUtil.TR_TitleObtainedDescription(ID);
			}
			return ObtainedDescription;
		}

		public string GetPurchaseDescription()
		{
			if (this is ChatEmoticon)
			{
				return StringUtil.TR_EmojiPurchaseDescription(ID);
			}
			if (this is ColorUnlockData)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						CharacterType index = (CharacterType)Index1;
						return StringUtil.TR_CharacterPatternColorPurchaseDescription(index.ToString(), Index2 + 1, Index3 + 1, ID + 1);
					}
					}
				}
			}
			if (this is TauntUnlockData)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						CharacterType index2 = (CharacterType)Index1;
						return StringUtil.TR_CharacterTauntPurchaseDescription(index2.ToString(), ID + 1);
					}
					}
				}
			}
			if (this is AbilityVfxUnlockData)
			{
				CharacterType index3 = (CharacterType)Index1;
				return StringUtil.TR_GetCharacterVFXSwapPurchaseDescription(index3.ToString(), Index2 + 1, ID);
			}
			return PurchaseDescription;
		}

		public string GetName()
		{
			return Name;
		}

		protected void CopyValuesToBase(PlayerUnlockable other)
		{
			other.ID = ID;
			other.Name = Name;
			other.m_sortOrder = m_sortOrder;
			other.ObtainedDescription = ObtainedDescription;
			other.PurchaseDescription = PurchaseDescription;
			other.Rarity = Rarity;
			other.Prices = Prices;
			other.Index1 = Index1;
			other.Index2 = Index2;
			other.Index3 = Index3;
			if (m_unlockData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				other.m_unlockData = m_unlockData.Clone();
			}
			else
			{
				other.m_unlockData = new UnlockData();
				other.m_unlockData.InitValues();
			}
			other.m_isHidden = m_isHidden;
		}
	}

	[Serializable]
	public class PlayerUnlockableReference
	{
		public UnlockableType Type;

		public int ID;

		public int Index1;

		public int Index2;

		public int Index3;
	}

	[Serializable]
	public class UnlockExlusivePool
	{
		public enum ExclusivePoolType
		{
			None,
			Banner,
			Ribbon,
			Emoticon,
			Overcon,
			Title
		}

		public string PoolName;

		public ExclusivePoolType PoolType;

		public int[] PoolOfBannerIDs;

		public int TotalBannersAbleToBeUnlockedAtOnce;
	}

	[Serializable]
	public class ChatEmoticon : PlayerUnlockable
	{
		public string IconPath;

		public string GetEmojiName()
		{
			return StringUtil.TR_EmojiName(ID);
		}
	}

	[Serializable]
	public class OverconUnlockData : PlayerUnlockable
	{
		public string m_commandName;

		public string GetOverconName()
		{
			return StringUtil.TR_GetOverconDisplayName(ID);
		}
	}

	[Serializable]
	public class LoadingScreenBackground : PlayerUnlockable
	{
		public string m_resourceString;

		public string m_iconPath;

		public string GetLoadingScreenBackgroundName()
		{
			return StringUtil.TR_GetLoadingScreenBackgroundName(ID);
		}
	}

	[Serializable]
	public class TitleLevelDefinition
	{
		public string m_name;

		public int m_titleId;

		public int m_maxLevel;
	}

	[Serializable]
	public class PlayerTitle : PlayerUnlockable
	{
		public CharacterType m_relatedCharacter;

		public const string c_levelToken = "[^level^]";

		public string GetTitleText(int titleLevel = -1)
		{
			string text = StringUtil.TR_PlayerTitle(ID);
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!text.IsNullOrEmpty())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (text.IndexOf("[^level^]", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (titleLevel < 0)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							titleLevel = ClientGameManager.Get().GetCurrentTitleLevel(ID);
						}
						text = ReplaceLevelIntoTitle(text, titleLevel);
					}
				}
			}
			return text;
		}

		public static string ReplaceLevelIntoTitle(string title, int level)
		{
			int length = "[^level^]".Length;
			string str = level.ToString();
			int num = 0;
			int num2 = -1;
			while ((num2 = title.IndexOf("[^level^]", StringComparison.OrdinalIgnoreCase)) >= 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (num < 100)
				{
					title = title.Substring(0, num2) + str + title.Substring(num2 + length);
					num++;
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			return title;
		}
	}

	[Serializable]
	public class PlayerBanner : PlayerUnlockable
	{
		public enum BannerType
		{
			Foreground,
			Background
		}

		public string m_resourceString;

		public string m_iconResourceString;

		public BannerType m_type;

		public CharacterType m_relatedCharacter;

		public bool m_isDefault;

		public string GetBannerName()
		{
			return StringUtil.TR_BannerName(ID);
		}

		public new string GetObtainedDescription()
		{
			return StringUtil.TR_BannerObtainedDescription(ID);
		}
	}

	[Serializable]
	public class PlayerRibbon : PlayerUnlockable
	{
		public string m_resourceString;

		public string m_resourceIconString;

		public string GetRibbonName()
		{
			return StringUtil.TR_RibbonName(ID);
		}
	}

	[Serializable]
	public class StoreItemForPurchase : PlayerUnlockable
	{
		public int m_itemTemplateId;

		public string m_productCode;

		public string m_overlayText;

		public string GetStoreItemName()
		{
			return StringUtil.TR_InventoryItemName(m_itemTemplateId);
		}
	}

	public enum GameRewardBucketType
	{
		FullVsHumanRewards,
		HumanVsBotsRewards,
		CasualGameRewards,
		NewPlayerRewards,
		PlacementMatchRewards,
		NoRewards
	}

	[Serializable]
	public class GameRewardBucket
	{
		public int XPBonusForWin;

		public float XPBonusPerMinute;

		public float XPBonusPerMinuteCap;

		public float XPBonusPerQueueTimeMinute;

		public float XPBonusPerQueueTimeMinuteCap;

		public GameCurrencyReward[] CurrencyRewards;
	}

	[Serializable]
	public class GameCurrencyReward
	{
		public CurrencyType Type;

		public int AmountOnWin;

		public int AmountOnLoss;
	}

	public const int NUM_GG_PACKS_ALLOWED_PER_GAME_PER_USER = 3;

	public int ResetSelectionVersion;

	public float GGPackEndGameUsageTimer = 15f;

	public float GGPackInGameCooldownTimer = 2f;

	public float GGPackXPBonusPerPack;

	public float GGPackFreelancerCurrencyMultPerPack;

	public float GGPackSelfXPBonus;

	public float GGPackSelfFreelancerCurrencyMult;

	public int[] GGPackISOAdditionalBonus;

	public int[] GGPackFreelancerCurrencyAdditionalBonus;

	public float[] GGPackXPAdditionalBonus;

	public float[] GGPackFreelancerCurrencyAdditionalMult;

	public int GGPackISOBonusPerPack;

	public int GGPackFreelancerCurrencyBonusPerPack;

	public int GGPackSelfISOBonus;

	public int GGPackSelfFreelancerCurrencyBonus;

	public float PlayWithFriendXPBonusMult;

	public float XPMultForOwnedFreelancers;

	public int m_firstLevelXpAwarded;

	public int m_secondLevelXpAwarded;

	public int FirstWinOfDayQuestId;

	public int IsoRewardedPerFreelancerOnGamePurchase = 1000;

	public int MaxNumberOfGlobalLoadoutSlots = 10;

	public int MaxNumberOfFreelancerLoadoutSlots = 10;

	public int GlobalLoadoutSlotFluxCost = 10000;

	public int FreelancerLoadoutSlotFluxCost = 1000;

	public LevelProgressInfo[] PlayerProgressInfo;

	public LevelProgressInfo[] CharacterProgressInfo;

	public LevelProgressInfo RepeatingCharacterProgressInfo;

	public RewardUtils.RewardType[] RewardDisplayPriorityOrder;

	public GameRewardBucket[] GameRewardBuckets;

	private LevelProgressInfo[] m_CharProgressInfoWithRepeating;

	public CharacterLevelReward[] RepeatingCharacterLevelRewards;

	public TitleLevelDefinition[] TitleLevelDefinitions;

	public PlayerTitle[] PlayerTitles;

	public PlayerBanner[] PlayerBanners;

	public PlayerRibbon[] PlayerRibbons;

	public ChatEmoticon[] ChatEmojis;

	public OverconUnlockData[] Overcons;

	public StoreItemForPurchase[] StoreItemsForPurchase;

	public UnlockExlusivePool[] ExclusivityPools;

	public GameResultBadge[] GameResultBadges;

	public LobbyStatSettings[] StatSettings;

	public LoadingScreenBackground[] LoadingScreenBackgrounds;

	public int NumSecsToOpenLootMatrix;

	public CharacterUnlockData[] characterUnlockData;

	public RAFReward[] RAFRewards;

	public int FreelancerCurrencyPerModToken;

	public bool UseModEquipCostAsModUnlockCost;

	public int FreelancerCurrencyToUnlockMod;

	public RankedTierLockedItems[] RankedTierLockedItemsList;

	public LevelProgressInfo[] CharProgressInfoWithRepeating
	{
		get
		{
			if (m_CharProgressInfoWithRepeating != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_CharProgressInfoWithRepeating.Length == CharacterProgressInfo.Length + 1)
				{
					goto IL_0090;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_CharProgressInfoWithRepeating = new LevelProgressInfo[CharacterProgressInfo.Length + 1];
			for (int i = 0; i < CharacterProgressInfo.Length; i++)
			{
				m_CharProgressInfoWithRepeating[i] = CharacterProgressInfo[i];
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_CharProgressInfoWithRepeating[CharacterProgressInfo.Length] = RepeatingCharacterProgressInfo;
			goto IL_0090;
			IL_0090:
			return m_CharProgressInfoWithRepeating;
		}
	}

	public int MaxPlayerLevel => 0;

	public int CharacterSilverLevel => 10;

	public int CharacterMasteryLevel => 20;

	public int CharacterPurpleLevel => 40;

	public int CharacterRedLevel => 60;

	public int CharacterDiamondLevel => 80;

	public int CharacterRainbowLevel => 100;

	public int MaxCharacterLevelForRewards
	{
		get
		{
			if (CharacterProgressInfo == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 0;
					}
				}
			}
			return CharacterProgressInfo.Length + 1;
		}
	}

	public int MaxCharacterLevel => 32767;

	public static GameBalanceVars Get()
	{
		return GameWideData.Get().m_gameBalanceVars;
	}

	public bool IsStatLowerBetter(StatDisplaySettings.StatType StatType)
	{
		for (int i = 0; i < StatSettings.Length; i++)
		{
			if (StatSettings[i].m_StatType != StatType)
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return StatSettings[i].LowerIsBetter;
			}
		}
		return false;
	}

	public int GetGGPackBonusISO(int numPacksUsed, int selfUsedPack)
	{
		int num = 0;
		GameBalanceVars gameBalanceVars = Get();
		num += gameBalanceVars.GGPackISOBonusPerPack * numPacksUsed;
		num += gameBalanceVars.GGPackSelfISOBonus * selfUsedPack;
		if (0 < numPacksUsed)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (numPacksUsed - 1 < gameBalanceVars.GGPackISOAdditionalBonus.Length)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				num += gameBalanceVars.GGPackISOAdditionalBonus[numPacksUsed - 1];
			}
		}
		return num;
	}

	public float GetGGPackXPMultiplier(int numPacksUsed, int selfUsedPack)
	{
		float num = 1f;
		GameBalanceVars gameBalanceVars = Get();
		num += gameBalanceVars.GGPackXPBonusPerPack * (float)numPacksUsed;
		num += gameBalanceVars.GGPackSelfXPBonus * (float)selfUsedPack;
		for (int i = 0; i < numPacksUsed; i++)
		{
			if (numPacksUsed - 1 < gameBalanceVars.GGPackXPAdditionalBonus.Length)
			{
				num += gameBalanceVars.GGPackXPAdditionalBonus[i];
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return (float)Math.Round(num, 1);
		}
	}

	public CharacterUnlockData GetCharacterUnlockData(CharacterType character)
	{
		if (this.characterUnlockData != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CharacterUnlockData[] array = this.characterUnlockData;
			foreach (CharacterUnlockData characterUnlockData in array)
			{
				if (characterUnlockData.character != character)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return characterUnlockData;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	public int PlayerExperienceToLevel(int currentLevel)
	{
		if (currentLevel >= 1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentLevel <= PlayerProgressInfo.Length)
			{
				return PlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		throw new ArgumentException($"Current level {currentLevel} is outside the player level range {1}-{PlayerProgressInfo.Length}");
	}

	public int CharacterExperienceToLevel(int currentLevel)
	{
		if (currentLevel < 1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new ArgumentException($"Attempting to access character experience less than 1");
				}
			}
		}
		if (currentLevel - 1 < CharacterProgressInfo.Length)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return CharacterProgressInfo[currentLevel - 1].ExperienceToNextLevel;
				}
			}
		}
		return RepeatingCharacterProgressInfo.ExperienceToNextLevel;
	}

	public string GetTitle(int titleID, string returnOnEmptyOverride = "", int titleLevel = -1)
	{
		for (int i = 0; i < PlayerTitles.Length; i++)
		{
			if (PlayerTitles[i].ID != titleID)
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return PlayerTitles[i].GetTitleText(titleLevel);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return returnOnEmptyOverride;
		}
	}

	public int GetMaxTitleLevel(int titleID)
	{
		for (int i = 0; i < TitleLevelDefinitions.Length; i++)
		{
			if (TitleLevelDefinitions[i].m_titleId != titleID)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return TitleLevelDefinitions[i].m_maxLevel;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return 0;
		}
	}

	public int GetChatEmojiIDByName(string emojiName)
	{
		for (int i = 0; i < ChatEmojis.Length; i++)
		{
			if (!(ChatEmojis[i].Name == emojiName))
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return ChatEmojis[i].ID;
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return -1;
		}
	}

	public int GetChatEmojiIndexByName(string emojiName)
	{
		for (int i = 0; i < ChatEmojis.Length; i++)
		{
			if (ChatEmojis[i].Name == emojiName)
			{
				return i;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return -1;
		}
	}

	public GameResultBadge GetGameBadge(int badgeID)
	{
		if (GameResultBadgeData.Get() != null)
		{
			for (int i = 0; i < GameResultBadgeData.Get().GameResultBadges.Length; i++)
			{
				if (GameResultBadgeData.Get().GameResultBadges[i].UniqueBadgeID == badgeID)
				{
					return GameResultBadgeData.Get().GameResultBadges[i];
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		for (int j = 0; j < GameResultBadges.Length; j++)
		{
			if (GameResultBadges[j].UniqueBadgeID != badgeID)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return GameResultBadges[j];
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return null;
		}
	}

	public string GetUnlocalizedChatEmojiName(int emojiID, string returnOnEmptyOverride = "")
	{
		for (int i = 0; i < ChatEmojis.Length; i++)
		{
			if (ChatEmojis[i].ID == emojiID)
			{
				return ChatEmojis[i].Name;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return returnOnEmptyOverride;
		}
	}

	public ChatEmoticon GetChatEmoji(int emojiID)
	{
		for (int i = 0; i < ChatEmojis.Length; i++)
		{
			if (ChatEmojis[i].ID != emojiID)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return ChatEmojis[i];
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return null;
		}
	}

	public string GetBannerName(int bannerID, string returnOnEmptyOverride = "")
	{
		PlayerBanner banner = GetBanner(bannerID);
		string result;
		if (banner != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = banner.GetBannerName();
		}
		else
		{
			result = returnOnEmptyOverride;
		}
		return result;
	}

	public PlayerBanner GetBanner(int bannerID)
	{
		for (int i = 0; i < PlayerBanners.Length; i++)
		{
			if (PlayerBanners[i].ID == bannerID)
			{
				return PlayerBanners[i];
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return null;
		}
	}

	public PlayerRibbon GetRibbon(int ribbonID)
	{
		for (int i = 0; i < PlayerRibbons.Length; i++)
		{
			if (PlayerRibbons[i].ID != ribbonID)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return PlayerRibbons[i];
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return null;
		}
	}

	public LoadingScreenBackground GetLoadingScreenBackground(int loadingScreenID)
	{
		for (int i = 0; i < LoadingScreenBackgrounds.Length; i++)
		{
			if (LoadingScreenBackgrounds[i].ID != loadingScreenID)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return LoadingScreenBackgrounds[i];
			}
		}
		return null;
	}

	public SkinUnlockData GetSkinUnlockData(CharacterType characterType, int skinIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex];
	}

	public PatternUnlockData GetPatternUnlockData(CharacterType characterType, int skinIndex, int patternIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex];
	}

	public ColorUnlockData GetColorUnlockData(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].colorUnlockData[colorIndex];
	}

	public string GetSkinName(CharacterType characterType, int skinIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].Name;
	}

	public string GetPatternName(CharacterType characterType, int skinIndex, int patternIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].Name;
	}

	public string GetColorName(CharacterType characterType, int skinIndex, int patternIndex, int colorIndex)
	{
		CharacterUnlockData characterUnlockData = GetCharacterUnlockData(characterType);
		return characterUnlockData.skinUnlockData[skinIndex].patternUnlockData[patternIndex].colorUnlockData[colorIndex].Name;
	}

	public int GetXPBonusForWin(GameRewardBucketType bucketType)
	{
		if (bucketType >= GameRewardBucketType.FullVsHumanRewards)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)bucketType < GameRewardBuckets.Length)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return GameRewardBuckets[(int)bucketType].XPBonusForWin;
					}
				}
			}
		}
		return 0;
	}

	public float GetXPBonusPerMinute(GameRewardBucketType bucketType)
	{
		if (bucketType >= GameRewardBucketType.FullVsHumanRewards)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)bucketType < GameRewardBuckets.Length)
			{
				return GameRewardBuckets[(int)bucketType].XPBonusPerMinute;
			}
		}
		return 0f;
	}

	public float GetXPBonusPerMinuteCap(GameRewardBucketType bucketType)
	{
		if (bucketType >= GameRewardBucketType.FullVsHumanRewards)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)bucketType < GameRewardBuckets.Length)
			{
				return GameRewardBuckets[(int)bucketType].XPBonusPerMinuteCap;
			}
		}
		return 0f;
	}

	public float GetXPBonusPerQueueTimeMinute(GameRewardBucketType bucketType)
	{
		if (bucketType >= GameRewardBucketType.FullVsHumanRewards)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)bucketType < GameRewardBuckets.Length)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return GameRewardBuckets[(int)bucketType].XPBonusPerQueueTimeMinute;
					}
				}
			}
		}
		return 0f;
	}

	public float GetXPBonusPerQueueTimeMinuteCap(GameRewardBucketType bucketType)
	{
		if (bucketType >= GameRewardBucketType.FullVsHumanRewards)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((int)bucketType < GameRewardBuckets.Length)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return GameRewardBuckets[(int)bucketType].XPBonusPerQueueTimeMinuteCap;
					}
				}
			}
		}
		return 0f;
	}

	public int GetXPBonusForMatchTime(GameRewardBucketType bucketType, TimeSpan matchDuration)
	{
		return GetXPBonusForMatchTime(bucketType, (float)matchDuration.TotalMinutes);
	}

	public int GetXPBonusForMatchTime(GameRewardBucketType bucketType, float totalMinutes)
	{
		if (totalMinutes < 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			totalMinutes = 0f;
		}
		float val = GetXPBonusPerMinute(bucketType) * totalMinutes;
		val = Math.Min(val, GetXPBonusPerMinuteCap(bucketType));
		return (int)val;
	}

	public int GetXPBonusForQueueTime(GameRewardBucketType bucketType, TimeSpan matchDuration)
	{
		return GetXPBonusForQueueTime(bucketType, (float)matchDuration.TotalMinutes);
	}

	public int GetXPBonusForQueueTime(GameRewardBucketType bucketType, float totalMinutes)
	{
		if (totalMinutes < 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			totalMinutes = 0f;
		}
		float val = GetXPBonusPerQueueTimeMinute(bucketType) * totalMinutes;
		val = Math.Min(val, GetXPBonusPerQueueTimeMinuteCap(bucketType));
		return (int)val;
	}

	public void OnValidate()
	{
		EnsureUniqueIDs(PlayerTitles);
		EnsureUniqueIDs(PlayerBanners);
		EnsureUniqueIDs(PlayerRibbons);
		EnsureUniqueIDs(ChatEmojis);
		EnsureUniqueIDs(Overcons);
		EnsureUniqueIDs(StoreItemsForPurchase);
	}

	public static void EnsureUniqueIDs<T>(T[] arrayOfIDs) where T : IUniqueID
	{
		if (arrayOfIDs == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (arrayOfIDs.Length <= 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < arrayOfIDs.Length; i++)
			{
				if (arrayOfIDs[i].GetID() > num)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num = arrayOfIDs[i].GetID();
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				for (int j = 0; j < arrayOfIDs.Length; j++)
				{
					if (arrayOfIDs[j].GetID() == 0)
					{
						arrayOfIDs[j].SetID(++num);
					}
					for (int k = j + 1; k < arrayOfIDs.Length; k++)
					{
						if (arrayOfIDs[k].GetID() == arrayOfIDs[j].GetID())
						{
							arrayOfIDs[k].SetID(++num);
							continue;
						}
						j = k - 1;
						break;
					}
				}
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}
