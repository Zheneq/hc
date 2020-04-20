using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class FireborgDualCones : GenericAbility_Container
{
	[Separator("Extra Damage for overlap state", true)]
	public int m_extraDamageIfOverlap;

	public int m_extraDamageNonOverlap;

	[Separator("Add Ignited Effect If Overlap Hit", true)]
	public bool m_igniteTargetIfOverlapHit = true;

	public bool m_igniteTargetIfSuperheated = true;

	[Separator("Ground Fire", true)]
	public bool m_groundFireOnAllIfNormal;

	public bool m_groundFireOnOverlapIfNormal;

	[Space(10f)]
	public bool m_groundFireOnAllIfSuperheated = true;

	public bool m_groundFireOnOverlapIfSuperheated = true;

	[Separator("Superheat Sequence", true)]
	public GameObject m_superheatCastSeqPrefab;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityMod_FireborgDualCones m_abilityMod;

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
		base.AddTokenInt(tokens, "ExtraDamageIfOverlap", string.Empty, this.m_extraDamageIfOverlap, false);
		base.AddTokenInt(tokens, "ExtraDamageNonOverlap", string.Empty, this.m_extraDamageNonOverlap, false);
	}

	public int GetExtraDamageIfOverlap()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GetExtraDamageIfOverlap()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageIfOverlapMod.GetModifiedValue(this.m_extraDamageIfOverlap);
		}
		else
		{
			result = this.m_extraDamageIfOverlap;
		}
		return result;
	}

	public int GetExtraDamageNonOverlap()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GetExtraDamageNonOverlap()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageNonOverlapMod.GetModifiedValue(this.m_extraDamageNonOverlap);
		}
		else
		{
			result = this.m_extraDamageNonOverlap;
		}
		return result;
	}

	public bool IgniteTargetIfOverlapHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.IgniteTargetIfOverlapHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_igniteTargetIfOverlapHitMod.GetModifiedValue(this.m_igniteTargetIfOverlapHit);
		}
		else
		{
			result = this.m_igniteTargetIfOverlapHit;
		}
		return result;
	}

	public bool IgniteTargetIfSuperheated()
	{
		return (!(this.m_abilityMod != null)) ? this.m_igniteTargetIfSuperheated : this.m_abilityMod.m_igniteTargetIfSuperheatedMod.GetModifiedValue(this.m_igniteTargetIfSuperheated);
	}

	public bool GroundFireOnAllIfNormal()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GroundFireOnAllIfNormal()).MethodHandle;
			}
			result = this.m_abilityMod.m_groundFireOnAllIfNormalMod.GetModifiedValue(this.m_groundFireOnAllIfNormal);
		}
		else
		{
			result = this.m_groundFireOnAllIfNormal;
		}
		return result;
	}

	public bool GroundFireOnOverlapIfNormal()
	{
		return (!(this.m_abilityMod != null)) ? this.m_groundFireOnOverlapIfNormal : this.m_abilityMod.m_groundFireOnOverlapIfNormalMod.GetModifiedValue(this.m_groundFireOnOverlapIfNormal);
	}

	public bool GroundFireOnAllIfSuperheated()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GroundFireOnAllIfSuperheated()).MethodHandle;
			}
			result = this.m_abilityMod.m_groundFireOnAllIfSuperheatedMod.GetModifiedValue(this.m_groundFireOnAllIfSuperheated);
		}
		else
		{
			result = this.m_groundFireOnAllIfSuperheated;
		}
		return result;
	}

	public bool GroundFireOnOverlapIfSuperheated()
	{
		return (!(this.m_abilityMod != null)) ? this.m_groundFireOnOverlapIfSuperheated : this.m_abilityMod.m_groundFireOnOverlapIfSuperheatedMod.GetModifiedValue(this.m_groundFireOnOverlapIfSuperheated);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		this.m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (this.GetExtraDamageIfOverlap() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (this.GetExtraDamageNonOverlap() <= 0)
			{
				return;
			}
		}
		if (actorHitContext.ContainsKey(targetActor))
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
			int num;
			actorHitContext[targetActor].\u0015.TryGetInt(ContextKeys.\u0019.GetHash(), out num);
			if (num > 1)
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
				if (this.GetExtraDamageIfOverlap() > 0)
				{
					results.m_damage += this.GetExtraDamageIfOverlap();
				}
			}
			else if (this.GetExtraDamageNonOverlap() > 0)
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
				results.m_damage += this.GetExtraDamageNonOverlap();
			}
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (this.ShouldAddGroundFire())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			bool flag;
			if (!this.ShouldAddGroundFireToAllSquares())
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
				flag = (this.GetTargeterHitCountOnTarget(targetActor) > 1);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
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
				if (!this.m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
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
					return this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
				}
			}
		}
		return null;
	}

	private int GetTargeterHitCountOnTarget(ActorData targetActor)
	{
		ActorHitContext actorHitContext;
		if (base.Targeter.GetActorContextVars().TryGetValue(targetActor, out actorHitContext))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.GetTargeterHitCountOnTarget(ActorData)).MethodHandle;
			}
			int result;
			if (actorHitContext.\u0015.TryGetInt(ContextKeys.\u0019.GetHash(), out result))
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
				return result;
			}
		}
		return 0;
	}

	private bool ShouldAddGroundFire()
	{
		if (this.m_syncComp.InSuperheatMode())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.ShouldAddGroundFire()).MethodHandle;
			}
			return this.GroundFireOnAllIfSuperheated() || this.GroundFireOnOverlapIfSuperheated();
		}
		bool result;
		if (!this.GroundFireOnAllIfNormal())
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
			result = this.GroundFireOnOverlapIfNormal();
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool ShouldAddGroundFireToAllSquares()
	{
		if (this.m_syncComp.InSuperheatMode())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FireborgDualCones.ShouldAddGroundFireToAllSquares()).MethodHandle;
			}
			return this.GroundFireOnAllIfSuperheated();
		}
		return this.GroundFireOnAllIfNormal();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_FireborgDualCones);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
