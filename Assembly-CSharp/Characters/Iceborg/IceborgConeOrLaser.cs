using System.Collections.Generic;
using AbilityContextNamespace;

public class IceborgConeOrLaser : GenericAbility_Container
{
	[Separator("Shielding per enemy hit on cast")]
	public int m_shieldPerEnemyHit; // TODO ICEBORG unused in any mods
	public int m_shieldDuration = 1; // TODO ICEBORG unused in any mods
	[Separator("Apply Nova effect?")]
	public bool m_applyDelayedAoeEffect;
	public bool m_skipDelayedAoeEffectIfHasExisting;
	[Separator("Cdr Per Hit Enemy with Nova Core")]
	public int m_cdrPerEnemyWithNovaCore;
	public static ContextNameKeyPair s_cvarHasSlow = new ContextNameKeyPair("HasSlow"); // TODO ICEBORG unused in any mods

	private Iceborg_SyncComponent m_syncComp;
	private AbilityMod_IceborgConeOrLaser m_abilityMod;
	private float m_cachedTargetingRadiusPreview;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor()
		       + ContextVars.GetContextUsageStr( // TODO ICEBORG unused in any mods
			       s_cvarHasSlow.GetName(),
			       "Set on enemies hit, 1 if has Slow, 0 otherwise")
		       + ContextVars.GetContextUsageStr(
			       Iceborg_SyncComponent.s_cvarHasNova.GetName(),
			       "set to 1 if target has nova core on start of turn, 0 otherwise");
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(s_cvarHasSlow.GetName());
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_cachedTargetingRadiusPreview = 0f;
		if (GetTargetSelectComp() is TargetSelect_ConeOrLaser)
		{
			TargetSelect_ConeOrLaser targetSelect_ConeOrLaser = GetTargetSelectComp() as TargetSelect_ConeOrLaser;
			m_cachedTargetingRadiusPreview = targetSelect_ConeOrLaser.m_coneInfo.m_radiusInSquares;
		}
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
		AddTokenInt(tokens, "CdrPerEnemyWithNovaCore", string.Empty, m_cdrPerEnemyWithNovaCore);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}
	
	// TODO ICEBORG unused in any mods
	public int GetShieldPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit)
			: m_shieldPerEnemyHit;
	}

	// TODO ICEBORG unused in any mods
	public int GetShieldDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration)
			: m_shieldDuration;
	}

	public bool ApplyDelayedAoeEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(m_applyDelayedAoeEffect)
			: m_applyDelayedAoeEffect;
	}

	public bool SkipDelayedAoeEffectIfHasExisting()
	{
		return m_abilityMod != null
			? m_abilityMod.m_skipDelayedAoeEffectIfHasExistingMod.GetModifiedValue(m_skipDelayedAoeEffectIfHasExisting)
			: m_skipDelayedAoeEffectIfHasExisting;
	}

	public int GetCdrPerEnemyWithNovaCore()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrPerEnemyWithNovaCoreMod.GetModifiedValue(m_cdrPerEnemyWithNovaCore)
			: m_cdrPerEnemyWithNovaCore;
	}

	public override void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext)
	{
		if (m_syncComp != null)
		{
			m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, ActorData);
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
		SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(
		ActorData targetActor,
		ActorData caster,
		int shieldPerEnemyHit,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit <= 0 || targetActor != caster)
		{
			return;
		}
		int enemiesHit = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> actorHit in actorHitContext)
		{
			if (actorHit.Key.GetTeam() != caster.GetTeam() && actorHit.Value.m_inRangeForTargeter)
			{
				enemiesHit++;
			}
		}

		if (enemiesHit <= 0)
		{
			return;
		}

		int absorb = shieldPerEnemyHit * enemiesHit;
		if (results.m_absorb >= 0)
		{
			results.m_absorb += absorb;
		}
		else
		{
			results.m_absorb = absorb;
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return m_cachedTargetingRadiusPreview > 0f;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_cachedTargetingRadiusPreview;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgConeOrLaser);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

#if SERVER
	// custom
	protected override void PreProcessForCalcAbilityHits(
		List<AbilityTarget> targets,
		ActorData caster,
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext)
	{
		base.PreProcessForCalcAbilityHits(targets, caster, actorHitContextMap, abilityContext);

		foreach (ActorData hitActor in actorHitContextMap.Keys)
		{
			actorHitContextMap[hitActor].m_contextVars.SetValue(
				Iceborg_SyncComponent.s_cvarHasNova.GetKey(),
				m_syncComp.HasNovaCore(hitActor) ? 1 : 0);
		}
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
		AbilityData abilityData = GetComponent<AbilityData>();
		ActorHitResults casterHitResults = null;
		
		int targetsWithNovaCoreNum = 0;
		foreach (ActorHitResults actorHitResult in actorHitResults)
		{
			ActorData hitActor = actorHitResult.m_hitParameters.Target;

			if (hitActor == caster)
			{
				casterHitResults = actorHitResult;
				continue;
			}
			
			bool hasNovaCore = m_syncComp.HasNovaCore(hitActor);

			if (ApplyDelayedAoeEffect() && (!SkipDelayedAoeEffectIfHasExisting() || !hasNovaCore))
			{
				actorHitResult.AddEffect(
					m_syncComp.CreateNovaCoreEffect(
						AsEffectSource(),
						hitActor.GetCurrentBoardSquare(),
						hitActor,
						caster));
			}

			if (hasNovaCore)
			{
				targetsWithNovaCoreNum++;
			}
		}
		
		int cdr = targetsWithNovaCoreNum * GetCdrPerEnemyWithNovaCore();
		if (cdr > 0)
		{
			if (casterHitResults == null) // never happens though
			{
				ActorHitParameters casterHitParams = new ActorHitParameters(caster, caster.GetFreePos());
				casterHitResults = new ActorHitResults(casterHitParams);
			}
			
			casterHitResults.AddMiscHitEvent(
				new MiscHitEventData_AddToCasterCooldown(
					abilityData.GetActionTypeOfAbility(this),
					-1 * cdr));
		}
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.IceborgStats.NumSlowsPlusRootsApplied);
		}
	}
#endif
}
