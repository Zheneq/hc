using System.Collections.Generic;
using AbilityContextNamespace;

public class ScampDualLasers : GenericAbility_Container
{
	[Separator("Target Select Component for when shield is down")]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;
	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;
	[Separator("Extra Damage and Aoe Radius for turn after losing suit (for AoE only)")]
	public int m_extraDamageTurnAfterLosingSuit;
	public float m_extraAoeRadiusTurnAfterLosingSuit;

	private AbilityMod_ScampDualLasers m_abilityMod;
	private Scamp_SyncComponent m_syncComp;
	private OnHitAuthoredData m_cachedShieldDownOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		m_cachedShieldDownOnHitData = m_abilityMod != null ? m_abilityMod.m_shieldDownOnHitDataMod.GetModdedOnHitData(m_shieldDownOnHitData) : m_shieldDownOnHitData;
		base.SetupTargetersAndCachedVars();
		if (Targeter is AbilityUtil_Targeter_ScampDualLasers targeter)
		{
			targeter.m_delegateLaserCount = GetNumLasers;
			targeter.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
		if (m_targetSelectComp is TargetSelect_DualMeetingLasers targetSelectComp)
		{
			targetSelectComp.m_delegateLaserCount = GetNumLasers;
			targetSelectComp.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
		if (m_shieldDownTargetSelect is TargetSelect_DualMeetingLasers shieldDownTargetSelect)
		{
			shieldDownTargetSelect.m_delegateLaserCount = GetNumLasers;
			shieldDownTargetSelect.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		Targeters.AddRange(!hasShield && m_shieldDownTargetSelect != null
			? m_shieldDownTargetSelect.CreateTargeters(this)
			: m_targetSelectComp.CreateTargeters(this));
		if (Targeter is AbilityUtil_Targeter_ScampDualLasers targeter)
		{
			targeter.m_delegateLaserCount = GetNumLasers;
			targeter.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
	}

	public int GetNumLasers(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return IsInSuit() ? 2 : 1;
	}

	public float GetExtraAoeRadius(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius)
	{
		return GetExtraAoeRadiusTurnAfterLosingSuit() > 0f && IsTurnAfterLostSuit()
			? GetExtraAoeRadiusTurnAfterLosingSuit()
			: 0f;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return IsInSuit()
			? base.GetTargetSelectComp()
			: m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		return IsInSuit()
			? base.GetOnHitAuthoredData()
			: m_cachedShieldDownOnHitData ?? m_shieldDownOnHitData;
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (IsTurnAfterLostSuit() && GetExtraDamageTurnAfterLosingSuit() > 0)
		{
			ActorHitContext ctx = actorHitContext[targetActor];
			int hash = ContextKeys.s_InAoe.GetKey();
			if (ctx.m_contextVars.HasVarInt(hash) &&
			    ctx.m_contextVars.GetValueInt(hash) > 0)
			{
				results.m_damage += GetExtraDamageTurnAfterLosingSuit();
			}
		}
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public bool IsTurnAfterLostSuit()
	{
		return m_syncComp != null
		       && m_syncComp.m_lastSuitLostTurn != 0
		       && GameFlowData.Get().CurrentTurn - m_syncComp.m_lastSuitLostTurn == 1;
	}

	public int GetExtraDamageTurnAfterLosingSuit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageTurnAfterLosingSuitMod.GetModifiedValue(m_extraDamageTurnAfterLosingSuit)
			: m_extraDamageTurnAfterLosingSuit;
	}

	public float GetExtraAoeRadiusTurnAfterLosingSuit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAoeRadiusTurnAfterLosingSuitMod.GetModifiedValue(m_extraAoeRadiusTurnAfterLosingSuit)
			: m_extraAoeRadiusTurnAfterLosingSuit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_shieldDownOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ExtraDamageTurnAfterLosingSuit", string.Empty, m_extraDamageTurnAfterLosingSuit);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_ScampDualLasers;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_defaultTargetSelectMod);
			m_shieldDownTargetSelect.SetTargetSelectMod(m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
			m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
	
#if SERVER
	protected override void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		base.ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);
		
		if (IsTurnAfterLostSuit() && GetExtraDamageTurnAfterLosingSuit() > 0)
		{
			Dictionary<ActorData, ActorHitContext> actorHitContextMap = GetTargetSelectComp().GetActorHitContextMap();
			
			foreach (ActorHitResults actorHitResult in actorHitResults)
			{
				ActorData hitActor = actorHitResult.m_hitParameters.Target;
				ActorHitContext ctx = actorHitContextMap[hitActor];
				int hash = ContextKeys.s_InAoe.GetKey();
				if (hitActor.GetTeam() != caster.GetTeam()
				    && actorHitContextMap[hitActor].m_contextVars.HasVarInt(hash)
				    && ctx.m_contextVars.GetValueInt(hash) > 0)
				{
					actorHitResult.AddBaseDamage(GetExtraDamageTurnAfterLosingSuit());
				}
			}
		}
	}
#endif
}
