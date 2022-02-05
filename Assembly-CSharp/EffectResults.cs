// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// was empty in reactor
public class EffectResults
{
#if SERVER
	private Effect m_effect;
	private ActorData m_casterOverride;
	private bool m_isReal;
	private bool m_gatheredResults;
	public Dictionary<ActorData, ActorHitResults> m_actorToHitResults;
	public Dictionary<Vector3, PositionHitResults> m_positionToHitResults;
	public List<NonActorTargetInfo> m_nonActorTargetInfo;
	private Dictionary<ActorData, int> m_damageResults;
	private Dictionary<ActorData, int> m_damageResults_gross;
	public List<ServerClientUtils.SequenceStartData> m_sequenceStartData = new List<ServerClientUtils.SequenceStartData>();

	public EffectResults(Effect effect, ActorData casterOverride, bool isReal, bool isStandalone = false)
	{
		m_effect = effect;
		m_casterOverride = casterOverride;
		m_isReal = isReal;
		IsStandalone = isStandalone;
		m_gatheredResults = false;
		m_actorToHitResults = new Dictionary<ActorData, ActorHitResults>();
		m_positionToHitResults = new Dictionary<Vector3, PositionHitResults>();
		m_nonActorTargetInfo = new List<NonActorTargetInfo>();
		m_damageResults = new Dictionary<ActorData, int>();
		m_damageResults_gross = new Dictionary<ActorData, int>();
	}

	// probably rogues
	public bool IsStandalone { get; set; }

	public ActorData Caster
	{
		get
		{
			if (m_casterOverride != null)
			{
				return m_casterOverride;
			}
			return m_effect.Caster;
		}
		private set
		{
		}
	}

	public Effect Effect
	{
		get
		{
			return m_effect;
		}
		private set
		{
		}
	}

	public bool GatheredResults
	{
		get
		{
			return m_gatheredResults;
		}
		set
		{
			m_gatheredResults = value;
		}
	}

	public Dictionary<ActorData, int> DamageResults
	{
		get
		{
			return m_damageResults;
		}
		private set
		{
			m_damageResults = value;
		}
	}

	public Dictionary<ActorData, int> DamageResults_Gross
	{
		get
		{
			return m_damageResults_gross;
		}
		private set
		{
			m_damageResults_gross = value;
		}
	}

	public void ClearResults()
	{
		m_gatheredResults = false;
		m_actorToHitResults.Clear();
		m_positionToHitResults.Clear();
		m_nonActorTargetInfo.Clear();
		m_damageResults.Clear();
		m_damageResults_gross.Clear();
	}

	public void StoreActorHit(ActorHitResults hitResults)
	{
		hitResults.m_hitParameters.Caster = m_effect.Caster;
		if (m_casterOverride != null)
		{
			hitResults.m_hitParameters.Caster = m_casterOverride;
		}
		hitResults.m_hitParameters.Effect = m_effect;
		if (m_effect.Parent.IsAbility())
		{
			hitResults.m_hitParameters.DamageSource = new DamageSource(m_effect.Parent.Ability, hitResults.m_hitParameters.Origin);
		}
		else if (m_effect.Parent.IsPassive())
		{
			hitResults.m_hitParameters.DamageSource = new DamageSource(m_effect.Parent.Passive, hitResults.m_hitParameters.Origin);
		}
		else
		{
			hitResults.m_hitParameters.DamageSource = new DamageSource(m_effect.Parent.GetName(), false, hitResults.m_hitParameters.Origin);
		}
		hitResults.CanBeReactedTo = m_effect.HitsCanBeReactedTo();

		// rogues?
		//hitResults.InitBaseValuesByCoeff(hitResults.m_hitParameters.Target, hitResults.m_hitParameters.Caster);

		hitResults.CalcFinalValues(ServerCombatManager.DamageType.Effect, ServerCombatManager.HealingType.Effect, ServerCombatManager.TechPointChangeType.Effect, m_isReal);
		ActorData target = hitResults.m_hitParameters.Target;
		if (!m_actorToHitResults.ContainsKey(target))
		{
			m_actorToHitResults.Add(target, hitResults);
			if (m_damageResults.ContainsKey(target))
			{
				Dictionary<ActorData, int> dictionary = m_damageResults;
				ActorData key = target;
				dictionary[key] += hitResults.HealthDelta;
			}
			else
			{
				m_damageResults.Add(target, hitResults.HealthDelta);
			}
			ActorData caster = hitResults.m_hitParameters.Caster;
			if (hitResults.LifestealHealingOnCaster > 0 && caster != null)
			{
				if (m_damageResults.ContainsKey(caster))
				{
					Dictionary<ActorData, int> dictionary = m_damageResults;
					ActorData key = caster;
					dictionary[key] += hitResults.LifestealHealingOnCaster;
				}
				else
				{
					m_damageResults[caster] = hitResults.LifestealHealingOnCaster;
				}
			}
			if (m_damageResults_gross.ContainsKey(target))
			{
				Dictionary<ActorData, int> dictionary = m_damageResults_gross;
				ActorData key = target;
				dictionary[key] += hitResults.FinalDamage;
			}
			else
			{
				m_damageResults_gross.Add(target, hitResults.FinalDamage);
			}
			if (AbilityResults.DebugTraceOn)
			{
				Debug.LogWarning(AbilityResults.s_storeActorHitHeader + hitResults.ToDebugString());
				return;
			}
		}
		else
		{
			string text = (m_effect.Target == null) ? "(null)" : m_effect.Target.DisplayName;
			Debug.LogWarning(string.Concat(new string[]
			{
				"Effect ",
				m_effect.m_effectName,
				" is double-storing a hit for actor ",
				target.name,
				"; the effect's target is ",
				text,
				"."
			}));
		}
	}

