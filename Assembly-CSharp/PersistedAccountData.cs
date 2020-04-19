using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class PersistedAccountData : ICloneable
{
	public PersistedAccountData()
	{
		this.SchemaVersion = new SchemaVersion<AccountSchemaChange>();
		this.AccountComponent = new AccountComponent();
		this.AdminComponent = new AdminComponent();
		this.BankComponent = new BankComponent(new List<CurrencyData>());
		this.ExperienceComponent = new ExperienceComponent();
		this.ExperienceComponent.Level = 0;
		this.InventoryComponent = new InventoryComponent();
		this.SocialComponent = new SocialComponent();
		this.CharacterData = new Dictionary<CharacterType, PersistedCharacterData>();
		this.AddedMatchData = new List<PersistedCharacterMatchData>();
		this.QuestComponent = new QuestComponent();
	}

	public long AccountId { get; set; }

	public string UserName { get; set; }

	public string Handle { get; set; }

	public SchemaVersion<AccountSchemaChange> SchemaVersion { get; set; }

	public DateTime CreateDate { get; set; }

	public DateTime UpdateDate { get; set; }

	public AccountComponent AccountComponent { get; set; }

	public AdminComponent AdminComponent { get; set; }

	public BankComponent BankComponent { get; set; }

	public ExperienceComponent ExperienceComponent { get; set; }

	public InventoryComponent InventoryComponent { get; set; }

	public QuestComponent QuestComponent { get; set; }

	public SocialComponent SocialComponent { get; set; }

	public Dictionary<CharacterType, PersistedCharacterData> CharacterData { get; set; }

	private List<PersistedCharacterMatchData> AddedMatchData { get; set; }

	[JsonIgnore]
	public string PersistedUserName { get; set; }

	[JsonIgnore]
	public string PersistedHandle { get; set; }

	[JsonIgnore]
	public bool Snapshotting { get; set; }

	[JsonIgnore]
	public string SnapshottingUserName { get; set; }

	[JsonIgnore]
	public PersistedAccountDataSnapshotReason SnapshotReason { get; set; }

	[JsonIgnore]
	public string SnapshotNote { get; set; }

	[JsonIgnore]
	public int SeasonLevel
	{
		get
		{
			return this.QuestComponent.SeasonLevel;
		}
		set
		{
			this.QuestComponent.SeasonLevel = value;
		}
	}

	[JsonIgnore]
	public int SeasonXPProgressThroughLevel
	{
		get
		{
			return this.QuestComponent.SeasonXPProgressThroughLevel;
		}
		set
		{
			this.QuestComponent.SeasonXPProgressThroughLevel = value;
		}
	}

	[JsonIgnore]
	public int HighestSeasonChapter
	{
		get
		{
			return this.QuestComponent.HighestSeasonChapter;
		}
		set
		{
			this.QuestComponent.HighestSeasonChapter = value;
		}
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public PersistedAccountData CloneForClient()
	{
		return new PersistedAccountData
		{
			AccountId = this.AccountId,
			UserName = this.UserName,
			Handle = this.Handle,
			SchemaVersion = this.SchemaVersion,
			CreateDate = this.CreateDate,
			UpdateDate = this.UpdateDate,
			AccountComponent = this.AccountComponent,
			AdminComponent = this.AdminComponent.CloneForClient(),
			BankComponent = this.BankComponent,
			ExperienceComponent = this.ExperienceComponent,
			InventoryComponent = this.InventoryComponent.CloneForClient(),
			SocialComponent = this.SocialComponent,
			QuestComponent = this.QuestComponent
		};
	}

	public PersistedAccountData CloneForAdminClient()
	{
		return new PersistedAccountData
		{
			AccountId = this.AccountId,
			UserName = this.UserName,
			Handle = this.Handle,
			SchemaVersion = this.SchemaVersion,
			CreateDate = this.CreateDate,
			UpdateDate = this.UpdateDate,
			AccountComponent = this.AccountComponent,
			AdminComponent = this.AdminComponent,
			BankComponent = this.BankComponent,
			ExperienceComponent = this.ExperienceComponent,
			InventoryComponent = this.InventoryComponent,
			SocialComponent = this.SocialComponent,
			QuestComponent = this.QuestComponent
		};
	}

	public PersistedAccountData CloneForSearchResult()
	{
		return new PersistedAccountData
		{
			AccountId = this.AccountId,
			UserName = this.UserName,
			Handle = this.Handle,
			SchemaVersion = this.SchemaVersion,
			CreateDate = this.CreateDate,
			UpdateDate = this.UpdateDate,
			AccountComponent = this.AccountComponent,
			AdminComponent = this.AdminComponent,
			ExperienceComponent = this.ExperienceComponent
		};
	}

	public void AddMatchData(PersistedCharacterMatchData matchData)
	{
		this.AddedMatchData.Add(matchData);
	}

	public IEnumerable<PersistedCharacterMatchData> GetAddedMatchData()
	{
		return this.AddedMatchData;
	}

	public void ClearAddedMatchData()
	{
		this.AddedMatchData.Clear();
	}

	public int GetReactorLevel(List<SeasonTemplate> seasons)
	{
		return this.QuestComponent.GetReactorLevel(seasons);
	}

	public override string ToString()
	{
		return string.Format("[{0}] {1}", this.AccountId, this.Handle);
	}
}
