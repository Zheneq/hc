// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

// missing in rogues
public class DinoLayerCones : GenericAbility_Container
{
	[Separator("Powering Up Params")]
	public int m_powerupPauseTurnsAfterCast = 1; // TODO DINO unused (hardcoded)
	public int m_initialPowerupStartDelay = 1; // TODO DINO unused (hardcoded)
	[Header("-- Power level if no inner hits")]
	public int m_powerLevelAdjustIfNoInnerHits;

	private Dino_SyncComponent m_syncComp;
	private AbilityMod_DinoLayerCones m_abilityMod;
	private AbilityData m_abilityData;
	private DinoDashOrShield m_dashOrShieldAbility;
	private AbilityData.ActionType m_dashOrShieldActionType = AbilityData.ActionType.INVALID_ACTION;
	
#if SERVER
	// custom
	private Passive_Dino m_passive;
#endif
	
	protected override void SetupTargetersAndCachedVars()
	{
#if SERVER
		// custom
		m_passive = GetPassiveOfType(typeof(Passive_Dino)) as Passive_Dino;
#endif
		
		m_syncComp = GetComponent<Dino_SyncComponent>();
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_dashOrShieldAbility = m_abilityData.GetAbilityOfType<DinoDashOrShield>();
			if (m_dashOrShieldAbility != null)
			{
				m_dashOrShieldActionType = m_abilityData.GetActionTypeOfAbility(m_dashOrShieldAbility);
			}
		}
		base.SetupTargetersAndCachedVars();
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null && targetSelectComp is TargetSelect_LayerCones targeter1)
		{
			targeter1.m_delegateNumActiveLayers = GetNumLayersActive;
		}

		if (Targeter is AbilityUtil_Targeter_LayerCones targeter2)
		{
			targeter2.m_delegateNumActiveLayers = GetNumLayersActive;
		}
	}

	public int GetPowerLevelAdjustIfNoInnerHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_powerLevelAdjustIfNoInnerHitsMod.GetModifiedValue(m_powerLevelAdjustIfNoInnerHits)
			: m_powerLevelAdjustIfNoInnerHits;
	}

	public int GetLayerCount()
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		return targetSelectComp != null && targetSelectComp is TargetSelect_LayerCones cones
			? cones.GetLayerCount()
			: 1;
	}

	public int GetNumLayersActive(int maxLayers)
	{
		if (m_syncComp == null)
		{
			return 0;
		}

		return m_dashOrShieldAbility != null
		       && m_dashOrShieldAbility.FullyChargeUpLayerCone()
		       && !m_dashOrShieldAbility.IsInReadyStance()
		       && m_abilityData.HasQueuedAction(m_dashOrShieldActionType)
			? maxLayers
			: Mathf.Min(maxLayers, m_syncComp.m_layerConePowerLevel + 1);
	}

	public bool IsAtMaxPowerLevel()
	{
		return GetNumLayersActive(GetLayerCount()) >= GetLayerCount();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "PowerLevelAdjustIfNoInnerHits", string.Empty, m_powerLevelAdjustIfNoInnerHits);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_DinoLayerCones;
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

		ActorHitResults casterHitResults = actorHitResults
			.FirstOrDefault(ahr => ahr.m_hitParameters.Target == caster);
		if (casterHitResults == null)
		{
			casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetLoSCheckPos()));
			actorHitResults.Add(casterHitResults);
		}

		int nextTurnPowerLevel = 0;
		if (GetPowerLevelAdjustIfNoInnerHits() > 0)
		{
			bool noInnerHits = GetTargetSelectComp().GetActorHitContextMap().Values
				.All(ahc => ahc.m_contextVars.GetValueInt(ContextKeys.s_Layer.GetKey()) > 0);
			if (noInnerHits)
			{
				nextTurnPowerLevel = GetPowerLevelAdjustIfNoInnerHits();
			}
		}
		
		casterHitResults.AddMiscHitEvent(
			new MiscHitEventData_UpdatePassive(
				m_passive, 
				new List<MiscHitEventPassiveUpdateParams> 
				{
					new Passive_Dino.SetPowerLevelParam(nextTurnPowerLevel)
				}));
	}
#endif
}
