using System.Collections.Generic;
using System.Runtime.InteropServices;
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

	private static int kListm_syncEnemySourcesForDamageOrDebuff;

	private static int kListm_syncAllySourcesForHealAndBuff;

	public int totalDeaths => m_totalDeaths;

	public int totalPlayerKills => m_totalPlayerKills;

	public int totalPlayerAssists => m_totalPlayerAssists;

	public int totalMinionKills => m_totalMinionKills;

	public int totalPlayerContribution => m_totalPlayerDamage + EffectiveHealingFromAbility + m_totalPlayerAbsorb;

	public int totalPlayerDamage => m_totalPlayerDamage;

	public int totalPlayerHealing => m_totalPlayerHealing;

	public int totalPlayerHealingFromAbility => m_totalPlayerHealingFromAbility;

	public int totalPlayerOverheal => m_totalPlayerOverheal;

	public int totalPlayerAbsorb => m_totalPlayerAbsorb;

	public int totalPlayerPotentialAbsorb => m_totalPlayerPotentialAbsorb;

	public int totalEnergyGained => m_totalEnergyGained;

	public int totalPlayerDamageReceived => m_totalPlayerDamageReceived;

	public int totalPlayerHealingReceived => m_totalPlayerHealingReceived;

	public int totalPlayerAbsorbReceived => m_totalPlayerAbsorbReceived;

	public float totalPlayerLockInTime => m_totalPlayerLockInTime;

	public int totalPlayerTurns => m_totalPlayerTurns;

	public int netDamageAvoidedByEvades => Mathf.Max(0, m_damageDodgedByEvades - m_damageInterceptedByEvades);

	public int damageDodgedByEvades => m_damageDodgedByEvades;

	public int damageInterceptedByEvades => m_damageInterceptedByEvades;

	public int myIncomingDamageReducedByCover => m_myIncomingDamageReducedByCover;

	public int myOutgoingDamageReducedByCover => m_myOutgoingDamageReducedByCover;

	public int myIncomingOverkillDamageTaken => m_myIncomingOverkillDamageTaken;

	public int myOutgoingOverkillDamageDealt => m_myOutgoingOverkillDamageDealt;

	public int myOutgoingExtraDamageFromEmpowered => m_myOutgoingExtraDamageFromEmpowered;

	public int myOutgoingReducedDamageFromWeakened => m_myOutgoingDamageReducedFromWeakened;

	public int myOutgoingExtraDamageFromTargetsVulnerable => m_myOutgoingExtraDamageFromTargetsVulnerable;

	public int myOutgoingReducedDamageFromTargetsArmored => m_myOutgoingDamageReducedFromTargetsArmored;

	public int myIncomingDamageReducedByArmored => m_myIncomingDamageReducedByArmored;

	public int myIncomingExtraDamageIncreasedByVulnerable => m_myIncomingExtraDamageIncreasedByVulnerable;

	public int teamOutgoingDamageIncreasedByEmpoweredFromMe => m_teamOutgoingDamageIncreasedByEmpoweredFromMe;

	public int teamIncomingDamageReducedByWeakenedFromMe => m_teamIncomingDamageReducedByWeakenedFromMe;

	public int teamIncomingDamageReducedByArmoredFromMe => m_teamIncomingDamageReducedByArmoredFromMe;

	public int teamOutgoingDamageIncreasedByVulnerableFromMe => m_teamOutgoingDamageIncreasedByVulnerableFromMe;

	public int teamExtraEnergyGainFromMe => m_teamExtraEnergyGainFromMe;

	public float movementDeniedByMe => m_movementDeniedByMe;

	public int totalEnemySighted => m_totalEnemySighted;

	private float NumTurnsForStatCalc => Mathf.Max(1f, m_totalPlayerTurns);

	private float NumLifeForStatCalc => Mathf.Max(1f, (float)m_totalDeaths + 1f);

	public float EnergyGainPerTurn => (float)m_totalEnergyGained / NumTurnsForStatCalc;

	public float DamagePerTurn => (float)m_totalPlayerDamage / NumTurnsForStatCalc;

	public float NumEnemiesSightedPerTurn => (float)m_totalEnemySighted / NumTurnsForStatCalc;

	public float HealAndAbsorbPerTurn => (float)(EffectiveHealingFromAbility + m_totalPlayerAbsorb) / NumTurnsForStatCalc;

	public float MovementDeniedPerTurn => m_movementDeniedByMe / NumTurnsForStatCalc;

	public float TeamEnergyBoostedByMePerTurn => (float)teamExtraEnergyGainFromMe / NumTurnsForStatCalc;

	public float TeamDamageSwingPerTurn => (float)(m_teamOutgoingDamageIncreasedByEmpoweredFromMe + m_teamIncomingDamageReducedByWeakenedFromMe) / NumTurnsForStatCalc;

	public int NetBoostedOutgoingDamage => m_myOutgoingExtraDamageFromEmpowered - m_myOutgoingDamageReducedFromWeakened;

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
			return Mathf.Clamp(num2 / (float)num, 0f, 1f);
		}
	}

	public float KillParticipation
	{
		get
		{
			int deathCountOfTeam = GameFlowData.Get().GetDeathCountOfTeam(m_actor.GetOpposingTeam());
			if (deathCountOfTeam <= 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 0f;
					}
				}
			}
			return (float)m_totalPlayerAssists / (float)deathCountOfTeam;
		}
	}

	public int EffectiveHealing => Mathf.Max(0, m_totalPlayerHealing - m_totalPlayerOverheal);

	public int EffectiveHealingFromAbility => Mathf.Max(0, m_totalPlayerHealingFromAbility - m_totalPlayerOverheal);

	public float NetDamageDodgedPerLife => (float)netDamageAvoidedByEvades / NumLifeForStatCalc;

	public float IncomingDamageReducedByCoverPerLife => (float)m_myIncomingDamageReducedByCover / NumLifeForStatCalc;

	public float DamageTakenPerLife => (float)m_totalPlayerDamageReceived / NumLifeForStatCalc;

	public float AvgLifeSpan => NumTurnsForStatCalc / NumLifeForStatCalc;

	public CharacterType? CharacterType
	{
		get
		{
			if (m_actor != null)
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
						return m_actor.m_characterType;
					}
				}
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
							return characterResourceLink.m_characterRole;
						}
					}
				}
			}
			return null;
		}
	}

	public int totalDeathsOnTurnStart => m_totalDeathsOnTurnStart;

	public int serverIncomingDamageReducedByCoverThisTurn => m_serverIncomingDamageReducedByCoverThisTurn;

	public bool _001D
	{
		get
		{
			if (Application.isEditor)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return false;
					}
				}
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
		kListm_syncEnemySourcesForDamageOrDebuff = -1894796663;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorBehavior), kListm_syncEnemySourcesForDamageOrDebuff, InvokeSyncListm_syncEnemySourcesForDamageOrDebuff);
		kListm_syncAllySourcesForHealAndBuff = 1807084515;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorBehavior), kListm_syncAllySourcesForHealAndBuff, InvokeSyncListm_syncAllySourcesForHealAndBuff);
		NetworkCRC.RegisterBehaviour("ActorBehavior", 0);
	}

	public string GetContributionBreakdownForUI()
	{
		int effectiveHealingFromAbility = EffectiveHealingFromAbility;
		string text = "\n";
		return string.Format(StringUtil.TR("TotalContribution", "Global"), totalPlayerContribution) + text + string.Format(StringUtil.TR("DamageContribution", "Global"), totalPlayerDamage) + text + string.Format(StringUtil.TR("HealingContribution", "Global"), totalPlayerHealingFromAbility, effectiveHealingFromAbility) + text + string.Format(StringUtil.TR("ShieldingContribution", "Global"), totalPlayerAbsorb, totalPlayerPotentialAbsorb) + text + string.Format(StringUtil.TR("DamageReceivedContribution", "Global"), totalPlayerDamageReceived) + text + string.Format(StringUtil.TR("HealingReceivedContribution", "Global"), totalPlayerHealingReceived) + text + string.Format(StringUtil.TR("ShieldingDamageContribution", "Global"), totalPlayerAbsorbReceived) + text;
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
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return NetDamageDodgedPerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return IncomingDamageReducedByCoverPerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			return m_totalPlayerAssists;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_totalDeaths;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return MovementDeniedPerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return EnergyGainPerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return DamagePerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			return (float)m_myOutgoingExtraDamageFromEmpowered / NumTurnsForStatCalc;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return DamageEfficiency;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return KillParticipation;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			return HealAndAbsorbPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			return TeamDamageSwingPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return TeamEnergyBoostedByMePerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			return DamageTakenPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return NumEnemiesSightedPerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			return m_totalPlayerTurns;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return (float)(m_totalPlayerDamageReceived + netDamageAvoidedByEvades + myIncomingDamageReducedByCover) / NumLifeForStatCalc;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return GameFlowData.Get().GetTotalTeamDamageReceived(m_actor.GetTeam());
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
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
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.SupportPerTurn)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return HealAndAbsorbPerTurn;
				}
			}
		}
		switch (TypeOfStat)
		{
		case StatDisplaySettings.StatType.DamageDonePerLife:
			return (float)m_totalPlayerDamage / NumLifeForStatCalc;
		case StatDisplaySettings.StatType.DamageTakenPerTurn:
			return (float)m_totalPlayerDamageReceived / NumTurnsForStatCalc;
		case StatDisplaySettings.StatType.AvgLifeSpan:
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				return AvgLifeSpan;
			}
		default:
			return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		FreelancerStats component = base.gameObject.GetComponent<FreelancerStats>();
		if (component != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return component.GetValueOfStat(FreelancerStatIndex);
				}
			}
		}
		return 0f;
	}

	private void DisplayActorBehavior()
	{
	}

	public string GetGeneralStatDebugString()
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + "Total Kills = " + totalPlayerKills + "\n";
		text = empty;
		empty = text + "Total Assists = " + totalPlayerAssists + "\n";
		text = empty;
		empty = text + "Total Deaths = " + totalDeaths + "\n";
		text = empty;
		empty = text + "\nTotal Damage = " + totalPlayerDamage + "\n";
		text = empty;
		empty = text + "ExtraDamage From Might = " + myOutgoingExtraDamageFromEmpowered + "\n";
		text = empty;
		empty = text + "LostDamage Due to Cover = " + myOutgoingDamageReducedByCover + "\n";
		text = empty;
		empty = text + "LostDamage due to Weaken = " + myOutgoingReducedDamageFromWeakened + "\n";
		text = empty;
		empty = text + "Overkill Damage Dealt = " + myOutgoingOverkillDamageDealt + "\n";
		text = empty;
		empty = text + "\nDamage Taken = " + totalPlayerDamageReceived + "\n";
		text = empty;
		empty = text + "Net Damage Dodged By Evades = " + netDamageAvoidedByEvades + "\n";
		text = empty;
		empty = text + "\nTotal Healing = " + totalPlayerHealing + "\n";
		text = empty;
		empty = text + "Overheal = " + totalPlayerOverheal + "\n";
		text = empty;
		empty = text + "Shielding Dealt, Effective = " + totalPlayerAbsorb + ", Total = " + totalPlayerPotentialAbsorb + "\n";
		text = empty;
		empty = text + "Team Outgoing ExtraDamage from my Mighted = " + teamOutgoingDamageIncreasedByEmpoweredFromMe + "\n";
		text = empty;
		empty = text + "Team Incoming LostDamage from my Weakened = " + teamIncomingDamageReducedByWeakenedFromMe + "\n";
		text = empty;
		empty = text + "\nEnergy Gain = " + totalEnergyGained + "\n";
		text = empty;
		empty = text + "Team Extra Energy from Me (energized + direct) = " + teamExtraEnergyGainFromMe + "\n";
		text = empty;
		empty = text + "\nMovement Denied by me = " + movementDeniedByMe + "\n";
		text = empty;
		empty = text + "\nNum Enemies Sighted Total = " + totalEnemySighted + "\n";
		text = empty;
		return text + "\nAverage Life Span = " + AvgLifeSpan + "\n";
	}

	public void Client_ResetKillAssistContribution()
	{
		m_clientEffectSourceActors.Clear();
		m_clientDamageSourceActors.Clear();
		m_clientHealSourceActors.Clear();
	}

	public void Client_RecordEffectFromActor(ActorData caster)
	{
		if (!(caster != null))
		{
			return;
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
			if (caster.ActorIndex < 0)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_clientEffectSourceActors.Contains(caster.ActorIndex))
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
					m_clientEffectSourceActors.Add(caster.ActorIndex);
					if (_001D)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.GetColoredDebugName("white") + " recording EFFECT from " + caster.GetColoredDebugName("yellow"));
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void Client_RecordDamageFromActor(ActorData caster)
	{
		if (!(caster != null))
		{
			return;
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
			if (caster.ActorIndex < 0)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (caster.GetTeam() == m_actor.GetTeam())
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (m_clientDamageSourceActors.Contains(caster.ActorIndex))
					{
						return;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						m_clientDamageSourceActors.Add(caster.ActorIndex);
						if (_001D)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.GetColoredDebugName("white") + " recording DAMAGE from " + caster.GetColoredDebugName("yellow"));
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public void Client_RecordHealingFromActor(ActorData caster)
	{
		if (!(caster != null))
		{
			return;
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
			if (caster.ActorIndex < 0 || caster.GetTeam() != m_actor.GetTeam() || m_clientHealSourceActors.Contains(caster.ActorIndex))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				m_clientHealSourceActors.Add(caster.ActorIndex);
				if (_001D)
				{
					Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.GetColoredDebugName("white") + " recording HEALING from " + caster.GetColoredDebugName("yellow"));
				}
				return;
			}
		}
	}

	public bool Client_ActorDamagedOrDebuffedByActor(ActorData caster)
	{
		if (caster != null && caster.ActorIndex >= 0)
		{
			while (true)
			{
				switch (7)
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
			if (caster.GetTeam() != m_actor.GetTeam())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_clientEffectSourceActors.Contains(caster.ActorIndex) || m_clientDamageSourceActors.Contains(caster.ActorIndex) || m_syncEnemySourcesForDamageOrDebuff.Contains((uint)caster.ActorIndex);
					}
				}
			}
		}
		return false;
	}

	public bool Client_ActorHealedOrBuffedByActor(ActorData caster)
	{
		if (caster != null)
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
			if (caster.ActorIndex >= 0)
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
				if (caster.GetTeam() == m_actor.GetTeam())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							int result;
							if (!m_clientEffectSourceActors.Contains(caster.ActorIndex) && !m_clientHealSourceActors.Contains(caster.ActorIndex))
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
								result = (m_syncAllySourcesForHealAndBuff.Contains((uint)caster.ActorIndex) ? 1 : 0);
							}
							else
							{
								result = 1;
							}
							return (byte)result != 0;
						}
						}
					}
				}
			}
		}
		return false;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_syncEnemySourcesForDamageOrDebuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("SyncList m_syncEnemySourcesForDamageOrDebuff called on server.");
					return;
				}
			}
		}
		((ActorBehavior)obj).m_syncEnemySourcesForDamageOrDebuff.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_syncAllySourcesForHealAndBuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
					Debug.LogError("SyncList m_syncAllySourcesForHealAndBuff called on server.");
					return;
				}
			}
		}
		((ActorBehavior)obj).m_syncAllySourcesForHealAndBuff.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_syncAllySourcesForHealAndBuff);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalDeaths);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerKills);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAssists);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerDamage);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealing);
		}
		if ((base.syncVarDirtyBits & 0x80) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealingFromAbility);
		}
		if ((base.syncVarDirtyBits & 0x100) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerOverheal);
		}
		if ((base.syncVarDirtyBits & 0x200) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorb);
		}
		if ((base.syncVarDirtyBits & 0x400) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerPotentialAbsorb);
		}
		if ((base.syncVarDirtyBits & 0x800) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalEnergyGained);
		}
		if ((base.syncVarDirtyBits & 0x1000) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerDamageReceived);
		}
		if ((base.syncVarDirtyBits & 0x2000) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerHealingReceived);
		}
		if ((base.syncVarDirtyBits & 0x4000) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerAbsorbReceived);
		}
		if ((base.syncVarDirtyBits & 0x8000) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_totalPlayerLockInTime);
		}
		if ((base.syncVarDirtyBits & 0x10000) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalPlayerTurns);
		}
		if ((base.syncVarDirtyBits & 0x20000) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageDodgedByEvades);
		}
		if ((base.syncVarDirtyBits & 0x40000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageInterceptedByEvades);
		}
		if ((base.syncVarDirtyBits & 0x80000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myIncomingDamageReducedByCover);
		}
		if ((base.syncVarDirtyBits & 0x100000) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedByCover);
		}
		if ((base.syncVarDirtyBits & 0x200000) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myIncomingOverkillDamageTaken);
		}
		if ((base.syncVarDirtyBits & 0x400000) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingOverkillDamageDealt);
		}
		if ((base.syncVarDirtyBits & 0x800000) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingExtraDamageFromEmpowered);
		}
		if ((base.syncVarDirtyBits & 0x1000000) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_myOutgoingDamageReducedFromWeakened);
		}
		if ((base.syncVarDirtyBits & 0x2000000) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
		}
		if ((base.syncVarDirtyBits & 0x4000000) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamIncomingDamageReducedByWeakenedFromMe);
		}
		if ((base.syncVarDirtyBits & 0x8000000) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_teamExtraEnergyGainFromMe);
		}
		if ((base.syncVarDirtyBits & 0x10000000) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_movementDeniedByMe);
		}
		if ((base.syncVarDirtyBits & 0x20000000) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalEnemySighted);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			SyncListUInt.ReadReference(reader, m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, m_syncAllySourcesForHealAndBuff);
		}
		if ((num & 4) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_totalDeaths = (short)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
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
			m_totalPlayerKills = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
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
			m_totalPlayerAssists = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_totalPlayerDamage = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) != 0)
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
			m_totalPlayerHealing = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80) != 0)
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
			m_totalPlayerHealingFromAbility = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100) != 0)
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
			m_totalPlayerOverheal = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200) != 0)
		{
			m_totalPlayerAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400) != 0)
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
			m_totalPlayerPotentialAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800) != 0)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_totalPlayerAbsorbReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_damageInterceptedByEvades = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80000) != 0)
		{
			m_myIncomingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100000) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_myOutgoingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200000) != 0)
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
			m_myIncomingOverkillDamageTaken = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400000) != 0)
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
			m_myOutgoingOverkillDamageDealt = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800000) != 0)
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
			m_myOutgoingExtraDamageFromEmpowered = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x1000000) != 0)
		{
			m_myOutgoingDamageReducedFromWeakened = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x2000000) != 0)
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
			m_teamOutgoingDamageIncreasedByEmpoweredFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x4000000) != 0)
		{
			m_teamIncomingDamageReducedByWeakenedFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000000) != 0)
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
			m_teamExtraEnergyGainFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x10000000) != 0)
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
			m_movementDeniedByMe = reader.ReadSingle();
		}
		if ((num & 0x20000000) == 0)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			m_totalEnemySighted = (int)reader.ReadPackedUInt32();
			return;
		}
	}
}
