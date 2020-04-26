using AbilityContextNamespace;
using System.Collections.Generic;

public class IceborgConeOrLaser : GenericAbility_Container
{
	[Separator("Shielding per enemy hit on cast", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect;

	public bool m_skipDelayedAoeEffectIfHasExisting;

	[Separator("Cdr Per Hit Enemy with Nova Core", true)]
	public int m_cdrPerEnemyWithNovaCore;

	public static ContextNameKeyPair s_cvarHasSlow = new ContextNameKeyPair("HasSlow");

	private Iceborg_SyncComponent m_syncComp;

	private AbilityMod_IceborgConeOrLaser m_abilityMod;

	private float m_cachedTargetingRadiusPreview;

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		usageForEditor += ContextVars.GetDebugString(s_cvarHasSlow.GetName(), "Set on enemies hit, 1 if has Slow, 0 otherwise");
		return usageForEditor + ContextVars.GetDebugString(Iceborg_SyncComponent.s_cvarHasNova.GetName(), "set to 1 if target has nova core on start of turn, 0 otherwise");
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
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			m_syncComp.AddTooltipTokens(tokens);
			return;
		}
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

	public bool ApplyDelayedAoeEffect()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(m_applyDelayedAoeEffect);
		}
		else
		{
			result = m_applyDelayedAoeEffect;
		}
		return result;
	}

	public bool SkipDelayedAoeEffectIfHasExisting()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_skipDelayedAoeEffectIfHasExistingMod.GetModifiedValue(m_skipDelayedAoeEffectIfHasExisting);
		}
		else
		{
			result = m_skipDelayedAoeEffectIfHasExisting;
		}
		return result;
	}

	public int GetCdrPerEnemyWithNovaCore()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_cdrPerEnemyWithNovaCoreMod.GetModifiedValue(m_cdrPerEnemyWithNovaCore);
		}
		else
		{
			result = m_cdrPerEnemyWithNovaCore;
		}
		return result;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, base.ActorData);
			return;
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(ActorData targetActor, ActorData caster, int shieldPerEnemyHit, Dictionary<ActorData, ActorHitContext> actorHitContext, TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit <= 0)
		{
			return;
		}
		while (true)
		{
			if (!(targetActor == caster))
			{
				return;
			}
			while (true)
			{
				int num = 0;
				using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, ActorHitContext> current = enumerator.Current;
						ActorData key = current.Key;
						if (key.GetTeam() != caster.GetTeam())
						{
							if (current.Value._0012)
							{
								num++;
							}
						}
					}
				}
				if (num <= 0)
				{
					return;
				}
				while (true)
				{
					int num2 = shieldPerEnemyHit * num;
					if (results.m_absorb >= 0)
					{
						results.m_absorb += num2;
					}
					else
					{
						results.m_absorb = num2;
					}
					return;
				}
			}
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		object result;
		if (m_syncComp != null)
		{
			result = m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return (string)result;
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
}
