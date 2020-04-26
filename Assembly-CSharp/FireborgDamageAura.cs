using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class FireborgDamageAura : GenericAbility_Container
{
	[Separator("Damage Aura", true)]
	public bool m_excludeTargetedActor = true;

	public int m_auraDuration = 1;

	public int m_auraDurationIfSuperheated = 1;

	public bool m_igniteIfNormal = true;

	public bool m_igniteIfSuperheated = true;

	[Separator("Effect on Cast Target", true)]
	public StandardEffectInfo m_onCastTargetAllyEffect;

	[Separator("Cooldown reduction", true)]
	public int m_cdrOnUltCast;

	[Separator("Sequences", true)]
	public GameObject m_auraPersistentSeqPrefab;

	public GameObject m_auraOnTriggerSeqPrefab;

	[Header("-- Superheated versions")]
	public GameObject m_superheatedCastSeqPrefab;

	public GameObject m_superheatedPersistentSeqPrefab;

	public GameObject m_superheatedOnTriggerSeqPrefab;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityMod_FireborgDamageAura m_abilityMod;

	private StandardEffectInfo m_cachedOnCastTargetAllyEffect;

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return usageForEditor + Fireborg_SyncComponent.GetSuperheatedCvarUsage();
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Fireborg_SyncComponent.s_cvarSuperheated.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Fireborg_SyncComponent>();
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "AuraDuration", string.Empty, m_auraDuration);
		AddTokenInt(tokens, "AuraDurationIfSuperheated", string.Empty, m_auraDurationIfSuperheated);
		AbilityMod.AddToken_EffectInfo(tokens, m_onCastTargetAllyEffect, "OnCastTargetAllyEffect", m_onCastTargetAllyEffect);
		AddTokenInt(tokens, "CdrOnUltCast", string.Empty, m_cdrOnUltCast);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOnCastTargetAllyEffect;
		if (m_abilityMod != null)
		{
			cachedOnCastTargetAllyEffect = m_abilityMod.m_onCastTargetAllyEffectMod.GetModifiedValue(m_onCastTargetAllyEffect);
		}
		else
		{
			cachedOnCastTargetAllyEffect = m_onCastTargetAllyEffect;
		}
		m_cachedOnCastTargetAllyEffect = cachedOnCastTargetAllyEffect;
	}

	public bool ExcludeTargetedActor()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_excludeTargetedActorMod.GetModifiedValue(m_excludeTargetedActor);
		}
		else
		{
			result = m_excludeTargetedActor;
		}
		return result;
	}

	public int GetAuraDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_auraDurationMod.GetModifiedValue(m_auraDuration);
		}
		else
		{
			result = m_auraDuration;
		}
		return result;
	}

	public int GetAuraDurationIfSuperheated()
	{
		return (!(m_abilityMod != null)) ? m_auraDurationIfSuperheated : m_abilityMod.m_auraDurationIfSuperheatedMod.GetModifiedValue(m_auraDurationIfSuperheated);
	}

	public bool IgniteIfNormal()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_igniteIfNormalMod.GetModifiedValue(m_igniteIfNormal);
		}
		else
		{
			result = m_igniteIfNormal;
		}
		return result;
	}

	public bool IgniteIfSuperheated()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_igniteIfSuperheatedMod.GetModifiedValue(m_igniteIfSuperheated);
		}
		else
		{
			result = m_igniteIfSuperheated;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastTargetAllyEffect()
	{
		return (m_cachedOnCastTargetAllyEffect == null) ? m_onCastTargetAllyEffect : m_cachedOnCastTargetAllyEffect;
	}

	public int GetCdrOnUltCast()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_cdrOnUltCastMod.GetModifiedValue(m_cdrOnUltCast);
		}
		else
		{
			result = m_cdrOnUltCast;
		}
		return result;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			StandardEffectInfo onCastTargetAllyEffect = GetOnCastTargetAllyEffect();
			if (!onCastTargetAllyEffect.m_applyEffect || onCastTargetAllyEffect.m_effectData.m_absorbAmount <= 0)
			{
				return;
			}
			while (true)
			{
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
				if (!(boardSquareSafe != null))
				{
					return;
				}
				while (true)
				{
					if (!(boardSquareSafe == targetActor.GetCurrentBoardSquare()))
					{
						return;
					}
					while (true)
					{
						if (results.m_absorb >= 0)
						{
							results.m_absorb += onCastTargetAllyEffect.m_effectData.m_absorbAmount;
						}
						else
						{
							results.m_absorb = onCastTargetAllyEffect.m_effectData.m_absorbAmount;
						}
						return;
					}
				}
			}
		}
		if (!m_excludeTargetedActor)
		{
			return;
		}
		while (true)
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
			if (!(boardSquareSafe2 != null))
			{
				return;
			}
			while (true)
			{
				if (boardSquareSafe2 == targetActor.GetCurrentBoardSquare())
				{
					while (true)
					{
						results.m_damage = 0;
						return;
					}
				}
				return;
			}
		}
	}

	public override bool ActorCountTowardsEnergyGain(ActorData target, ActorData caster)
	{
		if (m_excludeTargetedActor)
		{
			if (target.GetTeam() != caster.GetTeam())
			{
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
				if (boardSquareSafe != null)
				{
					if (boardSquareSafe == target.GetCurrentBoardSquare())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_FireborgDamageAura);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
