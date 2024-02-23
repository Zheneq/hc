using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClientBarrierResults
{
	private int m_barrierGUID;

	private ActorData m_barrierCaster;

	private Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;

	private Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

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
		Debug.LogError(new StringBuilder().Append("ClientBarrierResults error-- Sequence hitting actor ").Append(target.DebugNameString()).Append(", but that actor isn't in our hit results.").ToString());
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
		return new StringBuilder().Append(m_barrierCaster.DebugNameString()).Append("'s barrier, guid = ").Append(m_barrierGUID).ToString();
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(m_actorToHitResults, m_posToHitResults);
	}
}
