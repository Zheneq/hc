using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientKnockbackManager : MonoBehaviour
{
	private static ClientKnockbackManager s_instance;

	private Dictionary<ActorData, int> m_outgoingKnockbacks;

	private Dictionary<ActorData, int> m_incomingKnockbacks;

	private List<ActorData> m_pendingKnockbackActors;

	private void Awake()
	{
		ClientKnockbackManager.s_instance = this;
		this.m_outgoingKnockbacks = new Dictionary<ActorData, int>();
		this.m_incomingKnockbacks = new Dictionary<ActorData, int>();
		this.m_pendingKnockbackActors = new List<ActorData>();
	}

	private void OnDestroy()
	{
		ClientKnockbackManager.s_instance = null;
	}

	public static ClientKnockbackManager Get()
	{
		return ClientKnockbackManager.s_instance;
	}

	public void ResetPendingKnockbackCounts()
	{
		this.m_outgoingKnockbacks.Clear();
		this.m_incomingKnockbacks.Clear();
		this.m_pendingKnockbackActors.Clear();
	}

	public void InitKnockbacksFromActions(List<ClientResolutionAction> actions)
	{
		this.ResetPendingKnockbackCounts();
		foreach (ClientResolutionAction clientResolutionAction in actions)
		{
			clientResolutionAction.AdjustKnockbackCounts_ClientResolutionAction(ref this.m_outgoingKnockbacks, ref this.m_incomingKnockbacks);
		}
	}

	public bool ActorHasIncomingKnockback(ActorData actor)
	{
		return this.m_incomingKnockbacks.ContainsKey(actor);
	}

	public void OnKnockbackHit(ActorData sourceActor, ActorData hitActor)
	{
		bool flag;
		if (this.m_incomingKnockbacks.ContainsKey(sourceActor))
		{
			flag = (sourceActor != hitActor);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		Dictionary<ActorData, int> dictionary;
		(dictionary = this.m_incomingKnockbacks)[hitActor] = dictionary[hitActor] - 1;
		if (sourceActor != null)
		{
			(dictionary = this.m_outgoingKnockbacks)[sourceActor] = dictionary[sourceActor] - 1;
		}
		if (flag2)
		{
			if (this.ActorReadyToBeMoved(sourceActor))
			{
				this.KnockbackActor(sourceActor);
			}
		}
		if (this.ActorReadyToBeMoved(hitActor))
		{
			this.KnockbackActor(hitActor);
		}
	}

	public void NotifyOnActorAnimHitsDone(ActorData caster)
	{
		if (caster != null)
		{
			if (this.m_pendingKnockbackActors.Contains(caster))
			{
				if (this.ActorReadyToBeMoved(caster))
				{
					this.KnockbackActor(caster);
					this.m_pendingKnockbackActors.Remove(caster);
				}
			}
		}
	}

	public bool ActorReadyToBeMoved(ActorData target)
	{
		bool flag;
		if (target == null)
		{
			flag = false;
		}
		else
		{
			int num;
			if (this.m_outgoingKnockbacks.ContainsKey(target))
			{
				num = this.m_outgoingKnockbacks[target];
			}
			else
			{
				num = 0;
			}
			int num2;
			if (this.m_incomingKnockbacks.ContainsKey(target))
			{
				num2 = this.m_incomingKnockbacks[target];
			}
			else
			{
				num2 = 0;
			}
			bool flag2;
			if (num == 0)
			{
				flag2 = (num2 == 0);
			}
			else
			{
				flag2 = false;
			}
			flag = flag2;
			if (flag)
			{
				flag = !TheatricsManager.Get().ClientNeedToWaitBeforeKnockbackMove(target);
				if (!flag)
				{
					if (!this.m_pendingKnockbackActors.Contains(target))
					{
						this.m_pendingKnockbackActors.Add(target);
					}
				}
			}
		}
		return flag;
	}

	public void KnockbackActor(ActorData knockbackedActor)
	{
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().m_ownedActorDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData sendingPlayer = enumerator.Current;
				ClientResolutionManager.Get().SendActorReadyToResolveKnockback(knockbackedActor, sendingPlayer);
			}
		}
	}
}
