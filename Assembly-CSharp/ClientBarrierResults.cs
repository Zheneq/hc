using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientBarrierResults
{
	private int m_barrierGUID;

	private ActorData m_barrierCaster;

	private Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;

	private Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

	public ClientBarrierResults(int barrierGUID, ActorData barrierCaster, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		this.m_barrierGUID = barrierGUID;
		this.m_barrierCaster = barrierCaster;
		this.m_actorToHitResults = actorToHitResults;
		this.m_posToHitResults = posToHitResults;
	}

	public void RunClientBarrierHits()
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in this.m_actorToHitResults)
		{
			this.OnBarrierHitActor(keyValuePair.Key);
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = this.m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				this.OnBarrierHitPosition(keyValuePair2.Key);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientBarrierResults.RunClientBarrierHits()).MethodHandle;
			}
		}
	}

	internal void OnBarrierHitActor(ActorData target)
	{
		if (this.m_actorToHitResults.ContainsKey(target))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientBarrierResults.OnBarrierHitActor(ActorData)).MethodHandle;
			}
			this.m_actorToHitResults[target].ExecuteActorHit(target, this.m_barrierCaster);
		}
		else
		{
			Debug.LogError("ClientBarrierResults error-- Sequence hitting actor " + target.\u0018() + ", but that actor isn't in our hit results.");
		}
	}

	internal void OnBarrierHitPosition(Vector3 position)
	{
		if (this.m_posToHitResults.ContainsKey(position))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientBarrierResults.OnBarrierHitPosition(Vector3)).MethodHandle;
			}
			this.m_posToHitResults[position].ExecutePositionHit();
		}
	}

	internal bool DoneHitting()
	{
		return ClientResolutionAction.DoneHitting(this.m_actorToHitResults, this.m_posToHitResults);
	}

	internal bool HasUnexecutedHitOnActor(ActorData actor)
	{
		return ClientResolutionAction.HasUnexecutedHitOnActor(actor, this.m_actorToHitResults);
	}

	internal void ExecuteUnexecutedClientHits()
	{
		ClientResolutionAction.ExecuteUnexecutedHits(this.m_actorToHitResults, this.m_posToHitResults, this.m_barrierCaster);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(this.m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
	}

	public void MarkActorHitsAsMovementHits()
	{
		foreach (ClientActorHitResults clientActorHitResults in this.m_actorToHitResults.Values)
		{
			clientActorHitResults.IsMovementHit = true;
		}
	}

	public string GetDebugDescription()
	{
		return this.m_barrierCaster.\u0018() + "'s barrier, guid = " + this.m_barrierGUID;
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(this.m_actorToHitResults, this.m_posToHitResults);
	}
}
