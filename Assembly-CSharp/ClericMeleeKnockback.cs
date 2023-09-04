using System.Collections.Generic;
using UnityEngine;

public class ClericMeleeKnockback : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;
	public float m_minSeparationBetweenAoeAndCaster = 1f;
	public float m_maxSeparationBetweenAoeAndCaster = 2.5f;
	public float m_aoeRadius = 1.5f;
	public int m_maxTargets = 5;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;
	public float m_knockbackDistance = 1f;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	public StandardEffectInfo m_targetHitEffect;
	[Separator("Connecting Laser between caster and aoe center")]
	public float m_connectLaserWidth;
	public int m_connectLaserDamage = 20;
	public StandardEffectInfo m_connectLaserEnemyHitEffect;
	[Separator("-- Sequences")]
	public GameObject m_castSequencePrefab;
	[Header("-- Anim versions")]
	public float m_rangePercentForLongRangeAnim = 0.5f;

	private Cleric_SyncComponent m_syncComp;
	private AbilityMod_ClericMeleeKnockback m_abilityMod;
	private StandardEffectInfo m_cachedTargetHitEffect;
	private StandardEffectInfo m_cachedConnectLaserEnemyHitEffect;
	private StandardEffectInfo m_cachedSingleTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sphere of Might";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_AoE_Smooth_FixedOffset(
			this,
			GetMinSeparationBetweenAoeAndCaster(),
			GetMaxSeparationBetweenAoeAndCaster(),
			GetAoeRadius(),
			PenetrateLineOfSight(),
			GetKnockbackDistance(),
			GetKnockbackType(),
			GetConnectLaserWidth(),
			true,
			false,
			GetMaxTargets())
		{
			m_customShouldIncludeActorDelegate = ShouldIncludeAoEActor,
			m_delegateIsSquareInLos = IsSquareInLosForCone
		};
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericMeleeKnockback))
		{
			m_abilityMod = abilityMod as AbilityMod_ClericMeleeKnockback;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
		m_cachedConnectLaserEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_connectLaserEnemyHitEffectMod.GetModifiedValue(m_connectLaserEnemyHitEffect)
			: m_connectLaserEnemyHitEffect;
		m_cachedSingleTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_singleTargetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: null;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public float GetMinSeparationBetweenAoeAndCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minSeparationBetweenAoeAndCasterMod.GetModifiedValue(m_minSeparationBetweenAoeAndCaster)
			: m_minSeparationBetweenAoeAndCaster;
	}

	public float GetMaxSeparationBetweenAoeAndCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxSeparationBetweenAoeAndCasterMod.GetModifiedValue(m_maxSeparationBetweenAoeAndCaster)
			: m_maxSeparationBetweenAoeAndCaster;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius)
			: m_aoeRadius;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	public StandardEffectInfo GetSingleTargetHitEffect()
	{
		return m_cachedSingleTargetHitEffect ?? null;
	}

	public int GetExtraTechPointsPerHitWithAreaBuff()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTechPointsPerHitWithAreaBuff.GetModifiedValue(0)
			: 0;
	}

	public float GetConnectLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_connectLaserWidthMod.GetModifiedValue(m_connectLaserWidth)
			: m_connectLaserWidth;
	}

	public int GetConnectLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_connectLaserDamageMod.GetModifiedValue(m_connectLaserDamage)
			: m_connectLaserDamage;
	}

	public StandardEffectInfo GetConnectLaserEnemyHitEffect()
	{
		return m_cachedConnectLaserEnemyHitEffect ?? m_connectLaserEnemyHitEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
		AddTokenInt(tokens, "ConnectLaserDamage", string.Empty, m_connectLaserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_connectLaserEnemyHitEffect, "ConnectLaserEnemyHitEffect", m_connectLaserEnemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		if (GetConnectLaserWidth() > 0f)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_connectLaserDamage);
		}
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor.GetTeam() == ActorData.GetTeam())
		{
			return false;
		}
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			results.m_damage = GetDamageAmount();
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
		{
			results.m_damage = GetConnectLaserDamage();
		}
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int tpGain = base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		AbilityData abilityData = caster.GetAbilityData();
		if (abilityData != null && abilityData.HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			tpGain += Targeters[currentTargeterIndex].GetNumActorsInRange() * GetExtraTechPointsPerHitWithAreaBuff();
		}
		return tpGain;
	}

	public bool IsSquareInLosForCone(BoardSquare testSquare, Vector3 centerPos, ActorData targetingActor)
	{
		if (testSquare == null)
		{
			return false;
		}
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		centerPos.y = losCheckPos.y;
		Vector3 vector = centerPos - losCheckPos;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
			losCheckPos,
			vector.normalized,
			vector.magnitude,
			false,
			targetingActor);
		if (Vector3.Distance(laserEndPoint, centerPos) > 0.1f)
		{
			return false;
		}
		if (!PenetrateLineOfSight())
		{
			Vector3 testPos = testSquare.ToVector3();
			testPos.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 vector3 = testPos - centerPos;
			laserEndPoint = VectorUtils.GetLaserEndPoint(centerPos, vector3.normalized, vector3.magnitude, false, targetingActor);
			if (Vector3.Distance(laserEndPoint, testPos) > 0.1f)
			{
				return false;
			}
		}
		return true;
	}

	public bool ShouldIncludeAoEActor(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		return potentialActor != null
		       && IsSquareInLosForCone(potentialActor.GetCurrentBoardSquare(), centerPos, targetingActor);
	}

    // custom
    private List<ActorData> GetHitActorsLaser(
        ActorData caster,
        Vector3 targetPos,
        Vector3 aimDirection,
        List<NonActorTargetInfo> nonActorTargetInfo,
        out VectorUtils.LaserCoords adjustedCoords)
    {
		float distance = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), targetPos) - GetAoeRadius();
        
        adjustedCoords = default(VectorUtils.LaserCoords);
        adjustedCoords.start = caster.GetLoSCheckPos();
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            adjustedCoords.start,
            aimDirection,
            distance,
            GetConnectLaserWidth(),
            caster,
            caster.GetOtherTeams(),
            PenetrateLineOfSight(),
            GetMaxTargets(),
            false,
            true,
            out adjustedCoords.end,
            nonActorTargetInfo);
        return actorsInLaser;
    }

    // custom
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new ServerClientUtils.SequenceStartData(
            m_sequencePrefab,
			caster.GetCurrentBoardSquare(),
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource);
    }

    // custom
    public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
    {
		// Get enemies in AoE
        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        List<ActorData> actorsInAoE = AreaEffectUtils.GetActorsInRadius(targets[0].FreePos, GetAoeRadius(), false, caster, caster.GetEnemyTeam(), nonActorTargetInfo, true, caster.GetLoSCheckPos());

        foreach (ActorData target in actorsInAoE)
        {
            ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
            ActorHitResults hitResults = new ActorHitResults(GetDamageAmount(), HitActionType.Damage, GetTargetHitEffect(), hitParams);
            abilityResults.StoreActorHit(hitResults);
        }

		// Get enemies in Laser
        Vector3 aimDirection = targets[0].AimDirection;
        List<ActorData> actorsInLaser = GetHitActorsLaser(caster, targets[0].FreePos, aimDirection, nonActorTargetInfo, out var adjustedCoords);

		foreach (ActorData target in actorsInLaser)
		{
            ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
            ActorHitResults hitResults = new ActorHitResults(GetConnectLaserDamage(), HitActionType.Damage, GetConnectLaserEnemyHitEffect(), hitParams);
            abilityResults.StoreActorHit(hitResults);
        }

        abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }
}
