using System.Collections.Generic;
using UnityEngine;

public class DinoLayerCones : GenericAbility_Container
{
	[Separator("Powering Up Params")]
	public int m_powerupPauseTurnsAfterCast = 1;
	public int m_initialPowerupStartDelay = 1;
	[Header("-- Power level if no inner hits")]
	public int m_powerLevelAdjustIfNoInnerHits;

	private Dino_SyncComponent m_syncComp;
	private AbilityMod_DinoLayerCones m_abilityMod;
	private AbilityData m_abilityData;
	private DinoDashOrShield m_dashOrShieldAbility;
	private AbilityData.ActionType m_dashOrShieldActionType = AbilityData.ActionType.INVALID_ACTION;

	protected override void SetupTargetersAndCachedVars()
	{
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
		TargetSelect_LayerCones targeter1 = targetSelectComp as TargetSelect_LayerCones;
		if (targetSelectComp != null && !ReferenceEquals(targeter1, null))
		{
			targeter1.m_delegateNumActiveLayers = GetNumLayersActive;
		}

		AbilityUtil_Targeter_LayerCones targeter2 = Targeter as AbilityUtil_Targeter_LayerCones;
		if (targeter2 != null)
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
		TargetSelect_LayerCones cones = targetSelectComp as TargetSelect_LayerCones;
		return targetSelectComp != null && !ReferenceEquals(cones, null)
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
}
