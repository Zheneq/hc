// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
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
	
#if SERVER
	// custom
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list =  base.GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
		foreach (var actorAndHitResult in additionalData.m_abilityResults.m_actorToHitResults)
		{
			ActorData hitActor = actorAndHitResult.Key;
			if (hitActor.GetTeam() == caster.GetTeam())
			{
				continue;
			}
			
			list.Add(new ServerClientUtils.SequenceStartData(
				m_firstTurnMarkerSeqPrefab,
				hitActor.GetCurrentBoardSquare(),
				hitActor.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		
		return list;
	}

	// custom
	protected override void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		base.ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);

		AbilityTarget target = targets[0];

		List<ActorData> hitEnemies = actorHitResults
			.Select(acr => acr.m_hitParameters.Target)
			.Where(actor => actor.GetTeam() != caster.GetTeam())
			.ToList();
		if (hitEnemies.Count > 0)
		{
			ActorHitResults casterHitResult = GetOrAddHitResults(caster, actorHitResults);
			casterHitResult.AddEffect(
				new DinoMarkedAreaEffect(
					AsEffectSource(),
					Board.Get().GetSquare(target.GridPos),
					caster,
					hitEnemies,
					GetDelayTurns(),
					GetShape(),
					DelayedHitIgnoreLos(),
					hitEnemies.Count == 1 ? GetExtraDamageForSingleMark() : 0,
					m_delayedOnHitData,
					m_markerSeqPrefab,
					m_triggerSeqPrefab));
		}
		
        if (GetEnergyToAllyOnDamageHit() > 0)
        {
            foreach (ActorHitResults actorHitResult in actorHitResults)
            {
                ActorData hitActor = actorHitResult.m_hitParameters.Target;

                if (hitActor.GetTeam() == caster.GetTeam())
                {
                    continue;
                }

                actorHitResult.AddEffect(
                    new DinoMarkedAreaReactEffect(
                        AsEffectSource(),
                        hitActor.GetCurrentBoardSquare(),
                        hitActor,
                        caster,
                        GetEnergyToAllyOnDamageHit()));
            }
        }
    }
	
	// custom
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.DinoStats.MarkedAreaAttackDamage,
				results.FinalDamage);
		}
	}
#endif
}