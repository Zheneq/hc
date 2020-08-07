using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class DinoMarkedAreaAttack : GenericAbility_Container
{
	private const string c_inCenter = "InCenter";

	public static ContextNameKeyPair s_cvarInCenter = new ContextNameKeyPair("InCenter");

	[Separator("For Delayed Hit", true)]
	public int m_delayTurns = 1;

	public AbilityAreaShape m_shape;

	public bool m_delayedHitIgnoreLos;

	public int m_extraDamageForSingleMark;

	public int m_energyToAllyOnDamageHit;

	[Separator("On Hit Data for delayed hits", "yellow")]
	public OnHitAuthoredData m_delayedOnHitData;

	[Separator("Sequences for delayed hits", true)]
	public GameObject m_firstTurnMarkerSeqPrefab;

	public GameObject m_markerSeqPrefab;

	public GameObject m_triggerSeqPrefab;

	private AbilityMod_DinoMarkedAreaAttack m_abilityMod;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor() + ContextVars.GetDebugString("InCenter", "value set to 1 if delayed hit actor is in center of a shape, not set explicitly otherwise");
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add("InCenter");
		return contextNamesForEditor;
	}

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Delayed Hits --\n" + m_delayedOnHitData.GetInEditorDesc();
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (base.ActorData.GetTeam() == targetActor.GetTeam())
		{
			return;
		}
		while (true)
		{
			if (actorHitContext.ContainsKey(targetActor))
			{
				while (true)
				{
					actorHitContext[targetActor].context.SetInt(s_cvarInCenter.GetKey(), 1);
					return;
				}
			}
			return;
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!actorHitContext.ContainsKey(targetActor))
		{
			return;
		}
		while (true)
		{
			if (targetActor.GetTeam() == caster.GetTeam())
			{
				return;
			}
			while (true)
			{
				ActorHitContext actorContext = actorHitContext[targetActor];
				GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, m_delayedOnHitData.m_enemyHitIntFields, m_calculatedValuesForTargeter);
				results.m_damage = m_calculatedValuesForTargeter.m_damage;
				if (GetExtraDamageForSingleMark() <= 0)
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
							ActorHitContext value = current.Value;
							if (value.inRange)
							{
								if (key.GetTeam() != caster.GetTeam())
								{
									num++;
								}
							}
						}
					}
					if (num == 1)
					{
						while (true)
						{
							results.m_damage += GetExtraDamageForSingleMark();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public int GetDelayedHitDamage()
	{
		int num = 0;
		for (int i = 0; i < m_delayedOnHitData.m_enemyHitIntFields.Count; i++)
		{
			if (m_delayedOnHitData.m_enemyHitIntFields[i].m_hitType == OnHitIntField.HitType.Damage)
			{
				num += m_delayedOnHitData.m_enemyHitIntFields[i].m_baseValue;
			}
		}
		while (true)
		{
			return num;
		}
	}

	public int GetDelayTurns()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_delayTurnsMod.GetModifiedValue(m_delayTurns);
		}
		else
		{
			result = m_delayTurns;
		}
		return result;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
		}
		else
		{
			result = m_shape;
		}
		return result;
	}

	public bool DelayedHitIgnoreLos()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_delayedHitIgnoreLosMod.GetModifiedValue(m_delayedHitIgnoreLos);
		}
		else
		{
			result = m_delayedHitIgnoreLos;
		}
		return result;
	}

	public int GetExtraDamageForSingleMark()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageForSingleMarkMod.GetModifiedValue(m_extraDamageForSingleMark);
		}
		else
		{
			result = m_extraDamageForSingleMark;
		}
		return result;
	}

	public int GetEnergyToAllyOnDamageHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_energyToAllyOnDamageHitMod.GetModifiedValue(m_energyToAllyOnDamageHit);
		}
		else
		{
			result = m_energyToAllyOnDamageHit;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_delayedOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "DelayTurns", string.Empty, m_delayTurns);
		AddTokenInt(tokens, "ExtraDamageForSingleMark", string.Empty, m_extraDamageForSingleMark);
		AddTokenInt(tokens, "EnergyToAllyOnDamageHit", string.Empty, m_energyToAllyOnDamageHit);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_DinoMarkedAreaAttack);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
