using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ActorBehavior : NetworkBehaviour, StatDisplaySettings.IPersistatedStatValueSupplier
{
	private SyncListUInt m_syncEnemySourcesForDamageOrDebuff = new SyncListUInt();
	private SyncListUInt m_syncAllySourcesForHealAndBuff = new SyncListUInt();

	[SyncVar]
	private short m_totalDeaths;
	[SyncVar]
	private short m_totalPlayerKills;
	[SyncVar]
	private short m_totalPlayerAssists;

	private short m_totalMinionKills;

	[SyncVar]
	private int m_totalPlayerDamage;
	[SyncVar]
	private int m_totalPlayerHealing;
	[SyncVar]
	private int m_totalPlayerHealingFromAbility;
	[SyncVar]
	private int m_totalPlayerOverheal;
	[SyncVar]
	private int m_totalPlayerAbsorb;
	[SyncVar]
	private int m_totalPlayerPotentialAbsorb;
	[SyncVar]
	private int m_totalEnergyGained;
	[SyncVar]
	private int m_totalPlayerDamageReceived;
	[SyncVar]
	private int m_totalPlayerHealingReceived;
	[SyncVar]
	private int m_totalPlayerAbsorbReceived;
	[SyncVar]
	private float m_totalPlayerLockInTime;
	[SyncVar]
	private int m_totalPlayerTurns;
	[SyncVar]
	private int m_damageDodgedByEvades;
	[SyncVar]
	private int m_damageInterceptedByEvades;
	[SyncVar]
	private int m_myIncomingDamageReducedByCover;
	[SyncVar]
	private int m_myOutgoingDamageReducedByCover;
	[SyncVar]
	private int m_myIncomingOverkillDamageTaken;
	[SyncVar]
	private int m_myOutgoingOverkillDamageDealt;
	[SyncVar]
	private int m_myOutgoingExtraDamageFromEmpowered;
	[SyncVar]
	private int m_myOutgoingDamageReducedFromWeakened;

	private int m_myOutgoingExtraDamageFromTargetsVulnerable;
	private int m_myOutgoingDamageReducedFromTargetsArmored;
	private int m_myIncomingExtraDamageFromCastersEmpowered;
	private int m_myIncomingDamageReducedFromCastersWeakened;
	private int m_myIncomingExtraDamageIncreasedByVulnerable;
	private int m_myIncomingDamageReducedByArmored;

	[SyncVar]
	private int m_teamOutgoingDamageIncreasedByEmpoweredFromMe;
	[SyncVar]
	private int m_teamIncomingDamageReducedByWeakenedFromMe;

	private int m_teamIncomingDamageReducedByArmoredFromMe;
	private int m_teamOutgoingDamageIncreasedByVulnerableFromMe;

	[SyncVar]
	private int m_teamExtraEnergyGainFromMe;
	[SyncVar]
	private float m_movementDeniedByMe;
	[SyncVar]
	private int m_totalEnemySighted;

	private List<int> m_clientEffectSourceActors = new List<int>();
	private List<int> m_clientDamageSourceActors = new List<int>();
	private List<int> m_clientHealSourceActors = new List<int>();

	private ActorData m_actor;
	private int m_totalDeathsOnTurnStart;
	private int m_serverIncomingDamageReducedByCoverThisTurn;

	public const string c_debugHeader = "<color=magenta>ActorBehavior: </color>";

	private static int kListm_syncEnemySourcesForDamageOrDebuff = -1894796663;
	private static int kListm_syncAllySourcesForHealAndBuff = 1807084515;

	public int totalDeaths
	{
		get { return m_totalDeaths; }
	}

	public int totalPlayerKills
	{
		get { return m_totalPlayerKills; }
	}

	public int totalPlayerAssists
	{
		get { return m_totalPlayerAssists; }
	}

	public int totalMinionKills
	{
		get { return m_totalMinionKills; }
	}

	public int totalPlayerContribution
	{
		get { return m_totalPlayerDamage + EffectiveHealingFromAbility + m_totalPlayerAbsorb; }
	}

	public int totalPlayerDamage
	{
		get { return m_totalPlayerDamage; }
	}

	public int totalPlayerHealing
	{
		get { return m_totalPlayerHealing; }
	}

	public int totalPlayerHealingFromAbility
	{
		get { return m_totalPlayerHealingFromAbility; }
	}

	public int totalPlayerOverheal
	{
		get { return m_totalPlayerOverheal; }
	}

	public int totalPlayerAbsorb
	{
		get { return m_totalPlayerAbsorb; }
	}

	public int totalPlayerPotentialAbsorb
	{
		get { return m_totalPlayerPotentialAbsorb; }
	}

	public int totalEnergyGained
	{
		get { return m_totalEnergyGained; }
	}

	public int totalPlayerDamageReceived
	{
		get { return m_totalPlayerDamageReceived; }
	}

	public int totalPlayerHealingReceived
	{
		get { return m_totalPlayerHealingReceived; }
	}

	public int totalPlayerAbsorbReceived
	{
		get { return m_totalPlayerAbsorbReceived; }
	}

	public float totalPlayerLockInTime
	{
		get { return m_totalPlayerLockInTime; }
	}

	public int totalPlayerTurns
	{
		get { return m_totalPlayerTurns; }
	}

	public int netDamageAvoidedByEvades
	{
		get { return Mathf.Max(0, m_damageDodgedByEvades - m_damageInterceptedByEvades); }
	}

	public int damageDodgedByEvades
	{
		get { return m_damageDodgedByEvades; }
	}

	public int damageInterceptedByEvades
	{
		get { return m_damageInterceptedByEvades; }
	}

	public int myIncomingDamageReducedByCover
	{
		get { return m_myIncomingDamageReducedByCover; }
	}

	public int myOutgoingDamageReducedByCover
	{
		get { return m_myOutgoingDamageReducedByCover; }
	}

	public int myIncomingOverkillDamageTaken
	{
		get { return m_myIncomingOverkillDamageTaken; }
	}

	public int myOutgoingOverkillDamageDealt
	{
		get { return m_myOutgoingOverkillDamageDealt; }
	}

	public int myOutgoingExtraDamageFromEmpowered
	{
		get { return m_myOutgoingExtraDamageFromEmpowered; }
	}

	public int myOutgoingReducedDamageFromWeakened
	{
		get { return m_myOutgoingDamageReducedFromWeakened; }
	}

	public int myOutgoingExtraDamageFromTargetsVulnerable
	{
		get { return m_myOutgoingExtraDamageFromTargetsVulnerable; }
	}

	public int myOutgoingReducedDamageFromTargetsArmored
	{
		get { return m_myOutgoingDamageReducedFromTargetsArmored; }
	}

	public int myIncomingDamageReducedByArmored
	{
		get { return m_myIncomingDamageReducedByArmored; }
	}

	public int myIncomingExtraDamageIncreasedByVulnerable
	{
		get { return m_myIncomingExtraDamageIncreasedByVulnerable; }
	}

	public int teamOutgoingDamageIncreasedByEmpoweredFromMe
	{
		get { return m_teamOutgoingDamageIncreasedByEmpoweredFromMe; }
	}

	public int teamIncomingDamageReducedByWeakenedFromMe
	{
		get { return m_teamIncomingDamageReducedByWeakenedFromMe; }
	}

	public int teamIncomingDamageReducedByArmoredFromMe
	{
		get { return m_teamIncomingDamageReducedByArmoredFromMe; }
	}

	public int teamOutgoingDamageIncreasedByVulnerableFromMe
	{
		get { return m_teamOutgoingDamageIncreasedByVulnerableFromMe; }
	}

	public int teamExtraEnergyGainFromMe
	{
		get { return m_teamExtraEnergyGainFromMe; }
	}

	public float movementDeniedByMe
	{
		get { return m_movementDeniedByMe; }
	}

	public int totalEnemySighted
	{
		get { return m_totalEnemySighted; }
	}

	private float NumTurnsForStatCalc
	{
		get { return Mathf.Max(1f, m_totalPlayerTurns); }
	}

	private float NumLifeForStatCalc
	{
		get { return Mathf.Max(1f, m_totalDeaths + 1f); }
	}

	public float EnergyGainPerTurn
	{
		get { return m_totalEnergyGained / NumTurnsForStatCalc; }
	}

	public float DamagePerTurn
	{
		get { return m_totalPlayerDamage / NumTurnsForStatCalc; }
	}

	public float NumEnemiesSightedPerTurn
	{
		get { return m_totalEnemySighted / NumTurnsForStatCalc; }
	}

	public float HealAndAbsorbPerTurn
	{
		get { return (EffectiveHealingFromAbility + m_totalPlayerAbsorb) / NumTurnsForStatCalc; }
	}

	public float MovementDeniedPerTurn
	{
		get { return m_movementDeniedByMe / NumTurnsForStatCalc; }
	}

	public float TeamEnergyBoostedByMePerTurn
	{
		get { return teamExtraEnergyGainFromMe / NumTurnsForStatCalc; }
	}

	public float TeamDamageSwingPerTurn
	{
		get { return (m_teamOutgoingDamageIncreasedByEmpoweredFromMe + m_teamIncomingDamageReducedByWeakenedFromMe) / NumTurnsForStatCalc; }
	}

	public int NetBoostedOutgoingDamage
	{
		get { return m_myOutgoingExtraDamageFromEmpowered - m_myOutgoingDamageReducedFromWeakened; }
	}

	public float DamageEfficiency
	{
		get
		{
			int num = m_totalPlayerDamage + m_myOutgoingDamageReducedByCover;
			if (num <= 0)
			{
				return 0f;
			}
			float num2 = m_totalPlayerDamage - m_myOutgoingOverkillDamageDealt;
			return Mathf.Clamp(num2 / num, 0f, 1f);
		}
	}

	public float KillParticipation
	{
		get
		{
			int deathCountOfTeam = GameFlowData.Get().GetDeathCountOfTeam(m_actor.GetEnemyTeam());
			if (deathCountOfTeam <= 0)
			{
				return 0f;
			}
			return m_totalPlayerAssists / (float)deathCountOfTeam;
		}
	}

	public int EffectiveHealing
	{
		get { return Mathf.Max(0, m_totalPlayerHealing - m_totalPlayerOverheal); }
	}

	public int EffectiveHealingFromAbility
	{
		get { return Mathf.Max(0, m_totalPlayerHealingFromAbility - m_totalPlayerOverheal); }
	}

	public float NetDamageDodgedPerLife
	{
		get { return netDamageAvoidedByEvades / NumLifeForStatCalc; }
	}

	public float IncomingDamageReducedByCoverPerLife
	{
		get { return m_myIncomingDamageReducedByCover / NumLifeForStatCalc; }
	}

	public float DamageTakenPerLife
	{
		get { return m_totalPlayerDamageReceived / NumLifeForStatCalc; }
	}

	public float AvgLifeSpan
	{
		get { return NumTurnsForStatCalc / NumLifeForStatCalc; }
	}

	public CharacterType? CharacterType
	{
		get
		{
			if (m_actor != null)
			{
				return m_actor.m_characterType;
			}
			return null;
		}
	}

	public CharacterRole? CharacterRole
	{
		get
		{
			CharacterType? characterType = CharacterType;
			if (characterType.HasValue)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType.Value);
				if (characterResourceLink != null)
				{
					return characterResourceLink.m_characterRole;
				}
			}
			return null;
		}
	}

	public int totalDeathsOnTurnStart
	{
		get { return m_totalDeathsOnTurnStart; }
	}

	public int serverIncomingDamageReducedByCoverThisTurn
	{
		get { return m_serverIncomingDamageReducedByCoverThisTurn; }
	}

	// TODO
	public bool DebugTraceClientContribution
	{
		get
		{
			if (Application.isEditor)
			{
				return false;
			}
			return false;
		}
	}

	public short Networkm_totalDeaths
	{
		get
		{
			return m_totalDeaths;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalDeaths, 4u);
		}
	}

	public short Networkm_totalPlayerKills
	{
		get
		{
			return m_totalPlayerKills;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerKills, 8u);
		}
	}

	public short Networkm_totalPlayerAssists
	{
		get
		{
			return m_totalPlayerAssists;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerAssists, 16u);
		}
	}

	public int Networkm_totalPlayerDamage
	{
		get
		{
			return m_totalPlayerDamage;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerDamage, 32u);
		}
	}

	public int Networkm_totalPlayerHealing
	{
		get
		{
			return m_totalPlayerHealing;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerHealing, 64u);
		}
	}

	public int Networkm_totalPlayerHealingFromAbility
	{
		get
		{
			return m_totalPlayerHealingFromAbility;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerHealingFromAbility, 128u);
		}
	}

	public int Networkm_totalPlayerOverheal
	{
		get
		{
			return m_totalPlayerOverheal;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerOverheal, 256u);
		}
	}

	public int Networkm_totalPlayerAbsorb
	{
		get
		{
			return m_totalPlayerAbsorb;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerAbsorb, 512u);
		}
	}

	public int Networkm_totalPlayerPotentialAbsorb
	{
		get
		{
			return m_totalPlayerPotentialAbsorb;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerPotentialAbsorb, 1024u);
		}
	}

	public int Networkm_totalEnergyGained
	{
		get
		{
			return m_totalEnergyGained;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalEnergyGained, 2048u);
		}
	}

	public int Networkm_totalPlayerDamageReceived
	{
		get
		{
			return m_totalPlayerDamageReceived;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerDamageReceived, 4096u);
		}
	}

	public int Networkm_totalPlayerHealingReceived
	{
		get
		{
			return m_totalPlayerHealingReceived;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerHealingReceived, 8192u);
		}
	}

	public int Networkm_totalPlayerAbsorbReceived
	{
		get
		{
			return m_totalPlayerAbsorbReceived;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerAbsorbReceived, 16384u);
		}
	}

	public float Networkm_totalPlayerLockInTime
	{
		get
		{
			return m_totalPlayerLockInTime;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerLockInTime, 32768u);
		}
	}

	public int Networkm_totalPlayerTurns
	{
		get
		{
			return m_totalPlayerTurns;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalPlayerTurns, 65536u);
		}
	}

	public int Networkm_damageDodgedByEvades
	{
		get
		{
			return m_damageDodgedByEvades;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageDodgedByEvades, 131072u);
		}
	}

	public int Networkm_damageInterceptedByEvades
	{
		get
		{
			return m_damageInterceptedByEvades;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageInterceptedByEvades, 262144u);
		}
	}

	public int Networkm_myIncomingDamageReducedByCover
	{
		get
		{
			return m_myIncomingDamageReducedByCover;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myIncomingDamageReducedByCover, 524288u);
		}
	}

	public int Networkm_myOutgoingDamageReducedByCover
	{
		get
		{
			return m_myOutgoingDamageReducedByCover;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myOutgoingDamageReducedByCover, 1048576u);
		}
	}

	public int Networkm_myIncomingOverkillDamageTaken
	{
		get
		{
			return m_myIncomingOverkillDamageTaken;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myIncomingOverkillDamageTaken, 2097152u);
		}
	}

	public int Networkm_myOutgoingOverkillDamageDealt
	{
		get
		{
			return m_myOutgoingOverkillDamageDealt;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myOutgoingOverkillDamageDealt, 4194304u);
		}
	}

	public int Networkm_myOutgoingExtraDamageFromEmpowered
	{
		get
		{
			return m_myOutgoingExtraDamageFromEmpowered;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myOutgoingExtraDamageFromEmpowered, 8388608u);
		}
	}

	public int Networkm_myOutgoingDamageReducedFromWeakened
	{
		get
		{
			return m_myOutgoingDamageReducedFromWeakened;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_myOutgoingDamageReducedFromWeakened, 16777216u);
		}
	}

	public int Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe
	{
		get
		{
			return m_teamOutgoingDamageIncreasedByEmpoweredFromMe;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_teamOutgoingDamageIncreasedByEmpoweredFromMe, 33554432u);
		}
	}

	public int Networkm_teamIncomingDamageReducedByWeakenedFromMe
	{
		get
		{
			return m_teamIncomingDamageReducedByWeakenedFromMe;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_teamIncomingDamageReducedByWeakenedFromMe, 67108864u);
		}
	}

	public int Networkm_teamExtraEnergyGainFromMe
	{
		get
		{
			return m_teamExtraEnergyGainFromMe;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_teamExtraEnergyGainFromMe, 134217728u);
		}
	}

	public float Networkm_movementDeniedByMe
	{
		get
		{
			return m_movementDeniedByMe;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_movementDeniedByMe, 268435456u);
		}
	}

	public int Networkm_totalEnemySighted
	{
		get
		{
			return m_totalEnemySighted;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalEnemySighted, 536870912u);
		}
	}

	static ActorBehavior()
	{
		RegisterSyncListDelegate(typeof(ActorBehavior), kListm_syncEnemySourcesForDamageOrDebuff, InvokeSyncListm_syncEnemySourcesForDamageOrDebuff);
		RegisterSyncListDelegate(typeof(ActorBehavior), kListm_syncAllySourcesForHealAndBuff, InvokeSyncListm_syncAllySourcesForHealAndBuff);
		NetworkCRC.RegisterBehaviour("ActorBehavior", 0);
	}

	public string GetContributionBreakdownForUI()
	{
		int effectiveHealingFromAbility = EffectiveHealingFromAbility;
		string text = "\n";
		return
			new StringBuilder().Append(string.Format(StringUtil.TR("TotalContribution", "Global"), totalPlayerContribution)).Append(text).Append(string.Format(StringUtil.TR("DamageContribution", "Global"), totalPlayerDamage)).Append(text).Append(string.Format(StringUtil.TR("HealingContribution", "Global"), totalPlayerHealingFromAbility, effectiveHealingFromAbility)).Append(text).Append(string.Format(StringUtil.TR("ShieldingContribution", "Global"), totalPlayerAbsorb, totalPlayerPotentialAbsorb)).Append(text).Append(string.Format(StringUtil.TR("DamageReceivedContribution", "Global"), totalPlayerDamageReceived)).Append(text).Append(string.Format(StringUtil.TR("HealingReceivedContribution", "Global"), totalPlayerHealingReceived)).Append(text).Append(string.Format(StringUtil.TR("ShieldingDamageContribution", "Global"), totalPlayerAbsorbReceived)).Append(text).ToString();
	}

	private void Awake()
	{
		m_actor = GetComponent<ActorData>();
		m_syncEnemySourcesForDamageOrDebuff.InitializeBehaviour(this, kListm_syncEnemySourcesForDamageOrDebuff);
		m_syncAllySourcesForHealAndBuff.InitializeBehaviour(this, kListm_syncAllySourcesForHealAndBuff);
	}

	private void Start()
	{
	}

	public void Reset()
	{
		Networkm_totalDeaths = 0;
		Networkm_totalPlayerKills = 0;
		Networkm_totalPlayerAssists = 0;
		m_totalMinionKills = 0;
		Networkm_totalPlayerDamage = 0;
		Networkm_totalPlayerHealing = 0;
		Networkm_totalPlayerHealingFromAbility = 0;
		Networkm_totalPlayerOverheal = 0;
		Networkm_totalPlayerAbsorb = 0;
		Networkm_totalPlayerPotentialAbsorb = 0;
		Networkm_totalEnergyGained = 0;
		Networkm_totalPlayerDamageReceived = 0;
		Networkm_totalPlayerHealingReceived = 0;
		Networkm_totalPlayerAbsorbReceived = 0;
		Networkm_totalPlayerLockInTime = 0f;
		Networkm_totalPlayerTurns = 0;
		Networkm_damageDodgedByEvades = 0;
		Networkm_damageInterceptedByEvades = 0;
		Networkm_myIncomingDamageReducedByCover = 0;
		Networkm_myOutgoingDamageReducedByCover = 0;
		Networkm_myIncomingOverkillDamageTaken = 0;
		Networkm_myOutgoingOverkillDamageDealt = 0;
		Networkm_myOutgoingExtraDamageFromEmpowered = 0;
		Networkm_myOutgoingDamageReducedFromWeakened = 0;
		m_myOutgoingExtraDamageFromTargetsVulnerable = 0;
		m_myOutgoingDamageReducedFromTargetsArmored = 0;
		m_myIncomingExtraDamageFromCastersEmpowered = 0;
		m_myIncomingDamageReducedFromCastersWeakened = 0;
		m_myIncomingExtraDamageIncreasedByVulnerable = 0;
		m_myIncomingDamageReducedByArmored = 0;
		Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = 0;
		Networkm_teamIncomingDamageReducedByWeakenedFromMe = 0;
		m_teamIncomingDamageReducedByArmoredFromMe = 0;
		m_teamOutgoingDamageIncreasedByVulnerableFromMe = 0;
		Networkm_teamExtraEnergyGainFromMe = 0;
		Networkm_movementDeniedByMe = 0f;
	}

	public float? GetStat(StatDisplaySettings.StatType TypeOfStat)
	{
		switch (TypeOfStat)
		{
			case StatDisplaySettings.StatType.IncomingDamageDodgeByEvade:
				return NetDamageDodgedPerLife;
			case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
				return IncomingDamageReducedByCoverPerLife;
			case StatDisplaySettings.StatType.TotalAssists:
				return m_totalPlayerAssists;
			case StatDisplaySettings.StatType.TotalDeaths:
				return m_totalDeaths;
			case StatDisplaySettings.StatType.MovementDenied:
				return MovementDeniedPerTurn;
			case StatDisplaySettings.StatType.EnergyGainPerTurn:
				return EnergyGainPerTurn;
			case StatDisplaySettings.StatType.DamagePerTurn:
				return DamagePerTurn;
			case StatDisplaySettings.StatType.NetBoostedOutgoingDamage:
				return m_myOutgoingExtraDamageFromEmpowered / NumTurnsForStatCalc;
			case StatDisplaySettings.StatType.DamageEfficiency:
				return DamageEfficiency;
			case StatDisplaySettings.StatType.KillParticipation:
				return KillParticipation;
			case StatDisplaySettings.StatType.EffectiveHealAndAbsorb:
				return HealAndAbsorbPerTurn;
			case StatDisplaySettings.StatType.TeamDamageAdjustedByMe:
				return TeamDamageSwingPerTurn;
			case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
				return TeamEnergyBoostedByMePerTurn;
			case StatDisplaySettings.StatType.DamageTakenPerLife:
				return DamageTakenPerLife;
			case StatDisplaySettings.StatType.EnemiesSightedPerLife:
				return NumEnemiesSightedPerTurn;
			case StatDisplaySettings.StatType.TotalTurns:
				return m_totalPlayerTurns;
			case StatDisplaySettings.StatType.TankingPerLife:
				return (m_totalPlayerDamageReceived + netDamageAvoidedByEvades + myIncomingDamageReducedByCover) / NumLifeForStatCalc;
			case StatDisplaySettings.StatType.TotalTeamDamageReceived:
				return GameFlowData.Get().GetTotalTeamDamageReceived(m_actor.GetTeam());
			case StatDisplaySettings.StatType.TeamMitigation:
				{
					float num = EffectiveHealing + m_totalPlayerAbsorb + teamIncomingDamageReducedByWeakenedFromMe;
					float num2 = teamIncomingDamageReducedByWeakenedFromMe + GameFlowData.Get().GetTotalTeamDamageReceived(m_actor.GetTeam());
					if (num2 == 0f)
					{
						Log.Warning("Divide by Zero for Team Mitigation");
						return null;
					}
					return num / num2;
				}
			case StatDisplaySettings.StatType.SupportPerTurn:
				return HealAndAbsorbPerTurn;
			case StatDisplaySettings.StatType.DamageDonePerLife:
				return m_totalPlayerDamage / NumLifeForStatCalc;
			case StatDisplaySettings.StatType.DamageTakenPerTurn:
				return m_totalPlayerDamageReceived / NumTurnsForStatCalc;
			case StatDisplaySettings.StatType.AvgLifeSpan:
				return AvgLifeSpan;
			default:
				return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		FreelancerStats component = gameObject.GetComponent<FreelancerStats>();
		if (component != null)
		{
			return component.GetValueOfStat(FreelancerStatIndex);
		}
		return 0f;
	}

	private void DisplayActorBehavior()
	{
	}

	public string GetGeneralStatDebugString()
	{
		string text = "";
		text += new StringBuilder().Append("Total Kills = ").Append(totalPlayerKills).Append("\n").ToString();
		text += new StringBuilder().Append("Total Assists = ").Append(totalPlayerAssists).Append("\n").ToString();
		text += new StringBuilder().Append("Total Deaths = ").Append(totalDeaths).Append("\n").ToString();
		text += new StringBuilder().Append("\nTotal Damage = ").Append(totalPlayerDamage).Append("\n").ToString();
		text += new StringBuilder().Append("ExtraDamage From Might = ").Append(myOutgoingExtraDamageFromEmpowered).Append("\n").ToString();
		text += new StringBuilder().Append("LostDamage Due to Cover = ").Append(myOutgoingDamageReducedByCover).Append("\n").ToString();
		text += new StringBuilder().Append("LostDamage due to Weaken = ").Append(myOutgoingReducedDamageFromWeakened).Append("\n").ToString();
		text += new StringBuilder().Append("Overkill Damage Dealt = ").Append(myOutgoingOverkillDamageDealt).Append("\n").ToString();
		text += new StringBuilder().Append("\nDamage Taken = ").Append(totalPlayerDamageReceived).Append("\n").ToString();
		text += new StringBuilder().Append("Net Damage Dodged By Evades = ").Append(netDamageAvoidedByEvades).Append("\n").ToString();
		text += new StringBuilder().Append("\nTotal Healing = ").Append(totalPlayerHealing).Append("\n").ToString();
		text += new StringBuilder().Append("Overheal = ").Append(totalPlayerOverheal).Append("\n").ToString();
		text += new StringBuilder().Append("Shielding Dealt, Effective = ").Append(totalPlayerAbsorb).Append(", Total = ").Append(totalPlayerPotentialAbsorb).Append("\n").ToString();
		text += new StringBuilder().Append("Team Outgoing ExtraDamage from my Mighted = ").Append(teamOutgoingDamageIncreasedByEmpoweredFromMe).Append("\n").ToString();
		text += new StringBuilder().Append("Team Incoming LostDamage from my Weakened = ").Append(teamIncomingDamageReducedByWeakenedFromMe).Append("\n").ToString();
		text += new StringBuilder().Append("\nEnergy Gain = ").Append(totalEnergyGained).Append("\n").ToString();
		text += new StringBuilder().Append("Team Extra Energy from Me (energized + direct) = ").Append(teamExtraEnergyGainFromMe).Append("\n").ToString();
		text += new StringBuilder().Append("\nMovement Denied by me = ").Append(movementDeniedByMe).Append("\n").ToString();
		text += new StringBuilder().Append("\nNum Enemies Sighted Total = ").Append(totalEnemySighted).Append("\n").ToString();
		text += new StringBuilder().Append("\nAverage Life Span = ").Append(AvgLifeSpan).Append("\n").ToString();
		return text;
	}

	public void Client_ResetKillAssistContribution()
	{
		m_clientEffectSourceActors.Clear();
		m_clientDamageSourceActors.Clear();
		m_clientHealSourceActors.Clear();
	}

	public void Client_RecordEffectFromActor(ActorData caster)
	{
		if (caster != null
			&& caster.ActorIndex >= 0
			&& !m_clientEffectSourceActors.Contains(caster.ActorIndex))
		{
			m_clientEffectSourceActors.Add(caster.ActorIndex);
			if (DebugTraceClientContribution)
			{
				Debug.LogWarning(new StringBuilder().Append("<color=magenta>ActorBehavior: </color>").Append(m_actor.DebugNameString("white")).Append(" recording EFFECT from ").Append(caster.DebugNameString("yellow")).ToString());
			}
		}
	}

	public void Client_RecordDamageFromActor(ActorData caster)
	{
		if (caster != null
			&& caster.ActorIndex >= 0
			&& caster.GetTeam() != m_actor.GetTeam()
			&& !m_clientDamageSourceActors.Contains(caster.ActorIndex))
		{
			m_clientDamageSourceActors.Add(caster.ActorIndex);
			if (DebugTraceClientContribution)
			{
				Debug.LogWarning(new StringBuilder().Append("<color=magenta>ActorBehavior: </color>").Append(m_actor.DebugNameString("white")).Append(" recording DAMAGE from ").Append(caster.DebugNameString("yellow")).ToString());
			}
		}
	}

	public void Client_RecordHealingFromActor(ActorData caster)
	{
		if (caster != null
			&& caster.ActorIndex >= 0
			&& caster.GetTeam() == m_actor.GetTeam()
			&& !m_clientHealSourceActors.Contains(caster.ActorIndex))
		{
			m_clientHealSourceActors.Add(caster.ActorIndex);
			if (DebugTraceClientContribution)
			{
				Debug.LogWarning(new StringBuilder().Append("<color=magenta>ActorBehavior: </color>").Append(m_actor.DebugNameString("white")).Append(" recording HEALING from ").Append(caster.DebugNameString("yellow")).ToString());
			}
		}
	}

	public bool Client_ActorDamagedOrDebuffedByActor(ActorData caster)
	{
		return caster != null
			&& caster.ActorIndex >= 0
			&& caster.GetTeam() != m_actor.GetTeam()
			&& (m_clientEffectSourceActors.Contains(caster.ActorIndex)
				|| m_clientDamageSourceActors.Contains(caster.ActorIndex)
				|| m_syncEnemySourcesForDamageOrDebuff.Contains((uint)caster.ActorIndex));
	}

	public bool Client_ActorHealedOrBuffedByActor(ActorData caster)
	{
		return caster != null
			&& caster.ActorIndex >= 0
			&& caster.GetTeam() == m_actor.GetTeam()
			&& (m_clientEffectSourceActors.Contains(caster.ActorIndex)
				|| m_clientHealSourceActors.Contains(caster.ActorIndex)
				|| m_syncAllySourcesForHealAndBuff.Contains((uint)caster.ActorIndex));
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_syncEnemySourcesForDamageOrDebuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncEnemySourcesForDamageOrDebuff called on server.");
			return;
		}
		((ActorBehavior)obj).m_syncEnemySourcesForDamageOrDebuff.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_syncAllySourcesForHealAndBuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncAllySourcesForHealAndBuff called on server.");
			return;
		}
		((ActorBehavior)obj).m_syncAllySourcesForHealAndBuff.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, m_syncEnemySourcesForDamageOrDebuff);
			SyncListUInt.WriteInstance(writer, m_syncAllySourcesForHealAndBuff);
			writer.WritePackedUInt32((uint)m_totalDeaths);
			writer.WritePackedUInt32((uint)m_totalPlayerKills);
			writer.WritePackedUInt32((uint)m_totalPlayerAssists);
			writer.WritePackedUInt32((uint)m_totalPlayerDamage);
			writer.WritePackedUInt32((uint)m_totalPlayerHealing);
			writer.WritePackedUInt32((uint)m_totalPlayerHealingFromAbility);
			writer.WritePackedUInt32((uint)m_totalPlayerOverheal);
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorb);
			writer.WritePackedUInt32((uint)m_totalPlayerPotentialAbsorb);
			writer.WritePackedUInt32((uint)m_totalEnergyGained);
			writer.WritePackedUInt32((uint)m_totalPlayerDamageReceived);
			writer.WritePackedUInt32((uint)m_totalPlayerHealingReceived);
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorbReceived);
			writer.Write(m_totalPlayerLockInTime);
			writer.WritePackedUInt32((uint)m_totalPlayerTurns);
			writer.WritePackedUInt32((uint)m_damageDodgedByEvades);
			writer.WritePackedUInt32((uint)m_damageInterceptedByEvades);
			writer.WritePackedUInt32((uint)m_myIncomingDamageReducedByCover);
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedByCover);
			writer.WritePackedUInt32((uint)m_myIncomingOverkillDamageTaken);
			writer.WritePackedUInt32((uint)m_myOutgoingOverkillDamageDealt);
			writer.WritePackedUInt32((uint)m_myOutgoingExtraDamageFromEmpowered);
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedFromWeakened);
			writer.WritePackedUInt32((uint)m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
			writer.WritePackedUInt32((uint)m_teamIncomingDamageReducedByWeakenedFromMe);
			writer.WritePackedUInt32((uint)m_teamExtraEnergyGainFromMe);
			writer.Write(m_movementDeniedByMe);
			writer.WritePackedUInt32((uint)m_totalEnemySighted);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_syncAllySourcesForHealAndBuff);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalDeaths);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerKills);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAssists);
		}
		if ((syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerDamage);
		}
		if ((syncVarDirtyBits & 0x40) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealing);
		}
		if ((syncVarDirtyBits & 0x80) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealingFromAbility);
		}
		if ((syncVarDirtyBits & 0x100) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerOverheal);
		}
		if ((syncVarDirtyBits & 0x200) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorb);
		}
		if ((syncVarDirtyBits & 0x400) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerPotentialAbsorb);
		}
		if ((syncVarDirtyBits & 0x800) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalEnergyGained);
		}
		if ((syncVarDirtyBits & 0x1000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerDamageReceived);
		}
		if ((syncVarDirtyBits & 0x2000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealingReceived);
		}
		if ((syncVarDirtyBits & 0x4000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorbReceived);
		}
		if ((syncVarDirtyBits & 0x8000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_totalPlayerLockInTime);
		}
		if ((syncVarDirtyBits & 0x10000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerTurns);
		}
		if ((syncVarDirtyBits & 0x20000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageDodgedByEvades);
		}
		if ((syncVarDirtyBits & 0x40000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageInterceptedByEvades);
		}
		if ((syncVarDirtyBits & 0x80000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myIncomingDamageReducedByCover);
		}
		if ((syncVarDirtyBits & 0x100000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedByCover);
		}
		if ((syncVarDirtyBits & 0x200000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myIncomingOverkillDamageTaken);
		}
		if ((syncVarDirtyBits & 0x400000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingOverkillDamageDealt);
		}
		if ((syncVarDirtyBits & 0x800000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingExtraDamageFromEmpowered);
		}
		if ((syncVarDirtyBits & 0x1000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedFromWeakened);
		}
		if ((syncVarDirtyBits & 0x2000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
		}
		if ((syncVarDirtyBits & 0x4000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamIncomingDamageReducedByWeakenedFromMe);
		}
		if ((syncVarDirtyBits & 0x8000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamExtraEnergyGainFromMe);
		}
		if ((syncVarDirtyBits & 0x10000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_movementDeniedByMe);
		}
		if ((syncVarDirtyBits & 0x20000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalEnemySighted);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, m_syncEnemySourcesForDamageOrDebuff);
			SyncListUInt.ReadReference(reader, m_syncAllySourcesForHealAndBuff);
			m_totalDeaths = (short)reader.ReadPackedUInt32();
			m_totalPlayerKills = (short)reader.ReadPackedUInt32();
			m_totalPlayerAssists = (short)reader.ReadPackedUInt32();
			m_totalPlayerDamage = (int)reader.ReadPackedUInt32();
			m_totalPlayerHealing = (int)reader.ReadPackedUInt32();
			m_totalPlayerHealingFromAbility = (int)reader.ReadPackedUInt32();
			m_totalPlayerOverheal = (int)reader.ReadPackedUInt32();
			m_totalPlayerAbsorb = (int)reader.ReadPackedUInt32();
			m_totalPlayerPotentialAbsorb = (int)reader.ReadPackedUInt32();
			m_totalEnergyGained = (int)reader.ReadPackedUInt32();
			m_totalPlayerDamageReceived = (int)reader.ReadPackedUInt32();
			m_totalPlayerHealingReceived = (int)reader.ReadPackedUInt32();
			m_totalPlayerAbsorbReceived = (int)reader.ReadPackedUInt32();
			m_totalPlayerLockInTime = reader.ReadSingle();
			m_totalPlayerTurns = (int)reader.ReadPackedUInt32();
			m_damageDodgedByEvades = (int)reader.ReadPackedUInt32();
			m_damageInterceptedByEvades = (int)reader.ReadPackedUInt32();
			m_myIncomingDamageReducedByCover = (int)reader.ReadPackedUInt32();
			m_myOutgoingDamageReducedByCover = (int)reader.ReadPackedUInt32();
			m_myIncomingOverkillDamageTaken = (int)reader.ReadPackedUInt32();
			m_myOutgoingOverkillDamageDealt = (int)reader.ReadPackedUInt32();
			m_myOutgoingExtraDamageFromEmpowered = (int)reader.ReadPackedUInt32();
			m_myOutgoingDamageReducedFromWeakened = (int)reader.ReadPackedUInt32();
			m_teamOutgoingDamageIncreasedByEmpoweredFromMe = (int)reader.ReadPackedUInt32();
			m_teamIncomingDamageReducedByWeakenedFromMe = (int)reader.ReadPackedUInt32();
			m_teamExtraEnergyGainFromMe = (int)reader.ReadPackedUInt32();
			m_movementDeniedByMe = reader.ReadSingle();
			m_totalEnemySighted = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, m_syncAllySourcesForHealAndBuff);
		}
		if ((num & 4) != 0)
		{
			m_totalDeaths = (short)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_totalPlayerKills = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
		{
			m_totalPlayerAssists = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			m_totalPlayerDamage = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) != 0)
		{
			m_totalPlayerHealing = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80) != 0)
		{
			m_totalPlayerHealingFromAbility = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100) != 0)
		{
			m_totalPlayerOverheal = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200) != 0)
		{
			m_totalPlayerAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400) != 0)
		{
			m_totalPlayerPotentialAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800) != 0)
		{
			m_totalEnergyGained = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x1000) != 0)
		{
			m_totalPlayerDamageReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x2000) != 0)
		{
			m_totalPlayerHealingReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x4000) != 0)
		{
			m_totalPlayerAbsorbReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000) != 0)
		{
			m_totalPlayerLockInTime = reader.ReadSingle();
		}
		if ((num & 0x10000) != 0)
		{
			m_totalPlayerTurns = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20000) != 0)
		{
			m_damageDodgedByEvades = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40000) != 0)
		{
			m_damageInterceptedByEvades = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80000) != 0)
		{
			m_myIncomingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100000) != 0)
		{
			m_myOutgoingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200000) != 0)
		{
			m_myIncomingOverkillDamageTaken = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400000) != 0)
		{
			m_myOutgoingOverkillDamageDealt = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800000) != 0)
		{
			m_myOutgoingExtraDamageFromEmpowered = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x1000000) != 0)
		{
			m_myOutgoingDamageReducedFromWeakened = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x2000000) != 0)
		{
			m_teamOutgoingDamageIncreasedByEmpoweredFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x4000000) != 0)
		{
			m_teamIncomingDamageReducedByWeakenedFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000000) != 0)
		{
			m_teamExtraEnergyGainFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x10000000) != 0)
		{
			m_movementDeniedByMe = reader.ReadSingle();
		}
		if ((num & 0x20000000) != 0)
		{
			m_totalEnemySighted = (int)reader.ReadPackedUInt32();
		}
	}
}
