using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class DinoMarkedAreaAttack : GenericAbility_Container
{
	private const string c_inCenter = "InCenter";
	public static ContextNameKeyPair s_cvarInCenter = new ContextNameKeyPair(c_inCenter);

	[Separator("For Delayed Hit")]
	public int m_delayTurns = 1;
	public AbilityAreaShape m_shape;
	public bool m_delayedHitIgnoreLos;
	public int m_extraDamageForSingleMark;
	public int m_energyToAllyOnDamageHit;
	[Separator("On Hit Data for delayed hits", "yellow")]
	public OnHitAuthoredData m_delayedOnHitData;
	[Separator("Sequences for delayed hits")]
	public GameObject m_firstTurnMarkerSeqPrefab;
	public GameObject m_markerSeqPrefab;
	public GameObject m_triggerSeqPrefab;

	private AbilityMod_DinoMarkedAreaAttack m_abilityMod;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor()
		       + ContextVars.GetContextUsageStr(
			       c_inCenter,
			       "value set to 1 if delayed hit actor is in center of a shape, not set explicitly otherwise");
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(c_inCenter);
		return contextNamesForEditor;
	}

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Delayed Hits --\n" + m_delayedOnHitData.GetInEditorDesc();
	}

	public override void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext)
	{
		if (ActorData.GetTeam() != targetActor.GetTeam()
		    && actorHitContext.TryGetValue(targetActor, out ActorHitContext hitContext))
		{
			hitContext.m_contextVars.SetValue(s_cvarInCenter.GetKey(), 1);
		}
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (!actorHitContext.ContainsKey(targetActor)
		    || targetActor.GetTeam() == caster.GetTeam())
		{
			return;
		}
		ActorHitContext actorContext = actorHitContext[targetActor];
		CalcIntFieldValues(
			targetActor,
			caster,
			actorContext,
			abilityContext,
			m_delayedOnHitData.m_enemyHitIntFields,
			m_calculatedValuesForTargeter);
		results.m_damage = m_calculatedValuesForTargeter.m_damage;
		if (GetExtraDamageForSingleMark() <= 0)
		{
			return;
		}

		int enemiesHit = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> hitActor in actorHitContext)
		{
			if (hitActor.Value.m_inRangeForTargeter && hitActor.Key.GetTeam() != caster.GetTeam())
			{
				enemiesHit++;
			}
		}

		if (enemiesHit == 1)
		{
			results.m_damage += GetExtraDamageForSingleMark();
		}
	}

	public int GetDelayedHitDamage()
	{
		int damage = 0;
		foreach (OnHitIntField field in m_delayedOnHitData.m_enemyHitIntFields)
		{
			if (field.m_hitType == OnHitIntField.HitType.Damage)
			{
				damage += field.m_baseValue;
			}
		}
		return damage;
	}

	public int GetDelayTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_delayTurnsMod.GetModifiedValue(m_delayTurns)
			: m_delayTurns;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool DelayedHitIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_delayedHitIgnoreLosMod.GetModifiedValue(m_delayedHitIgnoreLos)
			: m_delayedHitIgnoreLos;
	}

	public int GetExtraDamageForSingleMark()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForSingleMarkMod.GetModifiedValue(m_extraDamageForSingleMark)
			: m_extraDamageForSingleMark;
	}

	public int GetEnergyToAllyOnDamageHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyToAllyOnDamageHitMod.GetModifiedValue(m_energyToAllyOnDamageHit)
			: m_energyToAllyOnDamageHit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_delayedOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "DelayTurns", string.Empty, m_delayTurns);
		AddTokenInt(tokens, "ExtraDamageForSingleMark", string.Empty, m_extraDamageForSingleMark);
		AddTokenInt(tokens, "EnergyToAllyOnDamageHit", string.Empty, m_energyToAllyOnDamageHit);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_DinoMarkedAreaAttack;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
