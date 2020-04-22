using System.Collections.Generic;
using UnityEngine;

public class SenseiHealAoE : Ability
{
	[Separator("Targeting Info", true)]
	public float m_circleRadius = 3f;

	public bool m_penetrateLoS;

	public bool m_penetrateEnemyBarriers = true;

	[Space(10f)]
	public bool m_includeSelf;

	[Separator("Self Hit", true)]
	public int m_selfHeal = 20;

	[Space(10f)]
	public float m_selfLowHealthThresh;

	public int m_extraSelfHealForLowHealth;

	[Separator("Ally Hit", true)]
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

	[Separator("For trigger on Subsequent Turns", true)]
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
			m_bideAbility = (m_abilityData.GetAbilityOfType(typeof(SenseiBide)) as SenseiBide);
			m_bideActionType = m_abilityData.GetActionTypeOfAbility(m_bideAbility);
		}
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, GetCircleRadius(), PenetrateLoS(), false, true);
		abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(false, true, m_includeSelf);
		abilityUtil_Targeter_AoE_Smooth.m_adjustPosInConfirmedTargeting = true;
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = GetCenterPosForTargeter;
		abilityUtil_Targeter_AoE_Smooth.m_affectCasterDelegate = CanIncludeSelfForTargeter;
		abilityUtil_Targeter_AoE_Smooth.m_penetrateEnemyBarriers = m_penetrateEnemyBarriers;
		base.Targeter = abilityUtil_Targeter_AoE_Smooth;
		base.Targeter.ShowArcToShape = false;
	}

	private bool CanIncludeSelfForTargeter(ActorData caster, List<ActorData> actorsSoFar)
	{
		return m_includeSelf;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.GetTravelBoardSquareWorldPosition();
		if (caster.GetActorTargeting() != null && GetRunPriority() > AbilityPriority.Evasion)
		{
			BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
			if (evadeDestinationForTargeter != null)
			{
				result = evadeDestinationForTargeter.ToVector3();
			}
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedAllyEffectOnSubsequentTurns;
		if ((bool)m_abilityMod)
		{
			cachedAllyEffectOnSubsequentTurns = m_abilityMod.m_allyEffectOnSubsequentTurnsMod.GetModifiedValue(m_allyEffectOnSubsequentTurns);
		}
		else
		{
			cachedAllyEffectOnSubsequentTurns = m_allyEffectOnSubsequentTurns;
		}
		m_cachedAllyEffectOnSubsequentTurns = cachedAllyEffectOnSubsequentTurns;
	}

	public float GetCircleRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_circleRadiusMod.GetModifiedValue(m_circleRadius);
		}
		else
		{
			result = m_circleRadius;
		}
		return result;
	}

	public bool PenetrateLoS()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
		}
		else
		{
			result = m_penetrateLoS;
		}
		return result;
	}

	public bool IncludeSelf()
	{
		return (!m_abilityMod) ? m_includeSelf : m_abilityMod.m_includeSelfMod.GetModifiedValue(m_includeSelf);
	}

	public int GetSelfHeal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealMod.GetModifiedValue(m_selfHeal);
		}
		else
		{
			result = m_selfHeal;
		}
		return result;
	}

	public float GetSelfLowHealthThresh()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfLowHealthThreshMod.GetModifiedValue(m_selfLowHealthThresh);
		}
		else
		{
			result = m_selfLowHealthThresh;
		}
		return result;
	}

	public int GetExtraSelfHealForLowHealth()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraSelfHealForLowHealthMod.GetModifiedValue(m_extraSelfHealForLowHealth);
		}
		else
		{
			result = m_extraSelfHealForLowHealth;
		}
		return result;
	}

	public int GetAllyHeal()
	{
		return (!m_abilityMod) ? m_allyHeal : m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHeal);
	}

	public int GetExtraAllyHealIfSingleHit()
	{
		return (!m_abilityMod) ? m_extraAllyHealIfSingleHit : m_abilityMod.m_extraAllyHealIfSingleHitMod.GetModifiedValue(m_extraAllyHealIfSingleHit);
	}

	public int GetExtraHealForAdjacent()
	{
		return (!m_abilityMod) ? m_extraHealForAdjacent : m_abilityMod.m_extraHealForAdjacentMod.GetModifiedValue(m_extraHealForAdjacent);
	}

	public float GetHealChangeStartDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healChangeStartDistMod.GetModifiedValue(m_healChangeStartDist);
		}
		else
		{
			result = m_healChangeStartDist;
		}
		return result;
	}

	public float GetHealChangePerDist()
	{
		return (!m_abilityMod) ? m_healChangePerDist : m_abilityMod.m_healChangePerDistMod.GetModifiedValue(m_healChangePerDist);
	}

	public float GetAllyLowHealthThresh()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(m_allyLowHealthThresh);
		}
		else
		{
			result = m_allyLowHealthThresh;
		}
		return result;
	}

	public int GetExtraAllyHealForLowHealth()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraAllyHealForLowHealthMod.GetModifiedValue(m_extraAllyHealForLowHealth);
		}
		else
		{
			result = m_extraAllyHealForLowHealth;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public int GetAllyEnergyGain()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain);
		}
		else
		{
			result = m_allyEnergyGain;
		}
		return result;
	}

	public int GetCdrForAnyDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrForAnyDamageMod.GetModifiedValue(m_cdrForAnyDamage);
		}
		else
		{
			result = m_cdrForAnyDamage;
		}
		return result;
	}

	public int GetCdrForDamagePerUniqueAbility()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrForDamagePerUniqueAbilityMod.GetModifiedValue(m_cdrForDamagePerUniqueAbility);
		}
		else
		{
			result = m_cdrForDamagePerUniqueAbility;
		}
		return result;
	}

	public int GetTurnsAfterInitialCast()
	{
		return (!m_abilityMod) ? m_turnsAfterInitialCast : m_abilityMod.m_turnsAfterInitialCastMod.GetModifiedValue(m_turnsAfterInitialCast);
	}

	public int GetAllyHealOnSubsequentTurns()
	{
		return (!m_abilityMod) ? m_allyHealOnSubsequentTurns : m_abilityMod.m_allyHealOnSubsequentTurnsMod.GetModifiedValue(m_allyHealOnSubsequentTurns);
	}

	public int GetSelfHealOnSubsequentTurns()
	{
		return (!m_abilityMod) ? m_selfHealOnSubsequentTurns : m_abilityMod.m_selfHealOnSubsequentTurnsMod.GetModifiedValue(m_selfHealOnSubsequentTurns);
	}

	public StandardEffectInfo GetAllyEffectOnSubsequentTurns()
	{
		StandardEffectInfo result;
		if (m_cachedAllyEffectOnSubsequentTurns != null)
		{
			result = m_cachedAllyEffectOnSubsequentTurns;
		}
		else
		{
			result = m_allyEffectOnSubsequentTurns;
		}
		return result;
	}

	public bool IgnoreDefaultEnergyOnSubseqTurns()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_ignoreDefaultEnergyOnSubseqTurnsMod.GetModifiedValue(m_ignoreDefaultEnergyOnSubseqTurns);
		}
		else
		{
			result = m_ignoreDefaultEnergyOnSubseqTurns;
		}
		return result;
	}

	public int GetEnergyPerAllyHitOnSubseqTurns()
	{
		return (!m_abilityMod) ? m_energyPerAllyHitOnSubseqTurns : m_abilityMod.m_energyPerAllyHitOnSubseqTurnsMod.GetModifiedValue(m_energyPerAllyHitOnSubseqTurns);
	}

	public int GetEnergyOnSelfHitOnSubseqTurns()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyOnSelfHitOnSubseqTurnsMod.GetModifiedValue(m_energyOnSelfHitOnSubseqTurns);
		}
		else
		{
			result = m_energyOnSelfHitOnSubseqTurns;
		}
		return result;
	}

	public int CalcExtraHealFromDist(ActorData targetActor, Vector3 centerPos)
	{
		if (GetExtraHealForAdjacent() > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - centerPos;
					vector.y = 0f;
					float num = vector.magnitude;
					if (GetHealChangeStartDist() > 0f)
					{
						num -= GetHealChangeStartDist();
						num = Mathf.Max(0f, num / Board.Get().squareSize);
					}
					int num2 = Mathf.RoundToInt(GetHealChangePerDist() * num);
					return Mathf.Max(0, GetExtraHealForAdjacent() + num2);
				}
				}
			}
		}
		return 0;
	}

	public int CalcExtraHealFromBide()
	{
		int num = 0;
		if (m_bideAbility != null && m_bideAbility.GetExtraHealOnHealAoeIfQueued() > 0)
		{
			if (m_abilityData.HasQueuedAction(m_bideActionType))
			{
				num += m_bideAbility.GetExtraHealOnHealAoeIfQueued();
			}
		}
		return num;
	}

	public int CalcTotalAllyHeal(ActorData targetActor, Vector3 centerPos, int numAllies)
	{
		int num = GetAllyHeal();
		if (GetExtraAllyHealIfSingleHit() > 0)
		{
			if (numAllies == 1)
			{
				num += GetExtraAllyHealIfSingleHit();
			}
		}
		num += CalcExtraHealFromDist(targetActor, centerPos);
		num += CalcExtraHealFromBide();
		if (GetExtraAllyHealForLowHealth() > 0)
		{
			if (GetAllyLowHealthThresh() > 0f)
			{
				if (targetActor.GetHitPointShareOfMax() < GetAllyLowHealthThresh())
				{
					num += GetExtraAllyHealForLowHealth();
				}
			}
		}
		return num;
	}

	public int CalcTotalSelfHeal(ActorData caster)
	{
		int selfHeal = GetSelfHeal();
		selfHeal += CalcExtraHealFromBide();
		if (GetExtraSelfHealForLowHealth() > 0)
		{
			if (GetSelfLowHealthThresh() > 0f)
			{
				if (caster.GetHitPointShareOfMax() < GetSelfLowHealthThresh())
				{
					selfHeal += GetExtraSelfHealForLowHealth();
				}
			}
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
		bool flag = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0;
		bool flag2 = targetActor == base.ActorData;
		if (!flag)
		{
			if (!flag2)
			{
				goto IL_00c9;
			}
		}
		int healing = 0;
		if (flag)
		{
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (base.Targeter is AbilityUtil_Targeter_AoE_Smooth)
			{
				AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = base.Targeter as AbilityUtil_Targeter_AoE_Smooth;
				healing = CalcTotalAllyHeal(targetActor, abilityUtil_Targeter_AoE_Smooth.m_lastUpdatedCenterPos, visibleActorsCountByTooltipSubject);
			}
		}
		else if (flag2)
		{
			healing = CalcTotalSelfHeal(base.ActorData);
		}
		results.m_healing = healing;
		goto IL_00c9;
		IL_00c9:
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SenseiHealAoE))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SenseiHealAoE);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
