using AbilityContextNamespace;
using System.Collections.Generic;

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
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		if (m_abilityMod != null)
		{
			m_cachedShieldDownOnHitData = m_abilityMod.m_shieldDownOnHitDataMod._001D(m_shieldDownOnHitData);
		}
		else
		{
			m_cachedShieldDownOnHitData = m_shieldDownOnHitData;
		}
		base.SetupTargetersAndCachedVars();
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = base.Targeter as AbilityUtil_Targeter_ScampDualLasers;
		if (abilityUtil_Targeter_ScampDualLasers != null)
		{
			abilityUtil_Targeter_ScampDualLasers.m_delegateLaserCount = GetNumLasers;
			abilityUtil_Targeter_ScampDualLasers.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
		TargetSelect_DualMeetingLasers targetSelect_DualMeetingLasers = m_targetSelectComp as TargetSelect_DualMeetingLasers;
		TargetSelect_DualMeetingLasers targetSelect_DualMeetingLasers2 = m_shieldDownTargetSelect as TargetSelect_DualMeetingLasers;
		if (targetSelect_DualMeetingLasers != null)
		{
			targetSelect_DualMeetingLasers.m_delegateLaserCount = GetNumLasers;
			targetSelect_DualMeetingLasers.m_delegateExtraAoeRadius = GetExtraAoeRadius;
		}
		if (!(targetSelect_DualMeetingLasers2 != null))
		{
			return;
		}
		while (true)
		{
			targetSelect_DualMeetingLasers2.m_delegateLaserCount = GetNumLasers;
			targetSelect_DualMeetingLasers2.m_delegateExtraAoeRadius = GetExtraAoeRadius;
			return;
		}
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
		{
			if (!(m_shieldDownTargetSelect == null))
			{
				collection = m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_0054;
			}
		}
		collection = m_targetSelectComp.CreateTargeters(this);
		goto IL_0054;
		IL_0054:
		base.Targeters.AddRange(collection);
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = base.Targeter as AbilityUtil_Targeter_ScampDualLasers;
		if (abilityUtil_Targeter_ScampDualLasers == null)
		{
			return;
		}
		while (true)
		{
			abilityUtil_Targeter_ScampDualLasers.m_delegateLaserCount = GetNumLasers;
			abilityUtil_Targeter_ScampDualLasers.m_delegateExtraAoeRadius = GetExtraAoeRadius;
			return;
		}
	}

	public int GetNumLasers(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return (!IsInSuit()) ? 1 : 2;
	}

	public float GetExtraAoeRadius(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius)
	{
		if (GetExtraAoeRadiusTurnAfterLosingSuit() > 0f)
		{
			if (IsTurnAfterLostSuit())
			{
				return GetExtraAoeRadiusTurnAfterLosingSuit();
			}
		}
		return 0f;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return base.GetTargetSelectComp();
				}
			}
		}
		return m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return base.GetOnHitAuthoredData();
				}
			}
		}
		OnHitAuthoredData result;
		if (m_cachedShieldDownOnHitData != null)
		{
			result = m_cachedShieldDownOnHitData;
		}
		else
		{
			result = m_shieldDownOnHitData;
		}
		return result;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!IsTurnAfterLostSuit())
		{
			return;
		}
		while (true)
		{
			if (GetExtraDamageTurnAfterLosingSuit() <= 0)
			{
				return;
			}
			while (true)
			{
				ActorHitContext actorHitContext2 = actorHitContext[targetActor];
				int hash = ContextKeys._001A.GetKey();
				if (!actorHitContext2.context.ContainsInt(hash))
				{
					return;
				}
				while (true)
				{
					if (actorHitContext2.context.GetInt(hash) > 0)
					{
						while (true)
						{
							results.m_damage += GetExtraDamageTurnAfterLosingSuit();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public bool IsTurnAfterLostSuit()
	{
		int result;
		if (m_syncComp != null && m_syncComp.m_lastSuitLostTurn != 0)
		{
			result = ((GameFlowData.Get().CurrentTurn - m_syncComp.m_lastSuitLostTurn == 1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetExtraDamageTurnAfterLosingSuit()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageTurnAfterLosingSuitMod.GetModifiedValue(m_extraDamageTurnAfterLosingSuit);
		}
		else
		{
			result = m_extraDamageTurnAfterLosingSuit;
		}
		return result;
	}

	public float GetExtraAoeRadiusTurnAfterLosingSuit()
	{
		return (!(m_abilityMod != null)) ? m_extraAoeRadiusTurnAfterLosingSuit : m_abilityMod.m_extraAoeRadiusTurnAfterLosingSuitMod.GetModifiedValue(m_extraAoeRadiusTurnAfterLosingSuit);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_shieldDownOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ExtraDamageTurnAfterLosingSuit", string.Empty, m_extraDamageTurnAfterLosingSuit);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_ScampDualLasers);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_defaultTargetSelectMod);
			m_shieldDownTargetSelect.SetTargetSelectMod(m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
			m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
}
