// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only -- was empty in reactor
public class BarrierResults
{
#if SERVER
	private Barrier m_barrier;
	private bool m_gatheredResults;
	[System.NonSerialized]
	public Dictionary<ActorData, ActorHitResults> m_actorToHitResults;
	[System.NonSerialized]
	public Dictionary<Vector3, PositionHitResults> m_positionToHitResults;
	private Dictionary<ActorData, int> m_damageResults;
	private Dictionary<ActorData, int> m_damageResults_gross;

	public BarrierResults(Barrier barrier)
	{
		m_barrier = barrier;
		m_gatheredResults = false;
		m_actorToHitResults = new Dictionary<ActorData, ActorHitResults>();
		m_positionToHitResults = new Dictionary<Vector3, PositionHitResults>();
		m_damageResults = new Dictionary<ActorData, int>();
		m_damageResults_gross = new Dictionary<ActorData, int>();
	}

	public ActorData Caster
	{
		get
		{
			return m_barrier.Caster;
		}
		private set
		{
		}
	}

	public Barrier Barrier
	{
		get
		{
			return m_barrier;
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
		m_damageResults.Clear();
		m_damageResults_gross.Clear();
	}

	public void StoreActorHit(ActorHitResults hitResults)
	{
		ActorData target = hitResults.m_hitParameters.Target;
		hitResults.m_hitParameters.Caster = ((m_barrier.Caster != null) ? m_barrier.Caster : target);
		hitResults.m_hitParameters.Barrier = m_barrier;
		hitResults.m_hitParameters.DamageSource = ((m_barrier.GetSourceAbility() != null) ? new DamageSource(m_barrier.GetSourceAbility(), hitResults.m_hitParameters.Origin) : new DamageSource(m_barrier.Name, true, hitResults.m_hitParameters.Origin));

		// rogues?
		//if (m_barrier.Caster != null)
		//{
		//	hitResults.InitBaseValuesByCoeff(target, m_barrier.Caster);
		//}

		hitResults.CalcFinalValues(ServerCombatManager.DamageType.Barrier, ServerCombatManager.HealingType.Barrier, ServerCombatManager.TechPointChangeType.Barrier, true);
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
			ActorData caster = m_barrier.Caster;
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
			Debug.LogWarning(string.Concat(new string[]
			{
				"Barrier ",
				m_barrier.Name,
				" is double-storing a hit for actor ",
				target.name,
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
				m_barrier.Caster.name,
				" ",
				m_barrier.Caster.DisplayName,
				"'s ",
				m_barrier.Name,
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
		hitResults.m_hitParameters.Caster = m_barrier.Caster;
		hitResults.m_hitParameters.Barrier = m_barrier;
		hitResults.m_hitParameters.DamageSource = new DamageSource(m_barrier.Name, true, hitResults.m_hitParameters.Pos);
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
			Debug.LogWarning("<color=yellow>(Barrier) OnPositionHit for a position we don't care about: " + position.ToString() + "</color>");
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

	public ActorData[] HitActorsArray()
	{
		ActorData[] array = new ActorData[m_actorToHitResults.Count];
		m_actorToHitResults.Keys.CopyTo(array, 0);
		return array;
	}

	public bool HitsDoneExecuting()
	{
		return !GatheredResults || (ActorHitResults.HitsInCollectionDoneExecuting(m_actorToHitResults) && PositionHitResults.HitsInCollectionDoneExecuting(m_positionToHitResults));
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos(ActorData mover)
	{
		return m_actorToHitResults.ContainsKey(mover) && m_actorToHitResults[mover].ShouldMovementHitUpdateTargetLastKnownPos();
	}
#endif
}
