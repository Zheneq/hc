// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BlasterKnockbackCone : Ability
{
	[Header("-- Cone Limits")]
	public float m_minLength;
	public float m_maxLength;
	public float m_minAngle;
	public float m_maxAngle;
	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	[Header("-- On Hit")]
	public int m_damageAmountNormal;
	
	// added in rogues
	// public int m_damageAmountOvercharged;
	
	public bool m_removeOverchargeEffectOnCast;
	public StandardEffectInfo m_enemyEffectNormal;
	public StandardEffectInfo m_enemyEffectOvercharged;
	[Header("-- Knockback on Enemy")]
	public float m_knockbackDistance;
	public float m_extraKnockbackDistOnOvercharged;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	[Header("-- Knockback on Self")]
	public float m_knockbackDistanceOnSelf;
	public KnockbackType m_knockbackTypeOnSelf = KnockbackType.BackwardAgainstAimDir;
	[Header("-- Set Overcharge as Free Action after cast?")]
	public bool m_overchargeAsFreeActionAfterCast;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_overchargedCastSequencePrefab;
	public GameObject m_unstoppableSetterSequencePrefab;

	private AbilityMod_BlasterKnockbackCone m_abilityMod;
	private BlasterOvercharge m_overchargeAbility;
	private Blaster_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEnemyEffectNormal;
	private StandardEffectInfo m_cachedEnemyEffectOvercharged;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Knockback Cone";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		// reactor
		m_overchargeAbility = GetAbilityOfType<BlasterOvercharge>();
		// rogues
		// m_overchargeAbility = GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge;
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone targeter = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
		targeter.InitKnockbackData(GetKnockbackDistance(), m_knockbackType, GetKnockbackDistanceOnSelf(), m_knockbackTypeOnSelf);
		targeter.SetExtraKnockbackDist(GetExtraKnockbackDistOnOvercharged());
		// reactor
		targeter.m_useExtraKnockbackDistDelegate = delegate
		{
			return m_syncComp != null && m_syncComp.m_overchargeBuffs > 0;
		};
		// rogues
		// targeter.m_useExtraKnockbackDistDelegate = caster => m_syncComp != null && m_syncComp.m_overchargeCount > 0;
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxLength();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyEffectNormal = m_abilityMod != null
			? m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal)
			: m_enemyEffectNormal;
		m_cachedEnemyEffectOvercharged = m_abilityMod != null
			? m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged)
			: m_enemyEffectOvercharged;
	}

	public float GetMinLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength) 
			: m_minLength;
	}

	public float GetMaxLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength)
			: m_maxLength;
	}

	public float GetMinAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle) 
			: m_minAngle;
	}

	public float GetMaxAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle)
			: m_maxAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight) 
			: m_penetrateLineOfSight;
	}

	public int GetDamageAmountNormal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal;
	}

	// added in rogues
	// public int GetDamageAmountOvercharged()
	// {
	// 	return m_abilityMod ? m_abilityMod.m_damageAmountOverchargedMod.GetModifiedValue(m_damageAmountOvercharged) : m_damageAmountOvercharged;
	// }

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		return m_cachedEnemyEffectNormal ?? m_enemyEffectNormal;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		return m_cachedEnemyEffectOvercharged ?? m_enemyEffectOvercharged;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance) 
			: m_knockbackDistance;
	}

	public float GetExtraKnockbackDistOnOvercharged()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraKnockbackDistOnOverchargedMod.GetModifiedValue(m_extraKnockbackDistOnOvercharged) 
			: m_extraKnockbackDistOnOvercharged;
	}

	public float GetKnockbackDistanceOnSelf()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_knockbackDistanceOnSelfMod.GetModifiedValue(m_knockbackDistanceOnSelf) 
			: m_knockbackDistanceOnSelf;
	}

	public bool OverchargeAsFreeActionAfterCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_overchargeAsFreeActionAfterCastMod.GetModifiedValue(m_overchargeAsFreeActionAfterCast)
			: m_overchargeAsFreeActionAfterCast;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(ActorData))
		{
			// reactor
			return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
			// rogues
			// return GetDamageAmountOvercharged() + GetMultiStackOverchargeDamage();
		}
		return GetDamageAmountNormal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterKnockbackCone abilityMod_BlasterKnockbackCone = modAsBase as AbilityMod_BlasterKnockbackCone;
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_BlasterKnockbackCone != null
			? abilityMod_BlasterKnockbackCone.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal);
		// added in rogues
		// AddTokenInt(tokens, "DamageAmountOvercharged", string.Empty, abilityMod_BlasterKnockbackCone
		// 	? abilityMod_BlasterKnockbackCone.m_damageAmountOverchargedMod.GetModifiedValue(m_damageAmountOvercharged)
		// 	: m_damageAmountOvercharged);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterKnockbackCone != null
			? abilityMod_BlasterKnockbackCone.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal)
			: m_enemyEffectNormal, "EnemyEffectNormal", m_enemyEffectNormal);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterKnockbackCone != null
			? abilityMod_BlasterKnockbackCone.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged)
			: m_enemyEffectOvercharged, "EnemyEffectOvercharged", m_enemyEffectOvercharged);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage())
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetCurrentModdedDamage();
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterKnockbackCone))
		{
			m_abilityMod = abilityMod as AbilityMod_BlasterKnockbackCone;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		if (ActorData.GetActorStatus().IsKnockbackImmune())
		{
			return MovementAdjustment.ReducedMovement;
		}
		AbilityData abilityData = ActorData.GetAbilityData();
		List<AbilityData.AbilityEntry> queuedOrAimingAbilities = abilityData.GetQueuedOrAimingAbilities();
		foreach (AbilityData.AbilityEntry current in queuedOrAimingAbilities)
		{
			Card_Standard_Ability card_Standard_Ability = current.ability as Card_Standard_Ability;
			if (card_Standard_Ability != null && card_Standard_Ability.m_applyEffect)
			{
				foreach (StatusType statusType in card_Standard_Ability.m_effect.m_statusChanges)
				{
					if (statusType == StatusType.KnockbackImmune || statusType == StatusType.Unstoppable)
					{
						return MovementAdjustment.ReducedMovement;
					}
				}
			}
		}
		return base.GetMovementAdjustment();
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		// reactor
		return m_syncComp.m_overchargeBuffs > 0;
		// rogues
		// return m_syncComp.m_overchargeCount > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		return m_syncComp != null
		       // reactor
		       && m_syncComp.m_overchargeBuffs > 1
		       // rogues
		       // && m_syncComp.m_overchargeCount > 1
		       && m_overchargeAbility != null
		       && m_overchargeAbility.GetExtraDamageForMultiCast() > 0
			? m_overchargeAbility.GetExtraDamageForMultiCast()
			: 0;
	}
	
