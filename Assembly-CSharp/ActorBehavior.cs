// ROGUES
// SERVER
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class ActorBehavior : NetworkBehaviour, StatDisplaySettings.IPersistatedStatValueSupplier
{
	// added in rogues
#if SERVER
	private List<TurnBehavior> m_turnBehaviorArchive;
#endif

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

#if SERVER
	// added in rogues
	private List<ActorData> m_snareSourceActorsThisTurn = new List<ActorData>();
	// added in rogues
	private List<ActorData> m_rootOrKnockbackSourceActorsThisTurn = new List<ActorData>();
	// added in rogues
	private float m_desiredMovementOnResolve;
	// added in rogues
	private float m_totalMovementLostThisTurn;
	// added in rogues
	private UnresolvedHitpointTrackingData m_unresolvedHealTrackData = new UnresolvedHitpointTrackingData();
	// added in rogues
	private UnresolvedHitpointTrackingData m_unresolvedDamageTrackingData = new UnresolvedHitpointTrackingData();
#endif

	public const string c_debugHeader = "<color=magenta>ActorBehavior: </color>";

	// removed in rogues
	private static int kListm_syncEnemySourcesForDamageOrDebuff = -1894796663;
	// removed in rogues
	private static int kListm_syncAllySourcesForHealAndBuff = 1807084515;

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
    private float NumLifeForStatCalc => Mathf.Max(1f, m_totalDeaths + 1f);
    public float EnergyGainPerTurn => m_totalEnergyGained / NumTurnsForStatCalc;
    public float DamagePerTurn => m_totalPlayerDamage / NumTurnsForStatCalc;
    public float NumEnemiesSightedPerTurn => m_totalEnemySighted / NumTurnsForStatCalc;
    public float HealAndAbsorbPerTurn => (EffectiveHealingFromAbility + m_totalPlayerAbsorb) / NumTurnsForStatCalc;
    public float MovementDeniedPerTurn => m_movementDeniedByMe / NumTurnsForStatCalc;
	// removed in rogues
	public float TeamEnergyBoostedByMePerTurn => teamExtraEnergyGainFromMe / NumTurnsForStatCalc;
	// removed in rogues
	public float TeamDamageSwingPerTurn => (m_teamOutgoingDamageIncreasedByEmpoweredFromMe + m_teamIncomingDamageReducedByWeakenedFromMe) / NumTurnsForStatCalc;
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

    public int EffectiveHealing => Mathf.Max(0, m_totalPlayerHealing - m_totalPlayerOverheal);
    public int EffectiveHealingFromAbility => Mathf.Max(0, m_totalPlayerHealingFromAbility - m_totalPlayerOverheal);
	// removed in rogues
	public float NetDamageDodgedPerLife => netDamageAvoidedByEvades / NumLifeForStatCalc;
	// removed in rogues
	public float IncomingDamageReducedByCoverPerLife => m_myIncomingDamageReducedByCover / NumLifeForStatCalc;

	// added in rogues (see also NetDamageDodgedPerLife)
#if SERVER
	public float DamageDodgedPerLife => m_damageDodgedByEvades / NumLifeForStatCalc;
#endif

    public float DamageTakenPerLife => m_totalPlayerDamageReceived / NumLifeForStatCalc;
    public float AvgLifeSpan => NumTurnsForStatCalc / NumLifeForStatCalc;

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
			if (characterType != null)
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

	public int totalDeathsOnTurnStart => m_totalDeathsOnTurnStart;
	public int serverIncomingDamageReducedByCoverThisTurn => m_serverIncomingDamageReducedByCoverThisTurn;

	// added in rogues
#if SERVER
	public TurnBehavior CurrentTurn
	{
		get
		{
			TurnBehavior result = null;
			if (m_turnBehaviorArchive != null && m_turnBehaviorArchive.Count > 0)
			{
				result = m_turnBehaviorArchive[m_turnBehaviorArchive.Count - 1];
			}
			return result;
		}
	}
#endif

	// added in rogues
#if SERVER
	public TurnBehavior LastTurn
	{
		get
		{
			TurnBehavior result = null;
			if (m_turnBehaviorArchive != null && m_turnBehaviorArchive.Count > 1)
			{
				result = m_turnBehaviorArchive[m_turnBehaviorArchive.Count - 2];
			}
			return result;
		}
	}
#endif

	// added in rogues
#if SERVER
	public TurnBehavior GetBehaviorOfTurn(int turn)
	{
		for (int i = m_turnBehaviorArchive.Count - 1; i >= 0; i--)
		{
			if (m_turnBehaviorArchive[i].m_turn == turn)
			{
				return m_turnBehaviorArchive[i];
			}
		}
		return null;
	}
#endif

	// added in rogues
#if SERVER
	private void OnDeath()
	{
		if (NetworkServer.active)
		{
			ActorData actor = m_actor;
			Networkm_totalDeaths = (short)(m_totalDeaths + 1);
			List<ActorData> list = new List<ActorData>();
			PlayersThatHaveDamagedMeThisTurn(ref list);
			foreach (ActorData actorData in list)
			{
				actorData.SendMessage("OnKill", actor, SendMessageOptions.DontRequireReceiver);
			}
			if (GameplayUtils.IsPlayerControlled(actor))
			{
				foreach (ActorData actorData2 in GameFlowData.Get().GetContributorsToKill(actor, false))
				{
					actorData2.SendMessage("OnAssistedKill", actor, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnDamageDealt(int amount, ActorData target)
	{
		Networkm_totalPlayerDamage = m_totalPlayerDamage + amount;
		TrackUnresolvedDamageToTarget(target, amount);
		if (target.GetActorBehavior() != null)
		{
			target.GetActorBehavior().TrackIncomingUnresolvedDamage(amount);
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnHealingDealt(int amount, bool fromKnownAbility, ActorData target)
	{
		Networkm_totalPlayerHealing = m_totalPlayerHealing + amount;
		if (fromKnownAbility)
		{
			Networkm_totalPlayerHealingFromAbility = m_totalPlayerHealingFromAbility + amount;
			TrackUnresolvedHealingToTarget(target, amount);
			if (target.GetActorBehavior() != null)
			{
				target.GetActorBehavior().TrackIncomingUnresolvedHealFromAbilities(amount);
			}
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnAbsorbDealt(int amount)
	{
		Networkm_totalPlayerAbsorb = m_totalPlayerAbsorb + (short)amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnPotentialAbsorbDealt(int amount)
	{
		Networkm_totalPlayerPotentialAbsorb = m_totalPlayerPotentialAbsorb + (short)amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnOverhealDealt(int amount)
	{
		Networkm_totalPlayerOverheal = m_totalPlayerOverheal + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnOverkillDamageDealt(int amount)
	{
		Networkm_myOutgoingOverkillDamageDealt = m_myOutgoingOverkillDamageDealt + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnEnergyGained(int amount)
	{
		Networkm_totalEnergyGained = m_totalEnergyGained + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnDamageReceived(int amount)
	{
		Networkm_totalPlayerDamageReceived = m_totalPlayerDamageReceived + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnHealingReceived(int amount)
	{
		Networkm_totalPlayerHealingReceived = m_totalPlayerHealingReceived + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnAbsorbReceived(int amount)
	{
		Networkm_totalPlayerAbsorbReceived = m_totalPlayerAbsorbReceived + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnResolutionStart(float seconds)
	{
		Networkm_totalPlayerLockInTime = m_totalPlayerLockInTime + seconds;
		Networkm_totalPlayerTurns = m_totalPlayerTurns + 1;
	}
#endif

	// added in rogues
#if SERVER
	public void OnOverkillDamageReceived(int amount)
	{
		Networkm_myIncomingOverkillDamageTaken = m_myIncomingOverkillDamageTaken + amount;
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedDamageTakenWithEvadesStats(ServerGameplayUtils.DamageDodgedStats stats)
	{
		if (stats != null)
		{
			if (stats.m_damageDodged != 0)
			{
				Networkm_damageDodgedByEvades = m_damageDodgedByEvades + stats.m_damageDodged;
			}
			if (stats.m_damageIntercepted != 0)
			{
				Networkm_damageInterceptedByEvades = m_damageInterceptedByEvades + stats.m_damageIntercepted;
			}
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedDamageAdjustmentsOfHitByMe(int damageAddedFromEmpowered, int damageMitigatedFromWeakened, int damageAddedFromVulnerable, int damageMitigatedFromArmored, int damageMitigatedFromCover)
	{
		if (damageAddedFromEmpowered != 0)
		{
			Networkm_myOutgoingExtraDamageFromEmpowered = m_myOutgoingExtraDamageFromEmpowered + damageAddedFromEmpowered;
		}
		if (damageMitigatedFromWeakened != 0)
		{
			Networkm_myOutgoingDamageReducedFromWeakened = m_myOutgoingDamageReducedFromWeakened + damageMitigatedFromWeakened;
		}
		if (damageAddedFromVulnerable != 0)
		{
			m_myOutgoingExtraDamageFromTargetsVulnerable += damageAddedFromVulnerable;
		}
		if (damageMitigatedFromArmored != 0)
		{
			m_myOutgoingDamageReducedFromTargetsArmored += damageMitigatedFromArmored;
		}
		if (damageMitigatedFromCover != 0)
		{
			Networkm_myOutgoingDamageReducedByCover = m_myOutgoingDamageReducedByCover + damageMitigatedFromCover;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedDamageAdjustmentsOfHitOnMe(int damageAddedFromEmpowered, int damageMitigatedFromWeakened, int damageAddedFromVulnerable, int damageMitigatedFromArmored, int damageMitigatedFromCover)
	{
		if (damageAddedFromEmpowered != 0)
		{
			m_myIncomingExtraDamageFromCastersEmpowered += damageAddedFromEmpowered;
		}
		if (damageMitigatedFromWeakened != 0)
		{
			m_myIncomingDamageReducedFromCastersWeakened += damageMitigatedFromWeakened;
		}
		if (damageAddedFromVulnerable != 0)
		{
			m_myIncomingExtraDamageIncreasedByVulnerable += damageAddedFromVulnerable;
		}
		if (damageMitigatedFromArmored != 0)
		{
			m_myIncomingDamageReducedByArmored += damageMitigatedFromArmored;
		}
		if (damageMitigatedFromCover != 0)
		{
			Networkm_myIncomingDamageReducedByCover = m_myIncomingDamageReducedByCover + damageMitigatedFromCover;
		}
		m_serverIncomingDamageReducedByCoverThisTurn += damageMitigatedFromCover;
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedExtraDamageFromEmpoweredGrantedByMe(int damageAddedFromEmpowered)
	{
		if (damageAddedFromEmpowered != 0)
		{
			Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = m_teamOutgoingDamageIncreasedByEmpoweredFromMe + damageAddedFromEmpowered;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedDamageReducedFromWeakenedGrantedByMe(int damageMitigatedFromWeakened)
	{
		if (damageMitigatedFromWeakened != 0)
		{
			Networkm_teamIncomingDamageReducedByWeakenedFromMe = m_teamIncomingDamageReducedByWeakenedFromMe + damageMitigatedFromWeakened;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedDamageReducedFromArmoredGrantedByMe(int damageMitigatedFromArmored)
	{
		if (damageMitigatedFromArmored != 0)
		{
			m_teamIncomingDamageReducedByArmoredFromMe += damageMitigatedFromArmored;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedExtraDamageFromVulnerableGrantedByMe(int damageAddedFromVulnerable)
	{
		if (damageAddedFromVulnerable != 0)
		{
			m_teamOutgoingDamageIncreasedByVulnerableFromMe += damageAddedFromVulnerable;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCalculatedExtraEnergyByMe(int toAdd)
	{
		if (toAdd > 0)
		{
			Networkm_teamExtraEnergyGainFromMe = m_teamExtraEnergyGainFromMe + toAdd;
		}
	}
#endif

	// added in rogues
#if SERVER
	public void AccumulateMovementDeniedByMe(float toAdd)
	{
		if (toAdd > 0f)
		{
			Networkm_movementDeniedByMe = m_movementDeniedByMe + toAdd;
			if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
			{
				GameplayMetricHelper.DebugLogMoveDenied(string.Concat(new object[]
				{
					m_actor.DebugNameString(),
					" accumulating movement denied stat, amount toAdd= ",
					toAdd,
					" | total= ",
					m_movementDeniedByMe
				}));
			}
			if (m_actor.GetPassiveData() != null)
			{
				m_actor.GetPassiveData().OnAccumulatedMovementDeniedByMe(toAdd);
			}
		}
	}
#endif

#if SERVER
	// added in rogues
	private void OnKill(ActorData killedActor)
	{
		if (NetworkServer.active)
		{
			if (GameplayUtils.IsPlayerControlled(killedActor))
			{
				Networkm_totalPlayerKills = (short)(m_totalPlayerKills + 1);
				return;
			}
			m_totalMinionKills += 1;
		}
	}

	// added in rogues
	private void OnAssistedKill(ActorData killedActor)
	{
		if (NetworkServer.active)
		{
			Networkm_totalPlayerAssists = (short)(m_totalPlayerAssists + 1);
		}
	}

	// added in rogues
	private void PlayersThatHaveDamagedMeThisTurn(ref List<ActorData> participants)
	{
		PlayersThatHaveDamagedMe(0, ref participants);
	}

	// added in rogues
	private void PlayersThatHaveDamagedMe(int turns, ref List<ActorData> participants)
	{
		ActorData actor = m_actor;
		foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(actor.GetEnemyTeam()))
		{
			if (GameplayUtils.IsPlayerControlled(actorData))
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				int num = currentTurn - turns;
				for (int i = currentTurn; i >= num; i--)
				{
					TurnBehavior behaviorOfTurn = GetBehaviorOfTurn(i);
					if (behaviorOfTurn != null && behaviorOfTurn.DamagedByActor(actorData))
					{
						participants.Add(actorData);
						break;
					}
				}
			}
		}
	}

	// added in rogues
	public void OnTurnStart()
	{
		if (NetworkServer.active)
		{
			m_totalDeathsOnTurnStart = m_totalDeaths;
			TurnBehavior item = new TurnBehavior(this, m_turnBehaviorArchive.Count, GameFlowData.Get().CurrentTurn);
			m_turnBehaviorArchive.Add(item);
			if (m_turnBehaviorArchive.Count > GameWideData.Get().m_killAssistMemory + 1)
			{
				m_turnBehaviorArchive.Remove(m_turnBehaviorArchive[0]);
			}
			m_serverIncomingDamageReducedByCoverThisTurn = 0;
			ClearMovementDebuffTracking();
		}
	}

	// added in rogues
	public void SetKillAssistSyncLists()
	{
		int currentTurn = GameFlowData.Get().CurrentTurn;
		int num = Mathf.Max(1, currentTurn - GameWideData.Get().m_killAssistMemory);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		if (actors != null)
		{
			foreach (ActorData actorData in actors)
			{
				if (actorData != null && actorData.ActorIndex >= 0 && GameplayUtils.IsPlayerControlled(actorData))
				{
					for (int i = currentTurn - 1; i >= num; i--)
					{
						TurnBehavior behaviorOfTurn = GetBehaviorOfTurn(i);
						if (behaviorOfTurn != null)
						{
							if (actorData.GetTeam() != m_actor.GetTeam())
							{
								if (behaviorOfTurn.DamagedByActor(actorData) || behaviorOfTurn.DebuffedByActor(actorData))
								{
									list.Add(actorData.ActorIndex);
								}
							}
							else if (behaviorOfTurn.HealedByActor(actorData) || behaviorOfTurn.BuffedByActor(actorData))
							{
								list2.Add(actorData.ActorIndex);
							}
						}
					}
				}
			}
		}
		for (int j = m_syncEnemySourcesForDamageOrDebuff.Count - 1; j >= 0; j--)
		{
			if (!list.Contains((int)m_syncEnemySourcesForDamageOrDebuff[j]))
			{
				m_syncEnemySourcesForDamageOrDebuff.RemoveAt(j);
			}
		}
		for (int k = m_syncAllySourcesForHealAndBuff.Count - 1; k >= 0; k--)
		{
			if (!list2.Contains((int)m_syncAllySourcesForHealAndBuff[k]))
			{
				m_syncAllySourcesForHealAndBuff.RemoveAt(k);
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			m_syncEnemySourcesForDamageOrDebuff.Add((uint)list[l]);
		}
		for (int m = 0; m < list2.Count; m++)
		{
			m_syncAllySourcesForHealAndBuff.Add((uint)list2[m]);
		}
	}

	// added in rogues
	public void OnSetDestination(BoardSquare destination)
	{
		if (NetworkServer.active && CurrentTurn != null)
		{
			BoardSquare currentBoardSquare = m_actor.GetCurrentBoardSquare();
			if (destination != currentBoardSquare && destination != null)
			{
				CurrentTurn.MoveDestination = destination;
				if (CurrentTurn.ChaseTargetActor != null && CurrentTurn.ChaseTargetActor.gameObject != destination.occupant)
				{
					CurrentTurn.ChaseTargetActor = null;
					return;
				}
			}
			else
			{
				CurrentTurn.MoveDestination = null;
				CurrentTurn.ChaseTargetActor = null;
			}
		}
	}

	// added in rogues
	private void RemoveReferencesToDestroyedActor(ActorData actor)
	{
		foreach (TurnBehavior turnBehavior in m_turnBehaviorArchive)
		{
			if (turnBehavior.m_damageToActors.ContainsKey(actor.ActorIndex))
			{
				string displayName = actor.DisplayName;
				int num = turnBehavior.m_damageToActors[actor.ActorIndex];
				if (turnBehavior.m_damageToDestroyedActors.ContainsKey(displayName))
				{
					Dictionary<string, int> dictionary = turnBehavior.m_damageToDestroyedActors;
					string key = displayName;
					dictionary[key] += num;
				}
				else
				{
					turnBehavior.m_damageToDestroyedActors.Add(displayName, num);
				}
				turnBehavior.m_damageToActors.Remove(actor.ActorIndex);
			}
			if (turnBehavior.m_healingToActors.ContainsKey(actor.ActorIndex))
			{
				string displayName2 = actor.DisplayName;
				int num2 = turnBehavior.m_healingToActors[actor.ActorIndex];
				if (turnBehavior.m_healingToDestroyedActors.ContainsKey(displayName2))
				{
					Dictionary<string, int> dictionary = turnBehavior.m_healingToDestroyedActors;
					string key = displayName2;
					dictionary[key] += num2;
				}
				else
				{
					turnBehavior.m_healingToDestroyedActors.Add(displayName2, num2);
				}
				turnBehavior.m_healingToActors.Remove(actor.ActorIndex);
			}
			if (turnBehavior.m_actorsResponsibleForEffects.Contains(actor))
			{
				turnBehavior.m_actorsResponsibleForEffects.Remove(actor);
			}
		}
	}

	// added in rogues
	private void ClearMovementDebuffTracking()
	{
		m_snareSourceActorsThisTurn.Clear();
		m_rootOrKnockbackSourceActorsThisTurn.Clear();
		m_desiredMovementOnResolve = 0f;
		m_totalMovementLostThisTurn = 0f;
	}

	// added in rogues
	public float DesiredMovementOnResolve => m_desiredMovementOnResolve;

	// added in rogues
	public void AddSnareSourceActor(ActorData sourceActor)
	{
		if (!m_snareSourceActorsThisTurn.Contains(sourceActor))
		{
			m_snareSourceActorsThisTurn.Add(sourceActor);
		}
	}

	// added in rogues
	public void AddRootOrKnockbackSourceActor(ActorData sourceActor)
	{
		if (!m_rootOrKnockbackSourceActorsThisTurn.Contains(sourceActor))
		{
			m_rootOrKnockbackSourceActorsThisTurn.Add(sourceActor);
		}
	}

	// added in rogues
	public void TrackDesiredMovementOnResolveStart(float desiredAmount)
	{
		if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
		{
			GameplayMetricHelper.DebugLogMoveDenied(m_actor.DebugNameString() + " | setting desired movement amount on resolve start, value= " + desiredAmount);
		}
		m_desiredMovementOnResolve = desiredAmount;
	}

	// added in rogues
	public void TrackPathLostDuringStabilization(float lostAmount)
	{
		if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
		{
			GameplayMetricHelper.DebugLogMoveDenied(string.Concat(new object[]
			{
				m_actor.DebugNameString(),
				" | incrementing total movement lost, current= ",
				m_totalMovementLostThisTurn,
				", increment= ",
				lostAmount
			}));
		}
		m_totalMovementLostThisTurn += lostAmount;
	}

	// added in rogues
	public void SetTotalMovementLostThisTurn(float lostAmount)
	{
		if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
		{
			GameplayMetricHelper.DebugLogMoveDenied(m_actor.DebugNameString() + " | Setting total movement lost this turn to " + lostAmount);
		}
		m_totalMovementLostThisTurn = lostAmount;
	}

	// added in rogues
	public void TrackMovementLostForDeadActor()
	{
		if (m_actor == null || !m_actor.IsDead())
		{
			return;
		}
		float desiredMovementOnResolve = DesiredMovementOnResolve;
		float num = m_actor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
		if (m_actor.GetAbilityData() != null)
		{
			num += m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjust();
		}
		num += m_actor.GetActorMovement().MoveRangeCompensation;
		float adjustedMovementFromBuffAndDebuff = m_actor.GetActorMovement().GetAdjustedMovementFromBuffAndDebuff(num, false, false);
		if (desiredMovementOnResolve > adjustedMovementFromBuffAndDebuff + 0.5f)
		{
			float totalMovementLostThisTurn = desiredMovementOnResolve - adjustedMovementFromBuffAndDebuff;
			SetTotalMovementLostThisTurn(totalMovementLostThisTurn);
		}
	}

	// added in rogues
	public void ProcessMovementDeniedStat()
	{
		if (m_totalMovementLostThisTurn > 0f)
		{
			if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
			{
				GameplayMetricHelper.DebugLogMoveDenied(string.Concat(new object[]
				{
					m_actor.DebugNameString(),
					" | Processing Movement Denied Stat, total= ",
					m_totalMovementLostThisTurn,
					"\nNum Root or Knockback sources= ",
					m_rootOrKnockbackSourceActorsThisTurn.Count,
					"\nNum Snare sources= ",
					m_snareSourceActorsThisTurn.Count
				}));
			}
			List<ActorData> list = null;
			if (m_rootOrKnockbackSourceActorsThisTurn.Count > 0)
			{
				list = m_rootOrKnockbackSourceActorsThisTurn;
			}
			else if (m_snareSourceActorsThisTurn.Count > 0)
			{
				list = m_snareSourceActorsThisTurn;
			}
			if (list != null && list.Count > 0)
			{
				float toAdd = m_totalMovementLostThisTurn / list.Count;
				foreach (ActorData actorData in list)
				{
					if (actorData.GetActorBehavior() != null)
					{
						actorData.GetActorBehavior().AccumulateMovementDeniedByMe(toAdd);
					}
				}
			}
		}
	}

	// added in rogues
	public void ProcessEnemySightedStat()
	{
		BoardSquare currentBoardSquare = m_actor.GetCurrentBoardSquare();
		if (!m_actor.IsDead() && currentBoardSquare != null)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetActorsVisibleToActor(m_actor, false))
			{
				BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
				if (actorData.GetTeam() != m_actor.GetTeam() && currentBoardSquare2 != null && currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2) <= m_actor.GetSightRange() && currentBoardSquare.GetLOS(currentBoardSquare2.x, currentBoardSquare2.y))
				{
					Networkm_totalEnemySighted = m_totalEnemySighted + 1;
				}
			}
		}
	}

	// added in rogues
	public int GetIncomingUnresolvedHealFromAbilities()
	{
		return m_unresolvedHealTrackData.GetTotalChangeOnMe();
	}

	// added in rogues
	private void TrackIncomingUnresolvedHealFromAbilities(int amount)
	{
		m_unresolvedHealTrackData.IncrementTotalOnMe(amount);
	}

	// added in rogues
	public void TrackUnresolvedHealingToTarget(ActorData healTarget, int healAmount)
	{
		m_unresolvedHealTrackData.IncrementChangeToTarget(healTarget, healAmount);
	}

	// added in rogues
	public int GetUnresolvedHealToTarget(ActorData healTarget)
	{
		return m_unresolvedHealTrackData.GetChangeToTarget(healTarget);
	}

	// added in rogues
	public void ClearUnresolvedHitpointsTracking()
	{
		m_unresolvedHealTrackData.Reset();
		m_unresolvedDamageTrackingData.Reset();
	}

	public int GetIncomingUnresolvedDamage()
	{
		return m_unresolvedDamageTrackingData.GetTotalChangeOnMe();
	}

	// added in rogues
	public void TrackIncomingUnresolvedDamage(int amount)
	{
		m_unresolvedDamageTrackingData.IncrementTotalOnMe(amount);
	}

	// added in rogues
	public void TrackUnresolvedDamageToTarget(ActorData target, int amount)
	{
		m_unresolvedDamageTrackingData.IncrementChangeToTarget(target, amount);
	}

	// added in rogues
	public int GetUnresolvedDamageToTarget(ActorData damageTarget)
	{
		return m_unresolvedDamageTrackingData.GetChangeToTarget(damageTarget);
	}
#endif

	public bool DebugTraceClientContribution
	{
		get
		{
			bool isEditor = Application.isEditor;
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
			//base.SetSyncVar<short>(value, ref m_totalDeaths, 1UL);
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
			//base.SetSyncVar<short>(value, ref m_totalPlayerKills, 2UL);
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
			//base.SetSyncVar<short>(value, ref m_totalPlayerAssists, 4UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerDamage, 8UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerHealing, 16UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerHealingFromAbility, 32UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerOverheal, 64UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerAbsorb, 128UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerPotentialAbsorb, 256UL);
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
			//base.SetSyncVar<int>(value, ref m_totalEnergyGained, 512UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerDamageReceived, 1024UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerHealingReceived, 2048UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerAbsorbReceived, 4096UL);
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
			//base.SetSyncVar<float>(value, ref m_totalPlayerLockInTime, 8192UL);
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
			//base.SetSyncVar<int>(value, ref m_totalPlayerTurns, 16384UL);
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
			//base.SetSyncVar<int>(value, ref m_damageDodgedByEvades, 32768UL);
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
			//base.SetSyncVar<int>(value, ref m_damageInterceptedByEvades, 65536UL);
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
			//base.SetSyncVar<int>(value, ref m_myIncomingDamageReducedByCover, 131072UL);
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
			//base.SetSyncVar<int>(value, ref m_myOutgoingDamageReducedByCover, 262144UL);
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
			//base.SetSyncVar<int>(value, ref m_myIncomingOverkillDamageTaken, 524288UL);
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
			//base.SetSyncVar<int>(value, ref m_myOutgoingOverkillDamageDealt, 1048576UL);
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
			//base.SetSyncVar<int>(value, ref m_myOutgoingExtraDamageFromEmpowered, 2097152UL);
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
			//base.SetSyncVar<int>(value, ref m_myOutgoingDamageReducedFromWeakened, 4194304UL);
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
			//base.SetSyncVar<int>(value, ref m_teamOutgoingDamageIncreasedByEmpoweredFromMe, 8388608UL);
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
			//base.SetSyncVar<int>(value, ref m_teamIncomingDamageReducedByWeakenedFromMe, 16777216UL);
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
			//base.SetSyncVar<int>(value, ref m_teamExtraEnergyGainFromMe, 33554432UL);
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
			//base.SetSyncVar<float>(value, ref m_movementDeniedByMe, 67108864UL);
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
			//base.SetSyncVar<int>(value, ref m_totalEnemySighted, 134217728UL);
		}
	}

	// removed in rogues
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
			string.Format(StringUtil.TR("TotalContribution", "Global"), totalPlayerContribution)
			+ text
			+ string.Format(StringUtil.TR("DamageContribution", "Global"), totalPlayerDamage)
			+ text
			+ string.Format(StringUtil.TR("HealingContribution", "Global"), totalPlayerHealingFromAbility, effectiveHealingFromAbility)
			+ text
			+ string.Format(StringUtil.TR("ShieldingContribution", "Global"), totalPlayerAbsorb, totalPlayerPotentialAbsorb)
			+ text
			+ string.Format(StringUtil.TR("DamageReceivedContribution", "Global"), totalPlayerDamageReceived)
			+ text
			+ string.Format(StringUtil.TR("HealingReceivedContribution", "Global"), totalPlayerHealingReceived)
			+ text
			+ string.Format(StringUtil.TR("ShieldingDamageContribution", "Global"), totalPlayerAbsorbReceived)
			+ text;
	}

	private void Awake()
	{
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			m_turnBehaviorArchive = new List<TurnBehavior>();
		}
#endif
		m_actor = GetComponent<ActorData>();

		// removed in rogues
		m_syncEnemySourcesForDamageOrDebuff.InitializeBehaviour(this, kListm_syncEnemySourcesForDamageOrDebuff);
		m_syncAllySourcesForHealAndBuff.InitializeBehaviour(this, kListm_syncAllySourcesForHealAndBuff);
	}

	private void Start()
	{
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			GameFlowData.s_onRemoveActor += RemoveReferencesToDestroyedActor;
		}
#endif
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
				return NetDamageDodgedPerLife; //  netDamageAvoidedByEvades in rogues
			case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
				return IncomingDamageReducedByCoverPerLife;  // m_myIncomingDamageReducedByCover in rogues
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
				return TeamDamageSwingPerTurn; //  (float)(m_teamOutgoingDamageIncreasedByEmpoweredFromMe + m_teamIncomingDamageReducedByWeakenedFromMe) in rogues
			case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
				return TeamEnergyBoostedByMePerTurn; // teamExtraEnergyGainFromMe in rogues
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

	// server-only -- empty in reactor
	private void DisplayActorBehavior()
	{
#if SERVER
		if (Application.isEditor
			&& NetworkServer.active
			&& DebugParameters.Get() != null
			&& DebugParameters.Get().GetParameterAsBool("DisplayActorBehavior"))
		{
			if (LastTurn == null)
			{
				string displayStr = "Last Turn: (null)";
				UIActorDebugPanel.Get().SetActorValue(m_actor, "DisplayActorBehavior", displayStr);
				return;
			}
			string displayStr2 = "Last Turn: " + LastTurn.ToString();
			UIActorDebugPanel.Get().SetActorValue(m_actor, "DisplayActorBehavior", displayStr2);
		}
#endif
	}

	public string GetGeneralStatDebugString()
	{
		string text = "";
		text += "Total Kills = " + totalPlayerKills + "\n";
		text += "Total Assists = " + totalPlayerAssists + "\n";
		text += "Total Deaths = " + totalDeaths + "\n";
		text += "\nTotal Damage = " + totalPlayerDamage + "\n";
		text += "ExtraDamage From Might = " + myOutgoingExtraDamageFromEmpowered + "\n";
		text += "LostDamage Due to Cover = " + myOutgoingDamageReducedByCover + "\n";
		text += "LostDamage due to Weaken = " + myOutgoingReducedDamageFromWeakened + "\n";
		text += "Overkill Damage Dealt = " + myOutgoingOverkillDamageDealt + "\n";
		text += "\nDamage Taken = " + totalPlayerDamageReceived + "\n";
		text += "Net Damage Dodged By Evades = " + netDamageAvoidedByEvades + "\n";
		text += "\nTotal Healing = " + totalPlayerHealing + "\n";
		text += "Overheal = " + totalPlayerOverheal + "\n";
		text += "Shielding Dealt, Effective = " + totalPlayerAbsorb + ", Total = " + totalPlayerPotentialAbsorb + "\n";
		text += "Team Outgoing ExtraDamage from my Mighted = " + teamOutgoingDamageIncreasedByEmpoweredFromMe + "\n";
		text += "Team Incoming LostDamage from my Weakened = " + teamIncomingDamageReducedByWeakenedFromMe + "\n";
		text += "\nEnergy Gain = " + totalEnergyGained + "\n";
		text += "Team Extra Energy from Me (energized + direct) = " + teamExtraEnergyGainFromMe + "\n";
		text += "\nMovement Denied by me = " + movementDeniedByMe + "\n";
		text += "\nNum Enemies Sighted Total = " + totalEnemySighted + "\n";
		text += "\nAverage Life Span = " + AvgLifeSpan + "\n";
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
				Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.DebugNameString("white") + " recording EFFECT from " + caster.DebugNameString("yellow"));
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
				Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.DebugNameString("white") + " recording DAMAGE from " + caster.DebugNameString("yellow"));
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
				Debug.LogWarning("<color=magenta>ActorBehavior: </color>" + m_actor.DebugNameString("white") + " recording HEALING from " + caster.DebugNameString("yellow"));
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

	// added in rogues
	//public ActorBehavior()
	//{
	//	base.InitSyncObject(m_syncEnemySourcesForDamageOrDebuff);
	//	base.InitSyncObject(m_syncAllySourcesForHealAndBuff);
	//}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	protected static void InvokeSyncListm_syncEnemySourcesForDamageOrDebuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncEnemySourcesForDamageOrDebuff called on server.");
			return;
		}
		((ActorBehavior)obj).m_syncEnemySourcesForDamageOrDebuff.HandleMsg(reader);
	}

	// removed in rogues
	protected static void InvokeSyncListm_syncAllySourcesForHealAndBuff(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_syncAllySourcesForHealAndBuff called on server.");
			return;
		}
		((ActorBehavior)obj).m_syncAllySourcesForHealAndBuff.HandleMsg(reader);
	}

	// reactor
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

	// reactor
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

	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.Write(m_totalDeaths);
	//		writer.Write(m_totalPlayerKills);
	//		writer.Write(m_totalPlayerAssists);
	//		writer.WritePackedInt32(m_totalPlayerDamage);
	//		writer.WritePackedInt32(m_totalPlayerHealing);
	//		writer.WritePackedInt32(m_totalPlayerHealingFromAbility);
	//		writer.WritePackedInt32(m_totalPlayerOverheal);
	//		writer.WritePackedInt32(m_totalPlayerAbsorb);
	//		writer.WritePackedInt32(m_totalPlayerPotentialAbsorb);
	//		writer.WritePackedInt32(m_totalEnergyGained);
	//		writer.WritePackedInt32(m_totalPlayerDamageReceived);
	//		writer.WritePackedInt32(m_totalPlayerHealingReceived);
	//		writer.WritePackedInt32(m_totalPlayerAbsorbReceived);
	//		writer.Write(m_totalPlayerLockInTime);
	//		writer.WritePackedInt32(m_totalPlayerTurns);
	//		writer.WritePackedInt32(m_damageDodgedByEvades);
	//		writer.WritePackedInt32(m_damageInterceptedByEvades);
	//		writer.WritePackedInt32(m_myIncomingDamageReducedByCover);
	//		writer.WritePackedInt32(m_myOutgoingDamageReducedByCover);
	//		writer.WritePackedInt32(m_myIncomingOverkillDamageTaken);
	//		writer.WritePackedInt32(m_myOutgoingOverkillDamageDealt);
	//		writer.WritePackedInt32(m_myOutgoingExtraDamageFromEmpowered);
	//		writer.WritePackedInt32(m_myOutgoingDamageReducedFromWeakened);
	//		writer.WritePackedInt32(m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
	//		writer.WritePackedInt32(m_teamIncomingDamageReducedByWeakenedFromMe);
	//		writer.WritePackedInt32(m_teamExtraEnergyGainFromMe);
	//		writer.Write(m_movementDeniedByMe);
	//		writer.WritePackedInt32(m_totalEnemySighted);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.Write(m_totalDeaths);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2UL) != 0UL)
	//	{
	//		writer.Write(m_totalPlayerKills);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4UL) != 0UL)
	//	{
	//		writer.Write(m_totalPlayerAssists);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerDamage);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerHealing);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 32UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerHealingFromAbility);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 64UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerOverheal);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 128UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerAbsorb);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 256UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerPotentialAbsorb);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 512UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalEnergyGained);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1024UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerDamageReceived);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2048UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerHealingReceived);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4096UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerAbsorbReceived);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8192UL) != 0UL)
	//	{
	//		writer.Write(m_totalPlayerLockInTime);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16384UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalPlayerTurns);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 32768UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_damageDodgedByEvades);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 65536UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_damageInterceptedByEvades);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 131072UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myIncomingDamageReducedByCover);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 262144UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myOutgoingDamageReducedByCover);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 524288UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myIncomingOverkillDamageTaken);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 1048576UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myOutgoingOverkillDamageDealt);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2097152UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myOutgoingExtraDamageFromEmpowered);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4194304UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_myOutgoingDamageReducedFromWeakened);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8388608UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_teamOutgoingDamageIncreasedByEmpoweredFromMe);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16777216UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_teamIncomingDamageReducedByWeakenedFromMe);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 33554432UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_teamExtraEnergyGainFromMe);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 67108864UL) != 0UL)
	//	{
	//		writer.Write(m_movementDeniedByMe);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 134217728UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_totalEnemySighted);
	//		result = true;
	//	}
	//	return result;
	//}

	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		short networkm_totalDeaths = reader.ReadInt16();
	//		Networkm_totalDeaths = networkm_totalDeaths;
	//		short networkm_totalPlayerKills = reader.ReadInt16();
	//		Networkm_totalPlayerKills = networkm_totalPlayerKills;
	//		short networkm_totalPlayerAssists = reader.ReadInt16();
	//		Networkm_totalPlayerAssists = networkm_totalPlayerAssists;
	//		int networkm_totalPlayerDamage = reader.ReadPackedInt32();
	//		Networkm_totalPlayerDamage = networkm_totalPlayerDamage;
	//		int networkm_totalPlayerHealing = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealing = networkm_totalPlayerHealing;
	//		int networkm_totalPlayerHealingFromAbility = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealingFromAbility = networkm_totalPlayerHealingFromAbility;
	//		int networkm_totalPlayerOverheal = reader.ReadPackedInt32();
	//		Networkm_totalPlayerOverheal = networkm_totalPlayerOverheal;
	//		int networkm_totalPlayerAbsorb = reader.ReadPackedInt32();
	//		Networkm_totalPlayerAbsorb = networkm_totalPlayerAbsorb;
	//		int networkm_totalPlayerPotentialAbsorb = reader.ReadPackedInt32();
	//		Networkm_totalPlayerPotentialAbsorb = networkm_totalPlayerPotentialAbsorb;
	//		int networkm_totalEnergyGained = reader.ReadPackedInt32();
	//		Networkm_totalEnergyGained = networkm_totalEnergyGained;
	//		int networkm_totalPlayerDamageReceived = reader.ReadPackedInt32();
	//		Networkm_totalPlayerDamageReceived = networkm_totalPlayerDamageReceived;
	//		int networkm_totalPlayerHealingReceived = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealingReceived = networkm_totalPlayerHealingReceived;
	//		int networkm_totalPlayerAbsorbReceived = reader.ReadPackedInt32();
	//		Networkm_totalPlayerAbsorbReceived = networkm_totalPlayerAbsorbReceived;
	//		float networkm_totalPlayerLockInTime = reader.ReadSingle();
	//		Networkm_totalPlayerLockInTime = networkm_totalPlayerLockInTime;
	//		int networkm_totalPlayerTurns = reader.ReadPackedInt32();
	//		Networkm_totalPlayerTurns = networkm_totalPlayerTurns;
	//		int networkm_damageDodgedByEvades = reader.ReadPackedInt32();
	//		Networkm_damageDodgedByEvades = networkm_damageDodgedByEvades;
	//		int networkm_damageInterceptedByEvades = reader.ReadPackedInt32();
	//		Networkm_damageInterceptedByEvades = networkm_damageInterceptedByEvades;
	//		int networkm_myIncomingDamageReducedByCover = reader.ReadPackedInt32();
	//		Networkm_myIncomingDamageReducedByCover = networkm_myIncomingDamageReducedByCover;
	//		int networkm_myOutgoingDamageReducedByCover = reader.ReadPackedInt32();
	//		Networkm_myOutgoingDamageReducedByCover = networkm_myOutgoingDamageReducedByCover;
	//		int networkm_myIncomingOverkillDamageTaken = reader.ReadPackedInt32();
	//		Networkm_myIncomingOverkillDamageTaken = networkm_myIncomingOverkillDamageTaken;
	//		int networkm_myOutgoingOverkillDamageDealt = reader.ReadPackedInt32();
	//		Networkm_myOutgoingOverkillDamageDealt = networkm_myOutgoingOverkillDamageDealt;
	//		int networkm_myOutgoingExtraDamageFromEmpowered = reader.ReadPackedInt32();
	//		Networkm_myOutgoingExtraDamageFromEmpowered = networkm_myOutgoingExtraDamageFromEmpowered;
	//		int networkm_myOutgoingDamageReducedFromWeakened = reader.ReadPackedInt32();
	//		Networkm_myOutgoingDamageReducedFromWeakened = networkm_myOutgoingDamageReducedFromWeakened;
	//		int networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = reader.ReadPackedInt32();
	//		Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe;
	//		int networkm_teamIncomingDamageReducedByWeakenedFromMe = reader.ReadPackedInt32();
	//		Networkm_teamIncomingDamageReducedByWeakenedFromMe = networkm_teamIncomingDamageReducedByWeakenedFromMe;
	//		int networkm_teamExtraEnergyGainFromMe = reader.ReadPackedInt32();
	//		Networkm_teamExtraEnergyGainFromMe = networkm_teamExtraEnergyGainFromMe;
	//		float networkm_movementDeniedByMe = reader.ReadSingle();
	//		Networkm_movementDeniedByMe = networkm_movementDeniedByMe;
	//		int networkm_totalEnemySighted = reader.ReadPackedInt32();
	//		Networkm_totalEnemySighted = networkm_totalEnemySighted;
	//		return;
	//	}
	//	long num = (long)reader.ReadPackedUInt64();
	//	if ((num & 1L) != 0L)
	//	{
	//		short networkm_totalDeaths2 = reader.ReadInt16();
	//		Networkm_totalDeaths = networkm_totalDeaths2;
	//	}
	//	if ((num & 2L) != 0L)
	//	{
	//		short networkm_totalPlayerKills2 = reader.ReadInt16();
	//		Networkm_totalPlayerKills = networkm_totalPlayerKills2;
	//	}
	//	if ((num & 4L) != 0L)
	//	{
	//		short networkm_totalPlayerAssists2 = reader.ReadInt16();
	//		Networkm_totalPlayerAssists = networkm_totalPlayerAssists2;
	//	}
	//	if ((num & 8L) != 0L)
	//	{
	//		int networkm_totalPlayerDamage2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerDamage = networkm_totalPlayerDamage2;
	//	}
	//	if ((num & 16L) != 0L)
	//	{
	//		int networkm_totalPlayerHealing2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealing = networkm_totalPlayerHealing2;
	//	}
	//	if ((num & 32L) != 0L)
	//	{
	//		int networkm_totalPlayerHealingFromAbility2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealingFromAbility = networkm_totalPlayerHealingFromAbility2;
	//	}
	//	if ((num & 64L) != 0L)
	//	{
	//		int networkm_totalPlayerOverheal2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerOverheal = networkm_totalPlayerOverheal2;
	//	}
	//	if ((num & 128L) != 0L)
	//	{
	//		int networkm_totalPlayerAbsorb2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerAbsorb = networkm_totalPlayerAbsorb2;
	//	}
	//	if ((num & 256L) != 0L)
	//	{
	//		int networkm_totalPlayerPotentialAbsorb2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerPotentialAbsorb = networkm_totalPlayerPotentialAbsorb2;
	//	}
	//	if ((num & 512L) != 0L)
	//	{
	//		int networkm_totalEnergyGained2 = reader.ReadPackedInt32();
	//		Networkm_totalEnergyGained = networkm_totalEnergyGained2;
	//	}
	//	if ((num & 1024L) != 0L)
	//	{
	//		int networkm_totalPlayerDamageReceived2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerDamageReceived = networkm_totalPlayerDamageReceived2;
	//	}
	//	if ((num & 2048L) != 0L)
	//	{
	//		int networkm_totalPlayerHealingReceived2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerHealingReceived = networkm_totalPlayerHealingReceived2;
	//	}
	//	if ((num & 4096L) != 0L)
	//	{
	//		int networkm_totalPlayerAbsorbReceived2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerAbsorbReceived = networkm_totalPlayerAbsorbReceived2;
	//	}
	//	if ((num & 8192L) != 0L)
	//	{
	//		float networkm_totalPlayerLockInTime2 = reader.ReadSingle();
	//		Networkm_totalPlayerLockInTime = networkm_totalPlayerLockInTime2;
	//	}
	//	if ((num & 16384L) != 0L)
	//	{
	//		int networkm_totalPlayerTurns2 = reader.ReadPackedInt32();
	//		Networkm_totalPlayerTurns = networkm_totalPlayerTurns2;
	//	}
	//	if ((num & 32768L) != 0L)
	//	{
	//		int networkm_damageDodgedByEvades2 = reader.ReadPackedInt32();
	//		Networkm_damageDodgedByEvades = networkm_damageDodgedByEvades2;
	//	}
	//	if ((num & 65536L) != 0L)
	//	{
	//		int networkm_damageInterceptedByEvades2 = reader.ReadPackedInt32();
	//		Networkm_damageInterceptedByEvades = networkm_damageInterceptedByEvades2;
	//	}
	//	if ((num & 131072L) != 0L)
	//	{
	//		int networkm_myIncomingDamageReducedByCover2 = reader.ReadPackedInt32();
	//		Networkm_myIncomingDamageReducedByCover = networkm_myIncomingDamageReducedByCover2;
	//	}
	//	if ((num & 262144L) != 0L)
	//	{
	//		int networkm_myOutgoingDamageReducedByCover2 = reader.ReadPackedInt32();
	//		Networkm_myOutgoingDamageReducedByCover = networkm_myOutgoingDamageReducedByCover2;
	//	}
	//	if ((num & 524288L) != 0L)
	//	{
	//		int networkm_myIncomingOverkillDamageTaken2 = reader.ReadPackedInt32();
	//		Networkm_myIncomingOverkillDamageTaken = networkm_myIncomingOverkillDamageTaken2;
	//	}
	//	if ((num & 1048576L) != 0L)
	//	{
	//		int networkm_myOutgoingOverkillDamageDealt2 = reader.ReadPackedInt32();
	//		Networkm_myOutgoingOverkillDamageDealt = networkm_myOutgoingOverkillDamageDealt2;
	//	}
	//	if ((num & 2097152L) != 0L)
	//	{
	//		int networkm_myOutgoingExtraDamageFromEmpowered2 = reader.ReadPackedInt32();
	//		Networkm_myOutgoingExtraDamageFromEmpowered = networkm_myOutgoingExtraDamageFromEmpowered2;
	//	}
	//	if ((num & 4194304L) != 0L)
	//	{
	//		int networkm_myOutgoingDamageReducedFromWeakened2 = reader.ReadPackedInt32();
	//		Networkm_myOutgoingDamageReducedFromWeakened = networkm_myOutgoingDamageReducedFromWeakened2;
	//	}
	//	if ((num & 8388608L) != 0L)
	//	{
	//		int networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe2 = reader.ReadPackedInt32();
	//		Networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe = networkm_teamOutgoingDamageIncreasedByEmpoweredFromMe2;
	//	}
	//	if ((num & 16777216L) != 0L)
	//	{
	//		int networkm_teamIncomingDamageReducedByWeakenedFromMe2 = reader.ReadPackedInt32();
	//		Networkm_teamIncomingDamageReducedByWeakenedFromMe = networkm_teamIncomingDamageReducedByWeakenedFromMe2;
	//	}
	//	if ((num & 33554432L) != 0L)
	//	{
	//		int networkm_teamExtraEnergyGainFromMe2 = reader.ReadPackedInt32();
	//		Networkm_teamExtraEnergyGainFromMe = networkm_teamExtraEnergyGainFromMe2;
	//	}
	//	if ((num & 67108864L) != 0L)
	//	{
	//		float networkm_movementDeniedByMe2 = reader.ReadSingle();
	//		Networkm_movementDeniedByMe = networkm_movementDeniedByMe2;
	//	}
	//	if ((num & 134217728L) != 0L)
	//	{
	//		int networkm_totalEnemySighted2 = reader.ReadPackedInt32();
	//		Networkm_totalEnemySighted = networkm_totalEnemySighted2;
	//	}
	//}

#if SERVER
	// added in rogues
	private class UnresolvedHitpointTrackingData
	{
		private Dictionary<ActorData, int> m_targetToAmount = new Dictionary<ActorData, int>();
		private int m_totalChangeOnMe;

		public int GetTotalChangeOnMe()
		{
			return m_totalChangeOnMe;
		}

		public void IncrementTotalOnMe(int amount)
		{
			if (amount > 0)
			{
				m_totalChangeOnMe += amount;
			}
		}

		public void IncrementChangeToTarget(ActorData targetActor, int healAmount)
		{
			if (healAmount > 0)
			{
				if (m_targetToAmount.ContainsKey(targetActor))
				{
					Dictionary<ActorData, int> targetToAmount = m_targetToAmount;
					targetToAmount[targetActor] += healAmount;
					return;
				}
				m_targetToAmount[targetActor] = healAmount;
			}
		}

		public int GetChangeToTarget(ActorData healTarget)
		{
			if (m_targetToAmount.ContainsKey(healTarget))
			{
				return m_targetToAmount[healTarget];
			}
			return 0;
		}

		public void Reset()
		{
			m_targetToAmount.Clear();
			m_totalChangeOnMe = 0;
		}
	}

	// added in rogues
	public class TurnBehavior
	{
		private BoardSquare m_square;
		private int m_startingHitPoints;
		private int m_startingTechPoints;
		private BoardSquare m_moveDestination;
		public int m_chaseTargetActorIndex;
		private bool m_knockedBack;
		private bool m_charged;
		private BoardSquarePathInfo m_path;
		private int m_damageTaken;
		private int m_healingTaken;
		public Dictionary<int, int> m_damageToActors;
		public Dictionary<int, int> m_healingToActors;
		public Dictionary<string, int> m_damageToDestroyedActors;
		public Dictionary<string, int> m_healingToDestroyedActors;
		public List<Ability> m_actionsTaken;
		private ActorBehavior m_behavior;
		private int m_index;
		public int m_turn;
		public List<ActorData> m_actorsResponsibleForEffects;

		public TurnBehavior(ActorBehavior owner, int index, int turn)
		{
			m_behavior = owner;
			m_index = index;
			m_turn = turn;
			m_damageToActors = new Dictionary<int, int>();
			m_healingToActors = new Dictionary<int, int>();
			m_damageToDestroyedActors = new Dictionary<string, int>();
			m_healingToDestroyedActors = new Dictionary<string, int>();
			m_actionsTaken = new List<Ability>();
			m_actorsResponsibleForEffects = new List<ActorData>();
		}

		public override string ToString()
		{
			string text = "";
			text += string.Format("Turn {0}, Index {1}\n", m_turn, m_index);
			if (Square != null)
			{
				text += string.Format("Square: ({0}, {1})\n", Square.x, Square.y);
			}
			text += string.Format("HitPoints: {0}\n", StartingHitPoints);
			if (MoveDestination != null)
			{
				text += string.Format("Destination: ({0}, {1})\n", MoveDestination.x, MoveDestination.y);
			}
			if (ChaseTargetActor != null)
			{
				text += string.Format("Chase Target: {0}\n", ChaseTargetActor.DisplayName);
			}
			if (KnockedBack && Charged)
			{
				text += "Knockbacked, Charged\n";
			}
			else if (KnockedBack && !Charged)
			{
				text += "Knockbacked\n";
			}
			else if (!KnockedBack && Charged)
			{
				text += "Charged\n";
			}
			if (DamageTaken != 0)
			{
				text += string.Format("Damage taken: {0}\n", DamageTaken);
			}
			if (HealingTaken != 0)
			{
				text += string.Format("Healing taken: {0}\n", HealingTaken);
			}
			if (m_damageToActors.Count != 0 || m_damageToDestroyedActors.Count != 0)
			{
				int num = 0;
				string text2 = "";
				foreach (KeyValuePair<int, int> keyValuePair in m_damageToActors)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(keyValuePair.Key);
					num += keyValuePair.Value;
					text2 += string.Format("\t{0} to {1}\n", keyValuePair.Value, actorData.DebugNameString());
				}
				foreach (KeyValuePair<string, int> keyValuePair2 in m_damageToDestroyedActors)
				{
					num += keyValuePair2.Value;
					text2 += string.Format("\t{0} to {1} (no longer exists)\n", keyValuePair2.Value, keyValuePair2.Key);
				}
				text += string.Format("Dealt {0} total dmg across {1} actor(s):\n{2}", num, m_damageToActors.Count, text2);
			}
			if (m_healingToActors.Count != 0 || m_healingToDestroyedActors.Count != 0)
			{
				int num2 = 0;
				string text3 = "";
				foreach (KeyValuePair<int, int> keyValuePair3 in m_healingToActors)
				{
					ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(keyValuePair3.Key);
					num2 += keyValuePair3.Value;
					text3 += string.Format("\t{0} to {1}\n", keyValuePair3.Value, actorData2.DebugNameString());
				}
				foreach (KeyValuePair<string, int> keyValuePair4 in m_healingToDestroyedActors)
				{
					num2 += keyValuePair4.Value;
					text3 += string.Format("\t{0} to {1} (no longer exists)\n", keyValuePair4.Value, keyValuePair4.Key);
				}
				text += string.Format("Dealt {0} healing across {1} actor(s):\n{2}", num2, m_healingToActors.Count, text3);
			}
			if (m_actionsTaken.Count > 0)
			{
				string text4 = "";
				for (int i = 0; i < m_actionsTaken.Count; i++)
				{
					Ability ability = m_actionsTaken[i];
					text4 += ability.m_abilityName;
					if (i == m_actionsTaken.Count - 1)
					{
						text4 += ".\n";
					}
					else if (i == m_actionsTaken.Count - 2)
					{
						text4 += ", and ";
					}
					else
					{
						text4 += ", ";
					}
				}
				text += string.Format("{0} actions taken: {1}\n", m_actionsTaken.Count, text4);
			}
			else
			{
				text += "No actions taken\n";
			}
			return text;
		}

		public BoardSquare Square
		{
			get
			{
				return m_square;
			}
			set
			{
				if (value != m_square)
				{
					m_square = value;
				}
			}
		}

		public int StartingHitPoints
		{
			get
			{
				return m_startingHitPoints;
			}
			set
			{
				if (value != m_startingHitPoints)
				{
					m_startingHitPoints = value;
				}
			}
		}

		public int StartingTechPoints
		{
			get
			{
				return m_startingTechPoints;
			}
			set
			{
				m_startingTechPoints = value;
			}
		}

		public BoardSquare MoveDestination
		{
			get
			{
				return m_moveDestination;
			}
			set
			{
				if (value != m_moveDestination)
				{
					m_moveDestination = value;
				}
			}
		}

		public ActorData ChaseTargetActor
		{
			get
			{
				if (m_chaseTargetActorIndex == ActorData.s_invalidActorIndex || GameFlowData.Get() == null)
				{
					return null;
				}
				return GameFlowData.Get().FindActorByActorIndex(m_chaseTargetActorIndex);
			}
			set
			{
				if (value == null && m_chaseTargetActorIndex != ActorData.s_invalidActorIndex)
				{
					m_chaseTargetActorIndex = ActorData.s_invalidActorIndex;
					return;
				}
				if (value != null && value.ActorIndex != m_chaseTargetActorIndex)
				{
					m_chaseTargetActorIndex = value.ActorIndex;
				}
			}
		}

		public bool KnockedBack
		{
			get
			{
				return m_knockedBack;
			}
			set
			{
				if (value != m_knockedBack)
				{
					m_knockedBack = value;
				}
			}
		}

		public bool Charged
		{
			get
			{
				return m_charged;
			}
			set
			{
				if (value != m_charged)
				{
					m_charged = value;
				}
			}
		}

		public BoardSquarePathInfo Path
		{
			get
			{
				return m_path;
			}
			set
			{
				if (value != m_path)
				{
					m_path = value;
				}
			}
		}

		public int DamageTaken
		{
			get
			{
				return m_damageTaken;
			}
			set
			{
				if (value != m_damageTaken)
				{
					m_damageTaken = value;
				}
			}
		}

		public int HealingTaken
		{
			get
			{
				return m_healingTaken;
			}
			set
			{
				if (value != m_healingTaken)
				{
					m_healingTaken = value;
				}
			}
		}

		public void RecordDamageToActor(ActorData target, int finalDamage, DamageSource src)
		{
			if (!m_damageToActors.ContainsKey(target.ActorIndex))
			{
				m_damageToActors.Add(target.ActorIndex, finalDamage);
				return;
			}
			Dictionary<int, int> damageToActors = m_damageToActors;
			int actorIndex = target.ActorIndex;
			damageToActors[actorIndex] += finalDamage;
		}

		public string GetDamageToActorDebugString()
		{
			string text = "";
			foreach (int num in m_damageToActors.Keys)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(num);
				text = string.Concat(new object[]
				{
					text,
					m_damageToActors[num],
					" damage to ",
					actorData.DebugNameString()
				});
			}
			return text;
		}

		public void RecordHealingToActor(ActorData target, int finalHealing, DamageSource src)
		{
			if (!m_healingToActors.ContainsKey(target.ActorIndex))
			{
				m_healingToActors.Add(target.ActorIndex, finalHealing);
				return;
			}
			Dictionary<int, int> healingToActors = m_healingToActors;
			int actorIndex = target.ActorIndex;
			healingToActors[actorIndex] += finalHealing;
		}

		public string GetHealingToActorDebugString()
		{
			string text = "";
			foreach (int num in m_healingToActors.Keys)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(num);
				text = string.Concat(new object[]
				{
					text,
					m_healingToActors[num],
					" healing to ",
					actorData.DebugNameString()
				});
			}
			return text;
		}

		public void RecordActionTaken(Ability actionTaken)
		{
			m_actionsTaken.Add(actionTaken);
		}

		public void RecordEffectFromActor(ActorData caster)
		{
			if (!m_actorsResponsibleForEffects.Contains(caster))
			{
				m_actorsResponsibleForEffects.Add(caster);
			}
		}

		public bool MovedCloserTo(BoardSquare considerationSquare, BoardSquare currentSquare)
		{
			bool result = false;
			if (considerationSquare != null && currentSquare != null && Square != null)
			{
				float num = currentSquare.HorizontalDistanceInSquaresTo(considerationSquare);
				float num2 = Square.HorizontalDistanceInSquaresTo(considerationSquare);
				result = (num < num2);
			}
			return result;
		}

		public bool MovedFartherFrom(BoardSquare considerationSquare, BoardSquare currentSquare)
		{
			bool result = false;
			if (considerationSquare != null && currentSquare != null && Square != null)
			{
				float num = currentSquare.HorizontalDistanceInSquaresTo(considerationSquare);
				float num2 = Square.HorizontalDistanceInSquaresTo(considerationSquare);
				result = (num > num2);
			}
			return result;
		}

		public bool Moved(BoardSquare currentSquare)
		{
			return Square != currentSquare;
		}

		public bool MovedIntenionally(BoardSquare currentSquare)
		{
			return Moved(currentSquare) && MoveDestination != null && !KnockedBack;
		}

		public bool MovedOrCharged(BoardSquare currentSquare)
		{
			return Moved(currentSquare) || Charged;
		}

		public bool MovedIntentionallyOrCharged(BoardSquare currentSquare)
		{
			return MovedIntenionally(currentSquare) || Charged;
		}

		public bool Damaged()
		{
			return DamageTaken > 0;
		}

		public bool DamagedByActor(ActorData damagerActor)
		{
			bool result = false;
			ActorData actor = m_behavior.m_actor;
			ActorBehavior actorBehavior = damagerActor.GetActorBehavior();
			if (actor != null && actorBehavior != null)
			{
				TurnBehavior behaviorOfTurn = actorBehavior.GetBehaviorOfTurn(m_turn);
				if (behaviorOfTurn != null)
				{
					result = behaviorOfTurn.DamagedSpecificActor(actor);
				}
			}
			return result;
		}

		public bool DamagedSomeone()
		{
			bool result = false;
			using (Dictionary<int, int>.ValueCollection.Enumerator enumerator = m_damageToActors.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current > 0)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool DamagedPlayerCharacter()
		{
			bool result = false;
			foreach (KeyValuePair<int, int> keyValuePair in m_damageToActors)
			{
				ActorData actor = GameFlowData.Get().FindActorByActorIndex(keyValuePair.Key);
				if (keyValuePair.Value > 0 && GameplayUtils.IsPlayerControlled(actor))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool DamagedSpecificActor(ActorData damagedActor)
		{
			bool result = false;
			if (m_damageToActors.ContainsKey(damagedActor.ActorIndex))
			{
				result = (m_damageToActors[damagedActor.ActorIndex] > 0);
			}
			return result;
		}

		public bool Healed()
		{
			return HealingTaken > 0;
		}

		public bool HealedByActor(ActorData healerActor)
		{
			bool result = false;
			ActorData actor = m_behavior.m_actor;
			ActorBehavior actorBehavior = healerActor.GetActorBehavior();
			if (actor != null && actorBehavior != null)
			{
				TurnBehavior behaviorOfTurn = actorBehavior.GetBehaviorOfTurn(m_turn);
				if (behaviorOfTurn != null)
				{
					result = behaviorOfTurn.HealedSpecificActor(actor);
				}
			}
			return result;
		}

		public bool HealedSomeone()
		{
			bool result = false;
			using (Dictionary<int, int>.ValueCollection.Enumerator enumerator = m_healingToActors.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current > 0)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool HealedSpecificActor(ActorData healedActor)
		{
			bool result = false;
			if (m_healingToActors.ContainsKey(healedActor.ActorIndex))
			{
				result = (m_healingToActors[healedActor.ActorIndex] > 0);
			}
			return result;
		}

		public bool UsedAbility(AbilityData.ActionType action)
		{
			bool result = false;
			AbilityData component = m_behavior.GetComponent<AbilityData>();
			if (action != AbilityData.ActionType.INVALID_ACTION)
			{
				Ability abilityOfActionType = component.GetAbilityOfActionType(action);
				result = m_actionsTaken.Contains(abilityOfActionType);
			}
			return result;
		}

		public bool UsedAbility(Ability ability)
		{
			bool result = false;
			if (ability != null)
			{
				result = m_actionsTaken.Contains(ability);
			}
			return result;
		}

		public bool UsedAbility()
		{
			return m_actionsTaken.Count > 0;
		}

		public bool BuffedByActor(ActorData caster)
		{
			bool result = false;
			if (m_behavior.m_actor.GetTeam() == caster.GetTeam())
			{
				result = m_actorsResponsibleForEffects.Contains(caster);
			}
			return result;
		}

		public bool DebuffedByActor(ActorData caster)
		{
			bool result = false;
			if (m_behavior.m_actor.GetTeam() != caster.GetTeam())
			{
				result = m_actorsResponsibleForEffects.Contains(caster);
			}
			return result;
		}

		public bool BuffedSpecificActor(ActorData target)
		{
			bool result = false;
			ActorData actor = m_behavior.m_actor;
			ActorBehavior actorBehavior = target.GetActorBehavior();
			if (actor != null && actorBehavior != null)
			{
				TurnBehavior behaviorOfTurn = actorBehavior.GetBehaviorOfTurn(m_turn);
				if (behaviorOfTurn != null)
				{
					result = behaviorOfTurn.BuffedByActor(actor);
				}
			}
			return result;
		}

		public bool DebuffedSpecificActor(ActorData target)
		{
			bool result = false;
			ActorData actor = m_behavior.m_actor;
			ActorBehavior actorBehavior = target.GetActorBehavior();
			if (actor != null && actorBehavior != null)
			{
				TurnBehavior behaviorOfTurn = actorBehavior.GetBehaviorOfTurn(m_turn);
				if (behaviorOfTurn != null)
				{
					result = behaviorOfTurn.DebuffedByActor(actor);
				}
			}
			return result;
		}
	}
#endif
}
