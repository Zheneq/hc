using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		this.m_syncComp = base.GetComponent<Fireborg_SyncComponent>();
		this.SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "AuraDuration", string.Empty, this.m_auraDuration, false);
		base.AddTokenInt(tokens, "AuraDurationIfSuperheated", string.Empty, this.m_auraDurationIfSuperheated, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_onCastTargetAllyEffect, "OnCastTargetAllyEffect", this.m_onCastTargetAllyEffect, true);
		base.AddTokenInt(tokens, "CdrOnUltCast", string.Empty, this.m_cdrOnUltCast, false);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOnCastTargetAllyEffect;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.SetCachedFields()).MethodHandle;
			}
			cachedOnCastTargetAllyEffect = this.m_abilityMod.m_onCastTargetAllyEffectMod.GetModifiedValue(this.m_onCastTargetAllyEffect);
		}
		else
		{
			cachedOnCastTargetAllyEffect = this.m_onCastTargetAllyEffect;
		}
		this.m_cachedOnCastTargetAllyEffect = cachedOnCastTargetAllyEffect;
	}

	public bool ExcludeTargetedActor()
	{
		bool result;
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.ExcludeTargetedActor()).MethodHandle;
			}
			result = this.m_abilityMod.m_excludeTargetedActorMod.GetModifiedValue(this.m_excludeTargetedActor);
		}
		else
		{
			result = this.m_excludeTargetedActor;
		}
		return result;
	}

	public int GetAuraDuration()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.GetAuraDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_auraDurationMod.GetModifiedValue(this.m_auraDuration);
		}
		else
		{
			result = this.m_auraDuration;
		}
		return result;
	}

	public int GetAuraDurationIfSuperheated()
	{
		return (!(this.m_abilityMod != null)) ? this.m_auraDurationIfSuperheated : this.m_abilityMod.m_auraDurationIfSuperheatedMod.GetModifiedValue(this.m_auraDurationIfSuperheated);
	}

	public bool IgniteIfNormal()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.IgniteIfNormal()).MethodHandle;
			}
			result = this.m_abilityMod.m_igniteIfNormalMod.GetModifiedValue(this.m_igniteIfNormal);
		}
		else
		{
			result = this.m_igniteIfNormal;
		}
		return result;
	}

	public bool IgniteIfSuperheated()
	{
		bool result;
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.IgniteIfSuperheated()).MethodHandle;
			}
			result = this.m_abilityMod.m_igniteIfSuperheatedMod.GetModifiedValue(this.m_igniteIfSuperheated);
		}
		else
		{
			result = this.m_igniteIfSuperheated;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastTargetAllyEffect()
	{
		return (this.m_cachedOnCastTargetAllyEffect == null) ? this.m_onCastTargetAllyEffect : this.m_cachedOnCastTargetAllyEffect;
	}

	public int GetCdrOnUltCast()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.GetCdrOnUltCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrOnUltCastMod.GetModifiedValue(this.m_cdrOnUltCast);
		}
		else
		{
			result = this.m_cdrOnUltCast;
		}
		return result;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		this.m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			StandardEffectInfo onCastTargetAllyEffect = this.GetOnCastTargetAllyEffect();
			if (onCastTargetAllyEffect.m_applyEffect && onCastTargetAllyEffect.m_effectData.m_absorbAmount > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
				}
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
				if (boardSquareSafe != null)
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
					if (boardSquareSafe == targetActor.GetCurrentBoardSquare())
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
						if (results.m_absorb >= 0)
						{
							results.m_absorb += onCastTargetAllyEffect.m_effectData.m_absorbAmount;
						}
						else
						{
							results.m_absorb = onCastTargetAllyEffect.m_effectData.m_absorbAmount;
						}
					}
				}
			}
		}
		else if (this.m_excludeTargetedActor)
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
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
			if (boardSquareSafe2 != null)
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
				if (boardSquareSafe2 == targetActor.GetCurrentBoardSquare())
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
					results.m_damage = 0;
				}
			}
		}
	}

	public override bool ActorCountTowardsEnergyGain(ActorData target, ActorData caster)
	{
		if (this.m_excludeTargetedActor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDamageAura.ActorCountTowardsEnergyGain(ActorData, ActorData)).MethodHandle;
			}
			if (target.GetTeam() != caster.GetTeam())
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
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
				if (boardSquareSafe != null)
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
					if (boardSquareSafe == target.GetCurrentBoardSquare())
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
						return false;
					}
				}
			}
		}
		return true;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_FireborgDamageAura);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
