// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
public class ActorHitResults
{
	public ActorHitParameters m_hitParameters;
	public bool m_serverGuaranteeHit;

	//public HitChanceBracket.HitType m_hitType = HitChanceBracket.HitType.Normal;  // rogues

	private int m_baseDamage;
	private int m_finalDamage;
	private int m_baseHealing;
	private int m_finalHealing;

	private MovementStage m_forMovemenetStage;

	private bool m_ignoreTechpointInteractionsForHit;
	private int m_baseTechPointsGain;
	private int m_finalTechPointsGain;
	private int m_baseTechPointsLoss;
	private int m_finalTechPointsLoss;
	private int m_baseTechPointGainOnCaster;
	private int m_baseDirectTechPointGainOnCaster;
	private int m_finalTechPointGainOnCaster;
	private int m_baseObjectivePointsChange;
	private int m_finalObjectivePointsChange;
	private int m_bounceCount;

	private ServerCombatManager.DamageType m_damageType;
	private ServerCombatManager.HealingType m_healingType;

	private List<StatusType> m_permanentStatusChanges = new List<StatusType>();
	private List<StatusType> m_permanentStatusChangesForRemoval = new List<StatusType>();
	private List<AbilityStatMod> m_permanentStatMods = new List<AbilityStatMod>();
	private List<AbilityStatMod> m_permanentStatModsForRemoval = new List<AbilityStatMod>();

	//public List<GearStatTerm> m_gearStatTerms;  // rogues

	public List<global::Effect> m_effects;

	//public List<EffectTrigger> m_effectTriggers;  // rogues?

	private List<StandardActorEffectData> m_standardEffectDatas;

	//private List<EffectTemplate> m_effectTemplates;  // rogues?

	//private List<EffectTriggerTemplate> m_effectTriggerTemplates;  // rogues?

	private List<ServerAbilityUtils.EffectRemovalData> m_effectsForRemoval;
	private List<ServerAbilityUtils.EffectRefreshData> m_effectsForRefresh;
	private List<ServerAbilityUtils.BarrierRemovalData> m_barriersForRemoval;
	private List<Barrier> m_barriers;
	public List<ServerAbilityUtils.PowerupRemovalData> m_powerupsForRemoval;
	public List<ServerAbilityUtils.PowerUpStealData> m_powerUpsToSteal;
	private List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;
	private KnockbackHitData m_knockbackData;
	private List<MiscHitEventData> m_miscEvents;
	private List<GameModeEvent> m_gameModeEvents;
	private List<ActorData> m_actorsToReveal;
	private List<SpoilSpawnDataForAbilityHit> m_spoilSpawns;
	private List<MovementResults> m_directSpoilHitResults;
	private List<AbilityResults_Reaction> m_reactions;
	private List<HitResultsTags> m_tags;
	private List<int> m_overconIds;

	// rogues
	//private Dictionary<BoardSquare, int> m_dynamicGeoForDamage;

	private int m_thornsDamage;
	private int m_lifestealHealing;
	private bool m_targetInCoverWrtDamage;
	private bool m_damageBoosted;
	private bool m_damageReduced;
	private bool m_isPartOfHealOverTime;
	private bool m_overrideAsInCover;
	private bool m_coverIgnoreMinDist;
	public int m_reactionDepth;
	private DamageCalcScratch m_damageCalcScratch = new DamageCalcScratch();
	private ServerGameplayUtils.DamageStatAdjustments m_damageStatAdjustments;
	private ServerGameplayUtils.EnergyStatAdjustments m_energyStatAdjustOnTarget;
	private ServerGameplayUtils.EnergyStatAdjustments m_energyStatAdjustOnCaster;
	private HitActorDelegate m_hitDelegate;
	private bool m_executed;
	private bool m_canBeReactedTo = true;
	private bool m_isReaction;
	private ActorHitResults m_triggeringHit;

	public ActorHitResults(ActorHitParameters hitParams)
	{
		m_hitParameters = hitParams;
	}

	public ActorHitResults(int amount, HitActionType deltaType, ActorHitParameters hitParams)
	{
		SetHitActionAmount(amount, deltaType);
		m_hitParameters = hitParams;
	}

	public ActorHitResults(int amount, HitActionType deltaType, global::Effect effect, ActorHitParameters hitParams)
	{
		SetHitActionAmount(amount, deltaType);
		m_effects = new List<global::Effect>(1);
		m_effects.Add(effect);
		m_hitParameters = hitParams;
	}

	public ActorHitResults(int amount, HitActionType deltaType, global::Effect effect, HitActorDelegate hitDelegate, ActorHitParameters hitParams)
	{
		SetHitActionAmount(amount, deltaType);
		m_effects = new List<global::Effect>(1);
		m_effects.Add(effect);
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public ActorHitResults(global::Effect effect, ActorHitParameters hitParams)
	{
		m_effects = new List<global::Effect>(1);
		m_effects.Add(effect);
		m_hitParameters = hitParams;
	}

	public ActorHitResults(global::Effect effect, HitActorDelegate hitDelegate, ActorHitParameters hitParams)
	{
		m_effects = new List<global::Effect>(1);
		m_effects.Add(effect);
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public ActorHitResults(int amount, HitActionType deltaType, StandardEffectInfo effectInfo, ActorHitParameters hitParams)
	{
		SetHitActionAmount(amount, deltaType);
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			m_standardEffectDatas = new List<StandardActorEffectData>(1);
			m_standardEffectDatas.Add(effectInfo.m_effectData);
		}
		m_hitParameters = hitParams;
	}

	public ActorHitResults(int amount, HitActionType deltaType, StandardEffectInfo effectInfo, HitActorDelegate hitDelegate, ActorHitParameters hitParams)
	{
		SetHitActionAmount(amount, deltaType);
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			m_standardEffectDatas = new List<StandardActorEffectData>(1);
			m_standardEffectDatas.Add(effectInfo.m_effectData);
		}
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public ActorHitResults(StandardEffectInfo effectInfo, ActorHitParameters hitParams)
	{
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			m_standardEffectDatas = new List<StandardActorEffectData>(1);
			m_standardEffectDatas.Add(effectInfo.m_effectData);
		}
		m_hitParameters = hitParams;
	}

	public ActorHitResults(StandardEffectInfo effectInfo, HitActorDelegate hitDelegate, ActorHitParameters hitParams)
	{
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			m_standardEffectDatas = new List<StandardActorEffectData>(1);
			m_standardEffectDatas.Add(effectInfo.m_effectData);
		}
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public ActorHitResults(HitActorDelegate hitDelegate, ActorHitParameters hitParams)
	{
		m_hitDelegate = hitDelegate;
		m_hitParameters = hitParams;
	}

	public void AddEffect(global::Effect effect)
	{
		if (m_effects == null)
		{
			m_effects = new List<global::Effect>(1);
		}
		m_effects.Add(effect);
	}

	public void AddEffectForRemoval(global::Effect effect, List<global::Effect> listToRemoveFrom)
	{
		if (m_effectsForRemoval == null)
		{
			m_effectsForRemoval = new List<ServerAbilityUtils.EffectRemovalData>(1);
		}
		m_effectsForRemoval.Add(new ServerAbilityUtils.EffectRemovalData(effect, listToRemoveFrom));
	}

	public void AddEffectForRemoval(global::Effect effect)
	{
		if (m_effectsForRemoval == null)
		{
			m_effectsForRemoval = new List<ServerAbilityUtils.EffectRemovalData>(1);
		}
		List<global::Effect> effectListToRemoveFrom;
		if (effect.Target == null)
		{
			effectListToRemoveFrom = ServerEffectManager.Get().GetWorldEffects();
		}
		else
		{
			effectListToRemoveFrom = ServerEffectManager.Get().GetActorEffects(effect.Target);
		}
		m_effectsForRemoval.Add(new ServerAbilityUtils.EffectRemovalData(effect, effectListToRemoveFrom));
	}

	public void AddEffectForRefresh(global::Effect effect, List<global::Effect> effectListOfEffect)
	{
		if (effect != null)
		{
			if (m_effectsForRefresh == null)
			{
				m_effectsForRefresh = new List<ServerAbilityUtils.EffectRefreshData>(1);
			}
			m_effectsForRefresh.Add(new ServerAbilityUtils.EffectRefreshData(effect, effectListOfEffect));
			return;
		}
		Debug.LogError("Trying to add null effect for refresh");
	}

	// rogues?
	//public void AddEffectTemplate(EffectTemplate effectTemplate)
	//{
	//	if (effectTemplate != null)
	//	{
	//		if (m_effectTemplates == null)
	//		{
	//			m_effectTemplates = new List<EffectTemplate>(1);
	//		}
	//		m_effectTemplates.Add(effectTemplate);
	//	}
	//}

	// rogues?
	//public void AddEffectTriggerTemplate(EffectTriggerTemplate effectTriggerTemplate)
	//{
	//	if (effectTriggerTemplate != null)
	//	{
	//		if (m_effectTriggerTemplates == null)
	//		{
	//			m_effectTriggerTemplates = new List<EffectTriggerTemplate>(1);
	//		}
	//		m_effectTriggerTemplates.Add(effectTriggerTemplate);
	//	}
	//}

