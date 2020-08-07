using System.Collections.Generic;
using UnityEngine;

public class ClientPositionHitResults
{
	public List<ClientEffectStartData> m_effectsToStart;

	public List<ClientBarrierStartData> m_barriersToStart;

	public List<int> m_effectsToRemove;

	public List<int> m_barriersToRemove;

	public List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;

	public List<ClientMovementResults> m_reactionsOnPosHit;

	public bool ExecutedHit
	{
		get;
		private set;
	}

	public ClientPositionHitResults(ref IBitStream stream)
	{
		m_effectsToStart = AbilityResultsUtils.DeSerializeEffectsToStartFromStream(ref stream);
		m_barriersToStart = AbilityResultsUtils.DeSerializeBarriersToStartFromStream(ref stream);
		m_effectsToRemove = AbilityResultsUtils.DeSerializeEffectsForRemovalFromStream(ref stream);
		m_barriersToRemove = AbilityResultsUtils.DeSerializeBarriersForRemovalFromStream(ref stream);
		m_sequencesToEnd = AbilityResultsUtils.DeSerializeSequenceEndDataListFromStream(ref stream);
		m_reactionsOnPosHit = AbilityResultsUtils.DeSerializeClientMovementResultsListFromStream(ref stream);
		ExecutedHit = false;
	}

	public void ExecutePositionHit()
	{
		if (ExecutedHit)
		{
			return;
		}
		if (ClientAbilityResults.DebugTraceOn)
		{
			Debug.LogWarning(ClientAbilityResults.s_executePositionHitHeader + " Executing Position Hit");
		}
		using (List<ClientEffectStartData>.Enumerator enumerator = m_effectsToStart.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientEffectStartData current = enumerator.Current;
				ClientEffectBarrierManager.Get().ExecuteEffectStart(current);
			}
		}
		foreach (ClientBarrierStartData item in m_barriersToStart)
		{
			ClientEffectBarrierManager.Get().ExecuteBarrierStart(item);
		}
		foreach (int item2 in m_effectsToRemove)
		{
			ClientEffectBarrierManager.Get().EndEffect(item2);
		}
		using (List<int>.Enumerator enumerator4 = m_barriersToRemove.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				int current4 = enumerator4.Current;
				ClientEffectBarrierManager.Get().EndBarrier(current4);
			}
		}
		using (List<ServerClientUtils.SequenceEndData>.Enumerator enumerator5 = m_sequencesToEnd.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				ServerClientUtils.SequenceEndData current5 = enumerator5.Current;
				current5.EndClientSequences();
			}
		}
		using (List<ClientMovementResults>.Enumerator enumerator6 = m_reactionsOnPosHit.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				ClientMovementResults current6 = enumerator6.Current;
				current6.ReactToMovement();
			}
		}
		ExecutedHit = true;
		ClientResolutionManager.Get().UpdateLastEventTime();
	}
}
