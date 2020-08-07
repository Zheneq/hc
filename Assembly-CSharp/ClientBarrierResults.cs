using System.Collections.Generic;
using UnityEngine;

public class ClientBarrierResults
{
	public int m_barrierGUID;

	public ActorData m_barrierCaster;

	public Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;

	public Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

	public ClientBarrierResults(int barrierGUID, ActorData barrierCaster, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		m_barrierGUID = barrierGUID;
		m_barrierCaster = barrierCaster;
		m_actorToHitResults = actorToHitResults;
		m_posToHitResults = posToHitResults;
	}

	public void RunClientBarrierHits()
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> actorToHitResult in m_actorToHitResults)
		{
			OnBarrierHitActor(actorToHitResult.Key);
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				OnBarrierHitPosition(enumerator2.Current.Key);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	internal void OnBarrierHitActor(ActorData target)
	{
		if (m_actorToHitResults.ContainsKey(target))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_actorToHitResults[target].ExecuteActorHit(target, m_barrierCaster);
					return;
				}
			}
		}
		Debug.LogError("ClientBarrierResults error-- Sequence hitting actor " + target.DebugNameString() + ", but that actor isn't in our hit results.");
	}

	internal void OnBarrierHitPosition(Vector3 position)
	{
		if (!m_posToHitResults.ContainsKey(position))
		{
			return;
		}
		while (true)
		{
			m_posToHitResults[position].ExecutePositionHit();
			return;
		}
	}

	internal bool DoneHitting()
	{
		return ClientResolutionAction.DoneHitting(m_actorToHitResults, m_posToHitResults);
	}

	internal bool HasUnexecutedHitOnActor(ActorData actor)
	{
		return ClientResolutionAction.HasUnexecutedHitOnActor(actor, m_actorToHitResults);
	}

	internal void ExecuteUnexecutedClientHits()
	{
		ClientResolutionAction.ExecuteUnexecutedHits(m_actorToHitResults, m_posToHitResults, m_barrierCaster);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
	}

	public void MarkActorHitsAsMovementHits()
	{
		foreach (ClientActorHitResults value in m_actorToHitResults.Values)
		{
			value.IsMovementHit = true;
		}
	}

	public string GetDebugDescription()
	{
		return m_barrierCaster.DebugNameString() + "'s barrier, guid = " + m_barrierGUID;
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(m_actorToHitResults, m_posToHitResults);
	}
}
