using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class PersistedAccountData : ICloneable
{
	public long AccountId
	{
		get;
		set;
	}

	public string UserName
	{
		get;
		set;
	}

	public string Handle
	{
		get;
		set;
	}

	public SchemaVersion<AccountSchemaChange> SchemaVersion
	{
		get;
		set;
	}

	public DateTime CreateDate
	{
		get;
		set;
	}

	public DateTime UpdateDate
	{
		get;
		set;
	}

	public AccountComponent AccountComponent
	{
		get;
		set;
	}

	public AdminComponent AdminComponent
	{
		get;
		set;
	}

	public BankComponent BankComponent
	{
		get;
		set;
	}

	public ExperienceComponent ExperienceComponent
	{
		get;
		set;
	}

	public InventoryComponent InventoryComponent
	{
		get;
		set;
	}

	public QuestComponent QuestComponent
	{
		get;
		set;
	}

	public SocialComponent SocialComponent
	{
		get;
		set;
	}

	public Dictionary<CharacterType, PersistedCharacterData> CharacterData
	{
		get;
		set;
	}

	private List<PersistedCharacterMatchData> AddedMatchData
	{
		get;
		set;
	}

	[JsonIgnore]
	public string PersistedUserName
	{
		get;
		set;
	}

	[JsonIgnore]
	public string PersistedHandle
	{
		get;
		set;
	}

	[JsonIgnore]
	public bool Snapshotting
	{
		get;
		set;
	}

	[JsonIgnore]
	public string SnapshottingUserName
	{
		get;
		set;
	}

	[JsonIgnore]
	public PersistedAccountDataSnapshotReason SnapshotReason
	{
		get;
		set;
	}

	[JsonIgnore]
	public string SnapshotNote
	{
		get;
		set;
	}

	[JsonIgnore]
	public int SeasonLevel
	{
		get
		{
			return QuestComponent.SeasonLevel;
		}
		set
		{
			QuestComponent.SeasonLevel = value;
		}
	}

	[JsonIgnore]
	public int SeasonXPProgressThroughLevel
	{
		get
		{
			return QuestComponent.SeasonXPProgressThroughLevel;
		}
		set
		{
			QuestComponent.SeasonXPProgressThroughLevel = value;
		}
	}

	[JsonIgnore]
	public int HighestSeasonChapter
	{
		get
		{
			return QuestComponent.HighestSeasonChapter;
		}
		set
		{
			QuestComponent.HighestSeasonChapter = value;
		}
	}

	public PersistedAccountData()
	{
		SchemaVersion = new SchemaVersion<AccountSchemaChange>();
		AccountComponent = new AccountComponent();
		AdminComponent = new AdminComponent();
		BankComponent = new BankComponent(new List<CurrencyData>());
		ExperienceComponent = new ExperienceComponent();
		ExperienceComponent.Level = 0;
		InventoryComponent = new InventoryComponent();
		SocialComponent = new SocialComponent();
		CharacterData = new Dictionary<CharacterType, PersistedCharacterData>();
		AddedMatchData = new List<PersistedCharacterMatchData>();
		QuestComponent = new QuestComponent();
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public PersistedAccountData CloneForClient()
	{
		PersistedAccountData persistedAccountData = new PersistedAccountData();
		persistedAccountData.AccountId = AccountId;
		persistedAccountData.UserName = UserName;
		persistedAccountData.Handle = Handle;
		persistedAccountData.SchemaVersion = SchemaVersion;
		persistedAccountData.CreateDate = CreateDate;
		persistedAccountData.UpdateDate = UpdateDate;
		persistedAccountData.AccountComponent = AccountComponent;
		persistedAccountData.AdminComponent = AdminComponent.CloneForClient();
		persistedAccountData.BankComponent = BankComponent;
		persistedAccountData.ExperienceComponent = ExperienceComponent;
		persistedAccountData.InventoryComponent = InventoryComponent.CloneForClient();
		persistedAccountData.SocialComponent = SocialComponent;
		persistedAccountData.QuestComponent = QuestComponent;
		return persistedAccountData;
	}

	public PersistedAccountData CloneForAdminClient()
	{
		PersistedAccountData persistedAccountData = new PersistedAccountData();
		persistedAccountData.AccountId = AccountId;
		persistedAccountData.UserName = UserName;
		persistedAccountData.Handle = Handle;
		persistedAccountData.SchemaVersion = SchemaVersion;
		persistedAccountData.CreateDate = CreateDate;
		persistedAccountData.UpdateDate = UpdateDate;
		persistedAccountData.AccountComponent = AccountComponent;
		persistedAccountData.AdminComponent = AdminComponent;
		persistedAccountData.BankComponent = BankComponent;
		persistedAccountData.ExperienceComponent = ExperienceComponent;
		persistedAccountData.InventoryComponent = InventoryComponent;
		persistedAccountData.SocialComponent = SocialComponent;
		persistedAccountData.QuestComponent = QuestComponent;
		return persistedAccountData;
	}

	public PersistedAccountData CloneForSearchResult()
	{
		PersistedAccountData persistedAccountData = new PersistedAccountData();
		persistedAccountData.AccountId = AccountId;
		persistedAccountData.UserName = UserName;
		persistedAccountData.Handle = Handle;
		persistedAccountData.SchemaVersion = SchemaVersion;
		persistedAccountData.CreateDate = CreateDate;
		persistedAccountData.UpdateDate = UpdateDate;
		persistedAccountData.AccountComponent = AccountComponent;
		persistedAccountData.AdminComponent = AdminComponent;
		persistedAccountData.ExperienceComponent = ExperienceComponent;
		return persistedAccountData;
	}

	public void AddMatchData(PersistedCharacterMatchData matchData)
	{
		AddedMatchData.Add(matchData);
	}

	public IEnumerable<PersistedCharacterMatchData> GetAddedMatchData()
	{
		return AddedMatchData;
	}

	public void ClearAddedMatchData()
	{
		AddedMatchData.Clear();
	}

	public int GetReactorLevel(List<SeasonTemplate> seasons)
	{
		return QuestComponent.GetReactorLevel(seasons);
	}

	public override string ToString()
	{
		return new StringBuilder().Append("[").Append(AccountId).Append("] ").Append(Handle).ToString();
	}
}