	public bool ExecuteForActor(ActorData target)
	{
		bool result = false;
		if (m_actorToHitResults.ContainsKey(target))
		{
			if (!m_actorToHitResults[target].ExecutedResults)
			{
				m_actorToHitResults[target].ExecuteResults();
				result = true;
				if (AbilityResults.DebugTraceOn)
				{
					Debug.LogWarning(AbilityResults.s_executeActorHitHeader + m_actorToHitResults[target].ToDebugString());
				}
			}
		}
		else
		{
			Debug.LogError(string.Concat(new string[]
			{
				m_effect.Caster.name,
				" ",
				m_effect.Caster.DisplayName,
				"'s ",
				m_effect.m_effectName,
				" trying to execute on ",
				target.name,
				" ",
				target.DisplayName,
				", but that actor isn't in hit results."
			}));
		}
		return result;
	}

	public void StorePositionHit(PositionHitResults hitResults)
	{
		hitResults.m_hitParameters.Caster = m_effect.Caster;
		hitResults.m_hitParameters.Effect = m_effect;
		if (m_effect.Parent.IsAbility())
		{
			hitResults.m_hitParameters.DamageSource = new DamageSource(m_effect.Parent.Ability, hitResults.m_hitParameters.Pos);
		}
		else
		{
			hitResults.m_hitParameters.DamageSource = new DamageSource(m_effect.Parent.Passive, hitResults.m_hitParameters.Pos);
		}
		Vector3 pos = hitResults.m_hitParameters.Pos;
		m_positionToHitResults.Add(pos, hitResults);
		if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning(AbilityResults.s_storePositionHitHeader + hitResults.ToDebugString());
		}
	}

	public bool ExecuteForPosition(Vector3 position)
	{
		bool result = false;
		if (m_positionToHitResults.ContainsKey(position))
		{
			m_positionToHitResults[position].ExecuteResults();
			result = true;
			if (AbilityResults.DebugTraceOn)
			{
				Debug.LogWarning(AbilityResults.s_executePositionHitHeader + m_positionToHitResults[position].ToDebugString());
			}
		}
		else if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning("<color=yellow>(Effect) OnPositionHit for a position we don't care about: " + position.ToString() + "</color>");
		}
		return result;
	}

	public bool StoredHitForActor(ActorData actor)
	{
		return m_actorToHitResults.ContainsKey(actor);
	}

	public bool StoredHitForPosition(Vector3 position)
	{
		return m_positionToHitResults.ContainsKey(position);
	}

	public void StoreNonActorTargetInfo(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (nonActorTargetInfo != null)
		{
			foreach (NonActorTargetInfo item in nonActorTargetInfo)
			{
				m_nonActorTargetInfo.Add(item);
			}
		}
	}

	public ActorData[] HitActorsArray()
	{
		ActorData[] array = new ActorData[m_actorToHitResults.Count];
		m_actorToHitResults.Keys.CopyTo(array, 0);
		return array;
	}

	public List<ActorData> BuildEffectKnockbackTargets()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (KeyValuePair<ActorData, ActorHitResults> keyValuePair in m_actorToHitResults)
		{
			if (keyValuePair.Value.HasKnockback)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	public bool HitsDoneExecuting()
	{
		return !GatheredResults || (ActorHitResults.HitsInCollectionDoneExecuting(m_actorToHitResults) && PositionHitResults.HitsInCollectionDoneExecuting(m_positionToHitResults));
	}

	public void ExecuteUnexecutedEffectHits(bool asFailsafe)
	{
		if (!GatheredResults)
		{
			return;
		}
		string text = m_effect.m_effectName;
		if (text.Length == 0)
		{
			text = "NO_NAME (" + m_effect.GetType().ToString() + ")";
		}
		string logHeader;
		if (m_effect.Caster != null)
		{
			logHeader = string.Concat(new string[]
			{
				m_effect.Caster.name,
				" ",
				m_effect.Caster.DisplayName,
				"'s ",
				text
			});
		}
		else
		{
			logHeader = text;
		}
		ActorHitResults.ExecuteUnexecutedActorHits(m_actorToHitResults, asFailsafe, logHeader);
		PositionHitResults.ExecuteUnexecutedPositionHits(m_positionToHitResults, asFailsafe, logHeader);
	}

	public Dictionary<ActorData, Vector2> GetKnockbackTargets()
	{
		Dictionary<ActorData, Vector2> dictionary = new Dictionary<ActorData, Vector2>();
		foreach (ActorHitResults actorHitResults in m_actorToHitResults.Values)
		{
			if (actorHitResults.HasKnockback)
			{
				Vector2 knockbackDeltaForType = KnockbackUtils.GetKnockbackDeltaForType(actorHitResults.KnockbackHitData);
				dictionary.Add(actorHitResults.m_hitParameters.Target, knockbackDeltaForType);
			}
		}
		return dictionary;
	}

	public void FinalizeEffectResults()
	{
		if (m_nonActorTargetInfo.Count > 0)
		{
			Vector3 vector = Vector3.one;
			List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = m_effect.GetEffectHitSeqDataList();
			if (effectHitSeqDataList != null && effectHitSeqDataList.Count > 0 && effectHitSeqDataList[0] != null)
			{
				vector = effectHitSeqDataList[0].GetTargetPos();
			}
			PositionHitResults positionHitResults = null;
			if (m_positionToHitResults.ContainsKey(vector))
			{
				positionHitResults = m_positionToHitResults[vector];
			}
			else
			{
				positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
				StorePositionHit(positionHitResults);
			}
			List<Barrier> list = new List<Barrier>();
			foreach (NonActorTargetInfo nonActorTargetInfo in m_nonActorTargetInfo)
			{
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock)
				{
					NonActorTargetInfo_BarrierBlock nonActorTargetInfo_BarrierBlock = nonActorTargetInfo as NonActorTargetInfo_BarrierBlock;
					if (nonActorTargetInfo_BarrierBlock.m_barrier != null && (nonActorTargetInfo_BarrierBlock.m_barrier == null || !list.Contains(nonActorTargetInfo_BarrierBlock.m_barrier)))
					{
						MovementResults reactHitResults = nonActorTargetInfo_BarrierBlock.GetReactHitResults(Caster);
						if (reactHitResults != null)
						{
							positionHitResults.AddReactionOnPositionHit(reactHitResults);
							Dictionary<ActorData, int> movementDamageResults = reactHitResults.GetMovementDamageResults();
							if (movementDamageResults != null)
							{
								foreach (ActorData actorData in movementDamageResults.Keys)
								{
									int num = movementDamageResults[actorData];
									if (m_damageResults.ContainsKey(actorData))
									{
										Dictionary<ActorData, int> dictionary = m_damageResults;
										ActorData key = actorData;
										dictionary[key] += num;
									}
									else
									{
										m_damageResults.Add(actorData, num);
									}
								}
							}
							Dictionary<ActorData, int> movementDamageResults_Gross = reactHitResults.GetMovementDamageResults_Gross();
							if (movementDamageResults_Gross != null)
							{
								foreach (ActorData actorData2 in movementDamageResults_Gross.Keys)
								{
									int num2 = movementDamageResults_Gross[actorData2];
									if (m_damageResults_gross.ContainsKey(actorData2))
									{
										Dictionary<ActorData, int> dictionary = m_damageResults_gross;
										ActorData key = actorData2;
										dictionary[key] += num2;
									}
									else
									{
										m_damageResults_gross.Add(actorData2, num2);
									}
								}
							}
							if (AbilityResults.DebugTraceOn)
							{
								Debug.LogWarning("<color=white>Storing Barrier Block Info at pos " + nonActorTargetInfo_BarrierBlock.m_crossPos + "</color>");
							}
						}
					}
				}
			}
		}
	}

	public void AddtHitActorIds(HashSet<int> hitActorIds)
	{
		foreach (ActorData actorData in m_actorToHitResults.Keys)
		{
			hitActorIds.Add(actorData.ActorIndex);
			ActorHitResults actorHitResults = m_actorToHitResults[actorData];
			if (actorHitResults.HasReactions && actorHitResults.CanBeReactedTo)
			{
				actorHitResults.AddHitActorIdsInReactions(hitActorIds);
			}
		}
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos(ActorData mover)
	{
		return m_actorToHitResults.ContainsKey(mover) && m_actorToHitResults[mover].ShouldMovementHitUpdateTargetLastKnownPos();
	}

	public void OnExecutionComplete()
	{
		Effect.OnExecutedEffectResults(this);
		if (Effect.CanAbsorb())
		{
			ServerEffectManager.Get().UpdateAbsorbPoints(Effect);
		}
	}
#endif
}
