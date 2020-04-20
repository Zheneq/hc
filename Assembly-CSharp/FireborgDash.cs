using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class FireborgDash : GenericAbility_Container
{
	[Separator("Whether to add ground fire effect", true)]
	public bool m_addGroundFire = true;

	public int m_groundFireDuration = 1;

	public int m_groundFireDurationIfSuperheated = 1;

	public bool m_igniteIfNormal;

	public bool m_igniteIfSuperheated = true;

	[Separator("Shield per Enemy Hit", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrPerTurnIfLowHealth;

	public int m_lowHealthThresh;

	[Separator("Sequence", true)]
	public GameObject m_superheatedCastSeqPrefab;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityMod_FireborgDash m_abilityMod;

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
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "GroundFireDuration", string.Empty, this.m_groundFireDuration, false);
		base.AddTokenInt(tokens, "GroundFireDurationIfSuperheated", string.Empty, this.m_groundFireDurationIfSuperheated, false);
		base.AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, this.m_shieldPerEnemyHit, false);
		base.AddTokenInt(tokens, "ShieldDuration", string.Empty, this.m_shieldDuration, false);
		base.AddTokenInt(tokens, "CdrPerTurnIfLowHealth", string.Empty, this.m_cdrPerTurnIfLowHealth, false);
		base.AddTokenInt(tokens, "LowHealthThresh", string.Empty, this.m_lowHealthThresh, false);
	}

	public bool AddGroundFire()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_addGroundFireMod.GetModifiedValue(this.m_addGroundFire);
		}
		else
		{
			result = this.m_addGroundFire;
		}
		return result;
	}

	public int GetGroundFireDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_groundFireDurationMod.GetModifiedValue(this.m_groundFireDuration);
		}
		else
		{
			result = this.m_groundFireDuration;
		}
		return result;
	}

	public int GetGroundFireDurationIfSuperheated()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_groundFireDurationIfSuperheatedMod.GetModifiedValue(this.m_groundFireDurationIfSuperheated);
		}
		else
		{
			result = this.m_groundFireDurationIfSuperheated;
		}
		return result;
	}

	public bool IgniteIfNormal()
	{
		return (!(this.m_abilityMod != null)) ? this.m_igniteIfNormal : this.m_abilityMod.m_igniteIfNormalMod.GetModifiedValue(this.m_igniteIfNormal);
	}

	public bool IgniteIfSuperheated()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_igniteIfSuperheatedMod.GetModifiedValue(this.m_igniteIfSuperheated);
		}
		else
		{
			result = this.m_igniteIfSuperheated;
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

	public int GetCdrPerTurnIfLowHealth()
	{
		return (!(this.m_abilityMod != null)) ? this.m_cdrPerTurnIfLowHealth : this.m_abilityMod.m_cdrPerTurnIfLowHealthMod.GetModifiedValue(this.m_cdrPerTurnIfLowHealth);
	}

	public int GetLowHealthThresh()
	{
		return (!(this.m_abilityMod != null)) ? this.m_lowHealthThresh : this.m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(this.m_lowHealthThresh);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		this.m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, this.GetShieldPerEnemyHit(), actorHitContext, results);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (this.AddGroundFire())
		{
			if (!this.m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
			{
				return this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
			}
		}
		return null;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_FireborgDash);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
