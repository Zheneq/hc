using AbilityContextNamespace;
using System.Collections.Generic;
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
		m_layerConeAbility = GetAbilityOfType<DinoLayerCones>();
		OnHitAuthoredData cachedKnockbackDestOnHitData;
		if (m_abilityMod != null)
		{
			cachedKnockbackDestOnHitData = m_abilityMod.m_knockbackDestOnHitDataMod._001D(m_knockbackDestOnHitData);
		}
		else
		{
			cachedKnockbackDestOnHitData = m_knockbackDestOnHitData;
		}
		m_cachedKnockbackDestOnHitData = cachedKnockbackDestOnHitData;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public int GetExtraDamageIfFullPowerLayerCone()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageIfFullPowerLayerConeMod.GetModifiedValue(m_extraDamageIfFullPowerLayerCone);
		}
		else
		{
			result = m_extraDamageIfFullPowerLayerCone;
		}
		return result;
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit);
		}
		else
		{
			result = m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration);
		}
		else
		{
			result = m_shieldDuration;
		}
		return result;
	}

	public bool DoHitsAroundKnockbackDest()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_doHitsAroundKnockbackDestMod.GetModifiedValue(m_doHitsAroundKnockbackDest);
		}
		else
		{
			result = m_doHitsAroundKnockbackDest;
		}
		return result;
	}

	public AbilityAreaShape GetHitsAroundKnockbackDestShape()
	{
		return (!(m_abilityMod != null)) ? m_hitsAroundKnockbackDestShape : m_abilityMod.m_hitsAroundKnockbackDestShapeMod.GetModifiedValue(m_hitsAroundKnockbackDestShape);
	}

	public OnHitAuthoredData GetKnockbackDestOnHitData()
	{
		OnHitAuthoredData result;
		if (m_cachedKnockbackDestOnHitData != null)
		{
			result = m_cachedKnockbackDestOnHitData;
		}
		else
		{
			result = m_knockbackDestOnHitData;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ExtraDamageIfFullPowerLayerCone", string.Empty, m_extraDamageIfFullPowerLayerCone);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor == caster && GetShieldPerEnemyHit() > 0)
		{
			int num = 0;
			using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, ActorHitContext> current = enumerator.Current;
					ActorData key = current.Key;
					if (current.Value.m_inRangeForTargeter)
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
					results.m_absorb += num * GetShieldPerEnemyHit();
				}
				else
				{
					results.m_absorb = num * GetShieldPerEnemyHit();
				}
			}
		}
		if (results.m_damage <= 0)
		{
			return;
		}
		while (true)
		{
			if (!(m_layerConeAbility != null))
			{
				return;
			}
			while (true)
			{
				if (GetExtraDamageIfFullPowerLayerCone() > 0 && m_layerConeAbility.IsAtMaxPowerLevel())
				{
					while (true)
					{
						results.m_damage += GetExtraDamageIfFullPowerLayerCone();
						return;
					}
				}
				return;
			}
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_DinoTargetedKnockback);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
