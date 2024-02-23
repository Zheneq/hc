using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClientPositionHitResults
{
	private List<ClientEffectStartData> m_effectsToStart;

	private List<ClientBarrierStartData> m_barriersToStart;

	private List<int> m_effectsToRemove;

	private List<int> m_barriersToRemove;

	private List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;

	private List<ClientMovementResults> m_reactionsOnPosHit;

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
			Debug.LogWarning(new StringBuilder().Append(ClientAbilityResults.s_executePositionHitHeader).Append(" Executing Position Hit").ToString());
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
