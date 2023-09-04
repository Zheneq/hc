using System.Collections.Generic;
using UnityEngine;

public class ClericBasicAttack : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;
	public float m_coneAngle = 180f;
	public float m_coneLengthInner = 1.5f;
	public float m_coneLength = 2.5f;
	public float m_coneBackwardOffset;
	public int m_maxTargets = 1;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmountInner = 28;
	public int m_damageAmount = 20;
	public StandardEffectInfo m_targetHitEffectInner;
	public StandardEffectInfo m_targetHitEffect;
	public AbilityModCooldownReduction m_cooldownReduction;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private Cleric_SyncComponent m_syncComp;
	private AbilityMod_ClericBasicAttack m_abilityMod;
	private StandardEffectInfo m_cachedTargetHitEffectInner;
	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cleric Bash";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		float coneAngle = GetConeAngle();
		Targeter = new AbilityUtil_Targeter_MultipleCones(
			this,
			new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
			{
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, GetConeLengthInner()),
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, GetConeLength())
			},
			m_coneBackwardOffset,
			PenetrateLineOfSight(),
			true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_ClericBasicAttack;
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
		m_cachedTargetHitEffectInner = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectInnerMod.GetModifiedValue(m_targetHitEffectInner)
			: m_targetHitEffectInner;
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public float GetConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneAngle)
			: m_coneAngle;
	}

	public float GetConeLengthInner()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(m_coneLengthInner)
			: m_coneLengthInner;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageAmountInner()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(m_damageAmountInner)
			: m_damageAmountInner;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetTargetHitEffectInner()
	{
		return m_cachedTargetHitEffectInner ?? m_targetHitEffectInner;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	public int GetExtraDamageToTargetsWhoEvaded()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageToTargetsWhoEvaded.GetModifiedValue(0)
			: 0;
	}

	public AbilityModCooldownReduction GetCooldownReduction()
	{
		return m_abilityMod != null && m_abilityMod.m_useCooldownReductionOverride
			? m_abilityMod.m_cooldownReductionOverrideMod
			: m_cooldownReduction;
	}

	public int GetHitsToIgnoreForCooldownReductionMultiplier()
	{
		return m_abilityMod != null && m_abilityMod.m_useCooldownReductionOverride
			? m_abilityMod.m_hitsToIgnoreForCooldownReductionMultiplier.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraTechPointGainInAreaBuff()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0)
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffectInner, "TargetHitEffectInner", m_targetHitEffectInner);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, GetDamageAmountInner()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, GetDamageAmount())
		};
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float radius = GetConeLengthInner() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetFreePos() - damageOrigin;
		vector.y = 0f;
		float dist = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			dist -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		return dist <= radius;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near && subjectType != AbilityTooltipSubject.Far)
		{
			return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
		}

		return InsideNearRadius(targetActor, damageOrigin)
			? subjectType == AbilityTooltipSubject.Near
			: subjectType == AbilityTooltipSubject.Far;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff))
			? GetExtraTechPointGainInAreaBuff() * Targeter.GetNumActorsInRange()
			: base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
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
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 aimDirection = targets[0].AimDirection;

        Vector3 loSCheckPos = caster.GetLoSCheckPos();
        float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
        List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(loSCheckPos,coneCenterAngleDegrees,GetConeAngle(),GetConeLength(),m_coneBackwardOffset,m_penetrateLineOfSight,caster,caster.GetOtherTeams(),nonActorTargetInfo);
		foreach(ActorData actor in actorsInCone)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actor, caster.GetLoSCheckPos());
			ActorHitResults hitResults = new ActorHitResults(GetDamageAmount(), HitActionType.Damage, GetTargetHitEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}

		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }
}
