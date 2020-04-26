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
		if (targetSelectComp != null && targetSelectComp is TargetSelect_LayerCones)
		{
			TargetSelect_LayerCones targetSelect_LayerCones = targetSelectComp as TargetSelect_LayerCones;
			targetSelect_LayerCones.m_delegateNumActiveLayers = GetNumLayersActive;
		}
		if (!(base.Targeter is AbilityUtil_Targeter_LayerCones))
		{
			return;
		}
		while (true)
		{
			AbilityUtil_Targeter_LayerCones abilityUtil_Targeter_LayerCones = base.Targeter as AbilityUtil_Targeter_LayerCones;
			abilityUtil_Targeter_LayerCones.m_delegateNumActiveLayers = GetNumLayersActive;
			return;
		}
	}

	public int GetPowerLevelAdjustIfNoInnerHits()
	{
		return (!(m_abilityMod != null)) ? m_powerLevelAdjustIfNoInnerHits : m_abilityMod.m_powerLevelAdjustIfNoInnerHitsMod.GetModifiedValue(m_powerLevelAdjustIfNoInnerHits);
	}

	public int GetLayerCount()
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			if (targetSelectComp is TargetSelect_LayerCones)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return (targetSelectComp as TargetSelect_LayerCones).GetLayerCount();
					}
				}
			}
		}
		return 1;
	}

	public int GetNumLayersActive(int maxLayers)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					bool flag = false;
					if (m_dashOrShieldAbility != null)
					{
						if (m_dashOrShieldAbility.FullyChargeUpLayerCone() && !m_dashOrShieldAbility.IsInReadyStance())
						{
							flag = m_abilityData.HasQueuedAction(m_dashOrShieldActionType);
						}
					}
					if (flag)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return maxLayers;
							}
						}
					}
					return Mathf.Min(maxLayers, m_syncComp.m_layerConePowerLevel + 1);
				}
				}
			}
		}
		return 0;
	}

	public bool IsAtMaxPowerLevel()
	{
		int layerCount = GetLayerCount();
		int numLayersActive = GetNumLayersActive(layerCount);
		return numLayersActive >= layerCount;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "PowerLevelAdjustIfNoInnerHits", string.Empty, m_powerLevelAdjustIfNoInnerHits);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_DinoLayerCones);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		base.SetTargetSelectModReference();
	}
}
