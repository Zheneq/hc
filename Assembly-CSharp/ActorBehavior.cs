using System;
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

	private static int kListm_syncEnemySourcesForDamageOrDebuff = -0x70F04D77;

	private static int kListm_syncAllySourcesForHealAndBuff;

	static ActorBehavior()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorBehavior), ActorBehavior.kListm_syncEnemySourcesForDamageOrDebuff, new NetworkBehaviour.CmdDelegate(ActorBehavior.InvokeSyncListm_syncEnemySourcesForDamageOrDebuff));
		ActorBehavior.kListm_syncAllySourcesForHealAndBuff = 0x6BB5EBE3;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorBehavior), ActorBehavior.kListm_syncAllySourcesForHealAndBuff, new NetworkBehaviour.CmdDelegate(ActorBehavior.InvokeSyncListm_syncAllySourcesForHealAndBuff));
		NetworkCRC.RegisterBehaviour("ActorBehavior", 0);
	}

	public int totalDeaths
	{
		get
		{
			return (int)this.m_totalDeaths;
		}
	}

	public int totalPlayerKills
	{
		get
		{
			return (int)this.m_totalPlayerKills;
		}
	}

	public int totalPlayerAssists
	{
		get
		{
			return (int)this.m_totalPlayerAssists;
		}
	}

	public int totalMinionKills
	{
		get
		{
			return (int)this.m_totalMinionKills;
		}
	}

	public int totalPlayerContribution
	{
		get
		{
			return this.m_totalPlayerDamage + this.EffectiveHealingFromAbility + this.m_totalPlayerAbsorb;
		}
	}

	public int totalPlayerDamage
	{
		get
		{
			return this.m_totalPlayerDamage;
		}
	}

	public int totalPlayerHealing
	{
		get
		{
			return this.m_totalPlayerHealing;
		}
	}

	public int totalPlayerHealingFromAbility
	{
		get
		{
			return this.m_totalPlayerHealingFromAbility;
		}
	}

	public int totalPlayerOverheal
	{
		get
		{
			return this.m_totalPlayerOverheal;
		}
	}

	public int totalPlayerAbsorb
	{
		get
		{
			return this.m_totalPlayerAbsorb;
		}
	}

	public int totalPlayerPotentialAbsorb
	{
		get
		{
			return this.m_totalPlayerPotentialAbsorb;
		}
	}

	public int totalEnergyGained
	{
		get
		{
			return this.m_totalEnergyGained;
		}
	}

	public int totalPlayerDamageReceived
	{
		get
		{
			return this.m_totalPlayerDamageReceived;
		}
	}

	public int totalPlayerHealingReceived
	{
		get
		{
			return this.m_totalPlayerHealingReceived;
		}
	}

	public int totalPlayerAbsorbReceived
	{
		get
		{
			return this.m_totalPlayerAbsorbReceived;
		}
	}

	public float totalPlayerLockInTime
	{
		get
		{
			return this.m_totalPlayerLockInTime;
		}
	}

	public int totalPlayerTurns
	{
		get
		{
			return this.m_totalPlayerTurns;
		}
	}

	public int netDamageAvoidedByEvades
	{
		get
		{
			return Mathf.Max(0, this.m_damageDodgedByEvades - this.m_damageInterceptedByEvades);
		}
	}

	public int damageDodgedByEvades
	{
		get
		{
			return this.m_damageDodgedByEvades;
		}
	}

	public int damageInterceptedByEvades
	{
		get
		{
			return this.m_damageInterceptedByEvades;
		}
	}

	public int myIncomingDamageReducedByCover
	{
		get
		{
			return this.m_myIncomingDamageReducedByCover;
		}
	}

	public int myOutgoingDamageReducedByCover
	{
		get
		{
			return this.m_myOutgoingDamageReducedByCover;
		}
	}

	public int myIncomingOverkillDamageTaken
	{
		get
		{
			return this.m_myIncomingOverkillDamageTaken;
		}
	}

	public int myOutgoingOverkillDamageDealt
	{
		get
		{
			return this.m_myOutgoingOverkillDamageDealt;
		}
	}

	public int myOutgoingExtraDamageFromEmpowered
	{
		get
		{
			return this.m_myOutgoingExtraDamageFromEmpowered;
		}
	}

	public int myOutgoingReducedDamageFromWeakened
	{
		get
		{
			return this.m_myOutgoingDamageReducedFromWeakened;
		}
	}

	public int myOutgoingExtraDamageFromTargetsVulnerable
	{
		get
		{
			return this.m_myOutgoingExtraDamageFromTargetsVulnerable;
		}
	}

	public int myOutgoingReducedDamageFromTargetsArmored
	{
		get
		{
			return this.m_myOutgoingDamageReducedFromTargetsArmored;
		}
	}

	public int myIncomingDamageReducedByArmored
	{
		get
		{
			return this.m_myIncomingDamageReducedByArmored;
		}
	}

	public int myIncomingExtraDamageIncreasedByVulnerable
	{
		get
		{
			return this.m_myIncomingExtraDamageIncreasedByVulnerable;
		}
	}

	public int teamOutgoingDamageIncreasedByEmpoweredFromMe
	{
		get
		{
			return this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe;
		}
	}

	public int teamIncomingDamageReducedByWeakenedFromMe
	{
		get
		{
			return this.m_teamIncomingDamageReducedByWeakenedFromMe;
		}
	}

	public int teamIncomingDamageReducedByArmoredFromMe
	{
		get
		{
			return this.m_teamIncomingDamageReducedByArmoredFromMe;
		}
	}

	public int teamOutgoingDamageIncreasedByVulnerableFromMe
	{
		get
		{
			return this.m_teamOutgoingDamageIncreasedByVulnerableFromMe;
		}
	}

	public int teamExtraEnergyGainFromMe
	{
		get
		{
			return this.m_teamExtraEnergyGainFromMe;
		}
	}

	public float movementDeniedByMe
	{
		get
		{
			return this.m_movementDeniedByMe;
		}
	}

	public int totalEnemySighted
	{
		get
		{
			return this.m_totalEnemySighted;
		}
	}

	private float NumTurnsForStatCalc
	{
		get
		{
			return Mathf.Max(1f, (float)this.m_totalPlayerTurns);
		}
	}

	private float NumLifeForStatCalc
	{
		get
		{
			return Mathf.Max(1f, (float)this.m_totalDeaths + 1f);
		}
	}

	public float EnergyGainPerTurn
	{
		get
		{
			return (float)this.m_totalEnergyGained / this.NumTurnsForStatCalc;
		}
	}

	public float DamagePerTurn
	{
		get
		{
			return (float)this.m_totalPlayerDamage / this.NumTurnsForStatCalc;
		}
	}

	public float NumEnemiesSightedPerTurn
	{
		get
		{
			return (float)this.m_totalEnemySighted / this.NumTurnsForStatCalc;
		}
	}

	public float HealAndAbsorbPerTurn
	{
		get
		{
			return (float)(this.EffectiveHealingFromAbility + this.m_totalPlayerAbsorb) / this.NumTurnsForStatCalc;
		}
	}

	public float MovementDeniedPerTurn
	{
		get
		{
			return this.m_movementDeniedByMe / this.NumTurnsForStatCalc;
		}
	}

	public float TeamEnergyBoostedByMePerTurn
	{
		get
		{
			return (float)this.teamExtraEnergyGainFromMe / this.NumTurnsForStatCalc;
		}
	}

	public float TeamDamageSwingPerTurn
	{
		get
		{
			return (float)(this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe + this.m_teamIncomingDamageReducedByWeakenedFromMe) / this.NumTurnsForStatCalc;
		}
	}

	public int NetBoostedOutgoingDamage
	{
		get
		{
			return this.m_myOutgoingExtraDamageFromEmpowered - this.m_myOutgoingDamageReducedFromWeakened;
		}
	}

	public float DamageEfficiency
	{
		get
		{
			int num = this.m_totalPlayerDamage + this.m_myOutgoingDamageReducedByCover;
			if (num <= 0)
			{
				return 0f;
			}
			float num2 = (float)(this.m_totalPlayerDamage - this.m_myOutgoingOverkillDamageDealt);
			return Mathf.Clamp(num2 / (float)num, 0f, 1f);
		}
	}

	public float KillParticipation
	{
		get
		{
			int deathCountOfTeam = GameFlowData.Get().GetDeathCountOfTeam(this.m_actor.GetOpposingTeam());
			if (deathCountOfTeam <= 0)
			{
				return 0f;
			}
			return (float)this.m_totalPlayerAssists / (float)deathCountOfTeam;
		}
	}

	public int EffectiveHealing
	{
		get
		{
			return Mathf.Max(0, this.m_totalPlayerHealing - this.m_totalPlayerOverheal);
		}
	}

	public int EffectiveHealingFromAbility
	{
		get
		{
			return Mathf.Max(0, this.m_totalPlayerHealingFromAbility - this.m_totalPlayerOverheal);
		}
	}

	public float NetDamageDodgedPerLife
	{
		get
		{
			return (float)this.netDamageAvoidedByEvades / this.NumLifeForStatCalc;
		}
	}

	public float IncomingDamageReducedByCoverPerLife
	{
		get
		{
			return (float)this.m_myIncomingDamageReducedByCover / this.NumLifeForStatCalc;
		}
	}

	public float DamageTakenPerLife
	{
		get
		{
			return (float)this.m_totalPlayerDamageReceived / this.NumLifeForStatCalc;
		}
	}

	public float AvgLifeSpan
	{
		get
		{
			return this.NumTurnsForStatCalc / this.NumLifeForStatCalc;
		}
	}

	public string GetContributionBreakdownForUI()
	{
		int effectiveHealingFromAbility = this.EffectiveHealingFromAbility;
		string text = "\n";
		return string.Concat(new string[]
		{
			string.Format(StringUtil.TR("TotalContribution", "Global"), this.totalPlayerContribution),
			text,
			string.Format(StringUtil.TR("DamageContribution", "Global"), this.totalPlayerDamage),
			text,
			string.Format(StringUtil.TR("HealingContribution", "Global"), this.totalPlayerHealingFromAbility, effectiveHealingFromAbility),
			text,
			string.Format(StringUtil.TR("ShieldingContribution", "Global"), this.totalPlayerAbsorb, this.totalPlayerPotentialAbsorb),
			text,
			string.Format(StringUtil.TR("DamageReceivedContribution", "Global"), this.totalPlayerDamageReceived),
			text,
			string.Format(StringUtil.TR("HealingReceivedContribution", "Global"), this.totalPlayerHealingReceived),
			text,
			string.Format(StringUtil.TR("ShieldingDamageContribution", "Global"), this.totalPlayerAbsorbReceived),
			text
		});
	}

	public CharacterType? CharacterType
	{
		get
		{
			if (this.m_actor != null)
			{
				return new CharacterType?(this.m_actor.m_characterType);
			}
			return null;
		}
	}

	public CharacterRole? CharacterRole
	{
		get
		{
			CharacterType? characterType = this.CharacterType;
			if (characterType != null)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType.Value);
				if (characterResourceLink != null)
				{
					return new CharacterRole?(characterResourceLink.m_characterRole);
				}
			}
			return null;
		}
	}

	public int totalDeathsOnTurnStart
	{
		get
		{
			return this.m_totalDeathsOnTurnStart;
		}
	}

	public int serverIncomingDamageReducedByCoverThisTurn
	{
		get
		{
			return this.m_serverIncomingDamageReducedByCoverThisTurn;
		}
	}

	private void Awake()
	{
		this.m_actor = base.GetComponent<ActorData>();
		this.m_syncEnemySourcesForDamageOrDebuff.InitializeBehaviour(this, ActorBehavior.kListm_syncEnemySourcesForDamageOrDebuff);
		this.m_syncAllySourcesForHealAndBuff.InitializeBehaviour(this, ActorBehavior.kListm_syncAllySourcesForHealAndBuff);
	}

	private void Start()
	{
	}

	public void Reset()
	{
		this.Networkm_totalDeaths = 0;
		this.Networkm_totalPlayerKills = 0;
		this.Networkm_totalPlayerAssists = 0;
		this.m_totalMinionKills = 0;
		this.Networkm_totalPlayerDamage = 0;
		this.Networkm_totalPlayerHealing = 0;
		this.Networkm_totalPlayerHealingFromAbility = 0;
		this.Networkm_totalPlayerOverheal = 0;
		this.Networkm_totalPlayerAbsorb = 0;
		this.Networkm_totalPlayerPotentialAbsorb = 0;
		this.Networkm_totalEnergyGained = 0;
		this.Networkm_totalPlayerDamageReceived = 0;
		this.Networkm_totalPlayerHealingReceived = 0;
		this.Networkm_totalPlayerAbsorbReceived = 0;
		this.Networkm_totalPlayerLockInTime = 0f;
		this.Networkm_totalPlayerTurns = 0;
		this.Networkm_damageDodgedByEvades = 0;
		this.Networkm_damageInterceptedByEvades = 0;
		this.Networkm_myIncomingDamageReducedByCover = 0;
		this.Networkm_myOutgoingDamageReducedByCover = 0;
		this.Networkm_myIncomingOverkillDamageTaken = 0;
		this.Networkm_myOutgoingOverkillDamageDealt = 0;
		this.Networkm_myOutgoingExtraDamageFromEmpowered = 0;
		this.Networkm_myOutgoingDamageReducedFromWeakened = 0;
		this.m_myOutgoingExtraDamageFromTargetsVulnerable = 0;
		this.m_myOutgoingDamageReducedFromTargetsArmored = 0;
		this.m_myIncomingExtraDamageFromCastersEmpowered = 0;
		this.m_myIncomingDamageReducedFromCastersWeakened = 0;
		this.m_myIncomingExtraDamageIncreasedByVulnerable = 0;
		this.m_myIncomingDamageReducedByArmored = 0;
		this.Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = 0;
		this.Networkm_teamIncomingDamageReducedByWeakenedFromMe = 0;
		this.m_teamIncomingDamageReducedByArmoredFromMe = 0;
		this.m_teamOutgoingDamageIncreasedByVulnerableFromMe = 0;
		this.Networkm_teamExtraEnergyGainFromMe = 0;
		this.Networkm_movementDeniedByMe = 0f;
	}

	public float? GetStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			return new float?(this.NetDamageDodgedPerLife);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			return new float?(this.IncomingDamageReducedByCoverPerLife);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			return new float?((float)this.m_totalPlayerAssists);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			return new float?((float)this.m_totalDeaths);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			return new float?(this.MovementDeniedPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
		{
			return new float?(this.EnergyGainPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			return new float?(this.DamagePerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			return new float?((float)this.m_myOutgoingExtraDamageFromEmpowered / this.NumTurnsForStatCalc);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			return new float?(this.DamageEfficiency);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return new float?(this.KillParticipation);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			return new float?(this.HealAndAbsorbPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			return new float?(this.TeamDamageSwingPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			return new float?(this.TeamEnergyBoostedByMePerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			return new float?(this.DamageTakenPerLife);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			return new float?(this.NumEnemiesSightedPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			return new float?((float)this.m_totalPlayerTurns);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			return new float?((float)(this.m_totalPlayerDamageReceived + this.netDamageAvoidedByEvades + this.myIncomingDamageReducedByCover) / this.NumLifeForStatCalc);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			return new float?((float)GameFlowData.Get().GetTotalTeamDamageReceived(this.m_actor.GetTeam()));
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			float num = (float)(this.EffectiveHealing + this.m_totalPlayerAbsorb + this.teamIncomingDamageReducedByWeakenedFromMe);
			float num2 = (float)(this.teamIncomingDamageReducedByWeakenedFromMe + GameFlowData.Get().GetTotalTeamDamageReceived(this.m_actor.GetTeam()));
			if (num2 == 0f)
			{
				Log.Warning("Divide by Zero for Team Mitigation", new object[0]);
				return null;
			}
			return new float?(num / num2);
		}
		else
		{
			if (TypeOfStat == StatDisplaySettings.StatType.SupportPerTurn)
			{
				return new float?(this.HealAndAbsorbPerTurn);
			}
			if (TypeOfStat == StatDisplaySettings.StatType.DamageDonePerLife)
			{
				return new float?((float)this.m_totalPlayerDamage / this.NumLifeForStatCalc);
			}
			if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerTurn)
			{
				return new float?((float)this.m_totalPlayerDamageReceived / this.NumTurnsForStatCalc);
			}
			if (TypeOfStat == StatDisplaySettings.StatType.AvgLifeSpan)
			{
				return new float?(this.AvgLifeSpan);
			}
			return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		FreelancerStats component = base.gameObject.GetComponent<FreelancerStats>();
		if (component != null)
		{
			return new float?((float)component.GetValueOfStat(FreelancerStatIndex));
		}
		return new float?(0f);
	}

	private void DisplayActorBehavior()
	{
	}

	public string GetGeneralStatDebugString()
	{
		string text = string.Empty;
		string text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Total Kills = ",
			this.totalPlayerKills,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Total Assists = ",
			this.totalPlayerAssists,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Total Deaths = ",
			this.totalDeaths,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nTotal Damage = ",
			this.totalPlayerDamage,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"ExtraDamage From Might = ",
			this.myOutgoingExtraDamageFromEmpowered,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"LostDamage Due to Cover = ",
			this.myOutgoingDamageReducedByCover,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"LostDamage due to Weaken = ",
			this.myOutgoingReducedDamageFromWeakened,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Overkill Damage Dealt = ",
			this.myOutgoingOverkillDamageDealt,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nDamage Taken = ",
			this.totalPlayerDamageReceived,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Net Damage Dodged By Evades = ",
			this.netDamageAvoidedByEvades,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nTotal Healing = ",
			this.totalPlayerHealing,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Overheal = ",
			this.totalPlayerOverheal,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Shielding Dealt, Effective = ",
			this.totalPlayerAbsorb,
			", Total = ",
			this.totalPlayerPotentialAbsorb,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Team Outgoing ExtraDamage from my Mighted = ",
			this.teamOutgoingDamageIncreasedByEmpoweredFromMe,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Team Incoming LostDamage from my Weakened = ",
			this.teamIncomingDamageReducedByWeakenedFromMe,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nEnergy Gain = ",
			this.totalEnergyGained,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"Team Extra Energy from Me (energized + direct) = ",
			this.teamExtraEnergyGainFromMe,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nMovement Denied by me = ",
			this.movementDeniedByMe,
			"\n"
		});
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\nNum Enemies Sighted Total = ",
			this.totalEnemySighted,
			"\n"
		});
		text2 = text;
		return string.Concat(new object[]
		{
			text2,
			"\nAverage Life Span = ",
			this.AvgLifeSpan,
			"\n"
		});
	}

	public bool symbol_001D
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

	public void Client_ResetKillAssistContribution()
	{
		this.m_clientEffectSourceActors.Clear();
		this.m_clientDamageSourceActors.Clear();
		this.m_clientHealSourceActors.Clear();
	}

	public void Client_RecordEffectFromActor(ActorData caster)
	{
		if (caster != null)
		{
			if (caster.ActorIndex >= 0)
			{
				if (!this.m_clientEffectSourceActors.Contains(caster.ActorIndex))
				{
					this.m_clientEffectSourceActors.Add(caster.ActorIndex);
					if (this.symbol_001D)
					{
						Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + this.m_actor.GetColoredDebugName("white") + " recording EFFECT from " + caster.GetColoredDebugName("yellow"));
					}
				}
			}
		}
	}

	public void Client_RecordDamageFromActor(ActorData caster)
	{
		if (caster != null)
		{
			if (caster.ActorIndex >= 0)
			{
				if (caster.GetTeam() != this.m_actor.GetTeam())
				{
					if (!this.m_clientDamageSourceActors.Contains(caster.ActorIndex))
					{
						this.m_clientDamageSourceActors.Add(caster.ActorIndex);
						if (this.symbol_001D)
						{
							Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + this.m_actor.GetColoredDebugName("white") + " recording DAMAGE from " + caster.GetColoredDebugName("yellow"));
						}
					}
				}
			}
		}
	}

	public void Client_RecordHealingFromActor(ActorData caster)
	{
		if (caster != null)
		{
			if (caster.ActorIndex >= 0 && caster.GetTeam() == this.m_actor.GetTeam() && !this.m_clientHealSourceActors.Contains(caster.ActorIndex))
			{
				this.m_clientHealSourceActors.Add(caster.ActorIndex);
				if (this.symbol_001D)
				{
					Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + this.m_actor.GetColoredDebugName("white") + " recording HEALING from " + caster.GetColoredDebugName("yellow"));
				}
			}
		}
	}

	public bool Client_ActorDamagedOrDebuffedByActor(ActorData caster)
	{
		if (caster != null && caster.ActorIndex >= 0)
		{
			if (caster.GetTeam() != this.m_actor.GetTeam())
			{
				return this.m_clientEffectSourceActors.Contains(caster.ActorIndex) || this.m_clientDamageSourceActors.Contains(caster.ActorIndex) || this.m_syncEnemySourcesForDamageOrDebuff.Contains((uint)caster.ActorIndex);
			}
		}
		return false;
	}

	public bool Client_ActorHealedOrBuffedByActor(ActorData caster)
	{
		if (caster != null)
		{
			if (caster.ActorIndex >= 0)
			{
				if (caster.GetTeam() == this.m_actor.GetTeam())
				{
					bool result;
					if (!this.m_clientEffectSourceActors.Contains(caster.ActorIndex) && !this.m_clientHealSourceActors.Contains(caster.ActorIndex))
					{
						result = this.m_syncAllySourcesForHealAndBuff.Contains((uint)caster.ActorIndex);
					}
					else
					{
						result = true;
					}
					return result;
				}
			}
		}
		return false;
	}

	private void UNetVersion()
	{
	}

	public short Networkm_totalDeaths
	{
		get
		{
			return this.m_totalDeaths;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_totalDeaths, 4U);
		}
	}

	public short Networkm_totalPlayerKills
	{
		get
		{
			return this.m_totalPlayerKills;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_totalPlayerKills, 8U);
		}
	}

	public short Networkm_totalPlayerAssists
	{
		get
		{
			return this.m_totalPlayerAssists;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_totalPlayerAssists, 0x10U);
		}
	}

	public int Networkm_totalPlayerDamage
	{
		get
		{
			return this.m_totalPlayerDamage;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerDamage, 0x20U);
		}
	}

	public int Networkm_totalPlayerHealing
	{
		get
		{
			return this.m_totalPlayerHealing;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerHealing, 0x40U);
		}
	}

	public int Networkm_totalPlayerHealingFromAbility
	{
		get
		{
			return this.m_totalPlayerHealingFromAbility;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerHealingFromAbility, 0x80U);
		}
	}

	public int Networkm_totalPlayerOverheal
	{
		get
		{
			return this.m_totalPlayerOverheal;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerOverheal, 0x100U);
		}
	}

	public int Networkm_totalPlayerAbsorb
	{
		get
		{
			return this.m_totalPlayerAbsorb;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerAbsorb, 0x200U);
		}
	}

	public int Networkm_totalPlayerPotentialAbsorb
	{
		get
		{
			return this.m_totalPlayerPotentialAbsorb;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerPotentialAbsorb, 0x400U);
		}
	}

	public int Networkm_totalEnergyGained
	{
		get
		{
			return this.m_totalEnergyGained;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalEnergyGained, 0x800U);
		}
	}

	public int Networkm_totalPlayerDamageReceived
	{
		get
		{
			return this.m_totalPlayerDamageReceived;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerDamageReceived, 0x1000U);
		}
	}

	public int Networkm_totalPlayerHealingReceived
	{
		get
		{
			return this.m_totalPlayerHealingReceived;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerHealingReceived, 0x2000U);
		}
	}

	public int Networkm_totalPlayerAbsorbReceived
	{
		get
		{
			return this.m_totalPlayerAbsorbReceived;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerAbsorbReceived, 0x4000U);
		}
	}

	public float Networkm_totalPlayerLockInTime
	{
		get
		{
			return this.m_totalPlayerLockInTime;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_totalPlayerLockInTime, 0x8000U);
		}
	}

	public int Networkm_totalPlayerTurns
	{
		get
		{
			return this.m_totalPlayerTurns;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalPlayerTurns, 0x10000U);
		}
	}

	public int Networkm_damageDodgedByEvades
	{
		get
		{
			return this.m_damageDodgedByEvades;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_damageDodgedByEvades, 0x20000U);
		}
	}

	public int Networkm_damageInterceptedByEvades
	{
		get
		{
			return this.m_damageInterceptedByEvades;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_damageInterceptedByEvades, 0x40000U);
		}
	}

	public int Networkm_myIncomingDamageReducedByCover
	{
		get
		{
			return this.m_myIncomingDamageReducedByCover;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myIncomingDamageReducedByCover, 0x80000U);
		}
	}

	public int Networkm_myOutgoingDamageReducedByCover
	{
		get
		{
			return this.m_myOutgoingDamageReducedByCover;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myOutgoingDamageReducedByCover, 0x100000U);
		}
	}

	public int Networkm_myIncomingOverkillDamageTaken
	{
		get
		{
			return this.m_myIncomingOverkillDamageTaken;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myIncomingOverkillDamageTaken, 0x200000U);
		}
	}

	public int Networkm_myOutgoingOverkillDamageDealt
	{
		get
		{
			return this.m_myOutgoingOverkillDamageDealt;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myOutgoingOverkillDamageDealt, 0x400000U);
		}
	}

	public int Networkm_myOutgoingExtraDamageFromEmpowered
	{
		get
		{
			return this.m_myOutgoingExtraDamageFromEmpowered;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myOutgoingExtraDamageFromEmpowered, 0x800000U);
		}
	}

	public int Networkm_myOutgoingDamageReducedFromWeakened
	{
		get
		{
			return this.m_myOutgoingDamageReducedFromWeakened;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_myOutgoingDamageReducedFromWeakened, 0x1000000U);
		}
	}

	public int Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe
	{
		get
		{
			return this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe, 0x2000000U);
		}
	}

	public int Networkm_teamIncomingDamageReducedByWeakenedFromMe
	{
		get
		{
			return this.m_teamIncomingDamageReducedByWeakenedFromMe;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_teamIncomingDamageReducedByWeakenedFromMe, 0x4000000U);
		}
	}

	public int Networkm_teamExtraEnergyGainFromMe
	{
		get
		{
			return this.m_teamExtraEnergyGainFromMe;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_teamExtraEnergyGainFromMe, 0x8000000U);
		}
	}

	public float Networkm_movementDeniedByMe
	{
		get
		{
			return this.m_movementDeniedByMe;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_movementDeniedByMe, 0x10000000U);
		}
	}

	public int Networkm_totalEnemySighted
	{
		get
		{
			return this.m_totalEnemySighted;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalEnemySighted, 0x20000000U);
		}
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
			SyncListUInt.WriteInstance(writer, this.m_syncEnemySourcesForDamageOrDebuff);
			SyncListUInt.WriteInstance(writer, this.m_syncAllySourcesForHealAndBuff);
			writer.WritePackedUInt32((uint)this.m_totalDeaths);
			writer.WritePackedUInt32((uint)this.m_totalPlayerKills);
			writer.WritePackedUInt32((uint)this.m_totalPlayerAssists);
			writer.WritePackedUInt32((uint)this.m_totalPlayerDamage);
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealing);
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealingFromAbility);
			writer.WritePackedUInt32((uint)this.m_totalPlayerOverheal);
			writer.WritePackedUInt32((uint)this.m_totalPlayerAbsorb);
			writer.WritePackedUInt32((uint)this.m_totalPlayerPotentialAbsorb);
			writer.WritePackedUInt32((uint)this.m_totalEnergyGained);
			writer.WritePackedUInt32((uint)this.m_totalPlayerDamageReceived);
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealingReceived);
			writer.WritePackedUInt32((uint)this.m_totalPlayerAbsorbReceived);
			writer.Write(this.m_totalPlayerLockInTime);
			writer.WritePackedUInt32((uint)this.m_totalPlayerTurns);
			writer.WritePackedUInt32((uint)this.m_damageDodgedByEvades);
			writer.WritePackedUInt32((uint)this.m_damageInterceptedByEvades);
			writer.WritePackedUInt32((uint)this.m_myIncomingDamageReducedByCover);
			writer.WritePackedUInt32((uint)this.m_myOutgoingDamageReducedByCover);
			writer.WritePackedUInt32((uint)this.m_myIncomingOverkillDamageTaken);
			writer.WritePackedUInt32((uint)this.m_myOutgoingOverkillDamageDealt);
			writer.WritePackedUInt32((uint)this.m_myOutgoingExtraDamageFromEmpowered);
			writer.WritePackedUInt32((uint)this.m_myOutgoingDamageReducedFromWeakened);
			writer.WritePackedUInt32((uint)this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
			writer.WritePackedUInt32((uint)this.m_teamIncomingDamageReducedByWeakenedFromMe);
			writer.WritePackedUInt32((uint)this.m_teamExtraEnergyGainFromMe);
			writer.Write(this.m_movementDeniedByMe);
			writer.WritePackedUInt32((uint)this.m_totalEnemySighted);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_syncAllySourcesForHealAndBuff);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalDeaths);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerKills);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerAssists);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerDamage);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealing);
		}
		if ((base.syncVarDirtyBits & 0x80U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealingFromAbility);
		}
		if ((base.syncVarDirtyBits & 0x100U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerOverheal);
		}
		if ((base.syncVarDirtyBits & 0x200U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerAbsorb);
		}
		if ((base.syncVarDirtyBits & 0x400U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerPotentialAbsorb);
		}
		if ((base.syncVarDirtyBits & 0x800U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalEnergyGained);
		}
		if ((base.syncVarDirtyBits & 0x1000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerDamageReceived);
		}
		if ((base.syncVarDirtyBits & 0x2000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerHealingReceived);
		}
		if ((base.syncVarDirtyBits & 0x4000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerAbsorbReceived);
		}
		if ((base.syncVarDirtyBits & 0x8000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_totalPlayerLockInTime);
		}
		if ((base.syncVarDirtyBits & 0x10000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalPlayerTurns);
		}
		if ((base.syncVarDirtyBits & 0x20000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_damageDodgedByEvades);
		}
		if ((base.syncVarDirtyBits & 0x40000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_damageInterceptedByEvades);
		}
		if ((base.syncVarDirtyBits & 0x80000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myIncomingDamageReducedByCover);
		}
		if ((base.syncVarDirtyBits & 0x100000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myOutgoingDamageReducedByCover);
		}
		if ((base.syncVarDirtyBits & 0x200000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myIncomingOverkillDamageTaken);
		}
		if ((base.syncVarDirtyBits & 0x400000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myOutgoingOverkillDamageDealt);
		}
		if ((base.syncVarDirtyBits & 0x800000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myOutgoingExtraDamageFromEmpowered);
		}
		if ((base.syncVarDirtyBits & 0x1000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_myOutgoingDamageReducedFromWeakened);
		}
		if ((base.syncVarDirtyBits & 0x2000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
		}
		if ((base.syncVarDirtyBits & 0x4000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_teamIncomingDamageReducedByWeakenedFromMe);
		}
		if ((base.syncVarDirtyBits & 0x8000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_teamExtraEnergyGainFromMe);
		}
		if ((base.syncVarDirtyBits & 0x10000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_movementDeniedByMe);
		}
		if ((base.syncVarDirtyBits & 0x20000000U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalEnemySighted);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, this.m_syncEnemySourcesForDamageOrDebuff);
			SyncListUInt.ReadReference(reader, this.m_syncAllySourcesForHealAndBuff);
			this.m_totalDeaths = (short)reader.ReadPackedUInt32();
			this.m_totalPlayerKills = (short)reader.ReadPackedUInt32();
			this.m_totalPlayerAssists = (short)reader.ReadPackedUInt32();
			this.m_totalPlayerDamage = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerHealing = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerHealingFromAbility = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerOverheal = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerAbsorb = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerPotentialAbsorb = (int)reader.ReadPackedUInt32();
			this.m_totalEnergyGained = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerDamageReceived = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerHealingReceived = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerAbsorbReceived = (int)reader.ReadPackedUInt32();
			this.m_totalPlayerLockInTime = reader.ReadSingle();
			this.m_totalPlayerTurns = (int)reader.ReadPackedUInt32();
			this.m_damageDodgedByEvades = (int)reader.ReadPackedUInt32();
			this.m_damageInterceptedByEvades = (int)reader.ReadPackedUInt32();
			this.m_myIncomingDamageReducedByCover = (int)reader.ReadPackedUInt32();
			this.m_myOutgoingDamageReducedByCover = (int)reader.ReadPackedUInt32();
			this.m_myIncomingOverkillDamageTaken = (int)reader.ReadPackedUInt32();
			this.m_myOutgoingOverkillDamageDealt = (int)reader.ReadPackedUInt32();
			this.m_myOutgoingExtraDamageFromEmpowered = (int)reader.ReadPackedUInt32();
			this.m_myOutgoingDamageReducedFromWeakened = (int)reader.ReadPackedUInt32();
			this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe = (int)reader.ReadPackedUInt32();
			this.m_teamIncomingDamageReducedByWeakenedFromMe = (int)reader.ReadPackedUInt32();
			this.m_teamExtraEnergyGainFromMe = (int)reader.ReadPackedUInt32();
			this.m_movementDeniedByMe = reader.ReadSingle();
			this.m_totalEnemySighted = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_syncEnemySourcesForDamageOrDebuff);
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_syncAllySourcesForHealAndBuff);
		}
		if ((num & 4) != 0)
		{
			this.m_totalDeaths = (short)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			this.m_totalPlayerKills = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
		{
			this.m_totalPlayerAssists = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			this.m_totalPlayerDamage = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) != 0)
		{
			this.m_totalPlayerHealing = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80) != 0)
		{
			this.m_totalPlayerHealingFromAbility = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100) != 0)
		{
			this.m_totalPlayerOverheal = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200) != 0)
		{
			this.m_totalPlayerAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400) != 0)
		{
			this.m_totalPlayerPotentialAbsorb = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800) != 0)
		{
			this.m_totalEnergyGained = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x1000) != 0)
		{
			this.m_totalPlayerDamageReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x2000) != 0)
		{
			this.m_totalPlayerHealingReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x4000) != 0)
		{
			this.m_totalPlayerAbsorbReceived = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000) != 0)
		{
			this.m_totalPlayerLockInTime = reader.ReadSingle();
		}
		if ((num & 0x10000) != 0)
		{
			this.m_totalPlayerTurns = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20000) != 0)
		{
			this.m_damageDodgedByEvades = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40000) != 0)
		{
			this.m_damageInterceptedByEvades = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x80000) != 0)
		{
			this.m_myIncomingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100000) != 0)
		{
			this.m_myOutgoingDamageReducedByCover = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200000) != 0)
		{
			this.m_myIncomingOverkillDamageTaken = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x400000) != 0)
		{
			this.m_myOutgoingOverkillDamageDealt = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x800000) != 0)
		{
			this.m_myOutgoingExtraDamageFromEmpowered = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x1000000) != 0)
		{
			this.m_myOutgoingDamageReducedFromWeakened = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x2000000) != 0)
		{
			this.m_teamOutgoingDamageIncreasedByEmpoweredFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x4000000) != 0)
		{
			this.m_teamIncomingDamageReducedByWeakenedFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x8000000) != 0)
		{
			this.m_teamExtraEnergyGainFromMe = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x10000000) != 0)
		{
			this.m_movementDeniedByMe = reader.ReadSingle();
		}
		if ((num & 0x20000000) != 0)
		{
			this.m_totalEnemySighted = (int)reader.ReadPackedUInt32();
		}
	}
}
