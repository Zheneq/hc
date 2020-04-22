using System.Collections.Generic;
using UnityEngine;

public class ClientBarrierStartData
{
	public int m_barrierGUID;

	public List<ServerClientUtils.SequenceStartData> m_sequenceStartDataList;

	public BarrierSerializeInfo m_barrierGameplayInfo;

	public ClientBarrierStartData(int barrierGUID, List<ServerClientUtils.SequenceStartData> sequenceStartDataList, BarrierSerializeInfo gameplayInfo)
	{
		m_barrierGUID = barrierGUID;
		m_sequenceStartDataList = sequenceStartDataList;
		m_barrierGameplayInfo = gameplayInfo;
	}

	public void ExecuteBarrierStart()
	{
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData current = enumerator.Current;
				current.CreateSequencesFromData(OnClientBarrierStartSequenceHitActor, OnClientBarrierStartSequenceHitPosition);
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

	internal void OnClientBarrierStartSequenceHitActor(ActorData target)
	{
	}

	internal void OnClientBarrierStartSequenceHitPosition(Vector3 position)
	{
	}
}
