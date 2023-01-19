using System.Collections.Generic;
using UnityEngine;

public class SenseiHealAoE : Ability
{
	[Separator("Targeting Info")]
	public float m_circleRadius = 3f;
	public bool m_penetrateLoS;
	public bool m_penetrateEnemyBarriers = true;
	[Space(10f)]
	public bool m_includeSelf;
	[Separator("Self Hit")]
	public int m_selfHeal = 20;
	[Space(10f)]
	public float m_selfLowHealthThresh;
	public int m_extraSelfHealForLowHealth;
	[Separator("Ally Hit")]
	public int m_allyHeal = 20;
	public int m_extraAllyHealIfSingleHit;
	[Space(10f)]
	public int m_extraHealForAdjacent;
	public float m_healChangeStartDist;
	public float m_healChangePerDist;
	[Header("-- Extra Ally Heal for low health")]
	public float m_allyLowHealthThresh;
	public int m_extraAllyHealForLowHealth;
	public StandardEffectInfo m_allyHitEffect;
	[Space(10f)]
	public int m_allyEnergyGain;
	[Header("-- Cooldown Reduction for damaging hits")]
	public int m_cdrForAnyDamage;
	public int m_cdrForDamagePerUniqueAbility = 1;
	[Separator("For trigger on Subsequent Turns")]
	public int m_turnsAfterInitialCast;
	public int m_allyHealOnSubsequentTurns;
	public int m_selfHealOnSubsequentTurns;
	public StandardEffectInfo m_allyEffectOnSubsequentTurns;
	[Header("-- Energy gain on subsequent turns")]
	public bool m_ignoreDefaultEnergyOnSubseqTurns = true;
	public int m_energyPerAllyHitOnSubseqTurns;
	public int m_energyOnSelfHitOnSubseqTurns;
	[Header("-- Sequences --")]
	public GameObject m_hitSequencePrefab;
	[Header("    Only used if has heal on subsequent turns")]
	public GameObject m_persistentSeqOnSubsequentTurnsPrefab;