	public void AddStandardEffectInfo(StandardEffectInfo effectInfo)
	{
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			if (m_standardEffectDatas == null)
			{
				m_standardEffectDatas = new List<StandardActorEffectData>(1);
			}
			m_standardEffectDatas.Add(effectInfo.m_effectData);
		}
	}

	public void AddBarrier(Barrier barrier)
	{
		if (m_barriers == null)
		{
			m_barriers = new List<Barrier>(1);
		}
		m_barriers.Add(barrier);
	}

	public void AddBarrierForRemoval(Barrier barrier, bool removeLinkedBarriers)
	{
		if (m_barriersForRemoval == null)
		{
			m_barriersForRemoval = new List<ServerAbilityUtils.BarrierRemovalData>(1);
		}
		m_barriersForRemoval.Add(new ServerAbilityUtils.BarrierRemovalData(barrier, removeLinkedBarriers));
	}

	public void AddPowerupForRemoval(PowerUp powerUp)
	{
		if (m_powerupsForRemoval == null)
		{
			m_powerupsForRemoval = new List<ServerAbilityUtils.PowerupRemovalData>(1);
		}
		m_powerupsForRemoval.Add(new ServerAbilityUtils.PowerupRemovalData(powerUp));
	}

	public void AddKnockbackData(KnockbackHitData knockbackData)
	{
		m_knockbackData = knockbackData;
		if (m_knockbackData != null && m_hitParameters.Target.GetActorStatus().IsKnockbackImmune(true))
		{
			m_knockbackData.m_distance = 0f;
			m_knockbackData.m_type = KnockbackType.AwayFromSource;
		}
	}

	public void AddMiscHitEvent(MiscHitEventData hitEvent)
	{
		if (hitEvent != null)
		{
			if (m_miscEvents == null)
			{
				m_miscEvents = new List<MiscHitEventData>(1);
			}
			m_miscEvents.Add(hitEvent);
		}
	}

	public void AddGameModeEvent(GameModeEvent gameModeEvent)
	{
		if (gameModeEvent != null)
		{
			if (m_gameModeEvents == null)
			{
				m_gameModeEvents = new List<GameModeEvent>(1);
			}
			m_gameModeEvents.Add(gameModeEvent);
		}
	}

	public void AddActorToReveal(ActorData actor)
	{
		if (actor != null)
		{
			if (m_actorsToReveal == null)
			{
				m_actorsToReveal = new List<ActorData>();
			}
			if (!m_actorsToReveal.Contains(actor))
			{
				m_actorsToReveal.Add(actor);
			}
		}
	}

	public void AddSpoilSpawnData(SpoilSpawnDataForAbilityHit spoilSpawnData)
	{
		if (m_spoilSpawns == null)
		{
			m_spoilSpawns = new List<SpoilSpawnDataForAbilityHit>();
		}
		if (spoilSpawnData.CanSpawnSpoils())
		{
			m_spoilSpawns.Add(spoilSpawnData);
		}
	}

	public void AddPowerUpForSteal(PowerUp powerup)
	{
		if (m_powerUpsToSteal == null)
		{
			m_powerUpsToSteal = new List<ServerAbilityUtils.PowerUpStealData>(1);
		}
		m_powerUpsToSteal.Add(new ServerAbilityUtils.PowerUpStealData(powerup, m_hitParameters.Target));
	}

	// rogues
	//public void AddDynamicMissionGeometryDamage(BoardSquare geoSquare, int damage)
	//{
	//	if (m_dynamicGeoForDamage == null)
	//	{
	//		m_dynamicGeoForDamage = new Dictionary<BoardSquare, int>();
	//	}
	//	if (m_dynamicGeoForDamage.ContainsKey(geoSquare))
	//	{
	//		damage += m_dynamicGeoForDamage[geoSquare];
	//	}
	//	m_dynamicGeoForDamage[geoSquare] = damage;
	//}

	public void AddEffectSequenceToEnd(GameObject sequencePrefab, int effectGuid)
	{
		AddEffectSequenceToEnd(sequencePrefab, effectGuid, Vector3.zero);
	}

	public void AddEffectSequenceToEnd(GameObject sequencePrefab, int effectGuid, Vector3 targetPos)
	{
		if (m_sequencesToEnd == null)
		{
			m_sequencesToEnd = new List<ServerClientUtils.SequenceEndData>();
		}
		ServerClientUtils.SequenceEndData item = new ServerClientUtils.SequenceEndData(sequencePrefab, ServerClientUtils.SequenceEndData.AssociationType.EffectGuid, effectGuid, targetPos);
		m_sequencesToEnd.Add(item);
	}

	public void AddBarrierSequenceToEnd(GameObject sequencePrefab, int barrierGuid)
	{
		AddBarrierSequenceToEnd(sequencePrefab, barrierGuid, Vector3.zero);
	}

	public void AddBarrierSequenceToEnd(GameObject sequencePrefab, int barrierGuid, Vector3 targetPos)
	{
		if (m_sequencesToEnd == null)
		{
			m_sequencesToEnd = new List<ServerClientUtils.SequenceEndData>();
		}
		ServerClientUtils.SequenceEndData item = new ServerClientUtils.SequenceEndData(sequencePrefab, ServerClientUtils.SequenceEndData.AssociationType.BarrierGuid, barrierGuid, targetPos);
		m_sequencesToEnd.Add(item);
	}

	public void AddHitResultsTag(HitResultsTags tag)
	{
		if (m_tags == null)
		{
			m_tags = new List<HitResultsTags>();
		}
		if (!m_tags.Contains(tag))
		{
			m_tags.Add(tag);
		}
	}

	public void AddOvercon(int overconId)
	{
		if (m_overconIds == null)
		{
			m_overconIds = new List<int>();
		}
		if (!m_overconIds.Contains(overconId))
		{
			m_overconIds.Add(overconId);
		}
	}

	// rogues
	//public void ModifyDamageCoeff(float min, float max)
	//{
	//	DamageCoeffMin += min;
	//	DamageCoeffMax += max;
	//}

	// rogues
	//public void ModifyHealingCoeff(float min, float max)
	//{
	//	HealingCoeffMin += min;
	//	HealingCoeffMax += max;
	//}

	// rogues
	//public void ModifyTechPointGainCoeff(float min, float max)
	//{
	//	EnergyCoeffMin += min;
	//	EnergyCoeffMax += max;
	//}

	// rogues
	//public void InitBaseValuesByCoeff(ActorData target, ActorData caster)
	//{
	//	EquipmentStats equipmentStats = caster.GetEquipmentStats();
	//	Ability relevantAbility = m_hitParameters.GetRelevantAbility();
	//	AbilityData.ActionType abilityIndex = (relevantAbility != null) ? relevantAbility.CachedActionType : AbilityData.ActionType.INVALID_ACTION;
	//	int num = Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.PowerAdjustment, (float)caster.GetBaseStatValue(GearStatType.PowerAdjustment), (int)abilityIndex, target));
	//	if (target != null && target.GetTeam() == caster.GetTeam())
	//	{
	//		num += Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.ExpertiseAdjustment, (float)caster.GetBaseStatValue(GearStatType.ExpertiseAdjustment), (int)abilityIndex, target));
	//	}
	//	else if (target != null && target.GetTeam() != caster.GetTeam())
	//	{
	//		num += Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.StrengthAdjustment, (float)caster.GetBaseStatValue(GearStatType.StrengthAdjustment), (int)abilityIndex, target));
	//	}
	//	if (DamageCoeffMin > 0f)
	//	{
	//		float num2 = UnityEngine.Random.Range(DamageCoeffMin, DamageCoeffMax);
	//		SetBaseDamage(Mathf.RoundToInt(num2 * (float)num));
	//	}
	//	if (HealingCoeffMin > 0f)
	//	{
	//		float num3 = UnityEngine.Random.Range(HealingCoeffMin, HealingCoeffMax);
	//		SetBaseHealing(Mathf.RoundToInt(num3 * (float)num));
	//	}
	//	if (EnergyCoeffMin > 0f)
	//	{
	//		float num4 = UnityEngine.Random.Range(EnergyCoeffMin, EnergyCoeffMax);
	//		SetTechPointGain(Mathf.RoundToInt(num4 * (float)num));
	//	}
	//}

	public void SetBaseDamage(int amount)
	{
		m_baseDamage = amount;
	}

	public void SetBaseHealing(int amount)
	{
		m_baseHealing = amount;
	}

	public void SetTechPointGain(int amount)
	{
		m_baseTechPointsGain = amount;
	}

	public void SetTechPointLoss(int amount)
	{
		m_baseTechPointsLoss = amount;
	}

	public void SetTechPointGainOnCaster(int amount)
	{
		m_baseTechPointGainOnCaster = amount;
	}

	public void SetObjectivePointChange(int amount)
	{
		m_baseObjectivePointsChange = amount;
	}

	public void SetIgnoreTechpointInteractionForHit(bool value)
	{
		m_ignoreTechpointInteractionsForHit = value;
	}

	public void AddBaseDamage(int addAmount)
	{
		m_baseDamage += addAmount;
	}

	public void AddBaseHealing(int addAmount)
	{
		m_baseHealing += addAmount;
	}

	public void AddTechPointGain(int addAmount)
	{
		m_baseTechPointsGain += addAmount;
	}

	public void AddTechPointLoss(int addAmount)
	{
		m_baseTechPointsLoss += addAmount;
	}

	public void AddTechPointGainOnCaster(int addAmount)
	{
		m_baseTechPointGainOnCaster += addAmount;
	}

	public void AddDirectTechPointGainOnCaster(int addAmount)
	{
		m_baseDirectTechPointGainOnCaster += addAmount;
	}

	public void AddObjectivePointChange(int amount)
	{
		m_baseObjectivePointsChange += amount;
	}

	public void SetIsPartOfHealOverTime(bool isHoT)
	{
		m_isPartOfHealOverTime = isHoT;
	}

	public void SetBounceCount(int amount)
	{
		m_bounceCount = amount;
	}

	public void AddBounceCount(int addAmount)
	{
		m_bounceCount += addAmount;
	}

	public int GetBounceCount()
	{
		return m_bounceCount;
	}

	public void AddPermanentStatMods(AbilityStatMod[] statMods)
	{
		foreach (AbilityStatMod item in statMods)
		{
			m_permanentStatMods.Add(item);
		}
	}

	public void AddPermanentStatusChanges(StatusType[] statusChanges)
	{
		foreach (StatusType item in statusChanges)
		{
			m_permanentStatusChanges.Add(item);
		}
	}

	public void AddPermanentStatModsForRemoval(AbilityStatMod[] statMods)
	{
		foreach (AbilityStatMod item in statMods)
		{
			m_permanentStatModsForRemoval.Add(item);
		}
	}

	public void AddPermanentStatusChangesForRemoval(StatusType[] statusChanges)
	{
		foreach (StatusType item in statusChanges)
		{
			m_permanentStatusChangesForRemoval.Add(item);
		}
	}

	public void SetHitActionAmount(int amount, HitActionType deltaType)
	{
		switch (deltaType)
		{
		case HitActionType.Damage:
			m_baseDamage = amount;
			return;
		case HitActionType.Healing:
			m_baseHealing = amount;
			return;
		case HitActionType.TechPointsGain:
			m_baseTechPointsGain = amount;
			return;
		case HitActionType.TechPointsLoss:
			m_baseTechPointsLoss = amount;
			return;
		case HitActionType.ObjectivePointChange:
			m_baseObjectivePointsChange = amount;
			return;
		default:
			return;
		}
	}

	// rogues
	//public void AddGearStatTerm(GearStatTerm gearStatTerm)
	//{
	//	if (this.m_gearStatTerms == null)
	//	{
	//		this.m_gearStatTerms = new List<GearStatTerm>();
	//	}
	//	this.m_gearStatTerms.Add(gearStatTerm);
	//}

	// rogues
	//public float DamageCoeffMin { get; private set; }

	// rogues
	//public float DamageCoeffMax { get; private set; }

	// rogues
	//public float HealingCoeffMin { get; private set; }

	// rogues
	//public float HealingCoeffMax { get; private set; }

	// rogues
	//public float EnergyCoeffMin { get; private set; }

	// rogues
	//public float EnergyCoeffMax { get; private set; }

	public int BaseDamage
	{
		get
		{
			return m_baseDamage;
		}
	}

	public int FinalDamage
	{
		get
		{
			return m_finalDamage;
		}
	}

	public int BaseHealing
	{
		get
		{
			return m_baseHealing;
		}
	}

	public int FinalHealing
	{
		get
		{
			return m_finalHealing;
		}
	}

	public MovementStage ForMovementStage
	{
		get
		{
			return m_forMovemenetStage;
		}
		set
		{
			m_forMovemenetStage = value;
		}
	}

	public bool IsFromMovement()
	{
		return ForMovementStage > MovementStage.INVALID;
	}

	public int BaseTechPointsGain
	{
		get
		{
			return m_baseTechPointsGain;
		}
	}

	public int FinalTechPointsGain
	{
		get
		{
			return m_finalTechPointsGain;
		}
	}

	public int BaseTechPointsLoss
	{
		get
		{
			return m_baseTechPointsLoss;
		}
	}

	public int FinalTechPointsLoss
	{
		get
		{
			return m_finalTechPointsLoss;
		}
	}

	public int BaseTechPointsCasterGain
	{
		get
		{
			return m_baseTechPointGainOnCaster;
		}
	}

	public int FinalTechPointsCasterGain
	{
		get
		{
			return m_finalTechPointGainOnCaster;
		}
	}

	public ServerCombatManager.DamageType DamageType
	{
		get
		{
			return m_damageType;
		}
	}

	public ServerCombatManager.HealingType HealingType
	{
		get
		{
			return m_healingType;
		}
	}

	public int ThornsDamage
	{
		get
		{
			return m_thornsDamage;
		}
	}

	public int LifestealHealingOnCaster
	{
		get
		{
			return m_lifestealHealing;
		}
	}

	public bool TargetInCoverWrtDamage
	{
		get
		{
			return m_targetInCoverWrtDamage;
		}
	}

	public void OverrideAsInCover()
	{
		m_overrideAsInCover = true;
	}

	public void SetIgnoreCoverMinDist(bool value)
	{
		m_coverIgnoreMinDist = value;
	}

	public DamageCalcScratch GetDamageCalcScratch()
	{
		return m_damageCalcScratch;
	}

	public bool ResultedInTargetDeath { get; private set; }

	public bool ResultedInCasterDeath { get; private set; }

	public void ExecuteResults()
	{
		bool flag = m_hitParameters.Target != null && m_hitParameters.Target.IsDead();
		bool flag2 = m_hitParameters.Caster != null && m_hitParameters.Caster.IsDead();
		if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning(AbilityResults.s_executeActorHitHeader + ToDebugString());
		}
		// rogues?
		//if (m_hitParameters.Target.HasBotController)
		//{
		//	if (!m_hitParameters.Target.Alerted && m_hitParameters.Caster.GetTeam() == Team.TeamA && !GameFlowData.Get().IsInCombat)
		//	{
		//		ServerActionBuffer.Get().RecordAbilityAlertInitiator(m_hitParameters.Caster);
		//	}
		//	m_hitParameters.Target.Alerted = true;
		//	BotManager.Get().CheckForAlertForTeam(m_hitParameters.Target.GetTeam());
		//}
		if (m_baseDamage != 0)
		{
			ServerCombatManager.Get().ExecuteDamage(this, m_damageType);
			if (m_damageStatAdjustments != null)
			{
				m_damageStatAdjustments.ApplyDamageAdjustmentStats();
			}
			ResultedInTargetDeath = (ResultedInTargetDeath ? ResultedInTargetDeath : (!flag && m_hitParameters.Target.IsDead()));
		}
		if (m_lifestealHealing > 0)
		{
			ServerCombatManager.Get().ExecuteHealing(this, ServerCombatManager.HealingType.Lifesteal);
			bool flag3 = m_hitParameters.Caster != null && m_hitParameters.Caster.IsDead();
			ResultedInCasterDeath = (ResultedInCasterDeath ? ResultedInCasterDeath : (!flag2 && flag3));
		}
		if (m_thornsDamage != 0)
		{
			ActorData target = m_hitParameters.Target;
			ActorData caster = m_hitParameters.Caster;
			ServerCombatManager.Get().ExecuteDamage(this, ServerCombatManager.DamageType.Thorns);
			bool flag4 = m_hitParameters.Caster != null && m_hitParameters.Caster.IsDead();
			ResultedInCasterDeath = (ResultedInCasterDeath ? ResultedInCasterDeath : (!flag2 && flag4));
		}
		if (m_baseHealing != 0)
		{
			ServerCombatManager.Get().ExecuteHealing(this, m_healingType);
		}
		if (m_baseTechPointsGain != 0 || m_finalTechPointsGain > 0)
		{
			ServerCombatManager.Get().ExecuteTechPointGain(m_hitParameters.Caster, m_hitParameters.Target, m_finalTechPointsGain, m_hitParameters.DamageSource);
			if (m_hitParameters.Caster.GetTeam() == m_hitParameters.Target.GetTeam() && m_hitParameters.Caster.GetActorBehavior() != null)
			{
				int toAdd = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(m_hitParameters.Target, StatusType.Energized).Contains(m_hitParameters.Caster) ? m_baseTechPointsGain : m_finalTechPointsGain;
				m_hitParameters.Caster.GetActorBehavior().OnCalculatedExtraEnergyByMe(toAdd);
			}
		}
		if (m_baseTechPointsLoss != 0)
		{
			ServerCombatManager.Get().ExecuteTechPointLoss(m_hitParameters.Caster, m_hitParameters.Target, m_finalTechPointsLoss, m_hitParameters.DamageSource);
		}
		if (m_baseTechPointGainOnCaster != 0 || m_finalTechPointGainOnCaster != 0)
		{
			ServerCombatManager.Get().ExecuteTechPointGain(m_hitParameters.Caster, m_hitParameters.Caster, m_finalTechPointGainOnCaster, m_hitParameters.DamageSource);
		}
		if (m_energyStatAdjustOnTarget != null)
		{
			m_energyStatAdjustOnTarget.ApplyStatAdjustments();
		}
		if (m_energyStatAdjustOnCaster != null)
		{
			m_energyStatAdjustOnCaster.ApplyStatAdjustments();
		}
		if (m_hitParameters.Caster != m_hitParameters.Target)
		{
			GameplayMetricHelper.IncrementTargetHitCount(m_hitParameters.Caster, m_hitParameters.DamageSource.Ability);
		}
		if (m_baseObjectivePointsChange != 0)
		{
			ServerCombatManager.Get().ExecuteObjectivePointGain(m_hitParameters.Caster, m_hitParameters.Caster, m_finalObjectivePointsChange);
		}
		if (m_permanentStatMods.Count > 0)
		{
			ActorStats actorStats = m_hitParameters.Target.GetActorStats();
			foreach (AbilityStatMod statMod in m_permanentStatMods)
			{
				actorStats.AddStatMod(statMod);
			}
		}
		if (m_permanentStatModsForRemoval.Count > 0)
		{
			ActorStats actorStats2 = m_hitParameters.Target.GetActorStats();
			foreach (AbilityStatMod statMod2 in m_permanentStatModsForRemoval)
			{
				actorStats2.RemoveStatMod(statMod2);
			}
		}
		if (m_permanentStatusChanges.Count > 0)
		{
			ActorStatus actorStatus = m_hitParameters.Target.GetActorStatus();
			foreach (StatusType status in m_permanentStatusChanges)
			{
				actorStatus.AddStatus(status, 0);
			}
		}
		if (m_permanentStatusChangesForRemoval.Count > 0)
		{
			ActorStatus actorStatus2 = m_hitParameters.Target.GetActorStatus();
			foreach (StatusType status2 in m_permanentStatusChangesForRemoval)
			{
				actorStatus2.RemoveStatus(status2);
			}
		}
		if (m_effectsForRemoval != null)
		{
			foreach (ServerAbilityUtils.EffectRemovalData effectRemovalData in m_effectsForRemoval)
			{
				global::Effect effectToRemove = effectRemovalData.m_effectToRemove;
				List<global::Effect> effectListToRemoveFrom = effectRemovalData.m_effectListToRemoveFrom;
				if (effectToRemove != null && effectListToRemoveFrom != null && effectListToRemoveFrom.Contains(effectToRemove))
				{
					ServerEffectManager.Get().RemoveEffect(effectToRemove, effectListToRemoveFrom);
				}
			}
		}
		if (m_effectsForRefresh != null)
		{
			foreach (ServerAbilityUtils.EffectRefreshData effectRefreshData in m_effectsForRefresh)
			{
				global::Effect effectToRefresh = effectRefreshData.m_effectToRefresh;
				List<global::Effect> effectListOfEffect = effectRefreshData.m_effectListOfEffect;
				if (effectToRefresh != null && effectListOfEffect != null && effectListOfEffect.Contains(effectToRefresh))
				{
					effectToRefresh.Refresh();
				}
			}
		}
		if (m_effects != null)
		{
			foreach (global::Effect effect in m_effects)
			{
				ServerEffectManager.Get().ApplyEffect(effect, 1);
			}
		}

		// rogues?
		//if (m_effectTriggers != null)
		//{
		//	foreach (EffectTrigger effectTrigger in m_effectTriggers)
		//	{
		//		ServerEffectManager.Get().ApplyEffectTrigger(effectTrigger);
		//	}
		//}

		if (m_barriersForRemoval != null)
		{
			foreach (ServerAbilityUtils.BarrierRemovalData barrierRemovalData in m_barriersForRemoval)
			{
				Barrier barrierToRemove = barrierRemovalData.m_barrierToRemove;
				if (barrierToRemove != null && BarrierManager.Get().HasBarrier(barrierToRemove))
				{
					if (barrierRemovalData.m_removeLinkedBarriers)
					{
						List<Barrier> linkedBarriers = barrierToRemove.GetLinkedBarriers();
						if (linkedBarriers != null)
						{
							foreach (Barrier barrier in linkedBarriers)
							{
								if (barrier != null && BarrierManager.Get().HasBarrier(barrier))
								{
									BarrierManager.Get().RemoveBarrier(barrier, true);
								}
							}
						}
					}
					BarrierManager.Get().RemoveBarrier(barrierToRemove, true);
				}
			}
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
		if (m_barriers != null)
		{
			List<ActorData> list = new List<ActorData>();
			foreach (Barrier barrierToAdd in m_barriers)
			{
				List<ActorData> list2;
				BarrierManager.Get().AddBarrier(barrierToAdd, true, out list2);
				foreach (ActorData item in list2)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				foreach (ActorData actorData in list)
				{
					actorData.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
				}
				foreach (ActorData actorData2 in GameFlowData.Get().GetActors())
				{
					if (actorData2.IsActorVisibleToAnyEnemy())
					{
						actorData2.SynchronizeTeamSensitiveData();
					}
				}
			}
			GameFlowData.Get().UpdateCoverFromBarriersForAllActors();
		}
		if (m_powerupsForRemoval != null)
		{
			foreach (ServerAbilityUtils.PowerupRemovalData powerupRemovalData in m_powerupsForRemoval)
			{
				PowerUp powerupToRemove = powerupRemovalData.m_powerupToRemove;
				if (powerupToRemove != null && PowerUpManager.Get().HasPowerup(powerupToRemove))
				{
					PowerUpManager.Get().UsePowerup(powerupToRemove, m_hitParameters.Target);
				}
			}
		}
		if (m_miscEvents != null)
		{
			foreach (MiscHitEventData miscHitEventData in m_miscEvents)
			{
				miscHitEventData.ExecuteMiscHitEvent(this);
			}
		}
		if (m_gameModeEvents != null)
		{
			foreach (GameModeEvent gameModeEvent in m_gameModeEvents)
			{
				gameModeEvent.ExecuteGameModeEvent();
			}
		}
		if (m_spoilSpawns != null)
		{
			foreach (SpoilSpawnDataForAbilityHit spoilSpawnDataForAbilityHit in m_spoilSpawns)
			{
				if (m_hitParameters.Ability == null && m_hitParameters.Effect != null && m_hitParameters.Effect.Parent.Ability != null)
				{
					Ability ability = m_hitParameters.Effect.Parent.Ability;
				}
				spoilSpawnDataForAbilityHit.SpawnSpoils(m_hitParameters.Caster, m_hitParameters.Ability);
			}
		}
		if (m_powerUpsToSteal != null)
		{
			foreach (ServerAbilityUtils.PowerUpStealData powerUpStealData in m_powerUpsToSteal)
			{
				powerUpStealData.m_results.ReactToStealingHitExecution();
			}
		}
		if (m_directSpoilHitResults != null)
		{
			foreach (MovementResults movementResults in m_directSpoilHitResults)
			{
				movementResults.ExecuteUnexecutedMovementHits(false);
			}
		}

		// rogues
		//if (m_dynamicGeoForDamage != null && !NetworkClient.active)
		//{
		//	foreach (KeyValuePair<BoardSquare, int> keyValuePair in m_dynamicGeoForDamage)
		//	{
		//		DynamicMissionGeoManager.Get().AddDamageForGeoAtSquare(keyValuePair.Key, keyValuePair.Value);
		//	}
		//}
		//if (this.m_gearStatTerms != null)
		//{
		//	EquipmentStats equipmentStats = m_hitParameters.Target.GetEquipmentStats();
		//	foreach (GearStatTerm term in this.m_gearStatTerms)
		//	{
		//		equipmentStats.AddActorStat(term);
		//	}
		//}

		if (m_hitDelegate != null)
		{
			m_hitDelegate(m_hitParameters);
		}
		if (m_hitParameters.DamageSource.IsAbility() && m_hitParameters.Caster != null)
		{
			m_hitParameters.Caster.GetAbilityData().OnAbilityHit(m_hitParameters.DamageSource.Ability, m_hitParameters.Target, IsFromMovement());
		}
		if (!IsFromMovement())
		{
			Ability relevantAbility = m_hitParameters.GetRelevantAbility();
			if (relevantAbility != null)
			{
				if (m_hitParameters.Caster != null && m_hitParameters.Caster.GetTeam() != m_hitParameters.Target.GetTeam())
				{
					if (m_hitParameters.Ability != null)
					{
						if (relevantAbility.ShouldRevealCasterOnHostileAbilityHit())
						{
							m_hitParameters.Caster.UpdateServerLastKnownPosForHit();
						}
						bool flag5 = relevantAbility.ShouldRevealTargetOnHostileAbilityHit();
						if (flag5 && IsReaction && TriggeringHit != null && TriggeringHit.m_hitParameters != null && TriggeringHit.m_hitParameters.GetRelevantAbility() != null && AbilityUtils.AbilityHasTag(TriggeringHit.m_hitParameters.GetRelevantAbility(), AbilityTags.DontRevealCasterWhenTriggeringHostileReactionHit))
						{
							flag5 = false;
						}
						if (flag5)
						{
							m_hitParameters.Target.UpdateServerLastKnownPosForHit();
						}
					}
					else if (m_hitParameters.Effect != null || m_hitParameters.Barrier != null)
					{
						if (relevantAbility.ShouldRevealCasterOnHostileEffectOrBarrierHit())
						{
							m_hitParameters.Caster.UpdateServerLastKnownPosForHit();
						}
						bool flag6 = relevantAbility.ShouldRevealTargetOnHostileEffectOrBarrierHit();
						if (flag6 && IsReaction && TriggeringHit != null && TriggeringHit.m_hitParameters != null && TriggeringHit.m_hitParameters.GetRelevantAbility() != null && AbilityUtils.AbilityHasTag(TriggeringHit.m_hitParameters.GetRelevantAbility(), AbilityTags.DontRevealCasterWhenTriggeringHostileReactionHit))
						{
							flag6 = false;
						}
						if (flag6)
						{
							m_hitParameters.Target.UpdateServerLastKnownPosForHit();
						}
					}
				}
				if (m_hitParameters.Effect != null && m_hitParameters.Effect.Target != null && m_hitParameters.Effect.Target.GetTeam() != m_hitParameters.Target.GetTeam() && relevantAbility.ShouldRevealEffectHolderOnHostileEffectHit())
				{
					m_hitParameters.Effect.Target.UpdateServerLastKnownPosForHit();
				}
			}
			if (m_actorsToReveal != null)
			{
				for (int i = 0; i < m_actorsToReveal.Count; i++)
				{
					m_actorsToReveal[i].UpdateServerLastKnownPosForHit();
				}
			}
		}
		ExecutedResults = true;
		if (m_hitParameters.GetRelevantAbility() != null)
		{
			m_hitParameters.GetRelevantAbility().UpdateStatsForActorHit(m_hitParameters.Caster, m_hitParameters.Target, this);
			m_hitParameters.GetRelevantAbility().OnExecutedActorHit_General(m_hitParameters.Caster, m_hitParameters.Target, this);
			if (m_hitParameters.Ability != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedActorHit_Ability(m_hitParameters.Caster, m_hitParameters.Target, this);
			}
			if (m_hitParameters.Effect != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedActorHit_Effect(m_hitParameters.Caster, m_hitParameters.Target, this);
			}
			if (m_hitParameters.Barrier != null)
			{
				m_hitParameters.GetRelevantAbility().OnExecutedActorHit_Barrier(m_hitParameters.Caster, m_hitParameters.Target, this);
			}
		}
		ServerEffectManager.Get().NotifyEffectsOnExecutedActorHit(m_hitParameters.Caster, m_hitParameters.Target, this);
	}

	public bool ExecutedResults
	{
		get
		{
			return m_executed;
		}
		private set
		{
			m_executed = value;
		}
	}

	// custom
	public bool TargetInCover(ServerCombatManager.DamageType damageType)
	{
		if (m_hitParameters.Caster.GetTeam() != m_hitParameters.Target.GetTeam())
		{
			return ServerCombatManager.TargetInCover(m_hitParameters.DamageSource, m_hitParameters.Target, m_hitParameters.Origin, m_coverIgnoreMinDist, damageType, IsFromMovement());
		}
		return false;
	}

	// rogues
	//public HitChanceBracketType TargetInCover(ServerCombatManager.DamageType damageType)
	//{
	//	HitChanceBracketType result = HitChanceBracketType.Default;
	//	if (m_hitParameters.Caster.GetTeam() != m_hitParameters.Target.GetTeam())
	//	{
	//		ServerCombatManager.TargetInCover(m_hitParameters.DamageSource, m_hitParameters.Target, m_hitParameters.Origin, m_coverIgnoreMinDist, damageType, IsFromMovement(), out result);
	//	}
	//	return result;
	//}

	// rogues?
	//public void ProcessEffectTemplates()
	//{
	//	if (m_effectTriggerTemplates != null)
	//	{
	//		m_effectTriggers = (m_effectTriggers ?? new List<EffectTrigger>(m_effectTriggerTemplates.Count));
	//		foreach (EffectTriggerTemplate effectTriggerTemplate in m_effectTriggerTemplates.Distinct<EffectTriggerTemplate>())
	//		{
	//			EffectTrigger effectTrigger = new EffectTrigger(effectTriggerTemplate, m_hitParameters.GetEffectSource(), m_hitParameters.Target.CurrentBoardSquare, m_hitParameters.Target.AsArray(), m_hitParameters.Caster);
	//			effectTrigger.EffectOrigin = m_hitParameters.EffectOrigin;
	//			m_effectTriggers.Add(effectTrigger);
	//			foreach (EffectTemplate item in effectTriggerTemplate.onStartEffectTemplates)
	//			{
	//				m_effectTemplates = (m_effectTemplates ?? new List<EffectTemplate>());
	//				m_effectTemplates.Add(item);
	//			}
	//		}
	//		m_effectTriggerTemplates.Clear();
	//		m_effectTriggerTemplates = null;
	//	}
	//	if (m_effectTemplates != null)
	//	{
	//		if (m_effects == null)
	//		{
	//			m_effects = new List<global::Effect>(m_effectTemplates.Count);
	//		}
	//		foreach (EffectTemplate effectTemplate in m_effectTemplates)
	//		{
	//			ActorData target = (effectTemplate.scope == EffectTemplate.Scope.Target) ? m_hitParameters.Target : null;
	//			EffectSystem.Effect effect = new EffectSystem.Effect(effectTemplate, m_hitParameters.EffectOrigin ?? effectTemplate, m_hitParameters.GetEffectSource(), m_hitParameters.Target.CurrentBoardSquare, target, m_hitParameters.Caster);
	//			effect.Resolve();
	//			m_effects.Add(effect);
	//		}
	//		m_effectTemplates.Clear();
	//		m_effectTemplates = null;
	//	}
	//}

	public void CalcFinalValues(ServerCombatManager.DamageType damageType, ServerCombatManager.HealingType healingType, ServerCombatManager.TechPointChangeType techPointChangeType, bool isReal)
	{
		m_damageType = damageType;
		m_healingType = healingType;
		if (m_baseDamage != 0)
		{
			m_finalDamage = ServerCombatManager.Get().CalcDamage(m_hitParameters.DamageSource, m_hitParameters.Caster, m_hitParameters.Target, m_baseDamage, m_hitParameters.Origin, m_coverIgnoreMinDist, damageType, IsFromMovement(), out m_lifestealHealing, out m_targetInCoverWrtDamage, out m_damageBoosted, out m_damageReduced, out m_damageStatAdjustments, m_damageCalcScratch, true);  // log: false in rogues
		}
		if (m_overrideAsInCover)
		{
			m_targetInCoverWrtDamage = true;
		}
		if (m_baseHealing != 0)
		{
			m_finalHealing = ServerCombatManager.Get().CalcHealing(m_hitParameters.Caster, m_hitParameters.Target, m_baseHealing, m_hitParameters.DamageSource, healingType);
		}
		Ability relevantAbility = m_hitParameters.GetRelevantAbility();
		AbilityData.ActionType actionType = (relevantAbility != null) ? relevantAbility.CachedActionType : AbilityData.ActionType.INVALID_ACTION;
		m_energyStatAdjustOnTarget = new ServerGameplayUtils.EnergyStatAdjustments(m_hitParameters.Caster, m_hitParameters.Target);
		if (m_baseTechPointsGain != 0)
		{
			m_finalTechPointsGain = ServerCombatManager.Get().CalcTechPointGain(m_hitParameters.Target, m_baseTechPointsGain, actionType, m_energyStatAdjustOnTarget);
		}
		if (m_finalDamage > 0)
		{
			int num = m_hitParameters.Target.GetActorStats().CalculateEnergyGainOnDamage(m_finalDamage, m_energyStatAdjustOnTarget);
			if (isReal)
			{
				PassiveData passiveData = m_hitParameters.Target.GetPassiveData();
				if (passiveData != null)
				{
					num += passiveData.CalculateEnergyGainOnDamageFromPassives(m_finalDamage, m_hitParameters.Caster, m_hitParameters.GetRelevantAbility(), m_hitParameters.Effect);
				}
			}
			m_finalTechPointsGain += num;
		}
		m_energyStatAdjustOnTarget.CalculateAdjustments();
		if (m_baseTechPointsLoss != 0)
		{
			m_finalTechPointsLoss = ServerCombatManager.Get().CalcTechPointLoss(m_hitParameters.Caster, m_hitParameters.Target, m_baseTechPointsLoss, m_hitParameters.DamageSource, techPointChangeType);
		}
		m_energyStatAdjustOnCaster = new ServerGameplayUtils.EnergyStatAdjustments(m_hitParameters.Caster, m_hitParameters.Caster);
		if (m_baseTechPointGainOnCaster != 0)
		{
			m_finalTechPointGainOnCaster = ServerCombatManager.Get().CalcTechPointGain(m_hitParameters.Caster, m_baseTechPointGainOnCaster, actionType, m_energyStatAdjustOnCaster);
		}
		if (m_baseDirectTechPointGainOnCaster > 0)
		{
			m_finalTechPointGainOnCaster += m_baseDirectTechPointGainOnCaster;
		}
		if (isReal)
		{
			Ability relevantAbility2 = m_hitParameters.GetRelevantAbility();
			if (relevantAbility2 != null && m_hitParameters.Caster != null)
			{
				AbilityData abilityData = m_hitParameters.Caster.GetAbilityData();
				if (!m_ignoreTechpointInteractionsForHit && !m_hitParameters.Target.IgnoreForEnergyOnHit)
				{
					if (m_baseDamage != 0)
					{
						int techPointRewardForAbilityDamage = abilityData.GetTechPointRewardForAbilityDamage(relevantAbility2, true, m_energyStatAdjustOnCaster);
						if (techPointRewardForAbilityDamage > 0)
						{
							m_finalTechPointGainOnCaster += techPointRewardForAbilityDamage;
						}
					}
					int techPointRewardForAbilityHit = abilityData.GetTechPointRewardForAbilityHit(relevantAbility2, true, m_hitParameters.Target, m_hitParameters.Caster, m_energyStatAdjustOnCaster);
					if (techPointRewardForAbilityHit > 0)
					{
						m_finalTechPointGainOnCaster += techPointRewardForAbilityHit;
					}
				}
			}
			m_energyStatAdjustOnCaster.CalculateAdjustments();
		}
		if (m_baseObjectivePointsChange != 0)
		{
			m_finalObjectivePointsChange = m_baseObjectivePointsChange;
		}
		ConvertSpoilSpawnsOnAllySquares();
		if (m_powerUpsToSteal != null)
		{
			foreach (ServerAbilityUtils.PowerUpStealData powerUpStealData in m_powerUpsToSteal)
			{
				powerUpStealData.m_results = powerUpStealData.m_powerUp.BuildPowerupResultsForAbilityHit(m_hitParameters.Target, null);
			}
		}
		CalculateReactionsToHit(m_reactionDepth, ForMovementStage, isReal);
		if (m_standardEffectDatas != null)
		{
			if (m_effects == null)
			{
				m_effects = new List<global::Effect>(m_standardEffectDatas.Count);
			}
			foreach (StandardActorEffectData data in m_standardEffectDatas)
			{
				StandardActorEffect item = new StandardActorEffect(m_hitParameters, data);
				m_effects.Add(item);
			}
			m_standardEffectDatas.Clear();
			m_standardEffectDatas = null;
		}

		// rogues?
		//ProcessEffectTemplates();
	}

	public void FinalizeHitResults(bool isReal)
	{
		if (isReal)
		{
			if (m_spoilSpawns != null && m_spoilSpawns.Count > 0 && m_hitParameters.Ability != null && m_hitParameters.Caster != null)
			{
				AbilityPriority runPriority = m_hitParameters.Ability.GetRunPriority();
				if (runPriority == AbilityPriority.Prep_Defense || runPriority == AbilityPriority.Prep_Offense || runPriority == AbilityPriority.Combat_Damage)
				{
					List<TempSpoilVfxEffect.VfxSpawnData> list = new List<TempSpoilVfxEffect.VfxSpawnData>();
					List<SpoilSpawnDataForAbilityHit> list2 = new List<SpoilSpawnDataForAbilityHit>();
					for (int i = m_spoilSpawns.Count - 1; i >= 0; i--)
					{
						SpoilSpawnDataForAbilityHit spoilSpawnDataForAbilityHit = m_spoilSpawns[i];
						BoardSquare desiredSpawnSquare = spoilSpawnDataForAbilityHit.GetDesiredSpawnSquare();
						if (desiredSpawnSquare != null && spoilSpawnDataForAbilityHit.CanSpawnSpoils())
						{
							foreach (BoardSquare boardSquare in SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSpawnSquare, m_hitParameters.Caster.GetTeam(), spoilSpawnDataForAbilityHit.m_numToSpawn, spoilSpawnDataForAbilityHit.m_canSpawnOnEnemyOccupiedSquare, spoilSpawnDataForAbilityHit.m_canSpawnOnAllyOccupiedSquare, 4, ServerActionBuffer.Get().m_tempReservedSquaresForAbilitySpoil))
							{
								PowerUp powerUp = spoilSpawnDataForAbilityHit.ChooseRandomPowerupComponent();
								if (powerUp != null)
								{
									SpoilSpawnDataForAbilityHit shallowCopy = spoilSpawnDataForAbilityHit.GetShallowCopy();
									shallowCopy.m_powerupPrefabs = new List<GameObject>
									{
										powerUp.gameObject
									};
									shallowCopy.m_numToSpawn = 1;
									shallowCopy.m_spawnSquareSourceType = SpoilSpawnDataForAbilityHit.SpawnSquareSource.FromBoardSquare;
									shallowCopy.m_boardSquare = boardSquare;
									shallowCopy.m_ignoreSpawnSplineForSequence = true;
									list2.Add(shallowCopy);
									ServerActionBuffer.Get().m_tempReservedSquaresForAbilitySpoil.Add(boardSquare);
									list.Add(new TempSpoilVfxEffect.VfxSpawnData(boardSquare, powerUp.m_sequencePrefab, powerUp.m_restrictPickupByTeam));
								}
								else if (Application.isEditor)
								{
									Debug.LogWarning("Spoil Spawn Data has prefab without powerup component, when trying to finalize spoil spawn data for " + m_hitParameters.Ability.GetDebugIdentifier(""));
								}
							}
						}
					}
					m_spoilSpawns = list2;
					if (list.Count > 0)
					{
						TempSpoilVfxEffect effect = new TempSpoilVfxEffect(m_hitParameters.Ability.AsEffectSource(), m_hitParameters.Caster, list, runPriority);
						this.AddEffect(effect);
					}
				}
			}
			if (m_powerUpsToSteal != null)
			{
				foreach (ServerAbilityUtils.PowerUpStealData powerUpStealData in m_powerUpsToSteal)
				{
					powerUpStealData.m_powerUp.OnWillBeStolen(m_hitParameters.Caster);
				}
			}
		}
	}

	private void ConvertSpoilSpawnsOnAllySquares()
	{
		if (m_spoilSpawns != null && SpoilsManager.Get() != null)
		{
			m_directSpoilHitResults = new List<MovementResults>();
			List<SpoilSpawnDataForAbilityHit> list = new List<SpoilSpawnDataForAbilityHit>();
			List<BoardSquare> list2 = new List<BoardSquare>();
			for (int i = m_spoilSpawns.Count - 1; i >= 0; i--)
			{
				SpoilSpawnDataForAbilityHit spoilSpawnDataForAbilityHit = m_spoilSpawns[i];
				if (spoilSpawnDataForAbilityHit.m_canSpawnOnAllyOccupiedSquare && spoilSpawnDataForAbilityHit.CanSpawnSpoils())
				{
					BoardSquare desiredSpawnSquare = spoilSpawnDataForAbilityHit.GetDesiredSpawnSquare();
					if (desiredSpawnSquare != null)
					{
						Team team;
						if (m_hitParameters.Caster == null)
						{
							team = Team.Invalid;
						}
						else
						{
							team = m_hitParameters.Caster.GetTeam();
						}
						List<BoardSquare> list3 = SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSpawnSquare, team, spoilSpawnDataForAbilityHit.m_numToSpawn, spoilSpawnDataForAbilityHit.m_canSpawnOnEnemyOccupiedSquare, spoilSpawnDataForAbilityHit.m_canSpawnOnAllyOccupiedSquare, 4, list2);
						int num = 0;
						while (num < list3.Count && num < spoilSpawnDataForAbilityHit.m_numToSpawn)
						{
							ActorData occupantActor = list3[num].OccupantActor;
							if (occupantActor != null && occupantActor.GetTeam() == team)
							{
								PowerUp powerUp = spoilSpawnDataForAbilityHit.ChooseRandomPowerupComponent();
								if (powerUp != null && powerUp.m_ability != null)
								{
									MovementResults movementResults = powerUp.BuildDirectPowerupHitResults(occupantActor, list3[num], m_hitParameters.DamageSource.Ability, m_hitParameters.Caster, spoilSpawnDataForAbilityHit.m_spoilMod);
									if (movementResults != null)
									{
										m_directSpoilHitResults.Add(movementResults);
									}
								}
								list2.Add(list3[num]);
							}
							else
							{
								SpoilSpawnDataForAbilityHit shallowCopy = spoilSpawnDataForAbilityHit.GetShallowCopy();
								shallowCopy.m_canSpawnOnAllyOccupiedSquare = false;
								shallowCopy.m_numToSpawn = 1;
								list.Add(shallowCopy);
							}
							num++;
						}
					}
					m_spoilSpawns.RemoveAt(i);
				}
			}
			for (int j = 0; j < m_spoilSpawns.Count; j++)
			{
				list.Add(m_spoilSpawns[j]);
			}
			m_spoilSpawns = list;
		}
	}

	private void CalculateReactionsToHit(int currentDepth, MovementStage movementStage, bool isReal)
	{
		bool flag = movementStage == MovementStage.Normal;
		if (currentDepth <= 10)
		{
			List<global::Effect> actorEffects = ServerEffectManager.Get().GetActorEffects(m_hitParameters.Target);
			m_reactions = new List<AbilityResults_Reaction>();
			foreach (global::Effect effect in actorEffects)
			{
				if (flag ? effect.CanReactToNormalMovementHit(this, true) : (CanBeReactedTo || effect.ShouldForceReactToHit(this)))
				{
					effect.GatherResultsInResponseToActorHit(this, ref m_reactions, isReal);
				}
			}
			if (!(m_hitParameters.Caster != null))
			{
				return;
			}
			using (List<global::Effect>.Enumerator enumerator = ServerEffectManager.Get().GetActorEffects(m_hitParameters.Caster).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					global::Effect effect2 = enumerator.Current;
					if (flag ? effect2.CanReactToNormalMovementHit(this, false) : (CanBeReactedTo || effect2.ShouldForceReactToHit(this)))
					{
						effect2.GatherResultsInResponseToOutgoingActorHit(this, ref m_reactions, isReal);
					}
				}
				return;
			}
		}
		Debug.LogError("more than 10 levels of reaction hit, may be infinite loop on reaction hits");
	}

	public void AdjustDamageResultsWithReactions(ref Dictionary<ActorData, int> damageResults, ref Dictionary<ActorData, int> damageResults_gross)
	{
		if (CanBeReactedTo && HasReactions)
		{
			foreach (AbilityResults_Reaction abilityResults_Reaction in m_reactions)
			{
				foreach (KeyValuePair<ActorData, int> keyValuePair in abilityResults_Reaction.GetReactionDamageResults())
				{
					if (damageResults.ContainsKey(keyValuePair.Key))
					{
						Dictionary<ActorData, int> dictionary = damageResults;
						ActorData key = keyValuePair.Key;
						dictionary[key] += keyValuePair.Value;
					}
					else
					{
						damageResults[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				foreach (KeyValuePair<ActorData, int> keyValuePair2 in abilityResults_Reaction.GetReactionDamageResults_Gross())
				{
					if (damageResults_gross.ContainsKey(keyValuePair2.Key))
					{
						Dictionary<ActorData, int> dictionary = damageResults_gross;
						ActorData key = keyValuePair2.Key;
						dictionary[key] += keyValuePair2.Value;
					}
					else
					{
						damageResults_gross[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
			}
		}
	}

	public void AdjustDamageResultsWithPowerups(ref Dictionary<ActorData, int> damageResults)
	{
		if (m_powerUpsToSteal != null)
		{
			foreach (ServerAbilityUtils.PowerUpStealData powerUpStealData in m_powerUpsToSteal)
			{
				foreach (KeyValuePair<ActorData, int> keyValuePair in powerUpStealData.m_results.GetPowerupDamageResults())
				{
					if (damageResults.ContainsKey(keyValuePair.Key))
					{
						Dictionary<ActorData, int> dictionary = damageResults;
						ActorData key = keyValuePair.Key;
						dictionary[key] += keyValuePair.Value;
					}
					else
					{
						damageResults[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
		}
		if (m_directSpoilHitResults != null)
		{
			for (int i = 0; i < m_directSpoilHitResults.Count; i++)
			{
				foreach (KeyValuePair<ActorData, int> keyValuePair2 in m_directSpoilHitResults[i].GetMovementDamageResults())
				{
					if (damageResults.ContainsKey(keyValuePair2.Key))
					{
						Dictionary<ActorData, int> dictionary = damageResults;
						ActorData key = keyValuePair2.Key;
						dictionary[key] += keyValuePair2.Value;
					}
					else
					{
						damageResults[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
			}
		}
	}

	public int HealthDelta
	{
		get
		{
			return m_finalHealing - m_finalDamage;
		}
		private set
		{
		}
	}

	public int AppliedAbsorb
	{
		get
		{
			int num = 0;
			if (m_effects != null)
			{
				foreach (global::Effect effect in m_effects)
				{
					if (effect.Absorbtion.m_absorbRemaining > 0)
					{
						num += effect.Absorbtion.m_absorbRemaining;
					}
				}
			}
			return num;
		}
	}

	public KnockbackHitData KnockbackHitData
	{
		get
		{
			return m_knockbackData;
		}
		private set
		{
		}
	}

	public bool HasKnockback
	{
		get
		{
			return m_knockbackData != null;
		}
		private set
		{
			Debug.LogError("Trying to directly set HasKnockback.");
		}
	}

	public bool HasReactions
	{
		get
		{
			return m_reactions != null && m_reactions.Count > 0;
		}
		private set
		{
			Debug.LogError("Trying to directly set HasReactions.");
		}
	}

	public bool ReactionHitsDone()
	{
		if (m_reactions != null && m_reactions.Count > 0)
		{
			using (List<AbilityResults_Reaction>.Enumerator enumerator = m_reactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.ReactionHitsDone())
					{
						return false;
					}
				}
			}
			return true;
		}
		return true;
	}

	public void ExecuteUnexecutedReactionHits(bool asFailsafe)
	{
		if (m_reactions != null && m_reactions.Count > 0)
		{
			foreach (AbilityResults_Reaction abilityResults_Reaction in m_reactions)
			{
				abilityResults_Reaction.ExecuteUnexecutedReactionHits(asFailsafe);
			}
		}
	}

	public bool HasDamage
	{
		get
		{
			return m_baseDamage > 0;
		}
		private set
		{
			Debug.LogError("Trying to directly set HasDamage.");
		}
	}

	public bool HasHealing
	{
		get
		{
			return m_baseHealing > 0;
		}
		private set
		{
			Debug.LogError("Trying to directly set HasHealing.");
		}
	}

	public bool CanBeReactedTo
	{
		get
		{
			return m_canBeReactedTo;
		}
		set
		{
			m_canBeReactedTo = value;
		}
	}

	public bool IsReaction
	{
		get
		{
			return m_isReaction;
		}
		set
		{
			m_isReaction = value;
		}
	}

	public ActorHitResults TriggeringHit
	{
		get
		{
			return m_triggeringHit;
		}
		set
		{
			m_triggeringHit = value;
		}
	}

	public bool AppliedStatus(StatusType status)
	{
		bool flag = false;
		if (m_effects != null)
		{
			foreach (global::Effect effect in m_effects)
			{
				List<StatusType> statuses = effect.GetStatuses();
				if (statuses != null && statuses.Contains(status))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag && m_gameModeEvents != null)
		{
			using (List<GameModeEvent>.Enumerator enumerator2 = m_gameModeEvents.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.AppliesStatusToActor(status, m_hitParameters.Target))
					{
						flag = true;
						break;
					}
				}
			}
		}
		return flag;
	}

	public bool HasHitResultsTag(HitResultsTags tag)
	{
		return m_tags != null && m_tags.Contains(tag);
	}

	public bool HasOvercon(int overconId)
	{
		return m_overconIds != null && m_overconIds.Contains(overconId);
	}

	public static bool HitsInCollectionDoneExecuting(Dictionary<ActorData, ActorHitResults> actorToHitResults)
	{
		bool result = true;
		foreach (ActorHitResults actorHitResults in actorToHitResults.Values)
		{
			if (!actorHitResults.ExecutedResults)
			{
				result = false;
				break;
			}
			if (actorHitResults.HasReactions && !actorHitResults.ReactionHitsDone())
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static void ExecuteUnexecutedActorHits(Dictionary<ActorData, ActorHitResults> actorHitResults, bool asFailsafe, string logHeader)
	{
		foreach (ActorHitResults actorHitResults2 in actorHitResults.Values)
		{
			if (!actorHitResults2.ExecutedResults)
			{
				if (asFailsafe)
				{
					ActorData target = actorHitResults2.m_hitParameters.Target;
					Debug.LogError(string.Concat(new string[]
					{
						logHeader,
						" force-executing ability hit on actor ",
						target.name,
						" ",
						target.DisplayName,
						" due to failsafe."
					}));
				}
				actorHitResults2.ExecuteResults();
			}
			if (actorHitResults2.HasReactions)
			{
				actorHitResults2.ExecuteUnexecutedReactionHits(asFailsafe);
			}
		}
	}

	public void AddHitActorIdsInReactions(HashSet<int> hitActorIds)
	{
		if (HasReactions && CanBeReactedTo)
		{
			for (int i = 0; i < m_reactions.Count; i++)
			{
				m_reactions[i].AddHitActorIds(hitActorIds);
			}
		}
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos()
	{
		bool result = false;
		if (!IsFromMovement())
		{
			Debug.LogWarning("AbilityResults testing whether movement hit should update target last known pos, but the hit isn't from movement.");
		}
		Ability relevantAbility = m_hitParameters.GetRelevantAbility();
		if (relevantAbility != null && m_hitParameters.Caster != null && m_hitParameters.Caster.GetTeam() != m_hitParameters.Target.GetTeam())
		{
			if (m_hitParameters.Ability != null)
			{
				result = relevantAbility.ShouldRevealTargetOnHostileAbilityHit();
			}
			else if (m_hitParameters.Effect != null || m_hitParameters.Barrier != null)
			{
				result = relevantAbility.ShouldRevealTargetOnHostileEffectOrBarrierHit();
			}
		}
		return result;
	}

	public string ToDebugString()
	{
		string text = (m_hitParameters.Caster == null) ? "[null]" : m_hitParameters.Caster.DebugNameString("white");
		string text2 = string.Concat(new string[]
		{
			"Caster: ",
			text,
			" to Target: ",
			m_hitParameters.Target.DebugNameString("white"),
			"\n"
		});
		text2 = text2 + " [Base Damage]=" + m_baseDamage;
		text2 = text2 + " [Base Healing]=" + m_baseHealing;
		text2 = text2 + " [Base TP Gain]=" + m_baseTechPointsGain;
		text2 = text2 + " [Base TP Loss]=" + m_baseTechPointsLoss;
		if (m_effects != null)
		{
			text2 += "\n";
			foreach (global::Effect effect in m_effects)
			{
				if (effect != null)
				{
					text2 += effect.GetInEditorDescription();
				}
			}
		}
		if (m_standardEffectDatas != null)
		{
			text2 += "\n";
			foreach (StandardActorEffectData standardActorEffectData in m_standardEffectDatas)
			{
				text2 += standardActorEffectData.GetInEditorDescription("", true, false, null);
			}
		}
		if (m_knockbackData != null)
		{
			text2 = string.Concat(new string[]
			{
				text2,
				"\nKnockback Data, By Actor ",
				m_knockbackData.m_sourceActor.GetClassName(),
				" to Actor ",
				m_knockbackData.m_target.GetClassName(),
				" KnockbackType ",
				m_knockbackData.m_type.ToString()
			});
		}
		text2 += "\n";
		return text2;
	}

	// TODO review serialization, some parts are obviously from rogues
	public void ActorHitResults_SerializeToStream(NetworkWriter writer)
	{
		int position = writer.Position;
		bool hasDamage = m_baseDamage != 0;
		bool hasHealing = m_baseHealing != 0;
		bool hasTechPointGain = m_baseTechPointsGain != 0 || m_finalTechPointsGain > 0;
		bool hasTechPointLoss = m_baseTechPointsLoss != 0;
		bool hasTechPointGainOnCaster = m_baseTechPointGainOnCaster != 0 || m_finalTechPointGainOnCaster > 0;
		bool triggerCasterVisOnHitVisualOnly = m_lifestealHealing > 0;
		bool updateCasterLastKnownPos = false;
		bool updateTargetLastKnownPos = false;
		bool updateEffectHolderLastKnownPos = false;
		bool updateOtherLastKnownPos = m_actorsToReveal != null && m_actorsToReveal.Count > 0;
		Ability relevantAbility = m_hitParameters.GetRelevantAbility();
		if (relevantAbility != null)
		{
			if (m_hitParameters.Caster != null && m_hitParameters.Caster.GetTeam() != m_hitParameters.Target.GetTeam())
			{
				if (m_hitParameters.Ability != null)
				{
					updateCasterLastKnownPos = relevantAbility.ShouldRevealCasterOnHostileAbilityHit();
					updateTargetLastKnownPos = relevantAbility.ShouldRevealTargetOnHostileAbilityHit();
				}
				if (m_hitParameters.Effect != null || m_hitParameters.Barrier != null)
				{
					updateCasterLastKnownPos = relevantAbility.ShouldRevealCasterOnHostileEffectOrBarrierHit();
					updateTargetLastKnownPos = relevantAbility.ShouldRevealTargetOnHostileEffectOrBarrierHit();
				}
			}
			if (m_hitParameters.Effect != null && m_hitParameters.Effect.Target != null && m_hitParameters.Effect.Target.GetTeam() != m_hitParameters.Target.GetTeam())
			{
				updateEffectHolderLastKnownPos = relevantAbility.ShouldRevealEffectHolderOnHostileEffectHit();
			}
		}
		byte bitField1 = ServerClientUtils.CreateBitfieldFromBools(hasDamage, hasHealing, hasTechPointGain, hasTechPointLoss, hasTechPointGainOnCaster, HasKnockback, m_targetInCoverWrtDamage, CanBeReactedTo);
		writer.Write(bitField1);
		byte bitField2 = ServerClientUtils.CreateBitfieldFromBools(m_damageBoosted, m_damageReduced, updateCasterLastKnownPos, updateTargetLastKnownPos, updateEffectHolderLastKnownPos, updateOtherLastKnownPos, m_isPartOfHealOverTime, triggerCasterVisOnHitVisualOnly);
		writer.Write(bitField2);
		if (hasDamage)
		{
			writer.Write((short)m_finalDamage);
		}
		if (hasHealing)
		{
			writer.Write((short)m_finalHealing);
		}
		if (hasTechPointGain)
		{
			writer.Write((short)m_finalTechPointsGain);
		}
		if (hasTechPointLoss)
		{
			writer.Write((short)m_finalTechPointsLoss);
		}
		if (hasTechPointGainOnCaster)
		{
			writer.Write((short)m_finalTechPointGainOnCaster);
		}
		// rogues
		//if (triggerCasterVisOnHitVisualOnly)
		//{
		//	writer.Write((short)m_lifestealHealing);
		//}
		if (HasKnockback)
		{
			short actorIndex = HasKnockback && KnockbackHitData != null && KnockbackHitData.m_sourceActor != null
				? (short)KnockbackHitData.m_sourceActor.ActorIndex
				: (short)ActorData.s_invalidActorIndex;
			writer.Write(actorIndex);
		}
		if ((hasDamage && m_targetInCoverWrtDamage) || HasKnockback)
		{
			Vector3 origin = m_hitParameters.Origin;
			float x = origin.x;
			float z = origin.z;
			writer.Write(x);
			writer.Write(z);
		}

		// rogues
		//sbyte b5 = (sbyte)this.m_hitType;
		//writer.Write(b5);

		if (updateEffectHolderLastKnownPos)
		{
			short effectHolderActor = updateEffectHolderLastKnownPos
				? (short)m_hitParameters.Effect.Target.ActorIndex
				: (short)ActorData.s_invalidActorIndex;
			writer.Write(effectHolderActor);
		}
		if (updateOtherLastKnownPos)
		{
			byte otherActorsToUpdateVisibilityNum = (byte)m_actorsToReveal.Count;
			writer.Write(otherActorsToUpdateVisibilityNum);
			for (int i = 0; i < m_actorsToReveal.Count; i++)
			{
				short actorIndex = (short)m_actorsToReveal[i].ActorIndex;
				writer.Write(actorIndex);
			}
		}
		bool hasEffectsToStart = m_effects != null && m_effects.Count > 0;
		bool hasEffectsToRemove = m_effectsForRemoval != null && m_effectsForRemoval.Count > 0;
		bool hasBarriersToAdd = m_barriers != null && m_barriers.Count > 0;
		bool hasBarriersToRemove = m_barriersForRemoval != null && m_barriersForRemoval.Count > 0;
		bool hasSequencesToEnd = m_sequencesToEnd != null && m_sequencesToEnd.Count > 0;
		bool hasReactions = m_reactions != null && m_reactions.Count > 0;
		bool hasPowerupsToRemove = m_powerupsForRemoval != null && m_powerupsForRemoval.Count > 0;
		bool hasPowerupsToSteal = m_powerUpsToSteal != null && m_powerUpsToSteal.Count > 0;
		bool hasDirectPowerupHits = m_directSpoilHitResults != null && m_directSpoilHitResults.Count > 0;
		bool hasGameModeEvents = m_gameModeEvents != null && m_gameModeEvents.Count > 0;
		bool isCharacterSpecificAbility = IsCharacterSpecificAbility();
		bool hasOverconIds = m_overconIds != null && m_overconIds.Count > 0;

        // custom
        bool flag19 = false;
        // rogues
        //bool flag19 = m_dynamicGeoForDamage != null && m_dynamicGeoForDamage.Count > 0;

		byte bitField3 = ServerClientUtils.CreateBitfieldFromBools(hasEffectsToStart, hasEffectsToRemove, hasBarriersToRemove, hasSequencesToEnd, hasReactions, hasPowerupsToRemove, hasPowerupsToSteal, hasDirectPowerupHits);
		byte bitField4 = ServerClientUtils.CreateBitfieldFromBools(hasGameModeEvents, isCharacterSpecificAbility, hasBarriersToAdd, hasOverconIds, flag19, false, false, false);
		writer.Write(bitField3);
		writer.Write(bitField4);
		if (hasEffectsToStart)
		{
			AbilityResultsUtils.SerializeEffectsToStartToStream(m_effects, writer);
		}
		if (hasEffectsToRemove)
		{
			AbilityResultsUtils.SerializeEffectsForRemovalToStream(m_effectsForRemoval, writer);
		}
		if (hasBarriersToAdd)
		{
			AbilityResultsUtils.SerializeBarriersToStartToStream(m_barriers, writer);
		}
		if (hasBarriersToRemove)
		{
			AbilityResultsUtils.SerializeBarriersForRemovalToStream(m_barriersForRemoval, writer);
		}
		if (hasSequencesToEnd)
		{
			AbilityResultsUtils.SerializeSequenceEndDataListToStream(m_sequencesToEnd, writer);
		}
		if (hasReactions)
		{
			AbilityResultsUtils.SerializeServerReactionResultsToStream(m_reactions, writer);
		}
		if (hasPowerupsToRemove)
		{
			AbilityResultsUtils.SerializePowerupsToRemoveToStream(m_powerupsForRemoval, writer);
		}
		if (hasPowerupsToSteal)
		{
			AbilityResultsUtils.SerializePowerupsToStealToStream(m_powerUpsToSteal, writer);
		}
		if (hasDirectPowerupHits)
		{
			AbilityResultsUtils.SerializeServerMovementResultsListToStream(m_directSpoilHitResults, writer);
		}
		if (hasGameModeEvents)
		{
			AbilityResultsUtils.SerializeServerGameModeEventListToStream(m_gameModeEvents, writer);
		}
		if (hasOverconIds)
		{
			AbilityResultsUtils.SerializeServerOverconListToStream(m_overconIds, writer);
		}
		// rogues
		//if (flag19)
		//{
		//	AbilityResultsUtils.SerializeDynamicGeoDamageToStream(m_dynamicGeoForDamage, writer);
		//}
		int num10 = writer.Position - position;
		if (ClientAbilityResults.DebugSerializeSizeOn)
		{
			Debug.LogWarning("\t\t\t\t Serializing ActorHit: \n\t\t\t\t numBytes: " + num10);
		}
	}

	private bool IsCharacterSpecificAbility()
	{
		return !(m_hitParameters.GetRelevantAbility() == null) && !(m_hitParameters.Caster == null) && !(m_hitParameters.Caster.GetAbilityData() == null) && AbilityData.IsCharacterSpecificAbility(m_hitParameters.Caster.GetAbilityData().GetActionTypeOfAbility(m_hitParameters.GetRelevantAbility()));
	}

	public class DamageCalcScratch
	{
		public int m_damageAfterOutgoingMod;

		public int m_damageAfterIncomingBuffDebuff;

		public int m_damageAfterIncomingBuffDebuffWithCover;
	}

	public delegate void HitActorDelegate(ActorHitParameters parameters);
}
#endif
