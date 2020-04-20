﻿using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class ScampDualLasers : GenericAbility_Container
{
	[Separator("Target Select Component for when shield is down", true)]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;

	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;

	[Separator("Extra Damage and Aoe Radius for turn after losing suit (for AoE only)", true)]
	public int m_extraDamageTurnAfterLosingSuit;

	public float m_extraAoeRadiusTurnAfterLosingSuit;

	private AbilityMod_ScampDualLasers m_abilityMod;

	private Scamp_SyncComponent m_syncComp;

	private OnHitAuthoredData m_cachedShieldDownOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + this.m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (this.m_shieldDownTargetSelect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.GetRelevantTargetSelectCompForEditor()).MethodHandle;
			}
			relevantTargetSelectCompForEditor.Add(this.m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		if (this.m_abilityMod != null)
		{
			this.m_cachedShieldDownOnHitData = this.m_abilityMod.m_shieldDownOnHitDataMod.\u001D(this.m_shieldDownOnHitData);
		}
		else
		{
			this.m_cachedShieldDownOnHitData = this.m_shieldDownOnHitData;
		}
		base.SetupTargetersAndCachedVars();
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = base.Targeter as AbilityUtil_Targeter_ScampDualLasers;
		if (abilityUtil_Targeter_ScampDualLasers != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.SetupTargetersAndCachedVars()).MethodHandle;
			}
			abilityUtil_Targeter_ScampDualLasers.m_delegateLaserCount = new AbilityUtil_Targeter_ScampDualLasers.LaserCountDelegate(this.GetNumLasers);
			abilityUtil_Targeter_ScampDualLasers.m_delegateExtraAoeRadius = new AbilityUtil_Targeter_ScampDualLasers.ExtraAoeRadiusDelegate(this.GetExtraAoeRadius);
		}
		TargetSelect_DualMeetingLasers targetSelect_DualMeetingLasers = this.m_targetSelectComp as TargetSelect_DualMeetingLasers;
		TargetSelect_DualMeetingLasers targetSelect_DualMeetingLasers2 = this.m_shieldDownTargetSelect as TargetSelect_DualMeetingLasers;
		if (targetSelect_DualMeetingLasers != null)
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
			targetSelect_DualMeetingLasers.m_delegateLaserCount = new TargetSelect_DualMeetingLasers.LaserCountDelegate(this.GetNumLasers);
			targetSelect_DualMeetingLasers.m_delegateExtraAoeRadius = new TargetSelect_DualMeetingLasers.ExtraAoeRadiusDelegate(this.GetExtraAoeRadius);
		}
		if (targetSelect_DualMeetingLasers2 != null)
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
			targetSelect_DualMeetingLasers2.m_delegateLaserCount = new TargetSelect_DualMeetingLasers.LaserCountDelegate(this.GetNumLasers);
			targetSelect_DualMeetingLasers2.m_delegateExtraAoeRadius = new TargetSelect_DualMeetingLasers.ExtraAoeRadiusDelegate(this.GetExtraAoeRadius);
		}
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		base.ClearTargeters();
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.ResetTargetersForShielding(bool)).MethodHandle;
			}
			if (!(this.m_shieldDownTargetSelect == null))
			{
				collection = this.m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_54;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		collection = this.m_targetSelectComp.CreateTargeters(this);
		IL_54:
		base.Targeters.AddRange(collection);
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = base.Targeter as AbilityUtil_Targeter_ScampDualLasers;
		if (abilityUtil_Targeter_ScampDualLasers != null)
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
			abilityUtil_Targeter_ScampDualLasers.m_delegateLaserCount = new AbilityUtil_Targeter_ScampDualLasers.LaserCountDelegate(this.GetNumLasers);
			abilityUtil_Targeter_ScampDualLasers.m_delegateExtraAoeRadius = new AbilityUtil_Targeter_ScampDualLasers.ExtraAoeRadiusDelegate(this.GetExtraAoeRadius);
		}
	}

	public int GetNumLasers(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return (!this.IsInSuit()) ? 1 : 2;
	}

	public float GetExtraAoeRadius(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius)
	{
		if (this.GetExtraAoeRadiusTurnAfterLosingSuit() > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.GetExtraAoeRadius(AbilityTarget, ActorData, float)).MethodHandle;
			}
			if (this.IsTurnAfterLostSuit())
			{
				return this.GetExtraAoeRadiusTurnAfterLosingSuit();
			}
		}
		return 0f;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.GetTargetSelectComp()).MethodHandle;
			}
			return base.GetTargetSelectComp();
		}
		return this.m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.GetOnHitAuthoredData()).MethodHandle;
			}
			return base.GetOnHitAuthoredData();
		}
		OnHitAuthoredData result;
		if (this.m_cachedShieldDownOnHitData != null)
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
			result = this.m_cachedShieldDownOnHitData;
		}
		else
		{
			result = this.m_shieldDownOnHitData;
		}
		return result;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (this.IsTurnAfterLostSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (this.GetExtraDamageTurnAfterLosingSuit() > 0)
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
				ActorHitContext actorHitContext2 = actorHitContext[targetActor];
				int hash = ContextKeys.\u001A.GetHash();
				if (actorHitContext2.\u0015.ContainsInt(hash))
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
					if (actorHitContext2.\u0015.GetInt(hash) > 0)
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
						results.m_damage += this.GetExtraDamageTurnAfterLosingSuit();
					}
				}
			}
		}
	}

	public bool IsInSuit()
	{
		return this.m_syncComp != null && this.m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public bool IsTurnAfterLostSuit()
	{
		bool result;
		if (this.m_syncComp != null && this.m_syncComp.m_lastSuitLostTurn > 0U)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.IsTurnAfterLostSuit()).MethodHandle;
			}
			result = ((long)GameFlowData.Get().CurrentTurn - (long)((ulong)this.m_syncComp.m_lastSuitLostTurn) == 1L);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetExtraDamageTurnAfterLosingSuit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDualLasers.GetExtraDamageTurnAfterLosingSuit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageTurnAfterLosingSuitMod.GetModifiedValue(this.m_extraDamageTurnAfterLosingSuit);
		}
		else
		{
			result = this.m_extraDamageTurnAfterLosingSuit;
		}
		return result;
	}

	public float GetExtraAoeRadiusTurnAfterLosingSuit()
	{
		return (!(this.m_abilityMod != null)) ? this.m_extraAoeRadiusTurnAfterLosingSuit : this.m_abilityMod.m_extraAoeRadiusTurnAfterLosingSuitMod.GetModifiedValue(this.m_extraAoeRadiusTurnAfterLosingSuit);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_shieldDownOnHitData.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "ExtraDamageTurnAfterLosingSuit", string.Empty, this.m_extraDamageTurnAfterLosingSuit, false);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_ScampDualLasers);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (this.m_abilityMod != null)
		{
			this.m_targetSelectComp.SetTargetSelectMod(this.m_abilityMod.m_defaultTargetSelectMod);
			this.m_shieldDownTargetSelect.SetTargetSelectMod(this.m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			this.m_targetSelectComp.ClearTargetSelectMod();
			this.m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
}