	private AbilityMod_SenseiHealAoE m_abilityMod;
	private AbilityData m_abilityData;
	private SenseiBide m_bideAbility;
	private AbilityData.ActionType m_bideActionType = AbilityData.ActionType.INVALID_ACTION;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedAllyEffectOnSubsequentTurns;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sensei Heal";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_bideAbility = m_abilityData.GetAbilityOfType(typeof(SenseiBide)) as SenseiBide;
			m_bideActionType = m_abilityData.GetActionTypeOfAbility(m_bideAbility);
		}
		AbilityUtil_Targeter_AoE_Smooth targeter = new AbilityUtil_Targeter_AoE_Smooth(
			this,
			GetCircleRadius(),
			PenetrateLoS(),
			false,
			true);
		targeter.SetAffectedGroups(false, true, m_includeSelf);
		targeter.m_adjustPosInConfirmedTargeting = true;
		targeter.m_customCenterPosDelegate = GetCenterPosForTargeter;
		targeter.m_affectCasterDelegate = CanIncludeSelfForTargeter;
		targeter.m_penetrateEnemyBarriers = m_penetrateEnemyBarriers;
		Targeter = targeter;
		Targeter.ShowArcToShape = false;
	}

	private bool CanIncludeSelfForTargeter(ActorData caster, List<ActorData> actorsSoFar)
	{
		return m_includeSelf;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.GetFreePos();
		if (caster.GetActorTargeting() != null && GetRunPriority() > AbilityPriority.Evasion)
		{
			BoardSquare dest = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
			if (dest != null)
			{
				result = dest.ToVector3();
			}
		}
		return result;
	}

	private void SetCachedFields()
	{
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedAllyEffectOnSubsequentTurns = m_abilityMod != null
			? m_abilityMod.m_allyEffectOnSubsequentTurnsMod.GetModifiedValue(m_allyEffectOnSubsequentTurns)
			: m_allyEffectOnSubsequentTurns;
	}

	public float GetCircleRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_circleRadiusMod.GetModifiedValue(m_circleRadius)
			: m_circleRadius;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public bool IncludeSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeSelfMod.GetModifiedValue(m_includeSelf)
			: m_includeSelf;
	}

	public int GetSelfHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealMod.GetModifiedValue(m_selfHeal)
			: m_selfHeal;
	}

	public float GetSelfLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfLowHealthThreshMod.GetModifiedValue(m_selfLowHealthThresh)
			: m_selfLowHealthThresh;
	}

	public int GetExtraSelfHealForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraSelfHealForLowHealthMod.GetModifiedValue(m_extraSelfHealForLowHealth)
			: m_extraSelfHealForLowHealth;
	}

	public int GetAllyHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHeal)
			: m_allyHeal;
	}

	public int GetExtraAllyHealIfSingleHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAllyHealIfSingleHitMod.GetModifiedValue(m_extraAllyHealIfSingleHit)
			: m_extraAllyHealIfSingleHit;
	}

	public int GetExtraHealForAdjacent()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealForAdjacentMod.GetModifiedValue(m_extraHealForAdjacent)
			: m_extraHealForAdjacent;
	}

	public float GetHealChangeStartDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healChangeStartDistMod.GetModifiedValue(m_healChangeStartDist)
			: m_healChangeStartDist;
	}

	public float GetHealChangePerDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healChangePerDistMod.GetModifiedValue(m_healChangePerDist)
			: m_healChangePerDist;
	}

	public float GetAllyLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(m_allyLowHealthThresh)
			: m_allyLowHealthThresh;
	}

	public int GetExtraAllyHealForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAllyHealForLowHealthMod.GetModifiedValue(m_extraAllyHealForLowHealth)
			: m_extraAllyHealForLowHealth;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public int GetAllyEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain)
			: m_allyEnergyGain;
	}

	public int GetCdrForAnyDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrForAnyDamageMod.GetModifiedValue(m_cdrForAnyDamage)
			: m_cdrForAnyDamage;
	}

	public int GetCdrForDamagePerUniqueAbility()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrForDamagePerUniqueAbilityMod.GetModifiedValue(m_cdrForDamagePerUniqueAbility)
			: m_cdrForDamagePerUniqueAbility;
	}

	public int GetTurnsAfterInitialCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_turnsAfterInitialCastMod.GetModifiedValue(m_turnsAfterInitialCast)
			: m_turnsAfterInitialCast;
	}

	public int GetAllyHealOnSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealOnSubsequentTurnsMod.GetModifiedValue(m_allyHealOnSubsequentTurns)
			: m_allyHealOnSubsequentTurns;
	}

	public int GetSelfHealOnSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealOnSubsequentTurnsMod.GetModifiedValue(m_selfHealOnSubsequentTurns)
			: m_selfHealOnSubsequentTurns;
	}

	public StandardEffectInfo GetAllyEffectOnSubsequentTurns()
	{
		return m_cachedAllyEffectOnSubsequentTurns ?? m_allyEffectOnSubsequentTurns;
	}

	public bool IgnoreDefaultEnergyOnSubseqTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreDefaultEnergyOnSubseqTurnsMod.GetModifiedValue(m_ignoreDefaultEnergyOnSubseqTurns)
			: m_ignoreDefaultEnergyOnSubseqTurns;
	}

	public int GetEnergyPerAllyHitOnSubseqTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyPerAllyHitOnSubseqTurnsMod.GetModifiedValue(m_energyPerAllyHitOnSubseqTurns)
			: m_energyPerAllyHitOnSubseqTurns;
	}

	public int GetEnergyOnSelfHitOnSubseqTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyOnSelfHitOnSubseqTurnsMod.GetModifiedValue(m_energyOnSelfHitOnSubseqTurns)
			: m_energyOnSelfHitOnSubseqTurns;
	}

	public int CalcExtraHealFromDist(ActorData targetActor, Vector3 centerPos)
	{
		if (GetExtraHealForAdjacent() > 0)
		{
			Vector3 vector = targetActor.GetFreePos() - centerPos;
			vector.y = 0f;
			float dist = vector.magnitude;
			if (GetHealChangeStartDist() > 0f)
			{
				dist -= GetHealChangeStartDist();
				dist = Mathf.Max(0f, dist / Board.Get().squareSize);
			}
			int healingForDist = Mathf.RoundToInt(GetHealChangePerDist() * dist);
			return Mathf.Max(0, GetExtraHealForAdjacent() + healingForDist);
		}
		return 0;
	}

	public int CalcExtraHealFromBide()
	{
		int num = 0;
		if (m_bideAbility != null
		    && m_bideAbility.GetExtraHealOnHealAoeIfQueued() > 0
		    && m_abilityData.HasQueuedAction(m_bideActionType))
		{
			num += m_bideAbility.GetExtraHealOnHealAoeIfQueued();
		}
		return num;
	}

	public int CalcTotalAllyHeal(ActorData targetActor, Vector3 centerPos, int numAllies)
	{
		int num = GetAllyHeal();
		if (GetExtraAllyHealIfSingleHit() > 0 && numAllies == 1)
		{
			num += GetExtraAllyHealIfSingleHit();
		}
		num += CalcExtraHealFromDist(targetActor, centerPos);
		num += CalcExtraHealFromBide();
		if (GetExtraAllyHealForLowHealth() > 0
		    && GetAllyLowHealthThresh() > 0f
		    && targetActor.GetHitPointPercent() < GetAllyLowHealthThresh())
		{
			num += GetExtraAllyHealForLowHealth();
		}
		return num;
	}

	public int CalcTotalSelfHeal(ActorData caster)
	{
		int selfHeal = GetSelfHeal();
		selfHeal += CalcExtraHealFromBide();
		if (GetExtraSelfHealForLowHealth() > 0
		    && GetSelfLowHealthThresh() > 0f
		    && caster.GetHitPointPercent() < GetSelfLowHealthThresh())
		{
			selfHeal += GetExtraSelfHealForLowHealth();
		}
		return selfHeal;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "SelfHeal", string.Empty, m_selfHeal);
		AddTokenFloatAsPct(tokens, "SelfLowHealthThresh_Pct", string.Empty, m_selfLowHealthThresh);
		AddTokenInt(tokens, "ExtraSelfHealForLowHealth", string.Empty, m_extraSelfHealForLowHealth);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_allyHeal);
		AddTokenInt(tokens, "ExtraAllyHealIfSingleHit", string.Empty, m_extraAllyHealIfSingleHit);
		AddTokenInt(tokens, "ExtraHealForAdjacent", string.Empty, m_extraHealForAdjacent);
		AddTokenFloatAsPct(tokens, "AllyLowHealthThresh_Pct", string.Empty, m_allyLowHealthThresh);
		AddTokenInt(tokens, "ExtraAllyHealForLowHealth", string.Empty, m_extraAllyHealForLowHealth);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "AllyEnergyGain", string.Empty, m_allyEnergyGain);
		AddTokenInt(tokens, "CdrForAnyDamage", string.Empty, m_cdrForAnyDamage);
		AddTokenInt(tokens, "CdrForDamagePerUniqueAbility", string.Empty, m_cdrForDamagePerUniqueAbility);
		AddTokenInt(tokens, "TurnsAfterInitialCast", string.Empty, m_turnsAfterInitialCast);
		AddTokenInt(tokens, "AllyHealOnSubsequentTurns", string.Empty, m_allyHealOnSubsequentTurns);
		AddTokenInt(tokens, "SelfHealOnSubsequentTurns", string.Empty, m_selfHealOnSubsequentTurns);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyEffectOnSubsequentTurns, "AllyEffectOnSubsequentTurns", m_allyEffectOnSubsequentTurns);
		AddTokenInt(tokens, "EnergyPerAllyHitOnSubseqTurns", string.Empty, m_energyPerAllyHitOnSubseqTurns);
		AddTokenInt(tokens, "EnergyOnSelfHitOnSubseqTurns", string.Empty, m_energyOnSelfHitOnSubseqTurns);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, GetAllyHeal());
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Ally);
		if (IncludeSelf())
		{
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetSelfHeal());
			m_allyHitEffect.ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		}
		AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Ally, GetAllyEnergyGain());
		return number;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		bool hasAllies = Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0;
		bool hasSelf = targetActor == ActorData;
		if (!hasAllies && !hasSelf)
		{
			return true;
		}
		int healing = 0;
		if (hasAllies)
		{
			int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (Targeter is AbilityUtil_Targeter_AoE_Smooth)
			{
				AbilityUtil_Targeter_AoE_Smooth targeter = Targeter as AbilityUtil_Targeter_AoE_Smooth;
				healing = CalcTotalAllyHeal(targetActor, targeter.m_lastUpdatedCenterPos, allyNum);
			}
		}
		else if (hasSelf)
		{
			healing = CalcTotalSelfHeal(ActorData);
		}
		results.m_healing = healing;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiHealAoE))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiHealAoE;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
