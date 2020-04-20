using System;
using System.Collections.Generic;
using AbilityContextNamespace;

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
		string str = base.GetUsageForEditor();
		str += ContextVars.GetDebugString(IceborgConeOrLaser.s_cvarHasSlow.GetName(), "Set on enemies hit, 1 if has Slow, 0 otherwise", true);
		return str + ContextVars.GetDebugString(Iceborg_SyncComponent.s_cvarHasNova.GetName(), "set to 1 if target has nova core on start of turn, 0 otherwise", true);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.GetName());
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_cachedTargetingRadiusPreview = 0f;
		if (this.GetTargetSelectComp() is TargetSelect_ConeOrLaser)
		{
			TargetSelect_ConeOrLaser targetSelect_ConeOrLaser = this.GetTargetSelectComp() as TargetSelect_ConeOrLaser;
			this.m_cachedTargetingRadiusPreview = targetSelect_ConeOrLaser.m_coneInfo.m_radiusInSquares;
		}
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, this.m_shieldPerEnemyHit, false);
		base.AddTokenInt(tokens, "ShieldDuration", string.Empty, this.m_shieldDuration, false);
		base.AddTokenInt(tokens, "CdrPerEnemyWithNovaCore", string.Empty, this.m_cdrPerEnemyWithNovaCore, false);
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			this.m_syncComp.AddTooltipTokens(tokens);
		}
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

	public bool ApplyDelayedAoeEffect()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(this.m_applyDelayedAoeEffect);
		}
		else
		{
			result = this.m_applyDelayedAoeEffect;
		}
		return result;
	}

	public bool SkipDelayedAoeEffectIfHasExisting()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_skipDelayedAoeEffectIfHasExistingMod.GetModifiedValue(this.m_skipDelayedAoeEffectIfHasExisting);
		}
		else
		{
			result = this.m_skipDelayedAoeEffectIfHasExisting;
		}
		return result;
	}

	public int GetCdrPerEnemyWithNovaCore()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrPerEnemyWithNovaCoreMod.GetModifiedValue(this.m_cdrPerEnemyWithNovaCore);
		}
		else
		{
			result = this.m_cdrPerEnemyWithNovaCore;
		}
		return result;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (this.m_syncComp != null)
		{
			this.m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, base.ActorData);
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, this.GetShieldPerEnemyHit(), actorHitContext, results);
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(ActorData targetActor, ActorData caster, int shieldPerEnemyHit, Dictionary<ActorData, ActorHitContext> actorHitContext, TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit > 0)
		{
			if (targetActor == caster)
			{
				int num = 0;
				using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
						ActorData key = keyValuePair.Key;
						if (key.GetTeam() != caster.GetTeam())
						{
							if (keyValuePair.Value.symbol_0012)
							{
								num++;
							}
						}
					}
				}
				if (num > 0)
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
				}
			}
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		string result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return result;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return this.m_cachedTargetingRadiusPreview > 0f;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.m_cachedTargetingRadiusPreview;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgConeOrLaser);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
