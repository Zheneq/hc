using AbilityContextNamespace;
using System.Collections.Generic;
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
		m_syncComp = GetComponent<Fireborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ExtraDamageIfOverlap", string.Empty, m_extraDamageIfOverlap);
		AddTokenInt(tokens, "ExtraDamageNonOverlap", string.Empty, m_extraDamageNonOverlap);
	}

	public int GetExtraDamageIfOverlap()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageIfOverlapMod.GetModifiedValue(m_extraDamageIfOverlap);
		}
		else
		{
			result = m_extraDamageIfOverlap;
		}
		return result;
	}

	public int GetExtraDamageNonOverlap()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageNonOverlapMod.GetModifiedValue(m_extraDamageNonOverlap);
		}
		else
		{
			result = m_extraDamageNonOverlap;
		}
		return result;
	}

	public bool IgniteTargetIfOverlapHit()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_igniteTargetIfOverlapHitMod.GetModifiedValue(m_igniteTargetIfOverlapHit);
		}
		else
		{
			result = m_igniteTargetIfOverlapHit;
		}
		return result;
	}

	public bool IgniteTargetIfSuperheated()
	{
		return (!(m_abilityMod != null)) ? m_igniteTargetIfSuperheated : m_abilityMod.m_igniteTargetIfSuperheatedMod.GetModifiedValue(m_igniteTargetIfSuperheated);
	}

	public bool GroundFireOnAllIfNormal()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_groundFireOnAllIfNormalMod.GetModifiedValue(m_groundFireOnAllIfNormal);
		}
		else
		{
			result = m_groundFireOnAllIfNormal;
		}
		return result;
	}

	public bool GroundFireOnOverlapIfNormal()
	{
		return (!(m_abilityMod != null)) ? m_groundFireOnOverlapIfNormal : m_abilityMod.m_groundFireOnOverlapIfNormalMod.GetModifiedValue(m_groundFireOnOverlapIfNormal);
	}

	public bool GroundFireOnAllIfSuperheated()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_groundFireOnAllIfSuperheatedMod.GetModifiedValue(m_groundFireOnAllIfSuperheated);
		}
		else
		{
			result = m_groundFireOnAllIfSuperheated;
		}
		return result;
	}

	public bool GroundFireOnOverlapIfSuperheated()
	{
		return (!(m_abilityMod != null)) ? m_groundFireOnOverlapIfSuperheated : m_abilityMod.m_groundFireOnOverlapIfSuperheatedMod.GetModifiedValue(m_groundFireOnOverlapIfSuperheated);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		m_syncComp.SetSuperheatedContextVar(abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (GetExtraDamageIfOverlap() <= 0)
		{
			if (GetExtraDamageNonOverlap() <= 0)
			{
				return;
			}
		}
		if (!actorHitContext.ContainsKey(targetActor))
		{
			return;
		}
		while (true)
		{
			actorHitContext[targetActor].m_contextVars.TryGetInt(ContextKeys._0019.GetKey(), out int value);
			if (value > 1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (GetExtraDamageIfOverlap() > 0)
						{
							results.m_damage += GetExtraDamageIfOverlap();
						}
						return;
					}
				}
			}
			if (GetExtraDamageNonOverlap() > 0)
			{
				while (true)
				{
					results.m_damage += GetExtraDamageNonOverlap();
					return;
				}
			}
			return;
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (ShouldAddGroundFire())
		{
			int num;
			if (!ShouldAddGroundFireToAllSquares())
			{
				num = ((GetTargeterHitCountOnTarget(targetActor) > 1) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			if (num != 0)
			{
				if (!m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
						}
					}
				}
			}
		}
		return null;
	}

	private int GetTargeterHitCountOnTarget(ActorData targetActor)
	{
		if (base.Targeter.GetActorContextVars().TryGetValue(targetActor, out ActorHitContext value))
		{
			if (value.m_contextVars.TryGetInt(ContextKeys._0019.GetKey(), out int value2))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return value2;
					}
				}
			}
		}
		return 0;
	}

	private bool ShouldAddGroundFire()
	{
		if (m_syncComp.InSuperheatMode())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return GroundFireOnAllIfSuperheated() || GroundFireOnOverlapIfSuperheated();
				}
			}
		}
		int result;
		if (!GroundFireOnAllIfNormal())
		{
			result = (GroundFireOnOverlapIfNormal() ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool ShouldAddGroundFireToAllSquares()
	{
		if (m_syncComp.InSuperheatMode())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return GroundFireOnAllIfSuperheated();
				}
			}
		}
		return GroundFireOnAllIfNormal();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_FireborgDualCones);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