#if SERVER
	// added in rogues
	public GameObject GetCurrentCastSequence(ActorData caster)
	{
		if (AmOvercharged(caster))
		{
			return m_overchargedCastSequencePrefab;
		}
		return m_castSequencePrefab;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp != null && GameFlowData.Get() != null && OverchargeAsFreeActionAfterCast())
		{
			m_syncComp.Networkm_lastUltCastTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(GetCurrentCastSequence(caster), caster.GetCurrentBoardSquare(), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
		list.Add(item);
		if (m_unstoppableSetterSequencePrefab != null && caster.GetActorStatus().IsKnockbackImmune())
		{
			SequenceSource shallowCopy = additionalData.m_sequenceSource.GetShallowCopy();
			shallowCopy.SetWaitForClientEnable(false);
			ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(m_unstoppableSetterSequencePrefab, caster.GetCurrentBoardSquare(), caster.AsArray(), caster, shallowCopy);
			list.Add(item2);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData target in list)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, loSCheckPos));
			int currentModdedDamage = GetCurrentModdedDamage();
			actorHitResults.SetBaseDamage(currentModdedDamage);
			float num = GetKnockbackDistance();
			if (AmOvercharged(caster))
			{
				num += GetExtraKnockbackDistOnOvercharged();
				actorHitResults.AddStandardEffectInfo(GetEnemyEffectOvercharged());
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(GetEnemyEffectNormal());
			}
			if (num > 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(target, caster, m_knockbackType, targets[0].AimDirection, caster.GetFreePos(), num);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if ((m_removeOverchargeEffectOnCast && AmOvercharged(caster)) || GetKnockbackDistanceOnSelf() > 0f)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, loSCheckPos));
			if (m_removeOverchargeEffectOnCast && AmOvercharged(caster))
			{
				m_syncComp.Networkm_overchargeUses -= 1; // custom
				if (m_syncComp.Networkm_overchargeUses <= 0) // custom, unconditional in rogues
				{
					Effect effect = ServerEffectManager.Get().GetEffect(caster, typeof(BlasterOverchargeEffect));
					if (effect != null)
					{
						actorHitResults2.AddEffectForRemoval(effect, ServerEffectManager.Get().GetActorEffects(caster));
					}
				}
			}
			if (GetKnockbackDistanceOnSelf() > 0f)
			{
				KnockbackHitData knockbackData2 = new KnockbackHitData(caster, caster, m_knockbackTypeOnSelf, targets[0].AimDirection, caster.GetFreePos(), GetKnockbackDistanceOnSelf());
				actorHitResults2.AddKnockbackData(knockbackData2);
			}
			abilityResults.StoreActorHit(actorHitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 freePos = targets[0].FreePos;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		float minLength = GetMinLength();
		float maxLength = GetMaxLength();
		float minAngle = GetMinAngle();
		float maxAngle = GetMaxAngle();
		AreaEffectUtils.GatherStretchConeDimensions(freePos, loSCheckPos, minLength, maxLength, minAngle, maxAngle, m_stretchStyle, out var coneLengthRadiusInSquares, out var coneWidthDegrees);
		return AreaEffectUtils.GetActorsInCone(loSCheckPos, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, GetConeBackwardOffset(), PenetrateLineOfSight(), caster, caster.GetOtherTeams(), nonActorTargetInfo);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0 && (AmOvercharged(caster) || m_syncComp.m_lastOverchargeTurn == GameFlowData.Get().CurrentTurn))
		{
			int damageAmountNormal = GetDamageAmountNormal();
			int addAmount = results.BaseDamage - damageAmountNormal;
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BlasterStats.DamageAddedFromOvercharge, addAmount);
		}
	}

	// added in rogues
	public override void OnAbilityAssistedKill(ActorData caster, ActorData target)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.BlasterStats.NumAssistsUsingUlt);
	}
#endif
}
