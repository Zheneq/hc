using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class DinoTargetedKnockback : GenericAbility_Container
{
	public int m_extraDamageIfFullPowerLayerCone;

	[Separator("Shield per enemy hit", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 2;

	[Separator("For hits around knockback destinations", true)]
	public bool m_doHitsAroundKnockbackDest;

	public AbilityAreaShape m_hitsAroundKnockbackDestShape = AbilityAreaShape.Three_x_Three;

	[Separator("On Hit Data for Knockback Destination Hit", "yellow")]
	public OnHitAuthoredData m_knockbackDestOnHitData;

	[Separator("Sequences for hit around knockback destination", true)]
	public GameObject m_onKnockbackDestHitSeqPrefab;

	private OnHitAuthoredData m_cachedKnockbackDestOnHitData;

	private AbilityMod_DinoTargetedKnockback m_abilityMod;

	private DinoLayerCones m_layerConeAbility;

	protected override void SetupTargetersAndCachedVars()
	{
		base.SetupTargetersAndCachedVars();
		this.m_layerConeAbility = base.GetAbilityOfType<DinoLayerCones>();
		OnHitAuthoredData cachedKnockbackDestOnHitData;
		if (this.m_abilityMod != null)
		{
			cachedKnockbackDestOnHitData = this.m_abilityMod.m_knockbackDestOnHitDataMod.symbol_001D(this.m_knockbackDestOnHitData);
		}
		else
		{
			cachedKnockbackDestOnHitData = this.m_knockbackDestOnHitData;
		}
		this.m_cachedKnockbackDestOnHitData = cachedKnockbackDestOnHitData;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public int GetExtraDamageIfFullPowerLayerCone()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_extraDamageIfFullPowerLayerConeMod.GetModifiedValue(this.m_extraDamageIfFullPowerLayerCone);
		}
		else
		{
			result = this.m_extraDamageIfFullPowerLayerCone;
		}
		return result;
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(this.m_shieldPerEnemyHit);
		}
		else
		{
			result = this.m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldDurationMod.GetModifiedValue(this.m_shieldDuration);
		}
		else
		{
			result = this.m_shieldDuration;
		}
		return result;
	}

	public bool DoHitsAroundKnockbackDest()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_doHitsAroundKnockbackDestMod.GetModifiedValue(this.m_doHitsAroundKnockbackDest);
		}
		else
		{
			result = this.m_doHitsAroundKnockbackDest;
		}
		return result;
	}

	public AbilityAreaShape GetHitsAroundKnockbackDestShape()
	{
		return (!(this.m_abilityMod != null)) ? this.m_hitsAroundKnockbackDestShape : this.m_abilityMod.m_hitsAroundKnockbackDestShapeMod.GetModifiedValue(this.m_hitsAroundKnockbackDestShape);
	}

	public OnHitAuthoredData GetKnockbackDestOnHitData()
	{
		OnHitAuthoredData result;
		if (this.m_cachedKnockbackDestOnHitData != null)
		{
			result = this.m_cachedKnockbackDestOnHitData;
		}
		else
		{
			result = this.m_knockbackDestOnHitData;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "ExtraDamageIfFullPowerLayerCone", string.Empty, this.m_extraDamageIfFullPowerLayerCone, false);
		base.AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, this.m_shieldPerEnemyHit, false);
		base.AddTokenInt(tokens, "ShieldDuration", string.Empty, this.m_shieldDuration, false);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor == caster && this.GetShieldPerEnemyHit() > 0)
		{
			int num = 0;
			using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
					ActorData key = keyValuePair.Key;
					if (keyValuePair.Value.symbol_0012)
					{
						if (key.GetTeam() != caster.GetTeam())
						{
							num++;
						}
					}
				}
			}
			if (num > 0)
			{
				if (results.m_absorb > 0)
				{
					results.m_absorb += num * this.GetShieldPerEnemyHit();
				}
				else
				{
					results.m_absorb = num * this.GetShieldPerEnemyHit();
				}
			}
		}
		if (results.m_damage > 0)
		{
			if (this.m_layerConeAbility != null)
			{
				if (this.GetExtraDamageIfFullPowerLayerCone() > 0 && this.m_layerConeAbility.IsAtMaxPowerLevel())
				{
					results.m_damage += this.GetExtraDamageIfFullPowerLayerCone();
				}
			}
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_DinoTargetedKnockback);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
