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
		s_instance = this;
		m_outgoingKnockbacks = new Dictionary<ActorData, int>();
		m_incomingKnockbacks = new Dictionary<ActorData, int>();
		m_pendingKnockbackActors = new List<ActorData>();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static ClientKnockbackManager Get()
	{
		return s_instance;
	}

	public void ResetPendingKnockbackCounts()
	{
		m_outgoingKnockbacks.Clear();
		m_incomingKnockbacks.Clear();
		m_pendingKnockbackActors.Clear();
	}

	public void InitKnockbacksFromActions(List<ClientResolutionAction> actions)
	{
		ResetPendingKnockbackCounts();
		foreach (ClientResolutionAction action in actions)
		{
			action.AdjustKnockbackCounts_ClientResolutionAction(ref m_outgoingKnockbacks, ref m_incomingKnockbacks);
		}
	}

	public bool ActorHasIncomingKnockback(ActorData actor)
	{
		return m_incomingKnockbacks.ContainsKey(actor);
	}

	public void OnKnockbackHit(ActorData sourceActor, ActorData hitActor)
	{
		int num;
		if (m_incomingKnockbacks.ContainsKey(sourceActor))
		{
			num = ((sourceActor != hitActor) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		m_incomingKnockbacks[hitActor]--;
		if (sourceActor != null)
		{
			m_outgoingKnockbacks[sourceActor]--;
		}
		if (flag)
		{
			if (ActorReadyToBeMoved(sourceActor))
			{
				KnockbackActor(sourceActor);
			}
		}
		if (!ActorReadyToBeMoved(hitActor))
		{
			return;
		}
		while (true)
		{
			KnockbackActor(hitActor);
			return;
		}
	}

	public void NotifyOnActorAnimHitsDone(ActorData caster)
	{
		if (!(caster != null))
		{
			return;
		}
		while (true)
		{
			if (!m_pendingKnockbackActors.Contains(caster))
			{
				return;
			}
			while (true)
			{
				if (ActorReadyToBeMoved(caster))
				{
					while (true)
					{
						KnockbackActor(caster);
						m_pendingKnockbackActors.Remove(caster);
						return;
					}
				}
				return;
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
			if (m_outgoingKnockbacks.ContainsKey(target))
			{
				num = m_outgoingKnockbacks[target];
			}
			else
			{
				num = 0;
			}
			int num2 = m_incomingKnockbacks.ContainsKey(target) ? m_incomingKnockbacks[target] : 0;
			int num3;
			if (num == 0)
			{
				num3 = ((num2 == 0) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			flag = ((byte)num3 != 0);
			if (flag)
			{
				flag = !TheatricsManager.Get().ClientNeedToWaitBeforeKnockbackMove(target);
				if (!flag)
				{
					if (!m_pendingKnockbackActors.Contains(target))
					{
						m_pendingKnockbackActors.Add(target);
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
				ActorData current = enumerator.Current;
				ClientResolutionManager.Get().SendActorReadyToResolveKnockback(knockbackedActor, current);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}
}
