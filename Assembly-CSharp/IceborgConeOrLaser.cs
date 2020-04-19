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
		str += ContextVars.\u0015(IceborgConeOrLaser.s_cvarHasSlow.\u0012(), "Set on enemies hit, 1 if has Slow, 0 otherwise", true);
		return str + ContextVars.\u0015(Iceborg_SyncComponent.s_cvarHasNova.\u0012(), "set to 1 if target has nova core on start of turn, 0 otherwise", true);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.\u0012());
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.\u0012());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_cachedTargetingRadiusPreview = 0f;
		if (this.GetTargetSelectComp() is TargetSelect_ConeOrLaser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.SetupTargetersAndCachedVars()).MethodHandle;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.GetShieldPerEnemyHit()).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.GetShieldDuration()).MethodHandle;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.ApplyDelayedAoeEffect()).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.SkipDelayedAoeEffectIfHasExisting()).MethodHandle;
			}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.GetCdrPerEnemyWithNovaCore()).MethodHandle;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.PreProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars)).MethodHandle;
			}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(ActorData, ActorData, int, Dictionary<ActorData, ActorHitContext>, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (targetActor == caster)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				int num = 0;
				using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
						ActorData key = keyValuePair.Key;
						if (key.\u000E() != caster.\u000E())
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (keyValuePair.Value.\u0012)
							{
								num++;
							}
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (num > 0)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConeOrLaser.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
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
