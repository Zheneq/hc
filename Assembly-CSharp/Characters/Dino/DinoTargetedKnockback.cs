// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class DinoTargetedKnockback : GenericAbility_Container
{
	public int m_extraDamageIfFullPowerLayerCone;
	[Separator("Shield per enemy hit")]
	public int m_shieldPerEnemyHit;
	public int m_shieldDuration = 2;
	[Separator("For hits around knockback destinations")]
	public bool m_doHitsAroundKnockbackDest;
	public AbilityAreaShape m_hitsAroundKnockbackDestShape = AbilityAreaShape.Three_x_Three;
	[Separator("On Hit Data for Knockback Destination Hit", "yellow")]
	public OnHitAuthoredData m_knockbackDestOnHitData;
	[Separator("Sequences for hit around knockback destination")]
	public GameObject m_onKnockbackDestHitSeqPrefab;

	private OnHitAuthoredData m_cachedKnockbackDestOnHitData;
	private AbilityMod_DinoTargetedKnockback m_abilityMod;
	private DinoLayerCones m_layerConeAbility;

	protected override void SetupTargetersAndCachedVars()
	{
		base.SetupTargetersAndCachedVars();
		m_layerConeAbility = GetAbilityOfType<DinoLayerCones>();
		m_cachedKnockbackDestOnHitData = m_abilityMod != null
			? m_abilityMod.m_knockbackDestOnHitDataMod.GetModdedOnHitData(m_knockbackDestOnHitData)
			: m_knockbackDestOnHitData;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public int GetExtraDamageIfFullPowerLayerCone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfFullPowerLayerConeMod.GetModifiedValue(m_extraDamageIfFullPowerLayerCone)
			: m_extraDamageIfFullPowerLayerCone;
	}

	public int GetShieldPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit)
			: m_shieldPerEnemyHit;
	}

	public int GetShieldDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration)
			: m_shieldDuration;
	}

	public bool DoHitsAroundKnockbackDest()
	{
		return m_abilityMod != null
			? m_abilityMod.m_doHitsAroundKnockbackDestMod.GetModifiedValue(m_doHitsAroundKnockbackDest)
			: m_doHitsAroundKnockbackDest;
	}

	public AbilityAreaShape GetHitsAroundKnockbackDestShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitsAroundKnockbackDestShapeMod.GetModifiedValue(m_hitsAroundKnockbackDestShape)
			: m_hitsAroundKnockbackDestShape;
	}

	public OnHitAuthoredData GetKnockbackDestOnHitData()
	{
		return m_cachedKnockbackDestOnHitData ?? m_knockbackDestOnHitData;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ExtraDamageIfFullPowerLayerCone", string.Empty, m_extraDamageIfFullPowerLayerCone);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (targetActor == caster && GetShieldPerEnemyHit() > 0)
		{
			int enemiesHit = 0;
			foreach (KeyValuePair<ActorData, ActorHitContext> hitActor in actorHitContext)
			{
				if (hitActor.Value.m_inRangeForTargeter && hitActor.Key.GetTeam() != caster.GetTeam())
				{
					enemiesHit++;
				}
			}
			if (enemiesHit > 0)
			{
				if (results.m_absorb > 0)
				{
					results.m_absorb += enemiesHit * GetShieldPerEnemyHit();
				}
				else
				{
					results.m_absorb = enemiesHit * GetShieldPerEnemyHit();
				}
			}
		}

		if (results.m_damage > 0
		    && m_layerConeAbility != null
		    && GetExtraDamageIfFullPowerLayerCone() > 0
		    && m_layerConeAbility.IsAtMaxPowerLevel())
		{
			results.m_damage += GetExtraDamageIfFullPowerLayerCone();
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_DinoTargetedKnockback;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
	
#if SERVER
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

		BoardSquare targetSquare = Board.Get().GetSquare(targets[1].GridPos);
		int enemiesHit = 0;
		foreach (ActorHitResults actorHitResult in actorHitResults)
		{
			ActorData hitActor = actorHitResult.m_hitParameters.Target;

			if (hitActor.GetTeam() == caster.GetTeam())
			{
				continue;
			}

			enemiesHit++;

			actorHitResult.AddKnockbackData(new KnockbackHitData(
				hitActor,
				caster,
				KnockbackType.PullToSource,
				targets[0].AimDirection,
				targetSquare.ToVector3(),
				0));
			
			if (actorHitResult.BaseDamage > 0
			    && m_layerConeAbility != null
			    && GetExtraDamageIfFullPowerLayerCone() > 0
			    && m_layerConeAbility.IsAtMaxPowerLevel())
			{
				actorHitResult.AddBaseDamage(GetExtraDamageIfFullPowerLayerCone());
			}
		}
		
		ActorHitResults casterHitResults = GetOrAddHitResults(caster, actorHitResults);
		casterHitResults.AddEffect(CreateShieldEffect(
			this, caster, GetShieldPerEnemyHit() * enemiesHit, GetShieldDuration()));
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.DinoStats.KnockbackDamageOnCastAndKnockback,
				results.FinalDamage);
		}
	}
#endif
}
