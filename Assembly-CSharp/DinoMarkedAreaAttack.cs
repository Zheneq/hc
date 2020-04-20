using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return base.GetUsageForEditor() + ContextVars.GetDebugString("InCenter", "value set to 1 if delayed hit actor is in center of a shape, not set explicitly otherwise", true);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add("InCenter");
		return contextNamesForEditor;
	}

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Delayed Hits --\n" + this.m_delayedOnHitData.GetInEditorDesc();
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (base.ActorData.GetTeam() != targetActor.GetTeam())
		{
			if (actorHitContext.ContainsKey(targetActor))
			{
				actorHitContext[targetActor].symbol_0015.SetInt(DinoMarkedAreaAttack.s_cvarInCenter.GetHash(), 1);
			}
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (actorHitContext.ContainsKey(targetActor))
		{
			if (targetActor.GetTeam() != caster.GetTeam())
			{
				ActorHitContext actorContext = actorHitContext[targetActor];
				GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, this.m_delayedOnHitData.m_enemyHitIntFields, this.m_calculatedValuesForTargeter);
				results.m_damage = this.m_calculatedValuesForTargeter.m_damage;
				if (this.GetExtraDamageForSingleMark() > 0)
				{
					int num = 0;
					using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorHitContext.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
							ActorData key = keyValuePair.Key;
							ActorHitContext value = keyValuePair.Value;
							if (value.symbol_0012)
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
						results.m_damage += this.GetExtraDamageForSingleMark();
					}
				}
			}
		}
	}

	public int GetDelayedHitDamage()
	{
		int num = 0;
		for (int i = 0; i < this.m_delayedOnHitData.m_enemyHitIntFields.Count; i++)
		{
			if (this.m_delayedOnHitData.m_enemyHitIntFields[i].m_hitType == OnHitIntField.HitType.Damage)
			{
				num += this.m_delayedOnHitData.m_enemyHitIntFields[i].m_baseValue;
			}
		}
		return num;
	}

	public int GetDelayTurns()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_delayTurnsMod.GetModifiedValue(this.m_delayTurns);
		}
		else
		{
			result = this.m_delayTurns;
		}
		return result;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
		}
		else
		{
			result = this.m_shape;
		}
		return result;
	}

	public bool DelayedHitIgnoreLos()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_delayedHitIgnoreLosMod.GetModifiedValue(this.m_delayedHitIgnoreLos);
		}
		else
		{
			result = this.m_delayedHitIgnoreLos;
		}
		return result;
	}

	public int GetExtraDamageForSingleMark()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_extraDamageForSingleMarkMod.GetModifiedValue(this.m_extraDamageForSingleMark);
		}
		else
		{
			result = this.m_extraDamageForSingleMark;
		}
		return result;
	}

	public int GetEnergyToAllyOnDamageHit()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_energyToAllyOnDamageHitMod.GetModifiedValue(this.m_energyToAllyOnDamageHit);
		}
		else
		{
			result = this.m_energyToAllyOnDamageHit;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_delayedOnHitData.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "DelayTurns", string.Empty, this.m_delayTurns, false);
		base.AddTokenInt(tokens, "ExtraDamageForSingleMark", string.Empty, this.m_extraDamageForSingleMark, false);
		base.AddTokenInt(tokens, "EnergyToAllyOnDamageHit", string.Empty, this.m_energyToAllyOnDamageHit, false);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_DinoMarkedAreaAttack);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
