﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class LobbyGameplayData
{
	private static LobbyGameplayData s_instance;

	public Dictionary<CharacterType, LobbyCharacterGameplayData> CharacterData;

	public Dictionary<CardType, LobbyCardGameplayData> CardData;

	public GameBalanceVars GameBalanceVars;

	public BannedWords BannedWords;

	public LobbyLootMatrixPackData LootMatrixPackData;

	public LobbyGamePackData GamePackData;

	public LobbyGGPackData GGPackData;

	public LobbyInventoryData InventoryData;

	public LobbyStoreData StoreData;

	public LobbyQuestData QuestData;

	public LobbySeasonData SeasonData;

	public LobbyFactionData FactionData;

	public LobbyGameplayData()
	{
		this.CharacterData = new Dictionary<CharacterType, LobbyCharacterGameplayData>();
		this.CardData = new Dictionary<CardType, LobbyCardGameplayData>();
		this.GameBalanceVars = new GameBalanceVars();
		this.BannedWords = new BannedWords();
		this.LootMatrixPackData = new LobbyLootMatrixPackData();
		this.GamePackData = new LobbyGamePackData();
		this.GGPackData = new LobbyGGPackData();
		this.InventoryData = new LobbyInventoryData();
		this.StoreData = new LobbyStoreData();
		this.QuestData = new LobbyQuestData();
		this.SeasonData = new LobbySeasonData();
		this.FactionData = new LobbyFactionData();
	}

	public static LobbyGameplayData Get()
	{
		return LobbyGameplayData.s_instance;
	}

	public static void Set(LobbyGameplayData instance)
	{
		LobbyGameplayData.s_instance = instance;
	}

	public CharacterModInfo GetDefaultAbilityMods(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = this.GetCharacterData(characterType);
		CharacterModInfo result;
		if (characterData != null)
		{
			result = characterData.GetDefaultModInfo();
		}
		else
		{
			result = default(CharacterModInfo);
		}
		return result;
	}

	public LobbyCharacterGameplayData GetCharacterData(CharacterType characterType)
	{
		LobbyCharacterGameplayData result = null;
		this.CharacterData.TryGetValue(characterType, out result);
		return result;
	}

	public string GetCharacterDisplayName(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = this.GetCharacterData(characterType);
		string result;
		if (characterData != null)
		{
			result = characterData.DisplayName;
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	public CharacterConfig GetDefaultCharacterConfig(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = this.GetCharacterData(characterType);
		CharacterConfig characterConfig;
		if (characterData != null)
		{
			characterConfig = characterData.CharacterConfig;
		}
		else
		{
			characterConfig = new CharacterConfig();
			characterConfig.CharacterType = characterType;
			characterConfig.IsHidden = true;
			characterConfig.IsHiddenFromFreeRotationUntil = DateTime.MaxValue;
		}
		return characterConfig;
	}

	public LobbyCardGameplayData GetCardData(CardType cardType)
	{
		LobbyCardGameplayData result = null;
		this.CardData.TryGetValue(cardType, out result);
		return result;
	}

	public CharacterCardInfo GetDefaultCardInfo()
	{
		CharacterCardInfo result = default(CharacterCardInfo);
		IEnumerable<LobbyCardGameplayData> values = this.CardData.Values;
		
		result.PrepCard = values.SingleOrDefault(delegate(LobbyCardGameplayData c)
			{
				bool result2;
				if (c.RunPhase == AbilityRunPhase.Prep)
				{
					result2 = c.IsDefault;
				}
				else
				{
					result2 = false;
				}
				return result2;
			}).CardType;
		IEnumerable<LobbyCardGameplayData> values2 = this.CardData.Values;
		
		result.CombatCard = values2.SingleOrDefault(delegate(LobbyCardGameplayData c)
			{
				bool result2;
				if (c.RunPhase == AbilityRunPhase.Combat)
				{
					result2 = c.IsDefault;
				}
				else
				{
					result2 = false;
				}
				return result2;
			}).CardType;
		result.DashCard = this.CardData.Values.SingleOrDefault(delegate(LobbyCardGameplayData c)
		{
			bool result2;
			if (c.RunPhase == AbilityRunPhase.Dash)
			{
				result2 = c.IsDefault;
			}
			else
			{
				result2 = false;
			}
			return result2;
		}).CardType;
		return result;
	}

	public CardType GetDefaultCardType(AbilityRunPhase runPhase)
	{
		return this.CardData.Values.SingleOrDefault(delegate(LobbyCardGameplayData c)
		{
			bool result;
			if (c.RunPhase == runPhase)
			{
				result = c.IsDefault;
			}
			else
			{
				result = false;
			}
			return result;
		}).CardType;
	}

	public LobbyInventoryItemTemplate GetItemTemplate(int itemTemplateId)
	{
		LobbyInventoryItemTemplate lobbyInventoryItemTemplate = null;
		this.InventoryData.ItemTemplates.TryGetValue(itemTemplateId, out lobbyInventoryItemTemplate);
		if (lobbyInventoryItemTemplate == null)
		{
			throw new ArgumentException(string.Format("Invalid itemTemplateId {0}", itemTemplateId));
		}
		return lobbyInventoryItemTemplate;
	}

	public KarmaTemplate GetKarmaTemplate(int karmaTemplateId)
	{
		KarmaTemplate karmaTemplate = (from x in this.InventoryData.KarmaTemplates
		where x.Index == karmaTemplateId
		select x).Single<KarmaTemplate>();
		if (karmaTemplate != null)
		{
			if (karmaTemplate.IsValid())
			{
				return karmaTemplate;
			}
		}
		throw new InvalidOperationException(string.Format("Invalid karmaTemplateId={0}", karmaTemplateId));
	}

	public LootTable GetLootTable(int lootTableId)
	{
		LootTable lootTable = (from x in this.InventoryData.LootTables
		where x.Index == lootTableId
		select x).Single<LootTable>();
		if (lootTable != null)
		{
			if (lootTable.IsValid())
			{
				return lootTable;
			}
		}
		throw new InvalidOperationException(string.Format("Invalid lootTableId={0}", lootTableId));
	}

	public int GetDefaultItemValue()
	{
		return this.InventoryData.DefaultItemValue;
	}

	public int GetCharacterItemDropBalance()
	{
		return this.InventoryData.CharacterItemDropBalance;
	}

	public FactionCompetition GetFactionCompetition(int competitionId)
	{
		if (0 < competitionId)
		{
			if (competitionId <= this.FactionData.m_factionCompetitions.Count)
			{
				return this.FactionData.m_factionCompetitions[competitionId - 1];
			}
		}
		throw new ArgumentException(string.Format("Invalid Faction competitionId={0}", competitionId));
	}

	public FactionGroup GetFactionGroup(int factionID)
	{
		for (int i = 0; i < this.FactionData.m_factionGroups.Count; i++)
		{
			if (this.FactionData.m_factionGroups[i].FactionGroupID == factionID)
			{
				return this.FactionData.m_factionGroups[i];
			}
		}
		return new FactionGroup();
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		if (0 < competitionId)
		{
			if (competitionId <= this.FactionData.m_factionCompetitions.Count && 0 <= factionId)
			{
				if (factionId < this.FactionData.m_factionCompetitions[competitionId - 1].Factions.Count)
				{
					return this.FactionData.m_factionCompetitions[competitionId - 1].Factions[factionId];
				}
			}
		}
		throw new ArgumentException(string.Format("Invalid Faction competitionId={0} factionId={1}", competitionId, factionId));
	}

	public virtual void LoadFromFile(string dirName)
	{
		string fileName = Path.Combine(dirName, "LobbyCharacterData.json");
		this.LoadFromFile(fileName, this.CharacterData);
		string fileName2 = Path.Combine(dirName, "LobbyCardData.json");
		this.LoadFromFile(fileName2, this.CardData);
		string fileName3 = Path.Combine(dirName, "GameBalanceVars.json");
		this.LoadFromFile(fileName3, this.GameBalanceVars);
		string fileName4 = Path.Combine(dirName, "BannedWords.json");
		this.LoadFromFile(fileName4, this.BannedWords);
		string fileName5 = Path.Combine(dirName, "LootMatrixPackData.json");
		this.LoadFromFile(fileName5, this.LootMatrixPackData);
		string fileName6 = Path.Combine(dirName, "GamePackData.json");
		this.LoadFromFile(fileName6, this.GamePackData);
		string fileName7 = Path.Combine(dirName, "GGPackData.json");
		this.LoadFromFile(fileName7, this.GGPackData);
		string fileName8 = Path.Combine(dirName, "LobbyInventoryData.json");
		this.LoadFromFile(fileName8, this.InventoryData);
		string fileName9 = Path.Combine(dirName, "LobbyStoreData.json");
		this.LoadFromFile(fileName9, this.StoreData);
		string fileName10 = Path.Combine(dirName, "LobbyQuestData.json");
		this.LoadFromFile(fileName10, this.QuestData);
		string fileName11 = Path.Combine(dirName, "LobbySeasonData.json");
		this.LoadFromFile(fileName11, this.SeasonData);
		string fileName12 = Path.Combine(dirName, "LobbyFactionData.json");
		this.LoadFromFile(fileName12, this.FactionData);
	}

	public virtual void SaveToFile(string dirName)
	{
		string fileName = Path.Combine(dirName, "LobbyCharacterData.json");
		this.SaveToFile(fileName, this.CharacterData);
		string fileName2 = Path.Combine(dirName, "LobbyCardData.json");
		this.SaveToFile(fileName2, this.CardData);
		string fileName3 = Path.Combine(dirName, "GameBalanceVars.json");
		this.SaveToFile(fileName3, this.GameBalanceVars);
		string fileName4 = Path.Combine(dirName, "BannedWords.json");
		this.SaveToFile(fileName4, this.BannedWords);
		string fileName5 = Path.Combine(dirName, "LootMatrixPackData.json");
		this.SaveToFile(fileName5, this.LootMatrixPackData);
		string fileName6 = Path.Combine(dirName, "GamePackData.json");
		this.SaveToFile(fileName6, this.GamePackData);
		string fileName7 = Path.Combine(dirName, "GGPackData.json");
		this.SaveToFile(fileName7, this.GGPackData);
		string fileName8 = Path.Combine(dirName, "LobbyInventoryData.json");
		this.SaveToFile(fileName8, this.InventoryData);
		string fileName9 = Path.Combine(dirName, "LobbyStoreData.json");
		this.SaveToFile(fileName9, this.StoreData);
		string fileName10 = Path.Combine(dirName, "LobbyQuestData.json");
		this.SaveToFile(fileName10, this.QuestData);
		string fileName11 = Path.Combine(dirName, "LobbySeasonData.json");
		this.SaveToFile(fileName11, this.SeasonData);
		string fileName12 = Path.Combine(dirName, "LobbyFactionData.json");
		this.SaveToFile(fileName12, this.FactionData);
	}

	public virtual void LoadFromFile(string fileName, object obj)
	{
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Exists)
		{
			return;
		}
		string value = File.ReadAllText(fileName);
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			ObjectCreationHandling = ObjectCreationHandling.Replace
		};
		JsonConvert.PopulateObject(value, obj, settings);
	}

	public virtual void SaveToFile(string fileName, object obj)
	{
		string contents = JsonConvert.SerializeObject(obj, Formatting.Indented);
		File.WriteAllText(fileName, contents);
	}

	[JsonIgnore]
	public IFreelancerSetQueryInterface FreelancerSetQueryInterface
	{
		get
		{
			return new LobbyGameplayFreelancerSetQueryInterface(this);
		}
	}
}
