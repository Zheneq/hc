using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientBarrierStartData
{
	public int m_barrierGUID;

	public List<ServerClientUtils.SequenceStartData> m_sequenceStartDataList;

	public BarrierSerializeInfo m_barrierGameplayInfo;

	public ClientBarrierStartData(int barrierGUID, List<ServerClientUtils.SequenceStartData> sequenceStartDataList, BarrierSerializeInfo gameplayInfo)
	{
		this.m_barrierGUID = barrierGUID;
		this.m_sequenceStartDataList = sequenceStartDataList;
		this.m_barrierGameplayInfo = gameplayInfo;
	}

	public void ExecuteBarrierStart()
	{
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnClientBarrierStartSequenceHitActor), new SequenceSource.Vector3Delegate(this.OnClientBarrierStartSequenceHitPosition));
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
