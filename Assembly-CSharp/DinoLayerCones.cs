using System;
using System.Collections.Generic;
using UnityEngine;

public class DinoLayerCones : GenericAbility_Container
{
	[Separator("Powering Up Params", true)]
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
		this.m_syncComp = base.GetComponent<Dino_SyncComponent>();
		this.m_abilityData = base.GetComponent<AbilityData>();
		if (this.m_abilityData != null)
		{
			this.m_dashOrShieldAbility = this.m_abilityData.GetAbilityOfType<DinoDashOrShield>();
			if (this.m_dashOrShieldAbility != null)
			{
				this.m_dashOrShieldActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_dashOrShieldAbility);
			}
		}
		base.SetupTargetersAndCachedVars();
		GenericAbility_TargetSelectBase targetSelectComp = this.GetTargetSelectComp();
		if (targetSelectComp != null && targetSelectComp is TargetSelect_LayerCones)
		{
			TargetSelect_LayerCones targetSelect_LayerCones = targetSelectComp as TargetSelect_LayerCones;
			targetSelect_LayerCones.m_delegateNumActiveLayers = new TargetSelect_LayerCones.NumActiveLayerDelegate(this.GetNumLayersActive);
		}
		if (base.Targeter is AbilityUtil_Targeter_LayerCones)
		{
			AbilityUtil_Targeter_LayerCones abilityUtil_Targeter_LayerCones = base.Targeter as AbilityUtil_Targeter_LayerCones;
			abilityUtil_Targeter_LayerCones.m_delegateNumActiveLayers = new AbilityUtil_Targeter_LayerCones.NumActiveLayerDelegate(this.GetNumLayersActive);
		}
	}

	public int GetPowerLevelAdjustIfNoInnerHits()
	{
		return (!(this.m_abilityMod != null)) ? this.m_powerLevelAdjustIfNoInnerHits : this.m_abilityMod.m_powerLevelAdjustIfNoInnerHitsMod.GetModifiedValue(this.m_powerLevelAdjustIfNoInnerHits);
	}

	public int GetLayerCount()
	{
		GenericAbility_TargetSelectBase targetSelectComp = this.GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			if (targetSelectComp is TargetSelect_LayerCones)
			{
				return (targetSelectComp as TargetSelect_LayerCones).GetLayerCount();
			}
		}
		return 1;
	}

	public int GetNumLayersActive(int maxLayers)
	{
		if (!(this.m_syncComp != null))
		{
			return 0;
		}
		bool flag = false;
		if (this.m_dashOrShieldAbility != null)
		{
			if (this.m_dashOrShieldAbility.FullyChargeUpLayerCone() && !this.m_dashOrShieldAbility.IsInReadyStance())
			{
				flag = this.m_abilityData.HasQueuedAction(this.m_dashOrShieldActionType);
			}
		}
		if (flag)
		{
			return maxLayers;
		}
		return Mathf.Min(maxLayers, (int)(this.m_syncComp.m_layerConePowerLevel + 1));
	}

	public bool IsAtMaxPowerLevel()
	{
		int layerCount = this.GetLayerCount();
		int numLayersActive = this.GetNumLayersActive(layerCount);
		return numLayersActive >= layerCount;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "PowerLevelAdjustIfNoInnerHits", string.Empty, this.m_powerLevelAdjustIfNoInnerHits, false);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_DinoLayerCones);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		base.SetTargetSelectModReference();
	}
}
